using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000265 RID: 613
	[ExecuteInEditMode]
	public class UIManagerToggle : MonoBehaviour
	{
		// Token: 0x06001020 RID: 4128 RVA: 0x0006B330 File Offset: 0x00069530
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateToggle();
				base.enabled = false;
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0006B37C File Offset: 0x0006957C
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateToggle();
			}
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0006B3A0 File Offset: 0x000695A0
		private void UpdateToggle()
		{
			this.border.color = this.UIManagerAsset.toggleBorderColor;
			this.background.color = this.UIManagerAsset.toggleBackgroundColor;
			this.check.color = this.UIManagerAsset.toggleCheckColor;
			if (this.onLabel != null)
			{
				this.onLabel.color = new Color(this.UIManagerAsset.toggleTextColor.r, this.UIManagerAsset.toggleTextColor.g, this.UIManagerAsset.toggleTextColor.b, this.onLabel.color.a);
				this.onLabel.font = this.UIManagerAsset.toggleFont;
			}
			if (this.offLabel != null)
			{
				this.offLabel.color = new Color(this.UIManagerAsset.toggleTextColor.r, this.UIManagerAsset.toggleTextColor.g, this.UIManagerAsset.toggleTextColor.b, this.offLabel.color.a);
				this.offLabel.font = this.UIManagerAsset.toggleFont;
			}
		}

		// Token: 0x040011B2 RID: 4530
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x040011B3 RID: 4531
		[Header("Resources")]
		[SerializeField]
		private Image border;

		// Token: 0x040011B4 RID: 4532
		[SerializeField]
		private Image background;

		// Token: 0x040011B5 RID: 4533
		[SerializeField]
		private Image check;

		// Token: 0x040011B6 RID: 4534
		[SerializeField]
		private TextMeshProUGUI onLabel;

		// Token: 0x040011B7 RID: 4535
		[SerializeField]
		private TextMeshProUGUI offLabel;
	}
}
