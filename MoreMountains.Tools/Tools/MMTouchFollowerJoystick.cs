using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
	// Token: 0x0200018F RID: 399
	[AddComponentMenu("More Mountains/Tools/Controls/MMTouchFollowerJoystick")]
	public class MMTouchFollowerJoystick : MMTouchJoystick
	{
		// Token: 0x06000972 RID: 2418 RVA: 0x0002B63C File Offset: 0x0002983C
		protected override void Start()
		{
			base.Start();
			this._rectTransform = base.GetComponent<RectTransform>();
			this._backgroundRectTransform = this.BackgroundCanvasGroup.GetComponent<RectTransform>();
			this._initialPosition = this._backgroundRectTransform.position;
			this._backgroundPositionTarget = this._initialPosition;
			this.CreateInnerRect();
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0002B68F File Offset: 0x0002988F
		public override void Initialize()
		{
			base.Initialize();
			this.SetKnobTransform(this.KnobCanvasGroup.transform);
			this._canvasGroup = this.KnobCanvasGroup;
			this._initialOpacity = this._canvasGroup.alpha;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0002B6C5 File Offset: 0x000298C5
		protected override void Update()
		{
			base.Update();
			this.HandleMovementInterpolation();
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0002B6D4 File Offset: 0x000298D4
		protected virtual void HandleMovementInterpolation()
		{
			if (!this.InterpolateFollowMovement)
			{
				this.BackgroundCanvasGroup.transform.position = this._backgroundPositionTarget;
				return;
			}
			if (this.SpringFollowInterpolation)
			{
				this._newBackgroundPosition = this.BackgroundCanvasGroup.transform.position;
				MMMaths.Spring(ref this._newBackgroundPosition, this._backgroundPositionTarget, ref this._springVelocity, this.SpringDamping, this.SpringFrequency, Time.unscaledDeltaTime);
				this.BackgroundCanvasGroup.transform.position = this._newBackgroundPosition;
				return;
			}
			this.BackgroundCanvasGroup.transform.position = MMMaths.Lerp(this.BackgroundCanvasGroup.transform.position, this._backgroundPositionTarget, this.InterpolateFollowMovementSpeed, Time.unscaledDeltaTime);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0002B794 File Offset: 0x00029994
		protected virtual void CreateInnerRect()
		{
			if (!this.ShouldConstrainBackground)
			{
				return;
			}
			if (this.BackgroundConstraintRectTransform == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.transform.SetParent(base.transform);
				gameObject.name = "BackgroundConstraintRectTransform";
				this.BackgroundConstraintRectTransform = gameObject.AddComponent<RectTransform>();
				this.BackgroundConstraintRectTransform.anchorMin = this._rectTransform.anchorMin;
				this.BackgroundConstraintRectTransform.anchorMax = this._rectTransform.anchorMax;
				this.BackgroundConstraintRectTransform.position = this._rectTransform.position;
				this.BackgroundConstraintRectTransform.localScale = this._rectTransform.localScale;
				this.BackgroundConstraintRectTransform.sizeDelta = new Vector2(this._rectTransform.sizeDelta.x - this._backgroundRectTransform.sizeDelta.y, this._rectTransform.sizeDelta.y - this._backgroundRectTransform.sizeDelta.y);
			}
			this.BackgroundConstraintRectTransform.offsetMin += new Vector2(this.BackgroundConstraintPaddingLeft, this.BackgroundConstraintPaddingBottom);
			this.BackgroundConstraintRectTransform.offsetMax -= new Vector2(this.BackgroundConstraintPaddingRight, this.BackgroundConstraintPaddingTop);
			this.BackgroundConstraintRectTransform.GetWorldCorners(this._innerRectCorners);
			this._innerRectTransformBottomLeft = this._innerRectCorners[0];
			this._innerRectTransformTopLeft = this._innerRectCorners[1];
			this._innerRectTransformTopRight = this._innerRectCorners[2];
			this._innerRectTransformBottomRight = this._innerRectCorners[3];
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0002B93C File Offset: 0x00029B3C
		public override void OnPointerDown(PointerEventData data)
		{
			base.OnPointerDown(data);
			this._newPosition = this.ConvertToWorld(data.position);
			this._newPosition.z = base.transform.position.z;
			this._backgroundPositionTarget = this._newPosition;
			this.ConstrainBackground();
			this.SetNeutralPosition(this.BackgroundCanvasGroup.transform.position);
			this._knobTransform.position = this._newPosition;
			this.ComputeJoystickValue();
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0002B9C4 File Offset: 0x00029BC4
		public override void OnDrag(PointerEventData eventData)
		{
			base.OnDrag(eventData);
			float num = Vector2.Distance(this._knobTransform.position, this.BackgroundCanvasGroup.transform.position);
			if (num >= base.ComputedMaxRange)
			{
				this._backgroundPositionTarget = this.BackgroundCanvasGroup.transform.position + (this._knobTransform.position - this.BackgroundCanvasGroup.transform.position).normalized * (num - base.ComputedMaxRange);
			}
			this.ConstrainBackground();
			this.ComputeJoystickValue();
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0002BA68 File Offset: 0x00029C68
		protected virtual void ComputeJoystickValue()
		{
			float num = Vector2.Distance(this._knobTransform.position, this.BackgroundCanvasGroup.transform.position);
			if (num <= base.ComputedMaxRange)
			{
				this.RawValue.x = this.EvaluateInputValue(this._knobTransform.position.x - this.BackgroundCanvasGroup.transform.position.x);
				this.RawValue.y = this.EvaluateInputValue(this._knobTransform.position.y - this.BackgroundCanvasGroup.transform.position.y);
				return;
			}
			float num2 = this._knobTransform.position.x - this.BackgroundCanvasGroup.transform.position.x;
			this.RawValue.x = Mathf.InverseLerp(0f, num, Mathf.Abs(num2)) * Mathf.Sign(num2);
			num2 = this._knobTransform.position.y - this.BackgroundCanvasGroup.transform.position.y;
			this.RawValue.y = Mathf.InverseLerp(0f, num, Mathf.Abs(num2)) * Mathf.Sign(num2);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0002BBAC File Offset: 0x00029DAC
		protected virtual void ConstrainBackground()
		{
			if (!this.ShouldConstrainBackground)
			{
				return;
			}
			this._newBackgroundPosition = this._backgroundPositionTarget;
			this._newBackgroundPosition.x = Mathf.Clamp(this._newBackgroundPosition.x, this._innerRectTransformTopLeft.x, this._innerRectTransformTopRight.x);
			this._newBackgroundPosition.y = Mathf.Clamp(this._newBackgroundPosition.y, this._innerRectTransformBottomLeft.y, this._innerRectTransformTopLeft.y);
			this._backgroundPositionTarget = this._newBackgroundPosition;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0002BC3C File Offset: 0x00029E3C
		public override void OnPointerUp(PointerEventData data)
		{
			base.OnPointerUp(data);
			this.ResetJoystick();
			this._knobTransform.position = this._backgroundPositionTarget;
			if (this.ResetPositionToInitialOnRelease)
			{
				this._backgroundPositionTarget = this._initialPosition;
				this._knobTransform.position = this._initialPosition;
			}
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0002BC8C File Offset: 0x00029E8C
		protected override void ClampToBounds()
		{
			this._newTargetPosition -= this._neutralPosition;
		}

		// Token: 0x04000B0E RID: 2830
		[MMInspectorGroup("Follower Joystick", true, 23, false)]
		[Tooltip("the canvas group to use as the joystick's knob - the part that moves under your thumb")]
		public CanvasGroup KnobCanvasGroup;

		// Token: 0x04000B0F RID: 2831
		[Tooltip("the canvas group to use as the joystick's background")]
		public CanvasGroup BackgroundCanvasGroup;

		// Token: 0x04000B10 RID: 2832
		[Tooltip("if this is true, the joystick will return back to its initial position when released")]
		public bool ResetPositionToInitialOnRelease;

		// Token: 0x04000B11 RID: 2833
		[Tooltip("if this is true, the background will follow its target with interpolation, otherwise it'll be instant movement")]
		public bool InterpolateFollowMovement;

		// Token: 0x04000B12 RID: 2834
		[Tooltip("if in interpolate mode, this defines the speed at which the backgrounds follows the knob")]
		[MMCondition("InterpolateFollowMovement", true)]
		public float InterpolateFollowMovementSpeed = 0.3f;

		// Token: 0x04000B13 RID: 2835
		[Tooltip("whether or not to add a spring to the interpolation of the background movement")]
		[MMCondition("InterpolateFollowMovement", true)]
		public bool SpringFollowInterpolation;

		// Token: 0x04000B14 RID: 2836
		[Tooltip("when in SpringFollowInterpolation mode, the amount of damping to apply to the spring")]
		[MMCondition("SpringFollowInterpolation", true)]
		public float SpringDamping = 0.6f;

		// Token: 0x04000B15 RID: 2837
		[Tooltip("when in SpringFollowInterpolation mode, the frequency to apply to the spring")]
		[MMCondition("SpringFollowInterpolation", true)]
		public float SpringFrequency = 4f;

		// Token: 0x04000B16 RID: 2838
		[MMInspectorGroup("Background Constraints", true, 24, false)]
		[Tooltip("if this is true, the joystick won't be able to travel beyond the bounds of the top level canvas")]
		public bool ShouldConstrainBackground = true;

		// Token: 0x04000B17 RID: 2839
		[Tooltip("the rect to consider as a background constraint zone, if left empty, will be auto created")]
		public RectTransform BackgroundConstraintRectTransform;

		// Token: 0x04000B18 RID: 2840
		[Tooltip("the left padding to apply to the background constraint")]
		public float BackgroundConstraintPaddingLeft;

		// Token: 0x04000B19 RID: 2841
		[Tooltip("the right padding to apply to the background constraint")]
		public float BackgroundConstraintPaddingRight;

		// Token: 0x04000B1A RID: 2842
		[Tooltip("the top padding to apply to the background constraint")]
		public float BackgroundConstraintPaddingTop;

		// Token: 0x04000B1B RID: 2843
		[Tooltip("the bottom padding to apply to the background constraint")]
		public float BackgroundConstraintPaddingBottom;

		// Token: 0x04000B1C RID: 2844
		protected Vector3 _initialPosition;

		// Token: 0x04000B1D RID: 2845
		protected Vector3 _newPosition;

		// Token: 0x04000B1E RID: 2846
		protected RectTransform _rectTransform;

		// Token: 0x04000B1F RID: 2847
		protected RectTransform _backgroundRectTransform;

		// Token: 0x04000B20 RID: 2848
		protected Vector3[] _innerRectCorners = new Vector3[4];

		// Token: 0x04000B21 RID: 2849
		protected Vector3 _newBackgroundPosition;

		// Token: 0x04000B22 RID: 2850
		protected Vector3 _backgroundPositionTarget;

		// Token: 0x04000B23 RID: 2851
		protected Vector3 _innerRectTransformBottomLeft;

		// Token: 0x04000B24 RID: 2852
		protected Vector3 _innerRectTransformTopLeft;

		// Token: 0x04000B25 RID: 2853
		protected Vector3 _innerRectTransformTopRight;

		// Token: 0x04000B26 RID: 2854
		protected Vector3 _innerRectTransformBottomRight;

		// Token: 0x04000B27 RID: 2855
		protected Vector3 _springVelocity;
	}
}
