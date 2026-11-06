using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000239 RID: 569
	public class RangeMinSlider : Slider
	{
		// Token: 0x06000F88 RID: 3976 RVA: 0x000674C4 File Offset: 0x000656C4
		protected override void Set(float input, bool sendCallback)
		{
			if (this.maxSlider == null)
			{
				this.maxSlider = base.transform.parent.Find("Max Slider").GetComponent<RangeMaxSlider>();
			}
			float num = input;
			if (base.wholeNumbers)
			{
				num = Mathf.Round(num);
			}
			if (num >= this.maxSlider.realValue && this.maxSlider.realValue != this.maxSlider.minValue)
			{
				return;
			}
			if (this.label != null)
			{
				this.label.text = num.ToString(this.numberFormat);
			}
			base.Set(input, sendCallback);
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x000674B0 File Offset: 0x000656B0
		public void Refresh(float input)
		{
			this.Set(input, false);
		}

		// Token: 0x04001077 RID: 4215
		[Header("RESOURCES")]
		public RangeMaxSlider maxSlider;

		// Token: 0x04001078 RID: 4216
		public TextMeshProUGUI label;

		// Token: 0x04001079 RID: 4217
		public string numberFormat;
	}
}
