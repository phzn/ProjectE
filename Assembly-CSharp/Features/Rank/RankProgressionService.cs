using System;
using System.Collections.Generic;
using System.IO;
using Bozo.AnimeCharacters;
using Features.Achievements;
using Features.GameOver;
using Features.RunModes;
using UnityEngine;

namespace Features.Rank
{
	// Token: 0x02000DB3 RID: 3507
	public static class RankProgressionService
	{
		// Token: 0x140000AD RID: 173
		// (add) Token: 0x06006716 RID: 26390 RVA: 0x00293FE4 File Offset: 0x002921E4
		// (remove) Token: 0x06006717 RID: 26391 RVA: 0x00294018 File Offset: 0x00292218
		public static event Action Changed;

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06006718 RID: 26392 RVA: 0x0029404B File Offset: 0x0029224B
		private static RankProgressionData Data
		{
			get
			{
				if (RankProgressionService._data == null)
				{
					RankProgressionService.Load();
				}
				return RankProgressionService._data;
			}
		}

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x06006719 RID: 26393 RVA: 0x0029405E File Offset: 0x0029225E
		public static RankProgressionConfigSO Config
		{
			get
			{
				if (!RankProgressionService._configResolved)
				{
					RankProgressionService._config = Resources.Load<RankProgressionConfigSO>("RankConfig");
					RankProgressionService._configResolved = true;
					if (RankProgressionService._config == null)
					{
						Debug.LogWarning("[RankProgressionService] Config 'RankConfig' non trovato in Resources/. Esegui Tools/Rank/Create Default Assets.");
					}
				}
				return RankProgressionService._config;
			}
		}

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x0600671A RID: 26394 RVA: 0x00294098 File Offset: 0x00292298
		public static int Level
		{
			get
			{
				return RankProgressionService.Data.level;
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x0600671B RID: 26395 RVA: 0x002940A4 File Offset: 0x002922A4
		public static float Xp
		{
			get
			{
				return RankProgressionService.Data.xp;
			}
		}

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x0600671C RID: 26396 RVA: 0x002940B0 File Offset: 0x002922B0
		public static int RankIndex
		{
			get
			{
				return RankProgressionService.Data.rankIndex;
			}
		}

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x0600671D RID: 26397 RVA: 0x002940BC File Offset: 0x002922BC
		public static float TotalXpEarned
		{
			get
			{
				RankProgressionService.MigrateTotalXp(RankProgressionService.Data);
				return RankProgressionService.Data.totalXpEarned;
			}
		}

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x0600671E RID: 26398 RVA: 0x002940D4 File Offset: 0x002922D4
		public static RankDefinitionSO CurrentRank
		{
			get
			{
				RankProgressionConfigSO config = RankProgressionService.Config;
				if (config == null)
				{
					return null;
				}
				if (RankProgressionService.Data.adminRankActive && config.adminRank != null)
				{
					return config.adminRank;
				}
				if (config.ranksInOrder == null)
				{
					return null;
				}
				int num = Mathf.Clamp(RankProgressionService.Data.rankIndex, 0, config.ranksInOrder.Count - 1);
				if (num < 0 || num >= config.ranksInOrder.Count)
				{
					return null;
				}
				return config.ranksInOrder[num];
			}
		}

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x0600671F RID: 26399 RVA: 0x0029415B File Offset: 0x0029235B
		public static bool IsAdminRankActive
		{
			get
			{
				return RankProgressionService.Data.adminRankActive;
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x06006720 RID: 26400 RVA: 0x00294167 File Offset: 0x00292367
		public static bool HasCreatorBoost
		{
			get
			{
				return RankProgressionService.Data.creatorBoostApplied;
			}
		}

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x06006721 RID: 26401 RVA: 0x00294173 File Offset: 0x00292373
		public static bool SuppressProgressionRewards
		{
			get
			{
				return RankProgressionService.IsAdminRankActive || RunModeService.PreparedStartedAsAdmin;
			}
		}

		// Token: 0x06006722 RID: 26402 RVA: 0x00294183 File Offset: 0x00292383
		public static bool HasCompletedOutfitRun(string rankId)
		{
			return RankFirstOutfitProgression.HasCompletedRun(RankProgressionService.Data, rankId);
		}

		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x06006723 RID: 26403 RVA: 0x00294190 File Offset: 0x00292390
		public static bool HasChosenOutfitForCurrentRank
		{
			get
			{
				return RankProgressionService.CurrentRank != null && string.Equals(RankProgressionService.Data.customOutfitRankId, RankProgressionService.CurrentRank.rankId, StringComparison.Ordinal);
			}
		}

		// Token: 0x06006724 RID: 26404 RVA: 0x002941BB File Offset: 0x002923BB
		public static void RecordCustomOutfitChoice()
		{
			if (!RankWardrobePolicy.IsRestricted || RankWardrobePolicy.IsFirstRunOutfitRequired || RankProgressionService.HasChosenOutfitForCurrentRank)
			{
				return;
			}
			RankProgressionService.Data.customOutfitRankId = RankProgressionService.CurrentRank.rankId;
			RankProgressionService.Save();
		}

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06006725 RID: 26405 RVA: 0x002941EC File Offset: 0x002923EC
		public static float BestQualifyingDistance
		{
			get
			{
				return RankProgressionService.Data.bestQualifyingDistance;
			}
		}

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06006726 RID: 26406 RVA: 0x002941F8 File Offset: 0x002923F8
		private static string FilePath
		{
			get
			{
				return Path.Combine(GameDataPaths.RuntimeRoot, "progression.json");
			}
		}

		// Token: 0x06006727 RID: 26407 RVA: 0x0029420C File Offset: 0x0029240C
		private static void Load()
		{
			try
			{
				string filePath = RankProgressionService.FilePath;
				if (File.Exists(filePath))
				{
					RankProgressionService._data = JsonUtility.FromJson<RankProgressionData>(File.ReadAllText(filePath)) ?? new RankProgressionData();
				}
				else
				{
					RankProgressionService._data = new RankProgressionData();
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[RankProgressionService] Load fallito, stato azzerato: " + ex.Message);
				RankProgressionService._data = new RankProgressionData();
			}
			if (RankProgressionService._data.unlockFlags == null)
			{
				RankProgressionService._data.unlockFlags = new List<bool>();
			}
			if (RankProgressionService._data.claimedMissionRewardIds == null)
			{
				RankProgressionService._data.claimedMissionRewardIds = new List<string>();
			}
			int level = RankProgressionService._data.level;
			float totalXpEarned = RankProgressionService._data.totalXpEarned;
			RankProgressionService.MigrateTotalXp(RankProgressionService._data);
			RankProgressionService.ApplyLevelUps(RankProgressionService._data, RankProgressionService.Config);
			RankProgressionService._storyProgressionNeedsSave = RankProgressionService._data.level != level || RankProgressionService._data.totalXpEarned != totalXpEarned;
			RankProgressionService._storyProgressionNeedsSave |= RankFirstOutfitProgression.Synchronize(RankProgressionService._data, RankProgressionService.Config);
			RankProgressionService._storyProgressionNeedsSave |= RankStoryProgression.Synchronize(RankProgressionService._data, RankProgressionService.Config);
		}

		// Token: 0x06006728 RID: 26408 RVA: 0x00294340 File Offset: 0x00292540
		public static void Save()
		{
			RankProgressionData data = RankProgressionService.Data;
			RankProgressionService.SynchronizeStory(data);
			RankProgressionService._storyProgressionNeedsSave = false;
			RankProgressionService.TryPersistData(data);
			Action changed = RankProgressionService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06006729 RID: 26409 RVA: 0x00294368 File Offset: 0x00292568
		private static bool TryPersistData(RankProgressionData data)
		{
			string text = null;
			bool flag;
			try
			{
				string filePath = RankProgressionService.FilePath;
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));
				text = filePath + "." + Guid.NewGuid().ToString("N") + ".tmp";
				File.WriteAllText(text, JsonUtility.ToJson(data, true));
				if (File.Exists(filePath))
				{
					File.Replace(text, filePath, null);
				}
				else
				{
					File.Move(text, filePath);
				}
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[RankProgressionService] Save fallito: " + ex.Message);
				flag = false;
			}
			finally
			{
				if (text != null)
				{
					try
					{
						if (File.Exists(text))
						{
							File.Delete(text);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return flag;
		}

		// Token: 0x0600672A RID: 26410 RVA: 0x00294434 File Offset: 0x00292634
		internal static RankProgressionData GetStoryProgressionData()
		{
			RankProgressionData data = RankProgressionService.Data;
			RankProgressionService.SynchronizeStory(data);
			if (RankProgressionService._storyProgressionNeedsSave)
			{
				RankProgressionService.Save();
			}
			return data;
		}

		// Token: 0x0600672B RID: 26411 RVA: 0x0029444D File Offset: 0x0029264D
		private static void SynchronizeStory(RankProgressionData data)
		{
			RankProgressionService._storyProgressionNeedsSave |= RankFirstOutfitProgression.Synchronize(data, RankProgressionService.Config);
			RankProgressionService._storyProgressionNeedsSave |= RankStoryProgression.Synchronize(data, RankProgressionService.Config);
		}

		// Token: 0x0600672C RID: 26412 RVA: 0x0029447B File Offset: 0x0029267B
		public static void ReloadFromDisk()
		{
			RankProgressionService._data = null;
			RankProgressionService.Load();
			Action changed = RankProgressionService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x0600672D RID: 26413 RVA: 0x00294498 File Offset: 0x00292698
		public static RunOutcome AddRunResults(RunResult run)
		{
			RankProgressionConfigSO config = RankProgressionService.Config;
			RankProgressionData data = RankProgressionService.Data;
			if (RankProgressionService.SuppressProgressionRewards || run.startedAsAdmin || RunModeService.IsFreeMode)
			{
				return new RunOutcome
				{
					previousLevel = data.level,
					previousXp = data.xp,
					newLevel = data.level,
					newRankIndex = data.rankIndex,
					totalXpAfter = data.xp
				};
			}
			if (!data.adminRankActive && !run.startedAsAdmin && !run.skipOutfitIntroduction && (RunModeService.CurrentMode == RunModeKind.Classic || RunModeService.CurrentMode == RunModeKind.Guided))
			{
				data.storyFirstRunCompleted = true;
				string text = ((!string.IsNullOrEmpty(run.startingRankId)) ? run.startingRankId : ((RankProgressionService.CurrentRank != null) ? RankProgressionService.CurrentRank.rankId : null));
				RankFirstOutfitProgression.RecordCompletedRun(data, text);
			}
			RunXpBreakdown runXpBreakdown = RankProgressionService.CalculateRunXp(run, config);
			float totalXp = runXpBreakdown.totalXp;
			int level = data.level;
			int rankIndex = data.rankIndex;
			float xp = data.xp;
			RankProgressionService.MigrateTotalXp(data);
			data.xp += totalXp;
			data.totalXpEarned += totalXp;
			RankProgressionService.ApplyLevelUps(data, config);
			if (!data.adminRankActive)
			{
				if (run.peakDistanceMeters > data.bestQualifyingDistance)
				{
					data.bestQualifyingDistance = run.peakDistanceMeters;
				}
				RankProgressionService.EvaluatePromotion(config, data);
			}
			RankProgressionService.Save();
			bool flag = data.rankIndex != rankIndex;
			RunOutcome runOutcome = new RunOutcome
			{
				xpGained = totalXp,
				baseXpGained = runXpBreakdown.baseXp,
				scoreXpGained = runXpBreakdown.scoreXp,
				scorePoints = Mathf.Max(0, run.scorePoints),
				scoreDistanceMultiplier = runXpBreakdown.scoreDistanceMultiplier,
				previousLevel = level,
				previousXp = xp,
				levelsGained = data.level - level,
				rankChanged = flag,
				newLevel = data.level,
				newRankIndex = data.rankIndex,
				totalXpAfter = data.xp
			};
			if (flag && RankProgressionService.CurrentRank != null)
			{
				Debug.Log(string.Format("[RankProgressionService] PROMOZIONE -> {0} (rank {1}).", RankProgressionService.CurrentRank.displayName, data.rankIndex));
			}
			if (flag)
			{
				AchievementTrackingService.RecordRank(data.rankIndex, data.adminRankActive);
			}
			return runOutcome;
		}

		// Token: 0x0600672E RID: 26414 RVA: 0x002946F0 File Offset: 0x002928F0
		private static void ApplyLevelUps(RankProgressionData data, RankProgressionConfigSO cfg)
		{
			if (cfg == null || float.IsNaN(data.xp) || float.IsInfinity(data.xp))
			{
				return;
			}
			float num = cfg.GetLevelCost(data.level);
			while (data.xp >= num && data.level < 2147483647)
			{
				data.xp -= num;
				data.level++;
				num = cfg.GetLevelCost(data.level);
			}
		}

		// Token: 0x0600672F RID: 26415 RVA: 0x00294770 File Offset: 0x00292970
		public static RunXpBreakdown CalculateRunXp(RunResult run, RankProgressionConfigSO cfg)
		{
			if (cfg == null)
			{
				return new RunXpBreakdown
				{
					scoreDistanceMultiplier = 1
				};
			}
			float num = Mathf.Max(0f, run.peakDistanceMeters) * Mathf.Max(0f, cfg.xpPerMeterPeak) + (float)Mathf.Max(0, run.timesSeen) * Mathf.Max(0f, cfg.xpPerTimeSeen) + (float)Mathf.Max(0, run.validEmotes) * Mathf.Max(0f, cfg.xpPerValidEmote);
			int scoreDistanceMultiplier = RankProgressionService.GetScoreDistanceMultiplier(run.peakDistanceMeters, cfg);
			float num2 = (float)Mathf.Max(0, run.scorePoints) * Mathf.Max(0f, cfg.xpPerScorePoint) * (float)scoreDistanceMultiplier;
			return new RunXpBreakdown
			{
				baseXp = num,
				scoreXp = num2,
				totalXp = num + num2,
				scoreDistanceMultiplier = scoreDistanceMultiplier
			};
		}

		// Token: 0x06006730 RID: 26416 RVA: 0x00294854 File Offset: 0x00292A54
		public static int GetScoreDistanceMultiplier(float peakDistanceMeters, RankProgressionConfigSO cfg = null)
		{
			cfg = ((cfg != null) ? cfg : RankProgressionService.Config);
			if (cfg == null)
			{
				return 1;
			}
			float num = Mathf.Max(1f, cfg.scoreDistanceStepMeters);
			return Mathf.Clamp(1 + Mathf.FloorToInt(Mathf.Max(0f, peakDistanceMeters) / num), 1, Mathf.Max(1, cfg.maxScoreDistanceMultiplier));
		}

		// Token: 0x06006731 RID: 26417 RVA: 0x002948B8 File Offset: 0x00292AB8
		public static float GetScoreDistanceMultiplierProgress(float peakDistanceMeters, RankProgressionConfigSO cfg = null)
		{
			cfg = ((cfg != null) ? cfg : RankProgressionService.Config);
			if (cfg == null)
			{
				return 0f;
			}
			if (RankProgressionService.GetScoreDistanceMultiplier(peakDistanceMeters, cfg) >= Mathf.Max(1, cfg.maxScoreDistanceMultiplier))
			{
				return 1f;
			}
			float num = Mathf.Max(1f, cfg.scoreDistanceStepMeters);
			return Mathf.Repeat(Mathf.Max(0f, peakDistanceMeters), num) / num;
		}

		// Token: 0x06006732 RID: 26418 RVA: 0x00294928 File Offset: 0x00292B28
		private static void MigrateTotalXp(RankProgressionData data)
		{
			if (data == null || data.creatorBoostApplied || data.totalXpEarned > 0f || (data.level <= 0 && data.xp <= 0f))
			{
				return;
			}
			data.totalXpEarned = (float)Mathf.Max(0, data.level) * 50f + Mathf.Max(0f, data.xp);
		}

		// Token: 0x06006733 RID: 26419 RVA: 0x00294990 File Offset: 0x00292B90
		private static void EvaluatePromotion(RankProgressionConfigSO cfg, RankProgressionData data)
		{
			if (cfg == null || cfg.ranksInOrder == null)
			{
				return;
			}
			while (data.rankIndex + 1 < cfg.ranksInOrder.Count)
			{
				RankDefinitionSO rankDefinitionSO = cfg.ranksInOrder[data.rankIndex + 1];
				if (rankDefinitionSO == null)
				{
					break;
				}
				bool flag = data.level >= rankDefinitionSO.minLevel;
				bool flag2 = data.bestQualifyingDistance >= rankDefinitionSO.promotionRunDistance;
				if (!flag || !flag2)
				{
					break;
				}
				data.rankIndex++;
				data.bestQualifyingDistance = 0f;
			}
		}

		// Token: 0x06006734 RID: 26420 RVA: 0x00294A24 File Offset: 0x00292C24
		public static bool IsUnlocked(RankUnlock unlock)
		{
			if (RankProgressionService.IsAdminRankActive && Enum.IsDefined(typeof(RankUnlock), unlock))
			{
				return true;
			}
			List<bool> unlockFlags = RankProgressionService.Data.unlockFlags;
			return unlockFlags != null && unlock >= RankUnlock.MoodSetting && unlock < (RankUnlock)unlockFlags.Count && unlockFlags[(int)unlock];
		}

		// Token: 0x06006735 RID: 26421 RVA: 0x00294A78 File Offset: 0x00292C78
		public static void SetUnlocked(RankUnlock unlock, bool value)
		{
			if (unlock < RankUnlock.MoodSetting)
			{
				return;
			}
			List<bool> unlockFlags = RankProgressionService.Data.unlockFlags;
			while (unlockFlags.Count <= (int)unlock)
			{
				unlockFlags.Add(false);
			}
			unlockFlags[(int)unlock] = value;
			RankProgressionService.Save();
		}

		// Token: 0x06006736 RID: 26422 RVA: 0x00294AB8 File Offset: 0x00292CB8
		public static bool ApplyCreatorBoost(out string error)
		{
			error = null;
			if (!RunModeService.IsAtHome)
			{
				error = "Unlock is available only on the home PC.";
				return false;
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			int num = ((config != null && config.ranksInOrder != null) ? (config.ranksInOrder.Count - 1) : (-1));
			RankDefinitionSO rankDefinitionSO = ((num >= 0) ? config.ranksInOrder[num] : null);
			if (rankDefinitionSO == null)
			{
				error = "Unlock is unavailable because progression configuration is missing.";
				return false;
			}
			RankProgressionData rankProgressionData = RankProgressionService.Data.Clone();
			RankProgressionService.MigrateTotalXp(rankProgressionData);
			rankProgressionData.creatorBoostApplied = true;
			rankProgressionData.adminRankActive = false;
			if (rankProgressionData.rankIndex != num)
			{
				rankProgressionData.bestQualifyingDistance = 0f;
			}
			rankProgressionData.rankIndex = num;
			rankProgressionData.level = Mathf.Max(rankProgressionData.level, Mathf.Max(50, rankDefinitionSO.minLevel));
			rankProgressionData.storyUnlockedPageCount = 7;
			if (rankProgressionData.unlockFlags == null)
			{
				rankProgressionData.unlockFlags = new List<bool>();
			}
			foreach (object obj in Enum.GetValues(typeof(RankUnlock)))
			{
				int num2 = (int)((RankUnlock)obj);
				while (rankProgressionData.unlockFlags.Count <= num2)
				{
					rankProgressionData.unlockFlags.Add(false);
				}
				rankProgressionData.unlockFlags[num2] = true;
			}
			RankFirstOutfitProgression.Synchronize(rankProgressionData, config);
			RankStoryProgression.Synchronize(rankProgressionData, config);
			if (!RankProgressionService.TryPersistData(rankProgressionData))
			{
				error = "Progress could not be saved. Try again.";
				return false;
			}
			RankProgressionService._data = rankProgressionData;
			RankProgressionService._storyProgressionNeedsSave = false;
			RunModeService.ClearPreparedRun();
			Action changed = RankProgressionService.Changed;
			if (changed != null)
			{
				changed();
			}
			return true;
		}

		// Token: 0x06006737 RID: 26423 RVA: 0x00294C5C File Offset: 0x00292E5C
		public static void ResetAll()
		{
			RunModeService.DisqualifyLeaderboardRun("Progression was reset during this run.");
			RankProgressionService._data = new RankProgressionData();
			RankProgressionService.Save();
		}

		// Token: 0x06006738 RID: 26424 RVA: 0x00294C78 File Offset: 0x00292E78
		public static void SetAdminRank(bool active)
		{
			if (RankProgressionService.Data.adminRankActive == active)
			{
				return;
			}
			RunModeService.DisqualifyLeaderboardRun("Admin rank was changed during this run.");
			RankProgressionService.Data.adminRankActive = active;
			if (active)
			{
				RunModeService.MarkPreparedRunAsAdmin();
			}
			RankProgressionService.Save();
			Debug.Log("[RankProgressionService] AdminRank " + (active ? "ATTIVO" : "disattivato") + " " + string.Format("(rank sottostante: idx {0}).", RankProgressionService.Data.rankIndex));
		}

		// Token: 0x06006739 RID: 26425 RVA: 0x00294CF4 File Offset: 0x00292EF4
		public static void DebugSetRank(int rankIndex)
		{
			RunModeService.DisqualifyLeaderboardRun("Rank was changed with a debug control.");
			RankProgressionConfigSO config = RankProgressionService.Config;
			RankProgressionData data = RankProgressionService.Data;
			RankProgressionService.MigrateTotalXp(data);
			data.adminRankActive = false;
			int num = ((config != null && config.ranksInOrder != null) ? (config.ranksInOrder.Count - 1) : rankIndex);
			data.rankIndex = Mathf.Clamp(rankIndex, 0, Mathf.Max(0, num));
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			data.level = ((currentRank != null) ? Mathf.Max(0, currentRank.minLevel) : 0);
			data.xp = 0f;
			data.bestQualifyingDistance = 0f;
			RankProgressionService.Save();
		}

		// Token: 0x0600673A RID: 26426 RVA: 0x00294D97 File Offset: 0x00292F97
		public static void DebugSetLevel(int level)
		{
			RunModeService.DisqualifyLeaderboardRun("Level was changed with a debug control.");
			RankProgressionService.Data.level = Mathf.Max(0, level);
			RankProgressionService.Save();
		}

		// Token: 0x0600673B RID: 26427 RVA: 0x00294DB9 File Offset: 0x00292FB9
		public static void DebugAddXp(float amount)
		{
			if (amount > 0f)
			{
				RunModeService.DisqualifyLeaderboardRun("Experience was granted with a debug control.");
			}
			RankProgressionService.GrantXp(amount);
		}

		// Token: 0x0600673C RID: 26428 RVA: 0x00294DD4 File Offset: 0x00292FD4
		public static XpGrantOutcome GrantXp(float amount)
		{
			RankProgressionData data = RankProgressionService.Data;
			if (RankProgressionService.SuppressProgressionRewards)
			{
				return RankProgressionService.NoOpGrantOutcome(data);
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			float num = Mathf.Max(0f, amount);
			int level = data.level;
			int rankIndex = data.rankIndex;
			float xp = data.xp;
			RankProgressionService.MigrateTotalXp(data);
			data.xp += num;
			data.totalXpEarned += num;
			RankProgressionService.ApplyLevelUps(data, config);
			if (config != null && !data.adminRankActive && !RankProgressionService.IsPromotionDeferred)
			{
				RankProgressionService.EvaluatePromotion(config, data);
			}
			RankProgressionService.Save();
			bool flag = data.rankIndex != rankIndex;
			if (flag)
			{
				AchievementTrackingService.RecordRank(data.rankIndex, data.adminRankActive);
			}
			return new XpGrantOutcome
			{
				xpGained = num,
				previousLevel = level,
				previousXp = xp,
				newLevel = data.level,
				newXp = data.xp,
				levelsGained = data.level - level,
				rankChanged = flag
			};
		}

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x0600673D RID: 26429 RVA: 0x00294EE4 File Offset: 0x002930E4
		private static bool IsPromotionDeferred
		{
			get
			{
				if (RunModeService.IsAtHome)
				{
					return false;
				}
				RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
				return (rankRuntimeController != null && rankRuntimeController.IsRunInProgress) || ((!(rankRuntimeController != null) || !rankRuntimeController.HasLastRunOutcome) && RunModeService.IsGameplayBoot && (GameOverManager.Instance == null || !GameOverManager.Instance.IsGameOver));
			}
		}

		// Token: 0x0600673E RID: 26430 RVA: 0x00294F4C File Offset: 0x0029314C
		public static bool CompletePendingPromotionAtHome()
		{
			if (!RunModeService.IsAtHome)
			{
				return false;
			}
			RankProgressionData data = RankProgressionService.Data;
			if (data.adminRankActive)
			{
				return false;
			}
			int rankIndex = data.rankIndex;
			RankProgressionService.EvaluatePromotion(RankProgressionService.Config, data);
			if (data.rankIndex == rankIndex)
			{
				return false;
			}
			RankProgressionService.Save();
			AchievementTrackingService.RecordRank(data.rankIndex, data.adminRankActive);
			return true;
		}

		// Token: 0x0600673F RID: 26431 RVA: 0x00294FA8 File Offset: 0x002931A8
		public static XpGrantOutcome GrantMissionXpOnce(string missionId, float amount)
		{
			RankProgressionData data = RankProgressionService.Data;
			if (RankProgressionService.SuppressProgressionRewards)
			{
				return RankProgressionService.NoOpGrantOutcome(data);
			}
			if (string.IsNullOrWhiteSpace(missionId))
			{
				Debug.LogWarning("[RankProgressionService] Mission XP ignored: missing stable mission id.");
				return RankProgressionService.NoOpGrantOutcome(data);
			}
			string text = missionId.Trim();
			if (data.claimedMissionRewardIds == null)
			{
				data.claimedMissionRewardIds = new List<string>();
			}
			for (int i = 0; i < data.claimedMissionRewardIds.Count; i++)
			{
				if (string.Equals(data.claimedMissionRewardIds[i], text, StringComparison.Ordinal))
				{
					return RankProgressionService.NoOpGrantOutcome(data);
				}
			}
			data.claimedMissionRewardIds.Add(text);
			return RankProgressionService.GrantXp(amount);
		}

		// Token: 0x06006740 RID: 26432 RVA: 0x00295040 File Offset: 0x00293240
		internal static bool TryPersistMissionRewardClaim(string missionId)
		{
			if (string.IsNullOrWhiteSpace(missionId))
			{
				return false;
			}
			RankProgressionData data = RankProgressionService.Data;
			return data.claimedMissionRewardIds != null && data.claimedMissionRewardIds.Contains(missionId.Trim()) && RankProgressionService.TryPersistData(data);
		}

		// Token: 0x06006741 RID: 26433 RVA: 0x00295080 File Offset: 0x00293280
		public static void SeedClaimedMissionRewards(IEnumerable<string> missionIds)
		{
			if (missionIds == null)
			{
				return;
			}
			RankProgressionData data = RankProgressionService.Data;
			if (data.claimedMissionRewardIds == null)
			{
				data.claimedMissionRewardIds = new List<string>();
			}
			bool flag = false;
			foreach (string text in missionIds)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					string text2 = text.Trim();
					bool flag2 = false;
					for (int i = 0; i < data.claimedMissionRewardIds.Count; i++)
					{
						if (string.Equals(data.claimedMissionRewardIds[i], text2, StringComparison.Ordinal))
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						data.claimedMissionRewardIds.Add(text2);
						flag = true;
					}
				}
			}
			if (flag)
			{
				RankProgressionService.Save();
			}
		}

		// Token: 0x06006742 RID: 26434 RVA: 0x00295144 File Offset: 0x00293344
		private static XpGrantOutcome NoOpGrantOutcome(RankProgressionData data)
		{
			return new XpGrantOutcome
			{
				xpGained = 0f,
				previousLevel = data.level,
				previousXp = data.xp,
				newLevel = data.level,
				newXp = data.xp,
				levelsGained = 0,
				rankChanged = false
			};
		}

		// Token: 0x06006743 RID: 26435 RVA: 0x002951AA File Offset: 0x002933AA
		private static bool IsFinalRank(RankProgressionData data, RankProgressionConfigSO cfg)
		{
			return data != null && cfg != null && cfg.ranksInOrder != null && cfg.ranksInOrder.Count > 0 && data.rankIndex >= cfg.ranksInOrder.Count - 1;
		}

		// Token: 0x06006744 RID: 26436 RVA: 0x002951E8 File Offset: 0x002933E8
		public static void DebugMarkQualifyingRun()
		{
			RankProgressionConfigSO config = RankProgressionService.Config;
			RankProgressionData data = RankProgressionService.Data;
			if (config == null || config.ranksInOrder == null)
			{
				return;
			}
			RunModeService.DisqualifyLeaderboardRun("A qualifying run was simulated with a debug control.");
			if (data.rankIndex + 1 < config.ranksInOrder.Count)
			{
				RankDefinitionSO rankDefinitionSO = config.ranksInOrder[data.rankIndex + 1];
				if (rankDefinitionSO != null && rankDefinitionSO.promotionRunDistance > data.bestQualifyingDistance)
				{
					data.bestQualifyingDistance = rankDefinitionSO.promotionRunDistance;
				}
			}
			RankProgressionService.EvaluatePromotion(config, data);
			RankProgressionService.Save();
		}

		// Token: 0x040069F3 RID: 27123
		private const string ConfigResourceName = "RankConfig";

		// Token: 0x040069F4 RID: 27124
		private const string ProgressionFileName = "progression.json";

		// Token: 0x040069F5 RID: 27125
		private static RankProgressionData _data;

		// Token: 0x040069F6 RID: 27126
		private static RankProgressionConfigSO _config;

		// Token: 0x040069F7 RID: 27127
		private static bool _configResolved;

		// Token: 0x040069F8 RID: 27128
		private static bool _storyProgressionNeedsSave;
	}
}
