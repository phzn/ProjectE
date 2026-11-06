using System;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x02001107 RID: 4359
	[CreateAssetMenu(fileName = "RaritySettings", menuName = "Features/Inventory/Rarity Settings")]
	public class RaritySettingsSO : ScriptableObject
	{
		// Token: 0x170014F9 RID: 5369
		// (get) Token: 0x06008452 RID: 33874 RVA: 0x0035F8BF File Offset: 0x0035DABF
		public static RaritySettingsSO Instance
		{
			get
			{
				if (RaritySettingsSO._instance == null)
				{
					RaritySettingsSO._instance = Resources.Load<RaritySettingsSO>("RaritySettings");
				}
				return RaritySettingsSO._instance;
			}
		}

		// Token: 0x06008453 RID: 33875 RVA: 0x0035F8E2 File Offset: 0x0035DAE2
		public Color GetColor(ItemRarity rarity)
		{
			if (rarity == ItemRarity.Rare)
			{
				return this.rareColor;
			}
			if (rarity != ItemRarity.Legendary)
			{
				return this.commonColor;
			}
			return this.legendaryColor;
		}

		// Token: 0x06008454 RID: 33876 RVA: 0x0035F902 File Offset: 0x0035DB02
		public bool ShowAura(ItemRarity rarity)
		{
			return rarity != ItemRarity.Common || this.auraOnCommon;
		}

		// Token: 0x06008455 RID: 33877 RVA: 0x0035F90F File Offset: 0x0035DB0F
		public bool ShowOutline(ItemRarity rarity)
		{
			return rarity != ItemRarity.Common || this.outlineOnCommon;
		}

		// Token: 0x06008456 RID: 33878 RVA: 0x0035F91C File Offset: 0x0035DB1C
		public bool ColorLabel(ItemRarity rarity)
		{
			return rarity != ItemRarity.Common || this.labelColorOnCommon;
		}

		// Token: 0x06008457 RID: 33879 RVA: 0x0035F92C File Offset: 0x0035DB2C
		public float GetDurationMultiplier(ItemRarity baseRarity, ItemRarity instanceRarity)
		{
			if (instanceRarity <= baseRarity)
			{
				return 1f;
			}
			float num = this.ReferenceWearDuration(baseRarity);
			float num2 = this.ReferenceWearDuration(instanceRarity);
			if (num <= 0f)
			{
				return 1f;
			}
			return num2 / num;
		}

		// Token: 0x06008458 RID: 33880 RVA: 0x0035F964 File Offset: 0x0035DB64
		private float ReferenceWearDuration(ItemRarity rarity)
		{
			if (rarity == ItemRarity.Rare)
			{
				return this.wearDurationRare;
			}
			if (rarity != ItemRarity.Legendary)
			{
				return this.wearDurationCommon;
			}
			return this.wearDurationLegendary;
		}

		// Token: 0x06008459 RID: 33881 RVA: 0x0035F984 File Offset: 0x0035DB84
		public ItemRarity RollInstanceRarity(InventoryItemSO item, ItemRarity minTier = ItemRarity.Common)
		{
			if (item == null)
			{
				return minTier;
			}
			ItemRarity rarity = item.rarity;
			ItemRarity effectiveMaxRollableRarity = item.EffectiveMaxRollableRarity;
			ItemRarity itemRarity = (ItemRarity)Mathf.Max((int)rarity, (int)minTier);
			if (itemRarity >= effectiveMaxRollableRarity)
			{
				if (itemRarity > effectiveMaxRollableRarity)
				{
					return effectiveMaxRollableRarity;
				}
				return itemRarity;
			}
			else
			{
				float num = 0f;
				for (int i = (int)itemRarity; i <= (int)effectiveMaxRollableRarity; i++)
				{
					num += this.WeightForDelta(i - (int)rarity);
				}
				if (num <= 0f)
				{
					return itemRarity;
				}
				float num2 = Random.value * num;
				for (int j = (int)itemRarity; j <= (int)effectiveMaxRollableRarity; j++)
				{
					num2 -= this.WeightForDelta(j - (int)rarity);
					if (num2 <= 0f)
					{
						return (ItemRarity)j;
					}
				}
				return effectiveMaxRollableRarity;
			}
		}

		// Token: 0x0600845A RID: 33882 RVA: 0x0035FA20 File Offset: 0x0035DC20
		public bool CanRollInstanceRarity(InventoryItemSO item, ItemRarity tier, ItemRarity minTier = ItemRarity.Common)
		{
			if (item == null)
			{
				return false;
			}
			ItemRarity effectiveMaxRollableRarity = item.EffectiveMaxRollableRarity;
			ItemRarity itemRarity = (ItemRarity)Mathf.Max((int)item.rarity, (int)minTier);
			if (itemRarity >= effectiveMaxRollableRarity)
			{
				return tier == ((itemRarity <= effectiveMaxRollableRarity) ? itemRarity : effectiveMaxRollableRarity);
			}
			if (tier < itemRarity || tier > effectiveMaxRollableRarity)
			{
				return false;
			}
			float num = 0f;
			for (int i = (int)itemRarity; i <= (int)effectiveMaxRollableRarity; i++)
			{
				num += this.WeightForDelta(i - (int)item.rarity);
			}
			if (num > 0f)
			{
				return this.WeightForDelta(tier - item.rarity) > 0f;
			}
			return tier == itemRarity;
		}

		// Token: 0x0600845B RID: 33883 RVA: 0x0035FAAB File Offset: 0x0035DCAB
		private float WeightForDelta(int delta)
		{
			if (delta >= 2)
			{
				return Mathf.Max(0f, this.rollWeightPlusTwo);
			}
			if (delta == 1)
			{
				return Mathf.Max(0f, this.rollWeightPlusOne);
			}
			return Mathf.Max(0f, this.rollWeightBase);
		}

		// Token: 0x170014FA RID: 5370
		// (get) Token: 0x0600845C RID: 33884 RVA: 0x0035FAE7 File Offset: 0x0035DCE7
		public float AuraIntensity
		{
			get
			{
				return this.auraIntensity;
			}
		}

		// Token: 0x170014FB RID: 5371
		// (get) Token: 0x0600845D RID: 33885 RVA: 0x0035FAEF File Offset: 0x0035DCEF
		public float AuraRange
		{
			get
			{
				return this.auraRange;
			}
		}

		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x0600845E RID: 33886 RVA: 0x0035FAF7 File Offset: 0x0035DCF7
		public float AuraHeight
		{
			get
			{
				return this.auraHeight;
			}
		}

		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x0600845F RID: 33887 RVA: 0x0035FAFF File Offset: 0x0035DCFF
		public float AuraPulseSpeed
		{
			get
			{
				return this.auraPulseSpeed;
			}
		}

		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x06008460 RID: 33888 RVA: 0x0035FB07 File Offset: 0x0035DD07
		public float AuraPulseAmount
		{
			get
			{
				return this.auraPulseAmount;
			}
		}

		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x06008461 RID: 33889 RVA: 0x0035FB0F File Offset: 0x0035DD0F
		public float OutlineWidth
		{
			get
			{
				return this.outlineWidth;
			}
		}

		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x06008462 RID: 33890 RVA: 0x0035FB17 File Offset: 0x0035DD17
		public float AuraLightIntensity
		{
			get
			{
				return this.auraLightIntensity;
			}
		}

		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x06008463 RID: 33891 RVA: 0x0035FB1F File Offset: 0x0035DD1F
		public float HaloRadius
		{
			get
			{
				return this.haloRadius;
			}
		}

		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x06008464 RID: 33892 RVA: 0x0035FB27 File Offset: 0x0035DD27
		public float HaloAlpha
		{
			get
			{
				return this.haloAlpha;
			}
		}

		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x06008465 RID: 33893 RVA: 0x0035FB2F File Offset: 0x0035DD2F
		public float HaloHeightOffset
		{
			get
			{
				return this.haloHeightOffset;
			}
		}

		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x06008466 RID: 33894 RVA: 0x0035FB37 File Offset: 0x0035DD37
		public float HaloShadowAlpha
		{
			get
			{
				return this.haloShadowAlpha;
			}
		}

		// Token: 0x0400871E RID: 34590
		private const string ResourceName = "RaritySettings";

		// Token: 0x0400871F RID: 34591
		[Header("Rarity colors (aura + prompt label + UI outline)")]
		[SerializeField]
		private Color commonColor = new Color(0.78f, 0.78f, 0.78f, 1f);

		// Token: 0x04008720 RID: 34592
		[SerializeField]
		private Color rareColor = new Color(0.25f, 0.55f, 1f, 1f);

		// Token: 0x04008721 RID: 34593
		[SerializeField]
		private Color legendaryColor = new Color(1f, 0.78f, 0.15f, 1f);

		// Token: 0x04008722 RID: 34594
		[Header("World aura (light on world pickups)")]
		[Tooltip("If false, Common items get no world aura (only Rare/Legendary glow).")]
		[SerializeField]
		private bool auraOnCommon = true;

		// Token: 0x04008723 RID: 34595
		[SerializeField]
		private float auraIntensity = 1.6f;

		// Token: 0x04008724 RID: 34596
		[SerializeField]
		private float auraRange = 1.4f;

		// Token: 0x04008725 RID: 34597
		[Tooltip("Height of the aura light above the pickup's pivot.")]
		[SerializeField]
		private float auraHeight = 0.25f;

		// Token: 0x04008726 RID: 34598
		[Tooltip("Pulse speed (cycles/sec). 0 = steady glow.")]
		[SerializeField]
		private float auraPulseSpeed = 1.2f;

		// Token: 0x04008727 RID: 34599
		[Range(0f, 1f)]
		[Tooltip("Pulse amount as a fraction of the base intensity.")]
		[SerializeField]
		private float auraPulseAmount = 0.25f;

		// Token: 0x04008728 RID: 34600
		[Header("Aura restyle: ground halo + faint light")]
		[Tooltip("Intensity of the point light in the restyled aura. Kept FAINT on purpose: the light only makes the item readable in the dark, the rarity signal is the ground halo. (The old washing-out look used auraIntensity above; that field now only feeds legacy code.)")]
		[SerializeField]
		private float auraLightIntensity = 0.35f;

		// Token: 0x04008729 RID: 34601
		[Tooltip("Radius (m) of the flat halo ring projected on the ground under the pickup.")]
		[SerializeField]
		private float haloRadius = 0.35f;

		// Token: 0x0400872A RID: 34602
		[Range(0f, 1f)]
		[Tooltip("Peak opacity of the ground halo.")]
		[SerializeField]
		private float haloAlpha = 0.65f;

		// Token: 0x0400872B RID: 34603
		[Tooltip("Height of the halo quad above the ground contact point (avoids z-fighting).")]
		[SerializeField]
		private float haloHeightOffset = 0.015f;

		// Token: 0x0400872C RID: 34604
		[Range(0f, 1f)]
		[Tooltip("Opacity of the dark grounding disc UNDER the colored ring. This is what makes the rarity color pop against bright asphalt/grass without putting a panel behind the UI.")]
		[SerializeField]
		private float haloShadowAlpha = 0.5f;

		// Token: 0x0400872D RID: 34605
		[Header("Inventory slot outline")]
		[Tooltip("If false, Common items get no outline in the inventory slots.")]
		[SerializeField]
		private bool outlineOnCommon;

		// Token: 0x0400872E RID: 34606
		[SerializeField]
		private float outlineWidth = 2.5f;

		// Token: 0x0400872F RID: 34607
		[Header("Prompt label")]
		[Tooltip("If false, Common items keep the default (white) prompt label color.")]
		[SerializeField]
		private bool labelColorOnCommon;

		// Token: 0x04008730 RID: 34608
		[Header("Instance rarity roll (per-drop tier upgrade)")]
		[Tooltip("Weight of an instance dropping at the item's BASE rarity.")]
		[SerializeField]
		private float rollWeightBase = 80f;

		// Token: 0x04008731 RID: 34609
		[Tooltip("Weight of an instance dropping ONE tier above base (only for items whose maxRollableRarity allows it).")]
		[SerializeField]
		private float rollWeightPlusOne = 17f;

		// Token: 0x04008732 RID: 34610
		[Tooltip("Weight of an instance dropping TWO tiers above base (Common items rollable to Legendary).")]
		[SerializeField]
		private float rollWeightPlusTwo = 3f;

		// Token: 0x04008733 RID: 34611
		[Header("Wear duration per FOUND rarity (reference seconds)")]
		[Tooltip("Reference wear duration of a Common-found instance. An instance rolled above its base rarity gets its authored wearDuration scaled by the RATIO between these reference tiers (e.g. 60/90/120: Common item rolled Legendary lasts x2, Rare item rolled Legendary lasts x1.333) - so the FOUND rarity, not the delta, decides the duration. Per-item exceptions live in each item's authored wearDuration.")]
		[SerializeField]
		private float wearDurationCommon = 60f;

		// Token: 0x04008734 RID: 34612
		[SerializeField]
		private float wearDurationRare = 90f;

		// Token: 0x04008735 RID: 34613
		[SerializeField]
		private float wearDurationLegendary = 120f;

		// Token: 0x04008736 RID: 34614
		private static RaritySettingsSO _instance;
	}
}
