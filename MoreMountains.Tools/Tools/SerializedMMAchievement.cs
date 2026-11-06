using System;

namespace MoreMountains.Tools
{
	// Token: 0x02000174 RID: 372
	[Serializable]
	public class SerializedMMAchievement
	{
		// Token: 0x060008BF RID: 2239 RVA: 0x0002900F File Offset: 0x0002720F
		public SerializedMMAchievement(string achievementID, bool unlockedStatus, int progressCurrent)
		{
			this.AchievementID = achievementID;
			this.UnlockedStatus = unlockedStatus;
			this.ProgressCurrent = progressCurrent;
		}

		// Token: 0x04000A5F RID: 2655
		public string AchievementID;

		// Token: 0x04000A60 RID: 2656
		public bool UnlockedStatus;

		// Token: 0x04000A61 RID: 2657
		public int ProgressCurrent;
	}
}
