using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x0200023C RID: 572
	[RequireComponent(typeof(Slider))]
	public class SliderManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06000F95 RID: 3989 RVA: 0x000679E4 File Offset: 0x00065BE4
		private void Awake()
		{
			if (this.enableSaving)
			{
				if (!PlayerPrefs.HasKey(this.sliderTag + "MUIPSliderValue"))
				{
					this.saveValue = this.mainSlider.value;
				}
				else
				{
					this.saveValue = PlayerPrefs.GetFloat(this.sliderTag + "MUIPSliderValue");
				}
				this.mainSlider.value = this.saveValue;
				this.mainSlider.onValueChanged.AddListener(delegate
				{
					this.saveValue = this.mainSlider.value;
					PlayerPrefs.SetFloat(this.sliderTag + "MUIPSliderValue", this.saveValue);
				});
			}
			this.mainSlider.onValueChanged.AddListener(delegate
			{
				this.sliderEvent.Invoke(this.mainSlider.value);
				this.UpdateUI();
			});
			if (this.sliderAnimator == null && this.showPopupValue)
			{
				try
				{
					this.sliderAnimator = base.gameObject.GetComponent<Animator>();
				}
				catch
				{
					this.showPopupValue = false;
				}
			}
			if (this.invokeOnAwake)
			{
				this.sliderEvent.Invoke(this.mainSlider.value);
			}
			this.UpdateUI();
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00067AF0 File Offset: 0x00065CF0
		public void UpdateUI()
		{
			if (this.useRoundValue)
			{
				if (this.usePercent)
				{
					if (this.valueText != null)
					{
						this.valueText.text = Mathf.Round(this.mainSlider.value * 1f).ToString() + "%";
					}
					if (this.popupValueText != null)
					{
						this.popupValueText.text = Mathf.Round(this.mainSlider.value * 1f).ToString() + "%";
						return;
					}
				}
				else
				{
					if (this.valueText != null)
					{
						this.valueText.text = Mathf.Round(this.mainSlider.value * 1f).ToString();
					}
					if (this.popupValueText != null)
					{
						this.popupValueText.text = Mathf.Round(this.mainSlider.value * 1f).ToString();
						return;
					}
				}
			}
			else if (this.usePercent)
			{
				if (this.valueText != null)
				{
					this.valueText.text = this.mainSlider.value.ToString(this.decimalFormat.ToString()) + "%";
				}
				if (this.popupValueText != null)
				{
					this.popupValueText.text = this.mainSlider.value.ToString(this.decimalFormat.ToString()) + "%";
					return;
				}
			}
			else
			{
				if (this.valueText != null)
				{
					this.valueText.text = this.mainSlider.value.ToString(this.decimalFormat.ToString());
				}
				if (this.popupValueText != null)
				{
					this.popupValueText.text = this.mainSlider.value.ToString(this.decimalFormat.ToString());
				}
			}
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x00067D22 File Offset: 0x00065F22
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.showPopupValue && this.sliderAnimator != null)
			{
				this.sliderAnimator.Play("Value In");
			}
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00067D4A File Offset: 0x00065F4A
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.showPopupValue && this.sliderAnimator != null)
			{
				this.sliderAnimator.Play("Value Out");
			}
		}

		// Token: 0x04001088 RID: 4232
		public Slider mainSlider;

		// Token: 0x04001089 RID: 4233
		public TextMeshProUGUI valueText;

		// Token: 0x0400108A RID: 4234
		public TextMeshProUGUI popupValueText;

		// Token: 0x0400108B RID: 4235
		public bool enableSaving;

		// Token: 0x0400108C RID: 4236
		public bool invokeOnAwake = true;

		// Token: 0x0400108D RID: 4237
		public string sliderTag = "My Slider";

		// Token: 0x0400108E RID: 4238
		public bool usePercent;

		// Token: 0x0400108F RID: 4239
		public bool showValue = true;

		// Token: 0x04001090 RID: 4240
		public bool showPopupValue = true;

		// Token: 0x04001091 RID: 4241
		public bool useRoundValue;

		// Token: 0x04001092 RID: 4242
		public SliderManager.DecimalFormat decimalFormat = SliderManager.DecimalFormat.F1;

		// Token: 0x04001093 RID: 4243
		public float minValue;

		// Token: 0x04001094 RID: 4244
		public float maxValue;

		// Token: 0x04001095 RID: 4245
		[SerializeField]
		public SliderManager.SliderEvent onValueChanged = new SliderManager.SliderEvent();

		// Token: 0x04001096 RID: 4246
		[Space(8f)]
		public SliderManager.SliderEvent sliderEvent;

		// Token: 0x04001097 RID: 4247
		[HideInInspector]
		public Animator sliderAnimator;

		// Token: 0x04001098 RID: 4248
		[HideInInspector]
		public float saveValue;

		// Token: 0x0200023D RID: 573
		[Serializable]
		public class SliderEvent : UnityEvent<float>
		{
		}

		// Token: 0x0200023E RID: 574
		public enum DecimalFormat
		{
			// Token: 0x0400109A RID: 4250
			F0,
			// Token: 0x0400109B RID: 4251
			F1,
			// Token: 0x0400109C RID: 4252
			F2,
			// Token: 0x0400109D RID: 4253
			F3,
			// Token: 0x0400109E RID: 4254
			F4,
			// Token: 0x0400109F RID: 4255
			F5
		}
	}
}
