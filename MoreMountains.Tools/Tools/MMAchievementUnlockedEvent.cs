using System;

namespace MoreMountains.Tools
{
	// Token: 0x0200016F RID: 367
	public struct MMAchievementUnlockedEvent
	{
		// Token: 0x060008A1 RID: 2209 RVA: 0x00028A78 File Offset: 0x00026C78
		public MMAchievementUnlockedEvent(MMAchievement newAchievement)
		{
			this.Achievement = newAchievement;
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00028A81 File Offset: 0x00026C81
		public static void Trigger(MMAchievement newAchievement)
		{
			MMAchievementUnlockedEvent.e.Achievement = newAchievement;
			MMEventManager.TriggerEvent<MMAchievementUnlockedEvent>(MMAchievementUnlockedEvent.e);
		}

		// Token: 0x04000A4F RID: 2639
		public MMAchievement Achievement;

		// Token: 0x04000A50 RID: 2640
		private static MMAchievementUnlockedEvent e;
	}
}
