using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000A0 RID: 160
	[VolumeComponentMenuForRenderPipeline("Post-processing/Panini Projection", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class PaniniProjection : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x0001FF6B File Offset: 0x0001E16B
		public bool IsActive()
		{
			return this.distance.value > 0f;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001FF7F File Offset: 0x0001E17F
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x04000420 RID: 1056
		[Tooltip("Panini projection distance.")]
		public ClampedFloatParameter distance = new ClampedFloatParameter(0f, 0f, 1f, false);

		// Token: 0x04000421 RID: 1057
		[Tooltip("Panini projection crop to fit.")]
		public ClampedFloatParameter cropToFit = new ClampedFloatParameter(1f, 0f, 1f, false);
	}
}
