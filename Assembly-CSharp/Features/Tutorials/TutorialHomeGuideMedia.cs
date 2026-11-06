using System;
using Features.CielOS.UI;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Tutorials
{
	// Token: 0x02000BD8 RID: 3032
	public static class TutorialHomeGuideMedia
	{
		// Token: 0x06005750 RID: 22352 RVA: 0x00220CAC File Offset: 0x0021EEAC
		public static void Build(RectTransform board, Texture2D desk)
		{
			TutorialHomeGuideMedia.Label(board, "Heading", "PREPARE YOUR NEXT RUN", 64f, 110f, 1152f, 64f, 44f, TutorialHomeGuideMedia.White, true, TextAlignmentOptions.TopLeft);
			TutorialHomeGuideMedia.Label(board, "Guide Explanation", "Set up Ciel and the city. Open Run Modes when you are ready.", 64f, 181f, 1152f, 34f, 23f, TutorialHomeGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
			TutorialHomeGuideMedia.DeskPhoto(board, desk);
			TutorialHomeGuideMedia.Label(board, "Desk Caption", "THE DESK IN CIELHOME", 64f, 482f, 310f, 28f, 17f, TutorialHomeGuideMedia.Pink, true, TextAlignmentOptions.TopLeft);
			TutorialHomeGuideMedia.Panel(board, "Use PC Key", 64f, 526f, 66f, 48f, TutorialHomeGuideMedia.Surface);
			TutorialHomeGuideMedia.Label(board, "Use PC Key Text", "E", 64f, 526f, 66f, 48f, 27f, TutorialHomeGuideMedia.White, true, TextAlignmentOptions.Center);
			TutorialHomeGuideMedia.Label(board, "Use PC Action", "USE THE PC", 148f, 536f, 226f, 34f, 23f, TutorialHomeGuideMedia.White, true, TextAlignmentOptions.TopLeft);
			TutorialHomeGuideMedia.Label(board, "Desktop Hint", "Double-click a desktop icon\nto open its app.", 64f, 590f, 310f, 55f, 19f, TutorialHomeGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
			TutorialHomeGuideMedia.App(board, "customization", "CUSTOMIZATION", "Ciel's appearance\nand outfit", 424f, 238f);
			TutorialHomeGuideMedia.App(board, "customization", "NPC PRESETS", "Characters used\nin the city", 828f, 238f);
			TutorialHomeGuideMedia.App(board, "dialoghi", "DIALOGUES", "Lines for civilians,\nreporters and police", 424f, 370f);
			TutorialHomeGuideMedia.App(board, "rank", "RANK", "Progression, missions\nand unlocks", 828f, 370f);
			TutorialHomeGuideMedia.Panel(board, "Run Modes Card", 424f, 502f, 792f, 130f, TutorialHomeGuideMedia.Surface);
			TutorialHomeGuideMedia.Panel(board, "Run Modes Accent", 424f, 502f, 792f, 2f, TutorialHomeGuideMedia.Pink);
			TutorialHomeGuideMedia.Icon(board, "start_run", 448f, 525f, 84f);
			TutorialHomeGuideMedia.Label(board, "Run Modes Title", "RUN MODES", 558f, 520f, 622f, 36f, 27f, TutorialHomeGuideMedia.White, true, TextAlignmentOptions.TopLeft);
			TutorialHomeGuideMedia.Label(board, "Run Modes Purpose", "Choose a mode and review its options.\nConfirm Start when your setup is ready.", 558f, 566f, 622f, 55f, 21f, TutorialHomeGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
			TutorialHomeGuideMedia.Panel(board, "Footer Rule", 64f, 656f, 1152f, 1f, new Color(0.65f, 0.47f, 0.68f, 0.3f));
			TutorialHomeGuideMedia.Label(board, "Artwork Footer", "ESC  LEAVE CIELOS AND RETURN TO THE ROOM", 64f, 675f, 1152f, 30f, 17f, TutorialHomeGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
		}

		// Token: 0x06005751 RID: 22353 RVA: 0x00220FD8 File Offset: 0x0021F1D8
		private static void App(RectTransform board, string appId, string title, string purpose, float x, float y)
		{
			TutorialHomeGuideMedia.Panel(board, title + " Card", x, y, 388f, 116f, TutorialHomeGuideMedia.Surface);
			TutorialHomeGuideMedia.Icon(board, appId, x + 18f, y + 22f, 72f);
			TutorialHomeGuideMedia.Label(board, title + " Title", title, x + 112f, y + 16f, 258f, 34f, 22f, TutorialHomeGuideMedia.White, true, TextAlignmentOptions.TopLeft);
			TutorialHomeGuideMedia.Label(board, title + " Purpose", purpose, x + 112f, y + 58f, 258f, 52f, 20f, TutorialHomeGuideMedia.Muted, false, TextAlignmentOptions.TopLeft);
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x0022109F File Offset: 0x0021F29F
		private static void Icon(RectTransform board, string appId, float x, float y, float size)
		{
			Image image = TutorialHomeGuideMedia.Rect(appId + " Desktop Icon", board, x, y, size, size).gameObject.AddComponent<Image>();
			CielOSIcon.ApplyBadge(image, CielOSAppIcons.ForApp(appId), 128);
			image.raycastTarget = false;
		}

		// Token: 0x06005753 RID: 22355 RVA: 0x002210DC File Offset: 0x0021F2DC
		private static void DeskPhoto(RectTransform board, Texture2D texture)
		{
			TutorialHomeGuideMedia.Panel(board, "Desk Photo Border", 62f, 236f, 314f, 234f, new Color(0.59f, 0.39f, 0.63f, 0.65f));
			if (texture == null)
			{
				return;
			}
			RawImage rawImage = TutorialHomeGuideMedia.Rect("CielHome Desk", board, 64f, 238f, 310f, 230f).gameObject.AddComponent<RawImage>();
			rawImage.texture = texture;
			float num = 0.52f;
			float num2 = (float)texture.width * num / ((float)texture.height * 1.3478261f);
			rawImage.uvRect = new Rect(0.375f, 0.86f - num2, num, num2);
			rawImage.raycastTarget = false;
		}

		// Token: 0x06005754 RID: 22356 RVA: 0x00221198 File Offset: 0x0021F398
		private static void Panel(RectTransform parent, string name, float x, float y, float width, float height, Color color)
		{
			Image image = TutorialHomeGuideMedia.Rect(name, parent, x, y, width, height).gameObject.AddComponent<Image>();
			image.color = color;
			image.raycastTarget = false;
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x002211C0 File Offset: 0x0021F3C0
		private static TMP_Text Label(RectTransform parent, string name, string value, float x, float y, float width, float height, float size, Color color, bool bold = false, TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft)
		{
			TextMeshProUGUI textMeshProUGUI = TutorialHomeGuideMedia.Rect(name, parent, x, y, width, height).gameObject.AddComponent<TextMeshProUGUI>();
			PauseMenuTutorialPanel componentInParent = parent.GetComponentInParent<PauseMenuTutorialPanel>();
			TMP_Text tmp_Text = ((componentInParent != null) ? componentInParent.GetComponentInChildren<TMP_Text>() : null);
			textMeshProUGUI.font = ((tmp_Text != null) ? LocalizedGameText.SourceFont(tmp_Text) : TMP_Settings.defaultFontAsset);
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = (bold ? FontStyles.Bold : FontStyles.Normal);
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.characterSpacing = (bold ? 0.6f : 0f);
			LocalizedGameText.BindSource(textMeshProUGUI, "tutorial", value, null).FitTranslationToRectangle(false);
			return textMeshProUGUI;
		}

		// Token: 0x06005756 RID: 22358 RVA: 0x00221278 File Offset: 0x0021F478
		private static RectTransform Rect(string name, Transform parent, float x, float y, float width, float height)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			Vector2 vector = new Vector2(0f, 1f);
			component.pivot = vector;
			component.anchorMin = (component.anchorMax = vector);
			component.anchoredPosition = new Vector2(x, -y);
			component.sizeDelta = new Vector2(width, height);
			return component;
		}

		// Token: 0x040057B3 RID: 22451
		private static readonly Color Pink = new Color(1f, 0.24f, 0.59f);

		// Token: 0x040057B4 RID: 22452
		private static readonly Color White = new Color(0.99f, 0.95f, 1f);

		// Token: 0x040057B5 RID: 22453
		private static readonly Color Muted = new Color(0.79f, 0.72f, 0.83f);

		// Token: 0x040057B6 RID: 22454
		private static readonly Color Surface = new Color(0.055f, 0.028f, 0.078f, 0.96f);
	}
}
