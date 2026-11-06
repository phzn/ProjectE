using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200009E RID: 158
	[Serializable]
	public sealed class MotionBlurModeParameter : VolumeParameter<MotionBlurMode>
	{
		// Token: 0x06000581 RID: 1409 RVA: 0x0001FF57 File Offset: 0x0001E157
		public MotionBlurModeParameter(MotionBlurMode value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
