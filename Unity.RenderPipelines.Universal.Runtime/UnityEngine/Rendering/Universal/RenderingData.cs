using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000F9 RID: 249
	public struct RenderingData
	{
		// Token: 0x04000674 RID: 1652
		internal CommandBuffer commandBuffer;

		// Token: 0x04000675 RID: 1653
		public CullingResults cullResults;

		// Token: 0x04000676 RID: 1654
		public CameraData cameraData;

		// Token: 0x04000677 RID: 1655
		public LightData lightData;

		// Token: 0x04000678 RID: 1656
		public ShadowData shadowData;

		// Token: 0x04000679 RID: 1657
		public PostProcessingData postProcessingData;

		// Token: 0x0400067A RID: 1658
		public bool supportsDynamicBatching;

		// Token: 0x0400067B RID: 1659
		public PerObjectData perObjectData;

		// Token: 0x0400067C RID: 1660
		public bool postProcessingEnabled;
	}
}
