using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000263 RID: 611
	[ExecuteInEditMode]
	public class UIManagerSlider : MonoBehaviour
	{
		// Token: 0x06001018 RID: 4120 RVA: 0x0006AE1C File Offset: 0x0006901C
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateSlider();
				base.enabled = false;
			}
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0006AE68 File Offset: 0x00069068
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateSlider();
			}
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0006AE8C File Offset: 0x0006908C
		private void UpdateSlider()
		{
			if (this.UIManagerAsset.sliderThemeType == UIManager.SliderThemeType.Basic)
			{
				if (!this.overrideColors)
				{
					this.background.color = this.UIManagerAsset.sliderBackgroundColor;
					this.bar.color = this.UIManagerAsset.sliderColor;
					this.handle.color = this.UIManagerAsset.sliderColor;
				}
				if (this.hasLabel)
				{
					if (!this.overrideColors)
					{
						this.label.color = new Color(this.UIManagerAsset.sliderColor.r, this.UIManagerAsset.sliderColor.g, this.UIManagerAsset.sliderColor.b, this.label.color.a);
					}
					if (!this.overrideFonts)
					{
						this.label.font = this.UIManagerAsset.sliderLabelFont;
					}
				}
				if (this.hasPopupLabel)
				{
					if (!this.overrideColors)
					{
						this.popupLabel.color = new Color(this.UIManagerAsset.sliderPopupLabelColor.r, this.UIManagerAsset.sliderPopupLabelColor.g, this.UIManagerAsset.sliderPopupLabelColor.b, this.popupLabel.color.a);
					}
					if (!this.overrideFonts)
					{
						this.popupLabel.font = this.UIManagerAsset.sliderLabelFont;
						return;
					}
				}
			}
			else if (this.UIManagerAsset.sliderThemeType == UIManager.SliderThemeType.Custom)
			{
				if (!this.overrideColors)
				{
					this.background.color = this.UIManagerAsset.sliderBackgroundColor;
					this.bar.color = this.UIManagerAsset.sliderColor;
					this.handle.color = this.UIManagerAsset.sliderHandleColor;
				}
				if (this.hasLabel)
				{
					if (!this.overrideColors)
					{
						this.label.color = new Color(this.UIManagerAsset.sliderLabelColor.r, this.UIManagerAsset.sliderLabelColor.g, this.UIManagerAsset.sliderLabelColor.b, this.label.color.a);
					}
					if (!this.overrideFonts)
					{
						this.label.font = this.UIManagerAsset.sliderLabelFont;
						this.label.font = this.UIManagerAsset.sliderLabelFont;
					}
				}
				if (this.hasPopupLabel)
				{
					if (!this.overrideColors)
					{
						this.popupLabel.color = new Color(this.UIManagerAsset.sliderPopupLabelColor.r, this.UIManagerAsset.sliderPopupLabelColor.g, this.UIManagerAsset.sliderPopupLabelColor.b, this.popupLabel.color.a);
					}
					if (!this.overrideFonts)
					{
						this.popupLabel.font = this.UIManagerAsset.sliderLabelFont;
					}
				}
			}
		}

		// Token: 0x040011A2 RID: 4514
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x040011A3 RID: 4515
		public bool hasLabel;

		// Token: 0x040011A4 RID: 4516
		public bool hasPopupLabel;

		// Token: 0x040011A5 RID: 4517
		[HideInInspector]
		public bool overrideColors;

		// Token: 0x040011A6 RID: 4518
		[HideInInspector]
		public bool overrideFonts;

		// Token: 0x040011A7 RID: 4519
		[Header("Resources")]
		[SerializeField]
		private Image background;

		// Token: 0x040011A8 RID: 4520
		[SerializeField]
		private Image bar;

		// Token: 0x040011A9 RID: 4521
		[SerializeField]
		private Image handle;

		// Token: 0x040011AA RID: 4522
		[HideInInspector]
		public TextMeshProUGUI label;

		// Token: 0x040011AB RID: 4523
		[HideInInspector]
		public TextMeshProUGUI popupLabel;
	}
}
