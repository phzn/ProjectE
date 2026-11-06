using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000034 RID: 52
	public class MMDebugMenuSliderEventListener : MonoBehaviour
	{
		// Token: 0x06000115 RID: 277 RVA: 0x00005ABB File Offset: 0x00003CBB
		protected virtual void TestSetValue()
		{
			MMDebugMenuSliderEvent.Trigger(this.SliderEventName, this.TestValue, MMDebugMenuSliderEvent.EventModes.SetSlider);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005ACF File Offset: 0x00003CCF
		protected virtual void OnMMDebugMenuSliderEvent(string sliderEventName, float value, MMDebugMenuSliderEvent.EventModes eventMode)
		{
			if (eventMode == MMDebugMenuSliderEvent.EventModes.FromSlider && sliderEventName == this.SliderEventName && this.MMDValueChangedEvent != null)
			{
				this.MMDValueChangedEvent.Invoke(value);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005AF6 File Offset: 0x00003CF6
		public virtual void OnEnable()
		{
			MMDebugMenuSliderEvent.Register(new MMDebugMenuSliderEvent.Delegate(this.OnMMDebugMenuSliderEvent));
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005B0A File Offset: 0x00003D0A
		public virtual void OnDisable()
		{
			MMDebugMenuSliderEvent.Unregister(new MMDebugMenuSliderEvent.Delegate(this.OnMMDebugMenuSliderEvent));
		}

		// Token: 0x0400013D RID: 317
		[Header("Events")]
		public string SliderEventName = "SliderEventName";

		// Token: 0x0400013E RID: 318
		public MMDSliderValueChangedEvent MMDValueChangedEvent;

		// Token: 0x0400013F RID: 319
		[Header("Test")]
		[Range(0f, 1f)]
		public float TestValue = 1f;

		// Token: 0x04000140 RID: 320
		[MMInspectorButton("TestSetValue")]
		public bool TestSetValueButton;
	}
}
