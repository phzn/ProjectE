using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000099 RID: 153
	[VolumeComponentMenuForRenderPipeline("Post-processing/Lens Distortion", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class LensDistortion : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000578 RID: 1400 RVA: 0x0001FD04 File Offset: 0x0001DF04
		public bool IsActive()
		{
			return Mathf.Abs(this.intensity.value) > 0f && (this.xMultiplier.value > 0f || this.yMultiplier.value > 0f);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001FD50 File Offset: 0x0001DF50
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x0400040D RID: 1037
		[Tooltip("Total distortion amount.")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, -1f, 1f, false);

		// Token: 0x0400040E RID: 1038
		[Tooltip("Intensity multiplier on X axis. Set it to 0 to disable distortion on this axis.")]
		public ClampedFloatParameter xMultiplier = new ClampedFloatParameter(1f, 0f, 1f, false);

		// Token: 0x0400040F RID: 1039
		[Tooltip("Intensity multiplier on Y axis. Set it to 0 to disable distortion on this axis.")]
		public ClampedFloatParameter yMultiplier = new ClampedFloatParameter(1f, 0f, 1f, false);

		// Token: 0x04000410 RID: 1040
		[Tooltip("Distortion center point. 0.5,0.5 is center of the screen")]
		public Vector2Parameter center = new Vector2Parameter(new Vector2(0.5f, 0.5f), false);

		// Token: 0x04000411 RID: 1041
		[Tooltip("Controls global screen scaling for the distortion effect. Use this to hide screen borders when using high \"Intensity.\"")]
		public ClampedFloatParameter scale = new ClampedFloatParameter(1f, 0.01f, 5f, false);
	}
}
