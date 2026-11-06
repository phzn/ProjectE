using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
	// Token: 0x0200018C RID: 396
	[RequireComponent(typeof(Rect))]
	[RequireComponent(typeof(CanvasGroup))]
	[AddComponentMenu("More Mountains/Tools/Controls/MMTouchAxis")]
	public class MMTouchAxis : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x0002ACFC File Offset: 0x00028EFC
		// (set) Token: 0x06000947 RID: 2375 RVA: 0x0002AD04 File Offset: 0x00028F04
		public virtual MMTouchAxis.ButtonStates CurrentState { get; protected set; }

		// Token: 0x06000948 RID: 2376 RVA: 0x0002AD0D File Offset: 0x00028F0D
		protected virtual void Awake()
		{
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			if (this._canvasGroup != null)
			{
				this._initialOpacity = this._canvasGroup.alpha;
			}
			this.ResetButton();
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0002AD40 File Offset: 0x00028F40
		protected virtual void Update()
		{
			if (this.AxisPressed != null && this.CurrentState == MMTouchAxis.ButtonStates.ButtonPressed)
			{
				this.AxisPressed.Invoke(this.AxisValue);
			}
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0002AD64 File Offset: 0x00028F64
		protected virtual void LateUpdate()
		{
			if (this.CurrentState == MMTouchAxis.ButtonStates.ButtonUp)
			{
				this.CurrentState = MMTouchAxis.ButtonStates.Off;
			}
			if (this.CurrentState == MMTouchAxis.ButtonStates.ButtonDown)
			{
				this.CurrentState = MMTouchAxis.ButtonStates.ButtonPressed;
			}
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0002AD88 File Offset: 0x00028F88
		public virtual void OnPointerDown(PointerEventData data)
		{
			if (this.CurrentState != MMTouchAxis.ButtonStates.Off)
			{
				return;
			}
			this.CurrentState = MMTouchAxis.ButtonStates.ButtonDown;
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = this.PressedOpacity;
			}
			if (this.AxisPressedFirstTime != null)
			{
				this.AxisPressedFirstTime.Invoke();
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002ADD8 File Offset: 0x00028FD8
		public virtual void OnPointerUp(PointerEventData data)
		{
			if (this.CurrentState != MMTouchAxis.ButtonStates.ButtonPressed && this.CurrentState != MMTouchAxis.ButtonStates.ButtonDown)
			{
				return;
			}
			this.CurrentState = MMTouchAxis.ButtonStates.ButtonUp;
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = this._initialOpacity;
			}
			if (this.AxisReleased != null)
			{
				this.AxisReleased.Invoke();
			}
			this.AxisPressed.Invoke(0f);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002AE41 File Offset: 0x00029041
		protected virtual void OnEnable()
		{
			this.ResetButton();
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002AE49 File Offset: 0x00029049
		protected virtual void ResetButton()
		{
			this.CurrentState = MMTouchAxis.ButtonStates.Off;
			this._canvasGroup.alpha = this._initialOpacity;
			this.CurrentState = MMTouchAxis.ButtonStates.Off;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0002AE6A File Offset: 0x0002906A
		public virtual void OnPointerEnter(PointerEventData data)
		{
			if (!this.MouseMode)
			{
				this.OnPointerDown(data);
			}
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0002AE7B File Offset: 0x0002907B
		public virtual void OnPointerExit(PointerEventData data)
		{
			if (!this.MouseMode)
			{
				this.OnPointerUp(data);
			}
		}

		// Token: 0x04000ADC RID: 2780
		[Header("Binding")]
		[Tooltip("The method(s) to call when the axis gets pressed down")]
		public UnityEvent AxisPressedFirstTime;

		// Token: 0x04000ADD RID: 2781
		[Tooltip("The method(s) to call when the axis gets released")]
		public UnityEvent AxisReleased;

		// Token: 0x04000ADE RID: 2782
		[Tooltip("The method(s) to call while the axis is being pressed")]
		public AxisEvent AxisPressed;

		// Token: 0x04000ADF RID: 2783
		[Header("Pressed Behaviour")]
		[MMInformation("Here you can set the opacity of the button when it's pressed. Useful for visual feedback.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("the new opacity to apply to the canvas group when the axis is pressed")]
		public float PressedOpacity = 0.5f;

		// Token: 0x04000AE0 RID: 2784
		[Tooltip("the value to send the bound method when the axis is pressed")]
		public float AxisValue;

		// Token: 0x04000AE1 RID: 2785
		[Header("Mouse Mode")]
		[MMInformation("If you set this to true, you'll need to actually press the axis for it to be triggered, otherwise a simple hover will trigger it (better for touch input).", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("If you set this to true, you'll need to actually press the axis for it to be triggered, otherwise a simple hover will trigger it (better for touch input).")]
		public bool MouseMode;

		// Token: 0x04000AE3 RID: 2787
		protected CanvasGroup _canvasGroup;

		// Token: 0x04000AE4 RID: 2788
		protected float _initialOpacity;

		// Token: 0x02000475 RID: 1141
		public enum ButtonStates
		{
			// Token: 0x040020D4 RID: 8404
			Off,
			// Token: 0x040020D5 RID: 8405
			ButtonDown,
			// Token: 0x040020D6 RID: 8406
			ButtonPressed,
			// Token: 0x040020D7 RID: 8407
			ButtonUp
		}
	}
}
