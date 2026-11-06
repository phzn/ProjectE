using System;
using Unity.Collections;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000FA RID: 250
	public struct LightData
	{
		// Token: 0x0400067D RID: 1661
		public int mainLightIndex;

		// Token: 0x0400067E RID: 1662
		public int additionalLightsCount;

		// Token: 0x0400067F RID: 1663
		public int maxPerObjectAdditionalLightsCount;

		// Token: 0x04000680 RID: 1664
		public NativeArray<VisibleLight> visibleLights;

		// Token: 0x04000681 RID: 1665
		public bool shadeAdditionalLightsPerVertex;

		// Token: 0x04000682 RID: 1666
		public bool supportsMixedLighting;

		// Token: 0x04000683 RID: 1667
		public bool reflectionProbeBoxProjection;

		// Token: 0x04000684 RID: 1668
		public bool reflectionProbeBlending;

		// Token: 0x04000685 RID: 1669
		public bool supportsLightLayers;

		// Token: 0x04000686 RID: 1670
		public bool supportsAdditionalLights;
	}
}
