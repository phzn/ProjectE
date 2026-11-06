using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000260 RID: 608
	[ExecuteInEditMode]
	public class UIManagerProgressBar : MonoBehaviour
	{
		// Token: 0x0600100C RID: 4108 RVA: 0x0006ABA8 File Offset: 0x00068DA8
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

		// Token: 0x0600100D RID: 4109 RVA: 0x0006ABF4 File Offset: 0x00068DF4
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

		// Token: 0x0600100E RID: 4110 RVA: 0x0006AC18 File Offset: 0x00068E18
		private void UpdateProgressBar()
		{
			if (!this.overrideColors)
			{
				this.bar.color = this.UIManagerAsset.progressBarColor;
				this.background.color = this.UIManagerAsset.progressBarBackgroundColor;
				this.label.color = this.UIManagerAsset.progressBarLabelColor;
			}
			if (!this.overrideFonts)
			{
				this.label.font = this.UIManagerAsset.progressBarLabelFont;
				this.label.fontSize = this.UIManagerAsset.progressBarLabelFontSize;
			}
		}

		// Token: 0x04001192 RID: 4498
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001193 RID: 4499
		[HideInInspector]
		public bool overrideColors;

		// Token: 0x04001194 RID: 4500
		[HideInInspector]
		public bool overrideFonts;

		// Token: 0x04001195 RID: 4501
		[Header("Resources")]
		[SerializeField]
		private Image bar;

		// Token: 0x04001196 RID: 4502
		[SerializeField]
		private Image background;

		// Token: 0x04001197 RID: 4503
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04001198 RID: 4504
		private bool dynamicUpdateEnabled;
	}
}
