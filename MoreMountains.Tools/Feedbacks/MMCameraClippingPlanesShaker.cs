using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000239 RID: 569
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Camera/MMCameraClippingPlanesShaker")]
	[RequireComponent(typeof(Camera))]
	public class MMCameraClippingPlanesShaker : MMShaker
	{
		// Token: 0x06000FBA RID: 4026 RVA: 0x0004639A File Offset: 0x0004459A
		protected override void Initialization()
		{
			base.Initialization();
			this._targetCamera = base.gameObject.GetComponent<Camera>();
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x000463B3 File Offset: 0x000445B3
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.5f;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x000463C0 File Offset: 0x000445C0
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeNear, this.RemapNearZero, this.RemapNearOne, this.RelativeClippingPlanes, this._initialNear);
			this._targetCamera.nearClipPlane = num;
			float num2 = this.ShakeFloat(this.ShakeFar, this.RemapFarZero, this.RemapFarOne, this.RelativeClippingPlanes, this._initialFar);
			this._targetCamera.farClipPlane = num2;
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0004642F File Offset: 0x0004462F
		protected override void GrabInitialValues()
		{
			this._initialNear = this._targetCamera.nearClipPlane;
			this._initialFar = this._targetCamera.farClipPlane;
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00046454 File Offset: 0x00044654
		public virtual void OnMMCameraClippingPlanesShakeEvent(AnimationCurve animNearCurve, float duration, float remapNearMin, float remapNearMax, AnimationCurve animFarCurve, float remapFarMin, float remapFarMax, bool relativeValues = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			if (!this.CheckEventAllowed(channelData, false, 0f, default(Vector3)))
			{
				return;
			}
			if (stop)
			{
				this.Stop();
				return;
			}
			if (restore)
			{
				this.ResetTargetValues();
				return;
			}
			if (!this.Interruptible && this.Shaking)
			{
				return;
			}
			this._resetShakerValuesAfterShake = resetShakerValuesAfterShake;
			this._resetTargetValuesAfterShake = resetTargetValuesAfterShake;
			if (resetShakerValuesAfterShake)
			{
				this._originalShakeDuration = this.ShakeDuration;
				this._originalShakeNear = this.ShakeNear;
				this._originalShakeFar = this.ShakeFar;
				this._originalRemapNearZero = this.RemapNearZero;
				this._originalRemapNearOne = this.RemapNearOne;
				this._originalRemapFarZero = this.RemapFarZero;
				this._originalRemapFarOne = this.RemapFarOne;
				this._originalRelativeClippingPlanes = this.RelativeClippingPlanes;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeNear = animNearCurve;
				this.RemapNearZero = remapNearMin * feedbacksIntensity;
				this.RemapNearOne = remapNearMax * feedbacksIntensity;
				this.ShakeFar = animFarCurve;
				this.RemapFarZero = remapFarMin * feedbacksIntensity;
				this.RemapFarOne = remapFarMax * feedbacksIntensity;
				this.RelativeClippingPlanes = relativeValues;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0004657D File Offset: 0x0004477D
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetCamera.nearClipPlane = this._initialNear;
			this._targetCamera.farClipPlane = this._initialFar;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x000465A8 File Offset: 0x000447A8
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeNear = this._originalShakeNear;
			this.ShakeFar = this._originalShakeFar;
			this.RemapNearZero = this._originalRemapNearZero;
			this.RemapNearOne = this._originalRemapNearOne;
			this.RemapFarZero = this._originalRemapFarZero;
			this.RemapFarOne = this._originalRemapFarOne;
			this.RelativeClippingPlanes = this._originalRelativeClippingPlanes;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0004661B File Offset: 0x0004481B
		public override void StartListening()
		{
			base.StartListening();
			MMCameraClippingPlanesShakeEvent.Register(new MMCameraClippingPlanesShakeEvent.Delegate(this.OnMMCameraClippingPlanesShakeEvent));
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00046635 File Offset: 0x00044835
		public override void StopListening()
		{
			base.StopListening();
			MMCameraClippingPlanesShakeEvent.Unregister(new MMCameraClippingPlanesShakeEvent.Delegate(this.OnMMCameraClippingPlanesShakeEvent));
		}

		// Token: 0x04001240 RID: 4672
		[MMInspectorGroup("Clipping Planes", true, 31, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeClippingPlanes;

		// Token: 0x04001241 RID: 4673
		[MMInspectorGroup("Near Plane", true, 32, false)]
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeNear = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001242 RID: 4674
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapNearZero = 0.3f;

		// Token: 0x04001243 RID: 4675
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapNearOne = 100f;

		// Token: 0x04001244 RID: 4676
		[MMInspectorGroup("Far Plane", true, 33, false)]
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeFar = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001245 RID: 4677
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapFarZero = 1000f;

		// Token: 0x04001246 RID: 4678
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapFarOne = 1000f;

		// Token: 0x04001247 RID: 4679
		protected Camera _targetCamera;

		// Token: 0x04001248 RID: 4680
		protected float _initialNear;

		// Token: 0x04001249 RID: 4681
		protected float _initialFar;

		// Token: 0x0400124A RID: 4682
		protected float _originalShakeDuration;

		// Token: 0x0400124B RID: 4683
		protected bool _originalRelativeClippingPlanes;

		// Token: 0x0400124C RID: 4684
		protected AnimationCurve _originalShakeNear;

		// Token: 0x0400124D RID: 4685
		protected float _originalRemapNearZero;

		// Token: 0x0400124E RID: 4686
		protected float _originalRemapNearOne;

		// Token: 0x0400124F RID: 4687
		protected AnimationCurve _originalShakeFar;

		// Token: 0x04001250 RID: 4688
		protected float _originalRemapFarZero;

		// Token: 0x04001251 RID: 4689
		protected float _originalRemapFarOne;
	}
}
