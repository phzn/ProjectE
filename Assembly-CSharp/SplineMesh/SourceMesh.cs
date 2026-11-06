using System;
using System.Collections.Generic;
using UnityEngine;

namespace SplineMesh
{
	// Token: 0x02000199 RID: 409
	public struct SourceMesh
	{
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x00056BDE File Offset: 0x00054DDE
		internal readonly Mesh Mesh { get; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x00056BE6 File Offset: 0x00054DE6
		internal List<MeshVertex> Vertices
		{
			get
			{
				if (this.vertices == null)
				{
					this.BuildData();
				}
				return this.vertices;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x00056BFC File Offset: 0x00054DFC
		internal int[] Triangles
		{
			get
			{
				if (this.vertices == null)
				{
					this.BuildData();
				}
				return this.triangles;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x00056C12 File Offset: 0x00054E12
		internal float MinX
		{
			get
			{
				if (this.vertices == null)
				{
					this.BuildData();
				}
				return this.minX;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x00056C28 File Offset: 0x00054E28
		internal float Length
		{
			get
			{
				if (this.vertices == null)
				{
					this.BuildData();
				}
				return this.length;
			}
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00056C40 File Offset: 0x00054E40
		private SourceMesh(Mesh mesh)
		{
			this.Mesh = mesh;
			this.translation = default(Vector3);
			this.rotation = default(Quaternion);
			this.scale = default(Vector3);
			this.vertices = null;
			this.triangles = null;
			this.minX = 0f;
			this.length = 0f;
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x00056C9C File Offset: 0x00054E9C
		private SourceMesh(SourceMesh other)
		{
			this.Mesh = other.Mesh;
			this.translation = other.translation;
			this.rotation = other.rotation;
			this.scale = other.scale;
			this.vertices = null;
			this.triangles = null;
			this.minX = 0f;
			this.length = 0f;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00056CFE File Offset: 0x00054EFE
		public static SourceMesh Build(Mesh mesh)
		{
			return new SourceMesh(mesh);
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00056D08 File Offset: 0x00054F08
		public SourceMesh Translate(Vector3 translation)
		{
			return new SourceMesh(this)
			{
				translation = translation
			};
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00056D2B File Offset: 0x00054F2B
		public SourceMesh Translate(float x, float y, float z)
		{
			return this.Translate(new Vector3(x, y, z));
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00056D3C File Offset: 0x00054F3C
		public SourceMesh Rotate(Quaternion rotation)
		{
			return new SourceMesh(this)
			{
				rotation = rotation
			};
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00056D60 File Offset: 0x00054F60
		public SourceMesh Scale(Vector3 scale)
		{
			return new SourceMesh(this)
			{
				scale = scale
			};
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00056D83 File Offset: 0x00054F83
		public SourceMesh Scale(float x, float y, float z)
		{
			return this.Scale(new Vector3(x, y, z));
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00056D94 File Offset: 0x00054F94
		private void BuildData()
		{
			bool flag = this.scale.x < 0f;
			if (this.scale.y < 0f)
			{
				flag = !flag;
			}
			if (this.scale.z < 0f)
			{
				flag = !flag;
			}
			this.triangles = (flag ? MeshUtility.GetReversedTriangles(this.Mesh) : this.Mesh.triangles);
			int num = 0;
			this.vertices = new List<MeshVertex>(this.Mesh.vertexCount);
			Vector3[] array = this.Mesh.vertices;
			for (int i = 0; i < array.Length; i++)
			{
				MeshVertex meshVertex = new MeshVertex(array[i], this.Mesh.normals[num++]);
				if (this.rotation != Quaternion.identity)
				{
					meshVertex.position = this.rotation * meshVertex.position;
					meshVertex.normal = this.rotation * meshVertex.normal;
				}
				if (this.scale != Vector3.one)
				{
					meshVertex.position = Vector3.Scale(meshVertex.position, this.scale);
					meshVertex.normal = Vector3.Scale(meshVertex.normal, this.scale);
				}
				if (this.translation != Vector3.zero)
				{
					meshVertex.position += this.translation;
				}
				this.vertices.Add(meshVertex);
			}
			this.minX = float.MaxValue;
			float num2 = float.MinValue;
			foreach (MeshVertex meshVertex2 in this.vertices)
			{
				Vector3 position = meshVertex2.position;
				num2 = Math.Max(num2, position.x);
				this.minX = Math.Min(this.minX, position.x);
			}
			this.length = Math.Abs(num2 - this.minX);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00056FB4 File Offset: 0x000551B4
		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			SourceMesh sourceMesh = (SourceMesh)obj;
			return this.Mesh == sourceMesh.Mesh && this.translation == sourceMesh.translation && this.rotation == sourceMesh.rotation && this.scale == sourceMesh.scale;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00057037 File Offset: 0x00055237
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00057049 File Offset: 0x00055249
		public static bool operator ==(SourceMesh sm1, SourceMesh sm2)
		{
			return sm1.Equals(sm2);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00057049 File Offset: 0x00055249
		public static bool operator !=(SourceMesh sm1, SourceMesh sm2)
		{
			return sm1.Equals(sm2);
		}

		// Token: 0x04000C92 RID: 3218
		private Vector3 translation;

		// Token: 0x04000C93 RID: 3219
		private Quaternion rotation;

		// Token: 0x04000C94 RID: 3220
		private Vector3 scale;

		// Token: 0x04000C96 RID: 3222
		private List<MeshVertex> vertices;

		// Token: 0x04000C97 RID: 3223
		private int[] triangles;

		// Token: 0x04000C98 RID: 3224
		private float minX;

		// Token: 0x04000C99 RID: 3225
		private float length;
	}
}
