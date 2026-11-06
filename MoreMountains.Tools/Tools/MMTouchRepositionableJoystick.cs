using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
	// Token: 0x02000193 RID: 403
	[AddComponentMenu("More Mountains/Tools/Controls/MMTouchRepositionableJoystick")]
	public class MMTouchRepositionableJoystick : MMTouchJoystick, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x06000996 RID: 2454 RVA: 0x0002C1FB File Offset: 0x0002A3FB
		protected override void Start()
		{
			base.Start();
			this._rectTransform = base.GetComponent<RectTransform>();
			this._initialPosition = this.BackgroundCanvasGroup.GetComponent<RectTransform>().position;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0002C225 File Offset: 0x0002A425
		public override void Initialize()
		{
			base.Initialize();
			this.SetKnobTransform(this.KnobCanvasGroup.transform);
			this._canvasGroup = this.KnobCanvasGroup;
			this._initialOpacity = this._canvasGroup.alpha;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0002C25C File Offset: 0x0002A45C
		public override void OnPointerDown(PointerEventData data)
		{
			base.OnPointerDown(data);
			if (this.ParentCanvasRenderMode == RenderMode.ScreenSpaceCamera)
			{
				this._newPosition = this.TargetCamera.ScreenToWorldPoint(data.position);
			}
			else
			{
				this._newPosition = data.position;
			}
			this._newPosition.z = base.transform.position.z;
			if (!this.WithinBounds())
			{
				return;
			}
			this.BackgroundCanvasGroup.transform.position = this._newPosition;
			this.SetNeutralPosition(this._newPosition);
			this._knobTransform.position = this._newPosition;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002C2FF File Offset: 0x0002A4FF
		protected virtual bool WithinBounds()
		{
			return !this.ConstrainToInitialRectangle || RectTransformUtility.RectangleContainsScreenPoint(this._rectTransform, this._newPosition);
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0002C321 File Offset: 0x0002A521
		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			if (this.ResetPositionToInitialOnRelease)
			{
				this.BackgroundCanvasGroup.transform.position = this._initialPosition;
				this._knobTransform.position = this._initialPosition;
			}
		}

		// Token: 0x04000B48 RID: 2888
		[MMInspectorGroup("Repositionable Joystick", true, 22, false)]
		[Tooltip("the canvas group to use as the joystick's knob")]
		public CanvasGroup KnobCanvasGroup;

		// Token: 0x04000B49 RID: 2889
		[Tooltip("the canvas group to use as the joystick's background")]
		public CanvasGroup BackgroundCanvasGroup;

		// Token: 0x04000B4A RID: 2890
		[Tooltip("if this is true, the joystick won't be able to travel beyond the bounds of the top level canvas")]
		public bool ConstrainToInitialRectangle = true;

		// Token: 0x04000B4B RID: 2891
		[Tooltip("if this is true, the joystick will return back to its initial position when released")]
		public bool ResetPositionToInitialOnRelease;

		// Token: 0x04000B4C RID: 2892
		protected Vector3 _initialPosition;

		// Token: 0x04000B4D RID: 2893
		protected Vector3 _newPosition;

		// Token: 0x04000B4E RID: 2894
		protected CanvasGroup _knobCanvasGroup;

		// Token: 0x04000B4F RID: 2895
		protected RectTransform _rectTransform;
	}
}
