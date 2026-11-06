using System;
using Features.CielOS.UI;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.WindowManager
{
	// Token: 0x0200139D RID: 5021
	public class CielOSWindow : MonoBehaviour
	{
		// Token: 0x170018BC RID: 6332
		// (get) Token: 0x06009B67 RID: 39783 RVA: 0x003F8506 File Offset: 0x003F6706
		public string WindowTitle
		{
			get
			{
				return this.windowTitle;
			}
		}

		// Token: 0x170018BD RID: 6333
		// (get) Token: 0x06009B68 RID: 39784 RVA: 0x003F850E File Offset: 0x003F670E
		// (set) Token: 0x06009B69 RID: 39785 RVA: 0x003F8516 File Offset: 0x003F6716
		public bool LocalizeWindowTitle { get; private set; }

		// Token: 0x170018BE RID: 6334
		// (get) Token: 0x06009B6A RID: 39786 RVA: 0x003F851F File Offset: 0x003F671F
		public Sprite WindowIcon
		{
			get
			{
				return this.windowIcon;
			}
		}

		// Token: 0x170018BF RID: 6335
		// (get) Token: 0x06009B6B RID: 39787 RVA: 0x003F8527 File Offset: 0x003F6727
		// (set) Token: 0x06009B6C RID: 39788 RVA: 0x003F852F File Offset: 0x003F672F
		public string AppId { get; private set; }

		// Token: 0x170018C0 RID: 6336
		// (get) Token: 0x06009B6D RID: 39789 RVA: 0x003F8538 File Offset: 0x003F6738
		public RectTransform Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x170018C1 RID: 6337
		// (get) Token: 0x06009B6E RID: 39790 RVA: 0x003F8540 File Offset: 0x003F6740
		public RectTransform Rect
		{
			get
			{
				return this.rectTransform;
			}
		}

		// Token: 0x170018C2 RID: 6338
		// (get) Token: 0x06009B6F RID: 39791 RVA: 0x003F8548 File Offset: 0x003F6748
		// (set) Token: 0x06009B70 RID: 39792 RVA: 0x003F8550 File Offset: 0x003F6750
		public bool IsMinimized { get; private set; }

		// Token: 0x170018C3 RID: 6339
		// (get) Token: 0x06009B71 RID: 39793 RVA: 0x003F8559 File Offset: 0x003F6759
		// (set) Token: 0x06009B72 RID: 39794 RVA: 0x003F8561 File Offset: 0x003F6761
		public bool IsOpen { get; private set; }

		// Token: 0x170018C4 RID: 6340
		// (get) Token: 0x06009B73 RID: 39795 RVA: 0x003F856A File Offset: 0x003F676A
		// (set) Token: 0x06009B74 RID: 39796 RVA: 0x003F8572 File Offset: 0x003F6772
		public bool IsResizable { get; private set; } = true;

		// Token: 0x170018C5 RID: 6341
		// (get) Token: 0x06009B75 RID: 39797 RVA: 0x003F857B File Offset: 0x003F677B
		// (set) Token: 0x06009B76 RID: 39798 RVA: 0x003F8583 File Offset: 0x003F6783
		public bool IsMaximized { get; private set; }

		// Token: 0x14000115 RID: 277
		// (add) Token: 0x06009B77 RID: 39799 RVA: 0x003F858C File Offset: 0x003F678C
		// (remove) Token: 0x06009B78 RID: 39800 RVA: 0x003F85C4 File Offset: 0x003F67C4
		public event Action<CielOSWindow> Closed;

		// Token: 0x14000116 RID: 278
		// (add) Token: 0x06009B79 RID: 39801 RVA: 0x003F85FC File Offset: 0x003F67FC
		// (remove) Token: 0x06009B7A RID: 39802 RVA: 0x003F8634 File Offset: 0x003F6834
		public event Action<CielOSWindow, bool> MinimizedChanged;

		// Token: 0x06009B7B RID: 39803 RVA: 0x003F8669 File Offset: 0x003F6869
		private void Reset()
		{
			this.rectTransform = base.GetComponent<RectTransform>();
			this.canvasGroup = base.GetComponent<CanvasGroup>();
		}

		// Token: 0x06009B7C RID: 39804 RVA: 0x003F8684 File Offset: 0x003F6884
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			if (this.canvasGroup == null)
			{
				this.canvasGroup = base.GetComponent<CanvasGroup>();
			}
			if (this.closeButton != null)
			{
				this.closeButton.onClick.AddListener(new UnityAction(this.Close));
			}
			if (this.minimizeButton != null)
			{
				this.minimizeButton.onClick.AddListener(new UnityAction(this.ToggleMinimize));
			}
			CielOSSfx.Attach(this.closeButton);
			CielOSSfx.Attach(this.minimizeButton);
		}

		// Token: 0x06009B7D RID: 39805 RVA: 0x003F8730 File Offset: 0x003F6930
		public void Initialize(CielOSWindowManager manager, string title, Sprite icon, string appId = null, bool allowResize = true, Vector2 minimumSize = default(Vector2), bool openMaximized = false, bool localizeTitle = false)
		{
			this._manager = manager;
			this.IsResizable = allowResize;
			if (minimumSize.x > 0f && minimumSize.y > 0f)
			{
				this.minSize = minimumSize;
			}
			if (!string.IsNullOrEmpty(title))
			{
				this.windowTitle = title;
			}
			if (icon != null)
			{
				this.windowIcon = icon;
			}
			if (!string.IsNullOrEmpty(appId))
			{
				this.AppId = appId;
			}
			this.LocalizeWindowTitle = localizeTitle;
			if (this.titleText != null)
			{
				this.titleText.text = this.windowTitle;
				if (localizeTitle)
				{
					LocalizedGameText.BindSource(this.titleText, "cielos_shell", this.windowTitle, null).FitTranslationToRectangle(false);
				}
				else
				{
					LocalizedGameText.BindFontOnly(this.titleText);
				}
			}
			if (this.rectTransform != null && this.rectTransform.sizeDelta == Vector2.zero)
			{
				this.rectTransform.sizeDelta = this.defaultSize;
			}
			if (openMaximized)
			{
				this.MaximizeToParent();
			}
			else
			{
				this.ClampPositionToParent();
				this.ClampSizeToMin();
				this.ClampPositionToParent();
			}
			this.ApplyTheme();
			this.ApplyResizePolicy();
		}

		// Token: 0x06009B7E RID: 39806 RVA: 0x003F8858 File Offset: 0x003F6A58
		private void ApplyTheme()
		{
			CielOSTheme cielOSTheme = ((this._manager != null) ? this._manager.Theme : CielOSTheme.Active);
			if (cielOSTheme == null)
			{
				return;
			}
			if (this.windowBackground == null)
			{
				this.windowBackground = base.GetComponent<Image>();
			}
			if (this.windowBackground != null)
			{
				this.windowBackground.color = cielOSTheme.windowBackground;
				CielOSTheme.Round(this.windowBackground);
			}
			if (this.titleBarImage != null)
			{
				this.titleBarImage.color = cielOSTheme.titleBarActive;
				CielOSTheme.Round(this.titleBarImage);
			}
			if (this.titleText != null)
			{
				this.titleText.color = cielOSTheme.titleText;
			}
			if (this.content != null)
			{
				this.content.offsetMin = new Vector2(3f, 3f);
				Vector2 offsetMax = this.content.offsetMax;
				this.content.offsetMax = new Vector2(-3f, offsetMax.y);
			}
			this.SkinControlButtons(cielOSTheme);
			this.SkinResizeGrip(cielOSTheme);
			this._border = CielOSTheme.AddBorder((this.rectTransform != null) ? this.rectTransform : ((RectTransform)base.transform), new Color?(cielOSTheme.outline), 1f);
		}

		// Token: 0x06009B7F RID: 39807 RVA: 0x003F89B6 File Offset: 0x003F6BB6
		private void SkinControlButtons(CielOSTheme theme)
		{
			this.SkinButton(this.closeButton, theme.closeButton, CielOSIconId.Close);
			this.SkinButton(this.minimizeButton, theme.minimizeButton, CielOSIconId.Minimize);
		}

		// Token: 0x06009B80 RID: 39808 RVA: 0x003F89E0 File Offset: 0x003F6BE0
		private void SkinButton(Button btn, Color dotColor, CielOSIconId glyph)
		{
			if (btn == null)
			{
				return;
			}
			Image component = btn.GetComponent<Image>();
			if (component != null)
			{
				component.color = dotColor;
				CielOSTheme.Circle(component);
			}
			Transform transform = btn.transform.Find("Glyph");
			if (transform != null)
			{
				Object.Destroy(transform.gameObject);
			}
			Transform transform2 = btn.transform.Find("IconGlyph");
			Image image;
			if (transform2 != null)
			{
				image = transform2.GetComponent<Image>();
			}
			else
			{
				GameObject gameObject = new GameObject("IconGlyph", new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				});
				gameObject.transform.SetParent(btn.transform, false);
				RectTransform rectTransform = (RectTransform)gameObject.transform;
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.one;
				rectTransform.offsetMin = new Vector2(5f, 5f);
				rectTransform.offsetMax = new Vector2(-5f, -5f);
				image = gameObject.GetComponent<Image>();
			}
			CielOSIcon.Apply(image, glyph, new Color(0.1f, 0.06f, 0.1f, 0.85f), 64);
			image.raycastTarget = false;
		}

		// Token: 0x06009B81 RID: 39809 RVA: 0x003F8B14 File Offset: 0x003F6D14
		private void SkinResizeGrip(CielOSTheme theme)
		{
			Transform transform = base.transform.Find("ResizeGrip");
			if (transform == null)
			{
				return;
			}
			Image component = transform.GetComponent<Image>();
			if (component != null)
			{
				CielOSIcon.Apply(component, CielOSIconId.ResizeGrip, new Color(theme.accent.r, theme.accent.g, theme.accent.b, 0.7f), 64);
			}
		}

		// Token: 0x06009B82 RID: 39810 RVA: 0x003F8B84 File Offset: 0x003F6D84
		private void ApplyResizePolicy()
		{
			Transform transform = base.transform.Find("ResizeGrip");
			if (transform != null)
			{
				transform.gameObject.SetActive(this.IsResizable);
			}
		}

		// Token: 0x06009B83 RID: 39811 RVA: 0x003F8BBC File Offset: 0x003F6DBC
		private void MaximizeToParent()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			this.rectTransform.anchorMin = Vector2.zero;
			this.rectTransform.anchorMax = Vector2.one;
			this.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			this.rectTransform.offsetMin = Vector2.zero;
			this.rectTransform.offsetMax = Vector2.zero;
			this.IsMaximized = true;
		}

		// Token: 0x06009B84 RID: 39812 RVA: 0x003F8C3C File Offset: 0x003F6E3C
		private void RestoreManualRectIfNeeded()
		{
			if (!this.IsMaximized || !this.IsResizable || this.rectTransform == null)
			{
				return;
			}
			Vector2 size = this.rectTransform.rect.size;
			this.rectTransform.anchorMin = new Vector2(0f, 1f);
			this.rectTransform.anchorMax = new Vector2(0f, 1f);
			this.rectTransform.pivot = new Vector2(0f, 1f);
			this.rectTransform.anchoredPosition = Vector2.zero;
			this.rectTransform.sizeDelta = size;
			this.IsMaximized = false;
		}

		// Token: 0x06009B85 RID: 39813 RVA: 0x003F8CED File Offset: 0x003F6EED
		public void Open()
		{
			this.IsOpen = true;
			this.IsMinimized = false;
			base.gameObject.SetActive(true);
			this.SetInteractable(true);
			this.BringToFront();
		}

		// Token: 0x06009B86 RID: 39814 RVA: 0x003F8D18 File Offset: 0x003F6F18
		public bool CanClose()
		{
			MonoBehaviour[] componentsInChildren = base.GetComponentsInChildren<MonoBehaviour>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				ICielOSWindowCloseGuard cielOSWindowCloseGuard = componentsInChildren[i] as ICielOSWindowCloseGuard;
				if (cielOSWindowCloseGuard != null && !cielOSWindowCloseGuard.CanCloseWindow())
				{
					if (this.IsMinimized)
					{
						this.Restore();
					}
					this.BringToFront();
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009B87 RID: 39815 RVA: 0x003F8D66 File Offset: 0x003F6F66
		public void Close()
		{
			if (!this.CanClose())
			{
				return;
			}
			this.IsOpen = false;
			Action<CielOSWindow> closed = this.Closed;
			if (closed != null)
			{
				closed(this);
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06009B88 RID: 39816 RVA: 0x003F8D95 File Offset: 0x003F6F95
		public void ToggleMinimize()
		{
			if (this.IsMinimized)
			{
				this.Restore();
				return;
			}
			this.Minimize();
		}

		// Token: 0x06009B89 RID: 39817 RVA: 0x003F8DAC File Offset: 0x003F6FAC
		public void Minimize()
		{
			this.IsMinimized = true;
			this.SetInteractable(false);
			base.gameObject.SetActive(false);
			Action<CielOSWindow, bool> minimizedChanged = this.MinimizedChanged;
			if (minimizedChanged == null)
			{
				return;
			}
			minimizedChanged(this, true);
		}

		// Token: 0x06009B8A RID: 39818 RVA: 0x003F8DDA File Offset: 0x003F6FDA
		public void Restore()
		{
			this.IsMinimized = false;
			base.gameObject.SetActive(true);
			this.SetInteractable(true);
			this.BringToFront();
			Action<CielOSWindow, bool> minimizedChanged = this.MinimizedChanged;
			if (minimizedChanged == null)
			{
				return;
			}
			minimizedChanged(this, false);
		}

		// Token: 0x06009B8B RID: 39819 RVA: 0x003F8E0E File Offset: 0x003F700E
		public void BringToFront()
		{
			if (this.rectTransform != null)
			{
				this.rectTransform.SetAsLastSibling();
			}
			if (this._manager != null)
			{
				this._manager.NotifyFocused(this);
			}
		}

		// Token: 0x06009B8C RID: 39820 RVA: 0x003F8E44 File Offset: 0x003F7044
		public void SetFocusedVisual(bool focused)
		{
			if (this._manager == null || this._manager.Theme == null)
			{
				return;
			}
			if (this.titleBarImage != null)
			{
				this.titleBarImage.color = (focused ? this._manager.Theme.titleBarActive : this._manager.Theme.titleBarInactive);
			}
			if (this._border != null)
			{
				this._border.color = (focused ? this._manager.Theme.accent : this._manager.Theme.outline);
			}
		}

		// Token: 0x06009B8D RID: 39821 RVA: 0x003F8EF0 File Offset: 0x003F70F0
		public void ApplySizeDelta(Vector2 delta)
		{
			if (!this.IsResizable || this.rectTransform == null)
			{
				return;
			}
			this.RestoreManualRectIfNeeded();
			this.ClampPositionToParent();
			Vector2 vector = this.rectTransform.sizeDelta + new Vector2(delta.x, -delta.y);
			Vector2 vector2 = this.MaximumSizeAtCurrentPosition();
			vector.x = Mathf.Clamp(vector.x, Mathf.Min(this.minSize.x, vector2.x), vector2.x);
			vector.y = Mathf.Clamp(vector.y, Mathf.Min(this.minSize.y, vector2.y), vector2.y);
			this.rectTransform.sizeDelta = vector;
			this.ClampPositionToParent();
		}

		// Token: 0x06009B8E RID: 39822 RVA: 0x003F8FB8 File Offset: 0x003F71B8
		public void ApplyMove(Vector2 delta)
		{
			if (this.rectTransform == null)
			{
				return;
			}
			if (this.IsMaximized)
			{
				return;
			}
			this.rectTransform.anchoredPosition += delta;
			this.ClampPositionToParent();
		}

		// Token: 0x06009B8F RID: 39823 RVA: 0x003F8FEF File Offset: 0x003F71EF
		public void SetMinimumSize(Vector2 value)
		{
			if (value.x <= 0f || value.y <= 0f)
			{
				return;
			}
			this.minSize = value;
			if (!this.IsMaximized)
			{
				this.ClampPositionToParent();
				this.ClampSizeToMin();
				this.ClampPositionToParent();
			}
		}

		// Token: 0x06009B90 RID: 39824 RVA: 0x003F9030 File Offset: 0x003F7230
		private void ClampSizeToMin()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			Vector2 sizeDelta = this.rectTransform.sizeDelta;
			Vector2 vector = this.MaximumSizeAtCurrentPosition();
			sizeDelta.x = Mathf.Clamp(sizeDelta.x, Mathf.Min(this.minSize.x, vector.x), vector.x);
			sizeDelta.y = Mathf.Clamp(sizeDelta.y, Mathf.Min(this.minSize.y, vector.y), vector.y);
			this.rectTransform.sizeDelta = sizeDelta;
		}

		// Token: 0x06009B91 RID: 39825 RVA: 0x003F90C8 File Offset: 0x003F72C8
		private void ClampPositionToParent()
		{
			if (this.rectTransform == null || this.IsMaximized)
			{
				return;
			}
			RectTransform rectTransform = this.rectTransform.parent as RectTransform;
			if (rectTransform == null || rectTransform.rect.width <= 0f || rectTransform.rect.height <= 0f)
			{
				return;
			}
			Vector2 size = this.rectTransform.rect.size;
			float num = Mathf.Max(0f, rectTransform.rect.width - size.x);
			float num2 = -Mathf.Max(0f, rectTransform.rect.height - size.y);
			Vector2 anchoredPosition = this.rectTransform.anchoredPosition;
			anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, 0f, num);
			anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, num2, 0f);
			this.rectTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x06009B92 RID: 39826 RVA: 0x003F91D8 File Offset: 0x003F73D8
		private Vector2 MaximumSizeAtCurrentPosition()
		{
			RectTransform rectTransform = ((this.rectTransform != null) ? (this.rectTransform.parent as RectTransform) : null);
			if (rectTransform == null || rectTransform.rect.width <= 0f || rectTransform.rect.height <= 0f)
			{
				return new Vector2(float.MaxValue, float.MaxValue);
			}
			Vector2 anchoredPosition = this.rectTransform.anchoredPosition;
			float num = Mathf.Clamp(anchoredPosition.x, 0f, rectTransform.rect.width);
			float num2 = Mathf.Clamp(-anchoredPosition.y, 0f, rectTransform.rect.height);
			return new Vector2(rectTransform.rect.width - num, rectTransform.rect.height - num2);
		}

		// Token: 0x06009B93 RID: 39827 RVA: 0x003F92B8 File Offset: 0x003F74B8
		private void SetInteractable(bool value)
		{
			if (this.canvasGroup == null)
			{
				return;
			}
			this.canvasGroup.interactable = value;
			this.canvasGroup.blocksRaycasts = value;
		}

		// Token: 0x0400A044 RID: 41028
		[Header("Identity")]
		[Tooltip("Title shown in the title bar and taskbar.")]
		[SerializeField]
		private string windowTitle = "Window";

		// Token: 0x0400A045 RID: 41029
		[Tooltip("Icon shown in the taskbar entry. Optional.")]
		[SerializeField]
		private Sprite windowIcon;

		// Token: 0x0400A046 RID: 41030
		[Header("References")]
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x0400A047 RID: 41031
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x0400A048 RID: 41032
		[SerializeField]
		private TMP_Text titleText;

		// Token: 0x0400A049 RID: 41033
		[SerializeField]
		private Image titleBarImage;

		// Token: 0x0400A04A RID: 41034
		[SerializeField]
		private Image windowBackground;

		// Token: 0x0400A04B RID: 41035
		[SerializeField]
		private RectTransform content;

		// Token: 0x0400A04C RID: 41036
		[SerializeField]
		private Button closeButton;

		// Token: 0x0400A04D RID: 41037
		[SerializeField]
		private Button minimizeButton;

		// Token: 0x0400A04E RID: 41038
		[Header("Sizing")]
		[SerializeField]
		private Vector2 minSize = new Vector2(320f, 200f);

		// Token: 0x0400A04F RID: 41039
		[SerializeField]
		private Vector2 defaultSize = new Vector2(720f, 460f);

		// Token: 0x0400A058 RID: 41048
		private CielOSWindowManager _manager;

		// Token: 0x0400A059 RID: 41049
		private Image _border;
	}
}
