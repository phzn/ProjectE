using System;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization
{
	// Token: 0x02000AC9 RID: 2761
	public sealed class CielHairBodySurfaceBinding : ScriptableObject
	{
		// Token: 0x04004FAE RID: 20398
		public Mesh sourceMesh;

		// Token: 0x04004FAF RID: 20399
		public int sourceVertexCount;

		// Token: 0x04004FB0 RID: 20400
		public string[] boneNames;

		// Token: 0x04004FB1 RID: 20401
		public Matrix4x4[] bindposes;

		// Token: 0x04004FB2 RID: 20402
		public CielHairBodySurfaceBinding.Vertex[] vertices;

		// Token: 0x04004FB3 RID: 20403
		public CielHairBodySurfaceBinding.Shape[] shapes;

		// Token: 0x04004FB4 RID: 20404
		public CielHairBodySurfaceBinding.Patch[] patches;

		// Token: 0x02000ACA RID: 2762
		[Serializable]
		public struct Vertex
		{
			// Token: 0x04004FB5 RID: 20405
			public int sourceIndex;

			// Token: 0x04004FB6 RID: 20406
			public Vector3 position;

			// Token: 0x04004FB7 RID: 20407
			public BoneWeight weights;
		}

		// Token: 0x02000ACB RID: 2763
		[Serializable]
		public sealed class Frame
		{
			// Token: 0x04004FB8 RID: 20408
			public float weight;

			// Token: 0x04004FB9 RID: 20409
			public Vector3[] deltas;
		}

		// Token: 0x02000ACC RID: 2764
		[Serializable]
		public sealed class Shape
		{
			// Token: 0x04004FBA RID: 20410
			public string name;

			// Token: 0x04004FBB RID: 20411
			public CielHairBodySurfaceBinding.Frame[] frames;
		}

		// Token: 0x02000ACD RID: 2765
		[Serializable]
		public sealed class Patch
		{
			// Token: 0x04004FBC RID: 20412
			public string name;

			// Token: 0x04004FBD RID: 20413
			public int frameBone;

			// Token: 0x04004FBE RID: 20414
			public Quaternion patchToFrame = Quaternion.identity;

			// Token: 0x04004FBF RID: 20415
			public bool front;

			// Token: 0x04004FC0 RID: 20416
			public int[] vertices;
		}
	}
}
