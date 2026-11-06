using System;
using TMPro;
using UnityEngine;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x020014B0 RID: 5296
	public class DialogueUIThemeSO : ScriptableObject
	{
		// Token: 0x0600A301 RID: 41729 RVA: 0x0043B6E9 File Offset: 0x004398E9
		public static DialogueUIThemeSO Load()
		{
			if (DialogueUIThemeSO._cached == null)
			{
				DialogueUIThemeSO._cached = Resources.Load<DialogueUIThemeSO>("DialogueUITheme");
			}
			return DialogueUIThemeSO._cached;
		}

		// Token: 0x0400A6A6 RID: 42662
		public const string ResourcePath = "DialogueUITheme";

		// Token: 0x0400A6A7 RID: 42663
		[Header("Panels / rows (9-slice)")]
		[Tooltip("character_card_bg — large violet panel background (windows, edit card).")]
		public Sprite panelBackground;

		// Token: 0x0400A6A8 RID: 42664
		[Tooltip("character_card_boarder — violet border frame for large panels.")]
		public Sprite panelBorder;

		// Token: 0x0400A6A9 RID: 42665
		[Tooltip("item_background — list row / control background.")]
		public Sprite itemBackground;

		// Token: 0x0400A6AA RID: 42666
		[Tooltip("item_border — list row / control border.")]
		public Sprite itemBorder;

		// Token: 0x0400A6AB RID: 42667
		[Tooltip("Boarder — generic thin border (badges / chips).")]
		public Sprite genericBorder;

		// Token: 0x0400A6AC RID: 42668
		[Header("Slider")]
		[Tooltip("BMAC_SliderHandle — the real slider handle sprite.")]
		public Sprite sliderHandle;

		// Token: 0x0400A6AD RID: 42669
		[Header("Icons")]
		public Sprite iconX;

		// Token: 0x0400A6AE RID: 42670
		public Sprite iconReset;

		// Token: 0x0400A6AF RID: 42671
		public Sprite iconArrow;

		// Token: 0x0400A6B0 RID: 42672
		[Header("Font")]
		[Tooltip("Karla SDF — the body font used across the customization UI.")]
		public TMP_FontAsset bodyFont;

		// Token: 0x0400A6B1 RID: 42673
		[Tooltip("Optional accent/title font (Roboto Condensed) for headings.")]
		public TMP_FontAsset titleFont;

		// Token: 0x0400A6B2 RID: 42674
		[Header("Palette (sampled from customization)")]
		public Color accent = new Color(0.85f, 0.36f, 0.62f, 1f);

		// Token: 0x0400A6B3 RID: 42675
		public Color panelTint = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0400A6B4 RID: 42676
		public Color rowTint = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0400A6B5 RID: 42677
		public Color textPrimary = new Color(0.96f, 0.94f, 0.98f, 1f);

		// Token: 0x0400A6B6 RID: 42678
		public Color textMuted = new Color(0.74f, 0.7f, 0.8f, 1f);

		// Token: 0x0400A6B7 RID: 42679
		public Color badgeSystem = new Color(0.42f, 0.3f, 0.55f, 1f);

		// Token: 0x0400A6B8 RID: 42680
		public Color badgeCustom = new Color(0.28f, 0.52f, 0.4f, 1f);

		// Token: 0x0400A6B9 RID: 42681
		public Color badgeOverride = new Color(0.55f, 0.36f, 0.22f, 1f);

		// Token: 0x0400A6BA RID: 42682
		public Color sliderFill = new Color(0.78f, 0.45f, 0.9f, 1f);

		// Token: 0x0400A6BB RID: 42683
		private static DialogueUIThemeSO _cached;
	}
}
