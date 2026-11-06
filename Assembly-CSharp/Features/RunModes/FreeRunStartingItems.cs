using System;
using System.Collections;
using System.Collections.Generic;
using Features.Inventory;
using Features.Rank;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CAC RID: 3244
	public static class FreeRunStartingItems
	{
		// Token: 0x06005E3C RID: 24124 RVA: 0x002543A3 File Offset: 0x002525A3
		public static IEnumerator GrantWhenPlayerReady()
		{
			if (!FreeRunStartingItems.IsFreeGameplay)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < 300; i = num + 1)
			{
				if (!FreeRunStartingItems.IsFreeGameplay)
				{
					yield break;
				}
				RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
				if (rankRuntimeController != null && rankRuntimeController.IsAtHome && !RunModeService.IsAtHome)
				{
					yield return null;
				}
				else
				{
					PlayerInventoryManager instance = PlayerInventoryManager.Instance;
					if (instance != null && instance.isActiveAndEnabled)
					{
						FreeRunStartingItems.GrantOnce(instance);
						yield break;
					}
					yield return null;
				}
				num = i;
			}
			Debug.LogWarning("[FreeRun] Starting item inventory was unavailable at run boot.");
			yield break;
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06005E3D RID: 24125 RVA: 0x002543AB File Offset: 0x002525AB
		private static bool IsFreeGameplay
		{
			get
			{
				return RunModeService.IsGameplayBoot && RunModeService.CurrentMode == RunModeKind.Free;
			}
		}

		// Token: 0x06005E3E RID: 24126 RVA: 0x002543C0 File Offset: 0x002525C0
		public static void GrantOnce(PlayerInventoryManager inventory)
		{
			if (!FreeRunStartingItems.IsFreeGameplay || inventory == null)
			{
				return;
			}
			InventoryItemSO[] array = FreeRunStartingItems.ResolveSelectedSlots(RunModeService.StartingItemIds);
			if (!RunModeService.TryClaimFreeStartingItems())
			{
				return;
			}
			for (int i = 0; i < inventory.MaxItemSlots; i++)
			{
				if (inventory.GetItemAt(i) != null)
				{
					inventory.DropEntireItemStack(i);
				}
			}
			int num = 0;
			while (num < array.Length && num < inventory.MaxItemSlots)
			{
				InventoryItemSO inventoryItemSO = array[num];
				if (!(inventoryItemSO == null) && inventory.AddItem(inventoryItemSO, inventoryItemSO.rarity, -1, 1f, false, false))
				{
					int j = 0;
					while (j < inventory.MaxItemSlots)
					{
						if (inventory.GetItemAt(j) == inventoryItemSO)
						{
							if (j != num)
							{
								inventory.MoveItem(j, num);
								break;
							}
							break;
						}
						else
						{
							j++;
						}
					}
				}
				num++;
			}
		}

		// Token: 0x06005E3F RID: 24127 RVA: 0x00254488 File Offset: 0x00252688
		public static InventoryItemSO[] ResolveSelectedSlots(string[] chosen)
		{
			InventoryItemSO[] array = new InventoryItemSO[3];
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			if (chosen == null)
			{
				return array;
			}
			int num = 0;
			while (num < array.Length && num < chosen.Length)
			{
				string text = chosen[num];
				if (!string.IsNullOrWhiteSpace(text) && hashSet.Add(text))
				{
					InventoryItemSO inventoryItemSO = FreeRunLoadoutCatalog.ResolveEligible(text);
					if (inventoryItemSO != null)
					{
						array[num] = inventoryItemSO;
					}
				}
				num++;
			}
			return array;
		}

		// Token: 0x06005E40 RID: 24128 RVA: 0x002544F0 File Offset: 0x002526F0
		public static List<InventoryItemSO> ResolveSelectedItems(string[] chosen)
		{
			List<InventoryItemSO> list = new List<InventoryItemSO>(3);
			foreach (InventoryItemSO inventoryItemSO in FreeRunStartingItems.ResolveSelectedSlots(chosen))
			{
				if (inventoryItemSO != null)
				{
					list.Add(inventoryItemSO);
				}
			}
			return list;
		}
	}
}
