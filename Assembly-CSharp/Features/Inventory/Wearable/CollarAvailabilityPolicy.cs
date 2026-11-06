using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.MainMenu;
using Features.Rank;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x0200110D RID: 4365
	public static class CollarAvailabilityPolicy
	{
		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x06008479 RID: 33913 RVA: 0x0035FED2 File Offset: 0x0035E0D2
		public static IReadOnlyList<string> ItemIds { get; } = Array.AsReadOnly<string>(new string[] { "cloth_collar", "cloth_neck_tag", "cloth_neck_sign" });

		// Token: 0x0600847A RID: 33914 RVA: 0x0035FEDC File Offset: 0x0035E0DC
		public static string GetPartId(string itemId)
		{
			string text;
			if (!(itemId == "cloth_collar"))
			{
				if (!(itemId == "cloth_neck_tag"))
				{
					if (!(itemId == "cloth_neck_sign"))
					{
						text = null;
					}
					else
					{
						text = "Accessory.NeckSign";
					}
				}
				else
				{
					text = "Accessory.NeckTag";
				}
			}
			else
			{
				text = "Accessory.Collar";
			}
			return text;
		}

		// Token: 0x0600847B RID: 33915 RVA: 0x0035FF2D File Offset: 0x0035E12D
		public static string GetPresetId(string itemId)
		{
			if (itemId == "cloth_collar")
			{
				return "collar";
			}
			if (CollarAvailabilityPolicy.GetPartId(itemId) == null)
			{
				return null;
			}
			return itemId;
		}

		// Token: 0x0600847C RID: 33916 RVA: 0x0035FF50 File Offset: 0x0035E150
		public static WearableItemSO LoadItem(string itemId)
		{
			string text;
			if (!(itemId == "cloth_collar"))
			{
				if (!(itemId == "cloth_neck_tag"))
				{
					if (!(itemId == "cloth_neck_sign"))
					{
						text = null;
					}
					else
					{
						text = "Wardrobe/Cloth_NeckSign";
					}
				}
				else
				{
					text = "Wardrobe/Cloth_NeckTag";
				}
			}
			else
			{
				text = "Wardrobe/Cloth_Collar";
			}
			string text2 = text;
			if (text2 == null)
			{
				return null;
			}
			return Resources.Load<WearableItemSO>(text2);
		}

		// Token: 0x0600847D RID: 33917 RVA: 0x0035FFAD File Offset: 0x0035E1AD
		public static bool IsCollarPart(string partId)
		{
			return partId == "Accessory.Collar" || partId == "Accessory.NeckTag" || partId == "Accessory.NeckSign" || partId == "Accessory.LeadChoker";
		}

		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x0600847E RID: 33918 RVA: 0x0035FFE3 File Offset: 0x0035E1E3
		public static bool IsRankUnlocked
		{
			get
			{
				if (!RankProgressionService.IsAdminRankActive)
				{
					RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
					return string.Equals((currentRank != null) ? currentRank.rankId : null, "hornyslut_declared", StringComparison.Ordinal);
				}
				return true;
			}
		}

		// Token: 0x0600847F RID: 33919 RVA: 0x0036000A File Offset: 0x0035E20A
		public static bool IsCollar(InventoryItemSO item)
		{
			return item != null && CollarAvailabilityPolicy.GetPartId(item.itemId) != null;
		}

		// Token: 0x06008480 RID: 33920 RVA: 0x00360028 File Offset: 0x0035E228
		public static bool IsCollarPreset(MCStylePresetSO preset)
		{
			return preset != null && (preset.presetId == "collar" || preset.presetId == "cloth_neck_tag" || preset.presetId == "cloth_neck_sign");
		}

		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x06008481 RID: 33921 RVA: 0x00360076 File Offset: 0x0035E276
		public static bool IsOwned
		{
			get
			{
				return CollarAvailabilityPolicy.IsOwnedItem(CollarAvailabilityPolicy.LoadItem("cloth_collar"));
			}
		}

		// Token: 0x06008482 RID: 33922 RVA: 0x00360088 File Offset: 0x0035E288
		public static bool IsOwnedItem(InventoryItemSO item)
		{
			if (!CollarAvailabilityPolicy.IsCollar(item))
			{
				return false;
			}
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (instance != null)
			{
				for (int i = 0; i < instance.MaxClothSlots; i++)
				{
					InventoryItemSO clothAt = instance.GetClothAt(i);
					if (((clothAt != null) ? clothAt.itemId : null) == item.itemId)
					{
						return true;
					}
				}
			}
			return CollectionUnlockRegistry.IsItemUnlocked(item);
		}

		// Token: 0x06008483 RID: 33923 RVA: 0x003600E7 File Offset: 0x0035E2E7
		public static bool IsAvailable(InventoryItemSO item)
		{
			return item != null && (!CollarAvailabilityPolicy.IsCollar(item) || CollarAvailabilityPolicy.IsRankUnlocked);
		}

		// Token: 0x06008484 RID: 33924 RVA: 0x00360103 File Offset: 0x0035E303
		public static bool CanWear(InventoryItemSO item)
		{
			return CollarAvailabilityPolicy.IsAvailable(item) && (!CollarAvailabilityPolicy.IsCollar(item) || CollarAvailabilityPolicy.IsOwnedItem(item));
		}

		// Token: 0x06008485 RID: 33925 RVA: 0x00360120 File Offset: 0x0035E320
		public static bool IsPresetAvailable(MCStylePresetSO preset)
		{
			return preset != null && (!CollarAvailabilityPolicy.IsCollarPreset(preset) || (CollarAvailabilityPolicy.IsRankUnlocked && CollarAvailabilityPolicy.IsOwnedItem(CollarAvailabilityPolicy.LoadItem((preset.presetId == "collar") ? "cloth_collar" : preset.presetId))));
		}

		// Token: 0x04008745 RID: 34629
		public const string ItemId = "cloth_collar";

		// Token: 0x04008746 RID: 34630
		public const string PartId = "Accessory.Collar";

		// Token: 0x04008747 RID: 34631
		public const string PresetId = "collar";

		// Token: 0x04008748 RID: 34632
		public const string RequiredRankId = "hornyslut_declared";

		// Token: 0x04008749 RID: 34633
		public const string ItemResourcePath = "Wardrobe/Cloth_Collar";

		// Token: 0x0400874A RID: 34634
		public const string NeckTagItemId = "cloth_neck_tag";

		// Token: 0x0400874B RID: 34635
		public const string NeckTagPartId = "Accessory.NeckTag";

		// Token: 0x0400874C RID: 34636
		public const string NeckTagPresetId = "cloth_neck_tag";

		// Token: 0x0400874D RID: 34637
		public const string NeckSignItemId = "cloth_neck_sign";

		// Token: 0x0400874E RID: 34638
		public const string NeckSignPartId = "Accessory.NeckSign";

		// Token: 0x0400874F RID: 34639
		public const string NeckSignPresetId = "cloth_neck_sign";
	}
}
