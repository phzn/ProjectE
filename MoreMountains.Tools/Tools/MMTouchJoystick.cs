using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
	// Token: 0x02000192 RID: 402
	[RequireComponent(typeof(Rect))]
	[RequireComponent(typeof(CanvasGroup))]
	[AddComponentMenu("More Mountains/Tools/Controls/MMTouchJoystick")]
	public class MMTouchJoystick : MMMonoBehaviour, IDragHandler, IEventSystemHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0002BCF4 File Offset: 0x00029EF4
		public float ComputedMaxRange
		{
			get
			{
				if (Application.isPlaying)
				{
					if (this.MaxRangeMode != MMTouchJoystick.MaxRangeModes.Distance)
					{
						return this._maxRangeTransformDistance;
					}
					return this.MaxRange;
				}
				else
				{
					if (this.MaxRangeMode == MMTouchJoystick.MaxRangeModes.Distance)
					{
						return this.MaxRange;
					}
					if (this.MaxRangeTransform == null)
					{
						return -1f;
					}
					this.RefreshMaxRangeDistance();
					return this._maxRangeTransformDistance;
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x0002BD4D File Offset: 0x00029F4D
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x0002BD55 File Offset: 0x00029F55
		public virtual RenderMode ParentCanvasRenderMode { get; protected set; }

		// Token: 0x06000983 RID: 2435 RVA: 0x0002BD5E File Offset: 0x00029F5E
		protected virtual void Start()
		{
			this.Initialize();
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0002BD68 File Offset: 0x00029F68
		public virtual void Initialize()
		{
			if (this.ParentCanvasRenderMode == RenderMode.ScreenSpaceCamera && this.TargetCamera == null)
			{
				throw new Exception("MMTouchJoystick : you have to set a target camera");
			}
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			this._rotatingIndicatorIsNotNull = this.RotatingIndicator != null;
			this.RefreshMaxRangeDistance();
			this.SetKnobTransform(base.transform);
			this.SetNeutralPosition();
			this.ParentCanvasRenderMode = base.GetComponentInParent<Canvas>().renderMode;
			this._initialZPosition = this._knobTransform.position.z;
			this._initialOpacity = this._canvasGroup.alpha;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0002BE05 File Offset: 0x0002A005
		public virtual void RefreshMaxRangeDistance()
		{
			if (this.MaxRangeMode == MMTouchJoystick.MaxRangeModes.DistanceToTransform)
			{
				this._maxRangeTransformDistance = Vector2.Distance(base.transform.position, this.MaxRangeTransform.position);
			}
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0002BE3B File Offset: 0x0002A03B
		public virtual void SetKnobTransform(Transform newTransform)
		{
			this._knobTransform = newTransform;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0002BE44 File Offset: 0x0002A044
		protected virtual void Update()
		{
			this.NormalizedValue = this.RawValue.normalized;
			this.Magnitude = this.RawValue.magnitude;
			if (this.HorizontalAxisEnabled || this.VerticalAxisEnabled)
			{
				this.JoystickValue.Invoke(this.RawValue);
				this.JoystickNormalizedValue.Invoke(this.NormalizedValue);
				this.JoystickMagnitudeValue.Invoke(this.Magnitude);
			}
			this.RotateIndicator();
			this.HandleOpacity();
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0002BEC4 File Offset: 0x0002A0C4
		protected virtual void HandleOpacity()
		{
			if (this.InterpolateOpacity)
			{
				this._canvasGroup.alpha = MMMaths.Lerp(this._canvasGroup.alpha, this._targetOpacity, this.InterpolateOpacitySpeed, Time.unscaledDeltaTime);
				return;
			}
			this._canvasGroup.alpha = this._targetOpacity;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0002BF18 File Offset: 0x0002A118
		protected virtual void RotateIndicator()
		{
			if (!this._rotatingIndicatorIsNotNull)
			{
				return;
			}
			this.RotatingIndicator.gameObject.SetActive(this.RawValue.magnitude > this.RotatingIndicatorThreshold);
			float num = Mathf.Atan2(this.RawValue.y, this.RawValue.x) * 57.29578f;
			this.RotatingIndicator.localRotation = Quaternion.AngleAxis(num, Vector3.forward);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0002BF89 File Offset: 0x0002A189
		public virtual void SetNeutralPosition()
		{
			this._neutralPosition = this._knobTransform.position;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0002BFA1 File Offset: 0x0002A1A1
		public virtual void SetNeutralPosition(Vector3 newPosition)
		{
			this._neutralPosition = newPosition;
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0002BFB0 File Offset: 0x0002A1B0
		public virtual void OnDrag(PointerEventData eventData)
		{
			this.OnDragEvent.Invoke();
			this._newTargetPosition = this.ConvertToWorld(eventData.position);
			this.ClampToBounds();
			if (!this.HorizontalAxisEnabled)
			{
				this._newTargetPosition.x = 0f;
			}
			if (!this.VerticalAxisEnabled)
			{
				this._newTargetPosition.y = 0f;
			}
			this.RawValue.x = this.EvaluateInputValue(this._newTargetPosition.x);
			this.RawValue.y = this.EvaluateInputValue(this._newTargetPosition.y);
			this._newJoystickPosition = this._neutralPosition + this._newTargetPosition;
			this._newJoystickPosition.z = this._initialZPosition;
			this._knobTransform.position = this._newJoystickPosition;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0002C090 File Offset: 0x0002A290
		protected virtual void ClampToBounds()
		{
			this._newTargetPosition = Vector2.ClampMagnitude(this._newTargetPosition - this._neutralPosition, this.ComputedMaxRange);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0002C0B4 File Offset: 0x0002A2B4
		protected virtual Vector3 ConvertToWorld(Vector3 position)
		{
			if (this.ParentCanvasRenderMode == RenderMode.ScreenSpaceCamera)
			{
				return this.TargetCamera.ScreenToWorldPoint(position);
			}
			return position;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0002C0D0 File Offset: 0x0002A2D0
		public virtual void ResetJoystick()
		{
			this._newJoystickPosition = this._neutralPosition;
			this._newJoystickPosition.z = this._initialZPosition;
			this._knobTransform.position = this._newJoystickPosition;
			this.RawValue.x = 0f;
			this.RawValue.y = 0f;
			this._targetOpacity = this._initialOpacity;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0002C13C File Offset: 0x0002A33C
		protected virtual float EvaluateInputValue(float vectorPosition)
		{
			return Mathf.InverseLerp(0f, this.ComputedMaxRange, Mathf.Abs(vectorPosition)) * Mathf.Sign(vectorPosition);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0002C15B File Offset: 0x0002A35B
		public virtual void OnEndDrag(PointerEventData eventData)
		{
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0002C15D File Offset: 0x0002A35D
		public virtual void OnPointerUp(PointerEventData data)
		{
			this.ResetJoystick();
			this.OnPointerUpEvent.Invoke();
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0002C170 File Offset: 0x0002A370
		public virtual void OnPointerDown(PointerEventData data)
		{
			this._targetOpacity = this.PressedOpacity;
			this.OnPointerDownEvent.Invoke();
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002C189 File Offset: 0x0002A389
		protected virtual void OnEnable()
		{
			this.Initialize();
			this._targetOpacity = this._initialOpacity;
		}

		// Token: 0x04000B28 RID: 2856
		[MMInspectorGroup("Camera", true, 16, false)]
		[Tooltip("The camera to use as the reference for any ScreenToWorldPoint computations")]
		public Camera TargetCamera;

		// Token: 0x04000B29 RID: 2857
		[MMInspectorGroup("Joystick Behaviour", true, 18, false)]
		[Tooltip("Determines whether the horizontal axis of this stick should be enabled. If not, the stick will only move vertically.")]
		public bool HorizontalAxisEnabled = true;

		// Token: 0x04000B2A RID: 2858
		[Tooltip("Determines whether the vertical axis of this stick should be enabled. If not, the stick will only move horizontally.")]
		public bool VerticalAxisEnabled = true;

		// Token: 0x04000B2B RID: 2859
		[Tooltip("the mode in which to compute the range. Distance will be a flat value, DistanceToTransform will be a distance to a transform you can move around and potentially resize as you wish for various resolutions")]
		public MMTouchJoystick.MaxRangeModes MaxRangeMode;

		// Token: 0x04000B2C RID: 2860
		[Tooltip("The MaxRange is the maximum distance from its initial center position you can drag the joystick to.")]
		[MMEnumCondition("MaxRangeMode", new int[] { 0 })]
		public float MaxRange = 1.5f;

		// Token: 0x04000B2D RID: 2861
		[Tooltip("in DistanceToTransform mode, the object whose distance to the center will be used to compute the max range. Note that this is computed once, at init. Call RefreshMaxRangeDistance() to recompute it.")]
		[MMEnumCondition("MaxRangeMode", new int[] { 1 })]
		public Transform MaxRangeTransform;

		// Token: 0x04000B2E RID: 2862
		[MMInspectorGroup("Value Events", true, 19, false)]
		[Tooltip("An event to use the raw value of the joystick")]
		public JoystickEvent JoystickValue;

		// Token: 0x04000B2F RID: 2863
		[Tooltip("An event to use the normalized value of the joystick")]
		public JoystickEvent JoystickNormalizedValue;

		// Token: 0x04000B30 RID: 2864
		[Tooltip("An event to use the joystick's amplitude (the magnitude of its Vector2 output)")]
		public JoystickFloatEvent JoystickMagnitudeValue;

		// Token: 0x04000B31 RID: 2865
		[MMInspectorGroup("Touch Events", true, 8, false)]
		[Tooltip("An event triggered when tapping the joystick for the first time")]
		public UnityEvent OnPointerDownEvent;

		// Token: 0x04000B32 RID: 2866
		[Tooltip("An event triggered when dragging the stick")]
		public UnityEvent OnDragEvent;

		// Token: 0x04000B33 RID: 2867
		[Tooltip("An event triggered when releasing the stick")]
		public UnityEvent OnPointerUpEvent;

		// Token: 0x04000B34 RID: 2868
		[MMInspectorGroup("Rotating Direction Indicator", true, 20, false)]
		[Tooltip("an object you can rotate to show the direction of the joystick. Will only be visible if the movement is above a threshold")]
		public Transform RotatingIndicator;

		// Token: 0x04000B35 RID: 2869
		[Tooltip("the threshold above which the rotating indicator will appear")]
		public float RotatingIndicatorThreshold = 0.1f;

		// Token: 0x04000B36 RID: 2870
		[MMInspectorGroup("Knob Opacity", true, 17, false)]
		[Tooltip("the new opacity to apply to the canvas group when the button is pressed")]
		public float PressedOpacity = 0.5f;

		// Token: 0x04000B37 RID: 2871
		[Tooltip("whether or not to interpolate opacity changes on the knob's canvas group")]
		public bool InterpolateOpacity = true;

		// Token: 0x04000B38 RID: 2872
		[Tooltip("the speed at which to interpolate opacity")]
		[MMCondition("InterpolateOpacity", true)]
		public float InterpolateOpacitySpeed = 1f;

		// Token: 0x04000B39 RID: 2873
		[MMInspectorGroup("Debug Output", true, 5, false)]
		[Tooltip("the raw value of the joystick, from 0 to 1 on each axis")]
		[MMReadOnly]
		public Vector2 RawValue;

		// Token: 0x04000B3A RID: 2874
		[Tooltip("the normalized value of the joystick")]
		[MMReadOnly]
		public Vector2 NormalizedValue;

		// Token: 0x04000B3B RID: 2875
		[Tooltip("the magnitude of the stick's vector")]
		[MMReadOnly]
		public float Magnitude;

		// Token: 0x04000B3C RID: 2876
		[Tooltip("whether or not to draw gizmos associated to this stick")]
		public bool DrawGizmos = true;

		// Token: 0x04000B3E RID: 2878
		protected Vector2 _neutralPosition;

		// Token: 0x04000B3F RID: 2879
		protected Vector2 _newTargetPosition;

		// Token: 0x04000B40 RID: 2880
		protected Vector3 _newJoystickPosition;

		// Token: 0x04000B41 RID: 2881
		protected float _initialZPosition;

		// Token: 0x04000B42 RID: 2882
		protected float _targetOpacity;

		// Token: 0x04000B43 RID: 2883
		protected CanvasGroup _canvasGroup;

		// Token: 0x04000B44 RID: 2884
		protected float _initialOpacity;

		// Token: 0x04000B45 RID: 2885
		protected Transform _knobTransform;

		// Token: 0x04000B46 RID: 2886
		protected bool _rotatingIndicatorIsNotNull;

		// Token: 0x04000B47 RID: 2887
		protected float _maxRangeTransformDistance;

		// Token: 0x02000478 RID: 1144
		public enum MaxRangeModes
		{
			// Token: 0x040020E3 RID: 8419
			Distance,
			// Token: 0x040020E4 RID: 8420
			DistanceToTransform
		}
	}
}
