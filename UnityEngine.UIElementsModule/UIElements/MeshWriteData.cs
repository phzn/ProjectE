using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace UnityEngine.UIElements
{
	// Token: 0x020002AB RID: 683
	public class MeshWriteData
	{
		// Token: 0x060013A6 RID: 5030 RVA: 0x00044FA7 File Offset: 0x000431A7
		internal MeshWriteData()
		{
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x00044FB4 File Offset: 0x000431B4
		public int vertexCount
		{
			get
			{
				return this.m_Vertices.Length;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x00044FD4 File Offset: 0x000431D4
		public int indexCount
		{
			get
			{
				return this.m_Indices.Length;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x00044FF4 File Offset: 0x000431F4
		public Rect uvRegion
		{
			get
			{
				return this.m_UVRegion;
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0004500C File Offset: 0x0004320C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetNextVertex(Vertex vertex)
		{
			int num = this.currentVertex;
			this.currentVertex = num + 1;
			this.m_Vertices[num] = vertex;
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00045038 File Offset: 0x00043238
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetNextIndex(ushort index)
		{
			int num = this.currentIndex;
			this.currentIndex = num + 1;
			this.m_Indices[num] = index;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00045064 File Offset: 0x00043264
		public void SetAllVertices(Vertex[] vertices)
		{
			bool flag = this.currentVertex == 0;
			if (flag)
			{
				this.m_Vertices.CopyFrom(vertices);
				this.currentVertex = this.m_Vertices.Length;
				return;
			}
			throw new InvalidOperationException("SetAllVertices may not be called after using SetNextVertex");
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x000450AC File Offset: 0x000432AC
		public void SetAllVertices(NativeSlice<Vertex> vertices)
		{
			bool flag = this.currentVertex == 0;
			if (flag)
			{
				this.m_Vertices.CopyFrom(vertices);
				this.currentVertex = this.m_Vertices.Length;
				return;
			}
			throw new InvalidOperationException("SetAllVertices may not be called after using SetNextVertex");
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x000450F4 File Offset: 0x000432F4
		public void SetAllIndices(ushort[] indices)
		{
			bool flag = this.currentIndex == 0;
			if (flag)
			{
				this.m_Indices.CopyFrom(indices);
				this.currentIndex = this.m_Indices.Length;
				return;
			}
			throw new InvalidOperationException("SetAllIndices may not be called after using SetNextIndex");
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x0004513C File Offset: 0x0004333C
		public void SetAllIndices(NativeSlice<ushort> indices)
		{
			bool flag = this.currentIndex == 0;
			if (flag)
			{
				this.m_Indices.CopyFrom(indices);
				this.currentIndex = this.m_Indices.Length;
				return;
			}
			throw new InvalidOperationException("SetAllIndices may not be called after using SetNextIndex");
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00045184 File Offset: 0x00043384
		internal void Reset(NativeSlice<Vertex> vertices, NativeSlice<ushort> indices)
		{
			this.m_Vertices = vertices;
			this.m_Indices = indices;
			this.m_UVRegion = new Rect(0f, 0f, 1f, 1f);
			this.currentIndex = (this.currentVertex = 0);
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x000451D0 File Offset: 0x000433D0
		internal void Reset(NativeSlice<Vertex> vertices, NativeSlice<ushort> indices, Rect uvRegion)
		{
			this.m_Vertices = vertices;
			this.m_Indices = indices;
			this.m_UVRegion = uvRegion;
			this.currentIndex = (this.currentVertex = 0);
		}

		// Token: 0x04000903 RID: 2307
		internal NativeSlice<Vertex> m_Vertices;

		// Token: 0x04000904 RID: 2308
		internal NativeSlice<ushort> m_Indices;

		// Token: 0x04000905 RID: 2309
		internal Rect m_UVRegion;

		// Token: 0x04000906 RID: 2310
		internal int currentIndex;

		// Token: 0x04000907 RID: 2311
		internal int currentVertex;
	}
}
