using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000A9 RID: 169
	[Serializable]
	public sealed class HDRACESPresetParameter : VolumeParameter<HDRACESPreset>
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x0002025C File Offset: 0x0001E45C
		public HDRACESPresetParameter(HDRACESPreset value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
