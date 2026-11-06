using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000C7 RID: 199
	[Serializable]
	internal class ScreenSpaceAmbientOcclusionSettings
	{
		// Token: 0x04000508 RID: 1288
		[SerializeField]
		internal ScreenSpaceAmbientOcclusionSettings.AOMethodOptions AOMethod;

		// Token: 0x04000509 RID: 1289
		[SerializeField]
		internal bool Downsample;

		// Token: 0x0400050A RID: 1290
		[SerializeField]
		internal bool AfterOpaque;

		// Token: 0x0400050B RID: 1291
		[SerializeField]
		internal ScreenSpaceAmbientOcclusionSettings.DepthSource Source = ScreenSpaceAmbientOcclusionSettings.DepthSource.DepthNormals;

		// Token: 0x0400050C RID: 1292
		[SerializeField]
		internal ScreenSpaceAmbientOcclusionSettings.NormalQuality NormalSamples = ScreenSpaceAmbientOcclusionSettings.NormalQuality.Medium;

		// Token: 0x0400050D RID: 1293
		[SerializeField]
		internal float Intensity = 3f;

		// Token: 0x0400050E RID: 1294
		[SerializeField]
		internal float DirectLightingStrength = 0.25f;

		// Token: 0x0400050F RID: 1295
		[SerializeField]
		internal float Radius = 0.035f;

		// Token: 0x04000510 RID: 1296
		[SerializeField]
		internal ScreenSpaceAmbientOcclusionSettings.AOSampleOption Samples = ScreenSpaceAmbientOcclusionSettings.AOSampleOption.Medium;

		// Token: 0x04000511 RID: 1297
		[SerializeField]
		internal ScreenSpaceAmbientOcclusionSettings.BlurQualityOptions BlurQuality;

		// Token: 0x04000512 RID: 1298
		[SerializeField]
		internal float Falloff = 100f;

		// Token: 0x04000513 RID: 1299
		[SerializeField]
		internal int SampleCount = -1;

		// Token: 0x020001AD RID: 429
		internal enum DepthSource
		{
			// Token: 0x04000B35 RID: 2869
			Depth,
			// Token: 0x04000B36 RID: 2870
			DepthNormals
		}

		// Token: 0x020001AE RID: 430
		internal enum NormalQuality
		{
			// Token: 0x04000B38 RID: 2872
			Low,
			// Token: 0x04000B39 RID: 2873
			Medium,
			// Token: 0x04000B3A RID: 2874
			High
		}

		// Token: 0x020001AF RID: 431
		internal enum AOSampleOption
		{
			// Token: 0x04000B3C RID: 2876
			High,
			// Token: 0x04000B3D RID: 2877
			Medium,
			// Token: 0x04000B3E RID: 2878
			Low
		}

		// Token: 0x020001B0 RID: 432
		internal enum AOMethodOptions
		{
			// Token: 0x04000B40 RID: 2880
			BlueNoise,
			// Token: 0x04000B41 RID: 2881
			InterleavedGradient
		}

		// Token: 0x020001B1 RID: 433
		internal enum BlurQualityOptions
		{
			// Token: 0x04000B43 RID: 2883
			High,
			// Token: 0x04000B44 RID: 2884
			Medium,
			// Token: 0x04000B45 RID: 2885
			Low
		}
	}
}
