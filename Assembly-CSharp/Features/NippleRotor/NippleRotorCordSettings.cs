using System;

namespace Features.NippleRotor
{
	// Token: 0x02000F57 RID: 3927
	[Serializable]
	public sealed class NippleRotorCordSettings
	{
		// Token: 0x040078D4 RID: 30932
		public float branchLength = 0.08f;

		// Token: 0x040078D5 RID: 30933
		public float mainLength = 0.34f;

		// Token: 0x040078D6 RID: 30934
		public float maxStretch = 1.6f;

		// Token: 0x040078D7 RID: 30935
		public int branchNodes = 5;

		// Token: 0x040078D8 RID: 30936
		public int mainNodes = 18;

		// Token: 0x040078D9 RID: 30937
		public float radius = 0.0011f;

		// Token: 0x040078DA RID: 30938
		public int radialSegments = 6;

		// Token: 0x040078DB RID: 30939
		public float gravity = 1f;

		// Token: 0x040078DC RID: 30940
		public float damping = 2.5f;

		// Token: 0x040078DD RID: 30941
		public float motionInfluence = 0.15f;

		// Token: 0x040078DE RID: 30942
		public int substepHz = 120;

		// Token: 0x040078DF RID: 30943
		public int iterations = 14;

		// Token: 0x040078E0 RID: 30944
		public float teleportDistance = 0.35f;

		// Token: 0x040078E1 RID: 30945
		public float torsoRadius = 0.11f;

		// Token: 0x040078E2 RID: 30946
		public float thighRadius = 0.075f;

		// Token: 0x040078E3 RID: 30947
		public float breastRadius = 0.055f;

		// Token: 0x040078E4 RID: 30948
		public float shapeSpring = 4f;

		// Token: 0x040078E5 RID: 30949
		public int renderSubdivisions = 3;

		// Token: 0x040078E6 RID: 30950
		public float textureRepeatsPerUnit = 40f;

		// Token: 0x040078E7 RID: 30951
		public float shinRadius = 0.04f;

		// Token: 0x040078E8 RID: 30952
		public float freeEndMass = 4f;

		// Token: 0x040078E9 RID: 30953
		public float freeEndRadius = 0.01f;

		// Token: 0x040078EA RID: 30954
		public float exitMinElevation = 10f;

		// Token: 0x040078EB RID: 30955
		public float exitStartHeight = 0.0073f;

		// Token: 0x040078EC RID: 30956
		public float skinClearance = 0.0006f;

		// Token: 0x040078ED RID: 30957
		public float skinContactReach = 0.01f;
	}
}
