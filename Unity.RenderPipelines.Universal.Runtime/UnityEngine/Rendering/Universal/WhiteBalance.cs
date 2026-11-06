using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000AB RID: 171
	[VolumeComponentMenuForRenderPipeline("Post-processing/White Balance", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class WhiteBalance : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000595 RID: 1429 RVA: 0x00020305 File Offset: 0x0001E505
		public bool IsActive()
		{
			return this.temperature.value != 0f || this.tint.value != 0f;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00020330 File Offset: 0x0001E530
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x04000445 RID: 1093
		[Tooltip("Sets the white balance to a custom color temperature.")]
		public ClampedFloatParameter temperature = new ClampedFloatParameter(0f, -100f, 100f, false);

		// Token: 0x04000446 RID: 1094
		[Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
		public ClampedFloatParameter tint = new ClampedFloatParameter(0f, -100f, 100f, false);
	}
}
