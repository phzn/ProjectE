using System;
using Features.CielOS.UI;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS.Desktop
{
	// Token: 0x020013E7 RID: 5095
	public class CielOSDesktopIcon : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, ISubmitHandler, ISelectHandler, IDeselectHandler
	{
		// Token: 0x170018D9 RID: 6361
		// (get) Token: 0x06009D3E RID: 40254 RVA: 0x004096F7 File Offset: 0x004078F7
		public string AppId
		{
			get
			{
				return this._appId;
			}
		}

		// Token: 0x170018DA RID: 6362
		// (get) Token: 0x06009D3F RID: 40255 RVA: 0x00409700 File Offset: 0x00407900
		public RectTransform Rect
		{
			get
			{
				if (!(this._rect != null))
				{
					return this._rect = (RectTransform)base.transform;
				}
				return this._rect;
			}
		}

		// Token: 0x06009D40 RID: 40256 RVA: 0x00409738 File Offset: 0x00407938
		public void Bind(CielOSDesktop desktop, string appId, string displayName, Sprite icon, bool colored = false, bool locked = false)
		{
			this._desktop = desktop;
			this._appId = appId;
			this._locked = locked;
			this._canvas = base.GetComponentInParent<Canvas>();
			if (this.label != null)
			{
				LocalizedGameText.BindSource(this.label, "cielos_shell", displayName, null).FitTranslationToRectangle(false);
				this.label.alignment = TextAlignmentOptions.Top;
			}
			if (this.iconImage != null)
			{
				this.iconImage.enabled = true;
				if (icon != null)
				{
					this.iconImage.sprite = icon;
					this.iconImage.color = Color.white;
				}
				else
				{
					CielOSIcon.ApplyBadge(this.iconImage, CielOSAppIcons.ForApp(appId), 64);
				}
			}
			if (this.selectionHighlight != null)
			{
				this.selectionHighlight.color = ((CielOSTheme.Active != null) ? CielOSTheme.Active.selection : new Color(0.66f, 0.38f, 0.79f, 0.28f));
				CielOSTheme.Round(this.selectionHighlight);
			}
			CielOSAppAccess.ApplyLock(this.iconImage, locked);
			this.SetSelected(false);
		}

		// Token: 0x06009D41 RID: 40257 RVA: 0x0040985D File Offset: 0x00407A5D
		public void SetSelected(bool selected)
		{
			if (this.selectionHighlight != null)
			{
				this.selectionHighlight.enabled = selected;
			}
		}

		// Token: 0x06009D42 RID: 40258 RVA: 0x0040987C File Offset: 0x00407A7C
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this._dragged)
			{
				this._dragged = false;
				return;
			}
			float unscaledTime = Time.unscaledTime;
			bool flag = unscaledTime - this._lastClickTime <= this.doubleClickTime;
			this._lastClickTime = unscaledTime;
			if (this._desktop != null)
			{
				this._desktop.OnIconClicked(this);
			}
			if ((flag || this._locked) && this._desktop != null)
			{
				this._desktop.LaunchIcon(this);
			}
		}

		// Token: 0x06009D43 RID: 40259 RVA: 0x004098F7 File Offset: 0x00407AF7
		public void OnSubmit(BaseEventData eventData)
		{
			if (this._desktop == null)
			{
				return;
			}
			this._desktop.OnIconClicked(this);
			this._desktop.LaunchIcon(this);
		}

		// Token: 0x06009D44 RID: 40260 RVA: 0x00409920 File Offset: 0x00407B20
		public void OnSelect(BaseEventData eventData)
		{
			this.SetSelected(true);
		}

		// Token: 0x06009D45 RID: 40261 RVA: 0x00409929 File Offset: 0x00407B29
		public void OnDeselect(BaseEventData eventData)
		{
			this.SetSelected(false);
		}

		// Token: 0x06009D46 RID: 40262 RVA: 0x00409932 File Offset: 0x00407B32
		public void OnBeginDrag(PointerEventData eventData)
		{
			this._dragged = true;
			if (this._desktop != null)
			{
				this._desktop.OnIconClicked(this);
			}
		}

		// Token: 0x06009D47 RID: 40263 RVA: 0x00409958 File Offset: 0x00407B58
		public void OnDrag(PointerEventData eventData)
		{
			float num = ((this._canvas != null) ? this._canvas.scaleFactor : 1f);
			if (num <= 0f)
			{
				num = 1f;
			}
			this.Rect.anchoredPosition += eventData.delta / num;
		}

		// Token: 0x06009D48 RID: 40264 RVA: 0x004099B6 File Offset: 0x00407BB6
		public void OnEndDrag(PointerEventData eventData)
		{
			if (this._desktop != null)
			{
				this._desktop.OnIconMoved(this, this.Rect.anchoredPosition);
			}
		}

		// Token: 0x0400A1BC RID: 41404
		[SerializeField]
		private Image iconImage;

		// Token: 0x0400A1BD RID: 41405
		[SerializeField]
		private TMP_Text label;

		// Token: 0x0400A1BE RID: 41406
		[SerializeField]
		private Image selectionHighlight;

		// Token: 0x0400A1BF RID: 41407
		[Tooltip("Seconds within which a second click counts as a double-click (launch).")]
		[SerializeField]
		private float doubleClickTime = 0.35f;

		// Token: 0x0400A1C0 RID: 41408
		private RectTransform _rect;

		// Token: 0x0400A1C1 RID: 41409
		private CielOSDesktop _desktop;

		// Token: 0x0400A1C2 RID: 41410
		private string _appId;

		// Token: 0x0400A1C3 RID: 41411
		private Canvas _canvas;

		// Token: 0x0400A1C4 RID: 41412
		private float _lastClickTime = -1f;

		// Token: 0x0400A1C5 RID: 41413
		private bool _dragged;

		// Token: 0x0400A1C6 RID: 41414
		private bool _locked;
	}
}
