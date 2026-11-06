using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000262 RID: 610
	[ExecuteInEditMode]
	public class UIManagerScrollbar : MonoBehaviour
	{
		// Token: 0x06001014 RID: 4116 RVA: 0x0006AD7C File Offset: 0x00068F7C
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateScrollbar();
				base.enabled = false;
			}
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0006ADC8 File Offset: 0x00068FC8
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateScrollbar();
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0006ADEC File Offset: 0x00068FEC
		private void UpdateScrollbar()
		{
			this.background.color = this.UIManagerAsset.scrollbarBackgroundColor;
			this.bar.color = this.UIManagerAsset.scrollbarColor;
		}

		// Token: 0x0400119F RID: 4511
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x040011A0 RID: 4512
		[Header("Resources")]
		[SerializeField]
		private Image background;

		// Token: 0x040011A1 RID: 4513
		[SerializeField]
		private Image bar;
	}
}
