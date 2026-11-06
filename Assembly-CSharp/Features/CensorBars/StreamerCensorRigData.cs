using System;
using UnityEngine;

namespace Features.CensorBars
{
	// Token: 0x020014C3 RID: 5315
	public sealed class StreamerCensorRigData : ScriptableObject
	{
		// Token: 0x0400A723 RID: 42787
		public Mesh sourceBody;

		// Token: 0x0400A724 RID: 42788
		public Mesh bodyProbe;

		// Token: 0x0400A725 RID: 42789
		public Mesh boxMesh;

		// Token: 0x0400A726 RID: 42790
		public int[] regions;

		// Token: 0x0400A727 RID: 42791
		public int[] sourceVertices;

		// Token: 0x0400A728 RID: 42792
		public int[] coverageVertices;

		// Token: 0x0400A729 RID: 42793
		public int[] coverageRegions;

		// Token: 0x0400A72A RID: 42794
		public string sourceDependencyHash;
	}
}
