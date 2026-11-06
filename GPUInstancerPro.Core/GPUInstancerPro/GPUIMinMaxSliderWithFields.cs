using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace GPUInstancerPro
{
	// Token: 0x0200006B RID: 107
	public class GPUIMinMaxSliderWithFields : VisualElement
	{
		// Token: 0x06000459 RID: 1113 RVA: 0x00018920 File Offset: 0x00016B20
		public GPUIMinMaxSliderWithFields(string label, float minLimit, float maxLimit, float initialMin, float initialMax, Action<Vector2> valueChangedCallback)
		{
			GPUIMinMaxSliderWithFields <>4__this = this;
			this.minMaxSlider = new MinMaxSlider(initialMin, initialMax, minLimit, maxLimit);
			this.minMaxSlider.label = label;
			this.minMaxSlider.labelElement.style.width = 170f;
			this.minMaxSlider.style.flexGrow = 1f;
			this.minMaxSlider.style.paddingRight = 5f;
			this.minMaxSlider.style.marginRight = 2f;
			base.Add(this.minMaxSlider);
			this.minField = new FloatField
			{
				value = initialMin
			};
			this.minField.style.flexGrow = 0f;
			this.minField.style.minWidth = 42f;
			this.minField.style.maxWidth = 42f;
			this.maxField = new FloatField
			{
				value = initialMax
			};
			this.maxField.style.flexGrow = 0f;
			this.maxField.style.minWidth = 42f;
			this.maxField.style.maxWidth = 42f;
			VisualElement visualElement = new VisualElement();
			visualElement.style.flexDirection = FlexDirection.Row;
			visualElement.Add(this.minMaxSlider);
			visualElement.Add(this.minField);
			visualElement.Add(this.maxField);
			base.Add(visualElement);
			this.minField.RegisterValueChangedCallback<float>(delegate(ChangeEvent<float> evt)
			{
				float num = Mathf.Clamp(evt.newValue, minLimit, <>4__this.maxField.value);
				<>4__this.minField.value = num;
				<>4__this.minMaxSlider.value = new Vector2(num, <>4__this.minMaxSlider.value.y);
				Action<Vector2> valueChangedCallback2 = valueChangedCallback;
				if (valueChangedCallback2 == null)
				{
					return;
				}
				valueChangedCallback2(<>4__this.minMaxSlider.value);
			});
			this.maxField.RegisterValueChangedCallback<float>(delegate(ChangeEvent<float> evt)
			{
				float num2 = Mathf.Clamp(evt.newValue, <>4__this.minField.value, maxLimit);
				<>4__this.maxField.value = num2;
				<>4__this.minMaxSlider.value = new Vector2(<>4__this.minMaxSlider.value.x, num2);
				Action<Vector2> valueChangedCallback3 = valueChangedCallback;
				if (valueChangedCallback3 == null)
				{
					return;
				}
				valueChangedCallback3(<>4__this.minMaxSlider.value);
			});
			this.minMaxSlider.RegisterValueChangedCallback<Vector2>(delegate(ChangeEvent<Vector2> evt)
			{
				<>4__this.minField.value = evt.newValue.x;
				<>4__this.maxField.value = evt.newValue.y;
				Action<Vector2> valueChangedCallback4 = valueChangedCallback;
				if (valueChangedCallback4 == null)
				{
					return;
				}
				valueChangedCallback4(evt.newValue);
			});
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00018B42 File Offset: 0x00016D42
		public void SetValues(float min, float max)
		{
			this.minField.value = min;
			this.maxField.value = max;
			this.minMaxSlider.value = new Vector2(min, max);
		}

		// Token: 0x04000342 RID: 834
		private MinMaxSlider minMaxSlider;

		// Token: 0x04000343 RID: 835
		private FloatField minField;

		// Token: 0x04000344 RID: 836
		private FloatField maxField;

		// Token: 0x04000345 RID: 837
		private const int floatFieldWidth = 42;
	}
}
