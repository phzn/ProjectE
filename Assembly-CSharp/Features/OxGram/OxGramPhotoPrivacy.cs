using System;
using Features.MatchRecap;
using Features.Rank;
using Features.Score;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.OxGram
{
	// Token: 0x02000EAD RID: 3757
	public static class OxGramPhotoPrivacy
	{
		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x060070BF RID: 28863 RVA: 0x002DC29D File Offset: 0x002DA49D
		public static bool CanSendUncensored
		{
			get
			{
				RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
				return OxGramPhotoPrivacy.CanSendUncensoredForRank((currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive);
			}
		}

		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x060070C0 RID: 28864 RVA: 0x002DC2BA File Offset: 0x002DA4BA
		public static bool RewardsPhotos
		{
			get
			{
				return !OxGramPhotoPrivacy.CanSendUncensored;
			}
		}

		// Token: 0x060070C1 RID: 28865 RVA: 0x002DC2C4 File Offset: 0x002DA4C4
		public static bool CanSendUncensoredForRank(string rankId, bool admin = false)
		{
			return admin || string.Equals(rankId, "hornyslut_declared", StringComparison.Ordinal) || string.Equals(rankId, "admin", StringComparison.Ordinal);
		}

		// Token: 0x060070C2 RID: 28866 RVA: 0x002DC2E5 File Offset: 0x002DA4E5
		public static bool RewardsPhotosForRank(string rankId, bool admin = false)
		{
			return !OxGramPhotoPrivacy.CanSendUncensoredForRank(rankId, admin);
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x060070C3 RID: 28867 RVA: 0x002DC2F1 File Offset: 0x002DA4F1
		public static bool CanLeakInCurrentScene
		{
			get
			{
				return OxGramPhotoPrivacy.IsInActiveRun && !OxGramPhotoPrivacy.CanSendUncensored;
			}
		}

		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x060070C4 RID: 28868 RVA: 0x002DC304 File Offset: 0x002DA504
		public static bool IsInActiveRun
		{
			get
			{
				SceneContext sceneContext = SceneContext.Current;
				return OxGramPhotoPrivacy.IsRunContext(SceneManager.GetActiveScene().name, (sceneContext != null) ? sceneContext.gameObject.scene.name : null, GameSystemsBootstrapper.IsRunReady(false), (sceneContext != null && sceneContext.Profile != null) ? new bool?(sceneContext.Profile.participatesInRun) : null);
			}
		}

		// Token: 0x060070C5 RID: 28869 RVA: 0x002DC380 File Offset: 0x002DA580
		public static bool AwardPhotoPoints()
		{
			if (!OxGramPhotoPrivacy.IsInActiveRun || !OxGramPhotoPrivacy.RewardsPhotos)
			{
				return false;
			}
			MatchStatsService instance = MatchStatsService.Instance;
			RunScoreAward runScoreAward;
			return instance != null && instance.AwardPoints(new RunScoreAwardRequest(RunScoreReason.CielGramPhoto, 5, "CIELGRAM", "PHOTO SENT", null, RunScorePresentation.Silent), out runScoreAward);
		}

		// Token: 0x060070C6 RID: 28870 RVA: 0x002DC3CC File Offset: 0x002DA5CC
		public static bool IsRunContext(string activeSceneName, string contextSceneName, bool isRunReady, bool? participatesInRun)
		{
			if (isRunReady)
			{
				bool? flag = participatesInRun;
				bool flag2 = false;
				if (!((flag.GetValueOrDefault() == flag2) & (flag != null)) && !string.IsNullOrEmpty(activeSceneName) && !OxGramPhotoPrivacy.IsOutsideRunScene(activeSceneName))
				{
					return !OxGramPhotoPrivacy.IsOutsideRunScene(contextSceneName);
				}
			}
			return false;
		}

		// Token: 0x060070C7 RID: 28871 RVA: 0x002DC410 File Offset: 0x002DA610
		private static bool IsOutsideRunScene(string sceneName)
		{
			return string.Equals(sceneName, "CielHome", StringComparison.OrdinalIgnoreCase) || string.Equals(sceneName, "_Core", StringComparison.OrdinalIgnoreCase) || string.Equals(sceneName, "MainMenu", StringComparison.OrdinalIgnoreCase) || string.Equals(sceneName, "Prison", StringComparison.OrdinalIgnoreCase) || string.Equals(sceneName, "Customization", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060070C8 RID: 28872 RVA: 0x002DC463 File Offset: 0x002DA663
		public static bool ShouldLeak(bool inRun, bool showsFace, bool hasCensored, float chancePercent, float rollPercent)
		{
			return inRun && showsFace && hasCensored && rollPercent >= 0f && rollPercent < Mathf.Clamp(chancePercent, 0f, 100f);
		}

		// Token: 0x04007387 RID: 29575
		public const int PhotoRewardPoints = 5;
	}
}
