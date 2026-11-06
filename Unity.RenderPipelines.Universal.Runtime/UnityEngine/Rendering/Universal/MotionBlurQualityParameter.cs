using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200009F RID: 159
	[Serializable]
	public sealed class MotionBlurQualityParameter : VolumeParameter<MotionBlurQuality>
	{
		// Token: 0x06000582 RID: 1410 RVA: 0x0001FF61 File Offset: 0x0001E161
		public MotionBlurQualityParameter(MotionBlurQuality value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
