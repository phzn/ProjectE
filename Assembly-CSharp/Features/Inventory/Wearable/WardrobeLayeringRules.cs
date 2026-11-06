using System;
using System.Collections.Generic;

namespace Features.Inventory.Wearable
{
	// Token: 0x0200111E RID: 4382
	public static class WardrobeLayeringRules
	{
		// Token: 0x060084D4 RID: 34004 RVA: 0x00362F9C File Offset: 0x0036119C
		public static bool TryFindConflict(WearableItemSO candidate, IEnumerable<WearableItemSO> equipped, out WearableItemSO conflict, out string reason)
		{
			conflict = null;
			reason = null;
			if (candidate == null || equipped == null)
			{
				return false;
			}
			foreach (WearableItemSO wearableItemSO in equipped)
			{
				if (!(wearableItemSO == null) && wearableItemSO != candidate && WardrobeLayeringRules.Conflicts(candidate, wearableItemSO, out reason))
				{
					conflict = wearableItemSO;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060084D5 RID: 34005 RVA: 0x00363014 File Offset: 0x00361214
		public static bool Conflicts(WearableItemSO a, WearableItemSO b, out string reason)
		{
			reason = null;
			if (a == null || b == null || a == b)
			{
				return false;
			}
			string text = a.mcStylePartId ?? string.Empty;
			string text2 = b.mcStylePartId ?? string.Empty;
			if (CollarAvailabilityPolicy.IsCollarPart(text) && CollarAvailabilityPolicy.IsCollarPart(text2))
			{
				reason = "Only one neck accessory can be worn at a time.";
				return true;
			}
			if (!string.IsNullOrEmpty(text) && string.Equals(text, text2, StringComparison.OrdinalIgnoreCase))
			{
				reason = "Both garments use the same wardrobe slot.";
				return true;
			}
			if ((a.mcStylePreset != null && a.mcStylePreset.ignoreOtherSlotConflicts) || (b.mcStylePreset != null && b.mcStylePreset.ignoreOtherSlotConflicts))
			{
				return false;
			}
			if ((WardrobeLayeringRules.IsSlot(text, "Outfit.Shoes") && WardrobeLayeringRules.IsSlot(text2, "Outfit.Socks")) || (WardrobeLayeringRules.IsSlot(text, "Outfit.Socks") && WardrobeLayeringRules.IsSlot(text2, "Outfit.Shoes")))
			{
				reason = "Shoes and socks cannot be worn together.";
				return true;
			}
			if ((WardrobeLayeringRules.IsSlot(text, "Outfit.Blindfold") && WardrobeLayeringRules.IsSlot(text2, "Accessory.Eyewear")) || (WardrobeLayeringRules.IsSlot(text2, "Outfit.Blindfold") && WardrobeLayeringRules.IsSlot(text, "Accessory.Eyewear")))
			{
				reason = "Remove the eyewear or blindfold first.";
				return true;
			}
			if ((WardrobeLayeringRules.IsFullSet(a) && WardrobeLayeringRules.IsFullSetConflict(b)) || (WardrobeLayeringRules.IsFullSet(b) && WardrobeLayeringRules.IsFullSetConflict(a)))
			{
				reason = "This garment overlaps the full outfit.";
				return true;
			}
			if ((WardrobeLayeringRules.IsTowel(a) && WardrobeLayeringRules.IsTowelConflict(b)) || (WardrobeLayeringRules.IsTowel(b) && WardrobeLayeringRules.IsTowelConflict(a)))
			{
				reason = "The towel cannot be layered with body garments.";
				return true;
			}
			if ((WardrobeLayeringRules.IsOnePieceOuter(a) && WardrobeLayeringRules.IsOnePieceConflict(b)) || (WardrobeLayeringRules.IsOnePieceOuter(b) && WardrobeLayeringRules.IsOnePieceConflict(a)))
			{
				reason = "This one-piece garment requires the upper and lower body layers to be removed.";
				return true;
			}
			return false;
		}

		// Token: 0x060084D6 RID: 34006 RVA: 0x003631BE File Offset: 0x003613BE
		private static bool IsFullSet(WearableItemSO item)
		{
			return item != null && WardrobeLayeringRules.FullSetIds.Contains(item.itemId ?? string.Empty);
		}

		// Token: 0x060084D7 RID: 34007 RVA: 0x003631E4 File Offset: 0x003613E4
		private static bool IsOnePieceOuter(WearableItemSO item)
		{
			return item != null && WardrobeLayeringRules.OnePieceOuterIds.Contains(item.itemId ?? string.Empty);
		}

		// Token: 0x060084D8 RID: 34008 RVA: 0x0036320A File Offset: 0x0036140A
		private static bool IsTowel(WearableItemSO item)
		{
			return item != null && string.Equals(item.itemId, "cloth_towel", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060084D9 RID: 34009 RVA: 0x00363228 File Offset: 0x00361428
		private static bool IsFullSetConflict(WearableItemSO other)
		{
			return other != null && (WardrobeLayeringRules.IsMainBodySlot(other.mcStylePartId) || WardrobeLayeringRules.IsFullSet(other) || WardrobeLayeringRules.IsOnePieceOuter(other) || WardrobeLayeringRules.IsSlot(other.mcStylePartId, "Outfit.Misc") || WardrobeLayeringRules.IsSlot(other.mcStylePartId, "Outfit.Drape") || WardrobeLayeringRules.IsSlot(other.mcStylePartId, "Outfit.Socks") || WardrobeLayeringRules.IsSlot(other.mcStylePartId, "Outfit.Shoes"));
		}

		// Token: 0x060084DA RID: 34010 RVA: 0x003632A5 File Offset: 0x003614A5
		private static bool IsTowelConflict(WearableItemSO other)
		{
			return other != null && (WardrobeLayeringRules.IsMainBodySlot(other.mcStylePartId) || WardrobeLayeringRules.IsFullSet(other) || WardrobeLayeringRules.IsOnePieceOuter(other));
		}

		// Token: 0x060084DB RID: 34011 RVA: 0x003632CF File Offset: 0x003614CF
		private static bool IsOnePieceConflict(WearableItemSO other)
		{
			return other != null && (WardrobeLayeringRules.IsMainBodySlot(other.mcStylePartId) || WardrobeLayeringRules.IsTowel(other) || WardrobeLayeringRules.IsFullSet(other));
		}

		// Token: 0x060084DC RID: 34012 RVA: 0x003632F9 File Offset: 0x003614F9
		private static bool IsMainBodySlot(string slot)
		{
			return WardrobeLayeringRules.IsSlot(slot, "Outfit.Top") || WardrobeLayeringRules.IsSlot(slot, "Outfit.Underwear") || WardrobeLayeringRules.IsSlot(slot, "Outfit.Bottom");
		}

		// Token: 0x060084DD RID: 34013 RVA: 0x00363322 File Offset: 0x00361522
		private static bool IsSlot(string actual, string expected)
		{
			return string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x04008787 RID: 34695
		private static readonly HashSet<string> FullSetIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "cloth_cheerleader_suite", "cloth_full_coverage_outfit_full", "cloth_gym_set_full", "cloth_university_uniform_full" };

		// Token: 0x04008788 RID: 34696
		private static readonly HashSet<string> OnePieceOuterIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "cloth_ghost_main", "cloth_onepiece_swimsuit", "cloth_raincoat", "cloth_swimsuit" };
	}
}
