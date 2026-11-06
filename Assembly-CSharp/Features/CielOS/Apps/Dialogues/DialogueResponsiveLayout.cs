using System;
using Features.CielOS.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x020014AA RID: 5290
	[DisallowMultipleComponent]
	public sealed class DialogueResponsiveLayout : MonoBehaviour
	{
		// Token: 0x0600A2DE RID: 41694 RVA: 0x00439BA8 File Offset: 0x00437DA8
		public void Initialize()
		{
			if (this._initialized)
			{
				return;
			}
			this._initialized = true;
			this._root = (RectTransform)base.transform;
			Transform transform = base.transform.Find("MainView");
			HorizontalLayoutGroup horizontalLayoutGroup = ((transform != null) ? transform.GetComponent<HorizontalLayoutGroup>() : null);
			if (horizontalLayoutGroup != null)
			{
				horizontalLayoutGroup.childControlWidth = true;
			}
			Transform transform2 = base.transform.Find("MainView/Main");
			LayoutElement layoutElement = ((transform2 != null) ? transform2.GetComponent<LayoutElement>() : null);
			if (layoutElement != null)
			{
				layoutElement.minWidth = 0f;
				layoutElement.preferredWidth = 0f;
				layoutElement.flexibleWidth = 1f;
			}
			Transform transform3 = base.transform.Find("DetailView/Container");
			if (transform3 != null)
			{
				LayoutElement layoutElement2 = transform3.GetComponent<LayoutElement>() ?? transform3.gameObject.AddComponent<LayoutElement>();
				layoutElement2.minHeight = 0f;
				layoutElement2.preferredHeight = 0f;
				layoutElement2.flexibleHeight = 1f;
			}
			this.FitModal("ConfirmDialog");
			this.FitModal("LineDialog");
			this.MakeSidebarScrollable();
			this.RefreshLayout();
		}

		// Token: 0x0600A2DF RID: 41695 RVA: 0x00439CBC File Offset: 0x00437EBC
		private void MakeSidebarScrollable()
		{
			Transform transform = base.transform.Find("MainView/Sidebar");
			VerticalLayoutGroup verticalLayoutGroup = ((transform != null) ? transform.GetComponent<VerticalLayoutGroup>() : null);
			if (verticalLayoutGroup == null)
			{
				return;
			}
			Transform[] array = new Transform[transform.childCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = transform.GetChild(i);
			}
			RectTransform rectTransform = DialogueResponsiveLayout.NewRect("FilterViewport", transform);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = new Vector2(-10f, 0f);
			rectTransform.gameObject.AddComponent<RectMask2D>();
			rectTransform.gameObject.AddComponent<Image>().color = Color.clear;
			this._sidebarContent = DialogueResponsiveLayout.NewRect("FilterContent", rectTransform);
			this._sidebarContent.anchorMin = Vector2.up;
			this._sidebarContent.anchorMax = Vector2.one;
			this._sidebarContent.pivot = new Vector2(0.5f, 1f);
			this._sidebarContent.sizeDelta = Vector2.zero;
			VerticalLayoutGroup verticalLayoutGroup2 = this._sidebarContent.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup2.padding = new RectOffset(verticalLayoutGroup.padding.left, verticalLayoutGroup.padding.right, verticalLayoutGroup.padding.top, verticalLayoutGroup.padding.bottom);
			verticalLayoutGroup2.spacing = verticalLayoutGroup.spacing;
			verticalLayoutGroup2.childAlignment = verticalLayoutGroup.childAlignment;
			verticalLayoutGroup2.childControlWidth = (verticalLayoutGroup2.childControlHeight = true);
			verticalLayoutGroup2.childForceExpandWidth = true;
			verticalLayoutGroup2.childForceExpandHeight = false;
			verticalLayoutGroup.enabled = false;
			Transform[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].SetParent(this._sidebarContent, false);
			}
			this._sidebarScroll = transform.gameObject.AddComponent<ScrollRect>();
			this._sidebarScroll.content = this._sidebarContent;
			this._sidebarScroll.viewport = rectTransform;
			this._sidebarScroll.horizontal = false;
			this._sidebarScroll.movementType = ScrollRect.MovementType.Clamped;
			transform.gameObject.AddComponent<CielOSSmoothWheelScroll>().Bind(this._sidebarScroll);
			RectTransform rectTransform2 = DialogueResponsiveLayout.NewRect("FilterScrollbar", transform);
			rectTransform2.anchorMin = Vector2.right;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.pivot = Vector2.one;
			rectTransform2.offsetMin = new Vector2(-7f, 16f);
			rectTransform2.offsetMax = new Vector2(-2f, -16f);
			RectTransform rectTransform3 = DialogueResponsiveLayout.NewRect("Handle", rectTransform2);
			rectTransform3.anchorMin = Vector2.zero;
			rectTransform3.anchorMax = Vector2.one;
			rectTransform3.offsetMin = (rectTransform3.offsetMax = Vector2.zero);
			Image image = rectTransform3.gameObject.AddComponent<Image>();
			image.color = new Color(0.66f, 0.38f, 0.81f, 0.85f);
			Scrollbar scrollbar = rectTransform2.gameObject.AddComponent<Scrollbar>();
			scrollbar.handleRect = rectTransform3;
			scrollbar.targetGraphic = image;
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			this._sidebarScroll.verticalScrollbar = scrollbar;
			this._sidebarScroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
		}

		// Token: 0x0600A2E0 RID: 41696 RVA: 0x0013F605 File Offset: 0x0013D805
		private static RectTransform NewRect(string name, Transform parent)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			return component;
		}

		// Token: 0x0600A2E1 RID: 41697 RVA: 0x00439FF4 File Offset: 0x004381F4
		private void RefreshSidebar()
		{
			if (this._sidebarContent == null || !this._sidebarContent.gameObject.activeInHierarchy)
			{
				return;
			}
			this._lastSidebarSize = this._sidebarScroll.viewport.rect.size;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this._sidebarContent);
			foreach (GridLayoutGroup gridLayoutGroup in this._sidebarContent.GetComponentsInChildren<GridLayoutGroup>(true))
			{
				int num = Mathf.Max(1, gridLayoutGroup.constraintCount);
				float num2 = ((RectTransform)gridLayoutGroup.transform).rect.width - (float)gridLayoutGroup.padding.horizontal;
				gridLayoutGroup.cellSize = new Vector2(Mathf.Max(1f, (num2 - gridLayoutGroup.spacing.x * (float)(num - 1)) / (float)num), gridLayoutGroup.cellSize.y);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this._sidebarContent);
			this._sidebarContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(this._sidebarScroll.viewport.rect.height, LayoutUtility.GetMinHeight(this._sidebarContent)));
			LayoutRebuilder.ForceRebuildLayoutImmediate(this._sidebarContent);
			Vector2 anchoredPosition = this._sidebarContent.anchoredPosition;
			anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, 0f, Mathf.Max(0f, this._sidebarContent.rect.height - this._sidebarScroll.viewport.rect.height));
			this._sidebarContent.anchoredPosition = anchoredPosition;
		}

		// Token: 0x0600A2E2 RID: 41698 RVA: 0x0043A190 File Offset: 0x00438390
		private void FitModal(string path)
		{
			RectTransform rectTransform = base.transform.Find(path) as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = (rectTransform.offsetMax = Vector2.zero);
			RectTransform rectTransform2 = rectTransform.Find("Card") as RectTransform;
			if (rectTransform2 == null)
			{
				return;
			}
			rectTransform2.anchorMin = (rectTransform2.anchorMax = (rectTransform2.pivot = Vector2.one * 0.5f));
			rectTransform2.anchoredPosition = Vector2.zero;
		}

		// Token: 0x0600A2E3 RID: 41699 RVA: 0x0043A230 File Offset: 0x00438430
		private void LateUpdate()
		{
			this.RefreshLayout();
		}

		// Token: 0x0600A2E4 RID: 41700 RVA: 0x0043A238 File Offset: 0x00438438
		public void RefreshLayout()
		{
			if (!this._initialized)
			{
				this.Initialize();
			}
			RectTransform rectTransform = this._root.parent as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Vector2 size = rectTransform.rect.size;
			if (size.x <= 0f || size.y <= 0f)
			{
				return;
			}
			float num = Mathf.Min(new float[]
			{
				1f,
				size.x / DialogueResponsiveLayout.MinimumWorkspace.x,
				size.y / DialogueResponsiveLayout.MinimumWorkspace.y
			});
			Vector2 vector = size / num;
			if (!(this._lastSize != size) && !(this._root.sizeDelta != vector) && Mathf.Approximately(this._root.localScale.x, num))
			{
				if (this._sidebarContent != null && this._sidebarContent.gameObject.activeInHierarchy && this._lastSidebarSize != this._sidebarScroll.viewport.rect.size)
				{
					this.RefreshSidebar();
				}
				return;
			}
			this._lastSize = size;
			this._root.anchorMin = (this._root.anchorMax = (this._root.pivot = Vector2.one * 0.5f));
			this._root.anchoredPosition = Vector2.zero;
			this._root.sizeDelta = vector;
			this._root.localScale = new Vector3(num, num, 1f);
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.activeInHierarchy)
				{
					RectTransform rectTransform2 = transform as RectTransform;
					if (rectTransform2 != null)
					{
						LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform2);
					}
				}
			}
			this.RefreshSidebar();
		}

		// Token: 0x0400A698 RID: 42648
		private static readonly Vector2 MinimumWorkspace = new Vector2(1280f, 900f);

		// Token: 0x0400A699 RID: 42649
		private RectTransform _root;

		// Token: 0x0400A69A RID: 42650
		private Vector2 _lastSize;

		// Token: 0x0400A69B RID: 42651
		private bool _initialized;

		// Token: 0x0400A69C RID: 42652
		private RectTransform _sidebarContent;

		// Token: 0x0400A69D RID: 42653
		private ScrollRect _sidebarScroll;

		// Token: 0x0400A69E RID: 42654
		private Vector2 _lastSidebarSize;
	}
}
