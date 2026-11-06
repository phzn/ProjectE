using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200009A RID: 154
	[VolumeComponentMenuForRenderPipeline("Post-processing/Lift, Gamma, Gain", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class LiftGammaGain : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x0600057B RID: 1403 RVA: 0x0001FDF0 File Offset: 0x0001DFF0
		public bool IsActive()
		{
			Vector4 vector = new Vector4(1f, 1f, 1f, 0f);
			return this.lift != vector || this.gamma != vector || this.gain != vector;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001FE42 File Offset: 0x0001E042
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x04000412 RID: 1042
		[Tooltip("Use this to control and apply a hue to the dark tones. This has a more exaggerated effect on shadows.")]
		public Vector4Parameter lift = new Vector4Parameter(new Vector4(1f, 1f, 1f, 0f), false);

		// Token: 0x04000413 RID: 1043
		[Tooltip("Use this to control and apply a hue to the mid-range tones with a power function.")]
		public Vector4Parameter gamma = new Vector4Parameter(new Vector4(1f, 1f, 1f, 0f), false);

		// Token: 0x04000414 RID: 1044
		[Tooltip("Use this to increase and apply a hue to the signal and make highlights brighter.")]
		public Vector4Parameter gain = new Vector4Parameter(new Vector4(1f, 1f, 1f, 0f), false);
	}
}
