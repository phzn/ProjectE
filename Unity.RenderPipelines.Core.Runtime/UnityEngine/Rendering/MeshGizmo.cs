using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Rendering
{
	// Token: 0x020000DD RID: 221
	internal class MeshGizmo : IDisposable
	{
		// Token: 0x0600076E RID: 1902 RVA: 0x00024840 File Offset: 0x00022A40
		public MeshGizmo(int capacity = 0)
		{
			this.vertices = new List<Vector3>(capacity);
			this.indices = new List<int>(capacity);
			this.colors = new List<Color>(capacity);
			this.mesh = new Mesh
			{
				indexFormat = IndexFormat.UInt32,
				hideFlags = HideFlags.HideAndDontSave
			};
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00024891 File Offset: 0x00022A91
		public void Clear()
		{
			this.vertices.Clear();
			this.indices.Clear();
			this.colors.Clear();
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x000248B4 File Offset: 0x00022AB4
		public void AddWireCube(Vector3 center, Vector3 size, Color color)
		{
			MeshGizmo.<>c__DisplayClass10_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.color = color;
			Vector3 vector = size / 2f;
			Vector3 vector2 = new Vector3(vector.x, vector.y, vector.z);
			Vector3 vector3 = new Vector3(-vector.x, vector.y, vector.z);
			Vector3 vector4 = new Vector3(-vector.x, -vector.y, vector.z);
			Vector3 vector5 = new Vector3(vector.x, -vector.y, vector.z);
			Vector3 vector6 = new Vector3(vector.x, vector.y, -vector.z);
			Vector3 vector7 = new Vector3(-vector.x, vector.y, -vector.z);
			Vector3 vector8 = new Vector3(-vector.x, -vector.y, -vector.z);
			Vector3 vector9 = new Vector3(vector.x, -vector.y, -vector.z);
			this.<AddWireCube>g__AddEdge|10_0(center + vector2, center + vector3, ref CS$<>8__locals1);
			this.<AddWireCube>g__AddEdge|10_0(center + vector3, center + vector4, ref CS$<>8__locals1);
			this.<AddWireCube>g__AddEdge|10_0(center + vector4, center + vector5, ref CS$<>8__locals1);
			this.<AddWireCube>g__AddEdge|10_0(center + vector5, center + vector2, ref CS$<>8__locals1);
			this.<AddWireCube>g__AddEdge|10_0(center + vector6, center + vector7, ref CS$<>8__locals1);
			this.<AddWireCube>g__AddEdge|10_0(center + vector7, center + vector8, ref CS$<>8__locals1);
			this.<AddWireCube>g__AddEdge|10_0(center + vector8, center + vector9, ref CS$<>8__locals1);
			this.<AddWireCube>g__AddEdge|10_0(center + vector9, center + vector6, ref CS$<>8__locals1);
			this.<AddWireCube>g__AddEdge|10_0(center + vector2, center + vector6, ref CS$<>8__locals1);
			this.<AddWireCube>g__AddEdge|10_0(center + vector3, center + vector7, ref CS$<>8__locals1);
			this.<AddWireCube>g__AddEdge|10_0(center + vector4, center + vector8, ref CS$<>8__locals1);
			this.<AddWireCube>g__AddEdge|10_0(center + vector5, center + vector9, ref CS$<>8__locals1);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00024ACC File Offset: 0x00022CCC
		private void DrawMesh(Matrix4x4 trs, Material mat, MeshTopology topology, CompareFunction depthTest, string gizmoName)
		{
			this.mesh.Clear();
			this.mesh.SetVertices(this.vertices);
			this.mesh.SetColors(this.colors);
			this.mesh.SetIndices(this.indices, topology, 0, true, 0);
			mat.SetFloat("_HandleZTest", (float)depthTest);
			CommandBuffer commandBuffer = CommandBufferPool.Get(gizmoName ?? "Mesh Gizmo Rendering");
			commandBuffer.DrawMesh(this.mesh, trs, mat, 0, 0);
			Graphics.ExecuteCommandBuffer(commandBuffer);
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00024B4E File Offset: 0x00022D4E
		public void RenderWireframe(Matrix4x4 trs, CompareFunction depthTest = CompareFunction.LessEqual, string gizmoName = null)
		{
			this.DrawMesh(trs, this.wireMaterial, MeshTopology.Lines, depthTest, gizmoName);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00024B60 File Offset: 0x00022D60
		public void Dispose()
		{
			CoreUtils.Destroy(this.mesh);
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00024B78 File Offset: 0x00022D78
		[CompilerGenerated]
		private void <AddWireCube>g__AddEdge|10_0(Vector3 p1, Vector3 p2, ref MeshGizmo.<>c__DisplayClass10_0 A_3)
		{
			this.vertices.Add(p1);
			this.vertices.Add(p2);
			this.indices.Add(this.indices.Count);
			this.indices.Add(this.indices.Count);
			this.colors.Add(A_3.color);
			this.colors.Add(A_3.color);
		}

		// Token: 0x040004A5 RID: 1189
		public static readonly int vertexCountPerCube = 24;

		// Token: 0x040004A6 RID: 1190
		public Mesh mesh;

		// Token: 0x040004A7 RID: 1191
		private List<Vector3> vertices;

		// Token: 0x040004A8 RID: 1192
		private List<int> indices;

		// Token: 0x040004A9 RID: 1193
		private List<Color> colors;

		// Token: 0x040004AA RID: 1194
		private Material wireMaterial;

		// Token: 0x040004AB RID: 1195
		private Material dottedWireMaterial;

		// Token: 0x040004AC RID: 1196
		private Material solidMaterial;
	}
}
