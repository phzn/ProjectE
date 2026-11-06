using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000266 RID: 614
	[ExecuteInEditMode]
	public class UIManagerTooltip : MonoBehaviour
	{
		// Token: 0x06001024 RID: 4132 RVA: 0x0006B4D8 File Offset: 0x000696D8
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateTooltip();
				base.enabled = false;
			}
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0006B524 File Offset: 0x00069724
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateTooltip();
			}
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0006B548 File Offset: 0x00069748
		private void UpdateTooltip()
		{
			this.background.color = this.UIManagerAsset.tooltipBackgroundColor;
			this.text.color = this.UIManagerAsset.tooltipTextColor;
			this.text.font = this.UIManagerAsset.tooltipFont;
			this.text.fontSize = this.UIManagerAsset.tooltipFontSize;
		}

		// Token: 0x040011B8 RID: 4536
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x040011B9 RID: 4537
		[Header("Resources")]
		[SerializeField]
		private Image background;

		// Token: 0x040011BA RID: 4538
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
