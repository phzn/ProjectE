using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000FC RID: 252
	public struct ShadowData
	{
		// Token: 0x040006B9 RID: 1721
		public bool supportsMainLightShadows;

		// Token: 0x040006BA RID: 1722
		internal bool mainLightShadowsEnabled;

		// Token: 0x040006BB RID: 1723
		[Obsolete("Obsolete, this feature was replaced by new 'ScreenSpaceShadows' renderer feature")]
		public bool requiresScreenSpaceShadowResolve;

		// Token: 0x040006BC RID: 1724
		public int mainLightShadowmapWidth;

		// Token: 0x040006BD RID: 1725
		public int mainLightShadowmapHeight;

		// Token: 0x040006BE RID: 1726
		public int mainLightShadowCascadesCount;

		// Token: 0x040006BF RID: 1727
		public Vector3 mainLightShadowCascadesSplit;

		// Token: 0x040006C0 RID: 1728
		public float mainLightShadowCascadeBorder;

		// Token: 0x040006C1 RID: 1729
		public bool supportsAdditionalLightShadows;

		// Token: 0x040006C2 RID: 1730
		internal bool additionalLightShadowsEnabled;

		// Token: 0x040006C3 RID: 1731
		public int additionalLightsShadowmapWidth;

		// Token: 0x040006C4 RID: 1732
		public int additionalLightsShadowmapHeight;

		// Token: 0x040006C5 RID: 1733
		public bool supportsSoftShadows;

		// Token: 0x040006C6 RID: 1734
		public int shadowmapDepthBufferBits;

		// Token: 0x040006C7 RID: 1735
		public List<Vector4> bias;

		// Token: 0x040006C8 RID: 1736
		public List<int> resolution;

		// Token: 0x040006C9 RID: 1737
		internal bool isKeywordAdditionalLightShadowsEnabled;

		// Token: 0x040006CA RID: 1738
		internal bool isKeywordSoftShadowsEnabled;
	}
}
