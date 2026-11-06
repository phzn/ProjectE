using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000A2 RID: 162
	[VolumeComponentMenuForRenderPipeline("Post-processing/Split Toning", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class SplitToning : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000589 RID: 1417 RVA: 0x000200F2 File Offset: 0x0001E2F2
		public bool IsActive()
		{
			return this.shadows != Color.grey || this.highlights != Color.grey;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00020118 File Offset: 0x0001E318
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x04000429 RID: 1065
		[Tooltip("The color to use for shadows.")]
		public ColorParameter shadows = new ColorParameter(Color.grey, false, false, true, false);

		// Token: 0x0400042A RID: 1066
		[Tooltip("The color to use for highlights.")]
		public ColorParameter highlights = new ColorParameter(Color.grey, false, false, true, false);

		// Token: 0x0400042B RID: 1067
		[Tooltip("Balance between the colors in the highlights and shadows.")]
		public ClampedFloatParameter balance = new ClampedFloatParameter(0f, -100f, 100f, false);
	}
}
