using System;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BoZo.CharacterCreator.MC.StyleSystem.UI
{
	// Token: 0x02000813 RID: 2067
	public class MCStyleShaderSliderRowUI : MonoBehaviour
	{
		// Token: 0x0600372D RID: 14125 RVA: 0x0013EA48 File Offset: 0x0013CC48
		public void Configure(string displayName, string propertyName, float min, float max, float step, float value, float defaultValue, Action<string, float> onValueChanged)
		{
			this._propertyName = propertyName;
			this._step = Mathf.Max(0f, step);
			this._onValueChanged = onValueChanged;
			this._defaultValue = defaultValue;
			if (this.title != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.title, "cielos_apps", displayName, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			if (this.slider != null)
			{
				this._suppress = true;
				this.slider.minValue = min;
				this.slider.maxValue = Mathf.Max(min, max);
				this.slider.wholeNumbers = this._step > 0f && Mathf.Approximately(this._step, 1f);
				this.slider.value = Mathf.Clamp(value, this.slider.minValue, this.slider.maxValue);
				this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnSliderChanged));
				this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderChanged));
				this._suppress = false;
			}
			if (this.resetButton != null)
			{
				this.resetButton.onClick.RemoveListener(new UnityAction(this.OnResetClicked));
				this.resetButton.onClick.AddListener(new UnityAction(this.OnResetClicked));
			}
			this.RefreshValueLabel(value);
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x0013EBC4 File Offset: 0x0013CDC4
		private void OnDestroy()
		{
			if (this.slider != null)
			{
				this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnSliderChanged));
			}
			if (this.resetButton != null)
			{
				this.resetButton.onClick.RemoveListener(new UnityAction(this.OnResetClicked));
			}
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x0013EC28 File Offset: 0x0013CE28
		private void OnSliderChanged(float rawValue)
		{
			if (this._suppress)
			{
				return;
			}
			float num = rawValue;
			if (this._step > 0f)
			{
				num = Mathf.Round(rawValue / this._step) * this._step;
				if (this.slider != null && !Mathf.Approximately(num, rawValue))
				{
					this._suppress = true;
					this.slider.value = num;
					this._suppress = false;
				}
			}
			this.RefreshValueLabel(num);
			Action<string, float> onValueChanged = this._onValueChanged;
			if (onValueChanged == null)
			{
				return;
			}
			onValueChanged(this._propertyName, num);
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x0013ECB4 File Offset: 0x0013CEB4
		private void OnResetClicked()
		{
			float num = this._defaultValue;
			if (this.slider != null)
			{
				num = Mathf.Clamp(this._defaultValue, this.slider.minValue, this.slider.maxValue);
				this._suppress = true;
				this.slider.value = num;
				this._suppress = false;
			}
			this.RefreshValueLabel(num);
			Action<string, float> onValueChanged = this._onValueChanged;
			if (onValueChanged == null)
			{
				return;
			}
			onValueChanged(this._propertyName, num);
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x0013ED30 File Offset: 0x0013CF30
		private void RefreshValueLabel(float value)
		{
			if (this.valueLabel != null)
			{
				LocalizedGameText.BindFontOnly(this.valueLabel);
				this.valueLabel.text = value.ToString("0.##");
			}
		}

		// Token: 0x0400374B RID: 14155
		[SerializeField]
		private TMP_Text title;

		// Token: 0x0400374C RID: 14156
		[SerializeField]
		private TMP_Text valueLabel;

		// Token: 0x0400374D RID: 14157
		[SerializeField]
		private Slider slider;

		// Token: 0x0400374E RID: 14158
		[SerializeField]
		private Button resetButton;

		// Token: 0x0400374F RID: 14159
		private string _propertyName;

		// Token: 0x04003750 RID: 14160
		private float _step;

		// Token: 0x04003751 RID: 14161
		private float _defaultValue;

		// Token: 0x04003752 RID: 14162
		private Action<string, float> _onValueChanged;

		// Token: 0x04003753 RID: 14163
		private bool _suppress;
	}
}
