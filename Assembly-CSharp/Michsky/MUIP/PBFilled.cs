using System;
using TMPro;
using UnityEngine;

namespace Michsky.MUIP
{
	// Token: 0x0200022D RID: 557
	public class PBFilled : MonoBehaviour
	{
		// Token: 0x06000F48 RID: 3912 RVA: 0x00065B70 File Offset: 0x00063D70
		private void Start()
		{
			this.progressBar = base.gameObject.GetComponent<ProgressBar>();
			this.barAnimatior = base.gameObject.GetComponent<Animator>();
			this.minLabel.color = this.minColor;
			this.maxLabel.color = this.maxColor;
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00065BC4 File Offset: 0x00063DC4
		private void Update()
		{
			if (this.progressBar.currentPercent >= (float)this.transitionAfter)
			{
				this.barAnimatior.Play("Radial PB Filled");
			}
			if (this.progressBar.currentPercent <= (float)this.transitionAfter)
			{
				this.barAnimatior.Play("Radial PB Empty");
			}
			this.maxLabel.text = this.minLabel.text;
		}

		// Token: 0x04001028 RID: 4136
		[Header("Resources")]
		public TextMeshProUGUI minLabel;

		// Token: 0x04001029 RID: 4137
		public TextMeshProUGUI maxLabel;

		// Token: 0x0400102A RID: 4138
		[Header("Settings")]
		[Range(0f, 100f)]
		public int transitionAfter = 50;

		// Token: 0x0400102B RID: 4139
		public Color minColor = new Color(0f, 0f, 0f, 255f);

		// Token: 0x0400102C RID: 4140
		public Color maxColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400102D RID: 4141
		private ProgressBar progressBar;

		// Token: 0x0400102E RID: 4142
		private Animator barAnimatior;
	}
}
