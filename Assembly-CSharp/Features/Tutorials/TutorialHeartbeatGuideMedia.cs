using System;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Tutorials
{
	// Token: 0x02000BD5 RID: 3029
	public static class TutorialHeartbeatGuideMedia
	{
		// Token: 0x06005747 RID: 22343 RVA: 0x002207C4 File Offset: 0x0021E9C4
		public static void Build(RectTransform board)
		{
			Texture2D texture2D = Resources.Load<Texture2D>("Tutorials/Art/heartbeat_native");
			Texture2D texture2D2 = Resources.Load<Texture2D>("Tutorials/Art/climax_meter_native");
			TextAsset textAsset = Resources.Load<TextAsset>("Tutorials/Art/hud_guide_capture");
			TutorialHeartbeatGuideMedia.CaptureGeometry captureGeometry = ((textAsset != null) ? JsonUtility.FromJson<TutorialHeartbeatGuideMedia.CaptureGeometry>(textAsset.text) : null);
			TutorialHeartbeatGuideMedia.Label(board, "Heading", "READ THE PULSE. WATCH THE BUILD-UP.", 64f, 112f, 1152f, 65f, 39f, TutorialHeartbeatGuideMedia.White, true);
			TutorialHeartbeatGuideMedia.Panel(board, "Heart Panel", 64f, 208f, 1152f, 150f, TutorialHeartbeatGuideMedia.Surface);
			TutorialHeartbeatGuideMedia.Panel(board, "Meter Panel", 64f, 382f, 1152f, 226f, TutorialHeartbeatGuideMedia.Surface);
			float num = ((texture2D2 != null) ? (1088f * (float)texture2D2.height / (float)texture2D2.width) : 100f);
			float num2 = num * TutorialHeartbeatGuideMedia.VisibleFraction((captureGeometry != null) ? captureGeometry.bar : null) / TutorialHeartbeatGuideMedia.VisibleFraction((captureGeometry != null) ? captureGeometry.heart : null);
			float num3 = ((texture2D != null) ? (num2 * (float)texture2D.width / (float)texture2D.height) : num2);
			TutorialHeartbeatGuideMedia.Photograph(board, "Production Heartbeat HUD", texture2D, 164f - num3 * 0.5f, 283f - num2 * 0.5f, num3, num2);
			TutorialHeartbeatGuideMedia.Photograph(board, "Production Climax Meter HUD", texture2D2, 96f, 540f - num * 0.5f, 1088f, num);
			TutorialHeartbeatGuideMedia.Label(board, "Heart Label", "HEARTBEAT", 264f, 233f, 890f, 40f, 28f, TutorialHeartbeatGuideMedia.Pink, true);
			TutorialHeartbeatGuideMedia.Label(board, "Heart Hint", "Current intensity. Running and attention raise it;\nslowing down lets it settle.", 264f, 284f, 890f, 64f, 24f, TutorialHeartbeatGuideMedia.Muted, false);
			TutorialHeartbeatGuideMedia.Label(board, "Meter Label", "CLIMAX METER", 96f, 406f, 330f, 40f, 28f, TutorialHeartbeatGuideMedia.Pink, true);
			TutorialHeartbeatGuideMedia.Label(board, "Meter Hint", "Build-up during stimulating actions.\nFollow its fill and let it cool by stopping.", 450f, 404f, 728f, 64f, 23f, TutorialHeartbeatGuideMedia.Muted, false);
			TutorialHeartbeatGuideMedia.Panel(board, "Footer Rule", 64f, 642f, 1152f, 1f, new Color(0.65f, 0.47f, 0.68f, 0.3f));
			TutorialHeartbeatGuideMedia.Label(board, "Artwork Footer", "TWO DIFFERENT SIGNALS  /  INTENSITY AND BUILD-UP", 64f, 660f, 1152f, 33f, 17f, TutorialHeartbeatGuideMedia.Muted, false);
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x00220A6C File Offset: 0x0021EC6C
		private static float VisibleFraction(TutorialHeartbeatGuideMedia.WidgetGeometry geometry)
		{
			if (geometry == null || geometry.alphaHeightFraction <= 0f)
			{
				return 1f;
			}
			return geometry.alphaHeightFraction;
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x00220A8C File Offset: 0x0021EC8C
		private static void Photograph(RectTransform parent, string name, Texture2D texture, float x, float y, float width, float height)
		{
			RawImage rawImage = TutorialHeartbeatGuideMedia.Rect(name, parent, x, y, width, height).gameObject.AddComponent<RawImage>();
			rawImage.texture = texture;
			rawImage.color = ((texture != null) ? Color.white : Color.clear);
			rawImage.raycastTarget = false;
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x00220AD9 File Offset: 0x0021ECD9
		private static void Panel(RectTransform parent, string name, float x, float y, float width, float height, Color color)
		{
			Image image = TutorialHeartbeatGuideMedia.Rect(name, parent, x, y, width, height).gameObject.AddComponent<Image>();
			image.color = color;
			image.raycastTarget = false;
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x00220B00 File Offset: 0x0021ED00
		private static TMP_Text Label(RectTransform parent, string name, string value, float x, float y, float width, float height, float size, Color color, bool bold = false)
		{
			TextMeshProUGUI textMeshProUGUI = TutorialHeartbeatGuideMedia.Rect(name, parent, x, y, width, height).gameObject.AddComponent<TextMeshProUGUI>();
			PauseMenuTutorialPanel componentInParent = parent.GetComponentInParent<PauseMenuTutorialPanel>();
			TMP_Text tmp_Text = ((componentInParent != null) ? componentInParent.GetComponentInChildren<TMP_Text>() : null);
			textMeshProUGUI.font = ((tmp_Text != null) ? LocalizedGameText.SourceFont(tmp_Text) : TMP_Settings.defaultFontAsset);
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = (bold ? FontStyles.Bold : FontStyles.Normal);
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = TextAlignmentOptions.TopLeft;
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.characterSpacing = (bold ? 0.6f : 0f);
			LocalizedGameText.BindSource(textMeshProUGUI, "tutorial", value, null).FitTranslationToRectangle(false);
			return textMeshProUGUI;
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x00220BBC File Offset: 0x0021EDBC
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

		// Token: 0x040057AC RID: 22444
		private static readonly Color Pink = new Color(1f, 0.24f, 0.59f);

		// Token: 0x040057AD RID: 22445
		private static readonly Color White = new Color(0.99f, 0.95f, 1f);

		// Token: 0x040057AE RID: 22446
		private static readonly Color Muted = new Color(0.79f, 0.72f, 0.83f);

		// Token: 0x040057AF RID: 22447
		private static readonly Color Surface = new Color(0.055f, 0.028f, 0.078f, 0.96f);

		// Token: 0x02000BD6 RID: 3030
		[Serializable]
		private sealed class CaptureGeometry
		{
			// Token: 0x040057B0 RID: 22448
			public TutorialHeartbeatGuideMedia.WidgetGeometry heart;

			// Token: 0x040057B1 RID: 22449
			public TutorialHeartbeatGuideMedia.WidgetGeometry bar;
		}

		// Token: 0x02000BD7 RID: 3031
		[Serializable]
		private sealed class WidgetGeometry
		{
			// Token: 0x040057B2 RID: 22450
			public float alphaHeightFraction;
		}
	}
}
