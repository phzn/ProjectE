using System;
using System.Globalization;
using Features.MatchRecap;
using UnityEngine;

namespace Features.Missions
{
	// Token: 0x02000FB8 RID: 4024
	public static class RunMissionPresentation
	{
		// Token: 0x0600797B RID: 31099 RVA: 0x00319998 File Offset: 0x00317B98
		public static string CompactDescription(MissionState mission)
		{
			return MissionLocalization.Compact(mission);
		}

		// Token: 0x0600797C RID: 31100 RVA: 0x003199A0 File Offset: 0x00317BA0
		internal static string CompactEnglish(MissionState mission)
		{
			if (mission == null)
			{
				return string.Empty;
			}
			string text = (MissionService.IsStationEmote(mission) ? "Perform {0}" : MissionLocalizationTemplates.Hud(mission.kind));
			if (text != null)
			{
				return string.Format(CultureInfo.CurrentCulture, text, mission.qualifierName ?? string.Empty, mission.secondaryQualifierName ?? string.Empty);
			}
			return mission.description ?? string.Empty;
		}

		// Token: 0x0600797D RID: 31101 RVA: 0x00319A10 File Offset: 0x00317C10
		public static string FormatProgress(MissionState mission)
		{
			if (mission == null)
			{
				return string.Empty;
			}
			float num;
			float num2;
			if (RunMissionPresentation.TryLiveHomeDistance(mission, out num, out num2))
			{
				return Mathf.FloorToInt(num).ToString() + " / " + Mathf.RoundToInt(num2).ToString() + " m";
			}
			string text = (MissionRankPolicy.IsMeters(mission.kind) ? " m" : (MissionRankPolicy.IsSeconds(mission.kind) ? " s" : string.Empty));
			return Mathf.FloorToInt(Mathf.Clamp(mission.progress, 0f, mission.target)).ToString() + " / " + Mathf.RoundToInt(mission.target).ToString() + text;
		}

		// Token: 0x0600797E RID: 31102 RVA: 0x00319AD0 File Offset: 0x00317CD0
		public static float DisplayProgress01(MissionState mission)
		{
			if (mission == null)
			{
				return 0f;
			}
			float num;
			float num2;
			if (!RunMissionPresentation.TryLiveHomeDistance(mission, out num, out num2))
			{
				return mission.NormalizedProgress;
			}
			if (num2 > 0f)
			{
				return Mathf.Min(0.99f, num / num2);
			}
			return 0f;
		}

		// Token: 0x0600797F RID: 31103 RVA: 0x00319B14 File Offset: 0x00317D14
		private static bool TryLiveHomeDistance(MissionState mission, out float reached, out float required)
		{
			reached = 0f;
			required = 0f;
			if (mission.IsComplete || mission.scope != MissionScope.Run || !MissionService.IsHomeReturnObjective(mission.kind))
			{
				return false;
			}
			MatchStatsService instance = MatchStatsService.Instance;
			if (instance == null || !instance.IsRunTrackingActive)
			{
				return false;
			}
			required = ((mission.kind == MissionObjectiveKind.SafeHomeReturnDistance) ? mission.target : 100f);
			reached = Mathf.Clamp(instance.PeakDistanceMeters, 0f, required);
			return true;
		}

		// Token: 0x04007CC4 RID: 31940
		internal const string StationEmoteHud = "Perform {0}";
	}
}
