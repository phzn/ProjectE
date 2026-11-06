using System;
using System.Collections.Generic;
using Features.Inventory.Wearable;

namespace Features.Rank
{
	// Token: 0x02000DA7 RID: 3495
	public static class RankFirstOutfitProgression
	{
		// Token: 0x060066EC RID: 26348 RVA: 0x002928FC File Offset: 0x00290AFC
		public static bool IntroducesNewOutfit(RankProgressionConfigSO config, int rankIndex)
		{
			if (config == null || config.ranksInOrder == null || rankIndex < 0 || rankIndex >= config.ranksInOrder.Count)
			{
				return false;
			}
			RankDefinitionSO rankDefinitionSO = config.ranksInOrder[rankIndex];
			if (rankDefinitionSO == null || !rankDefinitionSO.startOutfitReplacesCustom || rankDefinitionSO.startOutfitItems == null)
			{
				return false;
			}
			foreach (WearableItemSO wearableItemSO in rankDefinitionSO.startOutfitItems)
			{
				if (!(wearableItemSO == null))
				{
					bool flag = false;
					for (int i = 0; i < rankIndex; i++)
					{
						RankDefinitionSO rankDefinitionSO2 = config.ranksInOrder[i];
						if (!(rankDefinitionSO2 == null) && rankDefinitionSO2.startOutfitItems != null && rankDefinitionSO2.startOutfitItems.Contains(wearableItemSO))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060066ED RID: 26349 RVA: 0x002929F4 File Offset: 0x00290BF4
		public static bool HasCompletedRun(RankProgressionData data, string rankId)
		{
			return data != null && !string.IsNullOrEmpty(rankId) && data.completedOutfitRankIds != null && data.completedOutfitRankIds.Contains(rankId);
		}

		// Token: 0x060066EE RID: 26350 RVA: 0x00292A17 File Offset: 0x00290C17
		internal static bool RecordCompletedRun(RankProgressionData data, string rankId)
		{
			if (data == null || string.IsNullOrEmpty(rankId) || RankFirstOutfitProgression.HasCompletedRun(data, rankId))
			{
				return false;
			}
			if (data.completedOutfitRankIds == null)
			{
				data.completedOutfitRankIds = new List<string>();
			}
			data.completedOutfitRankIds.Add(rankId);
			return true;
		}

		// Token: 0x060066EF RID: 26351 RVA: 0x00292A50 File Offset: 0x00290C50
		public static bool Synchronize(RankProgressionData data, RankProgressionConfigSO config)
		{
			if (data == null)
			{
				return false;
			}
			bool flag = false;
			if (data.completedOutfitRankIds == null)
			{
				data.completedOutfitRankIds = new List<string>();
				flag = true;
			}
			if (data.firstOutfitProgressionVersion >= 1)
			{
				return flag;
			}
			if (config != null && config.ranksInOrder != null)
			{
				int num = 0;
				while (num < config.ranksInOrder.Count && num <= data.rankIndex)
				{
					RankDefinitionSO rankDefinitionSO = config.ranksInOrder[num];
					if (!(rankDefinitionSO == null) && (num < data.rankIndex || data.bestQualifyingDistance > 0f || (num == 0 && data.storyFirstRunCompleted)))
					{
						RankFirstOutfitProgression.RecordCompletedRun(data, rankDefinitionSO.rankId);
					}
					num++;
				}
			}
			data.firstOutfitProgressionVersion = 1;
			return true;
		}
	}
}
