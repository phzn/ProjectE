using System;
using System.Collections.Generic;
using Features.Inventory;
using Features.Inventory.Placement;
using Features.Inventory.Wearable;
using Features.TrashcanLoot;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CD6 RID: 3286
	public static class LongWayHomeLootPolicy
	{
		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x06005F86 RID: 24454 RVA: 0x0025DA98 File Offset: 0x0025BC98
		public static IReadOnlyList<LootTableSO.LootEntry> SupplementalTrashLoot
		{
			get
			{
				return LongWayHomeLootPolicy.PaintLoot;
			}
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x06005F87 RID: 24455 RVA: 0x0025DA9F File Offset: 0x0025BC9F
		private static bool IsRestricted
		{
			get
			{
				return RunModeService.IsLongWayHome && !RunModeService.UsesHomePresetOutfit;
			}
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06005F88 RID: 24456 RVA: 0x0025DAB2 File Offset: 0x0025BCB2
		public static bool RestrictsWorldLoot
		{
			get
			{
				return LongWayHomeLootPolicy.IsRestricted;
			}
		}

		// Token: 0x06005F89 RID: 24457 RVA: 0x0025DAB9 File Offset: 0x0025BCB9
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		public static void Clear()
		{
			LongWayHomeLootPolicy._generation++;
			LongWayHomeLootPolicy.StarterGarments.Clear();
			LongWayHomeLootPolicy.StarterPickups.Clear();
			LongWayHomeLootPolicy.PaintLoot.Clear();
			LongWayHomeLootPolicy._authorizedPickup = null;
		}

		// Token: 0x06005F8A RID: 24458 RVA: 0x0025DAEB File Offset: 0x0025BCEB
		public static int BeginRun()
		{
			LongWayHomeLootPolicy.Clear();
			return LongWayHomeLootPolicy._generation;
		}

		// Token: 0x06005F8B RID: 24459 RVA: 0x0025DAF7 File Offset: 0x0025BCF7
		public static void EndRun(int generation)
		{
			if (generation == LongWayHomeLootPolicy._generation)
			{
				LongWayHomeLootPolicy.Clear();
			}
		}

		// Token: 0x06005F8C RID: 24460 RVA: 0x0025DB06 File Offset: 0x0025BD06
		public static bool IsBodyPaintItem(InventoryItemSO item)
		{
			return item != null && (string.Equals(item.itemId, "pencil", StringComparison.Ordinal) || string.Equals(item.itemId, "custom_bodypaint_pen", StringComparison.Ordinal));
		}

		// Token: 0x06005F8D RID: 24461 RVA: 0x0025DB3C File Offset: 0x0025BD3C
		public static void CacheBodyPaintLoot(IEnumerable<InventoryItemSO> loadedItems)
		{
			LongWayHomeLootPolicy.PaintLoot.Clear();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			foreach (InventoryItemSO inventoryItemSO in loadedItems)
			{
				if (LongWayHomeLootPolicy.IsBodyPaintItem(inventoryItemSO) && LongWayHomeLootPolicy.IsAllowedTrashLoot(inventoryItemSO) && inventoryItemSO.floorPrefab != null && hashSet.Add(inventoryItemSO.itemId))
				{
					LongWayHomeLootPolicy.PaintLoot.Add(new LootTableSO.LootEntry
					{
						item = inventoryItemSO,
						weight = ((inventoryItemSO.itemId == "pencil") ? 3f : 1f)
					});
				}
			}
		}

		// Token: 0x06005F8E RID: 24462 RVA: 0x0025DBF8 File Offset: 0x0025BDF8
		public static bool IsAllowedTrashLoot(InventoryItemSO item)
		{
			return item != null && !item.isVaulted && item.category == InventoryCategory.Item;
		}

		// Token: 0x06005F8F RID: 24463 RVA: 0x0025DC18 File Offset: 0x0025BE18
		public static LongWayHomeLootPolicy.IntimateCoverage GetCoverage(InventoryItemSO item)
		{
			WearableItemSO wearableItemSO = item as WearableItemSO;
			if (wearableItemSO == null)
			{
				return LongWayHomeLootPolicy.IntimateCoverage.None;
			}
			LongWayHomeLootPolicy.IntimateCoverage intimateCoverage = LongWayHomeLootPolicy.IntimateCoverage.None;
			if (wearableItemSO.physicallyCoversTop || wearableItemSO.coversTopDecently)
			{
				intimateCoverage |= LongWayHomeLootPolicy.IntimateCoverage.Upper;
			}
			if (wearableItemSO.physicallyCoversBottom || wearableItemSO.coversBottomDecently)
			{
				intimateCoverage |= LongWayHomeLootPolicy.IntimateCoverage.Lower;
			}
			return intimateCoverage;
		}

		// Token: 0x06005F90 RID: 24464 RVA: 0x0025DC5C File Offset: 0x0025BE5C
		public static bool IsStarterGarment(InventoryItemSO item)
		{
			WearableItemSO wearableItemSO = item as WearableItemSO;
			return wearableItemSO != null && !CollarAvailabilityPolicy.IsCollar(item) && !item.isVaulted && item.category == InventoryCategory.Clothing && item.floorPrefab != null && (wearableItemSO.showInGameplayInventory || wearableItemSO.HasGameplaySurfaceOverride) && LongWayHomeLootPolicy.GetCoverage(item) != LongWayHomeLootPolicy.IntimateCoverage.Both;
		}

		// Token: 0x06005F91 RID: 24465 RVA: 0x0025DCB8 File Offset: 0x0025BEB8
		public static List<InventoryItemSO> SelectStartingItems(IReadOnlyList<InventoryItemSO> pool, int count, Random random)
		{
			List<InventoryItemSO> list = new List<InventoryItemSO>();
			List<InventoryItemSO> list2 = new List<InventoryItemSO>();
			HashSet<InventoryItemSO> hashSet = new HashSet<InventoryItemSO>();
			foreach (InventoryItemSO inventoryItemSO in pool)
			{
				if (!(inventoryItemSO == null) && !inventoryItemSO.isVaulted && !(inventoryItemSO.floorPrefab == null) && hashSet.Add(inventoryItemSO))
				{
					if (LongWayHomeLootPolicy.IsStarterGarment(inventoryItemSO))
					{
						list.Add(inventoryItemSO);
					}
					else if (LongWayHomeLootPolicy.IsBodyPaintItem(inventoryItemSO))
					{
						list2.Add(inventoryItemSO);
					}
				}
			}
			List<InventoryItemSO> list3 = new List<InventoryItemSO>();
			LongWayHomeLootPolicy.IntimateCoverage intimateCoverage = LongWayHomeLootPolicy.IntimateCoverage.None;
			count = Mathf.Clamp(count, 1, 3);
			while (list3.Count < count)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if ((intimateCoverage | LongWayHomeLootPolicy.GetCoverage(list[i])) == LongWayHomeLootPolicy.IntimateCoverage.Both)
					{
						list.RemoveAt(i);
					}
				}
				List<InventoryItemSO> list4 = ((list3.Count > 0 && list2.Count > 0 && (list.Count == 0 || random.Next(4) == 0)) ? list2 : list);
				if (list4.Count == 0)
				{
					break;
				}
				int num = random.Next(list4.Count);
				InventoryItemSO inventoryItemSO2 = list4[num];
				list4.RemoveAt(num);
				list3.Add(inventoryItemSO2);
				intimateCoverage |= LongWayHomeLootPolicy.GetCoverage(inventoryItemSO2);
			}
			return list3;
		}

		// Token: 0x06005F92 RID: 24466 RVA: 0x0025DE24 File Offset: 0x0025C024
		public static void RegisterStartingItems(IEnumerable<InventoryItemSO> items)
		{
			LongWayHomeLootPolicy.IntimateCoverage intimateCoverage = LongWayHomeLootPolicy.IntimateCoverage.None;
			foreach (InventoryItemSO inventoryItemSO in LongWayHomeLootPolicy.StarterGarments)
			{
				intimateCoverage |= LongWayHomeLootPolicy.GetCoverage(inventoryItemSO);
			}
			foreach (InventoryItemSO inventoryItemSO2 in items)
			{
				if (LongWayHomeLootPolicy.IsStarterGarment(inventoryItemSO2) && (intimateCoverage | LongWayHomeLootPolicy.GetCoverage(inventoryItemSO2)) != LongWayHomeLootPolicy.IntimateCoverage.Both)
				{
					LongWayHomeLootPolicy.StarterGarments.Add(inventoryItemSO2);
					intimateCoverage |= LongWayHomeLootPolicy.GetCoverage(inventoryItemSO2);
				}
			}
		}

		// Token: 0x06005F93 RID: 24467 RVA: 0x0025DED8 File Offset: 0x0025C0D8
		public static void RegisterStarterPickup(InventoryPickup pickup)
		{
			if (pickup != null)
			{
				LongWayHomeLootPolicy.StarterPickups.Add(pickup);
			}
		}

		// Token: 0x06005F94 RID: 24468 RVA: 0x0025DEF0 File Offset: 0x0025C0F0
		public static bool CanSpawnDrop(InventoryItemSO item, bool discovered, bool starter)
		{
			return !LongWayHomeLootPolicy.IsRestricted || item == null || item.category != InventoryCategory.Clothing || (!discovered && LongWayHomeLootPolicy.IsUsableCollar(item) && CollarAvailabilityPolicy.IsOwnedItem(item)) || (LongWayHomeLootPolicy.StarterGarments.Contains(item) && (!discovered || starter));
		}

		// Token: 0x06005F95 RID: 24469 RVA: 0x0025DF40 File Offset: 0x0025C140
		public static bool CanPickUp(InventoryItemSO item, Component source, bool alreadyOwned = false)
		{
			if (LongWayHomeLootPolicy.IsRestricted && !(item == null) && item.category == InventoryCategory.Clothing)
			{
				if (alreadyOwned && LongWayHomeLootPolicy.IsUsableCollar(item))
				{
					InventoryPickup inventoryPickup = source as InventoryPickup;
					if (inventoryPickup != null && inventoryPickup.ItemData == item)
					{
						return true;
					}
				}
				if (!LongWayHomeLootPolicy.StarterGarments.Contains(item))
				{
					return false;
				}
				if (!alreadyOwned)
				{
					InventoryPickup inventoryPickup2 = source as InventoryPickup;
					return inventoryPickup2 != null && LongWayHomeLootPolicy.StarterPickups.Contains(inventoryPickup2);
				}
				return true;
			}
			return true;
		}

		// Token: 0x06005F96 RID: 24470 RVA: 0x0025DFB8 File Offset: 0x0025C1B8
		public static bool CanAddClothing(InventoryItemSO item, bool registerFind)
		{
			return !LongWayHomeLootPolicy.IsRestricted || (!registerFind && CollarRunHandoff.IsRestoring(item)) || (!registerFind && LongWayHomeLootPolicy.IsUsableCollar(item) && LongWayHomeLootPolicy._authorizedPickup == item) || (LongWayHomeLootPolicy.StarterGarments.Contains(item) && (!registerFind || LongWayHomeLootPolicy._authorizedPickup == item));
		}

		// Token: 0x06005F97 RID: 24471 RVA: 0x0025E010 File Offset: 0x0025C210
		public static IDisposable BeginPickupGrant(InventoryPickup source, InventoryItemSO item)
		{
			InventoryItemSO authorizedPickup = LongWayHomeLootPolicy._authorizedPickup;
			if (source != null && (LongWayHomeLootPolicy.StarterPickups.Contains(source) || (source.ItemData == item && LongWayHomeLootPolicy.IsUsableCollar(item))))
			{
				LongWayHomeLootPolicy._authorizedPickup = item;
			}
			return new LongWayHomeLootPolicy.PickupGrant(authorizedPickup);
		}

		// Token: 0x06005F98 RID: 24472 RVA: 0x0025E04E File Offset: 0x0025C24E
		private static bool IsUsableCollar(InventoryItemSO item)
		{
			return CollarAvailabilityPolicy.IsCollar(item) && CollarAvailabilityPolicy.IsRankUnlocked;
		}

		// Token: 0x040060F9 RID: 24825
		private static readonly HashSet<InventoryItemSO> StarterGarments = new HashSet<InventoryItemSO>();

		// Token: 0x040060FA RID: 24826
		private static readonly HashSet<InventoryPickup> StarterPickups = new HashSet<InventoryPickup>();

		// Token: 0x040060FB RID: 24827
		private static readonly List<LootTableSO.LootEntry> PaintLoot = new List<LootTableSO.LootEntry>(2);

		// Token: 0x040060FC RID: 24828
		private static InventoryItemSO _authorizedPickup;

		// Token: 0x040060FD RID: 24829
		private static int _generation;

		// Token: 0x02000CD7 RID: 3287
		[Flags]
		public enum IntimateCoverage
		{
			// Token: 0x040060FF RID: 24831
			None = 0,
			// Token: 0x04006100 RID: 24832
			Upper = 1,
			// Token: 0x04006101 RID: 24833
			Lower = 2,
			// Token: 0x04006102 RID: 24834
			Both = 3
		}

		// Token: 0x02000CD8 RID: 3288
		private sealed class PickupGrant : IDisposable
		{
			// Token: 0x06005F9A RID: 24474 RVA: 0x0025E080 File Offset: 0x0025C280
			public PickupGrant(InventoryItemSO previous)
			{
				this._previous = previous;
			}

			// Token: 0x06005F9B RID: 24475 RVA: 0x0025E08F File Offset: 0x0025C28F
			public void Dispose()
			{
				LongWayHomeLootPolicy._authorizedPickup = this._previous;
			}

			// Token: 0x04006103 RID: 24835
			private readonly InventoryItemSO _previous;
		}
	}
}
