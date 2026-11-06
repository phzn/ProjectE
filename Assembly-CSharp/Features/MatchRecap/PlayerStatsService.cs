using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Bozo.AnimeCharacters;
using Features.Achievements;
using Features.Rank;
using Features.RunModes;
using UnityEngine;

namespace Features.MatchRecap
{
	// Token: 0x02000FE8 RID: 4072
	public static class PlayerStatsService
	{
		// Token: 0x140000DA RID: 218
		// (add) Token: 0x06007B35 RID: 31541 RVA: 0x003253D4 File Offset: 0x003235D4
		// (remove) Token: 0x06007B36 RID: 31542 RVA: 0x00325408 File Offset: 0x00323608
		public static event Action Changed;

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x06007B37 RID: 31543 RVA: 0x0032543B File Offset: 0x0032363B
		private static string FilePath
		{
			get
			{
				return Path.Combine(GameDataPaths.RuntimeRoot, "stats.json");
			}
		}

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x06007B38 RID: 31544 RVA: 0x0032544C File Offset: 0x0032364C
		private static PlayerStatsData Data
		{
			get
			{
				if (PlayerStatsService._data == null)
				{
					PlayerStatsService.Load();
				}
				return PlayerStatsService._data;
			}
		}

		// Token: 0x06007B39 RID: 31545 RVA: 0x0032545F File Offset: 0x0032365F
		public static PlayerStatsData GetSnapshot()
		{
			return PlayerStatsService.Data.Clone();
		}

		// Token: 0x06007B3A RID: 31546 RVA: 0x0032546B File Offset: 0x0032366B
		public static void CommitRun(MatchStatsSnapshot run)
		{
			if (RankProgressionService.SuppressProgressionRewards)
			{
				return;
			}
			PlayerStatsService.AccumulateRun(PlayerStatsService.Data, run, RunModeService.IsRankedRun);
			PlayerStatsService.Save();
		}

		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x06007B3B RID: 31547 RVA: 0x0032548A File Offset: 0x0032368A
		public static int DailyStepGoal
		{
			get
			{
				return PlayerStatsService.Data.dailyStepGoal;
			}
		}

		// Token: 0x06007B3C RID: 31548 RVA: 0x00325496 File Offset: 0x00323696
		public static void SetDailyStepGoal(int goal)
		{
			goal = Mathf.Clamp(goal, 500, 20000);
			if (PlayerStatsService.Data.dailyStepGoal == goal)
			{
				return;
			}
			PlayerStatsService.Data.dailyStepGoal = goal;
			PlayerStatsService.Save();
			AchievementTrackingService.RefreshTodayGoal();
		}

		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x06007B3D RID: 31549 RVA: 0x003254CD File Offset: 0x003236CD
		public static float ActiveGameplaySeconds
		{
			get
			{
				return PlayerStatsService.Data.totalActiveGameplaySeconds;
			}
		}

		// Token: 0x06007B3E RID: 31550 RVA: 0x003254D9 File Offset: 0x003236D9
		public static void AddActiveGameplaySeconds(float seconds)
		{
			if (RankProgressionService.SuppressProgressionRewards)
			{
				return;
			}
			if (seconds > 0f && !float.IsNaN(seconds) && !float.IsInfinity(seconds))
			{
				PlayerStatsService.Data.totalActiveGameplaySeconds += seconds;
			}
		}

		// Token: 0x06007B3F RID: 31551 RVA: 0x0032550D File Offset: 0x0032370D
		public static void FlushActiveGameplayTime()
		{
			PlayerStatsService.Save();
		}

		// Token: 0x06007B40 RID: 31552 RVA: 0x00325514 File Offset: 0x00323714
		private static void AccumulateRun(PlayerStatsData data, MatchStatsSnapshot run, bool ranked)
		{
			data.runsPlayed++;
			if (ranked)
			{
				data.rankedRunsPlayed++;
			}
			float num = Mathf.Max(0f, run.DistanceMeters);
			data.totalDistanceMeters += num;
			data.bestDistanceMeters = Mathf.Max(data.bestDistanceMeters, num);
			data.totalVehicleDistanceMeters += Mathf.Max(0f, run.DistanceWithVehiclesMeters);
			data.totalTimesSeen += Mathf.Max(0, run.TimesSeen);
			data.totalEmotes += Mathf.Max(0, run.TimesExhibitioned);
			data.totalLocationsVisited += Mathf.Max(0, run.LocationsVisited);
			data.bestLocationsVisitedInRun = Mathf.Max(data.bestLocationsVisitedInRun, run.LocationsVisited);
			data.totalCarSurfs += Mathf.Max(0, run.CarSurfCount);
			data.lastAverageBpm = Mathf.Max(0, run.AverageHeartbeatBpm);
			data.bestAverageBpm = Mathf.Max(data.bestAverageBpm, data.lastAverageBpm);
			data.totalPlayTimeSeconds += Mathf.Max(0f, run.PlayTimeSeconds);
			data.totalWalkingSeconds += Mathf.Max(0f, run.WalkingSeconds);
			data.totalRunningSeconds += Mathf.Max(0f, run.RunningSeconds);
			data.totalEmoteSeconds += Mathf.Max(0f, run.EmoteSeconds);
			data.totalVehicleSeconds += Mathf.Max(0f, run.VehicleSeconds);
			data.totalCarSeconds += Mathf.Max(0f, run.CarSeconds);
			data.totalBikeSeconds += Mathf.Max(0f, run.BikeSeconds);
			data.totalBoardSeconds += Mathf.Max(0f, run.BoardSeconds);
			data.totalSkatesSeconds += Mathf.Max(0f, run.SkatesSeconds);
			data.totalRoofSeconds += Mathf.Max(0f, run.RoofSeconds);
			int num2 = Mathf.Max(0, run.Steps);
			data.totalSteps = ((data.totalSteps > int.MaxValue - num2) ? int.MaxValue : (data.totalSteps + num2));
			data.totalWalkingMeters += Mathf.Max(0f, run.WalkingMeters);
			data.totalRunningMeters += Mathf.Max(0f, run.RunningMeters);
			if (run.HealthDays != null && run.HealthDays.Length != 0)
			{
				foreach (HealthDayData healthDayData in run.HealthDays)
				{
					if (healthDayData != null && !string.IsNullOrWhiteSpace(healthDayData.day))
					{
						PlayerStatsService.AddHealthDay(data, healthDayData.day, new MatchStatsSnapshot
						{
							Steps = healthDayData.steps,
							WalkingMeters = healthDayData.walkingMeters,
							RunningMeters = healthDayData.runningMeters
						});
					}
				}
				return;
			}
			PlayerStatsService.AddHealthDay(data, DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), run);
		}

		// Token: 0x06007B41 RID: 31553 RVA: 0x00325860 File Offset: 0x00323A60
		private static void AddHealthDay(PlayerStatsData data, string day, MatchStatsSnapshot run)
		{
			if (data.healthDays == null)
			{
				data.healthDays = new List<HealthDayData>();
			}
			HealthDayData healthDayData = null;
			for (int i = 0; i < data.healthDays.Count; i++)
			{
				if (data.healthDays[i].day == day)
				{
					healthDayData = data.healthDays[i];
					break;
				}
			}
			if (healthDayData == null)
			{
				healthDayData = new HealthDayData
				{
					day = day
				};
				data.healthDays.Add(healthDayData);
			}
			int num = Mathf.Max(0, run.Steps);
			healthDayData.steps = ((healthDayData.steps > int.MaxValue - num) ? int.MaxValue : (healthDayData.steps + num));
			healthDayData.walkingMeters += Mathf.Max(0f, run.WalkingMeters);
			healthDayData.runningMeters += Mathf.Max(0f, run.RunningMeters);
			DateTime oldest = DateTime.Today.AddDays(-13.0);
			data.healthDays.RemoveAll(delegate(HealthDayData item)
			{
				DateTime dateTime;
				return !DateTime.TryParseExact(item.day, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) || dateTime < oldest;
			});
			data.healthDays.Sort((HealthDayData left, HealthDayData right) => string.CompareOrdinal(left.day, right.day));
			if (data.healthDays.Count > 14)
			{
				data.healthDays.RemoveRange(0, data.healthDays.Count - 14);
			}
		}

		// Token: 0x06007B42 RID: 31554 RVA: 0x003259D5 File Offset: 0x00323BD5
		public static void ResetAll()
		{
			PlayerStatsService._data = new PlayerStatsData();
			PlayerStatsService.Save();
		}

		// Token: 0x06007B43 RID: 31555 RVA: 0x003259E6 File Offset: 0x00323BE6
		public static void ReloadFromDisk()
		{
			PlayerStatsService._data = null;
			PlayerStatsService.Load();
			Action changed = PlayerStatsService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06007B44 RID: 31556 RVA: 0x00325A02 File Offset: 0x00323C02
		private static void Load()
		{
			PlayerStatsService._data = PlayerStatsService.LoadData(PlayerStatsService.FilePath);
		}

		// Token: 0x06007B45 RID: 31557 RVA: 0x00325A14 File Offset: 0x00323C14
		private static PlayerStatsData LoadData(string path)
		{
			PlayerStatsData playerStatsData;
			try
			{
				if (File.Exists(path))
				{
					playerStatsData = JsonUtility.FromJson<PlayerStatsData>(File.ReadAllText(path)) ?? new PlayerStatsData();
				}
				else
				{
					playerStatsData = new PlayerStatsData();
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PlayerStatsService] Load failed, using empty stats: " + ex.Message);
				playerStatsData = new PlayerStatsData();
			}
			PlayerStatsService.Migrate(playerStatsData);
			return playerStatsData;
		}

		// Token: 0x06007B46 RID: 31558 RVA: 0x00325A80 File Offset: 0x00323C80
		private static void Save()
		{
			PlayerStatsService.SaveData(PlayerStatsService.FilePath, PlayerStatsService.Data);
			Action changed = PlayerStatsService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06007B47 RID: 31559 RVA: 0x00325AA0 File Offset: 0x00323CA0
		private static void SaveData(string path, PlayerStatsData data)
		{
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
				File.WriteAllText(path, JsonUtility.ToJson(data, true));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PlayerStatsService] Save failed: " + ex.Message);
			}
		}

		// Token: 0x06007B48 RID: 31560 RVA: 0x00325AF0 File Offset: 0x00323CF0
		private static void Migrate(PlayerStatsData data)
		{
			if (data == null)
			{
				return;
			}
			data.runsPlayed = Mathf.Max(0, data.runsPlayed);
			data.rankedRunsPlayed = Mathf.Clamp(data.rankedRunsPlayed, 0, data.runsPlayed);
			data.totalDistanceMeters = Mathf.Max(0f, data.totalDistanceMeters);
			data.bestDistanceMeters = Mathf.Max(0f, data.bestDistanceMeters);
			data.totalVehicleDistanceMeters = Mathf.Max(0f, data.totalVehicleDistanceMeters);
			data.totalTimesSeen = Mathf.Max(0, data.totalTimesSeen);
			data.totalEmotes = Mathf.Max(0, data.totalEmotes);
			data.totalLocationsVisited = Mathf.Max(0, data.totalLocationsVisited);
			data.bestLocationsVisitedInRun = Mathf.Max(0, data.bestLocationsVisitedInRun);
			data.totalCarSurfs = Mathf.Max(0, data.totalCarSurfs);
			data.lastAverageBpm = Mathf.Max(0, data.lastAverageBpm);
			data.bestAverageBpm = Mathf.Max(0, data.bestAverageBpm);
			data.totalPlayTimeSeconds = Mathf.Max(0f, data.totalPlayTimeSeconds);
			data.totalActiveGameplaySeconds = Mathf.Max(0f, data.totalActiveGameplaySeconds);
			data.totalWalkingSeconds = Mathf.Max(0f, data.totalWalkingSeconds);
			data.totalRunningSeconds = Mathf.Max(0f, data.totalRunningSeconds);
			data.totalEmoteSeconds = Mathf.Max(0f, data.totalEmoteSeconds);
			data.totalVehicleSeconds = Mathf.Max(0f, data.totalVehicleSeconds);
			data.totalCarSeconds = Mathf.Max(0f, data.totalCarSeconds);
			data.totalBikeSeconds = Mathf.Max(0f, data.totalBikeSeconds);
			data.totalBoardSeconds = Mathf.Max(0f, data.totalBoardSeconds);
			data.totalSkatesSeconds = Mathf.Max(0f, data.totalSkatesSeconds);
			data.totalRoofSeconds = Mathf.Max(0f, data.totalRoofSeconds);
			data.totalSteps = Mathf.Max(0, data.totalSteps);
			data.totalWalkingMeters = Mathf.Max(0f, data.totalWalkingMeters);
			data.totalRunningMeters = Mathf.Max(0f, data.totalRunningMeters);
			if (data.dailyStepGoal <= 0)
			{
				data.dailyStepGoal = 3000;
			}
			data.dailyStepGoal = Mathf.Clamp(data.dailyStepGoal, 500, 20000);
			if (data.healthDays == null)
			{
				data.healthDays = new List<HealthDayData>();
			}
			data.healthDays.RemoveAll((HealthDayData item) => item == null || string.IsNullOrEmpty(item.day));
			foreach (HealthDayData healthDayData in data.healthDays)
			{
				healthDayData.steps = Mathf.Max(0, healthDayData.steps);
				healthDayData.walkingMeters = Mathf.Max(0f, healthDayData.walkingMeters);
				healthDayData.runningMeters = Mathf.Max(0f, healthDayData.runningMeters);
			}
			data.healthDays.Sort((HealthDayData left, HealthDayData right) => string.CompareOrdinal(left.day, right.day));
			if (data.healthDays.Count > 14)
			{
				data.healthDays.RemoveRange(0, data.healthDays.Count - 14);
			}
			data.schemaVersion = 3;
		}

		// Token: 0x04007EAB RID: 32427
		private const string FileName = "stats.json";

		// Token: 0x04007EAC RID: 32428
		private const int CurrentSchemaVersion = 3;

		// Token: 0x04007EAD RID: 32429
		private static PlayerStatsData _data;
	}
}
