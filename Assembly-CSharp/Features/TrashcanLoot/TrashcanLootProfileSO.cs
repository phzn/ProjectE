using System;
using System.Collections.Generic;
using Features.Inventory;
using Features.RunModes;
using UnityEngine;

namespace Features.TrashcanLoot
{
	// Token: 0x02000BEE RID: 3054
	[CreateAssetMenu(fileName = "TrashcanProfile_New", menuName = "Features/Trashcan Loot/Trashcan Profile")]
	public class TrashcanLootProfileSO : ScriptableObject
	{
		// Token: 0x060057F8 RID: 22520 RVA: 0x00225F60 File Offset: 0x00224160
		public List<TrashcanLootProfileSO.RolledLoot> RollLoot()
		{
			List<TrashcanLootProfileSO.RolledLoot> list = new List<TrashcanLootProfileSO.RolledLoot>();
			if (this.lootTable == null)
			{
				return list;
			}
			HashSet<InventoryItemSO> hashSet = new HashSet<InventoryItemSO>();
			bool isLongWayHome = RunModeService.IsLongWayHome;
			int num = Random.Range(this.minItems, Mathf.Max(this.minItems, this.maxItems) + 1);
			float trashcanLootMultiplier = RunModeService.TrashcanLootMultiplier;
			if (trashcanLootMultiplier != 1f)
			{
				int num2 = ((num > 0) ? 1 : 0);
				float num3 = (float)num * trashcanLootMultiplier;
				num = Mathf.FloorToInt(num3);
				float num4 = num3 - (float)num;
				if (num4 > 0f && Random.value < num4)
				{
					num++;
				}
				num = Mathf.Max(num2, num);
			}
			for (int i = 0; i < num; i++)
			{
				InventoryItemSO inventoryItemSO = this.lootTable.RollForTrashcan(ItemRarity.Common, hashSet, isLongWayHome ? new Predicate<InventoryItemSO>(LongWayHomeLootPolicy.IsAllowedTrashLoot) : null, isLongWayHome ? LongWayHomeLootPolicy.SupplementalTrashLoot : null);
				if (inventoryItemSO == null)
				{
					break;
				}
				hashSet.Add(inventoryItemSO);
				list.Add(TrashcanLootProfileSO.MakeInstance(inventoryItemSO, ItemRarity.Common));
			}
			if (this.guaranteeRarity && list.Count > 0)
			{
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].rarity >= this.guaranteedRarity)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					int num5 = Random.Range(0, list.Count);
					hashSet.Remove(list[num5].item);
					InventoryItemSO inventoryItemSO2 = this.lootTable.RollForTrashcan(this.guaranteedRarity, hashSet, isLongWayHome ? new Predicate<InventoryItemSO>(LongWayHomeLootPolicy.IsAllowedTrashLoot) : null, isLongWayHome ? LongWayHomeLootPolicy.SupplementalTrashLoot : null);
					if (inventoryItemSO2 != null)
					{
						hashSet.Add(inventoryItemSO2);
						list[num5] = TrashcanLootProfileSO.MakeInstance(inventoryItemSO2, this.guaranteedRarity);
					}
					else
					{
						hashSet.Add(list[num5].item);
						Debug.LogWarning(string.Format("[TrashcanLoot] Profile '{0}' guarantees {1} ", base.name, this.guaranteedRarity) + "but no loot table entry can reach that rarity.", this);
					}
				}
			}
			list.Sort((TrashcanLootProfileSO.RolledLoot a, TrashcanLootProfileSO.RolledLoot b) => a.rarity.CompareTo(b.rarity));
			return list;
		}

		// Token: 0x060057F9 RID: 22521 RVA: 0x00226184 File Offset: 0x00224384
		private static TrashcanLootProfileSO.RolledLoot MakeInstance(InventoryItemSO item, ItemRarity minTier)
		{
			RaritySettingsSO instance = RaritySettingsSO.Instance;
			ItemRarity itemRarity = ((instance != null) ? instance.RollInstanceRarity(item, minTier) : ((ItemRarity)Mathf.Max((int)item.rarity, (int)minTier)));
			return new TrashcanLootProfileSO.RolledLoot
			{
				item = item,
				rarity = itemRarity,
				variantIndex = TrashcanDiscoveryLootPolicy.PickVariant(item, itemRarity)
			};
		}

		// Token: 0x0400586C RID: 22636
		[Tooltip("Tier of the trashcan itself (colors the prompt label; the texture lives on the prefab).")]
		public ItemRarity trashcanRarity;

		// Token: 0x0400586D RID: 22637
		[Tooltip("Pool this can rolls from. Can be shared across tiers.")]
		public LootTableSO lootTable;

		// Token: 0x0400586E RID: 22638
		[Header("Yield")]
		[Min(1f)]
		public int minItems = 1;

		// Token: 0x0400586F RID: 22639
		[Min(1f)]
		public int maxItems = 2;

		// Token: 0x04005870 RID: 22640
		[Header("Guarantee")]
		[Tooltip("If true, at least one rolled item is guaranteed to be of guaranteedRarity or better.")]
		public bool guaranteeRarity;

		// Token: 0x04005871 RID: 22641
		public ItemRarity guaranteedRarity = ItemRarity.Rare;

		// Token: 0x02000BEF RID: 3055
		[Serializable]
		public struct RolledLoot
		{
			// Token: 0x04005872 RID: 22642
			public InventoryItemSO item;

			// Token: 0x04005873 RID: 22643
			public ItemRarity rarity;

			// Token: 0x04005874 RID: 22644
			public int variantIndex;
		}
	}
}
