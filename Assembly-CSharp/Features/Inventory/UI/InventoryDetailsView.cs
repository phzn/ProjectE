using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Inventory.UI
{
	// Token: 0x02001155 RID: 4437
	public static class InventoryDetailsView
	{
		// Token: 0x060086F8 RID: 34552 RVA: 0x00372BE0 File Offset: 0x00370DE0
		public static ScrollRect Create(TextMeshProUGUI title, TextMeshProUGUI description)
		{
			if (title == null || description == null || title.transform.parent != description.transform.parent)
			{
				return null;
			}
			RectTransform rectTransform = title.transform.parent as RectTransform;
			if (rectTransform == null)
			{
				return null;
			}
			LayoutGroup component = rectTransform.GetComponent<LayoutGroup>();
			if (component != null)
			{
				component.enabled = false;
			}
			RectTransform rectTransform2 = title.rectTransform;
			rectTransform2.anchorMin = new Vector2(0f, 1f);
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.sizeDelta = new Vector2(0f, 50f);
			title.enableAutoSizing = true;
			title.fontSizeMin = 24f;
			title.fontSizeMax = 34f;
			title.enableWordWrapping = false;
			title.overflowMode = TextOverflowModes.Ellipsis;
			GameObject gameObject = new GameObject("ItemDetails", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(RectMask2D),
				typeof(ScrollRect)
			});
			gameObject.layer = rectTransform.gameObject.layer;
			RectTransform rectTransform3 = (RectTransform)gameObject.transform;
			rectTransform3.SetParent(rectTransform, false);
			rectTransform3.anchorMin = Vector2.zero;
			rectTransform3.anchorMax = Vector2.one;
			rectTransform3.offsetMin = Vector2.zero;
			rectTransform3.offsetMax = new Vector2(0f, -58f);
			gameObject.GetComponent<Image>().color = Color.clear;
			RectTransform rectTransform4 = description.rectTransform;
			rectTransform4.SetParent(rectTransform3, false);
			rectTransform4.anchorMin = new Vector2(0f, 1f);
			rectTransform4.anchorMax = Vector2.one;
			rectTransform4.pivot = new Vector2(0f, 1f);
			rectTransform4.anchoredPosition = Vector2.zero;
			rectTransform4.sizeDelta = new Vector2(-14f, 0f);
			LayoutElement component2 = rectTransform4.GetComponent<LayoutElement>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
			ContentSizeFitter contentSizeFitter = rectTransform4.GetComponent<ContentSizeFitter>();
			if (contentSizeFitter == null)
			{
				contentSizeFitter = rectTransform4.gameObject.AddComponent<ContentSizeFitter>();
			}
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			description.fontSize = 21f;
			description.enableAutoSizing = false;
			description.enableWordWrapping = true;
			description.richText = true;
			description.alignment = TextAlignmentOptions.TopLeft;
			description.overflowMode = TextOverflowModes.Overflow;
			description.lineSpacing = 3f;
			description.raycastTarget = false;
			ScrollRect component3 = gameObject.GetComponent<ScrollRect>();
			component3.viewport = rectTransform3;
			component3.content = rectTransform4;
			component3.horizontal = false;
			component3.vertical = true;
			component3.movementType = ScrollRect.MovementType.Clamped;
			component3.scrollSensitivity = 32f;
			GameObject gameObject2 = new GameObject("DetailsScrollbar", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Scrollbar)
			});
			gameObject2.layer = rectTransform.gameObject.layer;
			RectTransform rectTransform5 = (RectTransform)gameObject2.transform;
			rectTransform5.SetParent(rectTransform, false);
			rectTransform5.anchorMin = new Vector2(1f, 0f);
			rectTransform5.anchorMax = Vector2.one;
			rectTransform5.pivot = new Vector2(1f, 0.5f);
			rectTransform5.offsetMin = new Vector2(-4f, 0f);
			rectTransform5.offsetMax = new Vector2(0f, -58f);
			gameObject2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.08f);
			GameObject gameObject3 = new GameObject("Handle", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject3.layer = rectTransform.gameObject.layer;
			RectTransform rectTransform6 = (RectTransform)gameObject3.transform;
			rectTransform6.SetParent(rectTransform5, false);
			rectTransform6.anchorMin = Vector2.zero;
			rectTransform6.anchorMax = Vector2.one;
			rectTransform6.offsetMin = (rectTransform6.offsetMax = Vector2.zero);
			Image component4 = gameObject3.GetComponent<Image>();
			component4.color = new Color(1f, 0.78f, 0.86f, 0.65f);
			Scrollbar component5 = gameObject2.GetComponent<Scrollbar>();
			component5.handleRect = rectTransform6;
			component5.targetGraphic = component4;
			component5.direction = Scrollbar.Direction.BottomToTop;
			component3.verticalScrollbar = component5;
			component3.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
			return component3;
		}
	}
}
