using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000057 RID: 87
	[AddComponentMenu("More Mountains/Tools/GUI/MMSliderStep")]
	[RequireComponent(typeof(Slider))]
	public class MMSliderStep : MonoBehaviour
	{
		// Token: 0x060001DE RID: 478 RVA: 0x000093FF File Offset: 0x000075FF
		protected virtual void OnEnable()
		{
			this._slider = base.gameObject.GetComponent<Slider>();
			this._slider.onValueChanged.AddListener(new UnityAction<float>(this.ValueChangeCheck));
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000942F File Offset: 0x0000762F
		protected virtual void OnDisable()
		{
			this._slider.onValueChanged.RemoveListener(new UnityAction<float>(this.ValueChangeCheck));
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009450 File Offset: 0x00007650
		public virtual void ValueChangeCheck(float value)
		{
			if (Mathf.Abs(this._slider.value - this._lastStep) > this.StepThreshold)
			{
				this._lastStep = this._slider.value;
				UnityEvent onStep = this.OnStep;
				if (onStep == null)
				{
					return;
				}
				onStep.Invoke();
			}
		}

		// Token: 0x04000286 RID: 646
		[Header("Slider Step")]
		public float StepThreshold = 0.1f;

		// Token: 0x04000287 RID: 647
		public UnityEvent OnStep;

		// Token: 0x04000288 RID: 648
		protected Slider _slider;

		// Token: 0x04000289 RID: 649
		protected float _lastStep;
	}
}
