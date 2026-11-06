using System;
using System.Collections.Generic;
using System.Globalization;
using Features.Inventory.Wearable;
using Features.MainMenu;
using Features.MatchRecap;
using Features.Missions;
using Features.PlayerEmotes;
using Features.Rank;
using Features.RunModes;
using ProjectExhibitionist.Headphones;
using ProjectExhibitionist.Minimap;
using UnityEngine;

namespace Features.Achievements
{
	// Token: 0x0200153D RID: 5437
	public static class AchievementTrackingService
	{
		// Token: 0x170019E5 RID: 6629
		// (get) Token: 0x0600A84F RID: 43087 RVA: 0x00463F7C File Offset: 0x0046217C
		private static bool CanEarn
		{
			get
			{
				return Application.isPlaying && !RankProgressionService.SuppressProgressionRewards && !RunModeService.IsFreeMode;
			}
		}

		// Token: 0x0600A850 RID: 43088 RVA: 0x00463F96 File Offset: 0x00462196
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			AchievementTrackingService._progress = null;
			AchievementTrackingService._catalog = null;
			AchievementTrackingService._catalogLoaded = false;
			AchievementTrackingService._dirty = false;
			AchievementTrackingService._pendingPrisonScene = null;
			AchievementTrackingService._departurePrepared = false;
		}

		// Token: 0x170019E6 RID: 6630
		// (get) Token: 0x0600A851 RID: 43089 RVA: 0x00463FBC File Offset: 0x004621BC
		private static AchievementProgress Progress
		{
			get
			{
				if (AchievementTrackingService._progress != null)
				{
					return AchievementTrackingService._progress;
				}
				AchievementProgressData achievementProgressData = null;
				string @string = PlayerPrefs.GetString("Achievements.Progress.v1", string.Empty);
				if (!string.IsNullOrEmpty(@string))
				{
					try
					{
						achievementProgressData = JsonUtility.FromJson<AchievementProgressData>(@string);
					}
					catch (Exception ex)
					{
						Debug.LogWarning("[Achievements] Invalid progress save: " + ex.Message);
					}
				}
				AchievementTrackingService._progress = new AchievementProgress(achievementProgressData, delegate(string id)
				{
					AchievementTrackingService._dirty = true;
					AchievementService.Unlock(id);
				});
				if (!AchievementTrackingService._catalogLoaded)
				{
					AchievementTrackingService._catalogLoaded = true;
					AchievementTrackingService._catalog = Resources.Load<AchievementCollectionCatalog>("AchievementCollectionCatalog");
				}
				AchievementTrackingService._progress.SetRequiredPois(AchievementExplorationTargets.PoiIds);
				return AchievementTrackingService._progress;
			}
		}

		// Token: 0x0600A852 RID: 43090 RVA: 0x0046407C File Offset: 0x0046227C
		public static void PrepareDeparture()
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementTrackingService._headphonesAtDeparture = HeadphonesEquipment.IsWorn;
			AchievementTrackingService._departurePrepared = true;
		}

		// Token: 0x0600A853 RID: 43091 RVA: 0x00464096 File Offset: 0x00462296
		public static void BeginRun()
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementTrackingService.Progress.BeginRun(AchievementTrackingService._departurePrepared ? AchievementTrackingService._headphonesAtDeparture : HeadphonesEquipment.IsWorn);
			AchievementTrackingService._departurePrepared = false;
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A854 RID: 43092 RVA: 0x004640C8 File Offset: 0x004622C8
		public static void EndRun()
		{
			if (AchievementTrackingService._progress != null)
			{
				AchievementTrackingService._progress.EndRun();
			}
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A855 RID: 43093 RVA: 0x004640E0 File Offset: 0x004622E0
		public static void ReportMission(MissionObjectiveKind kind, float amount, string qualifierId)
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementTrackingService.Progress.ReportMission(kind, amount, qualifierId);
			if (kind == MissionObjectiveKind.NpcPhotos || kind == MissionObjectiveKind.TrashcanSearches || kind == MissionObjectiveKind.ClothesCollected || kind == MissionObjectiveKind.PoiDiscoveries || kind == MissionObjectiveKind.SafeHomeReturnDistance)
			{
				AchievementTrackingService._dirty = true;
				AchievementTrackingService.SaveNow();
			}
		}

		// Token: 0x0600A856 RID: 43094 RVA: 0x00464118 File Offset: 0x00462318
		public static void AddTravel(float meters)
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementTrackingService.Progress.AddTravel((double)meters);
			AchievementTrackingService._dirty = true;
		}

		// Token: 0x0600A857 RID: 43095 RVA: 0x00464134 File Offset: 0x00462334
		public static void RecordMapPoiVisit(MiniMapPoiKind kind)
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			string poiId = AchievementExplorationTargets.GetPoiId(kind);
			if (poiId == null)
			{
				return;
			}
			AchievementTrackingService.Progress.RecordPoi(poiId, true);
			AchievementTrackingService._dirty = true;
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A858 RID: 43096 RVA: 0x0046416B File Offset: 0x0046236B
		public static void AddStep()
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementTrackingService.Progress.AddStep(DateTime.Today, PlayerStatsService.DailyStepGoal);
			AchievementTrackingService._dirty = true;
		}

		// Token: 0x0600A859 RID: 43097 RVA: 0x00464190 File Offset: 0x00462390
		public static void RefreshTodayGoal()
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementProgressData data = AchievementTrackingService.Progress.Data;
			string text = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
			int num = 0;
			foreach (AchievementDay achievementDay in data.days)
			{
				if (achievementDay != null && achievementDay.date == text)
				{
					num = achievementDay.steps;
				}
			}
			AchievementTrackingService.Progress.RecordDailySteps(DateTime.Today, num, PlayerStatsService.DailyStepGoal);
			AchievementTrackingService._dirty = true;
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A85A RID: 43098 RVA: 0x00464244 File Offset: 0x00462444
		public static void RecordRank(int rankIndex, bool admin)
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementTrackingService.Progress.RecordRank(rankIndex, admin);
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A85B RID: 43099 RVA: 0x0046425F File Offset: 0x0046245F
		public static void RecordGroupPhoto(bool group, bool hasPhoto)
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementTrackingService.Progress.RecordGroupPhoto(group, hasPhoto);
			AchievementTrackingService._dirty = true;
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A85C RID: 43100 RVA: 0x00464280 File Offset: 0x00462480
		public static void RecordPosterDestroyed()
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementTrackingService.Progress.RecordPosterDestroyed();
			AchievementTrackingService._dirty = true;
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A85D RID: 43101 RVA: 0x0046429F File Offset: 0x0046249F
		public static void RecordCustomPaint(bool consumedPen, bool hasPaint)
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementTrackingService.Progress.RecordCustomPaint(consumedPen, hasPaint);
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A85E RID: 43102 RVA: 0x004642BA File Offset: 0x004624BA
		public static void RecordReporter()
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementTrackingService.Progress.RecordReporter();
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A85F RID: 43103 RVA: 0x004642D3 File Offset: 0x004624D3
		public static void RecordLongWayHomeWin()
		{
			if (!AchievementTrackingService.CanEarn || !RunModeService.IsLongWayHome)
			{
				return;
			}
			AchievementTrackingService.Progress.RecordLongWayHomeWin();
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A860 RID: 43104 RVA: 0x004642F3 File Offset: 0x004624F3
		public static void ArmPrisonArrival(string sceneName)
		{
			if (AchievementTrackingService.CanEarn && !string.IsNullOrWhiteSpace(sceneName))
			{
				AchievementTrackingService._pendingPrisonScene = sceneName;
			}
		}

		// Token: 0x0600A861 RID: 43105 RVA: 0x0046430A File Offset: 0x0046250A
		public static void SceneArrived(string sceneName)
		{
			if (!AchievementTrackingService.CanEarn || string.IsNullOrEmpty(AchievementTrackingService._pendingPrisonScene) || !string.Equals(sceneName, AchievementTrackingService._pendingPrisonScene, StringComparison.Ordinal))
			{
				return;
			}
			AchievementTrackingService._pendingPrisonScene = null;
			AchievementTrackingService.Progress.RecordPrisonArrival();
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A862 RID: 43106 RVA: 0x00464344 File Offset: 0x00462544
		public static void ReconcileExistingSave()
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementProgress progress = AchievementTrackingService.Progress;
			foreach (string text in progress.Data.unlockedIds)
			{
				if (!AchievementIds.IsLegacy(text))
				{
					AchievementService.Unlock(text);
				}
			}
			PlayerStatsData snapshot = PlayerStatsService.GetSnapshot();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (MissionState missionState in MissionService.AccountMissions)
			{
				if (string.IsNullOrEmpty(missionState.qualifierId) && string.IsNullOrEmpty(missionState.contextId))
				{
					int num4 = (int)Math.Min(2147483647.0, Math.Max(0.0, (double)missionState.progress));
					if (missionState.kind == MissionObjectiveKind.NpcPhotos)
					{
						num = Math.Max(num, num4);
					}
					if (missionState.kind == MissionObjectiveKind.TrashcanSearches)
					{
						num2 = Math.Max(num2, num4);
					}
					if (missionState.kind == MissionObjectiveKind.ClothesCollected)
					{
						num3 = Math.Max(num3, num4);
					}
				}
			}
			progress.SeedAccountProgress((double)snapshot.totalWalkingMeters + (double)snapshot.totalRunningMeters, num, num2, num3);
			progress.RecordRank(RankProgressionService.RankIndex, RankProgressionService.IsAdminRankActive);
			if (AchievementTrackingService._catalog != null && AchievementTrackingService._catalog.pointsOfInterest != null)
			{
				foreach (PoiEmoteProfileSO poiEmoteProfileSO in AchievementTrackingService._catalog.pointsOfInterest)
				{
					if (poiEmoteProfileSO != null && CollectionUnlockRegistry.IsPoiDiscovered(poiEmoteProfileSO))
					{
						progress.RecordPoi(string.IsNullOrWhiteSpace(poiEmoteProfileSO.PoiId) ? poiEmoteProfileSO.name : poiEmoteProfileSO.PoiId, false);
					}
				}
			}
			string text2 = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
			if (snapshot.healthDays != null)
			{
				foreach (HealthDayData healthDayData in snapshot.healthDays)
				{
					if (healthDayData != null && healthDayData.day == text2)
					{
						progress.RecordDailySteps(DateTime.Today, healthDayData.steps, snapshot.dailyStepGoal);
					}
				}
			}
			AchievementTrackingService.CheckCollection();
			AchievementTrackingService._dirty = true;
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A863 RID: 43107 RVA: 0x004645C0 File Offset: 0x004627C0
		public static void CheckCollection()
		{
			if (!AchievementTrackingService.CanEarn)
			{
				return;
			}
			AchievementProgress progress = AchievementTrackingService.Progress;
			if (AchievementTrackingService._catalog == null || !AchievementTrackingService._catalog.HasCurrentMetadata)
			{
				return;
			}
			int num;
			int num2;
			AchievementTrackingService.CountEarnedCollectionTargets(AchievementTrackingService._catalog, out num, out num2);
			progress.CheckWardrobe(num, num2);
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A864 RID: 43108 RVA: 0x00464610 File Offset: 0x00462810
		public static void CountEarnedCollectionTargets(AchievementCollectionCatalog catalog, out int found, out int total)
		{
			total = 0;
			found = 0;
			if (catalog == null || !catalog.HasCurrentMetadata)
			{
				return;
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (WardrobeMetadataIndex.Entry entry in catalog.ClothingMetadata)
			{
				if (entry != null)
				{
					string text = (string.IsNullOrWhiteSpace(entry.itemId) ? entry.assetName : entry.itemId);
					if (entry.rarityVariants != null && entry.rarityVariants.Length != 0)
					{
						foreach (WardrobeMetadataIndex.RarityEntry rarityEntry in entry.rarityVariants)
						{
							if (rarityEntry != null && rarityEntry.exists && hashSet.Add(text + ":" + rarityEntry.surfaceIndex.ToString()))
							{
								total++;
								if (CollectionUnlockRegistry.IsVariantEarnedMetadata(entry, rarityEntry.surfaceIndex))
								{
									found++;
								}
							}
						}
					}
					else if (hashSet.Add(text + ":-1"))
					{
						total++;
						if (CollectionUnlockRegistry.IsVariantEarnedMetadata(entry, -1))
						{
							found++;
						}
					}
				}
			}
		}

		// Token: 0x0600A865 RID: 43109 RVA: 0x00464748 File Offset: 0x00462948
		public static void SaveNow()
		{
			if (!AchievementTrackingService._dirty || AchievementTrackingService._progress == null)
			{
				return;
			}
			PlayerPrefs.SetString("Achievements.Progress.v1", JsonUtility.ToJson(AchievementTrackingService._progress.Data));
			PlayerPrefs.Save();
			AchievementTrackingService._dirty = false;
		}

		// Token: 0x0400AD14 RID: 44308
		private const string SaveKey = "Achievements.Progress.v1";

		// Token: 0x0400AD15 RID: 44309
		private static AchievementProgress _progress;

		// Token: 0x0400AD16 RID: 44310
		private static AchievementCollectionCatalog _catalog;

		// Token: 0x0400AD17 RID: 44311
		private static bool _catalogLoaded;

		// Token: 0x0400AD18 RID: 44312
		private static bool _dirty;

		// Token: 0x0400AD19 RID: 44313
		private static string _pendingPrisonScene;

		// Token: 0x0400AD1A RID: 44314
		private static bool _headphonesAtDeparture;

		// Token: 0x0400AD1B RID: 44315
		private static bool _departurePrepared;
	}
}
