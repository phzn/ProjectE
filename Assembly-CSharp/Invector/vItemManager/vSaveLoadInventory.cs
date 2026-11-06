using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x02000536 RID: 1334
	public static class vSaveLoadInventory
	{
		// Token: 0x06002021 RID: 8225 RVA: 0x000BE4D0 File Offset: 0x000BC6D0
		public static string InventoryToJsonText(vItemManager itemManager)
		{
			if (!itemManager.inventory)
			{
				return string.Empty;
			}
			vSaveLoadInventory.InventoryData inventoryData = new vSaveLoadInventory.InventoryData();
			vEquipArea[] equipAreas = itemManager.inventory.equipAreas;
			for (int i = 0; i < equipAreas.Length; i++)
			{
				vSaveLoadInventory.EquipAreaData equipAreaData = new vSaveLoadInventory.EquipAreaData();
				equipAreaData.indexOfSelectedSlot = equipAreas[i].indexOfEquippedItem;
				for (int j = 0; j < equipAreas[i].equipSlots.Count; j++)
				{
					vSaveLoadInventory.SlotData slotData = new vSaveLoadInventory.SlotData();
					slotData.hasItem = equipAreas[i].equipSlots[j].item != null;
					if (slotData.hasItem)
					{
						slotData.indexOfItem = itemManager.items.IndexOf(equipAreas[i].equipSlots[j].item);
					}
					equipAreaData.slotsData.Add(slotData);
				}
				inventoryData.equipAreas.Add(equipAreaData);
			}
			for (int k = 0; k < itemManager.items.Count; k++)
			{
				inventoryData.itemReferences.Add(new vSaveLoadInventory.ItemReference(itemManager.items[k]));
			}
			return JsonUtility.ToJson(inventoryData, true);
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x000BE5F4 File Offset: 0x000BC7F4
		public static string LoadInventoryJasonText()
		{
			if (File.Exists(vSaveLoadInventory.InventoryDataFile))
			{
				return File.ReadAllText(vSaveLoadInventory.InventoryDataFile);
			}
			return string.Empty;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x000BE614 File Offset: 0x000BC814
		public static void SaveInventory(this vItemManager itemManager)
		{
			string text = vSaveLoadInventory.InventoryToJsonText(itemManager);
			if (!string.IsNullOrEmpty(text))
			{
				File.WriteAllText(vSaveLoadInventory.InventoryDataFile, text);
				itemManager.onSaveItems.Invoke();
			}
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x000BE648 File Offset: 0x000BC848
		public static void LoadInventory(this vItemManager itemManager)
		{
			string text = vSaveLoadInventory.LoadInventoryJasonText();
			if (!string.IsNullOrEmpty(text))
			{
				vSaveLoadInventory.InventoryData inventoryData = new vSaveLoadInventory.InventoryData();
				JsonUtility.FromJsonOverwrite(text, inventoryData);
				itemManager.items = inventoryData.GetItems(itemManager.itemListData);
				vEquipArea[] equipAreas = itemManager.inventory.equipAreas;
				for (int i = 0; i < equipAreas.Length; i++)
				{
					if (i < inventoryData.equipAreas.Count)
					{
						vEquipArea vEquipArea = equipAreas[i];
						vSaveLoadInventory.EquipAreaData equipAreaData = inventoryData.equipAreas[i];
						vEquipArea.indexOfEquippedItem = equipAreaData.indexOfSelectedSlot;
						for (int j = 0; j < equipAreas[i].equipSlots.Count; j++)
						{
							if (j < equipAreaData.slotsData.Count)
							{
								vSaveLoadInventory.SlotData slotData = equipAreaData.slotsData[j];
								vEquipSlot vEquipSlot = equipAreas[i].equipSlots[j];
								itemManager.temporarilyIgnoreItemAnimation = true;
								if (slotData.hasItem)
								{
									vEquipArea.AddItemToEquipSlot(j, itemManager.items[slotData.indexOfItem], false);
								}
								else
								{
									vEquipArea.RemoveItemOfEquipSlot(j);
								}
							}
						}
					}
				}
			}
			itemManager.inventory.UpdateInventory();
			itemManager.temporarilyIgnoreItemAnimation = false;
			itemManager.onLoadItems.Invoke();
		}

		// Token: 0x040020F1 RID: 8433
		public static string InventoryDataFile = Application.dataPath + Path.DirectorySeparatorChar.ToString() + "InventoryData.json";

		// Token: 0x02000537 RID: 1335
		[Serializable]
		private class InventoryData
		{
			// Token: 0x06002026 RID: 8230 RVA: 0x000BE798 File Offset: 0x000BC998
			public List<vItem> GetItems(vItemListData itemListData)
			{
				List<vItem> list = new List<vItem>();
				int i;
				Predicate<vItem> <>9__0;
				int j;
				for (i = 0; i < this.itemReferences.Count; i = j + 1)
				{
					List<vItem> items = itemListData.items;
					Predicate<vItem> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (vItem a) => a.id.Equals(this.itemReferences[i].id));
					}
					vItem vItem = items.Find(predicate);
					vItem = Object.Instantiate<vItem>(vItem);
					vItem.amount = this.itemReferences[i].amount;
					vItem.attributes = this.itemReferences[i].attributes;
					vItem.name = vItem.name.Replace("(Clone)", string.Empty);
					list.Add(vItem);
					j = i;
				}
				return list;
			}

			// Token: 0x040020F2 RID: 8434
			public List<vSaveLoadInventory.ItemReference> itemReferences = new List<vSaveLoadInventory.ItemReference>();

			// Token: 0x040020F3 RID: 8435
			public List<vSaveLoadInventory.EquipAreaData> equipAreas = new List<vSaveLoadInventory.EquipAreaData>();
		}

		// Token: 0x02000539 RID: 1337
		[Serializable]
		private class EquipAreaData
		{
			// Token: 0x040020F7 RID: 8439
			public List<vSaveLoadInventory.SlotData> slotsData = new List<vSaveLoadInventory.SlotData>();

			// Token: 0x040020F8 RID: 8440
			public int indexOfSelectedSlot;
		}

		// Token: 0x0200053A RID: 1338
		[Serializable]
		private class SlotData
		{
			// Token: 0x040020F9 RID: 8441
			public bool hasItem;

			// Token: 0x040020FA RID: 8442
			public int indexOfItem;
		}

		// Token: 0x0200053B RID: 1339
		[Serializable]
		private class ItemReference
		{
			// Token: 0x0600202C RID: 8236 RVA: 0x000BE8D2 File Offset: 0x000BCAD2
			public ItemReference(vItem item)
			{
				this.amount = item.amount;
				this.id = item.id;
				this.attributes = item.attributes;
			}

			// Token: 0x040020FB RID: 8443
			[SerializeField]
			public int amount;

			// Token: 0x040020FC RID: 8444
			[SerializeField]
			public int id;

			// Token: 0x040020FD RID: 8445
			[SerializeField]
			public List<vItemAttribute> attributes;
		}
	}
}
