using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x0200003E RID: 62
	public class MMDebugMenuItemSlider : MonoBehaviour
	{
		// Token: 0x0600013D RID: 317 RVA: 0x00006EAD File Offset: 0x000050AD
		protected virtual void Awake()
		{
			this.TargetSlider.onValueChanged.AddListener(delegate
			{
				this.ValueChangeCheck();
			});
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006ECC File Offset: 0x000050CC
		public void ValueChangeCheck()
		{
			if (this._valueSetThisFrame)
			{
				this._valueSetThisFrame = false;
				return;
			}
			bool flag = true;
			this.SliderValue = MMMaths.Remap(this.TargetSlider.value, 0f, 1f, this.RemapZero, this.RemapOne);
			if (this.Mode == MMDebugMenuItemSlider.Modes.Int)
			{
				this.SliderValue = Mathf.Round(this.SliderValue);
				if (this.SliderValue == (float)this.SliderValueInt)
				{
					flag = false;
				}
				this.SliderValueInt = (int)this.SliderValue;
			}
			if (flag)
			{
				this.UpdateValue(this.SliderValue);
			}
			this.TriggerSliderEvent(this.SliderValue);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006F6A File Offset: 0x0000516A
		protected virtual void UpdateValue(float newValue)
		{
			this.SliderValueText.text = ((this.Mode == MMDebugMenuItemSlider.Modes.Int) ? newValue.ToString() : newValue.ToString("F3"));
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006F95 File Offset: 0x00005195
		protected virtual void TriggerSliderEvent(float value)
		{
			MMDebugMenuSliderEvent.Trigger(this.SliderEventName, value, MMDebugMenuSliderEvent.EventModes.FromSlider);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006FA4 File Offset: 0x000051A4
		protected virtual void OnMMDebugMenuSliderEvent(string sliderEventName, float value, MMDebugMenuSliderEvent.EventModes eventMode)
		{
			if (eventMode == MMDebugMenuSliderEvent.EventModes.SetSlider && sliderEventName == this.SliderEventName)
			{
				this._valueSetThisFrame = true;
				this.TargetSlider.value = MMMaths.Remap(value, this.RemapZero, this.RemapOne, 0f, 1f);
				this.UpdateValue(value);
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006FF8 File Offset: 0x000051F8
		public virtual void OnEnable()
		{
			if (!this._listening)
			{
				MMDebugMenuSliderEvent.Register(new MMDebugMenuSliderEvent.Delegate(this.OnMMDebugMenuSliderEvent));
				this._listening = true;
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000701B File Offset: 0x0000521B
		public virtual void OnDestroy()
		{
			this._listening = false;
			MMDebugMenuSliderEvent.Unregister(new MMDebugMenuSliderEvent.Delegate(this.OnMMDebugMenuSliderEvent));
		}

		// Token: 0x040001AA RID: 426
		[Header("Bindings")]
		public MMDebugMenuItemSlider.Modes Mode;

		// Token: 0x040001AB RID: 427
		public Slider TargetSlider;

		// Token: 0x040001AC RID: 428
		public Text SliderText;

		// Token: 0x040001AD RID: 429
		public Text SliderValueText;

		// Token: 0x040001AE RID: 430
		public Image SliderKnob;

		// Token: 0x040001AF RID: 431
		public Image SliderLine;

		// Token: 0x040001B0 RID: 432
		public float RemapZero;

		// Token: 0x040001B1 RID: 433
		public float RemapOne = 1f;

		// Token: 0x040001B2 RID: 434
		public string SliderEventName = "Checkbox";

		// Token: 0x040001B3 RID: 435
		[MMReadOnly]
		public float SliderValue;

		// Token: 0x040001B4 RID: 436
		[MMReadOnly]
		public int SliderValueInt;

		// Token: 0x040001B5 RID: 437
		protected bool _valueSetThisFrame;

		// Token: 0x040001B6 RID: 438
		protected bool _listening;

		// Token: 0x020003B7 RID: 951
		public enum Modes
		{
			// Token: 0x04001D9E RID: 7582
			Float,
			// Token: 0x04001D9F RID: 7583
			Int
		}
	}
}
