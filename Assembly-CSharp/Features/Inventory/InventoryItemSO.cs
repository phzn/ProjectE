using System;
using System.Collections.Generic;
using Features.Detection;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010FB RID: 4347
	[CreateAssetMenu(fileName = "Item_New", menuName = "Features/Inventory/Item Definition")]
	public class InventoryItemSO : ScriptableObject
	{
		// Token: 0x170014DC RID: 5340
		// (get) Token: 0x06008382 RID: 33666 RVA: 0x0035A789 File Offset: 0x00358989
		public ItemRarity EffectiveMaxRollableRarity
		{
			get
			{
				return (ItemRarity)Mathf.Max((int)this.rarity, (int)this.maxRollableRarity);
			}
		}

		// Token: 0x06008383 RID: 33667 RVA: 0x0035A79C File Offset: 0x0035899C
		public int PickVariantForTier(ItemRarity tier)
		{
			if (this.rarityVariants == null || this.rarityVariants.Count == 0)
			{
				return -1;
			}
			int num = 0;
			for (int i = 0; i < this.rarityVariants.Count; i++)
			{
				if (this.rarityVariants[i] != null && this.rarityVariants[i].tier == tier)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return 0;
			}
			int num2 = Random.Range(0, num);
			for (int j = 0; j < this.rarityVariants.Count; j++)
			{
				RarityVariantEntry rarityVariantEntry = this.rarityVariants[j];
				if (rarityVariantEntry != null && rarityVariantEntry.tier == tier)
				{
					if (num2 == 0)
					{
						return rarityVariantEntry.surfaceIndex;
					}
					num2--;
				}
			}
			return 0;
		}

		// Token: 0x170014DD RID: 5341
		// (get) Token: 0x06008384 RID: 33668 RVA: 0x0035A84D File Offset: 0x00358A4D
		public Vector3 EffectiveHandScaleMultiplier
		{
			get
			{
				if (!(this.handScaleMultiplier == Vector3.zero))
				{
					return this.handScaleMultiplier;
				}
				return Vector3.one;
			}
		}

		// Token: 0x0400867B RID: 34427
		[Header("Metadata")]
		[Tooltip("Temporarily unavailable: hidden from Collection and blocked from loot, inventory and use. Keep the asset and item ID for save compatibility.")]
		public bool isVaulted;

		// Token: 0x0400867C RID: 34428
		[Tooltip("Routes this item to the hotbar (Item) or the cloth slots (Clothing). NOT derived from the script type - the Head Bag is a WearableItemSO but stays an Item.")]
		public InventoryCategory category;

		// Token: 0x0400867D RID: 34429
		[Tooltip("BASE loot rarity of this item. Drives world aura, prompt label color and inventory slot outlines for base-tier instances; per-INSTANCE rarity can roll higher (see maxRollableRarity), carried by the inventory slot / world pickup, not by this asset.")]
		public ItemRarity rarity;

		// Token: 0x0400867E RID: 34430
		[Tooltip("Hide this item's world pickup halo and light while retaining the pickup prompt and rarity UI.")]
		public bool hideWorldPickupAura;

		// Token: 0x0400867F RID: 34431
		[Tooltip("Highest rarity an INSTANCE of this item can roll at drop time. Values at or below the base rarity mean FIXED rarity (the default 'Common' on old assets keeps every item fixed until configured). Upgraded instances get a scaled wear duration and a look from the matching tier pool in rarityVariants (weights in RaritySettings).")]
		public ItemRarity maxRollableRarity;

		// Token: 0x04008680 RID: 34432
		[Tooltip("Per-tier look pools: at drop time the instance picks a random surface variant among the entries of its rolled tier (base tier included). Empty = item has no look variants (instances always keep the preset's current surface). Populated by Tools/Inventory/Rarity/Apply Rarity Config.")]
		public List<RarityVariantEntry> rarityVariants = new List<RarityVariantEntry>();

		// Token: 0x04008681 RID: 34433
		public string itemName;

		// Token: 0x04008682 RID: 34434
		public string itemId;

		// Token: 0x04008683 RID: 34435
		[Tooltip("Pixelate this item in streamer mode, including inventory icons and collection previews.")]
		public bool sensitiveVisual;

		// Token: 0x04008684 RID: 34436
		public Sprite icon;

		// Token: 0x04008685 RID: 34437
		[TextArea]
		public string description;

		// Token: 0x04008686 RID: 34438
		[Header("Stacking")]
		[Tooltip("If true, identical instances can share one hotbar slot and are consumed/dropped one at a time.")]
		public bool isStackable;

		// Token: 0x04008687 RID: 34439
		[Min(1f)]
		[Tooltip("Maximum quantity held in one hotbar slot when isStackable is enabled.")]
		public int maxStackSize = 1;

		// Token: 0x04008688 RID: 34440
		[Header("Prefabs")]
		[Tooltip("The prefab held in hand (for picking up and holding)")]
		public GameObject handPrefab;

		// Token: 0x04008689 RID: 34441
		[Tooltip("The prefab placed in the world (for floor)")]
		public GameObject floorPrefab;

		// Token: 0x0400868A RID: 34442
		[Tooltip("The prefab placed in the world (for wall)")]
		public GameObject wallPrefab;

		// Token: 0x0400868B RID: 34443
		[Header("Holding Settings")]
		[Tooltip("HumanBodyBones bone to parent the hand prefab under")]
		public HumanBodyBones attachBone = HumanBodyBones.RightHand;

		// Token: 0x0400868C RID: 34444
		public Vector3 handPositionOffset;

		// Token: 0x0400868D RID: 34445
		public Vector3 handRotationOffset;

		// Token: 0x0400868E RID: 34446
		[Tooltip("Per-item scale multiplier applied to the held prefab. Existing items keep their prefab-authored scale when this is (1,1,1).")]
		public Vector3 handScaleMultiplier = Vector3.one;

		// Token: 0x0400868F RID: 34447
		[Header("Animation")]
		[Tooltip("The animator bool parameter to set true when holding this item")]
		public string holdAnimationParameter = "HoldingItem";

		// Token: 0x04008690 RID: 34448
		[Tooltip("Weight of the hold layer (0-1)")]
		public float holdLayerWeight = 1f;

		// Token: 0x04008691 RID: 34449
		[Tooltip("Optional: name of an ADDITIONAL animator layer to drive while this item is held (set to its weight on equip, 0 on unequip). Used by the Wanted Poster's two-arm 'hold in front of face' pose (PosterHold layer). Leave empty for normal items.")]
		public string secondaryHoldLayerName = "";

		// Token: 0x04008692 RID: 34450
		[Tooltip("Weight of the secondary hold layer (0-1) when this item is equipped.")]
		public float secondaryHoldLayerWeight = 1f;

		// Token: 0x04008693 RID: 34451
		[Tooltip("Specific pickup animation for this item (if null, uses manager default)")]
		public PlayerActionEntry pickupAnimationOverride;

		// Token: 0x04008694 RID: 34452
		[Header("Usage & Durability")]
		[Tooltip("If true, this item can be placed in the world")]
		public bool isPlaceable = true;

		// Token: 0x04008695 RID: 34453
		[Tooltip("Requires a stationary surface for placement and placed actions (for example the Flexi Dildo).")]
		public bool requiresStaticPlacement;

		// Token: 0x04008696 RID: 34454
		[Tooltip("If true, the item has a limited usage duration/quantity")]
		public bool hasDurability;

		// Token: 0x04008697 RID: 34455
		[Tooltip("Max duration/quantity of usage")]
		public float maxDurability = 100f;

		// Token: 0x04008698 RID: 34456
		[Tooltip("When durability drains (hotbar items only; clothes use hasWearDuration). WhileEquipped: whenever the item is out on the body (prop in hand or worn). OnlyWhileInUse: only during an actual use session (toys). ManualPerUse: the feature consumes discrete charges explicitly. At 0 the item is consumed.")]
		public DurabilityDrainMode durabilityDrainMode;

		// Token: 0x04008699 RID: 34457
		[Tooltip("If true, can be picked back up into inventory after being placed")]
		public bool canBePickedUpAfterPlacement = true;

		// Token: 0x0400869A RID: 34458
		[Tooltip("If true, a placed instance exposes the global placed-actions (e.g. dildo poses) in the HUD. If false, only 'Pick Up' is offered (used by the Wanted Poster).")]
		public bool supportsPlacedActions = true;

		// Token: 0x0400869B RID: 34459
		[Header("Vehicle Usage")]
		[Tooltip("If true, floorPrefab is treated as a rideable vehicle. When placed it is not wrapped in PlacedDildoInteractable; instead its vVehicleInteractable is wired to return this item to inventory on dismount. When equipped in hand, physics/vehicle components are disabled so it behaves as a static prop.")]
		public bool isVehicleItem;

		// Token: 0x0400869C RID: 34460
		[Header("Audio")]
		public AudioClip pickupSFX;

		// Token: 0x0400869D RID: 34461
		public AudioClip equipSFX;

		// Token: 0x0400869E RID: 34462
		public AudioClip unequipSFX;

		// Token: 0x0400869F RID: 34463
		public AudioClip dropSFX;
	}
}
