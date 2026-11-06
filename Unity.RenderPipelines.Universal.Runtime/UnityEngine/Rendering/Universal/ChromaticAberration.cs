using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200008F RID: 143
	[VolumeComponentMenuForRenderPipeline("Post-processing/Chromatic Aberration", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class ChromaticAberration : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x0001F642 File Offset: 0x0001D842
		public bool IsActive()
		{
			return this.intensity.value > 0f;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001F656 File Offset: 0x0001D856
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x040003DE RID: 990
		[Tooltip("Use the slider to set the strength of the Chromatic Aberration effect.")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f, false);
	}
}
