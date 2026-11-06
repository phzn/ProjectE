using System;
using DA_Assets.CR;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.Tutorials
{
	// Token: 0x02000BDD RID: 3037
	public static class TutorialIntroductionPage
	{
		// Token: 0x06005765 RID: 22373 RVA: 0x00221B6C File Offset: 0x0021FD6C
		public static void Build(RectTransform host, TMP_FontAsset font, UnityAction begin = null)
		{
			RectTransform rectTransform = TutorialIntroductionPage.Rect(host, "Welcome Background", 0f, 0f, 1280f, 720f);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = (rectTransform.offsetMax = Vector2.zero);
			RawImage rawImage = rectTransform.gameObject.AddComponent<RawImage>();
			rawImage.texture = Resources.Load<Texture2D>("Tutorials/Art/project_exhibitionist_background");
			rawImage.color = new Color(0.43f, 0.43f, 0.43f, 1f);
			rawImage.raycastTarget = true;
			RectTransform rectTransform2 = TutorialIntroductionPage.Rect(host, "Welcome Composition", 0f, 0f, 1280f, 720f);
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			RectTransform rectTransform5 = rectTransform2;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform5.pivot = vector;
			rectTransform3.anchorMin = (rectTransform4.anchorMax = vector);
			rectTransform2.anchoredPosition = Vector2.zero;
			(host.GetComponent<TutorialMediaFit>() ?? host.gameObject.AddComponent<TutorialMediaFit>()).Configure(rectTransform2);
			TutorialIntroductionPage.Label(rectTransform2, font, "Welcome Kicker", "MAKE YOURSELF AT HOME", 50f, 34f, 650f, 18f, 8f, TutorialIntroductionPage.Pink, true, 1.2f);
			TutorialIntroductionPage.Label(rectTransform2, font, "Welcome Title", "WELCOME TO\nPROJECT EXHIBITIONIST", 50f, 64f, 1160f, 88f, 34f, TutorialIntroductionPage.White, false, 2.5f).lineSpacing = 8f;
			TutorialIntroductionPage.Label(rectTransform2, font, "Welcome Summary", "Project Exhibitionist is a game about exhibitionism, with plenty of self-humiliation along the way.", 50f, 169f, 1160f, 38f, 14f, TutorialIntroductionPage.Muted, false, 0f);
			TutorialIntroductionPage.Label(rectTransform2, font, "Map Heading", "Procedural map & Ranked", 50f, 248f, 620f, 30f, 18f, TutorialIntroductionPage.White, true, 1.5f);
			TutorialIntroductionPage.Label(rectTransform2, font, "Map Description", "Complete the main story to fully unlock the procedurally generated map. Then explore freely or compete in Ranked mode.", 50f, 293f, 615f, 72f, 14f, TutorialIntroductionPage.Muted, false, 0f);
			TutorialIntroductionPage.Panel(rectTransform2, "First Run Card", 710f, 248f, 520f, 352f, TutorialIntroductionPage.Card, 10f);
			TutorialIntroductionPage.Panel(rectTransform2, "First Run Accent", 711f, 259f, 2f, 330f, TutorialIntroductionPage.Pink, 0f);
			TutorialIntroductionPage.Label(rectTransform2, font, "First Run Kicker", "YOUR FIRST RUN STARTS HERE", 730f, 264f, 475f, 18f, 8f, TutorialIntroductionPage.Pink, true, 0.6f);
			TutorialIntroductionPage.Label(rectTransform2, font, "First Run Heading", "It all begins at the PC.", 730f, 290f, 475f, 32f, 18f, TutorialIntroductionPage.White, true, 1.5f);
			TutorialIntroductionPage.Step(rectTransform2, font, "01", "Walk to the desk and press E to use the PC.", 338f);
			TutorialIntroductionPage.Step(rectTransform2, font, "02", "Open Story and read the highlighted chapter.", 404f);
			TutorialIntroductionPage.Step(rectTransform2, font, "03", "Open Run Modes, choose Classic, then Continue Classic.", 470f);
			TutorialIntroductionPage.Label(rectTransform2, font, "Thanks Kicker", "THANK YOU FOR BEING HERE", 50f, 620f, 820f, 18f, 8f, TutorialIntroductionPage.Pink, true, 0.8f);
			TutorialIntroductionPage.Label(rectTransform2, font, "Thanks Message", "Thank you for purchasing Project Exhibitionist. We're truly grateful for your support!", 50f, 642f, 820f, 30f, 11f, TutorialIntroductionPage.Muted, false, 0f);
			Image image = TutorialIntroductionPage.Panel(rectTransform2, "Welcome Begin", 923f, 631f, 307f, 41f, TutorialIntroductionPage.Pink, 3f);
			if (begin != null)
			{
				image.raycastTarget = true;
				Button button = image.gameObject.AddComponent<Button>();
				button.targetGraphic = image;
				ColorBlock colors = button.colors;
				colors.highlightedColor = new Color(1f, 0.82f, 0.92f, 1f);
				colors.pressedColor = new Color(0.82f, 0.7f, 0.77f, 1f);
				colors.selectedColor = colors.highlightedColor;
				button.colors = colors;
				button.onClick.AddListener(begin);
			}
			TutorialIntroductionPage.Label(image.rectTransform, font, "Welcome Begin Label", "LET'S BEGIN", 0f, 0f, 307f, 41f, 10f, TutorialIntroductionPage.Card, true, 0.8f).alignment = TextAlignmentOptions.Center;
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x00221FEC File Offset: 0x002201EC
		private static void Step(RectTransform board, TMP_FontAsset font, string number, string instruction, float y)
		{
			TutorialIntroductionPage.Label(board, font, "Step " + number, number, 730f, y, 30f, 24f, 15f, TutorialIntroductionPage.Pink, true, 0f);
			TutorialIntroductionPage.Label(board, font, "Step " + number + " Text", instruction, 766f, y, 437f, 42f, 14f, TutorialIntroductionPage.White, false, 0f);
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x00222068 File Offset: 0x00220268
		private static TMP_Text Label(RectTransform parent, TMP_FontAsset font, string name, string value, float x, float y, float width, float height, float size, Color color, bool bold = false, float spacing = 0f)
		{
			TextMeshProUGUI textMeshProUGUI = TutorialIntroductionPage.Rect(parent, name, x, y, width, height).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.font = ((font != null) ? font : TMP_Settings.defaultFontAsset);
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = (bold ? FontStyles.Bold : FontStyles.Normal);
			textMeshProUGUI.characterSpacing = spacing;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = TextAlignmentOptions.TopLeft;
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
			textMeshProUGUI.raycastTarget = false;
			LocalizedGameText.BindSource(textMeshProUGUI, "tutorial", value, null).FitTranslationToRectangle(false);
			return textMeshProUGUI;
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x00222100 File Offset: 0x00220300
		private static Image Panel(RectTransform parent, string name, float x, float y, float width, float height, Color color, float radius = 0f)
		{
			Image image = TutorialIntroductionPage.Rect(parent, name, x, y, width, height).gameObject.AddComponent<Image>();
			image.color = color;
			image.raycastTarget = false;
			if (radius > 0f)
			{
				image.gameObject.AddComponent<CornerRounder>().SetRadii(Vector4.one * radius);
			}
			return image;
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x0022215C File Offset: 0x0022035C
		private static RectTransform Rect(Transform parent, string name, float x, float y, float width, float height)
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

		// Token: 0x040057CF RID: 22479
		public const string Summary = "Project Exhibitionist is a game about exhibitionism, with plenty of self-humiliation along the way.";

		// Token: 0x040057D0 RID: 22480
		public const string MapDescription = "Complete the main story to fully unlock the procedurally generated map. Then explore freely or compete in Ranked mode.";

		// Token: 0x040057D1 RID: 22481
		public const string FirstStep = "Walk to the desk and press E to use the PC.";

		// Token: 0x040057D2 RID: 22482
		public const string SecondStep = "Open Story and read the highlighted chapter.";

		// Token: 0x040057D3 RID: 22483
		public const string ThirdStep = "Open Run Modes, choose Classic, then Continue Classic.";

		// Token: 0x040057D4 RID: 22484
		private static readonly Color Pink = new Color(1f, 0.25f, 0.59f, 1f);

		// Token: 0x040057D5 RID: 22485
		private static readonly Color White = new Color(0.98f, 0.97f, 0.98f, 1f);

		// Token: 0x040057D6 RID: 22486
		private static readonly Color Muted = new Color(0.78f, 0.74f, 0.79f, 1f);

		// Token: 0x040057D7 RID: 22487
		private static readonly Color Card = new Color(0.085f, 0.081f, 0.097f, 1f);
	}
}
