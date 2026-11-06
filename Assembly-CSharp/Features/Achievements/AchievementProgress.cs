using System;
using System.Collections.Generic;
using System.Globalization;
using Features.Missions;

namespace Features.Achievements
{
	// Token: 0x02001537 RID: 5431
	public sealed class AchievementProgress
	{
		// Token: 0x170019E0 RID: 6624
		// (get) Token: 0x0600A817 RID: 43031 RVA: 0x00463083 File Offset: 0x00461283
		public AchievementProgressData Data { get; }

		// Token: 0x0600A818 RID: 43032 RVA: 0x0046308C File Offset: 0x0046128C
		public AchievementProgress(AchievementProgressData data, Action<string> unlock)
		{
			this.Data = data ?? new AchievementProgressData();
			if (unlock == null)
			{
				throw new ArgumentNullException("unlock");
			}
			this._unlock = unlock;
			if (this.Data.visitedPoiIds == null)
			{
				this.Data.visitedPoiIds = new List<string>();
			}
			if (this.Data.unlockedIds == null)
			{
				this.Data.unlockedIds = new List<string>();
			}
			if (this.Data.days == null)
			{
				this.Data.days = new List<AchievementDay>();
			}
			if (double.IsNaN(this.Data.onFootMeters) || double.IsInfinity(this.Data.onFootMeters))
			{
				this.Data.onFootMeters = 0.0;
			}
			this.Data.onFootMeters = Math.Max(0.0, this.Data.onFootMeters);
			this.Data.npcPhotos = Math.Max(0, this.Data.npcPhotos);
			this.Data.trashcans = Math.Max(0, this.Data.trashcans);
			this.Data.clothes = Math.Max(0, this.Data.clothes);
			this.Data.groupPhotos = Math.Max(0, this.Data.groupPhotos);
			this.Data.posters = Math.Max(0, this.Data.posters);
			this._visited = new HashSet<string>(this.Data.visitedPoiIds, StringComparer.OrdinalIgnoreCase);
			this._unlocked = new HashSet<string>(this.Data.unlockedIds, StringComparer.Ordinal);
		}

		// Token: 0x0600A819 RID: 43033 RVA: 0x0046325C File Offset: 0x0046145C
		public void SetRequiredPois(IEnumerable<string> ids)
		{
			this._requiredPoi.Clear();
			if (ids != null)
			{
				foreach (string text in ids)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						this._requiredPoi.Add(text.Trim());
					}
				}
			}
			this.CheckPois();
		}

		// Token: 0x0600A81A RID: 43034 RVA: 0x004632CC File Offset: 0x004614CC
		public void BeginRun(bool headphonesWorn)
		{
			this._runActive = true;
			this._runVisited.Clear();
			if (headphonesWorn)
			{
				this.Unlock("ACH_HEY_DJ");
			}
		}

		// Token: 0x0600A81B RID: 43035 RVA: 0x004632EE File Offset: 0x004614EE
		public void EndRun()
		{
			this._runActive = false;
			this._runVisited.Clear();
		}

		// Token: 0x0600A81C RID: 43036 RVA: 0x00463304 File Offset: 0x00461504
		public void AddTravel(double meters)
		{
			if (!this._runActive || !AchievementProgress.ValidPositive(meters))
			{
				return;
			}
			this.Data.onFootMeters = Math.Min(double.MaxValue, this.Data.onFootMeters + meters);
			this.CheckThresholds(this.Data.onFootMeters, AchievementProgress.DistanceTargets, AchievementProgress.DistanceIds);
		}

		// Token: 0x0600A81D RID: 43037 RVA: 0x00463364 File Offset: 0x00461564
		public void ReportMission(MissionObjectiveKind kind, float amount, string qualifierId)
		{
			if (!this._runActive || !AchievementProgress.ValidPositive((double)amount))
			{
				return;
			}
			int num = ((amount >= 2.1474836E+09f) ? int.MaxValue : ((int)amount));
			if (kind <= MissionObjectiveKind.PoiDiscoveries)
			{
				if (kind == MissionObjectiveKind.NpcPhotos)
				{
					this.Data.npcPhotos = AchievementProgress.Add(this.Data.npcPhotos, num);
					this.CheckThresholds((double)this.Data.npcPhotos, AchievementProgress.CountTargets, AchievementProgress.PhotoIds);
					return;
				}
				if (kind != MissionObjectiveKind.PoiDiscoveries)
				{
					return;
				}
				this.RecordPoi(qualifierId, true);
				return;
			}
			else
			{
				if (kind == MissionObjectiveKind.TrashcanSearches)
				{
					this.Data.trashcans = AchievementProgress.Add(this.Data.trashcans, num);
					this.CheckThresholds((double)this.Data.trashcans, AchievementProgress.CountTargets, AchievementProgress.TrashIds);
					return;
				}
				if (kind == MissionObjectiveKind.ClothesCollected)
				{
					this.Data.clothes = AchievementProgress.Add(this.Data.clothes, 1);
					this.CheckClothes();
					return;
				}
				if (kind != MissionObjectiveKind.SafeHomeReturnDistance)
				{
					return;
				}
				this.CheckThresholds((double)amount, AchievementProgress.HomeTargets, AchievementProgress.HomeIds);
				return;
			}
		}

		// Token: 0x0600A81E RID: 43038 RVA: 0x00463468 File Offset: 0x00461668
		public void SeedAccountProgress(double meters, int photos, int trashcans, int clothes)
		{
			if (AchievementProgress.ValidPositive(meters))
			{
				this.Data.onFootMeters = Math.Max(this.Data.onFootMeters, meters);
			}
			this.Data.npcPhotos = Math.Max(this.Data.npcPhotos, photos);
			this.Data.trashcans = Math.Max(this.Data.trashcans, trashcans);
			this.Data.clothes = Math.Max(this.Data.clothes, clothes);
			this.CheckThresholds(this.Data.onFootMeters, AchievementProgress.DistanceTargets, AchievementProgress.DistanceIds);
			this.CheckThresholds((double)this.Data.npcPhotos, AchievementProgress.CountTargets, AchievementProgress.PhotoIds);
			this.CheckThresholds((double)this.Data.trashcans, AchievementProgress.CountTargets, AchievementProgress.TrashIds);
			this.CheckClothes();
		}

		// Token: 0x0600A81F RID: 43039 RVA: 0x00463548 File Offset: 0x00461748
		public void RecordPoi(string id, bool currentRun)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return;
			}
			id = id.Trim();
			if (this._visited.Add(id))
			{
				this.Data.visitedPoiIds.Add(id);
			}
			if (currentRun && this._runActive)
			{
				this._runVisited.Add(id);
			}
			this.CheckPois();
		}

		// Token: 0x0600A820 RID: 43040 RVA: 0x004635A4 File Offset: 0x004617A4
		private void CheckPois()
		{
			if (this._requiredPoi.Count == 0)
			{
				return;
			}
			if (this._requiredPoi.IsSubsetOf(this._visited))
			{
				this.Unlock("ACH_EXPLORER");
			}
			if (this._runActive && this._requiredPoi.IsSubsetOf(this._runVisited))
			{
				this.Unlock("ACH_HUMAN_EXPLORER");
			}
		}

		// Token: 0x0600A821 RID: 43041 RVA: 0x00463603 File Offset: 0x00461803
		private void CheckClothes()
		{
			if (this.Data.clothes >= 20)
			{
				this.Unlock("ACH_CLOTHES_20");
			}
			if (this.Data.clothes >= 50)
			{
				this.Unlock("ACH_CLOTHES_50");
			}
		}

		// Token: 0x0600A822 RID: 43042 RVA: 0x00463639 File Offset: 0x00461839
		public void CheckWardrobe(int unlockedVariants, int requiredVariants)
		{
			if (requiredVariants > 0 && unlockedVariants >= requiredVariants)
			{
				this.Unlock("ACH_CLOTHES_ALL");
			}
		}

		// Token: 0x0600A823 RID: 43043 RVA: 0x00463650 File Offset: 0x00461850
		public void RecordRank(int rankIndex, bool admin)
		{
			if (admin)
			{
				return;
			}
			int num = 0;
			while (num < AchievementProgress.RankIds.Length && num < rankIndex)
			{
				this.Unlock(AchievementProgress.RankIds[num]);
				num++;
			}
		}

		// Token: 0x0600A824 RID: 43044 RVA: 0x00463684 File Offset: 0x00461884
		public void RecordGroupPhoto(bool isGroup, bool hasPhoto)
		{
			if (!isGroup || !hasPhoto)
			{
				return;
			}
			this.Data.groupPhotos = AchievementProgress.Add(this.Data.groupPhotos, 1);
			if (this.Data.groupPhotos >= 20)
			{
				this.Unlock("ACH_RISKY_HOBBY");
			}
		}

		// Token: 0x0600A825 RID: 43045 RVA: 0x004636C4 File Offset: 0x004618C4
		public void RecordPosterDestroyed()
		{
			if (!this._runActive)
			{
				return;
			}
			this.Data.posters = AchievementProgress.Add(this.Data.posters, 1);
			if (this.Data.posters >= 100)
			{
				this.Unlock("ACH_WANTED");
			}
		}

		// Token: 0x0600A826 RID: 43046 RVA: 0x00463710 File Offset: 0x00461910
		public void RecordCustomPaint(bool consumedPen, bool hasPaint)
		{
			if (consumedPen && hasPaint)
			{
				this.Unlock("ACH_CHAMELEON");
			}
		}

		// Token: 0x0600A827 RID: 43047 RVA: 0x00463722 File Offset: 0x00461922
		public void RecordReporter()
		{
			if (this._runActive)
			{
				this.Unlock("ACH_YOU_ARE_LIVE");
			}
		}

		// Token: 0x0600A828 RID: 43048 RVA: 0x00463737 File Offset: 0x00461937
		public void RecordPrisonArrival()
		{
			this.Unlock("ACH_FIRST_PRISON");
		}

		// Token: 0x0600A829 RID: 43049 RVA: 0x00463744 File Offset: 0x00461944
		public void RecordLongWayHomeWin()
		{
			if (this._runActive)
			{
				this.Unlock("ACH_LONG_WAY_HOME");
			}
		}

		// Token: 0x0600A82A RID: 43050 RVA: 0x0046375C File Offset: 0x0046195C
		public void AddStep(DateTime day, int goal)
		{
			if (!this._runActive)
			{
				return;
			}
			AchievementDay day2 = this.GetDay(day);
			this.RecordDailySteps(day, AchievementProgress.Add(day2.steps, 1), goal);
		}

		// Token: 0x0600A82B RID: 43051 RVA: 0x00463790 File Offset: 0x00461990
		public void RecordDailySteps(DateTime day, int steps, int goal)
		{
			if (steps < 0 || goal <= 0)
			{
				return;
			}
			AchievementDay day2 = this.GetDay(day);
			day2.steps = Math.Max(day2.steps, steps);
			if (!day2.completed)
			{
				day2.goal = goal;
				day2.completed = day2.steps >= goal;
			}
			HashSet<DateTime> hashSet = new HashSet<DateTime>();
			foreach (AchievementDay achievementDay in this.Data.days)
			{
				DateTime dateTime;
				if (achievementDay != null && achievementDay.completed && DateTime.TryParseExact(achievementDay.date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
				{
					hashSet.Add(dateTime.Date);
				}
			}
			int num = 0;
			DateTime dateTime2 = day.Date;
			while (num < 7 && hashSet.Contains(dateTime2))
			{
				num++;
				dateTime2 = dateTime2.AddDays(-1.0);
			}
			if (num >= 7)
			{
				this.Unlock("ACH_HEALTH_STREAK_7");
			}
			this.Data.days.RemoveAll(delegate(AchievementDay item)
			{
				DateTime dateTime3;
				return item == null || !DateTime.TryParseExact(item.date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime3) || dateTime3 < day.Date.AddDays(-30.0);
			});
		}

		// Token: 0x0600A82C RID: 43052 RVA: 0x004638D8 File Offset: 0x00461AD8
		private AchievementDay GetDay(DateTime day)
		{
			string text = day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
			foreach (AchievementDay achievementDay in this.Data.days)
			{
				if (achievementDay != null && achievementDay.date == text)
				{
					return achievementDay;
				}
			}
			AchievementDay achievementDay2 = new AchievementDay
			{
				date = text
			};
			this.Data.days.Add(achievementDay2);
			return achievementDay2;
		}

		// Token: 0x0600A82D RID: 43053 RVA: 0x00463974 File Offset: 0x00461B74
		private void CheckThresholds(double value, int[] targets, string[] ids)
		{
			for (int i = 0; i < targets.Length; i++)
			{
				if (value >= (double)targets[i])
				{
					this.Unlock(ids[i]);
				}
			}
		}

		// Token: 0x0600A82E RID: 43054 RVA: 0x0046399F File Offset: 0x00461B9F
		private void Unlock(string id)
		{
			if (!this._unlocked.Add(id))
			{
				return;
			}
			this.Data.unlockedIds.Add(id);
			this._unlock(id);
		}

		// Token: 0x0600A82F RID: 43055 RVA: 0x004639CD File Offset: 0x00461BCD
		private static int Add(int value, int amount)
		{
			if (amount <= 0)
			{
				return value;
			}
			if (value <= 2147483647 - amount)
			{
				return value + amount;
			}
			return int.MaxValue;
		}

		// Token: 0x0600A830 RID: 43056 RVA: 0x004639E8 File Offset: 0x00461BE8
		private static bool ValidPositive(double value)
		{
			return value > 0.0 && !double.IsNaN(value) && !double.IsInfinity(value);
		}

		// Token: 0x0400ACFE RID: 44286
		private readonly Action<string> _unlock;

		// Token: 0x0400ACFF RID: 44287
		private readonly HashSet<string> _unlocked;

		// Token: 0x0400AD00 RID: 44288
		private readonly HashSet<string> _visited;

		// Token: 0x0400AD01 RID: 44289
		private readonly HashSet<string> _runVisited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400AD02 RID: 44290
		private readonly HashSet<string> _requiredPoi = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400AD03 RID: 44291
		private bool _runActive;

		// Token: 0x0400AD04 RID: 44292
		private static readonly int[] DistanceTargets = new int[] { 500, 2000, 5000, 10000, 50000 };

		// Token: 0x0400AD05 RID: 44293
		private static readonly string[] DistanceIds = new string[] { "ACH_DISTANCE_500", "ACH_DISTANCE_2000", "ACH_DISTANCE_5000", "ACH_DISTANCE_10000", "ACH_DISTANCE_50000" };

		// Token: 0x0400AD06 RID: 44294
		private static readonly int[] CountTargets = new int[] { 50, 200, 500, 1000 };

		// Token: 0x0400AD07 RID: 44295
		private static readonly string[] PhotoIds = new string[] { "ACH_NPC_PHOTOS_50", "ACH_NPC_PHOTOS_200", "ACH_NPC_PHOTOS_500", "ACH_NPC_PHOTOS_1000" };

		// Token: 0x0400AD08 RID: 44296
		private static readonly string[] TrashIds = new string[] { "ACH_TRASHCANS_50", "ACH_TRASHCANS_200", "ACH_TRASHCANS_500", "ACH_TRASHCANS_1000" };

		// Token: 0x0400AD09 RID: 44297
		private static readonly int[] HomeTargets = new int[] { 1000, 2000, 5000, 10000 };

		// Token: 0x0400AD0A RID: 44298
		private static readonly string[] HomeIds = new string[] { "ACH_HOME_1000", "ACH_HOME_2000", "ACH_HOME_5000", "ACH_HOME_10000" };

		// Token: 0x0400AD0B RID: 44299
		private static readonly string[] RankIds = new string[] { "ACH_RANK_CURIOUS", "ACH_RANK_AROUSED", "ACH_RANK_HORNY", "ACH_RANK_HORNY_SLUT", "ACH_RANK_CERTIFIED" };
	}
}
