using System;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.RapidActions
{
	// Token: 0x02000D95 RID: 3477
	[DisallowMultipleComponent]
	public class RapidActionSlotUI : MonoBehaviour
	{
		// Token: 0x06006660 RID: 26208 RVA: 0x0028F11D File Offset: 0x0028D31D
		private void OnEnable()
		{
			if (this._keyRect != null && ControllerInput.IsActive)
			{
				this.FitActionContainerBeforeRow();
			}
		}

		// Token: 0x06006661 RID: 26209 RVA: 0x0028F13C File Offset: 0x0028D33C
		private void FitActionContainerBeforeRow()
		{
			if (!(this.categoryLabelText == null))
			{
				RectTransform rectTransform = this.categoryLabelText.transform.parent as RectTransform;
				if (rectTransform != null)
				{
					ContentSizeFitter component = rectTransform.GetComponent<ContentSizeFitter>();
					if (component == null || !component.enabled || component.horizontalFit == ContentSizeFitter.FitMode.Unconstrained)
					{
						return;
					}
					LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
					RectTransform rectTransform2 = base.transform as RectTransform;
					if (rectTransform2 != null)
					{
						LayoutRebuilder.MarkLayoutForRebuild(rectTransform2);
					}
					return;
				}
			}
		}

		// Token: 0x06006662 RID: 26210 RVA: 0x0028F1B0 File Offset: 0x0028D3B0
		public void SetCategory(string category)
		{
			if (this.categoryLabelText == null)
			{
				return;
			}
			category = category ?? string.Empty;
			if (this._categorySource == category)
			{
				return;
			}
			this._categorySource = category;
			LocalizedGameText.BindSource(this.categoryLabelText, "gameplay_prompts", category ?? string.Empty, null).FitTranslationToRectangle(false);
		}

		// Token: 0x06006663 RID: 26211 RVA: 0x0028F20F File Offset: 0x0028D40F
		public void SetActionName(string actionName)
		{
			if (this.actionNameText == null)
			{
				return;
			}
			if (this._isDisabled)
			{
				return;
			}
			this.BindActionName(actionName);
		}

		// Token: 0x06006664 RID: 26212 RVA: 0x0028F230 File Offset: 0x0028D430
		public void SetDisabledState(bool disabled, string disabledLabel, string restoreActionName)
		{
			if (this.actionNameText == null)
			{
				return;
			}
			if (!this._disabledColorCached)
			{
				this._actionNameOriginalColor = this.actionNameText.color;
				this._disabledColorCached = true;
			}
			this._isDisabled = disabled;
			if (disabled)
			{
				this.actionNameText.color = new Color(0.9f, 0.25f, 0.25f);
				this.BindActionName(string.IsNullOrEmpty(disabledLabel) ? "Disabled" : disabledLabel);
				return;
			}
			this.actionNameText.color = this._actionNameOriginalColor;
			this.BindActionName(restoreActionName);
		}

		// Token: 0x06006665 RID: 26213 RVA: 0x0028F2C4 File Offset: 0x0028D4C4
		public void SetKeyCap(string keyCap)
		{
			if (this.keyCapText == null)
			{
				return;
			}
			bool isActive = ControllerInput.IsActive;
			if (this._keyRect == null)
			{
				this._keyRect = this.keyCapText.transform.parent as RectTransform;
				this._keyboardKeyWidth = ((this._keyRect != null) ? this._keyRect.rect.width : 28f);
				this._keyboardFontSize = this.keyCapText.fontSize;
				this._keyboardWrap = this.keyCapText.enableWordWrapping;
				this._keyboardIconEnabled = this.keyIconImage != null && this.keyIconImage.enabled;
			}
			this.keyCapText.fontSize = (isActive ? Mathf.Max(20f, this._keyboardFontSize) : this._keyboardFontSize);
			this.keyCapText.enableWordWrapping = !isActive && this._keyboardWrap;
			if (this.keyIconImage != null)
			{
				this.keyIconImage.enabled = !isActive && this._keyboardIconEnabled;
			}
			if (isActive)
			{
				ControllerGlyphs.Apply(this.keyCapText, keyCap ?? string.Empty);
			}
			else
			{
				this.keyCapText.text = keyCap ?? string.Empty;
			}
			if (this._keyRect != null)
			{
				float num = ((isActive && !string.IsNullOrEmpty(keyCap) && keyCap.Contains("+")) ? Mathf.Max(this._keyboardKeyWidth, this.keyCapText.fontSize * 3.4f) : this._keyboardKeyWidth);
				float num2 = (isActive ? Mathf.Max(num, this.keyCapText.GetPreferredValues(this.keyCapText.text, float.PositiveInfinity, float.PositiveInfinity).x + 8f) : this._keyboardKeyWidth);
				if (!Mathf.Approximately(this._keyRect.rect.width, num2))
				{
					this._keyRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num2);
					if (isActive && base.isActiveAndEnabled)
					{
						this.FitActionContainerBeforeRow();
					}
					RectTransform rectTransform = base.transform as RectTransform;
					if (rectTransform != null)
					{
						LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
					}
				}
			}
		}

		// Token: 0x06006666 RID: 26214 RVA: 0x0028F4F4 File Offset: 0x0028D6F4
		private void BindActionName(string english)
		{
			english = english ?? string.Empty;
			if (this._actionSource == english)
			{
				return;
			}
			this._actionSource = english;
			LocalizedGameText.BindSource(this.actionNameText, "gameplay_prompts", english ?? string.Empty, null).FitTranslationToRectangle(false);
		}

		// Token: 0x06006667 RID: 26215 RVA: 0x0028F544 File Offset: 0x0028D744
		public void SetKeyIcon(Sprite sprite)
		{
			if (this.keyIconImage == null || sprite == null)
			{
				return;
			}
			this.keyIconImage.sprite = sprite;
		}

		// Token: 0x06006668 RID: 26216 RVA: 0x0028F56A File Offset: 0x0028D76A
		public void Set(string category, string actionName, string keyCap, Sprite keyIcon = null)
		{
			this.SetCategory(category);
			this.SetActionName(actionName);
			this.SetKeyCap(keyCap);
			if (keyIcon != null)
			{
				this.SetKeyIcon(keyIcon);
			}
		}

		// Token: 0x040068EA RID: 26858
		[Header("Keycap")]
		[Tooltip("Background image of the key (the rounded sprite). Optional — if null the sprite is left untouched.")]
		[SerializeField]
		private Image keyIconImage;

		// Token: 0x040068EB RID: 26859
		[Tooltip("Short label drawn on the keycap (e.g. 'ML' for left mouse, '1', 'E').")]
		[SerializeField]
		private TMP_Text keyCapText;

		// Token: 0x040068EC RID: 26860
		[Header("Action Text")]
		[Tooltip("Static category prefix shown on the left of the action name (e.g. 'Arms:').")]
		[SerializeField]
		private TMP_Text categoryLabelText;

		// Token: 0x040068ED RID: 26861
		[Tooltip("Dynamic action name (e.g. the current pose label or the bound emote display name).")]
		[SerializeField]
		private TMP_Text actionNameText;

		// Token: 0x040068EE RID: 26862
		private string _categorySource;

		// Token: 0x040068EF RID: 26863
		private string _actionSource;

		// Token: 0x040068F0 RID: 26864
		private RectTransform _keyRect;

		// Token: 0x040068F1 RID: 26865
		private float _keyboardKeyWidth;

		// Token: 0x040068F2 RID: 26866
		private float _keyboardFontSize;

		// Token: 0x040068F3 RID: 26867
		private bool _keyboardWrap;

		// Token: 0x040068F4 RID: 26868
		private bool _keyboardIconEnabled;

		// Token: 0x040068F5 RID: 26869
		private bool _disabledColorCached;

		// Token: 0x040068F6 RID: 26870
		private Color _actionNameOriginalColor;

		// Token: 0x040068F7 RID: 26871
		private bool _isDisabled;
	}
}
