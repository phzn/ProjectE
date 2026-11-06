using System;

namespace Features.RunModes
{
	// Token: 0x02000D78 RID: 3448
	[Serializable]
	public sealed class LongWayHomeRunSettings
	{
		// Token: 0x06006563 RID: 25955 RVA: 0x0028716F File Offset: 0x0028536F
		public LongWayHomeRunSettings Clone()
		{
			return new LongWayHomeRunSettings
			{
				startingLootEnabled = this.startingLootEnabled,
				startAtNight = this.startAtNight,
				prologue = this.prologue
			};
		}

		// Token: 0x040067F0 RID: 26608
		public bool startingLootEnabled = true;

		// Token: 0x040067F1 RID: 26609
		public bool startAtNight = true;

		// Token: 0x040067F2 RID: 26610
		public LongWayHomePrologue prologue;
	}
}
