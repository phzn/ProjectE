using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000095 RID: 149
	[Serializable]
	public sealed class DepthOfFieldModeParameter : VolumeParameter<DepthOfFieldMode>
	{
		// Token: 0x06000573 RID: 1395 RVA: 0x0001FC4E File Offset: 0x0001DE4E
		public DepthOfFieldModeParameter(DepthOfFieldMode value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
