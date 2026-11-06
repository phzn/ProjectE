using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.RunModes;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x0200102E RID: 4142
	public static class CollectionItemCatalog
	{
		// Token: 0x06007D7D RID: 32125 RVA: 0x003369A0 File Offset: 0x00334BA0
		public static CollectionMetadataCatalog LoadMetadata()
		{
			if (CollectionItemCatalog._metadataLoadAttempted)
			{
				return CollectionItemCatalog._metadata;
			}
			CollectionItemCatalog._metadataLoadAttempted = true;
			CollectionItemCatalog._metadata = Resources.Load<CollectionMetadataCatalog>("CollectionMetadataCatalog");
			if (CollectionItemCatalog._metadata == null || !CollectionItemCatalog._metadata.IsCurrent)
			{
				CollectionItemCatalog._metadata = null;
				Debug.LogError("[Collection] Missing or incompatible metadata. Rebuild the collection catalogue before Play/build.");
			}
			return CollectionItemCatalog._metadata;
		}

		// Token: 0x06007D7E RID: 32126 RVA: 0x003369FD File Offset: 0x00334BFD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		public static void ResetMetadataCache()
		{
			CollectionItemCatalog._metadata = null;
			CollectionItemCatalog._metadataLoadAttempted = false;
		}

		// Token: 0x06007D7F RID: 32127 RVA: 0x00336A0C File Offset: 0x00334C0C
		public static InventoryItemSO[] Build(IEnumerable<InventoryItemSO> authoredItems)
		{
			CollectionItemCatalog.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.result = new List<InventoryItemSO>();
			CS$<>8__locals1.seen = new HashSet<InventoryItemSO>();
			CollectionItemCatalog.<Build>g__Add|4_0(authoredItems, ref CS$<>8__locals1);
			CollectionItemCatalog.<Build>g__Add|4_0(Resources.LoadAll<WearableItemSO>("Wardrobe"), ref CS$<>8__locals1);
			FreeRunItemCatalogSO freeRunItemCatalogSO = Resources.Load<FreeRunItemCatalogSO>("FreeRunItemCatalog");
			if (freeRunItemCatalogSO != null)
			{
				CollectionItemCatalog.<Build>g__Add|4_0(freeRunItemCatalogSO.items, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.result.ToArray();
		}

		// Token: 0x06007D80 RID: 32128 RVA: 0x00336A78 File Offset: 0x00334C78
		[CompilerGenerated]
		internal static void <Build>g__Add|4_0(IEnumerable<InventoryItemSO> items, ref CollectionItemCatalog.<>c__DisplayClass4_0 A_1)
		{
			if (items == null)
			{
				return;
			}
			foreach (InventoryItemSO inventoryItemSO in items)
			{
				if (inventoryItemSO != null && A_1.seen.Add(inventoryItemSO))
				{
					A_1.result.Add(inventoryItemSO);
				}
			}
		}

		// Token: 0x0400815D RID: 33117
		private static CollectionMetadataCatalog _metadata;

		// Token: 0x0400815E RID: 33118
		private static bool _metadataLoadAttempted;
	}
}
