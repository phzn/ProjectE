using System;
using System.Collections.Generic;
using UnityEngine;

namespace DA_Assets.SVGMeshUnity.Internals.Cdt2d
{
	// Token: 0x02000012 RID: 18
	public class Triangles
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00006703 File Offset: 0x00004903
		public Triangles(MeshData data)
		{
			this.PrivateVertices = data.Vertices;
			this.CreateStars(data.Vertices.Count);
			this.CreateEdges(data.Edges);
			this.CreateTriangles(data.Triangles);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00006740 File Offset: 0x00004940
		public List<Vector3> Vertices
		{
			get
			{
				return this.PrivateVertices;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00006748 File Offset: 0x00004948
		public WorkBuffer<Int2>[] Stars
		{
			get
			{
				return this.PrivateStars;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00006750 File Offset: 0x00004950
		private void CreateStars(int n)
		{
			WorkBuffer<Int2>[] array = new WorkBuffer<Int2>[n];
			for (int i = 0; i < n; i++)
			{
				array[i] = new WorkBuffer<Int2>(16);
			}
			this.PrivateStars = array;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00006784 File Offset: 0x00004984
		private void CreateEdges(List<Int2> source)
		{
			int count = source.Count;
			Int2[] array = new Int2[count];
			for (int i = 0; i < count; i++)
			{
				Int2 @int = source[i];
				if (@int.y < @int.x)
				{
					int x = @int.x;
					@int.x = @int.y;
					@int.y = x;
				}
				array[i] = @int;
			}
			Sort<Int2>.QuickSort(array, 0, array.Length - 1);
			this.Edges = array;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000067FC File Offset: 0x000049FC
		private void CreateTriangles(List<int> source)
		{
			int count = source.Count;
			for (int i = 0; i < count; i += 3)
			{
				this.AddTriangle(source[i], source[i + 1], source[i + 2]);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000683C File Offset: 0x00004A3C
		public void AddTriangle(int i, int j, int k)
		{
			Int2 @int = new Int2(j, k);
			Int2 int2 = new Int2(k, i);
			Int2 int3 = new Int2(i, j);
			this.PrivateStars[i].Push(ref @int);
			this.PrivateStars[j].Push(ref int2);
			this.PrivateStars[k].Push(ref int3);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00006891 File Offset: 0x00004A91
		public void RemoveTriangle(int i, int j, int k)
		{
			this.RemovePair(this.PrivateStars[i], j, k);
			this.RemovePair(this.PrivateStars[j], k, i);
			this.RemovePair(this.PrivateStars[k], i, j);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000068C4 File Offset: 0x00004AC4
		private void RemovePair(WorkBuffer<Int2> list, int j, int k)
		{
			int usedSize = list.UsedSize;
			Int2[] data = list.Data;
			for (int i = 0; i < usedSize; i++)
			{
				Int2 @int = data[i];
				if (@int.x == j && @int.y == k)
				{
					data[i] = data[usedSize - 1];
					list.RemoveLast(1);
					return;
				}
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00006920 File Offset: 0x00004B20
		public int Opposite(int j, int i)
		{
			WorkBuffer<Int2> workBuffer = this.PrivateStars[i];
			int usedSize = workBuffer.UsedSize;
			Int2[] data = workBuffer.Data;
			for (int k = 0; k < usedSize; k++)
			{
				if (data[k].y == j)
				{
					return data[k].x;
				}
			}
			return -1;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000696C File Offset: 0x00004B6C
		public void Flip(int i, int j)
		{
			int num = this.Opposite(i, j);
			int num2 = this.Opposite(j, i);
			this.RemoveTriangle(i, j, num);
			this.RemoveTriangle(j, i, num2);
			this.AddTriangle(i, num2, num);
			this.AddTriangle(j, num, num2);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000069B0 File Offset: 0x00004BB0
		public bool IsConstraint(int i, int j)
		{
			Int2 @int = new Int2(Mathf.Min(i, j), Mathf.Max(i, j));
			return BinarySearch.EQ<Int2>(this.Edges, @int, 0, this.Edges.Length) >= 0;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000069F0 File Offset: 0x00004BF0
		public void Fill(List<int> triangles)
		{
			int num = this.PrivateStars.Length;
			if (triangles.Capacity < num)
			{
				triangles.Capacity = num;
			}
			triangles.Clear();
			for (int i = 0; i < num; i++)
			{
				WorkBuffer<Int2> workBuffer = this.PrivateStars[i];
				Int2[] data = workBuffer.Data;
				int usedSize = workBuffer.UsedSize;
				for (int j = 0; j < usedSize; j++)
				{
					Int2 @int = data[j];
					if (i < Mathf.Min(@int.x, @int.y))
					{
						triangles.Add(i);
						triangles.Add(@int.x);
						triangles.Add(@int.y);
					}
				}
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00006A90 File Offset: 0x00004C90
		public void Fill(WorkBuffer<Int3> triangles)
		{
			int num = this.PrivateStars.Length;
			triangles.Extend(num);
			triangles.Clear();
			for (int i = 0; i < num; i++)
			{
				WorkBuffer<Int2> workBuffer = this.PrivateStars[i];
				Int2[] data = workBuffer.Data;
				int usedSize = workBuffer.UsedSize;
				for (int j = 0; j < usedSize; j++)
				{
					Int2 @int = data[j];
					if (i < Mathf.Min(@int.x, @int.y))
					{
						Int3 int2 = new Int3(i, @int.x, @int.y);
						triangles.Push(ref int2);
					}
				}
			}
		}

		// Token: 0x04000038 RID: 56
		private List<Vector3> PrivateVertices;

		// Token: 0x04000039 RID: 57
		private WorkBuffer<Int2>[] PrivateStars;

		// Token: 0x0400003A RID: 58
		private Int2[] Edges;
	}
}
