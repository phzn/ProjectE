using System;
using System.Globalization;
using Features.Missions;

namespace Features.MatchRecap
{
	// Token: 0x02000FDC RID: 4060
	public struct MatchStatsSnapshot
	{
		// Token: 0x06007AA7 RID: 31399 RVA: 0x0032180C File Offset: 0x0031FA0C
		public int StepsOnDay(DateTime day)
		{
			if (this.HealthDays != null && this.HealthDays.Length != 0)
			{
				string text = day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
				foreach (HealthDayData healthDayData in this.HealthDays)
				{
					if (healthDayData != null && healthDayData.day == text)
					{
						return Math.Max(0, healthDayData.steps);
					}
				}
				return 0;
			}
			if (!(day.Date == DateTime.Today))
			{
				return 0;
			}
			return Math.Max(0, this.Steps);
		}

		// Token: 0x04007DEE RID: 32238
		public float DistanceMeters;

		// Token: 0x04007DEF RID: 32239
		public float DistanceWithVehiclesMeters;

		// Token: 0x04007DF0 RID: 32240
		public int TimesSeen;

		// Token: 0x04007DF1 RID: 32241
		public int TimesExhibitioned;

		// Token: 0x04007DF2 RID: 32242
		public int LocationsVisited;

		// Token: 0x04007DF3 RID: 32243
		public int AverageHeartbeatBpm;

		// Token: 0x04007DF4 RID: 32244
		public int Points;

		// Token: 0x04007DF5 RID: 32245
		public float PeakDistanceMeters;

		// Token: 0x04007DF6 RID: 32246
		public int HomeReturnBonus;

		// Token: 0x04007DF7 RID: 32247
		public int HomeReturnMissionsCompleted;

		// Token: 0x04007DF8 RID: 32248
		public float HomeReturnMissionXp;

		// Token: 0x04007DF9 RID: 32249
		public MissionCompletion[] HomeReturnMissionRewards;

		// Token: 0x04007DFA RID: 32250
		public int CarSurfCount;

		// Token: 0x04007DFB RID: 32251
		public float PlayTimeSeconds;

		// Token: 0x04007DFC RID: 32252
		public float WalkingSeconds;

		// Token: 0x04007DFD RID: 32253
		public float RunningSeconds;

		// Token: 0x04007DFE RID: 32254
		public float EmoteSeconds;

		// Token: 0x04007DFF RID: 32255
		public float VehicleSeconds;

		// Token: 0x04007E00 RID: 32256
		public float CarSeconds;

		// Token: 0x04007E01 RID: 32257
		public float BikeSeconds;

		// Token: 0x04007E02 RID: 32258
		public float BoardSeconds;

		// Token: 0x04007E03 RID: 32259
		public float SkatesSeconds;

		// Token: 0x04007E04 RID: 32260
		public float RoofSeconds;

		// Token: 0x04007E05 RID: 32261
		public int Steps;

		// Token: 0x04007E06 RID: 32262
		public HealthDayData[] HealthDays;

		// Token: 0x04007E07 RID: 32263
		public float WalkingMeters;

		// Token: 0x04007E08 RID: 32264
		public float RunningMeters;
	}
}
