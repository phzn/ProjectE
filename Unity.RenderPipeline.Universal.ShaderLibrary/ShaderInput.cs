using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000010 RID: 16
	public static class ShaderInput
	{
		// Token: 0x02000013 RID: 19
		[Obsolete("ShaderInput.ShadowData was deprecated. Shadow slice matrices and per-light shadow parameters are now passed to the GPU using entries in buffers m_AdditionalLightsWorldToShadow_SSBO and m_AdditionalShadowParams_SSBO", false)]
		public struct ShadowData
		{
			// Token: 0x0400005A RID: 90
			public Matrix4x4 worldToShadowMatrix;

			// Token: 0x0400005B RID: 91
			public Vector4 shadowParams;
		}

		// Token: 0x02000014 RID: 20
		[GenerateHLSL(PackingRules.Exact, false, false, false, 1, false, false, false, -1, ".\\Packages\\com.unity.render-pipelines.universal\\ShaderLibrary\\ShaderTypes.cs")]
		public struct LightData
		{
			// Token: 0x0400005C RID: 92
			public Vector4 position;

			// Token: 0x0400005D RID: 93
			public Vector4 color;

			// Token: 0x0400005E RID: 94
			public Vector4 attenuation;

			// Token: 0x0400005F RID: 95
			public Vector4 spotDirection;

			// Token: 0x04000060 RID: 96
			public Vector4 occlusionProbeChannels;

			// Token: 0x04000061 RID: 97
			public uint layerMask;
		}
	}
}
