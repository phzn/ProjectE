using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x0200025D RID: 605
	[ExecuteInEditMode]
	public class UIManagerModalWindow : MonoBehaviour
	{
		// Token: 0x06001003 RID: 4099 RVA: 0x0006A908 File Offset: 0x00068B08
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateModalWindow();
				base.enabled = false;
			}
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0006A954 File Offset: 0x00068B54
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateModalWindow();
			}
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0006A978 File Offset: 0x00068B78
		private void UpdateModalWindow()
		{
			if (this.background != null)
			{
				this.background.color = this.UIManagerAsset.modalWindowBackgroundColor;
			}
			if (this.contentBackground != null)
			{
				this.contentBackground.color = this.UIManagerAsset.modalWindowContentPanelColor;
			}
			if (this.icon != null)
			{
				this.icon.color = this.UIManagerAsset.modalWindowIconColor;
			}
			if (this.title != null)
			{
				this.title.color = this.UIManagerAsset.modalWindowTitleColor;
				this.title.font = this.UIManagerAsset.modalWindowTitleFont;
			}
			if (this.description != null)
			{
				this.description.color = this.UIManagerAsset.modalWindowDescriptionColor;
				this.description.font = this.UIManagerAsset.modalWindowContentFont;
			}
		}

		// Token: 0x04001185 RID: 4485
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001186 RID: 4486
		[Header("Resources")]
		[SerializeField]
		private Image background;

		// Token: 0x04001187 RID: 4487
		[SerializeField]
		private Image contentBackground;

		// Token: 0x04001188 RID: 4488
		[SerializeField]
		private Image icon;

		// Token: 0x04001189 RID: 4489
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x0400118A RID: 4490
		[SerializeField]
		private TextMeshProUGUI description;
	}
}
