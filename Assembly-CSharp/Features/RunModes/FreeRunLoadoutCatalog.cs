using System;
using System.Collections.Generic;
using Features.Inventory;
using Features.MainMenu;
using Features.WantedPoster;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CAA RID: 3242
	public static class FreeRunLoadoutCatalog
	{
		// Token: 0x06005E34 RID: 24116 RVA: 0x002541B0 File Offset: 0x002523B0
		public static IReadOnlyList<InventoryItemSO> GetUnlockedChoices()
		{
			FreeRunLoadoutCatalog.EnsureLoaded();
			List<InventoryItemSO> list = new List<InventoryItemSO>();
			foreach (InventoryItemSO inventoryItemSO in FreeRunLoadoutCatalog.ById.Values)
			{
				if (FreeRunLoadoutCatalog.IsEligible(inventoryItemSO))
				{
					list.Add(inventoryItemSO);
				}
			}
			list.Sort((InventoryItemSO a, InventoryItemSO b) => string.Compare(string.IsNullOrWhiteSpace(a.itemName) ? a.name : a.itemName, string.IsNullOrWhiteSpace(b.itemName) ? b.name : b.itemName, StringComparison.CurrentCultureIgnoreCase));
			return list;
		}

		// Token: 0x06005E35 RID: 24117 RVA: 0x00254240 File Offset: 0x00252440
		public static bool IsEligible(InventoryItemSO item)
		{
			return item != null && !item.isVaulted && item.category == InventoryCategory.Item && !(item is WantedPosterItemSO) && !string.IsNullOrWhiteSpace(item.itemId) && CollectionUnlockRegistry.IsItemUnlocked(item);
		}

		// Token: 0x06005E36 RID: 24118 RVA: 0x00254278 File Offset: 0x00252478
		public static InventoryItemSO ResolveEligible(string itemId)
		{
			if (string.IsNullOrWhiteSpace(itemId))
			{
				return null;
			}
			FreeRunLoadoutCatalog.EnsureLoaded();
			InventoryItemSO inventoryItemSO;
			if (!FreeRunLoadoutCatalog.ById.TryGetValue(itemId, out inventoryItemSO) || !FreeRunLoadoutCatalog.IsEligible(inventoryItemSO))
			{
				return null;
			}
			return inventoryItemSO;
		}

		// Token: 0x06005E37 RID: 24119 RVA: 0x002542B0 File Offset: 0x002524B0
		private static void EnsureLoaded()
		{
			if (FreeRunLoadoutCatalog._catalog != null)
			{
				return;
			}
			FreeRunLoadoutCatalog._catalog = Resources.Load<FreeRunItemCatalogSO>("FreeRunItemCatalog");
			FreeRunLoadoutCatalog.ById.Clear();
			if (FreeRunLoadoutCatalog._catalog == null)
			{
				return;
			}
			foreach (InventoryItemSO inventoryItemSO in FreeRunLoadoutCatalog._catalog.items)
			{
				if (inventoryItemSO != null && !string.IsNullOrWhiteSpace(inventoryItemSO.itemId) && !FreeRunLoadoutCatalog.ById.ContainsKey(inventoryItemSO.itemId))
				{
					FreeRunLoadoutCatalog.ById.Add(inventoryItemSO.itemId, inventoryItemSO);
				}
			}
		}

		// Token: 0x04005F43 RID: 24387
		private static FreeRunItemCatalogSO _catalog;

		// Token: 0x04005F44 RID: 24388
		private static readonly Dictionary<string, InventoryItemSO> ById = new Dictionary<string, InventoryItemSO>(StringComparer.Ordinal);
	}
}
