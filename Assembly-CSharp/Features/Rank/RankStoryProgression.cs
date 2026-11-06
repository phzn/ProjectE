using System;
using UnityEngine;

namespace Features.Rank
{
	// Token: 0x02000DB7 RID: 3511
	public static class RankStoryProgression
	{
		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x0600678A RID: 26506 RVA: 0x0029643A File Offset: 0x0029463A
		public static int UnlockedPageCount
		{
			get
			{
				if (!RankProgressionService.IsAdminRankActive)
				{
					return RankProgressionService.GetStoryProgressionData().storyUnlockedPageCount;
				}
				return 7;
			}
		}

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x0600678B RID: 26507 RVA: 0x0029644F File Offset: 0x0029464F
		public static bool IsPersonalDiaryUnlocked
		{
			get
			{
				return RankStoryProgression.UnlockedPageCount >= 7;
			}
		}

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x0600678C RID: 26508 RVA: 0x0029645C File Offset: 0x0029465C
		public static int PendingPageIndex
		{
			get
			{
				if (RankProgressionService.IsAdminRankActive || RankProgressionService.HasCreatorBoost)
				{
					return -1;
				}
				RankProgressionData storyProgressionData = RankProgressionService.GetStoryProgressionData();
				for (int i = 0; i < storyProgressionData.storyUnlockedPageCount; i++)
				{
					if ((storyProgressionData.storyReadPageMask & (1 << i)) == 0)
					{
						return i;
					}
				}
				return -1;
			}
		}

		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x0600678D RID: 26509 RVA: 0x002964A2 File Offset: 0x002946A2
		public static bool HasPendingChapter
		{
			get
			{
				return RankStoryProgression.PendingPageIndex >= 0;
			}
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x002964AF File Offset: 0x002946AF
		public static void EnsureInitialized()
		{
			RankProgressionService.GetStoryProgressionData();
		}

		// Token: 0x0600678F RID: 26511 RVA: 0x002964B8 File Offset: 0x002946B8
		public static bool MarkPageRead(int index)
		{
			if (RankProgressionService.SuppressProgressionRewards)
			{
				return false;
			}
			RankProgressionData storyProgressionData = RankProgressionService.GetStoryProgressionData();
			if (index < 0 || index >= storyProgressionData.storyUnlockedPageCount)
			{
				return false;
			}
			int num = 1 << index;
			if ((storyProgressionData.storyReadPageMask & num) != 0)
			{
				return false;
			}
			storyProgressionData.storyReadPageMask |= num;
			RankProgressionService.Save();
			return true;
		}

		// Token: 0x06006790 RID: 26512 RVA: 0x0029650C File Offset: 0x0029470C
		internal static bool Synchronize(RankProgressionData data, RankProgressionConfigSO config)
		{
			if (data == null)
			{
				return false;
			}
			int storyProgressionVersion = data.storyProgressionVersion;
			int storyUnlockedPageCount = data.storyUnlockedPageCount;
			int storyReadPageMask = data.storyReadPageMask;
			bool storyFirstRunCompleted = data.storyFirstRunCompleted;
			int rankPageCount = RankStoryProgression.GetRankPageCount(data.rankIndex, config);
			if (data.storyProgressionVersion < 1)
			{
				data.storyFirstRunCompleted |= rankPageCount > 1 || data.level > 0 || data.xp > 0f || data.totalXpEarned > 0f || data.bestQualifyingDistance > 0f;
				data.storyProgressionVersion = 1;
			}
			int num = Mathf.Max(RankStoryProgression.ApplyFirstRankRunGate(data, config, rankPageCount), data.storyFirstRunCompleted ? 2 : 1);
			data.storyUnlockedPageCount = Mathf.Clamp(Mathf.Max(data.storyUnlockedPageCount, num), 1, 7);
			data.storyReadPageMask &= (1 << data.storyUnlockedPageCount) - 1;
			return storyProgressionVersion != data.storyProgressionVersion || storyUnlockedPageCount != data.storyUnlockedPageCount || storyReadPageMask != data.storyReadPageMask || storyFirstRunCompleted != data.storyFirstRunCompleted;
		}

		// Token: 0x06006791 RID: 26513 RVA: 0x00296614 File Offset: 0x00294814
		private static int ApplyFirstRankRunGate(RankProgressionData data, RankProgressionConfigSO config, int rankPages)
		{
			if (rankPages <= 1 || config == null || config.ranksInOrder == null || config.ranksInOrder.Count == 0)
			{
				return rankPages;
			}
			int num = Mathf.Clamp(data.rankIndex, 0, config.ranksInOrder.Count - 1);
			RankDefinitionSO rankDefinitionSO = config.ranksInOrder[num];
			if (rankDefinitionSO == null || string.Equals(rankDefinitionSO.rankId, "hornyslut_declared", StringComparison.Ordinal) || RankFirstOutfitProgression.HasCompletedRun(data, rankDefinitionSO.rankId))
			{
				return rankPages;
			}
			return rankPages - 1;
		}

		// Token: 0x06006792 RID: 26514 RVA: 0x0029669C File Offset: 0x0029489C
		private static int GetRankPageCount(int rankIndex, RankProgressionConfigSO config)
		{
			if (config == null || config.ranksInOrder == null || config.ranksInOrder.Count == 0)
			{
				return 1;
			}
			RankDefinitionSO rankDefinitionSO = config.ranksInOrder[Mathf.Clamp(rankIndex, 0, config.ranksInOrder.Count - 1)];
			string text = ((rankDefinitionSO != null) ? rankDefinitionSO.rankId : null);
			if (string.Equals(text, "curious", StringComparison.Ordinal))
			{
				return 3;
			}
			if (string.Equals(text, "aroused", StringComparison.Ordinal))
			{
				return 4;
			}
			if (string.Equals(text, "horny", StringComparison.Ordinal))
			{
				return 5;
			}
			if (string.Equals(text, "hornyslut", StringComparison.Ordinal))
			{
				return 6;
			}
			if (string.Equals(text, "hornyslut_declared", StringComparison.Ordinal))
			{
				return 7;
			}
			return 1;
		}

		// Token: 0x04006A1D RID: 27165
		public const int PageCount = 7;

		// Token: 0x04006A1E RID: 27166
		private const int CurrentVersion = 1;
	}
}
