using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000087 RID: 135
	public struct ProbeVolumeShadingParameters
	{
		// Token: 0x0400028A RID: 650
		public float normalBias;

		// Token: 0x0400028B RID: 651
		public float viewBias;

		// Token: 0x0400028C RID: 652
		public bool scaleBiasByMinDistanceBetweenProbes;

		// Token: 0x0400028D RID: 653
		public float samplingNoise;

		// Token: 0x0400028E RID: 654
		public float weight;

		// Token: 0x0400028F RID: 655
		public APVLeakReductionMode leakReductionMode;

		// Token: 0x04000290 RID: 656
		public float occlusionWeightContribution;

		// Token: 0x04000291 RID: 657
		public float minValidNormalWeight;

		// Token: 0x04000292 RID: 658
		public int frameIndexForNoise;

		// Token: 0x04000293 RID: 659
		public float reflNormalizationLowerClamp;

		// Token: 0x04000294 RID: 660
		public float reflNormalizationUpperClamp;
	}
}
