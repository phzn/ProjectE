using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x02000572 RID: 1394
	public static class vItemListOperations
	{
		// Token: 0x06002125 RID: 8485 RVA: 0x000C1A34 File Offset: 0x000BFC34
		public static List<vItem> GetSameItems(this List<vItem> itemList, int id)
		{
			return itemList.FindAll((vItem i) => i.id.Equals(id));
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000C1A60 File Offset: 0x000BFC60
		public static List<vItem> GetSameItems(this List<vItem> itemList, string name)
		{
			return itemList.FindAll((vItem i) => i.name.Equals(name));
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x000C1A8C File Offset: 0x000BFC8C
		public static List<vItem> GetSameItems(this List<vItem> itemList, params int[] ids)
		{
			return itemList.FindAll((vItem i) => Array.Exists<int>(ids, (int id) => i.id.Equals(id)));
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000C1AB8 File Offset: 0x000BFCB8
		public static List<vItem> GetSameItems(this List<vItem> itemList, params string[] names)
		{
			return itemList.FindAll((vItem i) => Array.Exists<string>(names, (string name) => i.name.Equals(name)));
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000C1AE4 File Offset: 0x000BFCE4
		public static bool HasItem(this List<vItem> itemList, int id)
		{
			return itemList.Exists((vItem i) => i.id.Equals(id));
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000C1B10 File Offset: 0x000BFD10
		public static bool HasItem(this List<vItem> itemList, string name)
		{
			return itemList.Exists((vItem i) => i.name.Equals(name));
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000C1B3C File Offset: 0x000BFD3C
		public static bool HasItems(this List<vItem> itemList, params int[] ids)
		{
			bool flag = true;
			for (int i = 0; i < ids.Length; i++)
			{
				if (!itemList.HasItem(ids[i]))
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x000C1B6C File Offset: 0x000BFD6C
		public static bool HasItems(this List<vItem> itemList, params string[] names)
		{
			bool flag = true;
			for (int i = 0; i < names.Length; i++)
			{
				if (!itemList.HasItem(names[i]))
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x000C1B9C File Offset: 0x000BFD9C
		public static int GetItemCount(this List<vItem> itemList, int id)
		{
			int count = 0;
			itemList.GetSameItems(id).ForEach(delegate(vItem item)
			{
				count += item.amount;
			});
			return count;
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x000C1BD4 File Offset: 0x000BFDD4
		public static int GetItemCount(this List<vItem> itemList, string name)
		{
			int count = 0;
			itemList.GetSameItems(name).ForEach(delegate(vItem item)
			{
				count += item.amount;
			});
			return count;
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x000C1C0C File Offset: 0x000BFE0C
		public static void DestroySameItems(this List<vItem> itemList, int id, int amount, Action<vItem, int> onChangeItemAmount = null)
		{
			List<vItem> sameItems = itemList.GetSameItems(id);
			for (int i = 0; i < sameItems.Count; i++)
			{
				vItem vItem = sameItems[i];
				if (vItem.amount > amount)
				{
					if (onChangeItemAmount != null)
					{
						onChangeItemAmount(vItem, amount);
					}
					vItem.amount -= amount;
					return;
				}
				if (onChangeItemAmount != null)
				{
					onChangeItemAmount(vItem, vItem.amount);
				}
				amount -= vItem.amount;
				vItem.amount = 0;
				itemList.Remove(vItem);
				Object.Destroy(vItem);
			}
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x000C1C8C File Offset: 0x000BFE8C
		public static void DestroySameItems(this List<vItem> itemList, string name, int amount, Action<vItem, int> onChangeItemAmount = null)
		{
			List<vItem> sameItems = itemList.GetSameItems(name);
			for (int i = 0; i < sameItems.Count; i++)
			{
				vItem vItem = sameItems[i];
				if (vItem.amount > amount)
				{
					if (onChangeItemAmount != null)
					{
						onChangeItemAmount(vItem, amount);
					}
					vItem.amount -= amount;
					return;
				}
				if (onChangeItemAmount != null)
				{
					onChangeItemAmount(vItem, vItem.amount);
				}
				amount -= vItem.amount;
				vItem.amount = 0;
				itemList.Remove(vItem);
				Object.Destroy(vItem);
			}
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x000C1D0C File Offset: 0x000BFF0C
		public static void DestroySameItems(this List<vItem> itemList, int id, Action<vItem, int> onChangeItemAmount = null)
		{
			List<vItem> sameItems = itemList.GetSameItems(id);
			itemList.RemoveAll((vItem i) => i.id.Equals(id));
			for (int j = 0; j < sameItems.Count; j++)
			{
				if (onChangeItemAmount != null)
				{
					onChangeItemAmount(sameItems[j], sameItems[j].amount);
				}
				Object.Destroy(sameItems[j]);
			}
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x000C1D80 File Offset: 0x000BFF80
		public static void DestroySameItems(this List<vItem> itemList, string name, Action<vItem, int> onChangeItemAmount = null)
		{
			List<vItem> sameItems = itemList.GetSameItems(name);
			itemList.RemoveAll((vItem i) => i.name.Equals(name));
			for (int j = 0; j < sameItems.Count; j++)
			{
				if (onChangeItemAmount != null)
				{
					onChangeItemAmount(sameItems[j], sameItems[j].amount);
				}
				Object.Destroy(sameItems[j]);
			}
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x000C1DF4 File Offset: 0x000BFFF4
		public static void DestroySameItems(this List<vItem> itemList, Action<vItem, int> onChangeItemAmount = null, params int[] ids)
		{
			List<vItem> sameItems = itemList.GetSameItems(ids);
			itemList.RemoveAll((vItem i) => Array.Exists<int>(ids, (int id) => i.id.Equals(id)));
			for (int j = 0; j < sameItems.Count; j++)
			{
				if (onChangeItemAmount != null)
				{
					onChangeItemAmount(sameItems[j], sameItems[j].amount);
				}
				Object.Destroy(sameItems[j]);
			}
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000C1E68 File Offset: 0x000C0068
		public static void DestroySameItems(this List<vItem> itemList, Action<vItem, int> onChangeItemAmount = null, params string[] names)
		{
			List<vItem> sameItems = itemList.GetSameItems(names);
			itemList.RemoveAll((vItem i) => Array.Exists<string>(names, (string name) => i.name.Equals(name)));
			for (int j = 0; j < sameItems.Count; j++)
			{
				if (onChangeItemAmount != null)
				{
					onChangeItemAmount(sameItems[j], sameItems[j].amount);
				}
				Object.Destroy(sameItems[j]);
			}
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000C1EDC File Offset: 0x000C00DC
		public static bool ItemIsEquipped(this vItemManager itemManager, int id)
		{
			return itemManager.inventory && Array.Find<vEquipArea>(itemManager.inventory.equipAreas, (vEquipArea equipArea) => equipArea.currentEquippedItem && equipArea.currentEquippedItem.id.Equals(id));
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000C1F28 File Offset: 0x000C0128
		public static bool ItemTypeIsEquipped(this vItemManager itemManager, vItemType type)
		{
			return itemManager.inventory && Array.Find<vEquipArea>(itemManager.inventory.equipAreas, (vEquipArea equipArea) => equipArea.currentEquippedItem && equipArea.currentEquippedItem.type.Equals(type));
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000C1F74 File Offset: 0x000C0174
		public static bool ItemIsEquipped(this vItemManager itemManager, int id, out vItemListOperations.EquipedItemInfo equipedItemInfo)
		{
			equipedItemInfo = null;
			if (itemManager.inventory)
			{
				vEquipArea vEquipArea = Array.Find<vEquipArea>(itemManager.inventory.equipAreas, (vEquipArea equipArea) => equipArea.currentEquippedItem && equipArea.currentEquippedItem.id.Equals(id));
				if (vEquipArea)
				{
					equipedItemInfo = new vItemListOperations.EquipedItemInfo(vEquipArea.currentEquippedItem, vEquipArea);
					equipedItemInfo.indexOfArea = Array.IndexOf<vEquipArea>(itemManager.inventory.equipAreas, vEquipArea);
					equipedItemInfo.indexOfItem = itemManager.items.IndexOf(vEquipArea.currentEquippedItem);
				}
				return vEquipArea != null;
			}
			return false;
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000C200C File Offset: 0x000C020C
		public static bool ItemTypeIsEquipped(this vItemManager itemManager, vItemType type, out vItemListOperations.EquipedItemInfo equipedItemInfo)
		{
			equipedItemInfo = null;
			if (itemManager.inventory)
			{
				vEquipArea vEquipArea = Array.Find<vEquipArea>(itemManager.inventory.equipAreas, (vEquipArea equipArea) => equipArea.currentEquippedItem && equipArea.currentEquippedItem.type.Equals(type));
				if (vEquipArea)
				{
					equipedItemInfo = new vItemListOperations.EquipedItemInfo(vEquipArea.currentEquippedItem, vEquipArea);
					equipedItemInfo.indexOfArea = Array.IndexOf<vEquipArea>(itemManager.inventory.equipAreas, vEquipArea);
					equipedItemInfo.indexOfItem = itemManager.items.IndexOf(vEquipArea.currentEquippedItem);
				}
				return vEquipArea != null;
			}
			return false;
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000C20A4 File Offset: 0x000C02A4
		public static vItem GetEquippedItem(this vItemManager itemManager, int id)
		{
			if (!itemManager.inventory)
			{
				return null;
			}
			vEquipArea vEquipArea = Array.Find<vEquipArea>(itemManager.inventory.equipAreas, (vEquipArea equipArea) => equipArea.currentEquippedItem && equipArea.currentEquippedItem.id.Equals(id));
			if (!vEquipArea)
			{
				return null;
			}
			return vEquipArea.currentEquippedItem;
		}

		// Token: 0x02000573 RID: 1395
		public class EquipedItemInfo
		{
			// Token: 0x0600213A RID: 8506 RVA: 0x00004D1F File Offset: 0x00002F1F
			public EquipedItemInfo()
			{
			}

			// Token: 0x0600213B RID: 8507 RVA: 0x000C20FA File Offset: 0x000C02FA
			public EquipedItemInfo(vItem item, vEquipArea area)
			{
				this.item = item;
				this.area = area;
			}

			// Token: 0x040021E6 RID: 8678
			public vItem item;

			// Token: 0x040021E7 RID: 8679
			public int indexOfItem;

			// Token: 0x040021E8 RID: 8680
			public vEquipArea area;

			// Token: 0x040021E9 RID: 8681
			public int indexOfArea;
		}
	}
}
