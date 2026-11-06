using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000A8 RID: 168
	[Serializable]
	public sealed class NeutralRangeReductionModeParameter : VolumeParameter<NeutralRangeReductionMode>
	{
		// Token: 0x06000590 RID: 1424 RVA: 0x00020252 File Offset: 0x0001E452
		public NeutralRangeReductionModeParameter(NeutralRangeReductionMode value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
