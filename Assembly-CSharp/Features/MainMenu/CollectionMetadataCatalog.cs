using System;
using Features.Inventory;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x02001031 RID: 4145
	public sealed class CollectionMetadataCatalog : ScriptableObject
	{
		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x06007D83 RID: 32131 RVA: 0x00336AE8 File Offset: 0x00334CE8
		public CollectionMetadataCatalog.Entry[] Entries
		{
			get
			{
				return this.entries ?? Array.Empty<CollectionMetadataCatalog.Entry>();
			}
		}

		// Token: 0x17001437 RID: 5175
		// (get) Token: 0x06007D84 RID: 32132 RVA: 0x00336AF9 File Offset: 0x00334CF9
		public bool IsCurrent
		{
			get
			{
				return this.schemaVersion == 1;
			}
		}

		// Token: 0x04008162 RID: 33122
		public const string ResourcePath = "CollectionMetadataCatalog";

		// Token: 0x04008163 RID: 33123
		public const int CurrentSchemaVersion = 1;

		// Token: 0x04008164 RID: 33124
		[SerializeField]
		private int schemaVersion = 1;

		// Token: 0x04008165 RID: 33125
		[SerializeField]
		private CollectionMetadataCatalog.Entry[] entries = Array.Empty<CollectionMetadataCatalog.Entry>();

		// Token: 0x02001032 RID: 4146
		[Serializable]
		public sealed class Entry
		{
			// Token: 0x06007D86 RID: 32134 RVA: 0x00336B20 File Offset: 0x00334D20
			public ResourceRequest LoadPreviewAsync()
			{
				if (!string.IsNullOrEmpty(this.previewReferencePath))
				{
					return Resources.LoadAsync<CollectionItemReference>(this.previewReferencePath);
				}
				if (this.item == null || string.IsNullOrEmpty(this.item.resourcePath))
				{
					return null;
				}
				return Resources.LoadAsync<InventoryItemSO>(this.item.resourcePath);
			}

			// Token: 0x06007D87 RID: 32135 RVA: 0x00336B74 File Offset: 0x00334D74
			public InventoryItemSO ResolvePreview(Object loaded)
			{
				CollectionItemReference collectionItemReference = loaded as CollectionItemReference;
				InventoryItemSO inventoryItemSO = ((collectionItemReference != null) ? collectionItemReference.Item : (loaded as InventoryItemSO));
				if (!(inventoryItemSO != null) || this.item == null || !(inventoryItemSO.itemId == this.item.itemId) || !(inventoryItemSO.name == this.item.assetName))
				{
					return null;
				}
				return inventoryItemSO;
			}

			// Token: 0x04008166 RID: 33126
			public WardrobeMetadataIndex.Entry item;

			// Token: 0x04008167 RID: 33127
			public bool authored;

			// Token: 0x04008168 RID: 33128
			public bool excluded;

			// Token: 0x04008169 RID: 33129
			public string previewReferencePath;
		}
	}
}
