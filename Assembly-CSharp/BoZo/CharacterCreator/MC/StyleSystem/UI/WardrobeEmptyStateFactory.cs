using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoZo.CharacterCreator.MC.StyleSystem.UI
{
	// Token: 0x02000816 RID: 2070
	internal static class WardrobeEmptyStateFactory
	{
		// Token: 0x0600373D RID: 14141 RVA: 0x0013F654 File Offset: 0x0013D854
		public static GameObject Create(Transform cardContainer, out TMP_Text titleLabel, out TMP_Text messageLabel)
		{
			titleLabel = null;
			messageLabel = null;
			if (cardContainer == null)
			{
				return null;
			}
			Transform transform = ((cardContainer.parent != null) ? cardContainer.parent : cardContainer);
			GameObject gameObject = new GameObject("Wardrobe Empty State", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup),
				typeof(VerticalLayoutGroup)
			});
			gameObject.transform.SetParent(transform, false);
			gameObject.transform.SetAsLastSibling();
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.pivot = new Vector2(0.5f, 0.5f);
			component.offsetMin = new Vector2(24f, 24f);
			component.offsetMax = new Vector2(-24f, -24f);
			CanvasGroup component2 = gameObject.GetComponent<CanvasGroup>();
			component2.interactable = false;
			component2.blocksRaycasts = false;
			VerticalLayoutGroup component3 = gameObject.GetComponent<VerticalLayoutGroup>();
			component3.padding = new RectOffset(12, 12, 48, 48);
			component3.spacing = 12f;
			component3.childAlignment = TextAnchor.MiddleCenter;
			component3.childControlWidth = true;
			component3.childControlHeight = true;
			component3.childForceExpandWidth = false;
			component3.childForceExpandHeight = false;
			GameObject gameObject2 = new GameObject("Accent", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(LayoutElement)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			gameObject2.GetComponent<Image>().color = WardrobeEmptyStateFactory.AccentColor;
			LayoutElement component4 = gameObject2.GetComponent<LayoutElement>();
			component4.preferredWidth = 52f;
			component4.preferredHeight = 5f;
			titleLabel = WardrobeEmptyStateFactory.CreateText(gameObject.transform, "Title", 26f, FontStyles.Bold, WardrobeEmptyStateFactory.TitleColor, 380f, 44f);
			messageLabel = WardrobeEmptyStateFactory.CreateText(gameObject.transform, "Message", 18f, FontStyles.Normal, WardrobeEmptyStateFactory.MessageColor, 380f, 64f);
			messageLabel.enableWordWrapping = true;
			return gameObject;
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x0013F864 File Offset: 0x0013DA64
		private static TMP_Text CreateText(Transform parent, string name, float fontSize, FontStyles fontStyle, Color color, float width, float height)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.alignment = TextAlignmentOptions.Center;
			component.fontSize = fontSize;
			component.fontStyle = fontStyle;
			component.color = color;
			component.raycastTarget = false;
			component.overflowMode = TextOverflowModes.Overflow;
			LayoutElement component2 = gameObject.GetComponent<LayoutElement>();
			component2.preferredWidth = width;
			component2.preferredHeight = height;
			return component;
		}

		// Token: 0x04003755 RID: 14165
		private static readonly Color AccentColor = new Color32(235, 83, 123, byte.MaxValue);

		// Token: 0x04003756 RID: 14166
		private static readonly Color TitleColor = new Color(1f, 1f, 1f, 0.94f);

		// Token: 0x04003757 RID: 14167
		private static readonly Color MessageColor = new Color(1f, 1f, 1f, 0.62f);
	}
}
