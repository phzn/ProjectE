using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x0200012D RID: 301
	internal struct Smoothen
	{
		// Token: 0x060009F4 RID: 2548 RVA: 0x000432A0 File Offset: 0x000414A0
		private static void RefineEdges(ref NativeArray<int4> refinedEdges, ref NativeArray<int4> delaEdges, ref int delaEdgeCount, ref NativeArray<int4> voronoiEdges)
		{
			int num = delaEdgeCount;
			delaEdgeCount = 0;
			for (int i = 0; i < num - 1; i++)
			{
				int4 @int = delaEdges[i];
				int4 int2 = delaEdges[i + 1];
				if (@int.x == int2.x && @int.y == int2.y)
				{
					@int.w = int2.z;
					i++;
				}
				int num2 = delaEdgeCount;
				delaEdgeCount = num2 + 1;
				refinedEdges[num2] = @int;
			}
			for (int j = 0; j < delaEdgeCount; j++)
			{
				int z = refinedEdges[j].z;
				int w = refinedEdges[j].w;
				if (z != -1 && w != -1)
				{
					int4 int3 = new int4(w, z, j, 0);
					voronoiEdges[j] = int3;
				}
			}
			ModuleHandle.Copy<int4>(refinedEdges, delaEdges, delaEdgeCount);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00043378 File Offset: 0x00041578
		private static void GetAffectingEdges(int pointIndex, NativeArray<int4> edges, int edgeCount, ref NativeArray<int> resultSet, ref NativeArray<int> checkSet, ref int resultCount)
		{
			resultCount = 0;
			for (int i = 0; i < edgeCount; i++)
			{
				if (pointIndex == edges[i].x || pointIndex == edges[i].y)
				{
					int num = resultCount;
					resultCount = num + 1;
					resultSet[num] = i;
				}
				checkSet[i] = 0;
			}
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000433D0 File Offset: 0x000415D0
		private static void CentroidByPoints(int triIndex, NativeArray<UTriangle> triangles, ref NativeArray<int> centroidTris, ref int centroidCount, ref float2 aggregate, ref float2 point)
		{
			for (int i = 0; i < centroidCount; i++)
			{
				if (triIndex == centroidTris[i])
				{
					return;
				}
			}
			int num = centroidCount;
			centroidCount = num + 1;
			centroidTris[num] = triIndex;
			aggregate += triangles[triIndex].c.center;
			point = aggregate / (float)centroidCount;
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00043440 File Offset: 0x00041640
		private static void CentroidByPolygon(int4 e, NativeArray<UTriangle> triangles, ref float2 centroid, ref float area, ref float distance)
		{
			float2 center = triangles[e.x].c.center;
			float2 center2 = triangles[e.y].c.center;
			float num = center.x * center2.y - center2.x * center.y;
			distance += math.distance(center, center2);
			area += num;
			centroid.x += (center2.x + center.x) * num;
			centroid.y += (center2.y + center.y) * num;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x000434E0 File Offset: 0x000416E0
		private static bool ConnectTriangles(ref NativeArray<int4> connectedTri, ref NativeArray<int> affectEdges, ref NativeArray<int> checkSet, NativeArray<int4> voronoiEdges, int triangleCount)
		{
			int num = affectEdges[0];
			int num2 = affectEdges[0];
			connectedTri[0] = new int4(voronoiEdges[num].x, voronoiEdges[num].y, 0, 0);
			checkSet[num2] = 1;
			int i = 1;
			while (i < triangleCount)
			{
				num2 = affectEdges[i];
				if (checkSet[num2] != 0)
				{
					goto IL_00FF;
				}
				if (voronoiEdges[num2].x == connectedTri[i - 1].y)
				{
					connectedTri[i] = new int4(voronoiEdges[num2].x, voronoiEdges[num2].y, 0, 0);
					checkSet[num2] = 1;
				}
				else
				{
					if (voronoiEdges[num2].y != connectedTri[i - 1].y)
					{
						goto IL_00FF;
					}
					connectedTri[i] = new int4(voronoiEdges[num2].y, voronoiEdges[num2].x, 0, 0);
					checkSet[num2] = 1;
				}
				IL_01D5:
				i++;
				continue;
				IL_00FF:
				bool flag = false;
				for (int j = 0; j < triangleCount; j++)
				{
					num2 = affectEdges[j];
					if (checkSet[num2] != 1)
					{
						if (voronoiEdges[num2].x == connectedTri[i - 1].y)
						{
							connectedTri[i] = new int4(voronoiEdges[num2].x, voronoiEdges[num2].y, 0, 0);
							checkSet[num2] = 1;
							flag = true;
							break;
						}
						if (voronoiEdges[num2].y == connectedTri[i - 1].y)
						{
							connectedTri[i] = new int4(voronoiEdges[num2].y, voronoiEdges[num2].x, 0, 0);
							checkSet[num2] = 1;
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
				goto IL_01D5;
			}
			return true;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x000436D0 File Offset: 0x000418D0
		internal static bool Condition(Allocator allocator, ref NativeArray<float2> pgPoints, int pgPointCount, NativeArray<int2> pgEdges, int pgEdgeCount, ref NativeArray<float2> vertices, ref int vertexCount, ref NativeArray<int> indices, ref int indexCount)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			bool flag = true;
			bool flag2 = true;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			NativeArray<UTriangle> nativeArray = new NativeArray<UTriangle>(indexCount, allocator, NativeArrayOptions.ClearMemory);
			NativeArray<int4> nativeArray2 = new NativeArray<int4>(indexCount, allocator, NativeArrayOptions.ClearMemory);
			NativeArray<int4> nativeArray3 = new NativeArray<int4>(indexCount, allocator, NativeArrayOptions.ClearMemory);
			NativeArray<int4> nativeArray4 = new NativeArray<int4>(vertexCount, allocator, NativeArrayOptions.ClearMemory);
			NativeArray<int> nativeArray5 = new NativeArray<int>(indexCount, allocator, NativeArrayOptions.ClearMemory);
			NativeArray<int> nativeArray6 = new NativeArray<int>(indexCount, allocator, NativeArrayOptions.ClearMemory);
			NativeArray<int> nativeArray7 = new NativeArray<int>(vertexCount, allocator, NativeArrayOptions.ClearMemory);
			ModuleHandle.BuildTrianglesAndEdges(vertices, vertexCount, indices, indexCount, ref nativeArray, ref num9, ref nativeArray2, ref num10, ref num, ref num5, ref num3);
			NativeArray<int4> nativeArray8 = new NativeArray<int4>(num10, allocator, NativeArrayOptions.ClearMemory);
			ModuleHandle.InsertionSort<int4, DelaEdgeCompare>(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<int4>(nativeArray2), 0, num10 - 1, default(DelaEdgeCompare));
			Smoothen.RefineEdges(ref nativeArray8, ref nativeArray2, ref num10, ref nativeArray3);
			for (int i = 0; i < vertexCount; i++)
			{
				Smoothen.GetAffectingEdges(i, nativeArray2, num10, ref nativeArray6, ref nativeArray5, ref num11);
				bool flag3 = num11 != 0;
				for (int j = 0; j < num11; j++)
				{
					int num12 = nativeArray6[j];
					if (nativeArray2[num12].z == -1 || nativeArray2[num12].w == -1)
					{
						flag3 = false;
						break;
					}
				}
				if (flag3)
				{
					flag = Smoothen.ConnectTriangles(ref nativeArray4, ref nativeArray6, ref nativeArray5, nativeArray3, num11);
					if (!flag)
					{
						break;
					}
					float2 @float = float2.zero;
					float num13 = 0f;
					float num14 = 0f;
					for (int k = 0; k < num11; k++)
					{
						Smoothen.CentroidByPolygon(nativeArray4[k], nativeArray, ref @float, ref num13, ref num14);
					}
					@float /= 3f * num13;
					pgPoints[i] = @float;
				}
			}
			int num15 = indexCount;
			int num16 = vertexCount;
			indexCount = 0;
			vertexCount = 0;
			num9 = 0;
			if (flag)
			{
				flag2 = Tessellator.Tessellate(allocator, pgPoints, pgPointCount, pgEdges, pgEdgeCount, ref vertices, ref vertexCount, ref indices, ref indexCount);
				if (flag2)
				{
					ModuleHandle.BuildTriangles(vertices, vertexCount, indices, indexCount, ref nativeArray, ref num9, ref num2, ref num5, ref num4, ref num7, ref num8, ref num6);
				}
				flag2 = flag2 && num2 < num * Smoothen.kMaxAreaTolerance && num7 < num8 * Smoothen.kMaxEdgeTolerance;
			}
			nativeArray.Dispose();
			nativeArray2.Dispose();
			nativeArray8.Dispose();
			nativeArray5.Dispose();
			nativeArray3.Dispose();
			nativeArray6.Dispose();
			nativeArray7.Dispose();
			nativeArray4.Dispose();
			return flag2 && num15 == indexCount && num16 == vertexCount;
		}

		// Token: 0x040008DE RID: 2270
		private static readonly float kMaxAreaTolerance = 1.842f;

		// Token: 0x040008DF RID: 2271
		private static readonly float kMaxEdgeTolerance = 2.482f;
	}
}
