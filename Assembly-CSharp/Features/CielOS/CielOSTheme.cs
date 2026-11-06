using System;
using Features.CielOS.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS
{
	// Token: 0x02001393 RID: 5011
	[CreateAssetMenu(fileName = "CielOSTheme", menuName = "CielOS/Theme", order = 10)]
	public class CielOSTheme : ScriptableObject
	{
		// Token: 0x170018B6 RID: 6326
		// (get) Token: 0x06009B17 RID: 39703 RVA: 0x003F6D0E File Offset: 0x003F4F0E
		// (set) Token: 0x06009B18 RID: 39704 RVA: 0x003F6D15 File Offset: 0x003F4F15
		public static CielOSTheme Active { get; set; }

		// Token: 0x06009B19 RID: 39705 RVA: 0x003F6D1D File Offset: 0x003F4F1D
		public static void Round(Image img)
		{
			if (img == null)
			{
				return;
			}
			img.sprite = CielOSShapes.RoundedRect(10);
			img.type = Image.Type.Sliced;
			img.pixelsPerUnitMultiplier = 1f;
		}

		// Token: 0x06009B1A RID: 39706 RVA: 0x003F6D48 File Offset: 0x003F4F48
		public static void Circle(Image img)
		{
			if (img == null)
			{
				return;
			}
			img.sprite = CielOSShapes.Circle(64);
			img.type = Image.Type.Simple;
			img.preserveAspect = true;
		}

		// Token: 0x06009B1B RID: 39707 RVA: 0x003F6D70 File Offset: 0x003F4F70
		public static Image AddBorder(RectTransform target, Color? colorOverride = null, float thickness = 1f)
		{
			if (target == null)
			{
				return null;
			}
			Color color = colorOverride ?? ((CielOSTheme.Active != null) ? CielOSTheme.Active.outline : new Color(0.78f, 0.62f, 0.9f, 0.35f));
			Transform transform = target.Find("ThemeBorder");
			Image image;
			if (transform != null)
			{
				image = transform.GetComponent<Image>();
			}
			else
			{
				GameObject gameObject = new GameObject("ThemeBorder", new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				});
				RectTransform rectTransform = (RectTransform)gameObject.transform;
				rectTransform.SetParent(target, false);
				rectTransform.SetAsFirstSibling();
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.one;
				rectTransform.offsetMin = new Vector2(-thickness, -thickness);
				rectTransform.offsetMax = new Vector2(thickness, thickness);
				image = gameObject.GetComponent<Image>();
				image.raycastTarget = false;
			}
			image.color = color;
			CielOSTheme.Round(image);
			return image;
		}

		// Token: 0x06009B1C RID: 39708 RVA: 0x003F6E80 File Offset: 0x003F5080
		public static void Outline(Graphic graphic, Color? colorOverride = null, float distance = 1f)
		{
			if (graphic == null)
			{
				return;
			}
			Outline outline = graphic.GetComponent<Outline>();
			if (outline == null)
			{
				outline = graphic.gameObject.AddComponent<Outline>();
			}
			outline.effectColor = colorOverride ?? ((CielOSTheme.Active != null) ? CielOSTheme.Active.outline : new Color(0.78f, 0.62f, 0.9f, 0.35f));
			outline.effectDistance = new Vector2(distance, -distance);
		}

		// Token: 0x04009FFF RID: 40959
		private static readonly Color Violet = new Color(0.663f, 0.376f, 0.788f, 1f);

		// Token: 0x0400A000 RID: 40960
		[Header("Desktop")]
		[Tooltip("Fallback desktop background colour when no wallpaper sprite is set.")]
		public Color desktopBackground = new Color(0.07f, 0.05f, 0.1f, 1f);

		// Token: 0x0400A001 RID: 40961
		[Tooltip("Default wallpaper sprite. Optional.")]
		public Sprite defaultWallpaper;

		// Token: 0x0400A002 RID: 40962
		[Header("Panel (top bar)")]
		public Color panelBackground = new Color(0.09f, 0.06f, 0.12f, 0.96f);

		// Token: 0x0400A003 RID: 40963
		public Color panelText = new Color(0.9f, 0.86f, 0.95f, 1f);

		// Token: 0x0400A004 RID: 40964
		public Color panelAccent = new Color(0.663f, 0.376f, 0.788f, 1f);

		// Token: 0x0400A005 RID: 40965
		[Header("Windows")]
		public Color windowBackground = new Color(0.11f, 0.09f, 0.14f, 0.98f);

		// Token: 0x0400A006 RID: 40966
		public Color windowBorder = new Color(0.663f, 0.376f, 0.788f, 1f);

		// Token: 0x0400A007 RID: 40967
		public Color titleBarActive = new Color(0.18f, 0.12f, 0.24f, 1f);

		// Token: 0x0400A008 RID: 40968
		public Color titleBarInactive = new Color(0.12f, 0.1f, 0.15f, 1f);

		// Token: 0x0400A009 RID: 40969
		public Color titleText = new Color(0.93f, 0.9f, 0.96f, 1f);

		// Token: 0x0400A00A RID: 40970
		public Color closeButton = new Color(0.86f, 0.32f, 0.46f, 1f);

		// Token: 0x0400A00B RID: 40971
		public Color minimizeButton = new Color(0.6f, 0.55f, 0.66f, 1f);

		// Token: 0x0400A00C RID: 40972
		[Header("Generic UI")]
		public Color accent = new Color(0.663f, 0.376f, 0.788f, 1f);

		// Token: 0x0400A00D RID: 40973
		public Color controlBackground = new Color(0.18f, 0.14f, 0.23f, 1f);

		// Token: 0x0400A00E RID: 40974
		public Color controlText = new Color(0.88f, 0.84f, 0.93f, 1f);

		// Token: 0x0400A00F RID: 40975
		[Header("Outline / hover")]
		[Tooltip("Light 1px border colour for windows, widgets, cards.")]
		public Color outline = new Color(0.78f, 0.62f, 0.9f, 0.35f);

		// Token: 0x0400A010 RID: 40976
		[Tooltip("Hover tint applied to buttons / tiles on pointer-over.")]
		public Color hover = new Color(0.663f, 0.376f, 0.788f, 0.22f);

		// Token: 0x0400A011 RID: 40977
		[Header("Tray / dock")]
		[Tooltip("Tint for top-bar tray glyphs (wifi/volume/bell/power).")]
		public Color trayIconTint = new Color(0.86f, 0.8f, 0.92f, 1f);

		// Token: 0x0400A012 RID: 40978
		[Tooltip("Translucent background of the side dock rail.")]
		public Color dockRail = new Color(0.1f, 0.07f, 0.14f, 0.78f);

		// Token: 0x0400A013 RID: 40979
		[Tooltip("Indicator dot shown under a dock app that has an open window.")]
		public Color dockActiveIndicator = new Color(0.78f, 0.5f, 0.92f, 1f);

		// Token: 0x0400A014 RID: 40980
		[Header("System widget meters")]
		[Tooltip("Empty track of a meter bar.")]
		public Color barTrack = new Color(0.2f, 0.15f, 0.26f, 1f);

		// Token: 0x0400A015 RID: 40981
		[Tooltip("Filled portion of a meter bar.")]
		public Color barFill = new Color(0.663f, 0.376f, 0.788f, 1f);

		// Token: 0x0400A016 RID: 40982
		[Header("Desktop icon selection")]
		public Color selection = new Color(0.663f, 0.376f, 0.788f, 0.28f);

		// Token: 0x0400A017 RID: 40983
		[Header("Logo (dragon mark, two-tone)")]
		public Color logoPrimary = new Color(0.78f, 0.52f, 0.95f, 1f);

		// Token: 0x0400A018 RID: 40984
		public Color logoAccent = new Color(0.95f, 0.86f, 1f, 1f);

		// Token: 0x0400A019 RID: 40985
		[Header("Fonts")]
		[Tooltip("Monospace font for terminal / code-y UI. Optional; falls back to TMP default.")]
		public TMP_FontAsset monospaceFont;

		// Token: 0x0400A01A RID: 40986
		[Tooltip("UI font for labels and menus. Optional; falls back to TMP default.")]
		public TMP_FontAsset uiFont;

		// Token: 0x0400A01B RID: 40987
		[Header("Shapes (9-sliced sprites for a rounded, modern look)")]
		[Tooltip("Rounded-rectangle 9-slice sprite for panels, windows, buttons, dock, cards.")]
		public Sprite roundedSprite;

		// Token: 0x0400A01C RID: 40988
		[Tooltip("Circle sprite for icon badges / avatars.")]
		public Sprite circleSprite;

		// Token: 0x0400A01D RID: 40989
		[Tooltip("Soft shadow sprite (9-slice) for elevation under windows/cards.")]
		public Sprite shadowSprite;

		// Token: 0x0400A01F RID: 40991
		public const float CornerRadiusPPU = 2f;
	}
}
