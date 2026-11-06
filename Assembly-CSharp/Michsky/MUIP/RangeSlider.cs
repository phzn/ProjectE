using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Michsky.MUIP
{
	// Token: 0x0200023A RID: 570
	public class RangeSlider : MonoBehaviour
	{
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x00067565 File Offset: 0x00065765
		public float CurrentLowerValue
		{
			get
			{
				return this.minSlider.value;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x00067572 File Offset: 0x00065772
		public float CurrentUpperValue
		{
			get
			{
				return this.maxSlider.realValue;
			}
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00067580 File Offset: 0x00065780
		private void Awake()
		{
			if (this.minSlider == null || this.maxSlider == null)
			{
				return;
			}
			if (this.showLabels)
			{
				this.minSlider.label = this.minSliderLabel;
				this.minSlider.numberFormat = "n" + this.decimalPlaces.ToString();
				this.maxSlider.label = this.maxSliderLabel;
				this.maxSlider.numberFormat = "n" + this.decimalPlaces.ToString();
			}
			else
			{
				this.minSliderLabel.gameObject.SetActive(false);
				this.maxSliderLabel.gameObject.SetActive(false);
			}
			if (this.useWholeNumbers)
			{
				this.minSlider.wholeNumbers = true;
				this.maxSlider.wholeNumbers = true;
			}
			this.minSlider.minValue = this.minValue;
			this.minSlider.maxValue = this.maxValue;
			this.minSlider.onValueChanged.AddListener(new UnityAction<float>(this.CheckForMinState));
			this.maxSlider.minValue = this.minValue;
			this.maxSlider.maxValue = this.maxValue;
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x000676B8 File Offset: 0x000658B8
		public void CheckForMinState(float value)
		{
			if (this.minSlider.value >= this.maxSlider.realValue)
			{
				this.maxSlider.realValue = this.minSlider.value;
				this.minSlider.value = this.maxSlider.realValue - 1f;
			}
		}

		// Token: 0x0400107A RID: 4218
		[Header("Settings")]
		[Range(0f, 2f)]
		public int decimalPlaces;

		// Token: 0x0400107B RID: 4219
		public float minValue;

		// Token: 0x0400107C RID: 4220
		public float maxValue = 1f;

		// Token: 0x0400107D RID: 4221
		public bool showLabels = true;

		// Token: 0x0400107E RID: 4222
		public bool useWholeNumbers = true;

		// Token: 0x0400107F RID: 4223
		[Header("Min Slider")]
		public RangeMinSlider minSlider;

		// Token: 0x04001080 RID: 4224
		public TextMeshProUGUI minSliderLabel;

		// Token: 0x04001081 RID: 4225
		[Header("Max Slider")]
		public RangeMaxSlider maxSlider;

		// Token: 0x04001082 RID: 4226
		public TextMeshProUGUI maxSliderLabel;
	}
}
