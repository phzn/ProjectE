using System;
using System.Collections.Generic;

namespace Features.MatchRecap
{
	// Token: 0x02000FE7 RID: 4071
	[Serializable]
	public class PlayerStatsData
	{
		// Token: 0x06007B33 RID: 31539 RVA: 0x003252E4 File Offset: 0x003234E4
		public PlayerStatsData Clone()
		{
			PlayerStatsData playerStatsData = (PlayerStatsData)base.MemberwiseClone();
			playerStatsData.healthDays = new List<HealthDayData>((this.healthDays != null) ? this.healthDays.Count : 0);
			if (this.healthDays != null)
			{
				foreach (HealthDayData healthDayData in this.healthDays)
				{
					if (healthDayData != null)
					{
						playerStatsData.healthDays.Add(new HealthDayData
						{
							day = healthDayData.day,
							steps = healthDayData.steps,
							walkingMeters = healthDayData.walkingMeters,
							runningMeters = healthDayData.runningMeters
						});
					}
				}
			}
			return playerStatsData;
		}

		// Token: 0x04007E8E RID: 32398
		public int schemaVersion = 3;

		// Token: 0x04007E8F RID: 32399
		public int runsPlayed;

		// Token: 0x04007E90 RID: 32400
		public int rankedRunsPlayed;

		// Token: 0x04007E91 RID: 32401
		public float totalDistanceMeters;

		// Token: 0x04007E92 RID: 32402
		public float bestDistanceMeters;

		// Token: 0x04007E93 RID: 32403
		public float totalVehicleDistanceMeters;

		// Token: 0x04007E94 RID: 32404
		public int totalTimesSeen;

		// Token: 0x04007E95 RID: 32405
		public int totalEmotes;

		// Token: 0x04007E96 RID: 32406
		public int totalLocationsVisited;

		// Token: 0x04007E97 RID: 32407
		public int bestLocationsVisitedInRun;

		// Token: 0x04007E98 RID: 32408
		public int totalCarSurfs;

		// Token: 0x04007E99 RID: 32409
		public int lastAverageBpm;

		// Token: 0x04007E9A RID: 32410
		public int bestAverageBpm;

		// Token: 0x04007E9B RID: 32411
		public float totalPlayTimeSeconds;

		// Token: 0x04007E9C RID: 32412
		public float totalActiveGameplaySeconds;

		// Token: 0x04007E9D RID: 32413
		public float totalWalkingSeconds;

		// Token: 0x04007E9E RID: 32414
		public float totalRunningSeconds;

		// Token: 0x04007E9F RID: 32415
		public float totalEmoteSeconds;

		// Token: 0x04007EA0 RID: 32416
		public float totalVehicleSeconds;

		// Token: 0x04007EA1 RID: 32417
		public float totalCarSeconds;

		// Token: 0x04007EA2 RID: 32418
		public float totalBikeSeconds;

		// Token: 0x04007EA3 RID: 32419
		public float totalBoardSeconds;

		// Token: 0x04007EA4 RID: 32420
		public float totalSkatesSeconds;

		// Token: 0x04007EA5 RID: 32421
		public float totalRoofSeconds;

		// Token: 0x04007EA6 RID: 32422
		public int totalSteps;

		// Token: 0x04007EA7 RID: 32423
		public float totalWalkingMeters;

		// Token: 0x04007EA8 RID: 32424
		public float totalRunningMeters;

		// Token: 0x04007EA9 RID: 32425
		public int dailyStepGoal = 3000;

		// Token: 0x04007EAA RID: 32426
		public List<HealthDayData> healthDays = new List<HealthDayData>();
	}
}
