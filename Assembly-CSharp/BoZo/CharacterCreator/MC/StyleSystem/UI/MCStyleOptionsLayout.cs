using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoZo.CharacterCreator.MC.StyleSystem.UI
{
	// Token: 0x02000815 RID: 2069
	internal static class MCStyleOptionsLayout
	{
		// Token: 0x06003739 RID: 14137 RVA: 0x0013F144 File Offset: 0x0013D344
		public static ScrollRect MakeScrollable(RectTransform content, Transform panel)
		{
			if (content == null || panel == null)
			{
				return null;
			}
			Transform transform = panel.Find("OptionsScroll");
			if (transform != null)
			{
				return transform.GetComponent<ScrollRect>();
			}
			VerticalLayoutGroup component = panel.GetComponent<VerticalLayoutGroup>();
			if (component == null)
			{
				return null;
			}
			component.childControlHeight = true;
			component.childControlWidth = true;
			component.childForceExpandHeight = false;
			component.childForceExpandWidth = true;
			foreach (object obj in panel)
			{
				Transform transform2 = (Transform)obj;
				if (!(transform2 == content))
				{
					LayoutElement layoutElement = transform2.GetComponent<LayoutElement>() ?? transform2.gameObject.AddComponent<LayoutElement>();
					layoutElement.minHeight = 48f;
					layoutElement.preferredHeight = 48f;
					layoutElement.flexibleHeight = 0f;
				}
			}
			RectTransform rectTransform = MCStyleOptionsLayout.Rect("OptionsScroll", panel);
			LayoutElement layoutElement2 = rectTransform.gameObject.AddComponent<LayoutElement>();
			layoutElement2.minHeight = 80f;
			layoutElement2.preferredHeight = 0f;
			layoutElement2.flexibleHeight = 1f;
			ScrollRect scrollRect = rectTransform.gameObject.AddComponent<ScrollRect>();
			RectTransform rectTransform2 = MCStyleOptionsLayout.Rect("Viewport", rectTransform);
			MCStyleOptionsLayout.Stretch(rectTransform2, Vector2.zero, new Vector2(-12f, 0f));
			rectTransform2.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.001f);
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			content.SetParent(rectTransform2, false);
			content.anchorMin = new Vector2(0f, 1f);
			content.anchorMax = Vector2.one;
			content.pivot = new Vector2(0.5f, 1f);
			content.anchoredPosition = Vector2.zero;
			content.sizeDelta = Vector2.zero;
			VerticalLayoutGroup verticalLayoutGroup = content.GetComponent<VerticalLayoutGroup>() ?? content.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			verticalLayoutGroup.padding.bottom = Mathf.Max(20, verticalLayoutGroup.padding.bottom);
			ContentSizeFitter contentSizeFitter = content.GetComponent<ContentSizeFitter>() ?? content.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			RectTransform rectTransform3 = MCStyleOptionsLayout.Rect("Scrollbar", rectTransform);
			rectTransform3.anchorMin = new Vector2(1f, 0f);
			rectTransform3.anchorMax = Vector2.one;
			rectTransform3.pivot = Vector2.one;
			rectTransform3.offsetMin = new Vector2(-8f, 8f);
			rectTransform3.offsetMax = new Vector2(-2f, -8f);
			rectTransform3.gameObject.AddComponent<Image>().color = new Color(0.15f, 0.09f, 0.2f, 0.45f);
			RectTransform rectTransform4 = MCStyleOptionsLayout.Rect("Handle", rectTransform3);
			MCStyleOptionsLayout.Stretch(rectTransform4, Vector2.zero, Vector2.zero);
			Image image = rectTransform4.gameObject.AddComponent<Image>();
			image.color = new Color(0.67f, 0.38f, 0.79f, 0.85f);
			Scrollbar scrollbar = rectTransform3.gameObject.AddComponent<Scrollbar>();
			scrollbar.handleRect = rectTransform4;
			scrollbar.targetGraphic = image;
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			scrollRect.viewport = rectTransform2;
			scrollRect.content = content;
			scrollRect.horizontal = false;
			scrollRect.vertical = true;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			scrollRect.scrollSensitivity = 28f;
			scrollRect.verticalScrollbar = scrollbar;
			scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
			return scrollRect;
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x0013F4EC File Offset: 0x0013D6EC
		public static void FitSliderRow(MCStyleShaderSliderRowUI row)
		{
			RectTransform rectTransform = row.transform.Find("Text (TMP)") as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			TMP_Text component = rectTransform.GetComponent<TMP_Text>();
			if (component != null)
			{
				component.margin = new Vector4(0f, 0f, 46f, 0f);
				component.enableAutoSizing = true;
				component.fontSizeMin = 12f;
				component.fontSizeMax = 17f;
				component.enableWordWrapping = false;
				component.overflowMode = TextOverflowModes.Ellipsis;
			}
			RectTransform rectTransform2 = rectTransform.Find("Text (TMP)_1") as RectTransform;
			if (rectTransform2 == null)
			{
				return;
			}
			rectTransform2.anchorMin = new Vector2(1f, 0f);
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.pivot = new Vector2(1f, 0.5f);
			rectTransform2.offsetMin = new Vector2(-42f, 0f);
			rectTransform2.offsetMax = Vector2.zero;
			TMP_Text component2 = rectTransform2.GetComponent<TMP_Text>();
			if (component2 != null)
			{
				component2.alignment = TextAlignmentOptions.MidlineRight;
				component2.raycastTarget = false;
			}
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x0013F605 File Offset: 0x0013D805
		private static RectTransform Rect(string name, Transform parent)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			return component;
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x0013F62D File Offset: 0x0013D82D
		private static void Stretch(RectTransform rect, Vector2 minimum, Vector2 maximum)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = minimum;
			rect.offsetMax = maximum;
		}
	}
}
