using System;
using Features.Localization;

namespace Features.WardrobeCombos
{
	// Token: 0x02000B63 RID: 2915
	public static class WardrobePerkCatalog
	{
		// Token: 0x060052CA RID: 21194 RVA: 0x00201C3C File Offset: 0x001FFE3C
		public static bool TryGetInventoryMarkup(string itemId, out string markup)
		{
			markup = null;
			if (string.IsNullOrWhiteSpace(itemId))
			{
				return false;
			}
			string text = itemId.Trim().ToLowerInvariant();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 2492328868U)
			{
				if (num <= 1512805593U)
				{
					if (num != 355800519U)
					{
						if (num != 1460279449U)
						{
							if (num == 1512805593U)
							{
								if (text == "cloth_gym_set_leggings")
								{
									markup = WardrobePerkCatalog.Entry("ATHLETIC", "Combine with Gym Tank Top", "Infinite stamina.");
								}
							}
						}
						else if (text == "cloth_ghost_main")
						{
							markup = WardrobePerkCatalog.Entry("GHOST", "Granted by this outfit", "NPCs scream and flee on sight; no photos or encounters while worn.");
						}
					}
					else if (text == "cloth_classic_panties")
					{
						markup = WardrobePerkCatalog.Entry("LUCKY", "Legendary rainbow variant only", "Invulnerable; reporters and police ignore you. Civilian detection remains active.");
					}
				}
				else if (num != 1661301672U)
				{
					if (num != 1722608525U)
					{
						if (num == 2492328868U)
						{
							if (text == "cloth_gagball")
							{
								markup = WardrobePerkCatalog.Entry("KINKY", "Combine with Rope", "Civilian final detection is blocked; reporters and police still react.");
							}
						}
					}
					else if (text == "cloth_panties_head_face")
					{
						markup = WardrobePerkCatalog.Entry("HIDDEN IDENTITY", "Granted by this item", "Reporters ignore you; civilian final detection is blocked.");
					}
				}
				else if (text == "cloth_rope")
				{
					markup = WardrobePerkCatalog.Entry("KINKY", "Combine with Gag Ball or Blindfold", "Civilian final detection is blocked; reporters and police still react.");
				}
			}
			else if (num <= 3373469908U)
			{
				if (num != 2519494092U)
				{
					if (num != 2945562100U)
					{
						if (num == 3373469908U)
						{
							if (text == "cloth_gym_set_full")
							{
								markup = WardrobePerkCatalog.Entry("ATHLETIC", "Granted by this full outfit", "Infinite stamina.");
							}
						}
					}
					else if (text == "cloth_glasses")
					{
						markup = WardrobePerkCatalog.Entry("HIDDEN IDENTITY", "Sunglasses variant + Mask", "Reporters ignore you; civilian final detection is blocked.");
					}
				}
				else if (text == "cloth_mask")
				{
					markup = WardrobePerkCatalog.Entry("HIDDEN IDENTITY", "Combine with Sunglasses", "Reporters ignore you; civilian final detection is blocked.");
				}
			}
			else if (num != 3379568419U)
			{
				if (num != 3678708888U)
				{
					if (num == 3814889008U)
					{
						if (text == "cloth_gym_set_tanktop")
						{
							markup = WardrobePerkCatalog.Entry("ATHLETIC", "Combine with Gym Leggings", "Infinite stamina.");
						}
					}
				}
				else if (text == "cloth_blindfold")
				{
					markup = WardrobePerkCatalog.Entry("BLIND", "Granted while worn", "See only nearby. NPCs beyond 3 m cannot catch you; police still see and arrest you.") + "\n\n" + WardrobePerkCatalog.Entry("KINKY", "Combine with Rope", "Civilian final detection is blocked; reporters and police still react.");
				}
			}
			else if (text == "headbag")
			{
				markup = WardrobePerkCatalog.Entry("HIDDEN IDENTITY", "Granted by this item", "Reporters ignore you; civilian final detection is blocked.");
			}
			return !string.IsNullOrEmpty(markup);
		}

		// Token: 0x060052CB RID: 21195 RVA: 0x00201F64 File Offset: 0x00200164
		private static string Entry(string perk, string requirement, string effect)
		{
			perk = GameLocalization.GetSource("wardrobe_perks", perk);
			requirement = GameLocalization.GetSource("wardrobe_perks", requirement);
			effect = GameLocalization.GetSource("wardrobe_perks", effect);
			string source = GameLocalization.GetSource("wardrobe_perks", "POSSIBLE PERK");
			return string.Concat(new string[] { "<size=14><color=#B9B3BD><b>", source, "</b></color></size>\n<size=18><color=#F5F1EA><b>", perk, "</b></color></size><size=15><color=#E95C7A>  ", requirement, "</color></size>\n<size=14><color=#C9C4CC>", effect, "</color></size>" });
		}
	}
}
