using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000253 RID: 595
	[ExecuteInEditMode]
	[RequireComponent(typeof(ButtonManager))]
	public class UIManagerButton : MonoBehaviour
	{
		// Token: 0x06000FE7 RID: 4071 RVA: 0x000699F0 File Offset: 0x00067BF0
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			if (this.buttonManager == null)
			{
				this.buttonManager = base.GetComponent<ButtonManager>();
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateButton();
				base.enabled = false;
			}
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x00069A56 File Offset: 0x00067C56
		private void Update()
		{
			if (this.UIManagerAsset == null || this.buttonManager == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateButton();
			}
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00069A88 File Offset: 0x00067C88
		private void UpdateButton()
		{
			if (!this.overrideColors)
			{
				if (this.disabledBackground != null)
				{
					Graphic graphic = this.disabledBackground;
					Graphic graphic2 = this.highlightedBackground;
					Color color = new Color(this.UIManagerAsset.buttonAccentColor.r, this.UIManagerAsset.buttonAccentColor.g, this.UIManagerAsset.buttonAccentColor.b, this.UIManagerAsset.buttonDisabledAlpha);
					graphic2.color = color;
					graphic.color = color;
				}
				if (this.normalBackground != null)
				{
					this.normalBackground.color = this.UIManagerAsset.buttonAccentColor;
				}
				if (this.highlightedBackground != null)
				{
					this.highlightedBackground.color = this.UIManagerAsset.buttonAccentColor;
				}
			}
			if (this.buttonManager.enableIcon && !this.overrideColors)
			{
				if (!this.outlineMode)
				{
					if (this.disabledIcon != null)
					{
						this.disabledIcon.color = this.UIManagerAsset.buttonNormalColor;
					}
					if (this.normalIcon != null)
					{
						this.normalIcon.color = this.UIManagerAsset.buttonNormalColor;
					}
					if (this.highlightedIcon != null)
					{
						this.highlightedIcon.color = this.UIManagerAsset.buttonNormalColor;
					}
				}
				else
				{
					if (this.disabledIcon != null)
					{
						this.disabledIcon.color = new Color(this.UIManagerAsset.buttonAccentColor.r, this.UIManagerAsset.buttonAccentColor.g, this.UIManagerAsset.buttonAccentColor.b, this.UIManagerAsset.buttonDisabledAlpha);
					}
					if (this.normalIcon != null)
					{
						this.normalIcon.color = this.UIManagerAsset.buttonAccentColor;
					}
					if (this.highlightedIcon != null)
					{
						this.highlightedIcon.color = this.UIManagerAsset.buttonNormalColor;
					}
				}
			}
			if (this.buttonManager.enableText)
			{
				if (!this.overrideColors)
				{
					if (!this.outlineMode)
					{
						if (this.disabledText != null)
						{
							this.disabledText.color = this.UIManagerAsset.buttonNormalColor;
						}
						if (this.normalText != null)
						{
							this.normalText.color = this.UIManagerAsset.buttonNormalColor;
						}
						if (this.highlightedText != null)
						{
							this.highlightedText.color = this.UIManagerAsset.buttonNormalColor;
						}
					}
					else
					{
						if (this.disabledText != null)
						{
							this.disabledText.color = new Color(this.UIManagerAsset.buttonAccentColor.r, this.UIManagerAsset.buttonAccentColor.g, this.UIManagerAsset.buttonAccentColor.b, this.UIManagerAsset.buttonDisabledAlpha);
						}
						if (this.normalText != null)
						{
							this.normalText.color = this.UIManagerAsset.buttonAccentColor;
						}
						if (this.highlightedText != null)
						{
							this.highlightedText.color = this.UIManagerAsset.buttonNormalColor;
						}
					}
				}
				if (!this.overrideFonts)
				{
					if (this.disabledText != null)
					{
						this.disabledText.font = this.UIManagerAsset.buttonFont;
					}
					if (this.normalText != null)
					{
						this.normalText.font = this.UIManagerAsset.buttonFont;
					}
					if (this.highlightedText != null)
					{
						this.highlightedText.font = this.UIManagerAsset.buttonFont;
					}
				}
			}
		}

		// Token: 0x04001147 RID: 4423
		[Header("Settings")]
		[SerializeField]
		private bool outlineMode;

		// Token: 0x04001148 RID: 4424
		[Header("Resources")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001149 RID: 4425
		public ButtonManager buttonManager;

		// Token: 0x0400114A RID: 4426
		[HideInInspector]
		public bool overrideColors;

		// Token: 0x0400114B RID: 4427
		[HideInInspector]
		public bool overrideFonts;

		// Token: 0x0400114C RID: 4428
		[HideInInspector]
		public Image disabledBackground;

		// Token: 0x0400114D RID: 4429
		[HideInInspector]
		public Image normalBackground;

		// Token: 0x0400114E RID: 4430
		[HideInInspector]
		public Image highlightedBackground;

		// Token: 0x0400114F RID: 4431
		[HideInInspector]
		public Image disabledIcon;

		// Token: 0x04001150 RID: 4432
		[HideInInspector]
		public Image normalIcon;

		// Token: 0x04001151 RID: 4433
		[HideInInspector]
		public Image highlightedIcon;

		// Token: 0x04001152 RID: 4434
		[HideInInspector]
		public TextMeshProUGUI disabledText;

		// Token: 0x04001153 RID: 4435
		[HideInInspector]
		public TextMeshProUGUI normalText;

		// Token: 0x04001154 RID: 4436
		[HideInInspector]
		public TextMeshProUGUI highlightedText;
	}
}
