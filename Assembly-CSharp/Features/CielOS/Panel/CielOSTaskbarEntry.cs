using System;
using Features.CielOS.UI;
using Features.CielOS.WindowManager;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Panel
{
	// Token: 0x020013E3 RID: 5091
	[RequireComponent(typeof(Button))]
	public class CielOSTaskbarEntry : MonoBehaviour
	{
		// Token: 0x06009D26 RID: 40230 RVA: 0x00408BCF File Offset: 0x00406DCF
		private void Awake()
		{
			if (this.button == null)
			{
				this.button = base.GetComponent<Button>();
			}
		}

		// Token: 0x06009D27 RID: 40231 RVA: 0x00408BEC File Offset: 0x00406DEC
		public void Bind(CielOSWindow window)
		{
			this._window = window;
			CielOSTheme active = CielOSTheme.Active;
			LayoutElement layoutElement = base.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = base.gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.minWidth = 120f;
			layoutElement.preferredWidth = 150f;
			layoutElement.minHeight = 28f;
			layoutElement.preferredHeight = 28f;
			layoutElement.flexibleHeight = 0f;
			Image component = base.GetComponent<Image>();
			if (component != null)
			{
				component.color = ((active != null) ? active.controlBackground : new Color(0.18f, 0.14f, 0.23f, 1f));
				CielOSTheme.Round(component);
			}
			if (this.label != null)
			{
				this.label.text = window.WindowTitle;
				this.label.color = ((active != null) ? active.controlText : Color.white);
				this.label.fontSize = 12f;
				this.label.enableWordWrapping = false;
				this.label.overflowMode = TextOverflowModes.Ellipsis;
				if (active != null && active.uiFont != null)
				{
					this.label.font = active.uiFont;
				}
				if (window.LocalizeWindowTitle)
				{
					LocalizedGameText.BindSource(this.label, "cielos_shell", window.WindowTitle, null).FitTranslationToRectangle(false);
				}
				else
				{
					LocalizedGameText.BindFontOnly(this.label);
				}
			}
			if (this.iconImage != null)
			{
				this.iconImage.enabled = true;
				Color color = ((active != null) ? active.logoAccent : Color.white);
				if (window.WindowIcon != null)
				{
					this.iconImage.sprite = window.WindowIcon;
					this.iconImage.color = Color.white;
				}
				else
				{
					CielOSIcon.Apply(this.iconImage, CielOSAppIcons.ForApp(window.AppId), color, 64);
				}
				((RectTransform)this.iconImage.transform).sizeDelta = new Vector2(16f, 16f);
			}
			if (this.button != null)
			{
				this.button.onClick.RemoveAllListeners();
				this.button.onClick.AddListener(new UnityAction(this.OnClicked));
				CielOSSfx.Attach(this.button);
			}
		}

		// Token: 0x06009D28 RID: 40232 RVA: 0x00408E4C File Offset: 0x0040704C
		private void OnClicked()
		{
			if (this._window == null)
			{
				return;
			}
			if (this._window.IsMinimized)
			{
				this._window.Restore();
				return;
			}
			this._window.Minimize();
		}

		// Token: 0x0400A1A8 RID: 41384
		[SerializeField]
		private Image iconImage;

		// Token: 0x0400A1A9 RID: 41385
		[SerializeField]
		private TMP_Text label;

		// Token: 0x0400A1AA RID: 41386
		[SerializeField]
		private Button button;

		// Token: 0x0400A1AB RID: 41387
		private CielOSWindow _window;
	}
}
