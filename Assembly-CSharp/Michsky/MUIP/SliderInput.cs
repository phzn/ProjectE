using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Michsky.MUIP
{
	// Token: 0x0200023B RID: 571
	[RequireComponent(typeof(CustomInputField))]
	public class SliderInput : MonoBehaviour
	{
		// Token: 0x06000F90 RID: 3984 RVA: 0x00067730 File Offset: 0x00065930
		private void Awake()
		{
			this.muipField = base.GetComponent<CustomInputField>();
			this.inputField = this.muipField.inputText;
			if (this.sliderManager == null)
			{
				Debug.LogWarning("'Slider Manager' is missing!");
				return;
			}
			if (this.sliderManager.mainSlider.wholeNumbers)
			{
				this.inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
			}
			else if (this.sliderManager.mainSlider.wholeNumbers)
			{
				this.inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
				this.decimals = 0;
			}
			this.inputField.characterLimit = this.maxChar;
			this.inputField.onDeselect.AddListener(delegate
			{
				this.SetText(this.sliderManager.mainSlider.value);
			});
			this.muipField.processSubmit = true;
			this.muipField.clearOnSubmit = false;
			this.muipField.onSubmit.AddListener(new UnityAction(this.SetValue));
			this.sliderManager.mainSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetText));
			this.sliderManager.mainSlider.onValueChanged.Invoke(this.sliderManager.mainSlider.value);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00067860 File Offset: 0x00065A60
		private void SetText(float value)
		{
			if (this.decimals == 0)
			{
				this.inputField.text = value.ToString("F0");
				return;
			}
			if (this.decimals == 1)
			{
				this.inputField.text = value.ToString("F1");
				return;
			}
			if (this.decimals == 2)
			{
				this.inputField.text = value.ToString("F2");
				return;
			}
			if (this.decimals == 3)
			{
				this.inputField.text = value.ToString("F3");
				return;
			}
			if (this.decimals == 4)
			{
				this.inputField.text = value.ToString("F4");
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00067910 File Offset: 0x00065B10
		private void SetValue()
		{
			if (this.sliderManager.mainSlider.wholeNumbers)
			{
				this.sliderManager.mainSlider.value = (float)int.Parse(this.inputField.text);
			}
			else
			{
				this.sliderManager.mainSlider.value = float.Parse(this.inputField.text);
			}
			if (float.Parse(this.inputField.text) > this.sliderManager.mainSlider.maxValue)
			{
				this.sliderManager.mainSlider.value = this.sliderManager.mainSlider.maxValue;
			}
		}

		// Token: 0x04001083 RID: 4227
		[Header("Resources")]
		[SerializeField]
		private SliderManager sliderManager;

		// Token: 0x04001084 RID: 4228
		private CustomInputField muipField;

		// Token: 0x04001085 RID: 4229
		private TMP_InputField inputField;

		// Token: 0x04001086 RID: 4230
		[Header("Settings")]
		[Range(1f, 10f)]
		public int maxChar = 5;

		// Token: 0x04001087 RID: 4231
		[Range(0f, 4f)]
		public int decimals = 1;
	}
}
