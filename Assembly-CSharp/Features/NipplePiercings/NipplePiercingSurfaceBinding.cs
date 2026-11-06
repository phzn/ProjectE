using System;
using UnityEngine;

namespace Features.NipplePiercings
{
	// Token: 0x02000F6A RID: 3946
	public sealed class NipplePiercingSurfaceBinding : ScriptableObject
	{
		// Token: 0x04007A2E RID: 31278
		public Mesh sourceMesh;

		// Token: 0x04007A2F RID: 31279
		public NipplePiercingSurfaceBinding.Vertex[] vertices;

		// Token: 0x04007A30 RID: 31280
		public int leftCount;

		// Token: 0x04007A31 RID: 31281
		public Matrix4x4[] bindposes;

		// Token: 0x04007A32 RID: 31282
		public NipplePiercingSurfaceBinding.Shape[] shapes;

		// Token: 0x02000F6B RID: 3947
		[Serializable]
		public sealed class Vertex
		{
			// Token: 0x04007A33 RID: 31283
			public int index;

			// Token: 0x04007A34 RID: 31284
			public Vector3 position;

			// Token: 0x04007A35 RID: 31285
			public Vector3 normal;

			// Token: 0x04007A36 RID: 31286
			public BoneWeight weights;
		}

		// Token: 0x02000F6C RID: 3948
		[Serializable]
		public sealed class Frame
		{
			// Token: 0x04007A37 RID: 31287
			public float weight;

			// Token: 0x04007A38 RID: 31288
			public Vector3[] positions;

			// Token: 0x04007A39 RID: 31289
			public Vector3[] normals;
		}

		// Token: 0x02000F6D RID: 3949
		[Serializable]
		public sealed class Shape
		{
			// Token: 0x04007A3A RID: 31290
			public string name;

			// Token: 0x04007A3B RID: 31291
			public NipplePiercingSurfaceBinding.Frame[] frames;
		}
	}
}
