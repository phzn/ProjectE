using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000171 RID: 369
	[CreateAssetMenu(fileName = "AchievementList", menuName = "MoreMountains/Achievement List")]
	public class MMAchievementList : ScriptableObject
	{
		// Token: 0x060008A5 RID: 2213 RVA: 0x00028AB8 File Offset: 0x00026CB8
		public virtual void ResetAchievements()
		{
			Debug.LogFormat("Reset Achievements", Array.Empty<object>());
			MMAchievementManager.ResetAchievements(this.AchievementsListID);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00028AD4 File Offset: 0x00026CD4
		protected virtual void OnEnable()
		{
			this._instances.Reference(this, false);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00028AE3 File Offset: 0x00026CE3
		protected virtual void OnDisable()
		{
			this._instances.Dispose();
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00028AF0 File Offset: 0x00026CF0
		public static MMAchievementList Any
		{
			get
			{
				return MMReferenceHolder<MMAchievementList>.Any;
			}
		}

		// Token: 0x04000A53 RID: 2643
		public string AchievementsListID = "AchievementsList";

		// Token: 0x04000A54 RID: 2644
		public List<MMAchievement> Achievements;

		// Token: 0x04000A55 RID: 2645
		private MMReferenceHolder<MMAchievementList> _instances;
	}
}
