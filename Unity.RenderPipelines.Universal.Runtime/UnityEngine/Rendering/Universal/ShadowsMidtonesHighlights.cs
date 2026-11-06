using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000A1 RID: 161
	[VolumeComponentMenuForRenderPipeline("Post-processing/Shadows, Midtones, Highlights", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class ShadowsMidtonesHighlights : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000586 RID: 1414 RVA: 0x0001FFC0 File Offset: 0x0001E1C0
		public bool IsActive()
		{
			Vector4 vector = new Vector4(1f, 1f, 1f, 0f);
			return this.shadows != vector || this.midtones != vector || this.highlights != vector;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00020012 File Offset: 0x0001E212
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x04000422 RID: 1058
		[Tooltip("Use this to control and apply a hue to the shadows.")]
		public Vector4Parameter shadows = new Vector4Parameter(new Vector4(1f, 1f, 1f, 0f), false);

		// Token: 0x04000423 RID: 1059
		[Tooltip("Use this to control and apply a hue to the midtones.")]
		public Vector4Parameter midtones = new Vector4Parameter(new Vector4(1f, 1f, 1f, 0f), false);

		// Token: 0x04000424 RID: 1060
		[Tooltip("Use this to control and apply a hue to the highlights.")]
		public Vector4Parameter highlights = new Vector4Parameter(new Vector4(1f, 1f, 1f, 0f), false);

		// Token: 0x04000425 RID: 1061
		[Header("Shadow Limits")]
		[Tooltip("Start point of the transition between shadows and midtones.")]
		public MinFloatParameter shadowsStart = new MinFloatParameter(0f, 0f, false);

		// Token: 0x04000426 RID: 1062
		[Tooltip("End point of the transition between shadows and midtones.")]
		public MinFloatParameter shadowsEnd = new MinFloatParameter(0.3f, 0f, false);

		// Token: 0x04000427 RID: 1063
		[Header("Highlight Limits")]
		[Tooltip("Start point of the transition between midtones and highlights.")]
		public MinFloatParameter highlightsStart = new MinFloatParameter(0.55f, 0f, false);

		// Token: 0x04000428 RID: 1064
		[Tooltip("End point of the transition between midtones and highlights.")]
		public MinFloatParameter highlightsEnd = new MinFloatParameter(1f, 0f, false);
	}
}
