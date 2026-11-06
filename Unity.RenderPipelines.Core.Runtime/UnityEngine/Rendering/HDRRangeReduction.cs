using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200009D RID: 157
	[GenerateHLSL(PackingRules.Exact, true, false, false, 1, false, false, false, -1, ".\\Library\\PackageCache\\com.unity.render-pipelines.core@14.0.12\\Runtime\\PostProcessing\\HDROutputDefines.cs")]
	public enum HDRRangeReduction
	{
		// Token: 0x04000360 RID: 864
		None,
		// Token: 0x04000361 RID: 865
		Reinhard,
		// Token: 0x04000362 RID: 866
		BT2390,
		// Token: 0x04000363 RID: 867
		ACES1000Nits,
		// Token: 0x04000364 RID: 868
		ACES2000Nits,
		// Token: 0x04000365 RID: 869
		ACES4000Nits
	}
}
