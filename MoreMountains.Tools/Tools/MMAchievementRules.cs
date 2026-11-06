using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000173 RID: 371
	public abstract class MMAchievementRules : MonoBehaviour, MMEventListener<MMGameEvent>, MMEventListenerBase
	{
		// Token: 0x060008B9 RID: 2233 RVA: 0x00028EF0 File Offset: 0x000270F0
		public virtual void PrintCurrentStatus()
		{
			foreach (MMAchievement mmachievement in MMAchievementManager.AchievementsList)
			{
				string text = (mmachievement.UnlockedStatus ? "unlocked" : "locked");
				MMDebug.DebugLogInfo(string.Concat(new string[]
				{
					"[",
					mmachievement.AchievementID,
					"] ",
					mmachievement.Title,
					", status : ",
					text,
					", progress : ",
					mmachievement.ProgressCurrent.ToString(),
					"/",
					mmachievement.ProgressTarget.ToString()
				}), "", 3, true);
			}
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00028FCC File Offset: 0x000271CC
		protected virtual void Awake()
		{
			MMAchievementManager.LoadAchievementList(this.AchievementList);
			MMAchievementManager.LoadSavedAchievements();
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00028FDE File Offset: 0x000271DE
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMGameEvent>();
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00028FE6 File Offset: 0x000271E6
		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMGameEvent>();
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00028FEE File Offset: 0x000271EE
		public virtual void OnMMEvent(MMGameEvent gameEvent)
		{
			if (gameEvent.EventName == "Save")
			{
				MMAchievementManager.SaveAchievements();
			}
		}

		// Token: 0x04000A5D RID: 2653
		public MMAchievementList AchievementList;

		// Token: 0x04000A5E RID: 2654
		[MMInspectorButton("PrintCurrentStatus")]
		public bool PrintCurrentStatusBtn;
	}
}
