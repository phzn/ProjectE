using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x0200025E RID: 606
	[ExecuteInEditMode]
	public class UIManagerNotification : MonoBehaviour
	{
		// Token: 0x06001007 RID: 4103 RVA: 0x0006AA68 File Offset: 0x00068C68
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateNotification();
				base.enabled = false;
			}
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0006AAB4 File Offset: 0x00068CB4
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateNotification();
			}
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0006AAD8 File Offset: 0x00068CD8
		private void UpdateNotification()
		{
			if (!this.overrideColors)
			{
				this.background.color = this.UIManagerAsset.notificationBackgroundColor;
				this.icon.color = this.UIManagerAsset.notificationIconColor;
				this.title.color = this.UIManagerAsset.notificationTitleColor;
				this.description.color = this.UIManagerAsset.notificationDescriptionColor;
			}
			if (!this.overrideFonts)
			{
				this.title.font = this.UIManagerAsset.notificationTitleFont;
				this.title.fontSize = this.UIManagerAsset.notificationTitleFontSize;
				this.description.font = this.UIManagerAsset.notificationDescriptionFont;
				this.description.fontSize = this.UIManagerAsset.notificationDescriptionFontSize;
			}
		}

		// Token: 0x0400118B RID: 4491
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x0400118C RID: 4492
		[HideInInspector]
		public bool overrideColors;

		// Token: 0x0400118D RID: 4493
		[HideInInspector]
		public bool overrideFonts;

		// Token: 0x0400118E RID: 4494
		[Header("Resources")]
		[SerializeField]
		private Image background;

		// Token: 0x0400118F RID: 4495
		[SerializeField]
		private Image icon;

		// Token: 0x04001190 RID: 4496
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x04001191 RID: 4497
		[SerializeField]
		private TextMeshProUGUI description;
	}
}
