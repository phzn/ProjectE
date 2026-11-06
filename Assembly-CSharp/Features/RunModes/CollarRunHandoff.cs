using System;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CA4 RID: 3236
	[Serializable]
	public sealed class CollarRunHandoff
	{
		// Token: 0x06005DED RID: 24045 RVA: 0x002521F0 File Offset: 0x002503F0
		public static bool IsRestoring(InventoryItemSO item)
		{
			return item != null && item == CollarRunHandoff._restoringItem && CollarAvailabilityPolicy.IsCollar(item) && CollarAvailabilityPolicy.IsRankUnlocked;
		}

		// Token: 0x06005DEE RID: 24046 RVA: 0x00252217 File Offset: 0x00250417
		public static CollarRunHandoff CaptureForPlayer(PlayerInventoryManager inventory)
		{
			return CollarRunHandoff.Capture(inventory, CollarRunHandoff.ResolveWearController(inventory));
		}

		// Token: 0x06005DEF RID: 24047 RVA: 0x00252228 File Offset: 0x00250428
		public static WearableUseController ResolveWearController(PlayerInventoryManager inventory)
		{
			if (inventory == null || WardrobeGameplayContext.IsPresentationOnly(inventory))
			{
				return null;
			}
			MCStyleCustomizationService componentInParent = inventory.GetComponentInParent<MCStyleCustomizationService>(true);
			foreach (WearableUseController wearableUseController in (componentInParent != null) ? componentInParent.GetComponentsInChildren<WearableUseController>(true) : inventory.GetComponentsInChildren<WearableUseController>(true))
			{
				if (wearableUseController != null && wearableUseController.Inventory == inventory && !WardrobeGameplayContext.IsPresentationOnly(wearableUseController))
				{
					return wearableUseController;
				}
			}
			return null;
		}

		// Token: 0x06005DF0 RID: 24048 RVA: 0x002522A0 File Offset: 0x002504A0
		public static CollarRunHandoff Capture(PlayerInventoryManager inventory, WearableUseController wear)
		{
			if (!CollarAvailabilityPolicy.IsRankUnlocked || inventory == null || wear == null || wear.Inventory != inventory || WardrobeGameplayContext.IsPresentationOnly(wear))
			{
				return null;
			}
			for (int i = 0; i < inventory.MaxClothSlots; i++)
			{
				WearableItemSO wearableItemSO = inventory.GetClothAt(i) as WearableItemSO;
				if (CollarAvailabilityPolicy.IsCollar(wearableItemSO) && wear.IsClothSlotWorn(i) && !(wear.GetWorn(wearableItemSO.itemId, true) != wearableItemSO))
				{
					string partId = CollarAvailabilityPolicy.GetPartId(wearableItemSO.itemId);
					MCStyleCustomizationService styleService = wear.StyleService;
					if (!(styleService == null) && styleService.IsSlotEquipped(partId))
					{
						MCStylePresetSO activePreset = styleService.GetActivePreset(partId);
						if (!(((activePreset != null) ? activePreset.presetId : null) != CollarAvailabilityPolicy.GetPresetId(wearableItemSO.itemId)))
						{
							GameObject spawnedRoot = styleService.GetSpawnedRoot(partId);
							if (!(spawnedRoot == null) && spawnedRoot.activeSelf)
							{
								return new CollarRunHandoff
								{
									itemId = wearableItemSO.itemId,
									rarity = inventory.GetClothRarityAt(i),
									variantIndex = inventory.GetClothVariantAt(i),
									durability01 = Mathf.Clamp01(inventory.GetClothDurability(i) / Mathf.Max(0.0001f, inventory.GetClothDurabilityBase(i)))
								};
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06005DF1 RID: 24049 RVA: 0x002523F0 File Offset: 0x002505F0
		public bool Restore(PlayerInventoryManager inventory, WearableUseController wear)
		{
			if (!CollarAvailabilityPolicy.IsRankUnlocked || inventory == null || wear == null || wear.Inventory != inventory || !wear.gameObject.activeInHierarchy || WardrobeGameplayContext.IsPresentationOnly(wear))
			{
				return false;
			}
			string text = (string.IsNullOrEmpty(this.itemId) ? "cloth_collar" : this.itemId);
			WearableItemSO wearableItemSO = CollarAvailabilityPolicy.LoadItem(text);
			string partId = CollarAvailabilityPolicy.GetPartId(text);
			if (!CollarAvailabilityPolicy.IsCollar(wearableItemSO) || wearableItemSO.isVaulted || wearableItemSO.mcStylePreset == null || wear.StyleService == null || wear.StyleService.GetDefinition(partId) == null)
			{
				return false;
			}
			WearableItemSO restoringItem = CollarRunHandoff._restoringItem;
			CollarRunHandoff._restoringItem = wearableItemSO;
			int num;
			try
			{
				num = inventory.AddClothingReturnIndex(wearableItemSO, false, this.rarity, this.variantIndex, this.durability01, false);
			}
			finally
			{
				CollarRunHandoff._restoringItem = restoringItem;
			}
			if (num < 0)
			{
				return false;
			}
			if (wear.IsWorn(wearableItemSO))
			{
				wear.Unwear(wearableItemSO);
			}
			inventory.SetClothInstanceLook(num, this.rarity, this.variantIndex);
			inventory.SetClothDurability(num, inventory.GetClothDurabilityBase(num) * Mathf.Clamp01(this.durability01));
			wear.WearClothFromSlot(wearableItemSO, num);
			GameObject spawnedRoot = wear.StyleService.GetSpawnedRoot(partId);
			if (wear.IsClothSlotWorn(num))
			{
				MCStylePresetSO activePreset = wear.StyleService.GetActivePreset(partId);
				if (((activePreset != null) ? activePreset.presetId : null) == CollarAvailabilityPolicy.GetPresetId(text) && spawnedRoot != null)
				{
					return spawnedRoot.activeSelf;
				}
			}
			return false;
		}

		// Token: 0x06005DF2 RID: 24050 RVA: 0x00252584 File Offset: 0x00250784
		public static void ClearLoadedCollar(PlayerInventoryManager inventory, WearableUseController wear)
		{
			if (inventory == null || wear == null)
			{
				return;
			}
			for (int i = 0; i < inventory.MaxClothSlots; i++)
			{
				WearableItemSO wearableItemSO = inventory.GetClothAt(i) as WearableItemSO;
				if (CollarAvailabilityPolicy.IsCollar(wearableItemSO))
				{
					if (wear.IsWorn(wearableItemSO))
					{
						wear.Unwear(wearableItemSO);
					}
					inventory.RemoveCloth(i);
				}
			}
			MCStyleCustomizationService styleService = wear.StyleService;
			if (styleService != null)
			{
				foreach (string text in CollarAvailabilityPolicy.ItemIds)
				{
					string partId = CollarAvailabilityPolicy.GetPartId(text);
					if (styleService.GetDefinition(partId) != null)
					{
						styleService.SetSlotEquipped(partId, false);
					}
				}
			}
		}

		// Token: 0x04005EF7 RID: 24311
		public string itemId;

		// Token: 0x04005EF8 RID: 24312
		public ItemRarity rarity;

		// Token: 0x04005EF9 RID: 24313
		public int variantIndex = -1;

		// Token: 0x04005EFA RID: 24314
		public float durability01 = 1f;

		// Token: 0x04005EFB RID: 24315
		private static WearableItemSO _restoringItem;
	}
}
