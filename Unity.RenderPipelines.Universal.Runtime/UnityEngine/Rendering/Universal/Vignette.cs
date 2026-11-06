using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000AA RID: 170
	[VolumeComponentMenuForRenderPipeline("Post-processing/Vignette", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class Vignette : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000592 RID: 1426 RVA: 0x00020266 File Offset: 0x0001E466
		public bool IsActive()
		{
			return this.intensity.value > 0f;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0002027A File Offset: 0x0001E47A
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x04000440 RID: 1088
		[Tooltip("Vignette color.")]
		public ColorParameter color = new ColorParameter(Color.black, false, false, true, false);

		// Token: 0x04000441 RID: 1089
		[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
		public Vector2Parameter center = new Vector2Parameter(new Vector2(0.5f, 0.5f), false);

		// Token: 0x04000442 RID: 1090
		[Tooltip("Amount of vignetting on screen.")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f, false);

		// Token: 0x04000443 RID: 1091
		[Tooltip("Smoothness of the vignette borders.")]
		public ClampedFloatParameter smoothness = new ClampedFloatParameter(0.2f, 0.01f, 1f, false);

		// Token: 0x04000444 RID: 1092
		[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
		public BoolParameter rounded = new BoolParameter(false, false);
	}
}
