using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering.Universal.LibTessDotNet;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200003D RID: 61
	internal class ShadowUtility
	{
		// Token: 0x06000269 RID: 617 RVA: 0x00012B84 File Offset: 0x00010D84
		private static ShadowUtility.Edge CreateEdge(int triangleIndexA, int triangleIndexB, List<Vector3> vertices, List<int> triangles)
		{
			ShadowUtility.Edge edge = default(ShadowUtility.Edge);
			edge.AssignVertexIndices(triangles[triangleIndexA], triangles[triangleIndexB]);
			Vector3 vector = vertices[edge.vertexIndex0];
			vector.z = 0f;
			Vector3 vector2 = vertices[edge.vertexIndex1];
			vector2.z = 0f;
			Vector3 vector3 = Vector3.Normalize(vector2 - vector);
			edge.tangent = Vector3.Cross(-Vector3.forward, vector3);
			return edge;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00012C0C File Offset: 0x00010E0C
		private static void PopulateEdgeArray(List<Vector3> vertices, List<int> triangles, List<ShadowUtility.Edge> edges)
		{
			for (int i = 0; i < triangles.Count; i += 3)
			{
				edges.Add(ShadowUtility.CreateEdge(i, i + 1, vertices, triangles));
				edges.Add(ShadowUtility.CreateEdge(i + 1, i + 2, vertices, triangles));
				edges.Add(ShadowUtility.CreateEdge(i + 2, i, vertices, triangles));
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00012C60 File Offset: 0x00010E60
		private static bool IsOutsideEdge(int edgeIndex, List<ShadowUtility.Edge> edgesToProcess)
		{
			int num = edgeIndex - 1;
			int num2 = edgeIndex + 1;
			int count = edgesToProcess.Count;
			ShadowUtility.Edge edge = edgesToProcess[edgeIndex];
			return (num < 0 || edge.CompareTo(edgesToProcess[edgeIndex - 1]) != 0) && (num2 >= count || edge.CompareTo(edgesToProcess[edgeIndex + 1]) != 0);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00012CB3 File Offset: 0x00010EB3
		private static void SortEdges(List<ShadowUtility.Edge> edgesToProcess)
		{
			edgesToProcess.Sort();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00012CBC File Offset: 0x00010EBC
		private static void CreateShadowTriangles(List<Vector3> vertices, List<Color> colors, List<int> triangles, List<Vector4> tangents, List<ShadowUtility.Edge> edges)
		{
			for (int i = 0; i < edges.Count; i++)
			{
				if (ShadowUtility.IsOutsideEdge(i, edges))
				{
					ShadowUtility.Edge edge = edges[i];
					tangents[edge.vertexIndex1] = -edge.tangent;
					int count = vertices.Count;
					vertices.Add(vertices[edge.vertexIndex0]);
					colors.Add(colors[edge.vertexIndex0]);
					tangents.Add(-edge.tangent);
					triangles.Add(edge.vertexIndex0);
					triangles.Add(count);
					triangles.Add(edge.vertexIndex1);
				}
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00012D66 File Offset: 0x00010F66
		private static object InterpCustomVertexData(Vec3 position, object[] data, float[] weights)
		{
			return data[0];
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00012D6C File Offset: 0x00010F6C
		private static void InitializeTangents(int tangentsToAdd, List<Vector4> tangents)
		{
			for (int i = 0; i < tangentsToAdd; i++)
			{
				tangents.Add(Vector4.zero);
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00012D90 File Offset: 0x00010F90
		internal static Bounds CalculateLocalBounds(Vector3[] inVertices)
		{
			if (inVertices.Length == 0)
			{
				return new Bounds(Vector3.zero, Vector3.zero);
			}
			Vector2 vector = Vector2.positiveInfinity;
			Vector2 vector2 = Vector2.negativeInfinity;
			int num = inVertices.Length;
			for (int i = 0; i < num; i++)
			{
				Vector2 vector3 = new Vector2(inVertices[i].x, inVertices[i].y);
				vector = Vector2.Min(vector, vector3);
				vector2 = Vector2.Max(vector2, vector3);
			}
			return new Bounds
			{
				max = vector2,
				min = vector
			};
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00012E24 File Offset: 0x00011024
		public static Bounds GenerateShadowMesh(Mesh mesh, Vector3[] shapePath)
		{
			List<Vector3> list = new List<Vector3>();
			List<int> list2 = new List<int>();
			List<Vector4> list3 = new List<Vector4>();
			List<Color> list4 = new List<Color>();
			int num = shapePath.Length;
			ContourVertex[] array = new ContourVertex[2 * num];
			for (int j = 0; j < num; j++)
			{
				Color color = new Color(shapePath[j].x, shapePath[j].y, shapePath[j].x, shapePath[j].y);
				int num2 = (j + 1) % num;
				array[2 * j] = new ContourVertex
				{
					Position = new Vec3
					{
						X = shapePath[j].x,
						Y = shapePath[j].y,
						Z = 0f
					},
					Data = color
				};
				color = new Color(shapePath[j].x, shapePath[j].y, shapePath[num2].x, shapePath[num2].y);
				Vector2 vector = 0.5f * (shapePath[j] + shapePath[num2]);
				array[2 * j + 1] = new ContourVertex
				{
					Position = new Vec3
					{
						X = vector.x,
						Y = vector.y,
						Z = 0f
					},
					Data = color
				};
			}
			Tess tess = new Tess();
			tess.AddContour(array, ContourOrientation.Original);
			tess.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3, new CombineCallback(ShadowUtility.InterpCustomVertexData));
			int[] array2 = tess.Elements.Select<int, int>((int i) => i).ToArray<int>();
			Vector3[] array3 = tess.Vertices.Select<ContourVertex, Vector3>((ContourVertex v) => new Vector3(v.Position.X, v.Position.Y, 0f)).ToArray<Vector3>();
			Color[] array4 = tess.Vertices.Select<ContourVertex, Color>((ContourVertex v) => new Color(((Color)v.Data).r, ((Color)v.Data).g, ((Color)v.Data).b, ((Color)v.Data).a)).ToArray<Color>();
			list.AddRange(array3);
			list2.AddRange(array2);
			list4.AddRange(array4);
			ShadowUtility.InitializeTangents(list.Count, list3);
			List<ShadowUtility.Edge> list5 = new List<ShadowUtility.Edge>();
			ShadowUtility.PopulateEdgeArray(list, list2, list5);
			ShadowUtility.SortEdges(list5);
			ShadowUtility.CreateShadowTriangles(list, list4, list2, list3, list5);
			Color[] array5 = list4.ToArray();
			Vector3[] array6 = list.ToArray();
			int[] array7 = list2.ToArray();
			Vector4[] array8 = list3.ToArray();
			mesh.Clear();
			mesh.vertices = array6;
			mesh.triangles = array7;
			mesh.tangents = array8;
			mesh.colors = array5;
			return ShadowUtility.CalculateLocalBounds(array6);
		}

		// Token: 0x02000163 RID: 355
		internal struct Edge : IComparable<ShadowUtility.Edge>
		{
			// Token: 0x06000A5D RID: 2653 RVA: 0x000471C8 File Offset: 0x000453C8
			public void AssignVertexIndices(int vi0, int vi1)
			{
				this.vertexIndex0 = vi0;
				this.vertexIndex1 = vi1;
				this.compareReversed = vi0 > vi1;
			}

			// Token: 0x06000A5E RID: 2654 RVA: 0x000471E4 File Offset: 0x000453E4
			public int Compare(ShadowUtility.Edge a, ShadowUtility.Edge b)
			{
				int num = (a.compareReversed ? a.vertexIndex1 : a.vertexIndex0);
				int num2 = (a.compareReversed ? a.vertexIndex0 : a.vertexIndex1);
				int num3 = (b.compareReversed ? b.vertexIndex1 : b.vertexIndex0);
				int num4 = (b.compareReversed ? b.vertexIndex0 : b.vertexIndex1);
				int num5 = num - num3;
				int num6 = num2 - num4;
				if (num5 == 0)
				{
					return num6;
				}
				return num5;
			}

			// Token: 0x06000A5F RID: 2655 RVA: 0x0004725B File Offset: 0x0004545B
			public int CompareTo(ShadowUtility.Edge edgeToCompare)
			{
				return this.Compare(this, edgeToCompare);
			}

			// Token: 0x040009AA RID: 2474
			public int vertexIndex0;

			// Token: 0x040009AB RID: 2475
			public int vertexIndex1;

			// Token: 0x040009AC RID: 2476
			public Vector4 tangent;

			// Token: 0x040009AD RID: 2477
			private bool compareReversed;
		}
	}
}
