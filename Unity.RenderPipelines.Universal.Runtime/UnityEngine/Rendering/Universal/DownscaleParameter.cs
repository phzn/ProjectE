using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200008D RID: 141
	[Serializable]
	public sealed class DownscaleParameter : VolumeParameter<BloomDownscaleMode>
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x0001F471 File Offset: 0x0001D671
		public DownscaleParameter(BloomDownscaleMode value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
