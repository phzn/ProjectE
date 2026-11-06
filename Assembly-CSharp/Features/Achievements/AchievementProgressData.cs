using System;
using System.Collections.Generic;

namespace Features.Achievements
{
	// Token: 0x02001536 RID: 5430
	[Serializable]
	public sealed class AchievementProgressData
	{
		// Token: 0x0400ACF3 RID: 44275
		public int version = 1;

		// Token: 0x0400ACF4 RID: 44276
		public double onFootMeters;

		// Token: 0x0400ACF5 RID: 44277
		public int npcPhotos;

		// Token: 0x0400ACF6 RID: 44278
		public int trashcans;

		// Token: 0x0400ACF7 RID: 44279
		public int clothes;

		// Token: 0x0400ACF8 RID: 44280
		public int groupPhotos;

		// Token: 0x0400ACF9 RID: 44281
		public int posters;

		// Token: 0x0400ACFA RID: 44282
		public List<string> visitedPoiIds = new List<string>();

		// Token: 0x0400ACFB RID: 44283
		public List<string> unlockedIds = new List<string>();

		// Token: 0x0400ACFC RID: 44284
		public List<AchievementDay> days = new List<AchievementDay>();
	}
}
