using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000261 RID: 609
	[ExecuteInEditMode]
	public class UIManagerProgressBarLoop : MonoBehaviour
	{
		// Token: 0x06001010 RID: 4112 RVA: 0x0006ACA4 File Offset: 0x00068EA4
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateProgressBar();
				base.enabled = false;
			}
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0006ACF0 File Offset: 0x00068EF0
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateProgressBar();
			}
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0006AD14 File Offset: 0x00068F14
		private void UpdateProgressBar()
		{
			if (!this.overrideColors)
			{
				this.bar.color = this.UIManagerAsset.progressBarColor;
				if (this.hasBackground)
				{
					if (this.useRegularBackground)
					{
						this.background.color = this.UIManagerAsset.progressBarBackgroundColor;
						return;
					}
					this.background.color = this.UIManagerAsset.progressBarLoopBackgroundColor;
				}
			}
		}

		// Token: 0x04001199 RID: 4505
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x0400119A RID: 4506
		public bool hasBackground;

		// Token: 0x0400119B RID: 4507
		public bool useRegularBackground;

		// Token: 0x0400119C RID: 4508
		public bool overrideColors;

		// Token: 0x0400119D RID: 4509
		[Header("Resources")]
		public Image bar;

		// Token: 0x0400119E RID: 4510
		[HideInInspector]
		public Image background;
	}
}
