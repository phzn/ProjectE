using System;
using System.Collections.Generic;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.RunModes;
using UnityEngine;

namespace Features.ClothingLoot
{
	// Token: 0x0200134D RID: 4941
	[CreateAssetMenu(fileName = "ClothingLootPool_New", menuName = "Features/Clothing Loot/Clothing Pool")]
	public sealed class ClothingLootPoolSO : ScriptableObject
	{
		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x060098B0 RID: 39088 RVA: 0x003E9C24 File Offset: 0x003E7E24
		public bool HasValidEntry
		{
			get
			{
				if (this.entries == null)
				{
					return false;
				}
				for (int i = 0; i < this.entries.Count; i++)
				{
					ClothingLootPoolEntry clothingLootPoolEntry = this.entries[i];
					if (clothingLootPoolEntry != null && clothingLootPoolEntry.weight > 0f && ClothingLootPoolSO.IsEligibleClothing(clothingLootPoolEntry.clothing))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060098B1 RID: 39089 RVA: 0x003E9C7E File Offset: 0x003E7E7E
		public static ClothingLootPoolSO LoadDefaultMallPool()
		{
			return Resources.Load<ClothingLootPoolSO>("ClothingLootPool_Mall");
		}

		// Token: 0x060098B2 RID: 39090 RVA: 0x003E9C8C File Offset: 0x003E7E8C
		public List<ClothingLootOffer> RollOffers(int minChoices, int maxChoices, PlayerInventoryManager inventory, bool preferUnowned)
		{
			List<ClothingLootPoolEntry> list = this.BuildCandidates(null, false);
			List<ClothingLootOffer> list2 = new List<ClothingLootOffer>(Mathf.Min(4, list.Count));
			if (list.Count == 0)
			{
				return list2;
			}
			int num = Mathf.Clamp(minChoices, 1, 4);
			int num2 = Mathf.Min(Mathf.Clamp(maxChoices, num, 4), list.Count);
			int num3 = Random.Range(Mathf.Min(num, num2), num2 + 1);
			if (preferUnowned && inventory != null)
			{
				ClothingLootPoolSO.RollInto(this.BuildCandidates(inventory, true), list2, num3);
				if (list2.Count < num3)
				{
					HashSet<WearableItemSO> hashSet = new HashSet<WearableItemSO>();
					for (int i = 0; i < list2.Count; i++)
					{
						hashSet.Add(list2[i].clothing);
					}
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (hashSet.Contains(list[j].clothing))
						{
							list.RemoveAt(j);
						}
					}
				}
			}
			ClothingLootPoolSO.RollInto(list, list2, num3);
			return list2;
		}

		// Token: 0x060098B3 RID: 39091 RVA: 0x003E9D89 File Offset: 0x003E7F89
		public static bool IsEligibleClothing(WearableItemSO item)
		{
			return !LongWayHomeLootPolicy.RestrictsWorldLoot && item != null && item.category == InventoryCategory.Clothing && item.showInGameplayInventory;
		}

		// Token: 0x060098B4 RID: 39092 RVA: 0x003E9DAC File Offset: 0x003E7FAC
		private List<ClothingLootPoolEntry> BuildCandidates(PlayerInventoryManager inventory, bool excludeOwned)
		{
			List<ClothingLootPoolEntry> list = new List<ClothingLootPoolEntry>();
			if (this.entries == null)
			{
				return list;
			}
			HashSet<WearableItemSO> hashSet = null;
			if (excludeOwned && inventory != null)
			{
				hashSet = new HashSet<WearableItemSO>();
				for (int i = 0; i < inventory.MaxClothSlots; i++)
				{
					WearableItemSO wearableItemSO = inventory.GetClothAt(i) as WearableItemSO;
					if (wearableItemSO != null)
					{
						hashSet.Add(wearableItemSO);
					}
				}
			}
			HashSet<WearableItemSO> hashSet2 = new HashSet<WearableItemSO>();
			for (int j = 0; j < this.entries.Count; j++)
			{
				ClothingLootPoolEntry clothingLootPoolEntry = this.entries[j];
				if (clothingLootPoolEntry != null && clothingLootPoolEntry.weight > 0f && ClothingLootPoolSO.IsEligibleClothing(clothingLootPoolEntry.clothing) && hashSet2.Add(clothingLootPoolEntry.clothing) && (hashSet == null || !hashSet.Contains(clothingLootPoolEntry.clothing)))
				{
					list.Add(clothingLootPoolEntry);
				}
			}
			return list;
		}

		// Token: 0x060098B5 RID: 39093 RVA: 0x003E9E88 File Offset: 0x003E8088
		private static int PickWeightedIndex(List<ClothingLootPoolEntry> candidates)
		{
			float num = 0f;
			for (int i = 0; i < candidates.Count; i++)
			{
				num += Mathf.Max(0f, candidates[i].weight);
			}
			if (num <= 0f)
			{
				return -1;
			}
			float num2 = Random.value * num;
			int num3 = -1;
			for (int j = 0; j < candidates.Count; j++)
			{
				float num4 = Mathf.Max(0f, candidates[j].weight);
				if (num4 > 0f)
				{
					num3 = j;
					num2 -= num4;
					if (num2 <= 0f)
					{
						return j;
					}
				}
			}
			return num3;
		}

		// Token: 0x060098B6 RID: 39094 RVA: 0x003E9F24 File Offset: 0x003E8124
		private static void RollInto(List<ClothingLootPoolEntry> candidates, List<ClothingLootOffer> offers, int targetCount)
		{
			while (offers.Count < targetCount && candidates.Count > 0)
			{
				int num = ClothingLootPoolSO.PickWeightedIndex(candidates);
				if (num < 0)
				{
					return;
				}
				WearableItemSO clothing = candidates[num].clothing;
				candidates.RemoveAt(num);
				RaritySettingsSO instance = RaritySettingsSO.Instance;
				ItemRarity itemRarity = ((instance != null) ? instance.RollInstanceRarity(clothing, ItemRarity.Common) : clothing.rarity);
				int num2 = clothing.PickVariantForTier(itemRarity);
				offers.Add(new ClothingLootOffer(clothing, itemRarity, num2));
			}
		}

		// Token: 0x04009DCB RID: 40395
		public const string DefaultMallResourceName = "ClothingLootPool_Mall";

		// Token: 0x04009DCC RID: 40396
		[Tooltip("Only entries with category Clothing, gameplay visibility and a positive weight can roll.")]
		public List<ClothingLootPoolEntry> entries = new List<ClothingLootPoolEntry>();
	}
}
