using System;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001A9 RID: 425
	[Serializable]
	public class MMChannelData
	{
		// Token: 0x06000A4D RID: 2637 RVA: 0x0002F2C4 File Offset: 0x0002D4C4
		public MMChannelData(MMChannelModes mode, int channel, MMChannel channelDefinition)
		{
			this.MMChannelMode = mode;
			this.Channel = channel;
			this.MMChannelDefinition = channelDefinition;
		}

		// Token: 0x04000C6B RID: 3179
		public MMChannelModes MMChannelMode;

		// Token: 0x04000C6C RID: 3180
		public int Channel;

		// Token: 0x04000C6D RID: 3181
		public MMChannel MMChannelDefinition;
	}
}
