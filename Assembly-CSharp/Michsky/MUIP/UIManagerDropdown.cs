using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000259 RID: 601
	[ExecuteInEditMode]
	public class UIManagerDropdown : MonoBehaviour
	{
		// Token: 0x06000FF3 RID: 4083 RVA: 0x0006A22C File Offset: 0x0006842C
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateDropdown();
				base.enabled = false;
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0006A278 File Offset: 0x00068478
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateDropdown();
			}
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0006A29C File Offset: 0x0006849C
		private void UpdateDropdown()
		{
			if (!this.overrideFonts && this.mainText != null)
			{
				this.mainText.font = this.UIManagerAsset.dropdownFont;
			}
			if (!this.overrideColors)
			{
				if (this.background != null)
				{
					this.background.color = this.UIManagerAsset.dropdownBackgroundColor;
				}
				if (this.contentBackground != null)
				{
					this.contentBackground.color = this.UIManagerAsset.dropdownContentBackgroundColor;
				}
				if (this.mainIcon != null)
				{
					this.mainIcon.color = this.UIManagerAsset.dropdownPrimaryColor;
				}
				if (this.mainText != null)
				{
					this.mainText.color = this.UIManagerAsset.dropdownPrimaryColor;
				}
				if (this.expandIcon != null)
				{
					this.expandIcon.color = this.UIManagerAsset.dropdownPrimaryColor;
				}
			}
		}

		// Token: 0x04001169 RID: 4457
		[HideInInspector]
		public bool overrideColors;

		// Token: 0x0400116A RID: 4458
		[HideInInspector]
		public bool overrideFonts;

		// Token: 0x0400116B RID: 4459
		[Header("Resources")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x0400116C RID: 4460
		[SerializeField]
		private Image background;

		// Token: 0x0400116D RID: 4461
		[SerializeField]
		private Image contentBackground;

		// Token: 0x0400116E RID: 4462
		[SerializeField]
		private Image mainIcon;

		// Token: 0x0400116F RID: 4463
		[SerializeField]
		private TextMeshProUGUI mainText;

		// Token: 0x04001170 RID: 4464
		[SerializeField]
		private Image expandIcon;
	}
}
