using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002E7 RID: 743
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Cinemachine/MMCinemachineClippingPlanesShaker")]
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class MMCinemachineClippingPlanesShaker : MMShaker
	{
		// Token: 0x060014D1 RID: 5329 RVA: 0x00058F20 File Offset: 0x00057120
		protected override void Initialization()
		{
			base.Initialization();
			this._targetCamera = base.gameObject.GetComponent<CinemachineVirtualCamera>();
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00058F39 File Offset: 0x00057139
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.5f;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00058F48 File Offset: 0x00057148
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeNear, this.RemapNearZero, this.RemapNearOne, this.RelativeClippingPlanes, this._initialNear);
			float num2 = this.ShakeFloat(this.ShakeFar, this.RemapFarZero, this.RemapFarOne, this.RelativeClippingPlanes, this._initialFar);
			this.SetNearFar(num, num2);
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x00058FA7 File Offset: 0x000571A7
		protected virtual void SetNearFar(float near, float far)
		{
			this._targetCamera.m_Lens.NearClipPlane = near;
			this._targetCamera.m_Lens.FarClipPlane = far;
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00058FCB File Offset: 0x000571CB
		protected override void GrabInitialValues()
		{
			this._initialNear = this._targetCamera.m_Lens.NearClipPlane;
			this._initialFar = this._targetCamera.m_Lens.FarClipPlane;
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x00058FFC File Offset: 0x000571FC
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

		// Token: 0x060014D7 RID: 5335 RVA: 0x00059125 File Offset: 0x00057325
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this.SetNearFar(this._initialNear, this._initialFar);
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x00059140 File Offset: 0x00057340
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

		// Token: 0x060014D9 RID: 5337 RVA: 0x000591B3 File Offset: 0x000573B3
		public override void StartListening()
		{
			base.StartListening();
			MMCameraClippingPlanesShakeEvent.Register(new MMCameraClippingPlanesShakeEvent.Delegate(this.OnMMCameraClippingPlanesShakeEvent));
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x000591CD File Offset: 0x000573CD
		public override void StopListening()
		{
			base.StopListening();
			MMCameraClippingPlanesShakeEvent.Unregister(new MMCameraClippingPlanesShakeEvent.Delegate(this.OnMMCameraClippingPlanesShakeEvent));
		}

		// Token: 0x040017C9 RID: 6089
		[MMInspectorGroup("Clipping Planes", true, 45, false)]
		public bool RelativeClippingPlanes;

		// Token: 0x040017CA RID: 6090
		[MMInspectorGroup("Near Plane", true, 46, false)]
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeNear = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040017CB RID: 6091
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapNearZero = 0.3f;

		// Token: 0x040017CC RID: 6092
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapNearOne = 100f;

		// Token: 0x040017CD RID: 6093
		[MMInspectorGroup("Far Plane", true, 47, false)]
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeFar = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040017CE RID: 6094
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapFarZero = 1000f;

		// Token: 0x040017CF RID: 6095
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapFarOne = 1000f;

		// Token: 0x040017D0 RID: 6096
		protected CinemachineVirtualCamera _targetCamera;

		// Token: 0x040017D1 RID: 6097
		protected float _initialNear;

		// Token: 0x040017D2 RID: 6098
		protected float _initialFar;

		// Token: 0x040017D3 RID: 6099
		protected float _originalShakeDuration;

		// Token: 0x040017D4 RID: 6100
		protected bool _originalRelativeClippingPlanes;

		// Token: 0x040017D5 RID: 6101
		protected AnimationCurve _originalShakeNear;

		// Token: 0x040017D6 RID: 6102
		protected float _originalRemapNearZero;

		// Token: 0x040017D7 RID: 6103
		protected float _originalRemapNearOne;

		// Token: 0x040017D8 RID: 6104
		protected AnimationCurve _originalShakeFar;

		// Token: 0x040017D9 RID: 6105
		protected float _originalRemapFarZero;

		// Token: 0x040017DA RID: 6106
		protected float _originalRemapFarOne;
	}
}
