using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000238 RID: 568
	public class RangeMaxSlider : Slider
	{
		// Token: 0x06000F84 RID: 3972 RVA: 0x000673D0 File Offset: 0x000655D0
		protected override void Start()
		{
			this.realValue = base.maxValue;
			base.Start();
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x000673E4 File Offset: 0x000655E4
		protected override void Set(float input, bool sendCallback)
		{
			if (this.minSlider == null)
			{
				this.minSlider = base.transform.parent.Find("Min Slider").GetComponent<RangeMinSlider>();
			}
			if (!this.assignedRealValue)
			{
				this.realValue = base.maxValue;
				this.assignedRealValue = true;
			}
			else
			{
				this.realValue = base.maxValue - input + base.minValue;
			}
			if (base.wholeNumbers)
			{
				this.realValue = Mathf.Round(this.realValue);
			}
			if (this.realValue <= this.minSlider.value)
			{
				return;
			}
			if (this.label != null)
			{
				this.label.text = this.realValue.ToString(this.numberFormat);
			}
			base.Set(input, sendCallback);
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x000674B0 File Offset: 0x000656B0
		public void Refresh(float input)
		{
			this.Set(input, false);
		}

		// Token: 0x04001072 RID: 4210
		public RangeMinSlider minSlider;

		// Token: 0x04001073 RID: 4211
		public TextMeshProUGUI label;

		// Token: 0x04001074 RID: 4212
		public string numberFormat;

		// Token: 0x04001075 RID: 4213
		public float realValue;

		// Token: 0x04001076 RID: 4214
		private bool assignedRealValue;
	}
}
