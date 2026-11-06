using System;
using Features.Inventory;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.ClothingLoot
{
	// Token: 0x0200134B RID: 4939
	[Serializable]
	public struct ClothingLootOffer
	{
		// Token: 0x060098AD RID: 39085 RVA: 0x003E9BE5 File Offset: 0x003E7DE5
		public ClothingLootOffer(WearableItemSO clothing, ItemRarity rarity, int variantIndex)
		{
			this.clothing = clothing;
			this.rarity = rarity;
			this.variantIndex = variantIndex;
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x060098AE RID: 39086 RVA: 0x003E9BFC File Offset: 0x003E7DFC
		public Sprite Icon
		{
			get
			{
				return ItemVariantLook.ResolveIcon(this.clothing, this.variantIndex);
			}
		}

		// Token: 0x04009DC6 RID: 40390
		public WearableItemSO clothing;

		// Token: 0x04009DC7 RID: 40391
		public ItemRarity rarity;

		// Token: 0x04009DC8 RID: 40392
		public int variantIndex;
	}
}
