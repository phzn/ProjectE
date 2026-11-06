using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.HideFlash;
using Features.InsertableToys;
using Features.Inventory.Wearable;
using Features.Localization;
using Features.WardrobeCombos;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.Inventory.UI
{
	// Token: 0x0200115A RID: 4442
	public static class InventoryItemDetails
	{
		// Token: 0x06008748 RID: 34632 RVA: 0x003759EC File Offset: 0x00373BEC
		public static string Build(InventoryItemSO item, ItemRarity rarity, int variantIndex, bool longWayHome = false)
		{
			if (item == null)
			{
				return string.Empty;
			}
			RaritySettingsSO instance = RaritySettingsSO.Instance;
			Color color = ((instance != null) ? instance.GetColor(rarity) : Color.white);
			MCSurfaceVariantSO mcsurfaceVariantSO = ItemVariantLook.ResolveVariant(item, variantIndex);
			StringBuilder stringBuilder = new StringBuilder(768);
			stringBuilder.Append("<size=20><color=#").Append(ColorUtility.ToHtmlStringRGB(color)).Append("><b>")
				.Append(GameLocalization.GetSource("rarity", rarity.ToString().ToUpperInvariant()))
				.Append("</b></color>");
			if (mcsurfaceVariantSO != null && !string.IsNullOrWhiteSpace(mcsurfaceVariantSO.displayName) && !string.Equals(mcsurfaceVariantSO.displayName.Trim(), "Default", StringComparison.OrdinalIgnoreCase))
			{
				stringBuilder.Append("  <color=#DDD1E4>").Append(GameLocalization.Format("inventory.variant", "Variant: {0}", new object[] { InventoryItemDetails.Escape(GameLocalization.GetSource("item_variant", mcsurfaceVariantSO.displayName)) })).Append("</color>");
			}
			stringBuilder.Append("\n\n").Append(InventoryItemDetails.Escape(GameLocalization.GetSource("items", item.description)));
			if (item.isVaulted)
			{
				return stringBuilder.Append("\n\n<color=#DDD1E4>").Append(InventoryItemDetails.T("Temporarily unavailable.")).Append("</color></size>")
					.ToString();
			}
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string text = (item.itemId ?? string.Empty).Trim().ToLowerInvariant();
			WearableItemSO wearableItemSO = item as WearableItemSO;
			float num = 0f;
			if (wearableItemSO != null)
			{
				num = wearableItemSO.ResolveDetectionCoverage(mcsurfaceVariantSO);
				if (!Mathf.Approximately(num, 0f))
				{
					list.Add(InventoryItemDetails.Tint(GameLocalization.Format("inventory.coverage", "Coverage {0}", new object[] { ((num > 0f) ? "+" : string.Empty) + InventoryItemDetails.Number(num) }), num > 0f));
				}
				if (wearableItemSO.forcesNaked)
				{
					list.Add(InventoryItemDetails.Tint("Counts as exposed while worn", false));
				}
				InventoryItemDetails.AddRestrictions(list, wearableItemSO.blockedActivitiesWhileWorn);
				if (wearableItemSO.staminaCostMultiplier > 0f && wearableItemSO.staminaCostMultiplier < 1f)
				{
					list.Add(InventoryItemDetails.Tint(GameLocalization.Format("inventory.stamina", "{0}% less stamina consumed while worn", new object[] { InventoryItemDetails.Number((1f - wearableItemSO.staminaCostMultiplier) * 100f) }), true));
				}
				if (wearableItemSO.hasWearDuration)
				{
					if (longWayHome && item.category == InventoryCategory.Clothing)
					{
						list.Add(InventoryItemDetails.T("No wear loss in Long Way Home"));
					}
					else
					{
						float num2 = ((instance != null) ? instance.GetDurationMultiplier(item.rarity, rarity) : 1f);
						list.Add(GameLocalization.Format("inventory.wear_time", "Base wear time: {0}", new object[] { InventoryItemDetails.Duration(wearableItemSO.wearDuration * num2) }));
					}
				}
				if (wearableItemSO.wearActions != null)
				{
					foreach (WearableActionDefinition wearableActionDefinition in wearableItemSO.wearActions)
					{
						if (!(wearableActionDefinition == null) && InventoryItemDetails.IsSpecialAction(wearableActionDefinition.ActionType))
						{
							InventoryItemDetails.AddAction(list2, wearableActionDefinition.DisplayName);
							InventoryItemDetails.AddAction(list2, wearableActionDefinition.DisplayNameWhenActive);
						}
					}
				}
			}
			HideFlashItemSO hideFlashItemSO = item as HideFlashItemSO;
			if (hideFlashItemSO != null)
			{
				if (hideFlashItemSO.coverage > 0f)
				{
					list.Add(InventoryItemDetails.Tint(GameLocalization.Format("inventory.concealment", "{0}% less visual detection while closed", new object[] { InventoryItemDetails.Number(Mathf.Clamp01(hideFlashItemSO.coverage) * 100f) }), true));
				}
				if (hideFlashItemSO.flashActionEntry != null)
				{
					InventoryItemDetails.AddAction(list2, "Flash");
					list.Add(InventoryItemDetails.T("Cover is lost during the flash"));
				}
			}
			InsertableToyItemSO insertableToyItemSO = item as InsertableToyItemSO;
			if (insertableToyItemSO != null)
			{
				if (insertableToyItemSO.AnalPlacement != null && insertableToyItemSO.AnalPlacement.Enabled)
				{
					InventoryItemDetails.AddAction(list2, insertableToyItemSO.AnalPlacement.ActionName);
				}
				if (insertableToyItemSO.VaginalPlacement != null && insertableToyItemSO.VaginalPlacement.Enabled)
				{
					InventoryItemDetails.AddAction(list2, insertableToyItemSO.VaginalPlacement.ActionName);
				}
				InventoryItemDetails.AddAction(list2, insertableToyItemSO.RemoveActionName);
				if (insertableToyItemSO.InsertedRunningSpeed > 0f)
				{
					list.Add(InventoryItemDetails.Tint(GameLocalization.Format("inventory.inserted_speed", "Run speed while inserted: {0} m/s", new object[] { InventoryItemDetails.Number(insertableToyItemSO.InsertedRunningSpeed) }), false));
				}
			}
			InventoryItemDetails.AddSpecialActions(text, list, list2);
			if (item.isVehicleItem)
			{
				InventoryItemDetails.AddAction(list2, "Ride");
			}
			if (item.hasDurability && (wearableItemSO == null || !wearableItemSO.hasWearDuration))
			{
				DurabilityDrainMode durabilityDrainMode = item.durabilityDrainMode;
				if (durabilityDrainMode != DurabilityDrainMode.OnlyWhileInUse)
				{
					if (durabilityDrainMode == DurabilityDrainMode.ManualPerUse)
					{
						list.Add(Mathf.Approximately(item.maxDurability, 1f) ? GameLocalization.Format("inventory.use_one", "{0} use", new object[] { InventoryItemDetails.Number(item.maxDurability) }) : GameLocalization.Format("inventory.use_many", "{0} uses", new object[] { InventoryItemDetails.Number(item.maxDurability) }));
					}
					else
					{
						list.Add((wearableItemSO != null) ? GameLocalization.Format("inventory.worn_time", "Base time while worn: {0}", new object[] { InventoryItemDetails.Duration(item.maxDurability) }) : GameLocalization.Format("inventory.equipped_time", "Base time while equipped: {0}", new object[] { InventoryItemDetails.Duration(item.maxDurability) }));
					}
				}
				else
				{
					list.Add(GameLocalization.Format("inventory.use_time", "Base use time: {0}", new object[] { InventoryItemDetails.Duration(item.maxDurability) }));
				}
			}
			InventoryItemDetails.AppendSection(stringBuilder, "EFFECTS", list);
			if (list2.Count > 0)
			{
				stringBuilder.Append("\n\n<size=18><color=#D8BEDF><b>").Append(InventoryItemDetails.T("SPECIAL ACTIONS")).Append("</b></color></size>\n")
					.Append(string.Join("  /  ", list2));
			}
			bool flag = text != "cloth_glasses" || num >= 3f;
			if (text == "cloth_classic_panties")
			{
				flag = WardrobeComboRules.IsLegendarySurface(item, variantIndex);
			}
			string text2;
			if (flag && WardrobePerkCatalog.TryGetInventoryMarkup(text, out text2))
			{
				stringBuilder.Append("\n\n").Append(text2.Replace("<size=18>", "<size=20>").Replace("<size=15>", "<size=18>").Replace("<size=14>", "<size=18>"));
			}
			return stringBuilder.Append("</size>").ToString();
		}

		// Token: 0x06008749 RID: 34633 RVA: 0x003760D0 File Offset: 0x003742D0
		private static bool IsSpecialAction(WearableActionType type)
		{
			return type == WearableActionType.SecondaryEffectToggle || type == WearableActionType.TimeFreeze;
		}

		// Token: 0x0600874A RID: 34634 RVA: 0x003760DC File Offset: 0x003742DC
		private static void AddSpecialActions(string id, List<string> effects, List<string> actions)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(id);
			if (num <= 2233071915U)
			{
				if (num <= 1696501429U)
				{
					if (num <= 567478011U)
					{
						if (num != 468466985U)
						{
							if (num != 567478011U)
							{
								return;
							}
							if (!(id == "cloth_bikini_panties"))
							{
								return;
							}
						}
						else
						{
							if (!(id == "hitachi"))
							{
								return;
							}
							InventoryItemDetails.AddAction(actions, "Use / stop");
							return;
						}
					}
					else if (num != 1128563062U)
					{
						if (num != 1696501429U)
						{
							return;
						}
						if (!(id == "egg_rotor"))
						{
							return;
						}
						InventoryItemDetails.AddAction(actions, "Vibration: off / low / medium / high");
						InventoryItemDetails.AddAction(actions, "Egg position: inside / outside / swing");
						return;
					}
					else
					{
						if (!(id == "wanted_poster"))
						{
							return;
						}
						effects.Add(InventoryItemDetails.Tint("Discarding may lower suspicion", true));
						InventoryItemDetails.AddAction(actions, "Show / hide face");
						InventoryItemDetails.AddAction(actions, "Discard");
						return;
					}
				}
				else if (num <= 2019719684U)
				{
					if (num != 1780411953U)
					{
						if (num != 2019719684U)
						{
							return;
						}
						if (!(id == "pencil"))
						{
							return;
						}
						InventoryItemDetails.AddAction(actions, "Apply body paint");
						return;
					}
					else
					{
						if (!(id == "cloth_raincoat"))
						{
							return;
						}
						InventoryItemDetails.AddAction(actions, "Open / close coat");
						InventoryItemDetails.AddAction(actions, "Toggle hood");
						return;
					}
				}
				else if (num != 2232676845U)
				{
					if (num != 2233071915U)
					{
						return;
					}
					if (!(id == "dildo"))
					{
						return;
					}
					InventoryItemDetails.AddAction(actions, "Floor and wall poses");
					return;
				}
				else
				{
					if (!(id == "custom_bodypaint_pen"))
					{
						return;
					}
					InventoryItemDetails.AddAction(actions, "Create body paint");
					return;
				}
			}
			else
			{
				if (num > 2665451265U)
				{
					if (num <= 3488487075U)
					{
						if (num != 3344473519U)
						{
							if (num != 3488487075U)
							{
								return;
							}
							if (!(id == "cloth_expose_panties"))
							{
								return;
							}
						}
						else
						{
							if (!(id == "cloth_towel"))
							{
								return;
							}
							InventoryItemDetails.AddAction(actions, "Flash");
							InventoryItemDetails.AddAction(actions, "Slip off");
							return;
						}
					}
					else if (num != 4022191764U)
					{
						if (num != 4135811292U)
						{
							return;
						}
						if (!(id == "cloth_expose_bra"))
						{
							return;
						}
					}
					else
					{
						if (!(id == "cloth_bikini_bra"))
						{
							return;
						}
						goto IL_01F8;
					}
					InventoryItemDetails.AddAction(actions, "Open / close");
					return;
				}
				if (num <= 2535808792U)
				{
					if (num != 2519494092U)
					{
						if (num != 2535808792U)
						{
							return;
						}
						if (!(id == "nipple_rotor"))
						{
							return;
						}
						InventoryItemDetails.AddAction(actions, "Vibration: off / low / medium / high");
						return;
					}
					else
					{
						if (!(id == "cloth_mask"))
						{
							return;
						}
						InventoryItemDetails.AddAction(actions, "Lower / raise mask");
						return;
					}
				}
				else if (num != 2549134794U)
				{
					if (num != 2665451265U)
					{
						return;
					}
					if (!(id == "cloth_swimsuit"))
					{
						return;
					}
					effects.Add(InventoryItemDetails.Tint("May slip off when diving", false));
					InventoryItemDetails.AddAction(actions, "Flash");
					InventoryItemDetails.AddAction(actions, "Open / close");
					InventoryItemDetails.AddAction(actions, "Slip off");
					return;
				}
				else
				{
					if (!(id == "flexi_dildo"))
					{
						return;
					}
					InventoryItemDetails.AddAction(actions, "Use near a wall");
					InventoryItemDetails.AddAction(actions, "Adjust speed");
					return;
				}
			}
			IL_01F8:
			effects.Add(InventoryItemDetails.Tint("May slip while running or diving", false));
			InventoryItemDetails.AddAction(actions, "Flash");
			InventoryItemDetails.AddAction(actions, "Open / close");
			InventoryItemDetails.AddAction(actions, "Readjust");
		}

		// Token: 0x0600874B RID: 34635 RVA: 0x00376420 File Offset: 0x00374620
		private static void AddRestrictions(List<string> effects, PlayerActivity blocked)
		{
			List<string> list = new List<string>();
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.ArmsPose, "arm poses");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.Cover, "wall cover");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.Flash, "flash");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.LingerieOpen, "opening clothes");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.Emote, "emotes");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.Crouch, "crouching");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.StanceSwitch, "stance changes");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.Undress, "undressing");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.Interact, "world interactions");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.Inventory, "inventory");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.Phone, "phone");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.RapidHUD, "quick actions");
			InventoryItemDetails.AddRestriction(list, blocked, PlayerActivity.DebugFly, "free camera");
			if (list.Count > 0)
			{
				effects.Add(InventoryItemDetails.Tint(GameLocalization.Format("inventory.blocks", "Blocks {0} while worn", new object[] { string.Join(", ", list) }), false));
			}
		}

		// Token: 0x0600874C RID: 34636 RVA: 0x0037652F File Offset: 0x0037472F
		private static void AddRestriction(List<string> labels, PlayerActivity blocked, PlayerActivity activity, string label)
		{
			if ((blocked & activity) != PlayerActivity.None)
			{
				labels.Add(InventoryItemDetails.T(label));
			}
		}

		// Token: 0x0600874D RID: 34637 RVA: 0x00376544 File Offset: 0x00374744
		private static void AddAction(List<string> actions, string action)
		{
			if (string.IsNullOrWhiteSpace(action))
			{
				return;
			}
			string label = InventoryItemDetails.Escape(InventoryItemDetails.T(action.Trim()));
			if (!actions.Exists((string value) => string.Equals(value, label, StringComparison.OrdinalIgnoreCase)))
			{
				actions.Add(label);
			}
		}

		// Token: 0x0600874E RID: 34638 RVA: 0x00376596 File Offset: 0x00374796
		private static void AppendSection(StringBuilder text, string title, List<string> lines)
		{
			if (lines.Count == 0)
			{
				return;
			}
			text.Append("\n\n<size=18><color=#D8BEDF><b>").Append(InventoryItemDetails.T(title)).Append("</b></color></size>\n")
				.Append(string.Join("\n", lines));
		}

		// Token: 0x0600874F RID: 34639 RVA: 0x003765D2 File Offset: 0x003747D2
		private static string Tint(string value, bool positive)
		{
			return string.Concat(new string[]
			{
				"<color=",
				positive ? "#ACDFC2" : "#FFAFC1",
				">",
				InventoryItemDetails.T(value),
				"</color>"
			});
		}

		// Token: 0x06008750 RID: 34640 RVA: 0x00376612 File Offset: 0x00374812
		private static string T(string value)
		{
			return GameLocalization.GetSource("inventory_ui", value);
		}

		// Token: 0x06008751 RID: 34641 RVA: 0x0037661F File Offset: 0x0037481F
		private static string Number(float value)
		{
			return value.ToString("0.#", CultureInfo.InvariantCulture);
		}

		// Token: 0x06008752 RID: 34642 RVA: 0x00376634 File Offset: 0x00374834
		private static string Duration(float seconds)
		{
			int num = Mathf.Max(0, Mathf.RoundToInt(seconds));
			if (num < 60)
			{
				return GameLocalization.Format("inventory.seconds", "{0} s", new object[] { num });
			}
			int num2 = num % 60;
			if (num2 <= 0)
			{
				return GameLocalization.Format("inventory.minutes", "{0} min", new object[] { num / 60 });
			}
			return GameLocalization.Format("inventory.minutes_seconds", "{0} min {1} s", new object[]
			{
				num / 60,
				num2
			});
		}

		// Token: 0x06008753 RID: 34643 RVA: 0x003766C7 File Offset: 0x003748C7
		private static string Escape(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				return value.Trim().Replace("<", "‹").Replace(">", "›");
			}
			return string.Empty;
		}

		// Token: 0x040089FE RID: 35326
		private const string Positive = "#ACDFC2";

		// Token: 0x040089FF RID: 35327
		private const string Negative = "#FFAFC1";
	}
}
