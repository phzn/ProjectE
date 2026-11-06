using System;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.MainMenu;
using UnityEngine;

namespace Features.TrashcanLoot
{
	// Token: 0x02000BE5 RID: 3045
	public static class TrashcanDiscoveryLootPolicy
	{
		// Token: 0x06005783 RID: 22403 RVA: 0x002227D0 File Offset: 0x002209D0
		public static float ItemWeightMultiplier(InventoryItemSO item, ItemRarity minimumTier)
		{
			if (!TrashcanDiscoveryLootPolicy.CanDiscover(item))
			{
				return 1f;
			}
			RaritySettingsSO instance = RaritySettingsSO.Instance;
			for (int i = (int)item.rarity; i <= (int)item.EffectiveMaxRollableRarity; i++)
			{
				ItemRarity itemRarity = (ItemRarity)i;
				if (((instance != null) ? instance.CanRollInstanceRarity(item, itemRarity, minimumTier) : (i == Mathf.Max((int)item.rarity, (int)minimumTier))) && TrashcanDiscoveryLootPolicy.HasMissingResult(item, itemRarity))
				{
					return 3f;
				}
			}
			return 1f;
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x00222844 File Offset: 0x00220A44
		public static int PickVariant(InventoryItemSO item, ItemRarity tier)
		{
			if (item == null)
			{
				return -1;
			}
			if (!TrashcanDiscoveryLootPolicy.CanDiscover(item) || item.rarityVariants == null || item.rarityVariants.Count == 0)
			{
				return item.PickVariantForTier(tier);
			}
			int num = 0;
			int num2 = 0;
			foreach (RarityVariantEntry rarityVariantEntry in item.rarityVariants)
			{
				if (rarityVariantEntry != null && rarityVariantEntry.tier == tier)
				{
					if (TrashcanDiscoveryLootPolicy.IsDiscovered(item, rarityVariantEntry.surfaceIndex))
					{
						num2++;
					}
					else
					{
						num++;
					}
				}
			}
			if (num == 0 || num2 == 0)
			{
				return item.PickVariantForTier(tier);
			}
			float num3 = Random.value * ((float)num2 + (float)num * 3f);
			foreach (RarityVariantEntry rarityVariantEntry2 in item.rarityVariants)
			{
				if (rarityVariantEntry2 != null && rarityVariantEntry2.tier == tier)
				{
					num3 -= (TrashcanDiscoveryLootPolicy.IsDiscovered(item, rarityVariantEntry2.surfaceIndex) ? 1f : 3f);
					if (num3 <= 0f)
					{
						return rarityVariantEntry2.surfaceIndex;
					}
				}
			}
			return 0;
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x0022298C File Offset: 0x00220B8C
		private static bool CanDiscover(InventoryItemSO item)
		{
			if (item != null && !item.isVaulted && CollarAvailabilityPolicy.IsAvailable(item))
			{
				WearableItemSO wearableItemSO = item as WearableItemSO;
				return wearableItemSO == null || wearableItemSO.mcStylePreset == null || !wearableItemSO.mcStylePreset.isVaulted;
			}
			return false;
		}

		// Token: 0x06005786 RID: 22406 RVA: 0x002229DC File Offset: 0x00220BDC
		private static bool HasMissingResult(InventoryItemSO item, ItemRarity tier)
		{
			if (item.rarityVariants == null || item.rarityVariants.Count == 0)
			{
				return !TrashcanDiscoveryLootPolicy.IsDiscovered(item, -1);
			}
			bool flag = false;
			foreach (RarityVariantEntry rarityVariantEntry in item.rarityVariants)
			{
				if (rarityVariantEntry != null && rarityVariantEntry.tier == tier)
				{
					flag = true;
					if (!TrashcanDiscoveryLootPolicy.IsDiscovered(item, rarityVariantEntry.surfaceIndex))
					{
						return true;
					}
				}
			}
			return !flag && !TrashcanDiscoveryLootPolicy.IsDiscovered(item, 0);
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x00222A7C File Offset: 0x00220C7C
		private static bool IsDiscovered(InventoryItemSO item, int surfaceIndex)
		{
			if (surfaceIndex < -1)
			{
				return true;
			}
			if (surfaceIndex >= 0)
			{
				WearableItemSO wearableItemSO = item as WearableItemSO;
				MCSurfaceVariantSO mcsurfaceVariantSO;
				if (wearableItemSO != null && wearableItemSO.mcStylePreset != null && (!wearableItemSO.mcStylePreset.TryGetSurfaceVariant(surfaceIndex, out mcsurfaceVariantSO) || mcsurfaceVariantSO == null))
				{
					return true;
				}
			}
			if (CollectionUnlockRegistry.IsVariantEarned(item, surfaceIndex))
			{
				return true;
			}
			if (surfaceIndex < 0)
			{
				WearableItemSO wearableItemSO2 = item as WearableItemSO;
				if (wearableItemSO2 != null && wearableItemSO2.mcStylePreset != null)
				{
					return CollectionUnlockRegistry.IsVariantEarned(item, wearableItemSO2.mcStylePreset.ResolveDefaultSurfaceIndex());
				}
			}
			return false;
		}

		// Token: 0x040057E6 RID: 22502
		public const float MissingDiscoveryWeight = 3f;
	}
}
