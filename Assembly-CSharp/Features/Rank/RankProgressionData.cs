using System;
using System.Collections.Generic;

namespace Features.Rank
{
	// Token: 0x02000DAE RID: 3502
	[Serializable]
	public class RankProgressionData
	{
		// Token: 0x06006714 RID: 26388 RVA: 0x00293E94 File Offset: 0x00292094
		public RankProgressionData Clone()
		{
			return new RankProgressionData
			{
				level = this.level,
				xp = this.xp,
				rankIndex = this.rankIndex,
				totalXpEarned = this.totalXpEarned,
				bestQualifyingDistance = this.bestQualifyingDistance,
				unlockFlags = ((this.unlockFlags != null) ? new List<bool>(this.unlockFlags) : new List<bool>()),
				claimedMissionRewardIds = ((this.claimedMissionRewardIds != null) ? new List<string>(this.claimedMissionRewardIds) : new List<string>()),
				adminRankActive = this.adminRankActive,
				creatorBoostApplied = this.creatorBoostApplied,
				storyProgressionVersion = this.storyProgressionVersion,
				storyUnlockedPageCount = this.storyUnlockedPageCount,
				storyReadPageMask = this.storyReadPageMask,
				storyFirstRunCompleted = this.storyFirstRunCompleted,
				firstOutfitProgressionVersion = this.firstOutfitProgressionVersion,
				customOutfitRankId = this.customOutfitRankId,
				completedOutfitRankIds = ((this.completedOutfitRankIds != null) ? new List<string>(this.completedOutfitRankIds) : new List<string>()),
				pcAdvancedAppsNoticeSeen = this.pcAdvancedAppsNoticeSeen,
				personalDiaryNoticeSeen = this.personalDiaryNoticeSeen
			};
		}

		// Token: 0x040069C3 RID: 27075
		public int level;

		// Token: 0x040069C4 RID: 27076
		public float xp;

		// Token: 0x040069C5 RID: 27077
		public int rankIndex;

		// Token: 0x040069C6 RID: 27078
		public float totalXpEarned;

		// Token: 0x040069C7 RID: 27079
		public float bestQualifyingDistance;

		// Token: 0x040069C8 RID: 27080
		public List<bool> unlockFlags = new List<bool>();

		// Token: 0x040069C9 RID: 27081
		public List<string> claimedMissionRewardIds = new List<string>();

		// Token: 0x040069CA RID: 27082
		public bool adminRankActive;

		// Token: 0x040069CB RID: 27083
		public bool creatorBoostApplied;

		// Token: 0x040069CC RID: 27084
		public int storyProgressionVersion;

		// Token: 0x040069CD RID: 27085
		public int storyUnlockedPageCount;

		// Token: 0x040069CE RID: 27086
		public int storyReadPageMask;

		// Token: 0x040069CF RID: 27087
		public bool storyFirstRunCompleted;

		// Token: 0x040069D0 RID: 27088
		public int firstOutfitProgressionVersion;

		// Token: 0x040069D1 RID: 27089
		public List<string> completedOutfitRankIds = new List<string>();

		// Token: 0x040069D2 RID: 27090
		public string customOutfitRankId;

		// Token: 0x040069D3 RID: 27091
		public bool pcAdvancedAppsNoticeSeen;

		// Token: 0x040069D4 RID: 27092
		public bool personalDiaryNoticeSeen;
	}
}
