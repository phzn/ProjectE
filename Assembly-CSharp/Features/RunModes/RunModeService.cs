using System;
using Features.Inventory;
using Features.NPC;
using Features.Rank;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Minimap;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000D66 RID: 3430
	public static class RunModeService
	{
		// Token: 0x060064A6 RID: 25766 RVA: 0x00283CFC File Offset: 0x00281EFC
		internal static bool ValidateRankedResumeEnvironment()
		{
			RunModeSession session = RunModeService._session;
			if (!RankedRunIntegrity.IsRanked(session) || session.leaderboardDisqualified || (session.rankedBuildIdentity != "PE_STEAM_BUILD.release.v1" && session.rankedBuildIdentity != "PE_STEAM_EDITOR_TEST.v1" && session.rankedBuildIdentity != "PE_STEAM_LOCAL_QA.v1") || !RankedRunIntegrity.VerifySession(session) || !string.IsNullOrEmpty(RankedRunIntegrity.GetExclusion(session)))
			{
				return false;
			}
			string text;
			if (!(session.rankedBuildIdentity != RankedPlatformPolicy.CurrentSubmissionIdentity))
			{
				if (!string.IsNullOrEmpty(session.startingRankId))
				{
					string startingRankId = session.startingRankId;
					RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
					if (string.Equals(startingRankId, (currentRank != null) ? currentRank.rankId : null, StringComparison.Ordinal))
					{
						text = string.Empty;
						goto IL_00B4;
					}
				}
				text = "The profile rank changed since this run started. Start a new run to compete.";
			}
			else
			{
				text = "This run belongs to a different score submission context. Start a new Classic Ranked run.";
			}
			IL_00B4:
			string text2 = text;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = RankedResumeEnvironment.GetExclusionReason(RankedPlatformPolicy.CurrentBuildKind, RankedLeaderboardService.CanSubmitScores, RankedLeaderboardService.IsSupported, RankedLeaderboardService.SubmissionDisabledReason, RankedPlatformPolicy.EditorTestUploadsEnabled);
			}
			if (string.IsNullOrEmpty(text2))
			{
				return false;
			}
			RankedRunIntegrity.Disqualify(session, text2);
			session.leaderboardDisqualified = true;
			session.leaderboardExclusionReason = text2;
			RankedRunIntegrity.SealSession(session);
			return true;
		}

		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x060064A7 RID: 25767 RVA: 0x00283E0C File Offset: 0x0028200C
		private static string CurrentSessionKey
		{
			get
			{
				return "RunModes.CurrentSession.v1";
			}
		}

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x060064A8 RID: 25768 RVA: 0x00283E14 File Offset: 0x00282014
		// (remove) Token: 0x060064A9 RID: 25769 RVA: 0x00283E48 File Offset: 0x00282048
		public static event Action Changed;

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x060064AA RID: 25770 RVA: 0x00283E7C File Offset: 0x0028207C
		// (remove) Token: 0x060064AB RID: 25771 RVA: 0x00283EB0 File Offset: 0x002820B0
		public static event Action<string> LeaderboardRunDisqualified;

		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x060064AC RID: 25772 RVA: 0x00283EE4 File Offset: 0x002820E4
		public static bool ModesUnlocked
		{
			get
			{
				if (RankProgressionService.IsAdminRankActive)
				{
					return true;
				}
				RankProgressionConfigSO config = RankProgressionService.Config;
				int num = ((config != null && config.ranksInOrder != null) ? Mathf.Max(0, config.ranksInOrder.Count - 1) : 5);
				return RankProgressionService.RankIndex >= num;
			}
		}

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x060064AD RID: 25773 RVA: 0x00283F33 File Offset: 0x00282133
		public static RunModeKind CurrentMode
		{
			get
			{
				if (!RunModeService.Session.prepared)
				{
					return RunModeKind.Classic;
				}
				return RunModeService.CanonicalMode(RunModeService.Session.mode);
			}
		}

		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x060064AE RID: 25774 RVA: 0x00283F52 File Offset: 0x00282152
		public static bool LongWayHomeUnlocked
		{
			get
			{
				return RankProgressionService.IsAdminRankActive || RunModeService.HasReachedLongWayHomeRank(RankProgressionService.RankIndex, RankProgressionService.Config);
			}
		}

		// Token: 0x060064AF RID: 25775 RVA: 0x00283F6C File Offset: 0x0028216C
		public static bool HasReachedLongWayHomeRank(int rankIndex, RankProgressionConfigSO config)
		{
			if (config == null || config.ranksInOrder == null)
			{
				return false;
			}
			for (int i = 0; i < config.ranksInOrder.Count; i++)
			{
				RankDefinitionSO rankDefinitionSO = config.ranksInOrder[i];
				if (rankDefinitionSO != null && string.Equals(rankDefinitionSO.rankId, "hornyslut_declared", StringComparison.Ordinal))
				{
					return rankIndex >= i;
				}
			}
			return false;
		}

		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x060064B0 RID: 25776 RVA: 0x00283FD4 File Offset: 0x002821D4
		public static bool IsFreeMode
		{
			get
			{
				return RunModeService.HasPreparedRun && RunModeService.CurrentMode == RunModeKind.Free;
			}
		}

		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x060064B1 RID: 25777 RVA: 0x00283FE7 File Offset: 0x002821E7
		public static bool IsLongWayHome
		{
			get
			{
				return RunModeService.HasPreparedRun && RunModeService.CurrentMode == RunModeKind.LongWayHome;
			}
		}

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x060064B2 RID: 25778 RVA: 0x00283FFA File Offset: 0x002821FA
		public static ClassicRunKind CurrentClassicRunKind
		{
			get
			{
				if (RunModeService.CurrentMode != RunModeKind.Classic)
				{
					return ClassicRunKind.Normal;
				}
				return RunModeService.Session.classicRunKind;
			}
		}

		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x060064B3 RID: 25779 RVA: 0x00284010 File Offset: 0x00282210
		public static bool IsRankedRun
		{
			get
			{
				return RunModeService.HasPreparedRun && RunModeService.CurrentMode == RunModeKind.Classic && RunModeService.CurrentClassicRunKind == ClassicRunKind.Ranked;
			}
		}

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x060064B4 RID: 25780 RVA: 0x0028402B File Offset: 0x0028222B
		public static string PreparedRankedAccountId
		{
			get
			{
				if (!RunModeService.IsRankedRun)
				{
					return null;
				}
				return RunModeService.Session.rankedAccountId;
			}
		}

		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x060064B5 RID: 25781 RVA: 0x00284040 File Offset: 0x00282240
		public static bool IsLeaderboardEligibleRun
		{
			get
			{
				return RunModeService.IsRankedRun && string.IsNullOrEmpty(RunModeService.LeaderboardIneligibilityReason);
			}
		}

		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x060064B6 RID: 25782 RVA: 0x00284058 File Offset: 0x00282258
		public static string LeaderboardIneligibilityReason
		{
			get
			{
				if (!RunModeService.IsRankedRun)
				{
					return "Only Classic Ranked runs can submit scores.";
				}
				if (RunModeService.Session.classicTutorial)
				{
					return "Tutorial runs do not submit scores.";
				}
				if (RunModeService.Session.startedAsAdmin || RankProgressionService.IsAdminRankActive)
				{
					return "Admin runs do not submit scores.";
				}
				if (RunModeService.Session.leaderboardDisqualified)
				{
					if (!string.IsNullOrEmpty(RunModeService.Session.leaderboardExclusionReason))
					{
						return RunModeService.Session.leaderboardExclusionReason;
					}
					return "This run was modified and cannot submit a score.";
				}
				else
				{
					if (RunModeService.Session.rankedBuildIdentity != RankedPlatformPolicy.CurrentSubmissionIdentity)
					{
						return "This run began in a preview or unsupported build. Start a new run in the Steam release to compete.";
					}
					if (string.IsNullOrEmpty(RunModeService.Session.rankedAccountId))
					{
						return "This run has no verified Steam account.";
					}
					if (!string.Equals(RunModeService.Session.rankedAccountId, RankedLeaderboardService.CurrentAccountId, StringComparison.Ordinal))
					{
						return "Sign in with the Steam account that started this run.";
					}
					return RankedRunIntegrity.GetExclusion(RunModeService.Session);
				}
			}
		}

		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x060064B7 RID: 25783 RVA: 0x00284125 File Offset: 0x00282325
		public static FreeRunSettings CurrentSettings
		{
			get
			{
				return RunModeService.Session.settings ?? new FreeRunSettings();
			}
		}

		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x060064B8 RID: 25784 RVA: 0x0028413A File Offset: 0x0028233A
		public static ClassicRunSettings CurrentClassicSettings
		{
			get
			{
				return (RunModeService.Session.classicSettings ?? new ClassicRunSettings()).Clone();
			}
		}

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x060064B9 RID: 25785 RVA: 0x00284154 File Offset: 0x00282354
		public static bool ClassicStartTimeUnlocked
		{
			get
			{
				return RunModeService.ModesUnlocked;
			}
		}

		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x060064BA RID: 25786 RVA: 0x0028415B File Offset: 0x0028235B
		public static int ClassicStartHour
		{
			get
			{
				if (!RunModeService.HasPreparedRun || RunModeService.CurrentMode != RunModeKind.Classic || !RunModeService.ClassicStartTimeUnlocked || RunModeService.Session.classicTutorial || !RunModeService.CurrentClassicSettings.startAtNight)
				{
					return 12;
				}
				return 22;
			}
		}

		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x060064BB RID: 25787 RVA: 0x00284190 File Offset: 0x00282390
		public static LongWayHomeRunSettings CurrentLongWayHomeSettings
		{
			get
			{
				return (RunModeService.Session.longWayHomeSettings ?? new LongWayHomeRunSettings()).Clone();
			}
		}

		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x060064BC RID: 25788 RVA: 0x002841AA File Offset: 0x002823AA
		public static bool LongWayHomeStartingLootEnabled
		{
			get
			{
				return !RunModeService.IsPoiPrologue(RunModeService.CurrentLongWayHomePrologue) && RunModeService.CurrentLongWayHomeSettings.startingLootEnabled;
			}
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x060064BD RID: 25789 RVA: 0x002841C4 File Offset: 0x002823C4
		public static int LongWayHomeStartHour
		{
			get
			{
				if (RunModeService.CurrentLongWayHomePrologue == LongWayHomePrologue.Stadium)
				{
					return 12;
				}
				if (RunModeService.CurrentLongWayHomePrologue != LongWayHomePrologue.University && !RunModeService.CurrentLongWayHomeSettings.startAtNight)
				{
					return 12;
				}
				return 22;
			}
		}

		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x060064BE RID: 25790 RVA: 0x002841EA File Offset: 0x002823EA
		public static LongWayHomePrologue CurrentLongWayHomePrologue
		{
			get
			{
				return RunModeService.CurrentLongWayHomeSettings.prologue;
			}
		}

		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x060064BF RID: 25791 RVA: 0x002841F6 File Offset: 0x002823F6
		public static bool IsLongWayHomeGymShower
		{
			get
			{
				return RunModeService.IsLongWayHome && RunModeService.CurrentLongWayHomePrologue == LongWayHomePrologue.GymShower;
			}
		}

		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x060064C0 RID: 25792 RVA: 0x00284209 File Offset: 0x00282409
		public static bool IsLongWayHomeUniversity
		{
			get
			{
				return RunModeService.IsLongWayHome && RunModeService.CurrentLongWayHomePrologue == LongWayHomePrologue.University;
			}
		}

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x060064C1 RID: 25793 RVA: 0x0028421C File Offset: 0x0028241C
		public static bool IsLongWayHomeStadium
		{
			get
			{
				return RunModeService.IsLongWayHome && RunModeService.CurrentLongWayHomePrologue == LongWayHomePrologue.Stadium;
			}
		}

		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x060064C2 RID: 25794 RVA: 0x0028422F File Offset: 0x0028242F
		public static bool IsLongWayHomePoiPrologue
		{
			get
			{
				return RunModeService.IsLongWayHome && RunModeService.IsPoiPrologue(RunModeService.CurrentLongWayHomePrologue);
			}
		}

		// Token: 0x060064C3 RID: 25795 RVA: 0x00284244 File Offset: 0x00282444
		public static bool IsPoiPrologue(LongWayHomePrologue prologue)
		{
			return prologue == LongWayHomePrologue.GymShower || prologue == LongWayHomePrologue.University || prologue == LongWayHomePrologue.Stadium;
		}

		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x060064C4 RID: 25796 RVA: 0x00284254 File Offset: 0x00282454
		public static bool HasPreparedRun
		{
			get
			{
				return RunModeService.Session.prepared;
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x060064C5 RID: 25797 RVA: 0x00284260 File Offset: 0x00282460
		public static bool IsMapsVisit
		{
			get
			{
				return RunModeService.HasPreparedRun && RunModeService.Session.mapsVisit;
			}
		}

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x060064C6 RID: 25798 RVA: 0x00284275 File Offset: 0x00282475
		public static bool HasSeededFreeWorld
		{
			get
			{
				return RunModeService.HasPreparedRun && RunModeService.CurrentMode == RunModeKind.Free && !RunModeService.IsMapsVisit;
			}
		}

		// Token: 0x060064C7 RID: 25799 RVA: 0x00284290 File Offset: 0x00282490
		public static bool TryGetFreeWorldSeed(out int seed)
		{
			seed = 0;
			if (!RunModeService.HasSeededFreeWorld || !RunModeService.Session.hasWorldSeed)
			{
				return false;
			}
			seed = RunModeService.Session.actualWorldSeed;
			return true;
		}

		// Token: 0x060064C8 RID: 25800 RVA: 0x002842B8 File Offset: 0x002824B8
		public static int ResolveWorldSeedForCurrentRun(int generated, int? diagnosticOverride = null)
		{
			if (RunModeService.HasSeededFreeWorld)
			{
				int num = diagnosticOverride ?? (RunModeService.Session.hasWorldSeed ? RunModeService.Session.actualWorldSeed : (RunModeService.CurrentSettings.useCustomWorldSeed ? RunModeService.CurrentSettings.worldSeed : generated));
				if (!RunModeService.Session.hasWorldSeed || RunModeService.Session.actualWorldSeed != num)
				{
					RunModeService.Session.hasWorldSeed = true;
					RunModeService.Session.actualWorldSeed = num;
					RunModeService.PersistSession();
					Action changed = RunModeService.Changed;
					if (changed != null)
					{
						changed();
					}
				}
				return num;
			}
			int num2;
			if (diagnosticOverride == null && LongWayHomeRuntimeController.TryGetPlannedCitySeed(out num2))
			{
				return num2;
			}
			int? num3 = diagnosticOverride;
			if (num3 == null)
			{
				return generated;
			}
			return num3.GetValueOrDefault();
		}

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x060064C9 RID: 25801 RVA: 0x00284380 File Offset: 0x00282580
		public static string PreparedStartingRankId
		{
			get
			{
				if (!RunModeService.HasPreparedRun)
				{
					return null;
				}
				return RunModeService.Session.startingRankId;
			}
		}

		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x060064CA RID: 25802 RVA: 0x00284395 File Offset: 0x00282595
		public static bool PreparedStartedAsAdmin
		{
			get
			{
				return RunModeService.HasPreparedRun && RunModeService.Session.startedAsAdmin;
			}
		}

		// Token: 0x060064CB RID: 25803 RVA: 0x002843AA File Offset: 0x002825AA
		public static void MarkPreparedRunAsAdmin()
		{
			if (!RunModeService.HasPreparedRun || RunModeService.Session.startedAsAdmin)
			{
				return;
			}
			RunModeService.Session.startedAsAdmin = true;
			RunModeService.PersistSession();
		}

		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x060064CC RID: 25804 RVA: 0x002843D0 File Offset: 0x002825D0
		public static NPCRosterSnapshot CurrentNpcRoster
		{
			get
			{
				if (!RunModeService.Session.prepared)
				{
					return null;
				}
				return RunModeService.Session.npcRoster;
			}
		}

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x060064CD RID: 25805 RVA: 0x002843EA File Offset: 0x002825EA
		public static string CurrentRunId
		{
			get
			{
				NPCRosterSnapshot currentSnapshot = NPCRosterSession.GetCurrentSnapshot();
				if (currentSnapshot == null)
				{
					return null;
				}
				return currentSnapshot.runId;
			}
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x060064CE RID: 25806 RVA: 0x002843FC File Offset: 0x002825FC
		internal static bool HasPendingCollarHandoff
		{
			get
			{
				return RunModeService.Session.prepared && RunModeService.Session.collarHandoffPending;
			}
		}

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x060064CF RID: 25807 RVA: 0x00284416 File Offset: 0x00282616
		internal static CollarRunHandoff PreparedCollarHandoff
		{
			get
			{
				if (!RunModeService.HasPendingCollarHandoff || !RunModeService.Session.collarHandoffWorn)
				{
					return null;
				}
				return RunModeService.Session.collarHandoff;
			}
		}

		// Token: 0x060064D0 RID: 25808 RVA: 0x00284437 File Offset: 0x00282637
		internal static void CompleteCollarHandoff()
		{
			if (!RunModeService.HasPendingCollarHandoff)
			{
				return;
			}
			RunModeService.Session.collarHandoffPending = false;
			RunModeService.Session.collarHandoffWorn = false;
			RunModeService.Session.collarHandoff = null;
			RunModeService.PersistSession();
		}

		// Token: 0x060064D1 RID: 25809 RVA: 0x00284467 File Offset: 0x00282667
		internal static void SaveMigratedNpcRoster(NPCRosterSnapshot snapshot)
		{
			if (!RunModeService.Session.prepared || RunModeService.Session.npcRoster != null || snapshot == null)
			{
				return;
			}
			RunModeService.Session.npcRoster = snapshot;
			RunModeService.PersistSession();
		}

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x060064D2 RID: 25810 RVA: 0x00284495 File Offset: 0x00282695
		public static bool IsClassicTutorialPhase
		{
			get
			{
				return RunModeService.CurrentMode == RunModeKind.Classic && !RunModeService.ModesUnlocked;
			}
		}

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x060064D3 RID: 25811 RVA: 0x002844AC File Offset: 0x002826AC
		public static bool IsAtHome
		{
			get
			{
				return string.Equals(SceneManager.GetActiveScene().name, "CielHome", StringComparison.Ordinal);
			}
		}

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x060064D4 RID: 25812 RVA: 0x002844D1 File Offset: 0x002826D1
		public static bool UsesHomePresetOutfit
		{
			get
			{
				return RunModeService.IsAtHome && (!RankWardrobePolicy.IsRestricted || RankWardrobePolicy.UsesCertifiedHomePreset);
			}
		}

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x060064D5 RID: 25813 RVA: 0x002844EC File Offset: 0x002826EC
		public static bool IsGameplayBoot
		{
			get
			{
				if (!Application.isPlaying || !RunModeService.HasPreparedRun)
				{
					return false;
				}
				Scene activeScene = SceneManager.GetActiveScene();
				SceneContext sceneContext = SceneContext.Current;
				SceneProfileSO sceneProfileSO = ((sceneContext != null && sceneContext.gameObject.scene == activeScene) ? sceneContext.Profile : null);
				return RunModeService.AllowsGameplayBootInScene(activeScene.name, sceneProfileSO);
			}
		}

		// Token: 0x060064D6 RID: 25814 RVA: 0x00284548 File Offset: 0x00282748
		private static bool AllowsGameplayBootInScene(string sceneName, SceneProfileSO profile)
		{
			return !string.Equals(sceneName, "CielHome", StringComparison.Ordinal) && (profile == null || profile.participatesInRun);
		}

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x060064D7 RID: 25815 RVA: 0x0028456B File Offset: 0x0028276B
		public static RunStartOutfitPolicy StartOutfitPolicy
		{
			get
			{
				return RunModeService.ResolveStartOutfitPolicy(RunModeService.CurrentMode, RunModeService.CurrentSettings, RunModeService.IsClassicTutorialPhase);
			}
		}

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x060064D8 RID: 25816 RVA: 0x00284581 File Offset: 0x00282781
		public static bool ShouldStartClean
		{
			get
			{
				return RunModeService.IsGameplayBoot && RunModeService.StartOutfitPolicy == RunStartOutfitPolicy.Clean;
			}
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x060064D9 RID: 25817 RVA: 0x00284594 File Offset: 0x00282794
		public static bool UsesPresetOutfit
		{
			get
			{
				return RunModeService.IsGameplayBoot && RunModeService.StartOutfitPolicy == RunStartOutfitPolicy.PresetOutfit;
			}
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x060064DA RID: 25818 RVA: 0x002845A7 File Offset: 0x002827A7
		public static bool UsesRankOutfit
		{
			get
			{
				return !RunModeService.UsesHomePresetOutfit && (!RunModeService.IsGameplayBoot || RunModeService.StartOutfitPolicy == RunStartOutfitPolicy.RankOutfit);
			}
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x060064DB RID: 25819 RVA: 0x002845C3 File Offset: 0x002827C3
		public static bool DetectionEnabled
		{
			get
			{
				return RunModeService.CurrentMode != RunModeKind.Free || RunModeService.CurrentSettings.detectionEnabled;
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x060064DC RID: 25820 RVA: 0x002845DC File Offset: 0x002827DC
		public static float NpcVisionMultiplier
		{
			get
			{
				if (!RunModeService.IsFreeMode || !RunModeService.CurrentSettings.detectionEnabled)
				{
					return 1f;
				}
				switch (RunModeService.CurrentSettings.npcDifficulty)
				{
				case FreeNpcDifficulty.VeryEasy:
					return 0.5f;
				case FreeNpcDifficulty.Easy:
					return 0.75f;
				case FreeNpcDifficulty.Hard:
					return 1.25f;
				case FreeNpcDifficulty.Extreme:
					return 1.5f;
				default:
					return 1f;
				}
			}
		}

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x060064DD RID: 25821 RVA: 0x00284644 File Offset: 0x00282844
		public static bool HomeReturnEnabled
		{
			get
			{
				return RunModeService.CurrentMode == RunModeKind.Classic || (RunModeService.CurrentMode == RunModeKind.Free && RunModeService.CurrentSettings.homeReturnEnabled);
			}
		}

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x060064DE RID: 25822 RVA: 0x00284664 File Offset: 0x00282864
		public static bool EncountersEnabled
		{
			get
			{
				return RunModeService.CurrentMode != RunModeKind.Free || (RunModeService.CurrentSettings.detectionEnabled && RunModeService.CurrentSettings.encountersEnabled);
			}
		}

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x060064DF RID: 25823 RVA: 0x00284688 File Offset: 0x00282888
		public static string[] StartingItemIds
		{
			get
			{
				if (RunModeService.CurrentMode != RunModeKind.Free)
				{
					return new string[3];
				}
				string[] startingItemIds = RunModeService.CurrentSettings.startingItemIds;
				return (string[])(((startingItemIds != null) ? startingItemIds.Clone() : null) ?? new string[3]);
			}
		}

		// Token: 0x060064E0 RID: 25824 RVA: 0x002846BE File Offset: 0x002828BE
		public static bool TryClaimFreeStartingItems()
		{
			if (!RunModeService.HasPreparedRun || RunModeService.CurrentMode != RunModeKind.Free || !RunModeService.IsGameplayBoot || RunModeService.Session.startingItemsGranted)
			{
				return false;
			}
			RunModeService.Session.startingItemsGranted = true;
			RunModeService.PersistSession();
			return true;
		}

		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x060064E1 RID: 25825 RVA: 0x002846F5 File Offset: 0x002828F5
		public static bool TimeProgressionEnabled
		{
			get
			{
				return RunModeService.CurrentMode != RunModeKind.Free || RunModeService.CurrentSettings.timeProgressionEnabled;
			}
		}

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x060064E2 RID: 25826 RVA: 0x0028470B File Offset: 0x0028290B
		public static bool VariableWeather
		{
			get
			{
				return RunModeService.CurrentMode == RunModeKind.Free && RunModeService.CurrentSettings.variableWeather;
			}
		}

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x060064E3 RID: 25827 RVA: 0x00284721 File Offset: 0x00282921
		public static int StartHour
		{
			get
			{
				return Mathf.Clamp(RunModeService.CurrentSettings.startHour, 0, 23);
			}
		}

		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x060064E4 RID: 25828 RVA: 0x00284735 File Offset: 0x00282935
		public static float DayLengthMinutes
		{
			get
			{
				return Mathf.Clamp(RunModeService.CurrentSettings.dayLengthMinutes, 5f, 120f);
			}
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x060064E5 RID: 25829 RVA: 0x00284750 File Offset: 0x00282950
		public static int WeatherIndex
		{
			get
			{
				return Mathf.Max(0, RunModeService.CurrentSettings.weatherIndex);
			}
		}

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x060064E6 RID: 25830 RVA: 0x00284762 File Offset: 0x00282962
		public static string WeatherPresetName
		{
			get
			{
				return RunModeService.CurrentSettings.weatherPresetName ?? string.Empty;
			}
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x060064E7 RID: 25831 RVA: 0x00284777 File Offset: 0x00282977
		public static string[] AllowedWeatherPresetNames
		{
			get
			{
				if (RunModeService.CurrentMode != RunModeKind.Free)
				{
					return Array.Empty<string>();
				}
				string[] allowedWeatherPresetNames = RunModeService.CurrentSettings.allowedWeatherPresetNames;
				return (string[])(((allowedWeatherPresetNames != null) ? allowedWeatherPresetNames.Clone() : null) ?? Array.Empty<string>());
			}
		}

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x060064E8 RID: 25832 RVA: 0x002847AB File Offset: 0x002829AB
		public static float WeatherChangeIntervalMinutes
		{
			get
			{
				return Mathf.Clamp(RunModeService.CurrentSettings.weatherChangeIntervalMinutes, 1f, 20f);
			}
		}

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x060064E9 RID: 25833 RVA: 0x002847C6 File Offset: 0x002829C6
		public static bool UnlimitedVehicleDuration
		{
			get
			{
				return RunModeService.CurrentMode == RunModeKind.Free && !RunModeService.CurrentSettings.vehicleDurationEnabled;
			}
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x060064EA RID: 25834 RVA: 0x002847DF File Offset: 0x002829DF
		public static bool UnlimitedStamina
		{
			get
			{
				return RunModeService.CurrentMode == RunModeKind.Free && RunModeService.CurrentSettings.infiniteStamina;
			}
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x060064EB RID: 25835 RVA: 0x002847F5 File Offset: 0x002829F5
		public static float TrashcanLootMultiplier
		{
			get
			{
				if (RunModeService.CurrentMode != RunModeKind.Free)
				{
					return 1f;
				}
				if (RunModeService.CurrentSettings.lootAmount == FreeLootAmount.Scarce)
				{
					return 0.5f;
				}
				if (RunModeService.CurrentSettings.lootAmount != FreeLootAmount.Abundant)
				{
					return 1f;
				}
				return 2f;
			}
		}

		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x060064EC RID: 25836 RVA: 0x0028482F File Offset: 0x00282A2F
		public static float VehicleSpawnMultiplier
		{
			get
			{
				if (RunModeService.CurrentMode != RunModeKind.Free)
				{
					return 1f;
				}
				return FreeRunSettings.NormalizeVehicleSpawnMultiplier(RunModeService.CurrentSettings.vehicleSpawnMultiplier);
			}
		}

		// Token: 0x060064ED RID: 25837 RVA: 0x00284850 File Offset: 0x00282A50
		public static float ResolveBodyPaintDuration(float authoredSeconds)
		{
			if (RunModeService.CurrentMode != RunModeKind.Free)
			{
				return authoredSeconds;
			}
			BodyPaintDurationMode bodyPaintDurationMode = RunModeService.CurrentSettings.bodyPaintDurationMode;
			if (bodyPaintDurationMode == BodyPaintDurationMode.Custom)
			{
				return FreeRunSettings.NormalizeBodyPaintDuration(RunModeService.CurrentSettings.bodyPaintDurationSeconds);
			}
			if (bodyPaintDurationMode != BodyPaintDurationMode.Infinite)
			{
				return authoredSeconds;
			}
			return float.PositiveInfinity;
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x060064EE RID: 25838 RVA: 0x00284893 File Offset: 0x00282A93
		public static bool UnlimitedBodyPaintPenUses
		{
			get
			{
				return RunModeService.CurrentMode == RunModeKind.Free && RunModeService.CurrentSettings.infiniteBodyPaintPenUses;
			}
		}

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x060064EF RID: 25839 RVA: 0x002848A9 File Offset: 0x00282AA9
		public static bool BodyPaintRainWashEnabled
		{
			get
			{
				return RunModeService.CurrentMode != RunModeKind.Free || RunModeService.CurrentSettings.bodyPaintRainWashEnabled;
			}
		}

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x060064F0 RID: 25840 RVA: 0x002848BF File Offset: 0x00282ABF
		public static bool UnlimitedItemDurability
		{
			get
			{
				return RunModeService.CurrentMode == RunModeKind.Free && !RunModeService.CurrentSettings.itemDurabilityEnabled;
			}
		}

		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x060064F1 RID: 25841 RVA: 0x002848D8 File Offset: 0x00282AD8
		public static bool UnlimitedClothingDurability
		{
			get
			{
				return RunModeService.IsLongWayHome || (RunModeService.CurrentMode == RunModeKind.Free && !RunModeService.CurrentSettings.clothingDurabilityEnabled);
			}
		}

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x060064F2 RID: 25842 RVA: 0x002848FA File Offset: 0x00282AFA
		public static float ItemDurationMultiplier
		{
			get
			{
				if (RunModeService.CurrentMode != RunModeKind.Free)
				{
					return 1f;
				}
				return Mathf.Clamp(RunModeService.CurrentSettings.itemDurationMultiplier, 0.25f, 5f);
			}
		}

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x060064F3 RID: 25843 RVA: 0x00284923 File Offset: 0x00282B23
		public static float ClothingDurationMultiplier
		{
			get
			{
				if (RunModeService.CurrentMode != RunModeKind.Free)
				{
					return 1f;
				}
				return Mathf.Clamp(RunModeService.CurrentSettings.clothingDurationMultiplier, 0.25f, 5f);
			}
		}

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x060064F4 RID: 25844 RVA: 0x0028494C File Offset: 0x00282B4C
		public static bool UnlimitedDurability
		{
			get
			{
				return RunModeService.UnlimitedItemDurability && RunModeService.UnlimitedClothingDurability;
			}
		}

		// Token: 0x060064F5 RID: 25845 RVA: 0x0028495C File Offset: 0x00282B5C
		public static RunStartOutfitPolicy ResolveStartOutfitPolicy(RunModeKind mode, FreeRunSettings settings = null, bool classicTutorial = false)
		{
			switch (mode)
			{
			case RunModeKind.Guided:
				return RunStartOutfitPolicy.RankOutfit;
			case RunModeKind.Classic:
				if (!classicTutorial)
				{
					return RunStartOutfitPolicy.Clean;
				}
				return RunStartOutfitPolicy.RankOutfit;
			case RunModeKind.Free:
				if (settings != null && !settings.usePresetOutfit)
				{
					return RunStartOutfitPolicy.Clean;
				}
				return RunStartOutfitPolicy.PresetOutfit;
			case RunModeKind.LongWayHome:
				return RunStartOutfitPolicy.Clean;
			default:
				return RunStartOutfitPolicy.Clean;
			}
		}

		// Token: 0x060064F6 RID: 25846 RVA: 0x00284994 File Offset: 0x00282B94
		public static FreeRunSettings LoadFreeSettings()
		{
			if (RunModeService._freeSettings != null)
			{
				return RunModeService._freeSettings.Clone();
			}
			string @string = PlayerPrefs.GetString("RunModes.FreeSettings.v1", string.Empty);
			try
			{
				RunModeService._freeSettings = new FreeRunSettings();
				if (!string.IsNullOrEmpty(@string))
				{
					JsonUtility.FromJsonOverwrite(@string, RunModeService._freeSettings);
					if (@string.IndexOf("\"schemaVersion\"", StringComparison.Ordinal) < 0)
					{
						RunModeService._freeSettings.schemaVersion = 0;
					}
				}
			}
			catch (ArgumentException)
			{
				RunModeService._freeSettings = new FreeRunSettings();
			}
			RunModeService._freeSettings.Normalize();
			return RunModeService._freeSettings.Clone();
		}

		// Token: 0x060064F7 RID: 25847 RVA: 0x00284A30 File Offset: 0x00282C30
		public static void SaveFreeSettings(FreeRunSettings settings)
		{
			RunModeService._freeSettings = ((settings != null) ? settings.Clone() : new FreeRunSettings());
			RunModeService._freeSettings.Normalize();
			PlayerPrefs.SetString("RunModes.FreeSettings.v1", JsonUtility.ToJson(RunModeService._freeSettings));
			PlayerPrefs.Save();
			Action changed = RunModeService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060064F8 RID: 25848 RVA: 0x00284A84 File Offset: 0x00282C84
		public static ClassicRunKind LoadPreferredClassicRunKind()
		{
			return RunModeService.ResolveClassicRunKind((ClassicRunKind)PlayerPrefs.GetInt("RunModes.ClassicRunKind.v1", 0), RunModeService.ModesUnlocked, RankedLeaderboardService.IsAvailable);
		}

		// Token: 0x060064F9 RID: 25849 RVA: 0x00284AA0 File Offset: 0x00282CA0
		public static ClassicRunSettings LoadClassicSettings()
		{
			if (RunModeService._classicSettings == null)
			{
				string @string = PlayerPrefs.GetString("RunModes.ClassicSettings.v1", string.Empty);
				try
				{
					RunModeService._classicSettings = (string.IsNullOrEmpty(@string) ? new ClassicRunSettings() : (JsonUtility.FromJson<ClassicRunSettings>(@string) ?? new ClassicRunSettings()));
				}
				catch (ArgumentException)
				{
					RunModeService._classicSettings = new ClassicRunSettings();
				}
			}
			return RunModeService._classicSettings.Clone();
		}

		// Token: 0x060064FA RID: 25850 RVA: 0x00284B10 File Offset: 0x00282D10
		public static void SaveClassicSettings(ClassicRunSettings settings)
		{
			RunModeService._classicSettings = ((settings != null) ? settings.Clone() : new ClassicRunSettings());
			PlayerPrefs.SetString("RunModes.ClassicSettings.v1", JsonUtility.ToJson(RunModeService._classicSettings));
			PlayerPrefs.Save();
			Action changed = RunModeService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060064FB RID: 25851 RVA: 0x00284B50 File Offset: 0x00282D50
		public static LongWayHomeRunSettings LoadLongWayHomeSettings()
		{
			if (RunModeService._longWayHomeSettings == null)
			{
				string @string = PlayerPrefs.GetString("RunModes.LongWayHomeSettings.v1", string.Empty);
				RunModeService._longWayHomeSettings = (string.IsNullOrEmpty(@string) ? new LongWayHomeRunSettings() : (JsonUtility.FromJson<LongWayHomeRunSettings>(@string) ?? new LongWayHomeRunSettings()));
			}
			return RunModeService._longWayHomeSettings.Clone();
		}

		// Token: 0x060064FC RID: 25852 RVA: 0x00284BA1 File Offset: 0x00282DA1
		public static void SaveLongWayHomeSettings(LongWayHomeRunSettings settings)
		{
			RunModeService._longWayHomeSettings = ((settings != null) ? settings.Clone() : new LongWayHomeRunSettings());
			PlayerPrefs.SetString("RunModes.LongWayHomeSettings.v1", JsonUtility.ToJson(RunModeService._longWayHomeSettings));
			PlayerPrefs.Save();
			Action changed = RunModeService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060064FD RID: 25853 RVA: 0x00284BE0 File Offset: 0x00282DE0
		public static ClassicRunKind ResolveClassicRunKind(ClassicRunKind requested, bool classicUnlocked, bool rankedBackendAvailable)
		{
			if (requested != ClassicRunKind.Ranked || !classicUnlocked || !rankedBackendAvailable)
			{
				return ClassicRunKind.Normal;
			}
			return ClassicRunKind.Ranked;
		}

		// Token: 0x060064FE RID: 25854 RVA: 0x00284BF0 File Offset: 0x00282DF0
		public static void SavePreferredClassicRunKind(ClassicRunKind kind)
		{
			ClassicRunKind classicRunKind = RunModeService.ResolveClassicRunKind(kind, RunModeService.ModesUnlocked, RankedLeaderboardService.IsAvailable);
			PlayerPrefs.SetInt("RunModes.ClassicRunKind.v1", (int)classicRunKind);
			PlayerPrefs.Save();
		}

		// Token: 0x060064FF RID: 25855 RVA: 0x00284C20 File Offset: 0x00282E20
		public static void DisqualifyLeaderboardRun(string reason = "Debug tools changed this run. Its score will not be submitted.")
		{
			if (!RunModeService.IsRankedRun || RunModeService.Session.leaderboardDisqualified)
			{
				return;
			}
			RunModeService.Session.leaderboardDisqualified = true;
			RunModeService.Session.leaderboardExclusionReason = (string.IsNullOrWhiteSpace(reason) ? "This run was modified and cannot submit a score." : reason);
			RankedRunIntegrity.Disqualify(RunModeService.Session, RunModeService.Session.leaderboardExclusionReason);
			RunModeService.PersistSession();
			Debug.LogWarning("[Ranked] " + RunModeService.Session.leaderboardExclusionReason);
			Action changed = RunModeService.Changed;
			if (changed != null)
			{
				changed();
			}
			Action<string> leaderboardRunDisqualified = RunModeService.LeaderboardRunDisqualified;
			if (leaderboardRunDisqualified == null)
			{
				return;
			}
			leaderboardRunDisqualified(RunModeService.Session.leaderboardExclusionReason);
		}

		// Token: 0x06006500 RID: 25856 RVA: 0x00284CC1 File Offset: 0x00282EC1
		internal static bool ValidateRankedSessionForSubmission()
		{
			if (!RunModeService.IsLeaderboardEligibleRun)
			{
				return false;
			}
			if (RankedRunIntegrity.VerifySession(RunModeService.Session))
			{
				return true;
			}
			RunModeService.DisqualifyLeaderboardRun("This ranked session could not be verified. You can keep playing without submitting a score.");
			return false;
		}

		// Token: 0x06006501 RID: 25857 RVA: 0x00284CE5 File Offset: 0x00282EE5
		internal static void MarkRankedSubmissionAccepted()
		{
			RankedRunIntegrity.MarkSubmitted(RunModeService.Session);
		}

		// Token: 0x06006502 RID: 25858 RVA: 0x00284CF2 File Offset: 0x00282EF2
		internal static void CloseRankedRun()
		{
			RankedRunIntegrity.Close(RunModeService.Session);
		}

		// Token: 0x06006503 RID: 25859 RVA: 0x00284D00 File Offset: 0x00282F00
		private static void PersistSession()
		{
			if (!RankedRunIntegrity.SealSession(RunModeService.Session) && RunModeService.IsRankedRun)
			{
				RunModeService.Session.leaderboardDisqualified = true;
				RunModeService.Session.leaderboardExclusionReason = "This ranked session could not be verified. You can keep playing without submitting a score.";
			}
			PlayerPrefs.SetString(RunModeService.CurrentSessionKey, JsonUtility.ToJson(RunModeService.Session));
			PlayerPrefs.Save();
		}

		// Token: 0x06006504 RID: 25860 RVA: 0x00284D54 File Offset: 0x00282F54
		private static void ValidateLoadedRankedSession()
		{
			if (!RankedRunIntegrity.IsRanked(RunModeService._session))
			{
				return;
			}
			string text = RankedRunIntegrity.GetExclusion(RunModeService._session);
			if (string.IsNullOrEmpty(text) && !RankedRunIntegrity.VerifySession(RunModeService._session))
			{
				text = "This ranked session could not be verified. You can keep playing without submitting a score.";
			}
			if (string.IsNullOrEmpty(text))
			{
				RunModeService.ValidateRankedResumeEnvironment();
				return;
			}
			RunModeService._session.leaderboardDisqualified = true;
			RunModeService._session.leaderboardExclusionReason = text;
		}

		// Token: 0x06006505 RID: 25861 RVA: 0x00284DB8 File Offset: 0x00282FB8
		public static bool TryPrepareRun(RunModeKind requestedMode, FreeRunSettings freeSettings, out string error)
		{
			return RunModeService.TryPrepareRun(requestedMode, freeSettings, ClassicRunKind.Normal, out error);
		}

		// Token: 0x06006506 RID: 25862 RVA: 0x00284DC4 File Offset: 0x00282FC4
		public static bool TryPrepareRun(RunModeKind requestedMode, FreeRunSettings freeSettings, ClassicRunKind classicRunKind, out string error)
		{
			error = null;
			if (requestedMode == RunModeKind.LongWayHome && !RunModeService.LongWayHomeUnlocked)
			{
				error = "Reach Certified Exhibitionist to unlock Long Way Home.";
				return false;
			}
			RunModeKind runModeKind = RunModeService.CanonicalMode(requestedMode);
			if (runModeKind == RunModeKind.Free && !RunModeService.ModesUnlocked)
			{
				error = "Become a real slut to unlock Free Mode.";
				return false;
			}
			ClassicRunKind classicRunKind2 = ((runModeKind == RunModeKind.Classic) ? classicRunKind : ClassicRunKind.Normal);
			if (classicRunKind2 == ClassicRunKind.Ranked && !RunModeService.ModesUnlocked)
			{
				error = "Finish Ciel's Classic journey before entering Ranked.";
				return false;
			}
			if (classicRunKind2 == ClassicRunKind.Ranked && !RankedLeaderboardService.IsAvailable)
			{
				error = "Ranked is unavailable on this platform.";
				return false;
			}
			string text = ((classicRunKind2 == ClassicRunKind.Ranked) ? RankedLeaderboardService.CurrentAccountId : null);
			if (classicRunKind2 == ClassicRunKind.Ranked && string.IsNullOrEmpty(text))
			{
				error = "Sign in to Steam before entering Ranked.";
				return false;
			}
			NPCRosterSnapshot npcrosterSnapshot;
			if (!NPCRosterSession.TryCapture(out npcrosterSnapshot, out error))
			{
				return false;
			}
			FreeRunSettings freeRunSettings = ((runModeKind == RunModeKind.Free) ? (freeSettings ?? RunModeService.LoadFreeSettings()).Clone() : new FreeRunSettings());
			freeRunSettings.Normalize();
			if (runModeKind == RunModeKind.Free)
			{
				RunModeService.SaveFreeSettings(freeRunSettings);
			}
			bool isAtHome = RunModeService.IsAtHome;
			CollarRunHandoff collarRunHandoff = CollarRunHandoff.CaptureForPlayer(isAtHome ? PlayerInventoryManager.Instance : null);
			MapPinController.ClearForTransition();
			RunModeSession runModeSession = new RunModeSession();
			runModeSession.prepared = true;
			runModeSession.mode = runModeKind;
			runModeSession.classicTutorial = runModeKind == RunModeKind.Classic && !RunModeService.ModesUnlocked;
			runModeSession.classicRunKind = classicRunKind2;
			runModeSession.classicSettings = ((runModeKind == RunModeKind.Classic && RunModeService.ClassicStartTimeUnlocked) ? RunModeService.LoadClassicSettings() : new ClassicRunSettings());
			runModeSession.settings = freeRunSettings;
			runModeSession.longWayHomeSettings = ((runModeKind == RunModeKind.LongWayHome) ? RunModeService.LoadLongWayHomeSettings() : new LongWayHomeRunSettings());
			runModeSession.npcRoster = npcrosterSnapshot;
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			runModeSession.startingRankId = ((currentRank != null) ? currentRank.rankId : null);
			runModeSession.startedAsAdmin = RankProgressionService.IsAdminRankActive;
			runModeSession.rankedAccountId = text;
			runModeSession.rankedBuildIdentity = (RankedLeaderboardService.CanSubmitScores ? RankedPlatformPolicy.CurrentSubmissionIdentity : RankedPlatformPolicy.CurrentBuildKind.ToString());
			runModeSession.collarHandoffPending = isAtHome;
			runModeSession.collarHandoffWorn = collarRunHandoff != null;
			runModeSession.collarHandoff = collarRunHandoff;
			RunModeService._session = runModeSession;
			NPCRosterSession.ResetFallback();
			if (classicRunKind2 == ClassicRunKind.Ranked && !RankedRunIntegrity.Begin(RunModeService._session))
			{
				RunModeService._session.leaderboardDisqualified = true;
				RunModeService._session.leaderboardExclusionReason = "This ranked session could not be verified. You can keep playing without submitting a score.";
			}
			if (classicRunKind2 == ClassicRunKind.Ranked && RunModeService._session.startedAsAdmin)
			{
				RunModeService._session.leaderboardDisqualified = true;
				RunModeService._session.leaderboardExclusionReason = "Admin review runs do not submit scores.";
				RankedRunIntegrity.Disqualify(RunModeService._session, RunModeService._session.leaderboardExclusionReason);
			}
			if (runModeKind == RunModeKind.Classic)
			{
				RunModeService.SavePreferredClassicRunKind(classicRunKind2);
			}
			RunModeService.PersistSession();
			Action changed = RunModeService.Changed;
			if (changed != null)
			{
				changed();
			}
			Debug.Log(string.Concat(new string[]
			{
				string.Format("[RunModes] Prepared {0} run (classic={1}, ", runModeKind, RunModeService.CurrentClassicRunKind),
				string.Format("tutorial={0}, ", RunModeService.IsClassicTutorialPhase),
				string.Format("outfit={0}, detection={1}, ", RunModeService.StartOutfitPolicy, RunModeService.DetectionEnabled),
				string.Format("carriedCollar={0}, ", collarRunHandoff != null),
				string.Format("time={0}, variableWeather={1}, ", RunModeService.TimeProgressionEnabled, RunModeService.VariableWeather),
				string.Format("unlimitedDurability={0}, presetOutfit={1}).", RunModeService.UnlimitedDurability, freeRunSettings.usePresetOutfit)
			}));
			return true;
		}

		// Token: 0x06006507 RID: 25863 RVA: 0x002850E0 File Offset: 0x002832E0
		public static void ClearPreparedRun()
		{
			MapPinController.ClearForTransition();
			NPCRosterSession.ResetFallback();
			if (!RunModeService.Session.prepared)
			{
				return;
			}
			RunModeService._session = new RunModeSession();
			PlayerPrefs.DeleteKey(RunModeService.CurrentSessionKey);
			PlayerPrefs.Save();
			Action changed = RunModeService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x06006508 RID: 25864 RVA: 0x0028512C File Offset: 0x0028332C
		private static RunModeSession Session
		{
			get
			{
				if (RunModeService._session != null)
				{
					return RunModeService._session;
				}
				string @string = PlayerPrefs.GetString(RunModeService.CurrentSessionKey, string.Empty);
				try
				{
					RunModeService._session = (string.IsNullOrEmpty(@string) ? new RunModeSession() : (JsonUtility.FromJson<RunModeSession>(@string) ?? new RunModeSession()));
				}
				catch (ArgumentException)
				{
					RunModeService._session = new RunModeSession();
				}
				RunModeService.ValidateLoadedRankedSession();
				if (RunModeService._session.settings == null)
				{
					RunModeService._session.settings = new FreeRunSettings();
				}
				RunModeService._session.settings.Normalize();
				if (RunModeService._session.mapsVisit)
				{
					RunModeService.NormalizeMapsSettings(RunModeService._session.settings);
				}
				RunModeService.MigrateLegacySession(RunModeService._session);
				return RunModeService._session;
			}
		}

		// Token: 0x06006509 RID: 25865 RVA: 0x002851F4 File Offset: 0x002833F4
		private static RunModeKind CanonicalMode(RunModeKind mode)
		{
			if (mode != RunModeKind.Guided)
			{
				return mode;
			}
			return RunModeKind.Classic;
		}

		// Token: 0x0600650A RID: 25866 RVA: 0x002851FC File Offset: 0x002833FC
		private static void MigrateLegacySession(RunModeSession session)
		{
			if (session == null)
			{
				return;
			}
			bool flag = false;
			if (session.mode == RunModeKind.Guided)
			{
				session.mode = RunModeKind.Classic;
				session.classicTutorial = true;
				flag = true;
			}
			if (session.version < 2)
			{
				session.version = 2;
				flag = true;
			}
			if (session.version < 3)
			{
				session.version = 3;
				session.classicRunKind = ClassicRunKind.Normal;
				flag = true;
			}
			if (session.version < 4)
			{
				session.version = 4;
				flag = true;
			}
			if (session.version < 5)
			{
				session.version = 5;
				RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
				session.startingRankId = ((currentRank != null) ? currentRank.rankId : null);
				session.startedAsAdmin = RankProgressionService.IsAdminRankActive;
				flag = true;
			}
			if (session.version < 6)
			{
				session.version = 6;
				session.longWayHomeSettings = new LongWayHomeRunSettings
				{
					startingLootEnabled = true,
					startAtNight = (RankProgressionService.CurrentRank != null && RankProgressionService.CurrentRank.nightOnly)
				};
				flag = true;
			}
			else if (session.longWayHomeSettings == null)
			{
				session.longWayHomeSettings = new LongWayHomeRunSettings();
				flag = true;
			}
			if (session.version < 7)
			{
				session.version = 7;
				session.startingItemsGranted = true;
				flag = true;
			}
			if (session.classicSettings == null)
			{
				session.classicSettings = new ClassicRunSettings();
				flag = true;
			}
			if ((session.mode != RunModeKind.Classic || session.classicTutorial || !Enum.IsDefined(typeof(ClassicRunKind), session.classicRunKind)) && session.classicRunKind != ClassicRunKind.Normal)
			{
				session.classicRunKind = ClassicRunKind.Normal;
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			PlayerPrefs.SetString(RunModeService.CurrentSessionKey, JsonUtility.ToJson(session));
			PlayerPrefs.Save();
		}

		// Token: 0x0600650B RID: 25867 RVA: 0x00285378 File Offset: 0x00283578
		public static FreeRunSettings LoadMapsSettings()
		{
			if (RunModeService._mapsSettings == null)
			{
				RunModeService._mapsSettings = new FreeRunSettings();
				try
				{
					string @string = PlayerPrefs.GetString("RunModes.MapsSettings.v1", string.Empty);
					if (!string.IsNullOrEmpty(@string))
					{
						JsonUtility.FromJsonOverwrite(@string, RunModeService._mapsSettings);
					}
				}
				catch (ArgumentException)
				{
					RunModeService._mapsSettings = new FreeRunSettings();
				}
				RunModeService.NormalizeMapsSettings(RunModeService._mapsSettings);
			}
			return RunModeService._mapsSettings.Clone();
		}

		// Token: 0x0600650C RID: 25868 RVA: 0x002853EC File Offset: 0x002835EC
		public static void SaveMapsSettings(FreeRunSettings settings)
		{
			RunModeService._mapsSettings = ((settings != null) ? settings.Clone() : null) ?? new FreeRunSettings();
			RunModeService.NormalizeMapsSettings(RunModeService._mapsSettings);
			PlayerPrefs.SetString("RunModes.MapsSettings.v1", JsonUtility.ToJson(RunModeService._mapsSettings));
			PlayerPrefs.Save();
		}

		// Token: 0x0600650D RID: 25869 RVA: 0x0028542B File Offset: 0x0028362B
		private static void NormalizeMapsSettings(FreeRunSettings settings)
		{
			settings.Normalize();
			settings.homeReturnEnabled = true;
			settings.useCustomWorldSeed = false;
			settings.worldSeed = 0;
			settings.vehicleDurationEnabled = true;
			settings.vehicleSpawnMultiplier = 1f;
		}

		// Token: 0x0600650E RID: 25870 RVA: 0x0028545C File Offset: 0x0028365C
		public static bool TryPrepareMapsVisit(FreeRunSettings preferences, out string error)
		{
			error = null;
			if (!RunModeService.IsAtHome)
			{
				error = "AVAILABLE FROM HOME ONLY";
				return false;
			}
			if (!RunModeService.LongWayHomeUnlocked)
			{
				error = "RANK REQUIRED";
				return false;
			}
			NPCRosterSnapshot npcrosterSnapshot;
			if (!NPCRosterSession.TryCapture(out npcrosterSnapshot, out error))
			{
				return false;
			}
			FreeRunSettings freeRunSettings = (preferences ?? RunModeService.LoadMapsSettings()).Clone();
			RunModeService.NormalizeMapsSettings(freeRunSettings);
			CollarRunHandoff collarRunHandoff = (freeRunSettings.usePresetOutfit ? CollarRunHandoff.CaptureForPlayer(PlayerInventoryManager.Instance) : null);
			MapPinController.ClearForTransition();
			RunModeSession runModeSession = new RunModeSession();
			runModeSession.prepared = true;
			runModeSession.mode = RunModeKind.Free;
			runModeSession.mapsVisit = true;
			runModeSession.settings = freeRunSettings;
			runModeSession.npcRoster = npcrosterSnapshot;
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			runModeSession.startingRankId = ((currentRank != null) ? currentRank.rankId : null);
			runModeSession.startedAsAdmin = RankProgressionService.IsAdminRankActive;
			runModeSession.collarHandoffPending = true;
			runModeSession.collarHandoffWorn = collarRunHandoff != null;
			runModeSession.collarHandoff = collarRunHandoff;
			RunModeService._session = runModeSession;
			NPCRosterSession.ResetFallback();
			RunModeService.PersistSession();
			Action changed = RunModeService.Changed;
			if (changed != null)
			{
				changed();
			}
			return true;
		}

		// Token: 0x04006775 RID: 26485
		private const string SessionKey = "RunModes.CurrentSession.v1";

		// Token: 0x04006776 RID: 26486
		private const string FreeSettingsKey = "RunModes.FreeSettings.v1";

		// Token: 0x04006777 RID: 26487
		private const string ClassicSettingsKey = "RunModes.ClassicSettings.v1";

		// Token: 0x04006778 RID: 26488
		private const string LongWayHomeSettingsKey = "RunModes.LongWayHomeSettings.v1";

		// Token: 0x04006779 RID: 26489
		private const string ClassicRunKindKey = "RunModes.ClassicRunKind.v1";

		// Token: 0x0400677A RID: 26490
		private static RunModeSession _session;

		// Token: 0x0400677B RID: 26491
		private static FreeRunSettings _freeSettings;

		// Token: 0x0400677C RID: 26492
		private static ClassicRunSettings _classicSettings;

		// Token: 0x0400677D RID: 26493
		private static LongWayHomeRunSettings _longWayHomeSettings;

		// Token: 0x04006780 RID: 26496
		private const string MapsSettingsKey = "RunModes.MapsSettings.v1";

		// Token: 0x04006781 RID: 26497
		private static FreeRunSettings _mapsSettings;
	}
}
