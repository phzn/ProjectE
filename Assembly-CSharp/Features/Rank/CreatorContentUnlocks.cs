using System;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory;
using Features.Inventory.Wearable;

namespace Features.Rank
{
	// Token: 0x02000D9A RID: 3482
	public static class CreatorContentUnlocks
	{
		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x0600668E RID: 26254 RVA: 0x0028FEA1 File Offset: 0x0028E0A1
		public static bool IsActive
		{
			get
			{
				return RankProgressionService.HasCreatorBoost;
			}
		}

		// Token: 0x0600668F RID: 26255 RVA: 0x0028FEA8 File Offset: 0x0028E0A8
		public static bool IsItemAvailable(InventoryItemSO item)
		{
			if (CreatorContentUnlocks.IsActive && item != null && !item.isVaulted)
			{
				WearableItemSO wearableItemSO = item as WearableItemSO;
				return wearableItemSO == null || wearableItemSO.mcStylePreset == null || !wearableItemSO.mcStylePreset.isVaulted;
			}
			return false;
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x0028FEF8 File Offset: 0x0028E0F8
		public static bool IsVariantAvailable(InventoryItemSO item, int variantIndex)
		{
			if (!CreatorContentUnlocks.IsItemAvailable(item) || variantIndex < -1)
			{
				return false;
			}
			if (variantIndex == -1)
			{
				return true;
			}
			WearableItemSO wearableItemSO = item as WearableItemSO;
			if (wearableItemSO != null && wearableItemSO.mcStylePreset != null)
			{
				MCSurfaceVariantSO mcsurfaceVariantSO;
				return wearableItemSO.mcStylePreset.TryGetSurfaceVariant(variantIndex, out mcsurfaceVariantSO) && mcsurfaceVariantSO != null;
			}
			if (item.rarityVariants != null)
			{
				foreach (RarityVariantEntry rarityVariantEntry in item.rarityVariants)
				{
					if (rarityVariantEntry != null && rarityVariantEntry.surfaceIndex == variantIndex)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}
	}
}
