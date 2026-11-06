using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Bozo.AnimeCharacters;
using Features.Achievements;
using Features.Rank;
using Features.RunModes;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Missions
{
	// Token: 0x02000F84 RID: 3972
	public static class MissionService
	{
		// Token: 0x060077BB RID: 30651 RVA: 0x0030C654 File Offset: 0x0030A854
		internal static bool MatchesGeneratedDescription(MissionState mission, out bool acrossAccount)
		{
			string text = MissionService.BuildDescription(mission.kind, mission.target, mission.scope, mission.contextName, mission.qualifierName, mission.secondaryQualifierName);
			acrossAccount = mission.description == text + " across your account";
			if (acrossAccount || mission.description == text)
			{
				return true;
			}
			if (mission.kind != MissionObjectiveKind.ClothesCollected && mission.kind != MissionObjectiveKind.ClothesWorn)
			{
				return false;
			}
			string text2 = text.Replace("clothing items", "garments");
			acrossAccount = mission.description == text2 + " across your account";
			return acrossAccount || mission.description == text2;
		}

		// Token: 0x060077BC RID: 30652 RVA: 0x0030C708 File Offset: 0x0030A908
		internal static string BuildCurrentDescription(MissionState mission)
		{
			return MissionService.BuildDescription(mission.kind, mission.target, mission.scope, mission.contextName, mission.qualifierName, mission.secondaryQualifierName);
		}

		// Token: 0x060077BD RID: 30653 RVA: 0x0030C733 File Offset: 0x0030A933
		private static MissionService.AccountMissionDefinition SecretAccountMission(string id, MissionObjectiveKind kind, float target, int xp)
		{
			return new MissionService.AccountMissionDefinition(id, kind, target, xp, MissionLocalizationTemplates.Full(kind, false), 1, 0, true);
		}

		// Token: 0x060077BE RID: 30654 RVA: 0x0030C748 File Offset: 0x0030A948
		private static MissionService.AccountMissionDefinition[] BuildAccountCatalog()
		{
			List<MissionService.AccountMissionDefinition> list = new List<MissionService.AccountMissionDefinition>(MissionService.LegacyAccountMissionCatalog);
			list.Add(new MissionService.AccountMissionDefinition("account_clothes_40", MissionObjectiveKind.ClothesCollected, 40f, 1500, "Collect 40 clothing items across your runs", 1, 0, false));
			list.Add(new MissionService.AccountMissionDefinition("account_outfits_30", MissionObjectiveKind.ClothesWorn, 30f, 1500, "Wear 30 clothing items across your runs", 1, 0, false));
			list.Add(new MissionService.AccountMissionDefinition("account_salvage_150", MissionObjectiveKind.TrashcanLootItems, 150f, 7000, "Recover 150 items from trashcans", 1, 0, false));
			list.Add(new MissionService.AccountMissionDefinition("account_cover_100", MissionObjectiveKind.WallCoverEntries, 100f, 500, "Enter wall cover 100 times", 1, 0, false));
			list.Add(new MissionService.AccountMissionDefinition("account_cover_time_1800", MissionObjectiveKind.WallCoverTime, 1800f, 750, "Spend 1,800 seconds in wall cover", 1, 0, false));
			foreach (MissionService.AccountMissionDefinition accountMissionDefinition in list.ToArray())
			{
				if (accountMissionDefinition.Kind != MissionObjectiveKind.PoiDiscoveries)
				{
					for (int j = 2; j <= 3; j++)
					{
						float num = accountMissionDefinition.Target * ((j == 2) ? 3f : 10f);
						list.Add(new MissionService.AccountMissionDefinition(accountMissionDefinition.Id + "_tier" + j.ToString(), accountMissionDefinition.Kind, num, accountMissionDefinition.Xp * j, MissionService.BuildDescription(accountMissionDefinition.Kind, num, MissionScope.Global, string.Empty, string.Empty, string.Empty) + " across your account", j, (j == 2) ? 2 : 4, false));
					}
				}
			}
			list.Add(new MissionService.AccountMissionDefinition("account_explorer_2", MissionObjectiveKind.PoiDiscoveries, 2f, 1500, "Discover 2 different locations across the city", 1, 0, false));
			list.Add(new MissionService.AccountMissionDefinition("account_explorer_4", MissionObjectiveKind.PoiDiscoveries, 4f, 2500, "Discover 4 different locations across the city", 2, 2, false));
			list.Add(new MissionService.AccountMissionDefinition("account_home_1", MissionObjectiveKind.SafeHomeReturns, 1f, 500, "Complete a Classic run by returning home safely", 1, 0, false));
			list.Add(new MissionService.AccountMissionDefinition("account_home_5", MissionObjectiveKind.SafeHomeReturns, 5f, 2000, "Return home safely from 5 Classic runs", 2, 2, false));
			list.Add(new MissionService.AccountMissionDefinition("account_home_20", MissionObjectiveKind.SafeHomeReturns, 20f, 6000, "Return home safely from 20 Classic runs", 3, 4, false));
			list.Add(new MissionService.AccountMissionDefinition("account_home_distance_500", MissionObjectiveKind.SafeHomeReturnDistance, 250f, 1000, MissionService.AccountObjective(MissionObjectiveKind.SafeHomeReturnDistance, 250, string.Empty, string.Empty), 1, 0, false));
			list.Add(new MissionService.AccountMissionDefinition("account_home_distance_2000", MissionObjectiveKind.SafeHomeReturnDistance, 1000f, 3000, MissionService.AccountObjective(MissionObjectiveKind.SafeHomeReturnDistance, 1000, string.Empty, string.Empty), 2, 2, false));
			list.Add(new MissionService.AccountMissionDefinition("account_home_distance_5000", MissionObjectiveKind.SafeHomeReturnDistance, 2500f, 6000, MissionService.AccountObjective(MissionObjectiveKind.SafeHomeReturnDistance, 2500, string.Empty, string.Empty), 3, 4, false));
			list.Add(new MissionService.AccountMissionDefinition("account_first_flight", MissionObjectiveKind.FirstFlight, 1f, 2500, "Fly for the first time", 1, 0, false));
			list.Add(MissionService.SecretAccountMission("account_egg_ghost_protocol", MissionObjectiveKind.GhostProtocol, 1f, 12500));
			list.Add(MissionService.SecretAccountMission("account_egg_group_shot", MissionObjectiveKind.GroupShot, 1f, 7500));
			list.Add(MissionService.SecretAccountMission("account_egg_one_wheel", MissionObjectiveKind.UnicycleDistance, 500f, 7500));
			list.Add(MissionService.SecretAccountMission("account_egg_most_wanted", MissionObjectiveKind.MostWantedHome, 1f, 7500));
			list.Add(MissionService.SecretAccountMission("account_egg_ghost_scare", MissionObjectiveKind.GhostScare, 1f, 5000));
			return list.ConvertAll<MissionService.AccountMissionDefinition>((MissionService.AccountMissionDefinition d) => new MissionService.AccountMissionDefinition(d.Id, d.Kind, d.Target, (d.Kind == MissionObjectiveKind.Escapes) ? 1000 : Mathf.Max(1, Mathf.RoundToInt((float)d.Xp / 5f)), d.Description, d.Tier, d.MinimumRankIndex, d.Secret)).ToArray();
		}

		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x060077BF RID: 30655 RVA: 0x0030CB04 File Offset: 0x0030AD04
		// (remove) Token: 0x060077C0 RID: 30656 RVA: 0x0030CB38 File Offset: 0x0030AD38
		public static event Action Changed;

		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x060077C1 RID: 30657 RVA: 0x0030CB6C File Offset: 0x0030AD6C
		// (remove) Token: 0x060077C2 RID: 30658 RVA: 0x0030CBA0 File Offset: 0x0030ADA0
		public static event Action<float> ClassicHomeReturnReached;

		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x060077C3 RID: 30659 RVA: 0x0030CBD4 File Offset: 0x0030ADD4
		// (remove) Token: 0x060077C4 RID: 30660 RVA: 0x0030CC08 File Offset: 0x0030AE08
		public static event Action<MissionState> Progressed;

		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x060077C5 RID: 30661 RVA: 0x0030CC3C File Offset: 0x0030AE3C
		// (remove) Token: 0x060077C6 RID: 30662 RVA: 0x0030CC70 File Offset: 0x0030AE70
		public static event Action<MissionCompletion> Completed;

		// Token: 0x17001394 RID: 5012
		// (get) Token: 0x060077C7 RID: 30663 RVA: 0x0030CCA3 File Offset: 0x0030AEA3
		public static IReadOnlyList<MissionState> GlobalMissions
		{
			get
			{
				MissionService.EnsureAccountMissions();
				return MissionService._save.globalMissions;
			}
		}

		// Token: 0x17001395 RID: 5013
		// (get) Token: 0x060077C8 RID: 30664 RVA: 0x0030CCA3 File Offset: 0x0030AEA3
		public static IReadOnlyList<MissionState> AccountMissions
		{
			get
			{
				MissionService.EnsureAccountMissions();
				return MissionService._save.globalMissions;
			}
		}

		// Token: 0x17001396 RID: 5014
		// (get) Token: 0x060077C9 RID: 30665 RVA: 0x0030CCB4 File Offset: 0x0030AEB4
		public static IReadOnlyList<MissionState> VisibleAccountMissions
		{
			get
			{
				MissionService.EnsureAccountMissions();
				MissionService.VisibleAccountStates.Clear();
				foreach (MissionState missionState in MissionService._save.globalMissions)
				{
					if (missionState.IsComplete || (!missionState.secret && MissionRankPolicy.IsAvailable(missionState.kind) && (RankProgressionService.IsAdminRankActive || RankProgressionService.RankIndex >= missionState.minimumRankIndex)))
					{
						MissionService.VisibleAccountStates.Add(missionState);
					}
				}
				MissionService.VisibleAccountStates.Sort(delegate(MissionState a, MissionState b)
				{
					int num = a.category.CompareTo(b.category);
					if (num != 0)
					{
						return num;
					}
					int num2 = a.kind.CompareTo(b.kind);
					if (num2 == 0)
					{
						return a.tier.CompareTo(b.tier);
					}
					return num2;
				});
				return MissionService.VisibleAccountStates;
			}
		}

		// Token: 0x17001397 RID: 5015
		// (get) Token: 0x060077CA RID: 30666 RVA: 0x0030CD80 File Offset: 0x0030AF80
		public static IReadOnlyList<MissionState> RunMissions
		{
			get
			{
				if (!MissionService.AreRunMissionsEnabled)
				{
					return Array.Empty<MissionState>();
				}
				return MissionService.RunStates;
			}
		}

		// Token: 0x17001398 RID: 5016
		// (get) Token: 0x060077CB RID: 30667 RVA: 0x0030CDA3 File Offset: 0x0030AFA3
		public static IReadOnlyList<MissionCompletion> CompletedRunMissions
		{
			get
			{
				return MissionService.RunCompleted;
			}
		}

		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x060077CC RID: 30668 RVA: 0x0030CDAA File Offset: 0x0030AFAA
		public static bool IsRunActive
		{
			get
			{
				return MissionService._runActive;
			}
		}

		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x060077CD RID: 30669 RVA: 0x0030CDB1 File Offset: 0x0030AFB1
		public static bool AreRunMissionsEnabled
		{
			get
			{
				return MissionService._runActive && !RunModeService.IsLongWayHome;
			}
		}

		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x060077CE RID: 30670 RVA: 0x0030CDC4 File Offset: 0x0030AFC4
		public static bool IsRunSceneContext
		{
			get
			{
				Scene activeScene = SceneManager.GetActiveScene();
				if (activeScene.name == "CielHome" || activeScene.name == "MainMenu" || activeScene.name == "Customization")
				{
					return false;
				}
				SceneContext sceneContext = SceneContext.Current;
				return sceneContext == null || sceneContext.gameObject.scene != activeScene || sceneContext.Profile == null || sceneContext.Profile.participatesInRun;
			}
		}

		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x060077CF RID: 30671 RVA: 0x0030CE4F File Offset: 0x0030B04F
		public static string RunContextId
		{
			get
			{
				return MissionService._runContextId;
			}
		}

		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x060077D0 RID: 30672 RVA: 0x0030CE56 File Offset: 0x0030B056
		public static string RunContextName
		{
			get
			{
				return MissionService._runContextName;
			}
		}

		// Token: 0x060077D1 RID: 30673 RVA: 0x0030CE5D File Offset: 0x0030B05D
		public static bool ShouldRotateCompletedMission(MissionScope scope)
		{
			return scope == MissionScope.Run;
		}

		// Token: 0x060077D2 RID: 30674 RVA: 0x0030CE63 File Offset: 0x0030B063
		public static void EnsureGlobalMissions()
		{
			MissionService.EnsureAccountMissions();
		}

		// Token: 0x060077D3 RID: 30675 RVA: 0x0030CE6C File Offset: 0x0030B06C
		public static void EnsureAccountMissions()
		{
			if (MissionService._reconcilingAccountMissions)
			{
				return;
			}
			MissionService._reconcilingAccountMissions = true;
			try
			{
				MissionService.ReconcileAccountMissions();
			}
			finally
			{
				MissionService._reconcilingAccountMissions = false;
			}
		}

		// Token: 0x060077D4 RID: 30676 RVA: 0x0030CEA8 File Offset: 0x0030B0A8
		private static void ReconcileAccountMissions()
		{
			MissionService.EnsureLoaded();
			bool flag = MissionService._saveNeedsWrite;
			for (int i = MissionService._save.globalMissions.Count - 1; i >= 0; i--)
			{
				MissionState missionState = MissionService._save.globalMissions[i];
				if (missionState == null || !MissionService.IsAccountMissionId(missionState.id))
				{
					MissionService._save.globalMissions.RemoveAt(i);
					flag = true;
				}
			}
			for (int j = 0; j < MissionService.AccountMissionCatalog.Length; j++)
			{
				MissionService.AccountMissionDefinition definition = MissionService.AccountMissionCatalog[j];
				MissionState missionState2 = MissionService._save.globalMissions.Find((MissionState m) => m != null && string.Equals(m.id, definition.Id, StringComparison.Ordinal));
				if (missionState2 == null)
				{
					missionState2 = MissionService.CreateAccountMission(definition, j);
					foreach (MissionState missionState3 in MissionService._save.globalMissions)
					{
						if (missionState3 != null && missionState3.kind == definition.Kind)
						{
							missionState2.progress = Mathf.Max(missionState2.progress, Mathf.Min(missionState2.target, missionState3.progress));
						}
					}
					MissionService._save.globalMissions.Add(missionState2);
					flag = true;
				}
				else
				{
					bool isComplete = missionState2.IsComplete;
					bool flag2 = definition.Kind == MissionObjectiveKind.SafeHomeReturnDistance;
					bool flag3 = flag2 && !isComplete && missionState2.target > definition.Target && missionState2.progress >= definition.Target;
					if (flag3)
					{
						if (RankProgressionService.SuppressProgressionRewards)
						{
							goto IL_0362;
						}
						RankProgressionService.GrantMissionXpOnce(missionState2.id, (float)definition.Xp);
						if (!RankProgressionService.TryPersistMissionRewardClaim(missionState2.id))
						{
							goto IL_0362;
						}
					}
					float num = (flag2 ? Mathf.Max(0f, missionState2.progress) : Mathf.Clamp(missionState2.progress, 0f, definition.Target));
					if (missionState2.target != definition.Target || missionState2.progress != num)
					{
						flag = true;
					}
					missionState2.scope = MissionScope.Global;
					missionState2.kind = definition.Kind;
					missionState2.target = definition.Target;
					missionState2.progress = num;
					if ((!missionState2.IsComplete || flag3) && missionState2.xpReward != definition.Xp)
					{
						missionState2.xpReward = definition.Xp;
						flag = true;
					}
					if (missionState2.description != definition.Description)
					{
						flag = true;
					}
					missionState2.description = definition.Description;
					missionState2.generation = j + 1;
					missionState2.category = MissionRankPolicy.CategoryFor(definition.Kind);
					missionState2.tier = definition.Tier;
					missionState2.minimumRankIndex = definition.MinimumRankIndex;
					missionState2.secret = definition.Secret;
					missionState2.contextId = string.Empty;
					missionState2.contextName = string.Empty;
					missionState2.qualifierId = string.Empty;
					missionState2.qualifierName = string.Empty;
					missionState2.secondaryQualifierId = string.Empty;
					missionState2.secondaryQualifierName = string.Empty;
				}
				IL_0362:;
			}
			MissionService._save.globalMissions.Sort((MissionState a, MissionState b) => a.generation.CompareTo(b.generation));
			if (!MissionService._completedClaimsSeeded)
			{
				List<string> list = new List<string>();
				for (int k = 0; k < MissionService._save.globalMissions.Count; k++)
				{
					MissionState missionState4 = MissionService._save.globalMissions[k];
					if (missionState4 != null && missionState4.IsComplete && !string.IsNullOrWhiteSpace(missionState4.id))
					{
						list.Add(missionState4.id);
					}
				}
				MissionService._completedClaimsSeeded = true;
				RankProgressionService.SeedClaimedMissionRewards(list);
			}
			if (flag)
			{
				MissionService.Save();
			}
		}

		// Token: 0x060077D5 RID: 30677 RVA: 0x0030D2E4 File Offset: 0x0030B4E4
		private static bool IsAccountMissionId(string id)
		{
			for (int i = 0; i < MissionService.AccountMissionCatalog.Length; i++)
			{
				if (string.Equals(MissionService.AccountMissionCatalog[i].Id, id, StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060077D6 RID: 30678 RVA: 0x0030D320 File Offset: 0x0030B520
		private static MissionState CreateAccountMission(MissionService.AccountMissionDefinition definition, int index)
		{
			return new MissionState
			{
				id = definition.Id,
				scope = MissionScope.Global,
				kind = definition.Kind,
				target = definition.Target,
				progress = 0f,
				xpReward = definition.Xp,
				generation = index + 1,
				description = definition.Description,
				category = MissionRankPolicy.CategoryFor(definition.Kind),
				tier = definition.Tier,
				minimumRankIndex = definition.MinimumRankIndex,
				secret = definition.Secret
			};
		}

		// Token: 0x060077D7 RID: 30679 RVA: 0x0030D3C0 File Offset: 0x0030B5C0
		public static void BeginRun(string contextId, string contextName)
		{
			MissionService.EnsureGlobalMissions();
			MissionService.ResetWardrobeUniqueness();
			MissionService.RunCompleted.Clear();
			MissionService.RunStates.Clear();
			MissionService.ParkedRunStates.Clear();
			MissionService.UnavailableSince.Clear();
			MissionService.PlaceKindsDoneThisVisit.Clear();
			MissionService._rankSlotResting = false;
			MissionService._lastRankKind = null;
			MissionService._runActive = true;
			MissionService._homeReturnReported = false;
			MissionService._runContextId = MissionService.NormalizeContextId(contextId);
			MissionService._runContextName = MissionService.NormalizeContextName(contextName);
			MissionService.RegenerateRunSet();
			MissionService._redrawAfterWorldScan = true;
			AchievementTrackingService.BeginRun();
		}

		// Token: 0x060077D8 RID: 30680 RVA: 0x0030D44C File Offset: 0x0030B64C
		public static void SetRunContext(string contextId, string contextName)
		{
			if (!MissionService._runActive)
			{
				return;
			}
			contextId = MissionService.NormalizeContextId(contextId);
			contextName = MissionService.NormalizeContextName(contextName);
			if (string.Equals(MissionService._runContextId, contextId, StringComparison.Ordinal))
			{
				return;
			}
			MissionService.PlaceKindsDoneThisVisit.Clear();
			List<MissionState> list = new List<MissionState>();
			for (int i = MissionService.RunStates.Count - 1; i >= 0; i--)
			{
				MissionState missionState = MissionService.RunStates[i];
				if (missionState != null && !MissionService.IsRunWideObjective(missionState))
				{
					list.Insert(0, missionState);
					MissionService.RunStates.RemoveAt(i);
					MissionService.UnavailableSince.Remove(missionState.id);
				}
			}
			if (!string.IsNullOrEmpty(MissionService._runContextId) && list.Count > 0)
			{
				List<MissionState> list2;
				if (MissionService.ParkedRunStates.TryGetValue(MissionService._runContextId, out list2))
				{
					list.AddRange(list2);
				}
				MissionService.ParkedRunStates[MissionService._runContextId] = list;
			}
			MissionService._runContextId = contextId;
			MissionService._runContextName = contextName;
			List<MissionState> list3;
			if (!MissionService.ParkedRunStates.TryGetValue(contextId, out list3))
			{
				MissionService.RegenerateRunSet();
				MissionService._redrawAfterWorldScan = true;
				return;
			}
			MissionService.ParkedRunStates.Remove(contextId);
			MissionService.RestoreParkedStates(contextId, list3);
			MissionService._redrawAfterWorldScan = false;
			MissionService.FillRunSet();
			Action changed = MissionService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060077D9 RID: 30681 RVA: 0x0030D578 File Offset: 0x0030B778
		private static void RestoreParkedStates(string contextId, List<MissionState> restored)
		{
			List<MissionState> list = restored.FindAll((MissionState state) => state.progress > 0f);
			list.AddRange(restored.FindAll((MissionState state) => state.progress <= 0f));
			List<MissionState> list2 = new List<MissionState>();
			foreach (MissionState missionState in list)
			{
				if (MissionService.RunStates.Count >= 3 && missionState.progress > 0f)
				{
					int num = MissionService.RunStates.FindIndex((MissionState other) => other != null && other.rankSlot && other.progress <= 0f);
					if (num >= 0)
					{
						MissionService.RunStates.RemoveAt(num);
					}
				}
				if (MissionService.RunStates.Count < 3)
				{
					MissionService.RunStates.Add(missionState);
				}
				else
				{
					list2.Add(missionState);
				}
			}
			if (list2.Count > 0)
			{
				MissionService.ParkedRunStates[contextId] = list2;
			}
		}

		// Token: 0x060077DA RID: 30682 RVA: 0x0030D6A0 File Offset: 0x0030B8A0
		private static MissionState TakeWaitingRunState()
		{
			List<MissionState> list;
			if (string.IsNullOrEmpty(MissionService._runContextId) || !MissionService.ParkedRunStates.TryGetValue(MissionService._runContextId, out list) || list.Count == 0)
			{
				return null;
			}
			MissionState missionState = list[0];
			list.RemoveAt(0);
			if (list.Count == 0)
			{
				MissionService.ParkedRunStates.Remove(MissionService._runContextId);
			}
			return missionState;
		}

		// Token: 0x060077DB RID: 30683 RVA: 0x0030D6FC File Offset: 0x0030B8FC
		public static void EndRun()
		{
			if (!MissionService._runActive && MissionService.RunStates.Count == 0)
			{
				return;
			}
			MissionService._runActive = false;
			AchievementTrackingService.EndRun();
			MissionService.ResetWardrobeUniqueness();
			MissionService._runContextId = string.Empty;
			MissionService._runContextName = string.Empty;
			MissionService.RunStates.Clear();
			MissionService.RunCompleted.Clear();
			MissionService.ParkedRunStates.Clear();
			MissionService.UnavailableSince.Clear();
			MissionService.RankCapableKinds.Clear();
			MissionService.PlaceCapableKinds.Clear();
			MissionService.PlaceKindsDoneThisVisit.Clear();
			MissionService._rankSlotResting = false;
			MissionService._lastRankKind = null;
			MissionService._wallCoverAvailable = false;
			MissionService._redrawAfterWorldScan = false;
			Action changed = MissionService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060077DC RID: 30684 RVA: 0x0030D7B3 File Offset: 0x0030B9B3
		public static void RefreshRunMissionsForCurrentContext()
		{
			if (MissionService._runActive)
			{
				MissionService.RegenerateRunSet();
			}
		}

		// Token: 0x060077DD RID: 30685 RVA: 0x0030D7C4 File Offset: 0x0030B9C4
		public static void RefreshRunMissionsAfterWorldScan()
		{
			if (!MissionService._runActive)
			{
				return;
			}
			bool flag;
			if (MissionService._redrawAfterWorldScan)
			{
				flag = !MissionService.RunStates.Exists((MissionState state) => state != null && state.progress > 0f && !MissionService.IsRunWideObjective(state));
			}
			else
			{
				flag = false;
			}
			MissionService._redrawAfterWorldScan = false;
			MissionService.UnavailableSince.Clear();
			if (flag)
			{
				MissionService.RegenerateRunSet();
				return;
			}
			MissionService.RefreshAvailableMissions();
		}

		// Token: 0x060077DE RID: 30686 RVA: 0x0030D82C File Offset: 0x0030BA2C
		public static void SetRankCapabilities(IEnumerable<MissionObjectiveKind> kinds)
		{
			MissionService.RankCapableKinds.Clear();
			if (kinds == null)
			{
				return;
			}
			foreach (MissionObjectiveKind missionObjectiveKind in kinds)
			{
				if (MissionRankPolicy.IsRankSpecial(missionObjectiveKind) || MissionService.IsExposedPhotoObjective(missionObjectiveKind))
				{
					MissionService.RankCapableKinds.Add(missionObjectiveKind);
				}
			}
		}

		// Token: 0x060077DF RID: 30687 RVA: 0x0002FE8E File Offset: 0x0002E08E
		private static bool IsExposedPhotoObjective(MissionObjectiveKind kind)
		{
			return kind == MissionObjectiveKind.NpcPhotosTopless || kind == MissionObjectiveKind.NpcPhotosNaked;
		}

		// Token: 0x060077E0 RID: 30688 RVA: 0x0030D898 File Offset: 0x0030BA98
		public static void SetPlaceCapabilities(IEnumerable<MissionObjectiveKind> kinds)
		{
			MissionService.PlaceCapableKinds.Clear();
			if (kinds == null)
			{
				return;
			}
			foreach (MissionObjectiveKind missionObjectiveKind in kinds)
			{
				if (MissionRankPolicy.IsPlaceObjective(missionObjectiveKind))
				{
					MissionService.PlaceCapableKinds.Add(missionObjectiveKind);
				}
			}
		}

		// Token: 0x060077E1 RID: 30689 RVA: 0x0030D8FC File Offset: 0x0030BAFC
		public static void SetWallCoverAvailable(bool available)
		{
			MissionService._wallCoverAvailable = available;
		}

		// Token: 0x060077E2 RID: 30690 RVA: 0x0030D904 File Offset: 0x0030BB04
		public static bool HasActiveRunMission(MissionObjectiveKind kind)
		{
			if (!MissionService.AreRunMissionsEnabled)
			{
				return false;
			}
			for (int i = 0; i < MissionService.RunStates.Count; i++)
			{
				if (MissionService.RunStates[i] != null && MissionService.RunStates[i].kind == kind && !MissionService.RunStates[i].IsComplete)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060077E3 RID: 30691 RVA: 0x0030D964 File Offset: 0x0030BB64
		private static bool IsRunWideObjective(MissionState state)
		{
			return state.rankSlot || (MissionService.IsHomeReturnObjective(state.kind) && !state.IsComplete && RunModeService.CurrentMode == RunModeKind.Classic);
		}

		// Token: 0x060077E4 RID: 30692 RVA: 0x0030D990 File Offset: 0x0030BB90
		public static void RegisterAvailableEmotes(IEnumerable<MissionOption> options)
		{
			if (options == null)
			{
				return;
			}
			using (IEnumerator<MissionOption> enumerator = options.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MissionOption option = enumerator.Current;
					if (!string.IsNullOrWhiteSpace(option.Id))
					{
						int num = MissionService.AvailableEmotes.FindIndex((MissionOption e) => string.Equals(e.Id, option.Id, StringComparison.OrdinalIgnoreCase));
						if (num >= 0)
						{
							MissionService.AvailableEmotes[num] = option;
						}
						else
						{
							MissionService.AvailableEmotes.Add(option);
						}
					}
				}
			}
		}

		// Token: 0x060077E5 RID: 30693 RVA: 0x0030DA30 File Offset: 0x0030BC30
		public static void SetAvailableEmotes(IEnumerable<MissionOption> options)
		{
			MissionService.AvailableEmotes.Clear();
			MissionService.RegisterAvailableEmotes(options);
		}

		// Token: 0x060077E6 RID: 30694 RVA: 0x0030DA44 File Offset: 0x0030BC44
		public static void RefreshAvailableMissions()
		{
			if (!MissionService.AreRunMissionsEnabled)
			{
				return;
			}
			List<MissionObjectiveKind> list = MissionService.BuildPool(MissionScope.Run, MissionService._runContextId);
			bool flag = false;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			for (int i = 0; i < MissionService.RunStates.Count; i++)
			{
				MissionState missionState = MissionService.RunStates[i];
				if (missionState == null || !MissionService.IsHomeReturnObjective(missionState.kind) || RunModeService.CurrentMode != RunModeKind.Classic)
				{
					bool flag2 = missionState != null && (missionState.rankSlot ? (MissionRankPolicy.IsAvailable(missionState.kind) && MissionService.RankCapableKinds.Contains(missionState.kind)) : (list.Contains(missionState.kind) && MissionService.HasAvailableQualifier(missionState)));
					string text = ((missionState != null) ? missionState.id : null) ?? string.Empty;
					if (flag2)
					{
						MissionService.UnavailableSince.Remove(text);
					}
					else
					{
						if (missionState != null && MissionRankPolicy.IsAvailable(missionState.kind))
						{
							bool flag3 = missionState.progress > 0f;
							if (flag3 && !MissionService.CanLoseQualifierForGood(missionState))
							{
								goto IL_0190;
							}
							float num;
							if (!MissionService.UnavailableSince.TryGetValue(text, out num))
							{
								MissionService.UnavailableSince[text] = realtimeSinceStartup;
								goto IL_0190;
							}
							if (realtimeSinceStartup - num < (flag3 ? 120f : 15f))
							{
								goto IL_0190;
							}
						}
						MissionService.UnavailableSince.Remove(text);
						MissionState missionState2 = ((missionState != null && missionState.rankSlot) ? MissionService.GenerateRankMission(new MissionObjectiveKind?(missionState.kind)) : MissionService.TakeWaitingRunState());
						MissionService.RunStates[i] = missionState2 ?? MissionService.Generate(MissionScope.Run, MissionService._runContextId, MissionService._runContextName, MissionService.ActiveKinds(MissionService.RunStates, i));
						flag = true;
					}
				}
				IL_0190:;
			}
			if (MissionService.EnsureRankSlot())
			{
				flag = true;
			}
			if (MissionService.EnsurePlaceObjective())
			{
				flag = true;
			}
			if (flag)
			{
				Action changed = MissionService.Changed;
				if (changed == null)
				{
					return;
				}
				changed();
			}
		}

		// Token: 0x060077E7 RID: 30695 RVA: 0x0030DC1C File Offset: 0x0030BE1C
		private static bool EnsurePlaceObjective()
		{
			if (MissionService.AreRunMissionsEnabled && !string.IsNullOrEmpty(MissionService._runContextId) && !string.Equals(MissionService._runContextId, "CITY", StringComparison.Ordinal))
			{
				if (!MissionService.RunStates.Exists((MissionState state) => state != null && !state.IsComplete && MissionRankPolicy.IsPlaceObjective(state.kind)))
				{
					if (!MissionService.BuildPool(MissionScope.Run, MissionService._runContextId).Exists(new Predicate<MissionObjectiveKind>(MissionRankPolicy.IsPlaceObjective)))
					{
						return false;
					}
					int num = MissionService.RunStates.FindIndex((MissionState state) => state != null && !MissionService.IsRunWideObjective(state) && state.progress <= 0f);
					if (num < 0)
					{
						return false;
					}
					MissionService.UnavailableSince.Remove(MissionService.RunStates[num].id);
					MissionService.RunStates[num] = MissionService.Generate(MissionScope.Run, MissionService._runContextId, MissionService._runContextName, MissionService.ActiveKinds(MissionService.RunStates, num));
					return true;
				}
			}
			return false;
		}

		// Token: 0x060077E8 RID: 30696 RVA: 0x0030DD10 File Offset: 0x0030BF10
		private static bool CanLoseQualifierForGood(MissionState state)
		{
			return state.rankSlot || MissionRankPolicy.IsPlaceObjective(state.kind) || state.kind == MissionObjectiveKind.SpecificClothingWorn || state.kind == MissionObjectiveKind.SpecificClothingEmote || state.kind == MissionObjectiveKind.SpecificClothingExhibition || state.kind == MissionObjectiveKind.ClothingBroken || state.kind == MissionObjectiveKind.WearableSpecialAction;
		}

		// Token: 0x060077E9 RID: 30697 RVA: 0x0030DD68 File Offset: 0x0030BF68
		private static bool EnsureRankSlot()
		{
			if (MissionService.AreRunMissionsEnabled && !MissionService._rankSlotResting)
			{
				if (!MissionService.RunStates.Exists((MissionState state) => state != null && state.rankSlot))
				{
					if (MissionService.BuildRankPool(null).Count == 0)
					{
						return false;
					}
					int num = MissionService.RunStates.FindIndex((MissionState state) => state == null);
					if (num < 0 && MissionService.RunStates.Count >= 3)
					{
						if (MissionService.RunStates.FindAll((MissionState state) => !MissionService.IsRunWideObjective(state)).Count > 1)
						{
							num = MissionService.RunStates.FindIndex((MissionState state) => !MissionService.IsRunWideObjective(state) && state.progress <= 0f);
						}
						if (num < 0)
						{
							return false;
						}
					}
					MissionState missionState = MissionService.GenerateRankMission(null);
					if (missionState == null)
					{
						return false;
					}
					if (num >= 0)
					{
						MissionService.RunStates.RemoveAt(num);
					}
					MissionService.RunStates.Insert(0, missionState);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060077EA RID: 30698 RVA: 0x0030DE94 File Offset: 0x0030C094
		private static List<MissionObjectiveKind> BuildRankPool(MissionObjectiveKind? completed)
		{
			List<MissionObjectiveKind> list = new List<MissionObjectiveKind>();
			foreach (MissionObjectiveKind missionObjectiveKind in MissionRankPolicy.RankSpecialKinds)
			{
				if (MissionService.RankCapableKinds.Contains(missionObjectiveKind) && MissionRankPolicy.IsAvailable(missionObjectiveKind))
				{
					list.Add(missionObjectiveKind);
				}
			}
			if (completed != null && list.Count > 1)
			{
				int family = MissionRankPolicy.RankSpecialFamily(completed.Value);
				List<MissionObjectiveKind> list2 = list.FindAll((MissionObjectiveKind kind) => MissionRankPolicy.RankSpecialFamily(kind) != family);
				if (list2.Count > 0)
				{
					return list2;
				}
				list.Remove(completed.Value);
			}
			return list;
		}

		// Token: 0x060077EB RID: 30699 RVA: 0x0030DF38 File Offset: 0x0030C138
		private static MissionState GenerateRankMission(MissionObjectiveKind? completed)
		{
			if (!MissionService.AreRunMissionsEnabled)
			{
				return null;
			}
			List<MissionObjectiveKind> list = MissionService.BuildRankPool(completed);
			if (list.Count == 0)
			{
				return null;
			}
			MissionService.EnsureLoaded();
			MissionService.MissionSaveData save = MissionService._save;
			int num = save.generationCounter + 1;
			save.generationCounter = num;
			int num2 = num;
			Random random = new Random((MissionService._save.seed * 397) ^ (num2 * 7919));
			int rankIndex = (RankProgressionService.IsAdminRankActive ? (-1) : RankProgressionService.RankIndex);
			List<MissionObjectiveKind> list2 = list.FindAll((MissionObjectiveKind kind) => MissionRankPolicy.RankSpecialTier(kind) == rankIndex);
			List<MissionObjectiveKind> list3 = ((list2.Count > 0 && (list2.Count == list.Count || random.NextDouble() < 0.6)) ? list2 : list);
			List<int> list4 = new List<int>();
			foreach (MissionObjectiveKind missionObjectiveKind in list3)
			{
				int num3 = MissionRankPolicy.RankSpecialFamily(missionObjectiveKind);
				if (!list4.Contains(num3))
				{
					list4.Add(num3);
				}
			}
			int family = list4[random.Next(list4.Count)];
			List<MissionObjectiveKind> list5 = list3.FindAll((MissionObjectiveKind candidate) => MissionRankPolicy.RankSpecialFamily(candidate) == family);
			float num4 = 0f;
			foreach (MissionObjectiveKind missionObjectiveKind2 in list5)
			{
				num4 += MissionService.RankSpecialWeight(missionObjectiveKind2);
			}
			double num5 = random.NextDouble() * (double)num4;
			MissionObjectiveKind missionObjectiveKind3 = list5[list5.Count - 1];
			foreach (MissionObjectiveKind missionObjectiveKind4 in list5)
			{
				num5 -= (double)MissionService.RankSpecialWeight(missionObjectiveKind4);
				if (num5 < 0.0)
				{
					missionObjectiveKind3 = missionObjectiveKind4;
					break;
				}
			}
			int num6 = Mathf.Clamp(RankProgressionService.RankIndex, 0, 5);
			float num7;
			int num8;
			MissionService.ResolveDifficulty(MissionScope.Run, missionObjectiveKind3, random, num6, out num7, out num8);
			return new MissionState
			{
				id = Guid.NewGuid().ToString("N"),
				scope = MissionScope.Run,
				kind = missionObjectiveKind3,
				contextId = string.Empty,
				contextName = string.Empty,
				qualifierId = string.Empty,
				qualifierName = string.Empty,
				secondaryQualifierId = string.Empty,
				secondaryQualifierName = string.Empty,
				target = num7,
				progress = 0f,
				xpReward = num8,
				generation = num2,
				category = MissionRankPolicy.CategoryFor(missionObjectiveKind3),
				rankSlot = true,
				description = MissionService.BuildDescription(missionObjectiveKind3, num7, MissionScope.Run, string.Empty, string.Empty, string.Empty)
			};
		}

		// Token: 0x060077EC RID: 30700 RVA: 0x0030E22C File Offset: 0x0030C42C
		private static float RankSpecialWeight(MissionObjectiveKind kind)
		{
			if (kind != MissionObjectiveKind.FlashBothSeenTime)
			{
				return 1f;
			}
			return 0.5f;
		}

		// Token: 0x060077ED RID: 30701 RVA: 0x0030E23E File Offset: 0x0030C43E
		private static bool IsEmoteAvailable(MissionOption option, string contextId)
		{
			return (string.IsNullOrEmpty(option.ContextId) || string.Equals(option.ContextId, contextId, StringComparison.OrdinalIgnoreCase)) && (!option.IsMasturbation || MissionRankPolicy.IsAvailable(MissionObjectiveKind.Orgasms));
		}

		// Token: 0x060077EE RID: 30702 RVA: 0x0030E270 File Offset: 0x0030C470
		internal static bool IsStationEmote(MissionState mission)
		{
			return mission != null && mission.kind == MissionObjectiveKind.SpecificEmote && MissionService.AvailableEmotes.Exists((MissionOption e) => e.IsContextual && string.Equals(e.Id, mission.qualifierId, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x060077EF RID: 30703 RVA: 0x0030E2B8 File Offset: 0x0030C4B8
		private static bool HasAvailableQualifier(MissionState mission)
		{
			switch (mission.kind)
			{
			case MissionObjectiveKind.SpecificEmote:
			case MissionObjectiveKind.MovingEmoteDistance:
			case MissionObjectiveKind.SpecificEmoteExhibition:
			case MissionObjectiveKind.SpecificEmoteNearLandmark:
				return MissionService.AvailableEmotes.Exists((MissionOption e) => e.Id == mission.qualifierId && MissionService.IsEmoteAvailable(e, mission.contextId)) && (mission.kind != MissionObjectiveKind.SpecificEmoteNearLandmark || MissionService.AvailableLandmarks.Exists((MissionOption e) => e.Id == mission.secondaryQualifierId));
			case MissionObjectiveKind.SpecificClothingWorn:
			case MissionObjectiveKind.SpecificClothingExhibition:
			case MissionObjectiveKind.SpecificClothingEmote:
			case MissionObjectiveKind.ClothingBroken:
				return MissionService.AvailableWearables.Exists((MissionWearableOption e) => string.Equals(e.Id, mission.qualifierId, StringComparison.OrdinalIgnoreCase) && MissionService.IsWearableQualifierUsable(mission.kind, mission.scope, e));
			case MissionObjectiveKind.WearableSpecialAction:
				return MissionService.AvailableWearableActions.Exists((MissionWearableActionOption e) => e.WearableId == mission.qualifierId && e.ActionId == mission.secondaryQualifierId);
			case MissionObjectiveKind.EmoteNearLandmark:
				return MissionService.AvailableLandmarks.Exists((MissionOption e) => e.Id == mission.qualifierId);
			}
			return true;
		}

		// Token: 0x060077F0 RID: 30704 RVA: 0x0030E3C0 File Offset: 0x0030C5C0
		private static bool IsWearableQualifierUsable(MissionObjectiveKind kind, MissionScope scope, MissionWearableOption option)
		{
			switch (kind)
			{
			case MissionObjectiveKind.SpecificClothingWorn:
				return !option.IsWorn && (scope != MissionScope.Run || !MissionService._specificClothingWornThisRun.Contains(option.Id));
			case MissionObjectiveKind.SpecificClothingExhibition:
			case MissionObjectiveKind.SpecificClothingEmote:
				return !option.BlocksEmotes;
			case MissionObjectiveKind.ClothingBroken:
				return option.Breakable;
			}
			return true;
		}

		// Token: 0x060077F1 RID: 30705 RVA: 0x0030E424 File Offset: 0x0030C624
		private static List<MissionOption> PreferPlayable(List<MissionOption> choices)
		{
			List<MissionOption> list = choices.FindAll((MissionOption option) => option.PlayableNow);
			if (list.Count <= 0)
			{
				return choices;
			}
			return list;
		}

		// Token: 0x060077F2 RID: 30706 RVA: 0x0030E464 File Offset: 0x0030C664
		public static void SetAvailableWearables(IEnumerable<MissionWearableOption> options)
		{
			MissionService.AvailableWearables.Clear();
			if (options == null)
			{
				return;
			}
			using (IEnumerator<MissionWearableOption> enumerator = options.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MissionWearableOption option = enumerator.Current;
					if (!string.IsNullOrWhiteSpace(option.Id) && !MissionService.AvailableWearables.Exists((MissionWearableOption e) => string.Equals(e.Id, option.Id, StringComparison.OrdinalIgnoreCase)))
					{
						MissionService.AvailableWearables.Add(option);
					}
				}
			}
		}

		// Token: 0x060077F3 RID: 30707 RVA: 0x0030E4F8 File Offset: 0x0030C6F8
		public static void SetAvailableWearableActions(IEnumerable<MissionWearableActionOption> options)
		{
			MissionService.AvailableWearableActions.Clear();
			if (options == null)
			{
				return;
			}
			foreach (MissionWearableActionOption missionWearableActionOption in options)
			{
				if (!string.IsNullOrWhiteSpace(missionWearableActionOption.WearableId) && !string.IsNullOrWhiteSpace(missionWearableActionOption.ActionId))
				{
					MissionService.AvailableWearableActions.Add(missionWearableActionOption);
				}
			}
		}

		// Token: 0x060077F4 RID: 30708 RVA: 0x0030E56C File Offset: 0x0030C76C
		public static void SetAvailableLandmarks(IEnumerable<MissionOption> options)
		{
			MissionService.AvailableLandmarks.Clear();
			if (options == null)
			{
				return;
			}
			using (IEnumerator<MissionOption> enumerator = options.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MissionOption option = enumerator.Current;
					if (!string.IsNullOrWhiteSpace(option.Id) && !MissionService.AvailableLandmarks.Exists((MissionOption e) => string.Equals(e.Id, option.Id, StringComparison.OrdinalIgnoreCase)))
					{
						MissionService.AvailableLandmarks.Add(option);
					}
				}
			}
		}

		// Token: 0x060077F5 RID: 30709 RVA: 0x0030E600 File Offset: 0x0030C800
		public static void Report(MissionObjectiveKind kind, float amount, string qualifierId = null, string secondaryQualifierId = null, MissionScope? onlyScope = null)
		{
			if (!MissionService._runActive || amount <= 0f || float.IsNaN(amount) || float.IsInfinity(amount))
			{
				return;
			}
			if (MissionService.IsHomeReturnObjective(kind) && !MissionService._resolvingHomeReturn)
			{
				return;
			}
			if (!MissionService.TryRecordWardrobeItem(kind, qualifierId))
			{
				return;
			}
			if (kind == MissionObjectiveKind.ClothesCollected || kind == MissionObjectiveKind.ClothesWorn || kind == MissionObjectiveKind.SpecificClothingWorn)
			{
				amount = 1f;
			}
			MissionScope? missionScope = onlyScope;
			MissionScope missionScope2 = MissionScope.Run;
			if (!((missionScope.GetValueOrDefault() == missionScope2) & (missionScope != null)) && !RunModeService.IsFreeMode && !RankProgressionService.SuppressProgressionRewards)
			{
				MissionService.EnsureGlobalMissions();
				AchievementTrackingService.ReportMission(kind, amount, qualifierId);
				bool flag = false;
				if (kind != MissionObjectiveKind.PoiDiscoveries || (flag = MissionService.TryRecordDistinctAccountPoi(qualifierId)))
				{
					float num = ((kind == MissionObjectiveKind.PoiDiscoveries) ? 1f : amount);
					bool flag2 = MissionService.ApplyProgress(MissionService._save.globalMissions, MissionScope.Global, kind, num, qualifierId, secondaryQualifierId);
					if (flag && !flag2)
					{
						MissionService.Save();
					}
				}
			}
			missionScope = onlyScope;
			missionScope2 = MissionScope.Global;
			if (!((missionScope.GetValueOrDefault() == missionScope2) & (missionScope != null)) && MissionService.AreRunMissionsEnabled)
			{
				MissionService.ApplyProgress(MissionService.RunStates, MissionScope.Run, kind, amount, qualifierId, secondaryQualifierId);
			}
		}

		// Token: 0x060077F6 RID: 30710 RVA: 0x0030E700 File Offset: 0x0030C900
		private static bool TryRecordWardrobeItem(MissionObjectiveKind kind, string itemId)
		{
			HashSet<string> hashSet;
			switch (kind)
			{
			case MissionObjectiveKind.ClothesCollected:
				hashSet = MissionService._collectedClothingThisRun;
				goto IL_0035;
			case MissionObjectiveKind.ClothesWorn:
				hashSet = MissionService._wornClothingThisRun;
				goto IL_0035;
			case MissionObjectiveKind.SpecificClothingWorn:
				hashSet = MissionService._specificClothingWornThisRun;
				goto IL_0035;
			}
			return true;
			IL_0035:
			return !string.IsNullOrWhiteSpace(itemId) && hashSet.Add(itemId.Trim());
		}

		// Token: 0x060077F7 RID: 30711 RVA: 0x0030E758 File Offset: 0x0030C958
		private static void ResetWardrobeUniqueness()
		{
			MissionService._collectedClothingThisRun = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			MissionService._wornClothingThisRun = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			MissionService._specificClothingWornThisRun = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x060077F8 RID: 30712 RVA: 0x0030E787 File Offset: 0x0030C987
		public static bool IsHomeReturnObjective(MissionObjectiveKind kind)
		{
			return kind == MissionObjectiveKind.SafeHomeReturns || kind == MissionObjectiveKind.SafeHomeReturnDistance;
		}

		// Token: 0x060077F9 RID: 30713 RVA: 0x0030E798 File Offset: 0x0030C998
		internal static void ReportClassicHomeReturn(float peakMeters, out int completed, out float xp)
		{
			MissionService.<>c__DisplayClass117_0 CS$<>8__locals1 = new MissionService.<>c__DisplayClass117_0();
			completed = 0;
			xp = 0f;
			if (!MissionService._runActive || MissionService._homeReturnReported || RunModeService.CurrentMode != RunModeKind.Classic || ClassicHomeReturnPolicy.CalculateBonus(peakMeters) <= 0)
			{
				return;
			}
			MissionService._homeReturnReported = true;
			MissionService._resolvingHomeReturn = true;
			CS$<>8__locals1.count = 0;
			CS$<>8__locals1.awardedXp = 0f;
			MissionService.Completed += CS$<>8__locals1.<ReportClassicHomeReturn>g__Collect|0;
			try
			{
				MissionService.Report(MissionObjectiveKind.SafeHomeReturns, 1f, null, null, null);
				MissionService.Report(MissionObjectiveKind.SafeHomeReturnDistance, peakMeters, null, null, null);
				Action<float> classicHomeReturnReached = MissionService.ClassicHomeReturnReached;
				if (classicHomeReturnReached != null)
				{
					classicHomeReturnReached(peakMeters);
				}
			}
			finally
			{
				MissionService.Completed -= CS$<>8__locals1.<ReportClassicHomeReturn>g__Collect|0;
				MissionService._resolvingHomeReturn = false;
			}
			completed = CS$<>8__locals1.count;
			xp = CS$<>8__locals1.awardedXp;
		}

		// Token: 0x060077FA RID: 30714 RVA: 0x0030E87C File Offset: 0x0030CA7C
		private static bool TryRecordDistinctAccountPoi(string poiId)
		{
			if (string.IsNullOrWhiteSpace(poiId))
			{
				return false;
			}
			string text = poiId.Trim();
			for (int i = 0; i < MissionService._save.discoveredAccountPoiIds.Count; i++)
			{
				if (string.Equals(MissionService._save.discoveredAccountPoiIds[i], text, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
			}
			MissionService._save.discoveredAccountPoiIds.Add(text);
			return true;
		}

		// Token: 0x060077FB RID: 30715 RVA: 0x0030E8E0 File Offset: 0x0030CAE0
		public static void ResetAllForDebug()
		{
			RunModeService.DisqualifyLeaderboardRun("Missions were reset with a debug control.");
			MissionService._save = new MissionService.MissionSaveData
			{
				schemaVersion = 8,
				seed = MissionService.NewSeed()
			};
			MissionService._completedClaimsSeeded = false;
			MissionService.RunStates.Clear();
			MissionService.RunCompleted.Clear();
			MissionService.ParkedRunStates.Clear();
			MissionService.UnavailableSince.Clear();
			MissionService.PlaceKindsDoneThisVisit.Clear();
			MissionService._rankSlotResting = false;
			MissionService._lastRankKind = null;
			MissionService._runActive = false;
			MissionService.ResetWardrobeUniqueness();
			MissionService.EnsureGlobalMissions();
			MissionService.Save();
			Action changed = MissionService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060077FC RID: 30716 RVA: 0x0030E980 File Offset: 0x0030CB80
		private static bool ApplyProgress(List<MissionState> states, MissionScope scope, MissionObjectiveKind kind, float amount, string qualifierId, string secondaryQualifierId)
		{
			bool flag = false;
			for (int i = 0; i < states.Count; i++)
			{
				MissionState missionState = states[i];
				if (missionState != null && missionState.kind == kind && !missionState.IsComplete && (string.IsNullOrEmpty(missionState.contextId) || string.Equals(missionState.contextId, MissionService._runContextId, StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrEmpty(missionState.qualifierId) || string.Equals(missionState.qualifierId, qualifierId, StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrEmpty(missionState.secondaryQualifierId) || string.Equals(missionState.secondaryQualifierId, secondaryQualifierId, StringComparison.OrdinalIgnoreCase)))
				{
					MissionService.NormalizePendingCoverReward(missionState);
					float num = Mathf.Min(missionState.target, MissionRankPolicy.IsBestAttempt(kind) ? Mathf.Max(missionState.progress, amount) : (missionState.progress + amount));
					if (num > missionState.progress)
					{
						missionState.progress = num;
						flag = true;
						Action<MissionState> progressed = MissionService.Progressed;
						if (progressed != null)
						{
							progressed(missionState.Clone());
						}
						if (missionState.IsComplete)
						{
							MissionState missionState2 = missionState.Clone();
							if (scope == MissionScope.Run && MissionRankPolicy.IsPlaceObjective(missionState2.kind))
							{
								MissionService.PlaceKindsDoneThisVisit.Add(missionState2.kind);
							}
							XpGrantOutcome xpGrantOutcome = RankProgressionService.GrantMissionXpOnce(missionState2.id, (float)missionState2.xpReward);
							if (scope == MissionScope.Run)
							{
								MissionService.RunCompleted.Add(new MissionCompletion(missionState2, xpGrantOutcome));
							}
							if (!MissionService.ShouldRotateCompletedMission(scope) || MissionService._resolvingHomeReturn)
							{
								MissionService.Save();
								Action<MissionCompletion> completed = MissionService.Completed;
								if (completed != null)
								{
									completed(new MissionCompletion(missionState2, xpGrantOutcome));
								}
								Action changed = MissionService.Changed;
								if (changed != null)
								{
									changed();
								}
							}
							else
							{
								HashSet<MissionObjectiveKind> hashSet = MissionService.ActiveKinds(states, i);
								if (!missionState2.rankSlot)
								{
									hashSet.Add(missionState2.kind);
								}
								if (MissionService.IsWallCoverObjective(missionState2.kind))
								{
									hashSet.Add(MissionObjectiveKind.WallCoverEntries);
									hashSet.Add(MissionObjectiveKind.WallCoverTime);
								}
								MissionState missionState3 = null;
								if (scope == MissionScope.Run && missionState2.rankSlot)
								{
									MissionService._rankSlotResting = true;
									MissionService._rankRestCompletions = 0;
									MissionService._lastRankKind = new MissionObjectiveKind?(missionState2.kind);
								}
								else if (scope == MissionScope.Run)
								{
									if (!states.Exists((MissionState state) => state != null && state.rankSlot) && (!MissionService._rankSlotResting || ++MissionService._rankRestCompletions >= 2 || !MissionService.HasEmoteObjective(states, i)))
									{
										missionState3 = MissionService.GenerateRankMission(MissionService._lastRankKind);
										if (missionState3 != null)
										{
											MissionService._rankSlotResting = false;
										}
									}
								}
								int num2 = i;
								MissionState missionState4;
								if ((missionState4 = missionState3) == null && (missionState4 = ((scope == MissionScope.Run) ? MissionService.TakeWaitingRunState() : null)) == null)
								{
									missionState4 = MissionService.Generate(scope, (scope == MissionScope.Run) ? MissionService._runContextId : string.Empty, (scope == MissionScope.Run) ? MissionService._runContextName : string.Empty, hashSet);
								}
								states[num2] = missionState4;
								if (scope == MissionScope.Global)
								{
									MissionService.Save();
								}
								Action<MissionCompletion> completed2 = MissionService.Completed;
								if (completed2 != null)
								{
									completed2(new MissionCompletion(missionState2, xpGrantOutcome));
								}
								Action changed2 = MissionService.Changed;
								if (changed2 != null)
								{
									changed2();
								}
							}
						}
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			if (scope == MissionScope.Global)
			{
				MissionService.Save();
			}
			Action changed3 = MissionService.Changed;
			if (changed3 != null)
			{
				changed3();
			}
			return true;
		}

		// Token: 0x060077FD RID: 30717 RVA: 0x0030EC9C File Offset: 0x0030CE9C
		private static void RegenerateRunSet()
		{
			List<MissionState> list = MissionService.RunStates.FindAll((MissionState state) => state != null && MissionService.IsRunWideObjective(state));
			MissionService.RunStates.Clear();
			MissionService.RunStates.AddRange(list);
			MissionService.FillRunSet();
			Action changed = MissionService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060077FE RID: 30718 RVA: 0x0030ECFC File Offset: 0x0030CEFC
		private static void FillRunSet()
		{
			if (!MissionService.AreRunMissionsEnabled)
			{
				return;
			}
			if (!MissionService._rankSlotResting && MissionService.RunStates.Count < 3)
			{
				if (!MissionService.RunStates.Exists((MissionState state) => state != null && state.rankSlot))
				{
					MissionState missionState = MissionService.GenerateRankMission(null);
					if (missionState != null)
					{
						MissionService.RunStates.Insert(0, missionState);
					}
				}
			}
			while (MissionService.RunStates.Count < 3)
			{
				MissionService.RunStates.Add(MissionService.Generate(MissionScope.Run, MissionService._runContextId, MissionService._runContextName, MissionService.ActiveKinds(MissionService.RunStates, -1)));
			}
		}

		// Token: 0x060077FF RID: 30719 RVA: 0x0030EDA4 File Offset: 0x0030CFA4
		private static MissionState Generate(MissionScope scope, string contextId, string contextName, HashSet<MissionObjectiveKind> blocked)
		{
			MissionService.EnsureLoaded();
			MissionService.MissionSaveData save = MissionService._save;
			int num = save.generationCounter + 1;
			save.generationCounter = num;
			int num2 = num;
			Random random = new Random((MissionService._save.seed * 397) ^ (num2 * 7919));
			List<MissionObjectiveKind> list = MissionService.BuildPool(scope, contextId);
			List<MissionObjectiveKind> list2 = new List<MissionObjectiveKind>(list);
			list2.RemoveAll(new Predicate<MissionObjectiveKind>(blocked.Contains));
			if (list2.Count == 0 && scope == MissionScope.Run)
			{
				HashSet<MissionObjectiveKind> onBoard = MissionService.ActiveKinds(MissionService.RunStates, -1);
				list2 = list.FindAll((MissionObjectiveKind candidate) => !onBoard.Contains(candidate));
			}
			if (list2.Count == 0)
			{
				list2 = list;
			}
			if (scope == MissionScope.Run)
			{
				if (MissionService.RunStates.Exists((MissionState state) => state != null && MissionService.IsHomeReturnObjective(state.kind) && !state.IsComplete))
				{
					List<MissionObjectiveKind> list3 = list2.FindAll((MissionObjectiveKind candidate) => !MissionService.IsHomeReturnObjective(candidate));
					if (list3.Count > 0)
					{
						list2 = list3;
					}
				}
			}
			bool flag = false;
			if (scope == MissionScope.Run && !string.IsNullOrEmpty(contextId) && !string.Equals(contextId, "CITY", StringComparison.Ordinal))
			{
				if (!MissionService.RunStates.Exists((MissionState state) => state != null && !state.IsComplete && MissionRankPolicy.IsPlaceObjective(state.kind)))
				{
					List<MissionObjectiveKind> list4 = list2.FindAll(new Predicate<MissionObjectiveKind>(MissionRankPolicy.IsPlaceObjective));
					if (list4.Count > 0)
					{
						list2 = list4;
						flag = true;
					}
				}
			}
			MissionObjectiveKind kind = ((scope == MissionScope.Run && !flag) ? MissionService.PickVaried(MissionService.PreferVariety(list2, blocked), random) : list2[random.Next(list2.Count)]);
			string generatedContextId = contextId ?? string.Empty;
			string text = contextName ?? string.Empty;
			if (MissionService.IsHomeReturnObjective(kind))
			{
				generatedContextId = string.Empty;
				text = string.Empty;
			}
			if (string.Equals(MissionRankPolicy.PlaceContext(kind), "CITY", StringComparison.Ordinal))
			{
				text = string.Empty;
			}
			string text2 = string.Empty;
			string text3 = string.Empty;
			string text4 = string.Empty;
			string text5 = string.Empty;
			bool flag2 = false;
			if (kind == MissionObjectiveKind.SpecificEmote || kind == MissionObjectiveKind.SpecificEmoteExhibition || kind == MissionObjectiveKind.MovingEmoteDistance)
			{
				List<MissionOption> list5 = MissionService.PreferPlayable(MissionService.AvailableEmotes.FindAll((MissionOption e) => (kind != MissionObjectiveKind.MovingEmoteDistance || e.IsMoving) && MissionService.IsEmoteAvailable(e, generatedContextId)));
				if (list5.Count > 0)
				{
					MissionOption missionOption = list5[random.Next(list5.Count)];
					text2 = missionOption.Id;
					text3 = missionOption.Name;
					if (scope == MissionScope.Global && !string.IsNullOrEmpty(missionOption.ContextId))
					{
						generatedContextId = missionOption.ContextId;
						text = missionOption.ContextName;
					}
				}
			}
			else if (kind == MissionObjectiveKind.SpecificEmoteNearLandmark)
			{
				if (MissionService.AvailableLandmarks.Count > 0)
				{
					MissionOption missionOption2 = MissionService.AvailableLandmarks[random.Next(MissionService.AvailableLandmarks.Count)];
					text4 = missionOption2.Id;
					text5 = missionOption2.Name;
				}
				List<MissionOption> list6 = MissionService.PreferPlayable(MissionService.AvailableEmotes.FindAll((MissionOption e) => !e.IsContextual && MissionService.IsEmoteAvailable(e, generatedContextId)));
				if (list6.Count > 0)
				{
					MissionOption missionOption3 = list6[random.Next(list6.Count)];
					text2 = missionOption3.Id;
					text3 = missionOption3.Name;
				}
				if (scope == MissionScope.Global && MissionService._runActive)
				{
					generatedContextId = MissionService._runContextId;
					text = MissionService._runContextName;
				}
			}
			else if (kind == MissionObjectiveKind.SpecificClothingWorn || kind == MissionObjectiveKind.SpecificClothingExhibition || kind == MissionObjectiveKind.SpecificClothingEmote || kind == MissionObjectiveKind.ClothingBroken)
			{
				List<MissionWearableOption> list7 = MissionService.AvailableWearables.FindAll((MissionWearableOption e) => MissionService.IsWearableQualifierUsable(kind, scope, e));
				if (list7.Count > 0)
				{
					MissionWearableOption missionWearableOption = list7[random.Next(list7.Count)];
					text2 = missionWearableOption.Id;
					text3 = missionWearableOption.Name;
				}
			}
			else if (kind == MissionObjectiveKind.WearableSpecialAction)
			{
				if (MissionService.AvailableWearableActions.Count > 0)
				{
					MissionWearableActionOption missionWearableActionOption = MissionService.AvailableWearableActions[random.Next(MissionService.AvailableWearableActions.Count)];
					text2 = missionWearableActionOption.WearableId;
					text3 = missionWearableActionOption.WearableName;
					text4 = missionWearableActionOption.ActionId;
					text5 = missionWearableActionOption.ActionName;
					flag2 = missionWearableActionOption.SingleUse;
				}
			}
			else if (kind == MissionObjectiveKind.EmoteNearLandmark)
			{
				if (MissionService.AvailableLandmarks.Count > 0)
				{
					MissionOption missionOption4 = MissionService.AvailableLandmarks[random.Next(MissionService.AvailableLandmarks.Count)];
					text2 = missionOption4.Id;
					text3 = missionOption4.Name;
					if (scope == MissionScope.Global && MissionService._runActive)
					{
						generatedContextId = MissionService._runContextId;
						text = MissionService._runContextName;
					}
				}
			}
			else if (scope == MissionScope.Global && MissionService.CanBePoiSpecific(kind) && random.NextDouble() < 0.3)
			{
				MissionOption missionOption5 = MissionService.KnownPoiContexts[random.Next(MissionService.KnownPoiContexts.Length)];
				generatedContextId = missionOption5.Id;
				text = missionOption5.Name;
			}
			int num3 = Mathf.Clamp(RankProgressionService.RankIndex, 0, 5);
			float num4;
			int num5;
			MissionService.ResolveDifficulty(scope, kind, random, num3, out num4, out num5);
			if (flag2 && num4 > 1f)
			{
				num4 = 1f;
				num5 = MissionService.FinishXp((scope == MissionScope.Global) ? 875 : 395);
			}
			return new MissionState
			{
				id = Guid.NewGuid().ToString("N"),
				scope = scope,
				kind = kind,
				contextId = generatedContextId,
				contextName = text,
				qualifierId = text2,
				qualifierName = text3,
				secondaryQualifierId = text4,
				secondaryQualifierName = text5,
				target = num4,
				progress = 0f,
				xpReward = num5,
				generation = num2,
				category = MissionRankPolicy.CategoryFor(kind),
				description = MissionService.BuildDescription(kind, num4, scope, text, text3, text5)
			};
		}

		// Token: 0x06007800 RID: 30720 RVA: 0x0030F400 File Offset: 0x0030D600
		private static List<MissionObjectiveKind> PreferVariety(List<MissionObjectiveKind> pool, HashSet<MissionObjectiveKind> blocked)
		{
			HashSet<MissionVarietyFamily> taken = new HashSet<MissionVarietyFamily>();
			bool emoteTaken = false;
			foreach (MissionObjectiveKind missionObjectiveKind in blocked)
			{
				taken.Add(MissionRankPolicy.VarietyFamily(missionObjectiveKind));
				emoteTaken |= MissionRankPolicy.IsEmoteLike(missionObjectiveKind);
			}
			List<MissionObjectiveKind> list = pool.FindAll((MissionObjectiveKind kind) => !taken.Contains(MissionRankPolicy.VarietyFamily(kind)) && (!emoteTaken || !MissionRankPolicy.IsEmoteLike(kind)));
			if (list.Count > 0)
			{
				return list;
			}
			list = pool.FindAll((MissionObjectiveKind kind) => !taken.Contains(MissionRankPolicy.VarietyFamily(kind)));
			if (list.Count <= 0)
			{
				return pool;
			}
			return list;
		}

		// Token: 0x06007801 RID: 30721 RVA: 0x0030F4C4 File Offset: 0x0030D6C4
		private static bool HasEmoteObjective(List<MissionState> states, int exceptIndex)
		{
			for (int i = 0; i < states.Count; i++)
			{
				if (i != exceptIndex && states[i] != null && !states[i].IsComplete && !MissionRankPolicy.IsPlaceObjective(states[i].kind) && MissionRankPolicy.IsEmoteLike(states[i].kind))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007802 RID: 30722 RVA: 0x0030F528 File Offset: 0x0030D728
		private static MissionObjectiveKind PickVaried(List<MissionObjectiveKind> pool, Random random)
		{
			List<MissionVarietyFamily> list = new List<MissionVarietyFamily>();
			float num = 0f;
			foreach (MissionObjectiveKind missionObjectiveKind in pool)
			{
				MissionVarietyFamily missionVarietyFamily = MissionRankPolicy.VarietyFamily(missionObjectiveKind);
				if (!list.Contains(missionVarietyFamily))
				{
					list.Add(missionVarietyFamily);
					num += MissionRankPolicy.VarietyWeight(missionVarietyFamily);
				}
			}
			double num2 = random.NextDouble() * (double)num;
			MissionVarietyFamily chosen = list[list.Count - 1];
			foreach (MissionVarietyFamily missionVarietyFamily2 in list)
			{
				num2 -= (double)MissionRankPolicy.VarietyWeight(missionVarietyFamily2);
				if (num2 < 0.0)
				{
					chosen = missionVarietyFamily2;
					break;
				}
			}
			List<MissionObjectiveKind> list2 = pool.FindAll((MissionObjectiveKind kind) => MissionRankPolicy.VarietyFamily(kind) == chosen);
			return list2[random.Next(list2.Count)];
		}

		// Token: 0x06007803 RID: 30723 RVA: 0x0030F644 File Offset: 0x0030D844
		private static List<MissionObjectiveKind> BuildPool(MissionScope scope, string contextId)
		{
			List<MissionObjectiveKind> list = new List<MissionObjectiveKind>
			{
				MissionObjectiveKind.NpcPhotos,
				MissionObjectiveKind.SuccessfulExhibitions,
				MissionObjectiveKind.Escapes,
				MissionObjectiveKind.ScoreEarned,
				MissionObjectiveKind.EmotesPerformed,
				MissionObjectiveKind.Orgasms
			};
			if (scope == MissionScope.Run)
			{
				foreach (MissionObjectiveKind missionObjectiveKind in new MissionObjectiveKind[]
				{
					MissionObjectiveKind.NpcPhotosTopless,
					MissionObjectiveKind.NpcPhotosNaked
				})
				{
					if (MissionService.RankCapableKinds.Contains(missionObjectiveKind))
					{
						list.Add(missionObjectiveKind);
					}
				}
			}
			bool flag = MissionService.AvailableEmotes.Exists((MissionOption e) => MissionService.IsEmoteAvailable(e, contextId));
			if (flag)
			{
				list.Add(MissionObjectiveKind.SpecificEmote);
				list.Add(MissionObjectiveKind.SpecificEmoteExhibition);
			}
			if (MissionService.AvailableEmotes.Exists((MissionOption e) => e.IsMoving && MissionService.IsEmoteAvailable(e, contextId)))
			{
				list.Add(MissionObjectiveKind.MovingEmoteDistance);
			}
			if (MissionService.AvailableWearables.Exists((MissionWearableOption e) => MissionService.IsWearableQualifierUsable(MissionObjectiveKind.SpecificClothingWorn, scope, e)))
			{
				list.Add(MissionObjectiveKind.SpecificClothingWorn);
			}
			if (MissionService.AvailableWearables.Exists((MissionWearableOption e) => MissionService.IsWearableQualifierUsable(MissionObjectiveKind.SpecificClothingEmote, scope, e)))
			{
				list.Add(MissionObjectiveKind.SpecificClothingExhibition);
				list.Add(MissionObjectiveKind.SpecificClothingEmote);
			}
			if (MissionService.AvailableWearables.Exists((MissionWearableOption e) => e.Breakable))
			{
				list.Add(MissionObjectiveKind.ClothingBroken);
			}
			if (MissionService.AvailableWearableActions.Count > 0)
			{
				list.Add(MissionObjectiveKind.WearableSpecialAction);
			}
			if (MissionService.AvailableLandmarks.Count > 0 && flag)
			{
				list.Add(MissionObjectiveKind.EmoteNearLandmark);
				list.Add(MissionObjectiveKind.SpecificEmoteNearLandmark);
			}
			if (!MissionService._wallCoverAvailable)
			{
				if (!MissionService.AvailableLandmarks.Exists((MissionOption e) => string.Equals(e.Id, "COVER_WALL", StringComparison.OrdinalIgnoreCase)))
				{
					goto IL_01C9;
				}
			}
			list.Add(MissionObjectiveKind.WallCoverEntries);
			list.Add(MissionObjectiveKind.WallCoverTime);
			IL_01C9:
			if (scope == MissionScope.Run && !string.IsNullOrEmpty(contextId))
			{
				foreach (MissionObjectiveKind missionObjectiveKind2 in MissionRankPolicy.PlaceKinds)
				{
					if (MissionService.PlaceCapableKinds.Contains(missionObjectiveKind2) && !MissionService.PlaceKindsDoneThisVisit.Contains(missionObjectiveKind2) && string.Equals(MissionRankPolicy.PlaceContext(missionObjectiveKind2), contextId, StringComparison.Ordinal))
					{
						list.Add(missionObjectiveKind2);
					}
				}
			}
			if (scope != MissionScope.Run || string.IsNullOrEmpty(contextId) || string.Equals(contextId, "CITY", StringComparison.Ordinal))
			{
				if (RunModeService.CurrentMode == RunModeKind.Classic)
				{
					list.Add(MissionObjectiveKind.SafeHomeReturns);
					list.Add(MissionObjectiveKind.SafeHomeReturnDistance);
				}
				list.Add(MissionObjectiveKind.CarSurfs);
				list.Add(MissionObjectiveKind.VehicleDistance);
				list.Add(MissionObjectiveKind.TrashcanSearches);
				list.Add(MissionObjectiveKind.TrashcanLootItems);
				list.Add(MissionObjectiveKind.ItemsCollected);
				list.Add(MissionObjectiveKind.ClothesCollected);
				list.Add(MissionObjectiveKind.RareLootCollected);
			}
			if (scope == MissionScope.Global)
			{
				list.Add(MissionObjectiveKind.PoiDiscoveries);
			}
			list.RemoveAll((MissionObjectiveKind kind) => !MissionRankPolicy.IsAvailable(kind));
			if (!flag)
			{
				list.Remove(MissionObjectiveKind.EmotesPerformed);
				list.Remove(MissionObjectiveKind.SuccessfulExhibitions);
				list.Remove(MissionObjectiveKind.SpecificClothingEmote);
				list.Remove(MissionObjectiveKind.SpecificClothingExhibition);
				list.Remove(MissionObjectiveKind.StadiumScreenEmotes);
				list.Remove(MissionObjectiveKind.SeatedStudentsEmoteAudience);
			}
			if (!MissionService.AvailableEmotes.Exists((MissionOption e) => e.IsMasturbation && MissionService.IsEmoteAvailable(e, contextId)))
			{
				list.Remove(MissionObjectiveKind.Orgasms);
			}
			return list;
		}

		// Token: 0x06007804 RID: 30724 RVA: 0x0030F994 File Offset: 0x0030DB94
		private static void ResolveDifficulty(MissionScope scope, MissionObjectiveKind kind, Random random, int band, out float target, out int xp)
		{
			bool flag = scope == MissionScope.Global;
			if (MissionService.TryResolveRankSpecialDifficulty(kind, random, band, out target, out xp) || MissionService.TryResolvePlaceDifficulty(kind, random, band, out target, out xp))
			{
				xp = MissionService.FinishXp(xp);
				if (!flag && kind == MissionObjectiveKind.NakedSwimDistance)
				{
					target *= 0.5f;
				}
				return;
			}
			switch (kind)
			{
			case MissionObjectiveKind.NpcPhotos:
				target = (float)(random.Next(flag ? 3 : 1, flag ? 8 : 4) + band / 2);
				xp = Mathf.RoundToInt((float)(flag ? 420 : 140) + target * (float)(flag ? 95 : 70));
				goto IL_0831;
			case MissionObjectiveKind.SuccessfulExhibitions:
				target = (float)(random.Next(flag ? 3 : 1, flag ? 7 : 4) + band / 2);
				xp = Mathf.RoundToInt((float)(flag ? 400 : 150) + target * (float)(flag ? 105 : 80));
				goto IL_0831;
			case MissionObjectiveKind.PoiDiscoveries:
				target = (float)random.Next(2, 5);
				xp = Mathf.RoundToInt(650f + target * 170f);
				goto IL_0831;
			case MissionObjectiveKind.CarSurfs:
				target = (float)random.Next(flag ? 2 : 1, flag ? 5 : 3);
				xp = Mathf.RoundToInt((float)(flag ? 600 : 190) + target * (float)(flag ? 180 : 100));
				goto IL_0831;
			case MissionObjectiveKind.Escapes:
				target = (float)(random.Next(flag ? 3 : 1, flag ? 7 : 4) + band / 3);
				xp = Mathf.RoundToInt((float)(flag ? 500 : 180) + target * (float)(flag ? 125 : 95));
				goto IL_0831;
			case MissionObjectiveKind.VehicleDistance:
				if (!flag)
				{
					RunModeKind currentMode = RunModeService.CurrentMode;
					RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
					if (GuidedRankGameplayPolicy.ShouldBlockRideableVehicles(currentMode, (currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive))
					{
						target = (float)random.Next(1, 3) * 100f;
						goto IL_0326;
					}
				}
				target = (float)(random.Next(flag ? 4 : 2, flag ? 11 : 6) + band) * 100f;
				IL_0326:
				xp = Mathf.RoundToInt((float)(flag ? 400 : 150) + target * 0.65f);
				if (!flag)
				{
					target *= 0.5f;
					goto IL_0831;
				}
				goto IL_0831;
			case MissionObjectiveKind.ScoreEarned:
				break;
			case MissionObjectiveKind.EmotesPerformed:
				target = (float)(random.Next(flag ? 4 : 2, flag ? 10 : 5) + band / 3);
				xp = Mathf.RoundToInt((float)(flag ? 430 : 150) + target * (float)(flag ? 90 : 70));
				goto IL_0831;
			case MissionObjectiveKind.SpecificEmote:
				target = (float)random.Next(flag ? 2 : 1, flag ? 5 : 3);
				xp = Mathf.RoundToInt((float)(flag ? 520 : 185) + target * (float)(flag ? 120 : 90));
				goto IL_0831;
			case MissionObjectiveKind.Orgasms:
				target = (float)random.Next(flag ? 2 : 1, flag ? 5 : 3);
				xp = Mathf.RoundToInt((float)(flag ? 600 : 220) + target * (float)(flag ? 175 : 120));
				goto IL_0831;
			case MissionObjectiveKind.TrashcanSearches:
				target = (float)(random.Next(flag ? 3 : 1, flag ? 8 : 4) + band / 4);
				xp = Mathf.RoundToInt((float)(flag ? 450 : 160) + target * (float)(flag ? 105 : 75));
				goto IL_0831;
			case MissionObjectiveKind.TrashcanLootItems:
				target = (float)(random.Next(flag ? 5 : 2, flag ? 13 : 6) + band / 3);
				xp = Mathf.RoundToInt((float)(flag ? 500 : 170) + target * (float)(flag ? 75 : 55));
				goto IL_0831;
			case MissionObjectiveKind.ItemsCollected:
			case MissionObjectiveKind.ClothesCollected:
				target = (float)(random.Next(flag ? 3 : 1, flag ? 9 : 4) + band / 4);
				xp = Mathf.RoundToInt((float)(flag ? 475 : 165) + target * (float)(flag ? 100 : 75));
				goto IL_0831;
			case MissionObjectiveKind.RareLootCollected:
				target = (float)random.Next(flag ? 2 : 1, flag ? 5 : 3);
				xp = Mathf.RoundToInt((float)(flag ? 650 : 240) + target * (float)(flag ? 190 : 130));
				goto IL_0831;
			case MissionObjectiveKind.ClothesWorn:
				target = (float)random.Next(flag ? 2 : 1, flag ? 6 : 3);
				xp = Mathf.RoundToInt((float)(flag ? 500 : 180) + target * (float)(flag ? 130 : 95));
				goto IL_0831;
			case MissionObjectiveKind.SpecificClothingWorn:
				target = 1f;
				xp = (flag ? 650 : 250);
				goto IL_0831;
			case MissionObjectiveKind.SpecificClothingExhibition:
			case MissionObjectiveKind.SpecificClothingEmote:
				target = (float)random.Next(flag ? 2 : 1, flag ? 5 : 3);
				xp = Mathf.RoundToInt((float)(flag ? 625 : 235) + target * (float)(flag ? 155 : 105));
				goto IL_0831;
			case MissionObjectiveKind.WearableSpecialAction:
				target = (float)random.Next(flag ? 2 : 1, flag ? 5 : 3);
				xp = Mathf.RoundToInt((float)(flag ? 700 : 275) + target * (float)(flag ? 175 : 120));
				goto IL_0831;
			case MissionObjectiveKind.ClothingBroken:
				target = 1f;
				xp = (flag ? 850 : 325);
				goto IL_0831;
			case MissionObjectiveKind.MovingEmoteDistance:
				target = (float)random.Next(flag ? 3 : 1, flag ? 9 : 4) * 10f;
				xp = Mathf.RoundToInt((float)(flag ? 600 : 220) + target * (flag ? 7f : 5f));
				if (!flag)
				{
					target *= 0.5f;
					goto IL_0831;
				}
				goto IL_0831;
			case MissionObjectiveKind.SpecificEmoteExhibition:
				target = (float)random.Next(flag ? 2 : 1, flag ? 5 : 3);
				xp = Mathf.RoundToInt((float)(flag ? 650 : 250) + target * (float)(flag ? 170 : 115));
				goto IL_0831;
			case MissionObjectiveKind.WallCoverEntries:
				target = (float)random.Next(flag ? 3 : 1, flag ? 8 : 4);
				xp = MissionService.CoverRunReward(kind, target);
				return;
			case MissionObjectiveKind.WallCoverTime:
				target = (float)random.Next(flag ? 3 : 1, flag ? 8 : 4) * 10f;
				xp = MissionService.CoverRunReward(kind, target);
				return;
			case MissionObjectiveKind.EmoteNearLandmark:
				target = (float)random.Next(flag ? 2 : 1, flag ? 5 : 3);
				xp = Mathf.RoundToInt((float)(flag ? 575 : 225) + target * (float)(flag ? 145 : 100));
				goto IL_0831;
			case MissionObjectiveKind.SpecificEmoteNearLandmark:
				target = (float)random.Next(flag ? 2 : 1, flag ? 4 : 3);
				xp = Mathf.RoundToInt((float)(flag ? 700 : 275) + target * (float)(flag ? 175 : 125));
				goto IL_0831;
			case MissionObjectiveKind.SafeHomeReturns:
				target = 1f;
				xp = 300;
				goto IL_0831;
			case MissionObjectiveKind.SafeHomeReturnDistance:
			{
				int num = random.Next(1, 4);
				target = (float)(num + Mathf.Min(band, 5)) * 250f;
				xp = Mathf.RoundToInt(250f + target * 0.4f);
				if (!flag)
				{
					target = MissionService.RunHomeDistanceTarget(band, num);
					goto IL_0831;
				}
				goto IL_0831;
			}
			default:
				if (kind == MissionObjectiveKind.NpcPhotosTopless)
				{
					target = (float)(random.Next(1, 3) + band / 3);
					xp = Mathf.RoundToInt(200f + target * 85f);
					goto IL_0831;
				}
				if (kind == MissionObjectiveKind.NpcPhotosNaked)
				{
					target = (float)(random.Next(1, 3) + band / 4);
					xp = Mathf.RoundToInt(240f + target * 95f);
					goto IL_0831;
				}
				break;
			}
			target = (float)(random.Next(flag ? 4 : 2, flag ? 11 : 6) + band) * 500f;
			xp = Mathf.RoundToInt((float)(flag ? 350 : 130) + target * 0.11f);
			IL_0831:
			xp = MissionService.FinishXp(xp);
			if (kind == MissionObjectiveKind.ClothesCollected || kind == MissionObjectiveKind.ClothesWorn || kind == MissionObjectiveKind.SpecificClothingWorn)
			{
				xp = Mathf.Max(10, Mathf.RoundToInt((float)xp / 20f) * 5);
			}
		}

		// Token: 0x06007805 RID: 30725 RVA: 0x00310207 File Offset: 0x0030E407
		private static int FinishXp(int rawXp)
		{
			return Mathf.Max(50, Mathf.RoundToInt((float)rawXp / 25f) * 25) / 5;
		}

		// Token: 0x06007806 RID: 30726 RVA: 0x00310224 File Offset: 0x0030E424
		private static float RunHomeDistanceTarget(int band, int draw)
		{
			RankProgressionConfigSO config = RankProgressionService.Config;
			if (config == null || config.ranksInOrder == null || config.ranksInOrder.Count == 0)
			{
				return (float)(draw + Mathf.Min(band, 5)) * 125f;
			}
			int num = Mathf.Clamp(band + 1, 0, config.ranksInOrder.Count - 1);
			RankDefinitionSO rankDefinitionSO = config.ranksInOrder[num];
			if (rankDefinitionSO == null)
			{
				return (float)(draw + Mathf.Min(band, 5)) * 125f;
			}
			float num2 = Mathf.Max(100f, rankDefinitionSO.promotionRunDistance);
			float num3 = 0.25f * (float)(draw + 1);
			return Mathf.Clamp(Mathf.Round(num2 * num3 / 25f) * 25f, 100f, num2);
		}

		// Token: 0x06007807 RID: 30727 RVA: 0x003102E4 File Offset: 0x0030E4E4
		private static bool TryResolvePlaceDifficulty(MissionObjectiveKind kind, Random random, int band, out float target, out int xp)
		{
			bool flag = band >= 4;
			switch (kind)
			{
			case MissionObjectiveKind.StatuePoseSeenTime:
				target = (float)(random.Next(2, 5) * 5 + ((band >= 3) ? 5 : 0));
				xp = Mathf.RoundToInt(300f + target * 15f);
				return true;
			case MissionObjectiveKind.StatuePoseAudience:
				target = (float)random.Next(2, 5);
				xp = Mathf.RoundToInt(280f + target * 70f);
				return true;
			case MissionObjectiveKind.PaintedStatueSeenTime:
				target = (float)random.Next(5, 9);
				xp = Mathf.RoundToInt(420f + target * 35f);
				return true;
			case MissionObjectiveKind.StatuePoseCollection:
				target = 3f;
				xp = 400;
				return true;
			case MissionObjectiveKind.StadiumScreenExposedTime:
				target = ((band <= 1) ? 20f : ((band <= 3) ? 30f : 45f));
				xp = Mathf.RoundToInt(250f + target * 5f);
				return true;
			case MissionObjectiveKind.StadiumScreenEmotes:
				target = (float)Mathf.Clamp(band - 2, 1, 3);
				xp = Mathf.RoundToInt(230f + target * 100f);
				return true;
			case MissionObjectiveKind.SeatedStudentsEmoteAudience:
				target = ((band >= 5) ? 12f : (flag ? 8f : 5f));
				xp = ((band >= 5) ? 850 : (flag ? 650 : 500));
				return true;
			case MissionObjectiveKind.LedWallFaceBroadcast:
				target = 1f;
				xp = 625;
				return true;
			case MissionObjectiveKind.UniformSkirtSeenTime:
				target = (float)random.Next(6, 11);
				xp = Mathf.RoundToInt(330f + target * 30f);
				return true;
			case MissionObjectiveKind.KombiniToplessTheft:
				target = 1f;
				xp = 550;
				return true;
			case MissionObjectiveKind.TreadmillToplessTime:
				target = (float)(random.Next(2, 5) * 5);
				xp = Mathf.RoundToInt(300f + target * 20f);
				return true;
			case MissionObjectiveKind.GymShowerTime:
				target = (float)(random.Next(3, 7) * 5);
				xp = Mathf.RoundToInt(350f + target * 8f);
				return true;
			case MissionObjectiveKind.ShowerFoamSeenTime:
				target = (float)random.Next(5, 11);
				xp = Mathf.RoundToInt(400f + target * 30f);
				return true;
			case MissionObjectiveKind.NakedSwimDistance:
				target = (flag ? 80f : 40f);
				xp = Mathf.RoundToInt(300f + target * 4f);
				return true;
			case MissionObjectiveKind.PoolScreenToplessSeenTime:
				target = (float)random.Next(6, 13);
				xp = Mathf.RoundToInt(350f + target * 25f);
				return true;
			case MissionObjectiveKind.GrateWindSeenTime:
				target = (float)random.Next(4, 9);
				xp = Mathf.RoundToInt(330f + target * 30f);
				return true;
			case MissionObjectiveKind.SubwayToplessRides:
				target = (flag ? ((float)random.Next(1, 3)) : 1f);
				xp = Mathf.RoundToInt(400f + target * 150f);
				return true;
			default:
				target = 0f;
				xp = 0;
				return false;
			}
		}

		// Token: 0x06007808 RID: 30728 RVA: 0x003105C4 File Offset: 0x0030E7C4
		private static bool TryResolveRankSpecialDifficulty(MissionObjectiveKind kind, Random random, int band, out float target, out int xp)
		{
			int num = Mathf.Max(0, band - MissionRankPolicy.RankSpecialTier(kind));
			switch (kind)
			{
			case MissionObjectiveKind.FlashTopSeenTime:
			case MissionObjectiveKind.FlashBottomSeenTime:
				target = (float)(random.Next(5, 10) + num * 2);
				xp = Mathf.RoundToInt(330f + target * 30f);
				return true;
			case MissionObjectiveKind.FlashAnySeenTime:
				target = (float)(random.Next(6, 11) + num * 2);
				xp = Mathf.RoundToInt(300f + target * 25f);
				return true;
			case MissionObjectiveKind.FlashBothSeenTime:
				target = (float)(random.Next(4, 8) + num);
				xp = Mathf.RoundToInt(420f + target * 45f);
				return true;
			case MissionObjectiveKind.StimulationTopSeenTime:
				target = (float)(random.Next(5, 9) + num * 2);
				xp = Mathf.RoundToInt(340f + target * 32f);
				return true;
			case MissionObjectiveKind.StimulationBottomSeenTime:
				target = (float)(random.Next(4, 8) + num);
				xp = Mathf.RoundToInt(420f + target * 45f);
				return true;
			case MissionObjectiveKind.StimulationAnySeenTime:
				target = (float)(random.Next(5, 10) + num * 2);
				xp = Mathf.RoundToInt(320f + target * 30f);
				return true;
			case MissionObjectiveKind.BralessSeenTime:
				target = (float)(random.Next(2, 5) * 5 + num * 5);
				xp = Mathf.RoundToInt(300f + target * 16f);
				return true;
			case MissionObjectiveKind.HoldBoobsSeenTime:
				target = (float)(random.Next(5, 10) + num * 2);
				xp = Mathf.RoundToInt(340f + target * 32f);
				return true;
			case MissionObjectiveKind.TowelFlashSeenTime:
				target = (float)(random.Next(6, 12) + num * 2);
				xp = Mathf.RoundToInt(340f + target * 28f);
				return true;
			case MissionObjectiveKind.TowelDropSeen:
				target = (float)((band >= 4) ? random.Next(1, 3) : 1);
				xp = Mathf.RoundToInt(500f + target * 150f);
				return true;
			default:
				target = 0f;
				xp = 0;
				return false;
			}
		}

		// Token: 0x06007809 RID: 30729 RVA: 0x003107B3 File Offset: 0x0030E9B3
		private static bool IsWallCoverObjective(MissionObjectiveKind kind)
		{
			return kind == MissionObjectiveKind.WallCoverEntries || kind == MissionObjectiveKind.WallCoverTime;
		}

		// Token: 0x0600780A RID: 30730 RVA: 0x003107C1 File Offset: 0x0030E9C1
		private static int CoverRunReward(MissionObjectiveKind kind, float target)
		{
			return Mathf.Clamp(Mathf.RoundToInt(target * ((kind == MissionObjectiveKind.WallCoverEntries) ? 5f : 0.5f)), 5, 15);
		}

		// Token: 0x0600780B RID: 30731 RVA: 0x003107E4 File Offset: 0x0030E9E4
		private static void NormalizePendingCoverReward(MissionState mission)
		{
			if (mission == null || mission.scope != MissionScope.Run || mission.IsComplete || !MissionService.IsWallCoverObjective(mission.kind))
			{
				return;
			}
			mission.xpReward = Mathf.Min(mission.xpReward, MissionService.CoverRunReward(mission.kind, mission.target));
		}

		// Token: 0x0600780C RID: 30732 RVA: 0x00310838 File Offset: 0x0030EA38
		private static string BuildDescription(MissionObjectiveKind kind, float target, MissionScope scope, string contextName, string qualifierName, string secondaryQualifierName)
		{
			int num = Mathf.RoundToInt(target);
			string text = (MissionService.UsesRunCopy(kind, scope, num) ? string.Format(CultureInfo.CurrentCulture, MissionLocalizationTemplates.RunFull(kind, num == 1), num, qualifierName, secondaryQualifierName) : MissionService.AccountObjective(kind, num, qualifierName, secondaryQualifierName));
			if (!string.IsNullOrWhiteSpace(contextName))
			{
				text = text + " in " + contextName;
			}
			return text;
		}

		// Token: 0x0600780D RID: 30733 RVA: 0x00310898 File Offset: 0x0030EA98
		private static string AccountObjective(MissionObjectiveKind kind, int n, string qualifierName, string secondaryQualifierName)
		{
			switch (kind)
			{
			case MissionObjectiveKind.NpcPhotos:
				return (n == 1) ? "Be photographed 1 time by NPCs" : string.Format("Be photographed {0} times by NPCs", n);
			case MissionObjectiveKind.SuccessfulExhibitions:
				return (n == 1) ? "Be seen by an NPC during 1 emote" : string.Format("Be seen by an NPC during {0} emotes", n);
			case MissionObjectiveKind.PoiDiscoveries:
				return string.Format("Discover {0} locations", n);
			case MissionObjectiveKind.CarSurfs:
				return (n == 1) ? "Complete 1 car surf" : string.Format("Complete {0} car surfs", n);
			case MissionObjectiveKind.Escapes:
				return (n == 1) ? "Escape attention 1 time" : string.Format("Escape attention {0} times", n);
			case MissionObjectiveKind.VehicleDistance:
				return string.Format("Travel {0}m with vehicles", n);
			case MissionObjectiveKind.EmotesPerformed:
				return string.Format("Perform {0} emotes", n);
			case MissionObjectiveKind.SpecificEmote:
				return string.Format("Perform {0} {1} time{2}", qualifierName, n, (n == 1) ? string.Empty : "s");
			case MissionObjectiveKind.Orgasms:
				return string.Format("Reach orgasm while masturbating {0} time{1}", n, (n == 1) ? string.Empty : "s");
			case MissionObjectiveKind.TrashcanSearches:
				return (n == 1) ? "Loot 1 trashcan" : string.Format("Loot {0} trashcans", n);
			case MissionObjectiveKind.TrashcanLootItems:
				return string.Format("Recover {0} items from trashcans", n);
			case MissionObjectiveKind.ItemsCollected:
				return (n == 1) ? "Collect 1 object" : string.Format("Collect {0} objects", n);
			case MissionObjectiveKind.ClothesCollected:
				return (n == 1) ? "Collect 1 clothing item" : string.Format("Collect {0} clothing items", n);
			case MissionObjectiveKind.RareLootCollected:
				return (n == 1) ? "Find 1 rare or legendary item" : string.Format("Find {0} rare or legendary items", n);
			case MissionObjectiveKind.ClothesWorn:
				return string.Format("Wear {0} clothing items", n);
			case MissionObjectiveKind.SpecificClothingWorn:
				return "Put on " + qualifierName;
			case MissionObjectiveKind.SpecificClothingExhibition:
				return string.Format("Be seen by an NPC {0} time{1} wearing {2}", n, (n == 1) ? string.Empty : "s", qualifierName);
			case MissionObjectiveKind.SpecificClothingEmote:
				return string.Format("Perform {0} emote{1} wearing {2}", n, (n == 1) ? string.Empty : "s", qualifierName);
			case MissionObjectiveKind.WearableSpecialAction:
				return string.Format("Use {0} with {1} {2} time{3}", new object[]
				{
					secondaryQualifierName,
					qualifierName,
					n,
					(n == 1) ? string.Empty : "s"
				});
			case MissionObjectiveKind.ClothingBroken:
				return "Wear out " + qualifierName;
			case MissionObjectiveKind.MovingEmoteDistance:
				return string.Format("Travel {0}m using {1}", n, qualifierName);
			case MissionObjectiveKind.SpecificEmoteExhibition:
				return string.Format("Be seen by an NPC during {0} {1} time{2}", qualifierName, n, (n == 1) ? string.Empty : "s");
			case MissionObjectiveKind.WallCoverEntries:
				return string.Format("Use wall cover {0} time{1}", n, (n == 1) ? string.Empty : "s");
			case MissionObjectiveKind.WallCoverTime:
				return string.Format("Spend {0}s in wall cover", n);
			case MissionObjectiveKind.EmoteNearLandmark:
				return string.Format("Perform {0} emote{1} near {2}", n, (n == 1) ? string.Empty : "s", qualifierName);
			case MissionObjectiveKind.SpecificEmoteNearLandmark:
				return string.Format("Perform {0} near {1} {2} time{3}", new object[]
				{
					qualifierName,
					secondaryQualifierName,
					n,
					(n == 1) ? string.Empty : "s"
				});
			case MissionObjectiveKind.SafeHomeReturns:
				return (n == 1) ? "Return home safely to finish this Classic run" : string.Format("Return home safely from {0} Classic runs", n);
			case MissionObjectiveKind.SafeHomeReturnDistance:
				return string.Format("Reach {0:N0} m in one Classic run, then return home safely", n);
			case MissionObjectiveKind.FirstFlight:
				return "Fly for the first time";
			case MissionObjectiveKind.GhostProtocol:
			case MissionObjectiveKind.GroupShot:
			case MissionObjectiveKind.UnicycleDistance:
			case MissionObjectiveKind.MostWantedHome:
			case MissionObjectiveKind.GhostScare:
				return MissionLocalizationTemplates.Full(kind, false);
			}
			return string.Format("Earn {0} score", n);
		}

		// Token: 0x0600780E RID: 30734 RVA: 0x00310D49 File Offset: 0x0030EF49
		internal static bool UsesRunCopy(MissionObjectiveKind kind, MissionScope scope, int count)
		{
			return MissionLocalizationTemplates.RunFull(kind, count == 1) != null && (scope == MissionScope.Run || MissionLocalizationTemplates.Full(kind, count == 1) == null);
		}

		// Token: 0x0600780F RID: 30735 RVA: 0x00310D6C File Offset: 0x0030EF6C
		private static bool CanBePoiSpecific(MissionObjectiveKind kind)
		{
			if (kind <= MissionObjectiveKind.EmotesPerformed)
			{
				if (kind > MissionObjectiveKind.SuccessfulExhibitions && kind != MissionObjectiveKind.EmotesPerformed)
				{
					return false;
				}
			}
			else if (kind != MissionObjectiveKind.Orgasms && kind != MissionObjectiveKind.ClothesWorn)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007810 RID: 30736 RVA: 0x00310D8C File Offset: 0x0030EF8C
		private static HashSet<MissionObjectiveKind> ActiveKinds(List<MissionState> states, int exceptIndex = -1)
		{
			HashSet<MissionObjectiveKind> hashSet = new HashSet<MissionObjectiveKind>();
			for (int i = 0; i < states.Count; i++)
			{
				if (i != exceptIndex && states[i] != null)
				{
					hashSet.Add(states[i].kind);
				}
			}
			return hashSet;
		}

		// Token: 0x06007811 RID: 30737 RVA: 0x00310DD4 File Offset: 0x0030EFD4
		private static bool Sanitize(List<MissionState> states, MissionScope expectedScope)
		{
			bool flag = false;
			if (states == null)
			{
				return true;
			}
			for (int i = states.Count - 1; i >= 0; i--)
			{
				MissionState missionState = states[i];
				if (missionState == null || missionState.target <= 0f || missionState.scope != expectedScope || (MissionService.RequiresQualifier(missionState.kind) && string.IsNullOrWhiteSpace(missionState.qualifierId)) || ((missionState.kind == MissionObjectiveKind.WearableSpecialAction || missionState.kind == MissionObjectiveKind.SpecificEmoteNearLandmark) && string.IsNullOrWhiteSpace(missionState.secondaryQualifierId)))
				{
					states.RemoveAt(i);
					flag = true;
				}
			}
			while (states.Count > MissionService.AccountMissionCatalog.Length && expectedScope == MissionScope.Global)
			{
				states.RemoveAt(states.Count - 1);
				flag = true;
			}
			return flag;
		}

		// Token: 0x06007812 RID: 30738 RVA: 0x00310E88 File Offset: 0x0030F088
		private static bool RequiresQualifier(MissionObjectiveKind kind)
		{
			switch (kind)
			{
			case MissionObjectiveKind.SpecificEmote:
			case MissionObjectiveKind.SpecificClothingWorn:
			case MissionObjectiveKind.SpecificClothingExhibition:
			case MissionObjectiveKind.SpecificClothingEmote:
			case MissionObjectiveKind.WearableSpecialAction:
			case MissionObjectiveKind.ClothingBroken:
			case MissionObjectiveKind.MovingEmoteDistance:
			case MissionObjectiveKind.SpecificEmoteExhibition:
			case MissionObjectiveKind.EmoteNearLandmark:
			case MissionObjectiveKind.SpecificEmoteNearLandmark:
				return true;
			}
			return false;
		}

		// Token: 0x06007813 RID: 30739 RVA: 0x00310EF0 File Offset: 0x0030F0F0
		private static void EnsureLoaded()
		{
			if (MissionService._save != null)
			{
				return;
			}
			try
			{
				string savePath = MissionService.SavePath;
				MissionService._save = (File.Exists(savePath) ? JsonUtility.FromJson<MissionService.MissionSaveData>(File.ReadAllText(savePath)) : null);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[MissionService] Mission save could not be loaded: " + ex.Message);
			}
			if (MissionService._save == null)
			{
				MissionService._save = new MissionService.MissionSaveData();
			}
			if (MissionService._save.globalMissions == null)
			{
				MissionService._save.globalMissions = new List<MissionState>();
			}
			if (MissionService._save.discoveredAccountPoiIds == null)
			{
				MissionService._save.discoveredAccountPoiIds = new List<string>();
				MissionService._saveNeedsWrite = true;
			}
			if (MissionService._save.seed == 0)
			{
				MissionService._save.seed = MissionService.NewSeed();
			}
			if (MissionService._save.schemaVersion < 8)
			{
				int schemaVersion = MissionService._save.schemaVersion;
				MissionService._save.schemaVersion = 8;
				if (schemaVersion <= 3)
				{
					MissionService._save.globalMissions.Clear();
				}
				MissionService._saveNeedsWrite = true;
			}
		}

		// Token: 0x06007814 RID: 30740 RVA: 0x00310FF4 File Offset: 0x0030F1F4
		private static void Save()
		{
			MissionService.EnsureLoaded();
			try
			{
				string savePath = MissionService.SavePath;
				string directoryName = Path.GetDirectoryName(savePath);
				if (!string.IsNullOrEmpty(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllText(savePath, JsonUtility.ToJson(MissionService._save, true));
				MissionService._saveNeedsWrite = false;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[MissionService] Mission save could not be written: " + ex.Message);
			}
		}

		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x06007815 RID: 30741 RVA: 0x00311064 File Offset: 0x0030F264
		private static string SavePath
		{
			get
			{
				return Path.Combine(GameDataPaths.RuntimeRoot, "missions.json");
			}
		}

		// Token: 0x06007816 RID: 30742 RVA: 0x00311078 File Offset: 0x0030F278
		private static int NewSeed()
		{
			int num = Environment.TickCount ^ Guid.NewGuid().GetHashCode();
			if (num != 0)
			{
				return num;
			}
			return 1357911;
		}

		// Token: 0x06007817 RID: 30743 RVA: 0x003110A9 File Offset: 0x0030F2A9
		private static string NormalizeContextId(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				return value.Trim();
			}
			return "CITY";
		}

		// Token: 0x06007818 RID: 30744 RVA: 0x003110BF File Offset: 0x0030F2BF
		private static string NormalizeContextName(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				return value.Trim();
			}
			return "the city";
		}

		// Token: 0x04007AE1 RID: 31457
		private const int RunMissionCount = 3;

		// Token: 0x04007AE2 RID: 31458
		private const int CurrentSchemaVersion = 8;

		// Token: 0x04007AE3 RID: 31459
		private const int LastRotatingGlobalMissionSchemaVersion = 3;

		// Token: 0x04007AE4 RID: 31460
		private const string SaveFileName = "missions.json";

		// Token: 0x04007AE5 RID: 31461
		private static readonly MissionService.AccountMissionDefinition[] LegacyAccountMissionCatalog = new MissionService.AccountMissionDefinition[]
		{
			new MissionService.AccountMissionDefinition("account_candid_50", MissionObjectiveKind.NpcPhotos, 50f, 5000, "Be photographed by NPCs 50 times across any number of runs", 1, 0, false),
			new MissionService.AccountMissionDefinition("account_exhibitions_75", MissionObjectiveKind.SuccessfulExhibitions, 75f, 6500, "Trigger visual detection during 75 completed emotes", 1, 0, false),
			new MissionService.AccountMissionDefinition("account_explorer_6", MissionObjectiveKind.PoiDiscoveries, 6f, 4000, "Discover 6 different locations across the city", 3, 5, false),
			new MissionService.AccountMissionDefinition("account_rooftop_20", MissionObjectiveKind.CarSurfs, 20f, 6000, "Complete 20 car surfs across your account", 1, 0, false),
			new MissionService.AccountMissionDefinition("account_escapes_50", MissionObjectiveKind.Escapes, 50f, 6500, "Escape attention successfully 50 times", 1, 0, false),
			new MissionService.AccountMissionDefinition("account_vehicle_10000", MissionObjectiveKind.VehicleDistance, 10000f, 7500, "Travel a total of 10,000m with vehicles", 1, 0, false),
			new MissionService.AccountMissionDefinition("account_score_250000", MissionObjectiveKind.ScoreEarned, 250000f, 10000, "Earn a cumulative score of 250,000", 1, 0, false),
			new MissionService.AccountMissionDefinition("account_emotes_100", MissionObjectiveKind.EmotesPerformed, 100f, 7000, "Perform 100 emotes across any number of runs", 1, 0, false),
			new MissionService.AccountMissionDefinition("account_private_25", MissionObjectiveKind.Orgasms, 25f, 6000, "Reach orgasm while masturbating 25 times", 1, 0, false),
			new MissionService.AccountMissionDefinition("account_trash_50", MissionObjectiveKind.TrashcanSearches, 50f, 5500, "Search 50 trashcans across the city", 1, 0, false),
			new MissionService.AccountMissionDefinition("account_collector_75", MissionObjectiveKind.ItemsCollected, 75f, 7000, "Collect 75 objects across any number of runs", 1, 0, false),
			new MissionService.AccountMissionDefinition("account_rare_15", MissionObjectiveKind.RareLootCollected, 15f, 8000, "Collect 15 rare or legendary items", 1, 0, false)
		};

		// Token: 0x04007AE6 RID: 31462
		private static readonly MissionService.AccountMissionDefinition[] AccountMissionCatalog = MissionService.BuildAccountCatalog();

		// Token: 0x04007AE7 RID: 31463
		private static MissionService.MissionSaveData _save;

		// Token: 0x04007AE8 RID: 31464
		private static bool _saveNeedsWrite;

		// Token: 0x04007AE9 RID: 31465
		private static bool _completedClaimsSeeded;

		// Token: 0x04007AEA RID: 31466
		private static bool _reconcilingAccountMissions;

		// Token: 0x04007AEB RID: 31467
		private static readonly List<MissionState> RunStates = new List<MissionState>();

		// Token: 0x04007AEC RID: 31468
		private static readonly List<MissionState> VisibleAccountStates = new List<MissionState>();

		// Token: 0x04007AED RID: 31469
		private static readonly List<MissionCompletion> RunCompleted = new List<MissionCompletion>();

		// Token: 0x04007AEE RID: 31470
		private static string _runContextId = string.Empty;

		// Token: 0x04007AEF RID: 31471
		private static string _runContextName = string.Empty;

		// Token: 0x04007AF0 RID: 31472
		private static bool _runActive;

		// Token: 0x04007AF1 RID: 31473
		private static bool _homeReturnReported;

		// Token: 0x04007AF2 RID: 31474
		private static bool _resolvingHomeReturn;

		// Token: 0x04007AF3 RID: 31475
		private static HashSet<string> _collectedClothingThisRun = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04007AF4 RID: 31476
		private static HashSet<string> _wornClothingThisRun = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04007AF5 RID: 31477
		private static HashSet<string> _specificClothingWornThisRun = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04007AF6 RID: 31478
		private static readonly List<MissionOption> AvailableEmotes = new List<MissionOption>();

		// Token: 0x04007AF7 RID: 31479
		private static readonly List<MissionWearableOption> AvailableWearables = new List<MissionWearableOption>();

		// Token: 0x04007AF8 RID: 31480
		private static readonly List<MissionWearableActionOption> AvailableWearableActions = new List<MissionWearableActionOption>();

		// Token: 0x04007AF9 RID: 31481
		private static readonly List<MissionOption> AvailableLandmarks = new List<MissionOption>();

		// Token: 0x04007AFA RID: 31482
		private static readonly HashSet<MissionObjectiveKind> RankCapableKinds = new HashSet<MissionObjectiveKind>();

		// Token: 0x04007AFB RID: 31483
		private static readonly HashSet<MissionObjectiveKind> PlaceCapableKinds = new HashSet<MissionObjectiveKind>();

		// Token: 0x04007AFC RID: 31484
		private static readonly HashSet<MissionObjectiveKind> PlaceKindsDoneThisVisit = new HashSet<MissionObjectiveKind>();

		// Token: 0x04007AFD RID: 31485
		private static bool _rankSlotResting;

		// Token: 0x04007AFE RID: 31486
		private static int _rankRestCompletions;

		// Token: 0x04007AFF RID: 31487
		private static MissionObjectiveKind? _lastRankKind;

		// Token: 0x04007B00 RID: 31488
		private const float UnicycleEggMeters = 500f;

		// Token: 0x04007B01 RID: 31489
		private static readonly Dictionary<string, List<MissionState>> ParkedRunStates = new Dictionary<string, List<MissionState>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04007B02 RID: 31490
		private static readonly Dictionary<string, float> UnavailableSince = new Dictionary<string, float>(StringComparer.Ordinal);

		// Token: 0x04007B03 RID: 31491
		private const float UnavailableGraceSeconds = 15f;

		// Token: 0x04007B04 RID: 31492
		private const float ProgressedUnavailableSeconds = 120f;

		// Token: 0x04007B05 RID: 31493
		private static bool _wallCoverAvailable;

		// Token: 0x04007B06 RID: 31494
		private static bool _redrawAfterWorldScan;

		// Token: 0x04007B07 RID: 31495
		private static readonly MissionOption[] KnownPoiContexts = new MissionOption[]
		{
			new MissionOption("ArtGallery", "Art Gallery", "", "", false, false, false, true),
			new MissionOption("Pool", "City Pool", "", "", false, false, false, true),
			new MissionOption("University", "University", "", "", false, false, false, true),
			new MissionOption("Park", "Park", "", "", false, false, false, true),
			new MissionOption("Stadium", "Stadium", "", "", false, false, false, true),
			new MissionOption("Gymnasium", "School Gymnasium", "", "", false, false, false, true)
		};

		// Token: 0x02000F85 RID: 3973
		private readonly struct AccountMissionDefinition
		{
			// Token: 0x0600781A RID: 30746 RVA: 0x00311458 File Offset: 0x0030F658
			public AccountMissionDefinition(string id, MissionObjectiveKind kind, float target, int xp, string description, int tier = 1, int minimumRankIndex = 0, bool secret = false)
			{
				this.Id = id;
				this.Kind = kind;
				this.Target = target;
				this.Xp = xp;
				this.Description = description;
				this.Tier = tier;
				this.MinimumRankIndex = minimumRankIndex;
				this.Secret = secret;
			}

			// Token: 0x04007B0C RID: 31500
			public readonly string Id;

			// Token: 0x04007B0D RID: 31501
			public readonly MissionObjectiveKind Kind;

			// Token: 0x04007B0E RID: 31502
			public readonly float Target;

			// Token: 0x04007B0F RID: 31503
			public readonly int Xp;

			// Token: 0x04007B10 RID: 31504
			public readonly string Description;

			// Token: 0x04007B11 RID: 31505
			public readonly int Tier;

			// Token: 0x04007B12 RID: 31506
			public readonly int MinimumRankIndex;

			// Token: 0x04007B13 RID: 31507
			public readonly bool Secret;
		}

		// Token: 0x02000F86 RID: 3974
		[Serializable]
		private sealed class MissionSaveData
		{
			// Token: 0x04007B14 RID: 31508
			public int schemaVersion;

			// Token: 0x04007B15 RID: 31509
			public int seed;

			// Token: 0x04007B16 RID: 31510
			public int generationCounter;

			// Token: 0x04007B17 RID: 31511
			public List<MissionState> globalMissions = new List<MissionState>();

			// Token: 0x04007B18 RID: 31512
			public List<string> discoveredAccountPoiIds = new List<string>();
		}
	}
}
