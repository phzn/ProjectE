using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DA_Assets.SVGMeshUnity.Internals
{
	// Token: 0x02000009 RID: 9
	public class MeshData
	{
		// Token: 0x0600003C RID: 60 RVA: 0x000039FA File Offset: 0x00001BFA
		public MeshData()
		{
			this.Vertices = new List<Vector3>();
			this.Edges = new List<Int2>();
			this.Triangles = new List<int>();
			this.VertexIndices = new Hashtable();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003A2E File Offset: 0x00001C2E
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00003A36 File Offset: 0x00001C36
		public List<Vector3> Vertices { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003A3F File Offset: 0x00001C3F
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00003A47 File Offset: 0x00001C47
		public List<Int2> Edges { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003A50 File Offset: 0x00001C50
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00003A58 File Offset: 0x00001C58
		public List<int> Triangles { get; private set; }

		// Token: 0x06000043 RID: 67 RVA: 0x00003A61 File Offset: 0x00001C61
		public void Clear()
		{
			this.Vertices.Clear();
			this.Edges.Clear();
			this.Triangles.Clear();
			this.VertexIndices.Clear();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003A90 File Offset: 0x00001C90
		public void AddVertices(WorkBuffer<Vector2> buffer)
		{
			int num = -1;
			int num2 = -1;
			List<Vector3> vertices = this.Vertices;
			List<Int2> edges = this.Edges;
			Hashtable vertexIndices = this.VertexIndices;
			int usedSize = buffer.UsedSize;
			Vector2[] data = buffer.Data;
			for (int i = 0; i < usedSize; i++)
			{
				Vector2 vector = data[i];
				int num3 = -1;
				object obj = vertexIndices[vector];
				if (obj != null)
				{
					num3 = (int)obj;
				}
				if (num3 == -1)
				{
					vertices.Add(vector);
					num3 = vertices.Count - 1;
					vertexIndices[vector] = num3;
				}
				if (num3 != num2)
				{
					if (i == 0)
					{
						num = num3;
					}
					else
					{
						edges.Add(new Int2(num2, num3));
					}
					num2 = num3;
				}
			}
			if (num2 != num)
			{
				edges.Add(new Int2(num2, num));
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003B6C File Offset: 0x00001D6C
		public void MakeUnityFriendly()
		{
			List<Vector3> vertices = this.Vertices;
			int count = vertices.Count;
			for (int i = 0; i < count; i++)
			{
				Vector3 vector = vertices[i];
				vector.y *= -1f;
				vertices[i] = vector;
			}
			List<int> triangles = this.Triangles;
			int count2 = triangles.Count;
			for (int j = 0; j < count2; j += 3)
			{
				int num = triangles[j];
				int num2 = triangles[j + 1];
				int num3 = triangles[j + 2];
				triangles[j] = num2;
				triangles[j + 1] = num3;
				triangles[j + 2] = num;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003C1F File Offset: 0x00001E1F
		public void Upload(Mesh m)
		{
			m.Clear();
			m.SetVertices(this.Vertices);
			m.SetTriangles(this.Triangles, 0);
			m.RecalculateBounds();
			m.RecalculateNormals();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003C4C File Offset: 0x00001E4C
		public void DumpTriangles()
		{
			Debug.Log(this.Triangles.Aggregate("", (string _, int i) => _ + i.ToString() + "\n"));
		}

		// Token: 0x04000027 RID: 39
		private Hashtable VertexIndices;
	}
}
