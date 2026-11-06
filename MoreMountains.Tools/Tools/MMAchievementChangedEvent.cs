using System;

namespace MoreMountains.Tools
{
	// Token: 0x02000170 RID: 368
	public struct MMAchievementChangedEvent
	{
		// Token: 0x060008A3 RID: 2211 RVA: 0x00028A98 File Offset: 0x00026C98
		public MMAchievementChangedEvent(MMAchievement newAchievement)
		{
			this.Achievement = newAchievement;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00028AA1 File Offset: 0x00026CA1
		public static void Trigger(MMAchievement newAchievement)
		{
			MMAchievementChangedEvent.e.Achievement = newAchievement;
			MMEventManager.TriggerEvent<MMAchievementChangedEvent>(MMAchievementChangedEvent.e);
		}

		// Token: 0x04000A51 RID: 2641
		public MMAchievement Achievement;

		// Token: 0x04000A52 RID: 2642
		private static MMAchievementChangedEvent e;
	}
}
