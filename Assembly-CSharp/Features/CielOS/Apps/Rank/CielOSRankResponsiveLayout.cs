using System;
using Features.CielOS.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Rank
{
	// Token: 0x02001462 RID: 5218
	public sealed class CielOSRankResponsiveLayout : MonoBehaviour
	{
		// Token: 0x0600A10C RID: 41228 RVA: 0x0042B16C File Offset: 0x0042936C
		public void Initialize()
		{
			if (this._scroll != null)
			{
				return;
			}
			VerticalLayoutGroup component = base.GetComponent<VerticalLayoutGroup>();
			if (component == null)
			{
				return;
			}
			Transform[] array = new Transform[base.transform.childCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = base.transform.GetChild(i);
			}
			RectTransform rectTransform = CielOSRankResponsiveLayout.NewRect("RankViewport", base.transform);
			CielOSRankResponsiveLayout.Stretch(rectTransform);
			rectTransform.offsetMax = new Vector2(-14f, 0f);
			rectTransform.gameObject.AddComponent<Image>().color = Color.clear;
			rectTransform.gameObject.AddComponent<RectMask2D>();
			this._content = CielOSRankResponsiveLayout.NewRect("RankScrollContent", rectTransform);
			this._content.anchorMin = new Vector2(0f, 1f);
			this._content.anchorMax = Vector2.one;
			this._content.pivot = new Vector2(0.5f, 1f);
			this._content.sizeDelta = Vector2.zero;
			VerticalLayoutGroup verticalLayoutGroup = this._content.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(component.padding.left, component.padding.right, component.padding.top, component.padding.bottom);
			verticalLayoutGroup.spacing = component.spacing;
			verticalLayoutGroup.childAlignment = component.childAlignment;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = component.childForceExpandHeight;
			component.enabled = false;
			Transform[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].SetParent(this._content, false);
			}
			this._scroll = base.gameObject.AddComponent<ScrollRect>();
			this._scroll.viewport = rectTransform;
			this._scroll.content = this._content;
			this._scroll.horizontal = false;
			this._scroll.vertical = true;
			this._scroll.movementType = ScrollRect.MovementType.Clamped;
			base.gameObject.AddComponent<CielOSSmoothWheelScroll>().Bind(this._scroll);
			RectTransform rectTransform2 = CielOSRankResponsiveLayout.NewRect("RankScrollbar", base.transform);
			rectTransform2.anchorMin = new Vector2(1f, 0f);
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.pivot = Vector2.one;
			rectTransform2.sizeDelta = new Vector2(8f, -24f);
			rectTransform2.anchoredPosition = new Vector2(-3f, -12f);
			rectTransform2.gameObject.AddComponent<Image>().color = new Color(0.17f, 0.11f, 0.22f, 0.6f);
			RectTransform rectTransform3 = CielOSRankResponsiveLayout.NewRect("Handle", rectTransform2);
			CielOSRankResponsiveLayout.Stretch(rectTransform3);
			Image image = rectTransform3.gameObject.AddComponent<Image>();
			image.color = new Color(0.66f, 0.38f, 0.81f, 0.85f);
			Scrollbar scrollbar = rectTransform2.gameObject.AddComponent<Scrollbar>();
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			scrollbar.handleRect = rectTransform3;
			scrollbar.targetGraphic = image;
			this._scroll.verticalScrollbar = scrollbar;
			this._scroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
			this.RefreshLayout();
		}

		// Token: 0x0600A10D RID: 41229 RVA: 0x0042B4AB File Offset: 0x004296AB
		private void LateUpdate()
		{
			this.RefreshLayout();
		}

		// Token: 0x0600A10E RID: 41230 RVA: 0x0042B4B4 File Offset: 0x004296B4
		public void RefreshLayout()
		{
			if (this._scroll == null || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			Vector2 size = this._scroll.viewport.rect.size;
			if (size != this._lastViewportSize)
			{
				this._lastViewportSize = size;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this._content);
			}
			float num = Mathf.Max(size.y, LayoutUtility.GetMinHeight(this._content));
			if (Mathf.Abs(this._content.rect.height - num) > 0.1f)
			{
				this._content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num);
				LayoutRebuilder.ForceRebuildLayoutImmediate(this._content);
			}
			Vector2 anchoredPosition = this._content.anchoredPosition;
			anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, 0f, Mathf.Max(0f, num - size.y));
			this._content.anchoredPosition = anchoredPosition;
		}

		// Token: 0x0600A10F RID: 41231 RVA: 0x0013F605 File Offset: 0x0013D805
		private static RectTransform NewRect(string name, Transform parent)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			return component;
		}

		// Token: 0x0600A110 RID: 41232 RVA: 0x0042B5A8 File Offset: 0x004297A8
		private static void Stretch(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = (rect.offsetMax = Vector2.zero);
		}

		// Token: 0x0400A4D1 RID: 42193
		private ScrollRect _scroll;

		// Token: 0x0400A4D2 RID: 42194
		private RectTransform _content;

		// Token: 0x0400A4D3 RID: 42195
		private Vector2 _lastViewportSize;
	}
}
