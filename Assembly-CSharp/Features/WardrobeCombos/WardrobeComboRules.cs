using System;
using Features.Detection;
using Features.Inventory;

namespace Features.WardrobeCombos
{
	// Token: 0x02000B61 RID: 2913
	public static class WardrobeComboRules
	{
		// Token: 0x060052A8 RID: 21160 RVA: 0x002012F4 File Offset: 0x001FF4F4
		public static bool IsLegendarySurface(InventoryItemSO item, int surfaceIndex)
		{
			if (item == null || item.rarityVariants == null || surfaceIndex < 0)
			{
				return false;
			}
			for (int i = 0; i < item.rarityVariants.Count; i++)
			{
				RarityVariantEntry rarityVariantEntry = item.rarityVariants[i];
				if (rarityVariantEntry != null && rarityVariantEntry.surfaceIndex == surfaceIndex && rarityVariantEntry.tier == ItemRarity.Legendary)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x00201354 File Offset: 0x001FF554
		public static WardrobeCombo Evaluate(bool mask, bool sunglasses, bool gymTop, bool gymLeggings, bool gymFull, bool rope, bool gag, bool blindfold, bool classicPanties, bool ghostMain = false, bool headBag = false, bool directIdentityConcealer = false)
		{
			WardrobeCombo wardrobeCombo = WardrobeCombo.None;
			if (headBag || directIdentityConcealer || (mask && sunglasses))
			{
				wardrobeCombo |= WardrobeCombo.HiddenIdentity;
			}
			if (gymFull || (gymTop && gymLeggings))
			{
				wardrobeCombo |= WardrobeCombo.Athletic;
			}
			if (rope && (gag || blindfold))
			{
				wardrobeCombo |= WardrobeCombo.Kinky;
			}
			if (classicPanties)
			{
				wardrobeCombo |= WardrobeCombo.Lucky;
			}
			if (ghostMain)
			{
				wardrobeCombo |= WardrobeCombo.Ghost;
			}
			if (blindfold)
			{
				wardrobeCombo |= WardrobeCombo.Blind;
			}
			return wardrobeCombo;
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x002013AA File Offset: 0x001FF5AA
		public static bool ShouldFleeOnSight(WardrobeCombo combos, bool rawLineOfSight, bool playerFullyHidden)
		{
			return (combos & WardrobeCombo.Ghost) > WardrobeCombo.None && rawLineOfSight && !playerFullyHidden;
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x002013BD File Offset: 0x001FF5BD
		public static bool ShouldIgnore(WardrobeCombo combos, DetectionActorType actor)
		{
			return (combos & WardrobeCombo.Ghost) != WardrobeCombo.None || ((combos & WardrobeCombo.Lucky) != WardrobeCombo.None && (actor == DetectionActorType.Reporter || actor == DetectionActorType.Police)) || ((combos & WardrobeCombo.HiddenIdentity) != WardrobeCombo.None && actor == DetectionActorType.Reporter);
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x002013E1 File Offset: 0x001FF5E1
		public static bool ShouldDisableFinalDetection(WardrobeCombo combos, DetectionActorType actor, float distanceSquared = 0f)
		{
			return ((combos & WardrobeCombo.Blind) != WardrobeCombo.None && actor != DetectionActorType.Police && distanceSquared > 9f) || ((actor == DetectionActorType.Generic || actor == DetectionActorType.Referee) && (combos & (WardrobeCombo.HiddenIdentity | WardrobeCombo.Kinky)) > WardrobeCombo.None);
		}

		// Token: 0x04005340 RID: 21312
		public const float BlindNearRadius = 3f;

		// Token: 0x04005341 RID: 21313
		public const float BlindClearRadius = 1.5f;
	}
}
