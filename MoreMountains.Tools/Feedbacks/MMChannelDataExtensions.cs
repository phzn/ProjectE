using System;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001AA RID: 426
	public static class MMChannelDataExtensions
	{
		// Token: 0x06000A4E RID: 2638 RVA: 0x0002F2E1 File Offset: 0x0002D4E1
		public static MMChannelData Set(this MMChannelData data, MMChannelModes mode, int channel, MMChannel channelDefinition)
		{
			data.MMChannelMode = mode;
			data.Channel = channel;
			data.MMChannelDefinition = channelDefinition;
			return data;
		}
	}
}
