using System;
using System.Collections.Generic;
using Features.Localization;
using Features.Rank;
using Features.RunModes;

namespace Features.Missions
{
	// Token: 0x02000F7C RID: 3964
	public static class AccountMissionPresentation
	{
		// Token: 0x06007770 RID: 30576 RVA: 0x00309EE4 File Offset: 0x003080E4
		public static List<MissionState> Catalog()
		{
			List<MissionState> list = new List<MissionState>(MissionService.AccountMissions);
			list.RemoveAll((MissionState m) => m == null);
			list.Sort(delegate(MissionState a, MissionState b)
			{
				int num = a.category.CompareTo(b.category);
				if (num != 0)
				{
					return num;
				}
				int num2 = a.kind.CompareTo(b.kind);
				if (num2 == 0)
				{
					return a.target.CompareTo(b.target);
				}
				return num2;
			});
			return list;
		}

		// Token: 0x06007771 RID: 30577 RVA: 0x00309F46 File Offset: 0x00308146
		public static bool IsConcealed(MissionState mission)
		{
			return mission != null && mission.secret && !mission.IsComplete;
		}

		// Token: 0x06007772 RID: 30578 RVA: 0x00309F60 File Offset: 0x00308160
		public static string SecretTitle(MissionState mission)
		{
			string text = ((mission != null) ? mission.description : null) ?? string.Empty;
			int num = text.IndexOf(':');
			if (num <= 0)
			{
				return text;
			}
			return text.Substring(0, num);
		}

		// Token: 0x06007773 RID: 30579 RVA: 0x00309F9C File Offset: 0x0030819C
		public static string Availability(MissionState mission)
		{
			if (AccountMissionPresentation.IsConcealed(mission))
			{
				return "???";
			}
			if (mission.IsComplete)
			{
				return MissionLocalization.Text("Completed");
			}
			if (MissionRankPolicy.IsAvailable(mission.kind))
			{
				return MissionLocalization.Text("Available");
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			List<RankDefinitionSO> list = ((config != null) ? config.ranksInOrder : null);
			if (list != null)
			{
				foreach (RankDefinitionSO rankDefinitionSO in list)
				{
					if (rankDefinitionSO != null && MissionRankPolicy.IsAvailable(mission.kind, rankDefinitionSO, RunModeService.CurrentMode, false, RunModeService.DetectionEnabled, !RunModeService.UnlimitedClothingDurability, RunModeService.EncountersEnabled))
					{
						return MissionLocalization.Format("Requires {0}", new object[] { GameLocalization.GetSource("rank", rankDefinitionSO.displayName) });
					}
				}
			}
			return MissionLocalization.Text("Unavailable in this mode");
		}
	}
}
