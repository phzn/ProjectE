using System;

namespace Features.Rank
{
	// Token: 0x02000DB1 RID: 3505
	public struct RunOutcome
	{
		// Token: 0x040069E0 RID: 27104
		public float xpGained;

		// Token: 0x040069E1 RID: 27105
		public float baseXpGained;

		// Token: 0x040069E2 RID: 27106
		public float scoreXpGained;

		// Token: 0x040069E3 RID: 27107
		public int scorePoints;

		// Token: 0x040069E4 RID: 27108
		public int scoreDistanceMultiplier;

		// Token: 0x040069E5 RID: 27109
		public int previousLevel;

		// Token: 0x040069E6 RID: 27110
		public float previousXp;

		// Token: 0x040069E7 RID: 27111
		public int levelsGained;

		// Token: 0x040069E8 RID: 27112
		public bool rankChanged;

		// Token: 0x040069E9 RID: 27113
		public int newLevel;

		// Token: 0x040069EA RID: 27114
		public int newRankIndex;

		// Token: 0x040069EB RID: 27115
		public float totalXpAfter;
	}
}
