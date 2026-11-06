using System;
using System.Collections.Generic;
using Features.Inventory;
using UnityEngine;

namespace Features.TrashcanLoot
{
	// Token: 0x02000BE1 RID: 3041
	[CreateAssetMenu(fileName = "LootTable_New", menuName = "Features/Trashcan Loot/Loot Table")]
	public class LootTableSO : ScriptableObject
	{
		// Token: 0x06005778 RID: 22392 RVA: 0x002224A6 File Offset: 0x002206A6
		public InventoryItemSO Roll(HashSet<InventoryItemSO> exclude = null)
		{
			return this.RollFiltered(ItemRarity.Common, exclude, null, null, null);
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x002224B3 File Offset: 0x002206B3
		public InventoryItemSO RollWithMinRarity(ItemRarity minRarity, HashSet<InventoryItemSO> exclude = null)
		{
			return this.RollFiltered(minRarity, exclude, null, null, null);
		}

		// Token: 0x0600577A RID: 22394 RVA: 0x002224C0 File Offset: 0x002206C0
		public InventoryItemSO RollMatching(Predicate<InventoryItemSO> predicate, ItemRarity minRarity, HashSet<InventoryItemSO> exclude = null, IReadOnlyList<LootTableSO.LootEntry> supplements = null)
		{
			return this.RollFiltered(minRarity, exclude, predicate, supplements, null);
		}

		// Token: 0x0600577B RID: 22395 RVA: 0x002224D0 File Offset: 0x002206D0
		public InventoryItemSO RollForTrashcan(ItemRarity minRarity = ItemRarity.Common, HashSet<InventoryItemSO> exclude = null, Predicate<InventoryItemSO> predicate = null, IReadOnlyList<LootTableSO.LootEntry> supplements = null)
		{
			return this.RollFiltered(minRarity, exclude, predicate, supplements, (InventoryItemSO item) => TrashcanDiscoveryLootPolicy.ItemWeightMultiplier(item, minRarity));
		}

		// Token: 0x0600577C RID: 22396 RVA: 0x00222508 File Offset: 0x00220708
		private InventoryItemSO RollFiltered(ItemRarity minRarity, HashSet<InventoryItemSO> exclude, Predicate<InventoryItemSO> predicate = null, IReadOnlyList<LootTableSO.LootEntry> supplements = null, Func<InventoryItemSO, float> weightMultiplier = null)
		{
			int num = this.entries.Count + ((supplements != null) ? supplements.Count : 0);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				LootTableSO.LootEntry lootEntry = this.ResolveEntry(i, supplements);
				if (lootEntry != null && !(lootEntry.item == null) && !lootEntry.item.isVaulted && lootEntry.weight > 0f && (predicate == null || predicate(lootEntry.item)) && lootEntry.item.EffectiveMaxRollableRarity >= minRarity && (exclude == null || !exclude.Contains(lootEntry.item)))
				{
					num2 += ((weightMultiplier == null) ? lootEntry.weight : (lootEntry.weight * weightMultiplier(lootEntry.item)));
				}
			}
			if (num2 <= 0f)
			{
				return null;
			}
			float num3 = Random.value * num2;
			for (int j = 0; j < num; j++)
			{
				LootTableSO.LootEntry lootEntry2 = this.ResolveEntry(j, supplements);
				if (lootEntry2 != null && !(lootEntry2.item == null) && !lootEntry2.item.isVaulted && lootEntry2.weight > 0f && (predicate == null || predicate(lootEntry2.item)) && lootEntry2.item.EffectiveMaxRollableRarity >= minRarity && (exclude == null || !exclude.Contains(lootEntry2.item)))
				{
					num3 -= ((weightMultiplier == null) ? lootEntry2.weight : (lootEntry2.weight * weightMultiplier(lootEntry2.item)));
					if (num3 <= 0f)
					{
						return lootEntry2.item;
					}
				}
			}
			return null;
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x002226B4 File Offset: 0x002208B4
		private LootTableSO.LootEntry ResolveEntry(int index, IReadOnlyList<LootTableSO.LootEntry> supplements)
		{
			if (index < this.entries.Count)
			{
				return this.entries[index];
			}
			int num = index - this.entries.Count;
			LootTableSO.LootEntry lootEntry = supplements[num];
			if (lootEntry == null || lootEntry.item == null)
			{
				return null;
			}
			foreach (LootTableSO.LootEntry lootEntry2 in this.entries)
			{
				if (lootEntry2 != null && lootEntry2.item == lootEntry.item)
				{
					return null;
				}
			}
			for (int i = 0; i < num; i++)
			{
				if (supplements[i] != null && supplements[i].item == lootEntry.item)
				{
					return null;
				}
			}
			return lootEntry;
		}

		// Token: 0x040057DB RID: 22491
		public List<LootTableSO.LootEntry> entries = new List<LootTableSO.LootEntry>();

		// Token: 0x02000BE2 RID: 3042
		[Serializable]
		public class LootEntry
		{
			// Token: 0x040057DC RID: 22492
			public InventoryItemSO item;

			// Token: 0x040057DD RID: 22493
			[Min(0f)]
			[Tooltip("Relative weight inside the pool (chance = weight / total weight of eligible entries).")]
			public float weight = 1f;
		}
	}
}
