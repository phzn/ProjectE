using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory.Wearable;
using Features.Localization;
using Features.MainMenu;
using UnityEngine;

namespace Features.Rank
{
	// Token: 0x02000DB8 RID: 3512
	public static class RankWardrobePolicy
	{
		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x06006793 RID: 26515 RVA: 0x0029674D File Offset: 0x0029494D
		public static bool IsRestricted
		{
			get
			{
				return !RankProgressionService.IsAdminRankActive && !CreatorContentUnlocks.IsActive && RankProgressionService.CurrentRank != null && RankProgressionService.CurrentRank.startOutfitReplacesCustom;
			}
		}

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x06006794 RID: 26516 RVA: 0x00296776 File Offset: 0x00294976
		public static bool UsesCertifiedHomePreset
		{
			get
			{
				return RankWardrobePolicy.IsRestricted && string.Equals(RankProgressionService.CurrentRank.rankId, "hornyslut_declared", StringComparison.Ordinal);
			}
		}

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x06006795 RID: 26517 RVA: 0x00296796 File Offset: 0x00294996
		public static bool IsFirstRunOutfitRequired
		{
			get
			{
				return RankWardrobePolicy.IsRestricted && RankFirstOutfitProgression.IntroducesNewOutfit(RankProgressionService.Config, RankProgressionService.RankIndex) && !RankProgressionService.HasCompletedOutfitRun(RankProgressionService.CurrentRank.rankId);
			}
		}

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x06006796 RID: 26518 RVA: 0x002967C4 File Offset: 0x002949C4
		public static string FirstRunOutfitMessage
		{
			get
			{
				RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
				List<WearableItemSO> list = ((currentRank != null) ? currentRank.startOutfitItems : null);
				string text = ((list != null && list.Count == 1 && list[0] != null && !string.IsNullOrWhiteSpace(list[0].itemName)) ? list[0].itemName.ToLowerInvariant() : "outfit");
				return "Ciel can't wait to take this " + text + " outside. Finish one run with it before changing outfits.";
			}
		}

		// Token: 0x06006797 RID: 26519 RVA: 0x0029683C File Offset: 0x00294A3C
		public static Func<string> CreateFirstRunOutfitMessage()
		{
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			List<WearableItemSO> list = ((currentRank != null) ? currentRank.startOutfitItems : null);
			string garment = ((list != null && list.Count == 1 && list[0] != null && !string.IsNullOrWhiteSpace(list[0].itemName)) ? list[0].itemName : null);
			string english = RankWardrobePolicy.FirstRunOutfitMessage;
			return delegate
			{
				if (!(GameLocalization.CurrentCode == "en"))
				{
					return GameLocalization.FormatSource("rank_ui", "Ciel can't wait to take this {0} outside. Finish one run with it before changing outfits.", new object[] { (garment == null) ? GameLocalization.GetSource("rank_ui", "outfit") : GameLocalization.GetSource("items", garment) });
				}
				return english;
			};
		}

		// Token: 0x06006798 RID: 26520 RVA: 0x002968B9 File Offset: 0x00294AB9
		public static bool IsWardrobePart(string partId)
		{
			return !string.IsNullOrEmpty(partId) && partId.StartsWith("Outfit.", StringComparison.OrdinalIgnoreCase) && !string.Equals(partId, "Outfit.PubicHair", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06006799 RID: 26521 RVA: 0x002968E4 File Offset: 0x00294AE4
		public static List<WearableItemSO> GetUnlockedItems()
		{
			List<WearableItemSO> list = new List<WearableItemSO>();
			RankProgressionConfigSO config = RankProgressionService.Config;
			if (config == null || config.ranksInOrder == null)
			{
				return list;
			}
			int num = Math.Min(RankProgressionService.RankIndex, config.ranksInOrder.Count - 1);
			for (int i = 0; i <= num; i++)
			{
				RankDefinitionSO rankDefinitionSO = config.ranksInOrder[i];
				if (!(rankDefinitionSO == null) && rankDefinitionSO.startOutfitItems != null)
				{
					foreach (WearableItemSO wearableItemSO in rankDefinitionSO.startOutfitItems)
					{
						if (wearableItemSO != null && !list.Contains(wearableItemSO))
						{
							list.Add(wearableItemSO);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600679A RID: 26522 RVA: 0x002969B4 File Offset: 0x00294BB4
		public static List<WearableItemSO> GetAvailableItems()
		{
			List<WearableItemSO> unlockedItems = RankWardrobePolicy.GetUnlockedItems();
			foreach (WearableItemSO wearableItemSO in Resources.LoadAll<WearableItemSO>("Wardrobe"))
			{
				if (wearableItemSO != null && !wearableItemSO.isVaulted && wearableItemSO.mcStylePreset != null && !wearableItemSO.mcStylePreset.isVaulted && !wearableItemSO.mcStylePreset.gameplayOnly && HalloweenWardrobePolicy.IsItemAvailable(wearableItemSO) && (HalloweenWardrobePolicy.IsGranted(wearableItemSO) || CollectionUnlockRegistry.IsItemUnlocked(wearableItemSO)) && !unlockedItems.Contains(wearableItemSO))
				{
					unlockedItems.Add(wearableItemSO);
				}
			}
			return unlockedItems;
		}

		// Token: 0x0600679B RID: 26523 RVA: 0x00296A46 File Offset: 0x00294C46
		public static bool IsUnlocked(WearableItemSO item)
		{
			return item != null && HalloweenWardrobePolicy.IsItemAvailable(item) && (!RankWardrobePolicy.IsRestricted || HalloweenWardrobePolicy.IsGranted(item) || CollectionUnlockRegistry.IsItemUnlocked(item));
		}

		// Token: 0x0600679C RID: 26524 RVA: 0x00296A74 File Offset: 0x00294C74
		public static bool IsPresetUnlocked(MCStylePresetSO preset)
		{
			if (preset == null || !HalloweenWardrobePolicy.IsPresetAvailable(preset))
			{
				return false;
			}
			if (HalloweenWardrobePolicy.IsGranted(preset))
			{
				return true;
			}
			if (!RankWardrobePolicy.IsRestricted)
			{
				return true;
			}
			using (List<WearableItemSO>.Enumerator enumerator = RankWardrobePolicy.GetUnlockedItems().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.mcStylePreset == preset)
					{
						return true;
					}
				}
			}
			WearableItemSO mappedWearable = CustomizationWardrobeDiscoveryPolicy.GetMappedWearable(preset);
			return mappedWearable != null && CollectionUnlockRegistry.IsItemUnlocked(mappedWearable);
		}

		// Token: 0x0600679D RID: 26525 RVA: 0x00296B10 File Offset: 0x00294D10
		public static bool IsAllowed(WearableItemSO item)
		{
			return RankWardrobePolicy.IsUnlocked(item) && (!RankWardrobePolicy.IsFirstRunOutfitRequired || RankProgressionService.CurrentRank.startOutfitItems.Contains(item));
		}

		// Token: 0x0600679E RID: 26526 RVA: 0x00296B38 File Offset: 0x00294D38
		public static bool IsAllowedPreset(MCStylePresetSO preset)
		{
			if (!RankWardrobePolicy.IsPresetUnlocked(preset))
			{
				return false;
			}
			if (!RankWardrobePolicy.IsFirstRunOutfitRequired)
			{
				return true;
			}
			foreach (WearableItemSO wearableItemSO in RankProgressionService.CurrentRank.startOutfitItems)
			{
				if (wearableItemSO != null && wearableItemSO.mcStylePreset == preset)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600679F RID: 26527 RVA: 0x00296BBC File Offset: 0x00294DBC
		public static List<WearableItemSO> ResolveStartingOutfit(IEnumerable<WearableItemSO> selected, bool atHome = false)
		{
			IEnumerable<WearableItemSO> enumerable;
			if (!RankWardrobePolicy.IsRestricted || (atHome && RankWardrobePolicy.UsesCertifiedHomePreset) || (RankProgressionService.HasChosenOutfitForCurrentRank && (!string.Equals(RankProgressionService.CurrentRank.rankId, "hornyslut_declared", StringComparison.Ordinal) || atHome)))
			{
				enumerable = selected;
			}
			else
			{
				IEnumerable<WearableItemSO> startOutfitItems = RankProgressionService.CurrentRank.startOutfitItems;
				enumerable = startOutfitItems;
			}
			return RankWardrobePolicy.ResolveOutfit(enumerable);
		}

		// Token: 0x060067A0 RID: 26528 RVA: 0x00296C10 File Offset: 0x00294E10
		public static List<WearableItemSO> ResolveOutfit(IEnumerable<WearableItemSO> selected)
		{
			List<WearableItemSO> list = new List<WearableItemSO>();
			if (RankWardrobePolicy.IsFirstRunOutfitRequired)
			{
				foreach (WearableItemSO wearableItemSO in RankProgressionService.CurrentRank.startOutfitItems)
				{
					if (wearableItemSO != null && !list.Contains(wearableItemSO))
					{
						list.Add(wearableItemSO);
					}
				}
				return list;
			}
			if (selected != null)
			{
				foreach (WearableItemSO wearableItemSO2 in selected)
				{
					if (RankWardrobePolicy.IsAllowed(wearableItemSO2))
					{
						RankWardrobePolicy.AddReplacingConflicts(list, wearableItemSO2);
					}
				}
			}
			if (!RankWardrobePolicy.IsRestricted)
			{
				return list;
			}
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			int num = RankWardrobePolicy.Coverage(currentRank.startOutfitItems);
			if (list.Count == 0)
			{
				if (currentRank.startOutfitItems != null)
				{
					foreach (WearableItemSO wearableItemSO3 in currentRank.startOutfitItems)
					{
						if (wearableItemSO3 != null)
						{
							RankWardrobePolicy.AddReplacingConflicts(list, wearableItemSO3);
						}
					}
				}
				return list;
			}
			int num2 = num & ~RankWardrobePolicy.Coverage(list);
			if (num2 == 0)
			{
				return list;
			}
			List<WearableItemSO> list2 = new List<WearableItemSO>();
			if (currentRank.startOutfitItems != null)
			{
				list2.AddRange(currentRank.startOutfitItems);
			}
			List<WearableItemSO> unlockedItems = RankWardrobePolicy.GetUnlockedItems();
			unlockedItems.Reverse();
			list2.AddRange(unlockedItems);
			int num3 = 0;
			while (num3 < 2 && num2 != 0)
			{
				foreach (WearableItemSO wearableItemSO4 in list2)
				{
					int num4 = RankWardrobePolicy.Coverage(wearableItemSO4);
					if (!(wearableItemSO4 == null) && (num4 & num2) != 0 && (num3 != 0 || (num4 & ~(num2 != 0)) == 0))
					{
						RankWardrobePolicy.AddReplacingConflicts(list, wearableItemSO4);
						num2 = num & ~RankWardrobePolicy.Coverage(list);
						if (num2 == 0)
						{
							break;
						}
					}
				}
				num3++;
			}
			return list;
		}

		// Token: 0x060067A1 RID: 26529 RVA: 0x00296E20 File Offset: 0x00295020
		private static void AddReplacingConflicts(List<WearableItemSO> items, WearableItemSO added)
		{
			items.RemoveAll((WearableItemSO item) => item == added || string.Equals(item.mcStylePartId, added.mcStylePartId, StringComparison.OrdinalIgnoreCase) || (RankWardrobePolicy.Coverage(item) & RankWardrobePolicy.Coverage(added)) != 0);
			items.Add(added);
		}

		// Token: 0x060067A2 RID: 26530 RVA: 0x00296E59 File Offset: 0x00295059
		private static int Coverage(WearableItemSO item)
		{
			if (!(item == null))
			{
				return (item.physicallyCoversTop ? 1 : 0) | (item.physicallyCoversBottom ? 2 : 0);
			}
			return 0;
		}

		// Token: 0x060067A3 RID: 26531 RVA: 0x00296E80 File Offset: 0x00295080
		private static int Coverage(IEnumerable<WearableItemSO> items)
		{
			int num = 0;
			if (items != null)
			{
				foreach (WearableItemSO wearableItemSO in items)
				{
					num |= RankWardrobePolicy.Coverage(wearableItemSO);
				}
			}
			return num;
		}

		// Token: 0x04006A1F RID: 27167
		public const string FirstRunOutfitTemplate = "Ciel can't wait to take this {0} outside. Finish one run with it before changing outfits.";
	}
}
