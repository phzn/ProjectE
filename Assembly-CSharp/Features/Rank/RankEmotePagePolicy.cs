using System;
using Features.Detection;
using Features.MainMenu;
using Features.PlayerEmotes;
using UnityEngine;

namespace Features.Rank
{
	// Token: 0x02000DA5 RID: 3493
	public static class RankEmotePagePolicy
	{
		// Token: 0x060066BB RID: 26299 RVA: 0x002912A3 File Offset: 0x0028F4A3
		public static int UnlockedPages(RankDefinitionSO rank, int pageCount)
		{
			if (rank == null)
			{
				return Mathf.Max(0, pageCount);
			}
			if (!rank.allowEmoteWheel)
			{
				return 0;
			}
			if (rank.unlockedEmoteWheelPages >= 0)
			{
				return Mathf.Clamp(rank.unlockedEmoteWheelPages, 0, pageCount);
			}
			return Mathf.Max(0, pageCount);
		}

		// Token: 0x060066BC RID: 26300 RVA: 0x002912DE File Offset: 0x0028F4DE
		public static bool IsPageAvailable(RankDefinitionSO rank, int page, int pageCount)
		{
			return page >= 0 && page < RankEmotePagePolicy.UnlockedPages(rank, pageCount);
		}

		// Token: 0x060066BD RID: 26301 RVA: 0x002912F0 File Offset: 0x0028F4F0
		public static bool MatchesAvatar(EmoteDefinitionSO emote, bool futanari)
		{
			if (!(emote != null))
			{
				return false;
			}
			if (emote.FutanariRequirement == FutanariRequirement.Always)
			{
				return true;
			}
			if (!futanari)
			{
				return emote.FutanariRequirement == FutanariRequirement.NoFutanari;
			}
			return emote.FutanariRequirement == FutanariRequirement.FutanariOnly;
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x00291320 File Offset: 0x0028F520
		public static int PageOf(GameplayEmoteCollectionSO collection, EmoteDefinitionSO emote, bool futanari)
		{
			if (collection == null || emote == null)
			{
				return -1;
			}
			int num = 0;
			foreach (EmoteDefinitionSO emoteDefinitionSO in collection.Emotes)
			{
				if (RankEmotePagePolicy.MatchesAvatar(emoteDefinitionSO, futanari) && CollectionUnlockRegistry.IsEmoteUnlocked(emoteDefinitionSO))
				{
					if (emoteDefinitionSO == emote)
					{
						return num / 6;
					}
					num++;
				}
			}
			return -1;
		}

		// Token: 0x060066BF RID: 26303 RVA: 0x002913AC File Offset: 0x0028F5AC
		public static bool IsEmoteAvailable(RankDefinitionSO rank, GameplayEmoteCollectionSO collection, EmoteDefinitionSO emote, bool futanari)
		{
			int num = RankEmotePagePolicy.PageOf(collection, emote, futanari);
			return num < 0 || RankEmotePagePolicy.IsPageAvailable(rank, num, int.MaxValue);
		}

		// Token: 0x0400696E RID: 26990
		public const int PageSize = 6;
	}
}
