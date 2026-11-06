using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200023B RID: 571
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Camera/MMCameraFieldOfViewShaker")]
	[RequireComponent(typeof(Camera))]
	public class MMCameraFieldOfViewShaker : MMShaker
	{
		// Token: 0x06000FCA RID: 4042 RVA: 0x000467F0 File Offset: 0x000449F0
		protected override void Initialization()
		{
			base.Initialization();
			this._targetCamera = base.gameObject.GetComponent<Camera>();
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00046809 File Offset: 0x00044A09
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.5f;
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00046818 File Offset: 0x00044A18
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeFieldOfView, this.RemapFieldOfViewZero, this.RemapFieldOfViewOne, this.RelativeFieldOfView, this._initialFieldOfView);
			this._targetCamera.fieldOfView = num;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00046856 File Offset: 0x00044A56
		protected override void GrabInitialValues()
		{
			this._initialFieldOfView = this._targetCamera.fieldOfView;
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0004686C File Offset: 0x00044A6C
		public virtual void OnMMCameraFieldOfViewShakeEvent(AnimationCurve distortionCurve, float duration, float remapMin, float remapMax, bool relativeDistortion = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeFieldOfView = this.ShakeFieldOfView;
				this._originalRemapFieldOfViewZero = this.RemapFieldOfViewZero;
				this._originalRemapFieldOfViewOne = this.RemapFieldOfViewOne;
				this._originalRelativeFieldOfView = this.RelativeFieldOfView;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeFieldOfView = distortionCurve;
				this.RemapFieldOfViewZero = remapMin * feedbacksIntensity;
				this.RemapFieldOfViewOne = remapMax * feedbacksIntensity;
				this.RelativeFieldOfView = relativeDistortion;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00046953 File Offset: 0x00044B53
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetCamera.fieldOfView = this._initialFieldOfView;
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0004696C File Offset: 0x00044B6C
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeFieldOfView = this._originalShakeFieldOfView;
			this.RemapFieldOfViewZero = this._originalRemapFieldOfViewZero;
			this.RemapFieldOfViewOne = this._originalRemapFieldOfViewOne;
			this.RelativeFieldOfView = this._originalRelativeFieldOfView;
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x000469BB File Offset: 0x00044BBB
		public override void StartListening()
		{
			base.StartListening();
			MMCameraFieldOfViewShakeEvent.Register(new MMCameraFieldOfViewShakeEvent.Delegate(this.OnMMCameraFieldOfViewShakeEvent));
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x000469D5 File Offset: 0x00044BD5
		public override void StopListening()
		{
			base.StopListening();
			MMCameraFieldOfViewShakeEvent.Unregister(new MMCameraFieldOfViewShakeEvent.Delegate(this.OnMMCameraFieldOfViewShakeEvent));
		}

		// Token: 0x04001253 RID: 4691
		[MMInspectorGroup("Field of View", true, 34, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeFieldOfView;

		// Token: 0x04001254 RID: 4692
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeFieldOfView = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001255 RID: 4693
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 179f)]
		public float RemapFieldOfViewZero = 60f;

		// Token: 0x04001256 RID: 4694
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 179f)]
		public float RemapFieldOfViewOne = 120f;

		// Token: 0x04001257 RID: 4695
		protected Camera _targetCamera;

		// Token: 0x04001258 RID: 4696
		protected float _initialFieldOfView;

		// Token: 0x04001259 RID: 4697
		protected float _originalShakeDuration;

		// Token: 0x0400125A RID: 4698
		protected bool _originalRelativeFieldOfView;

		// Token: 0x0400125B RID: 4699
		protected AnimationCurve _originalShakeFieldOfView;

		// Token: 0x0400125C RID: 4700
		protected float _originalRemapFieldOfViewZero;

		// Token: 0x0400125D RID: 4701
		protected float _originalRemapFieldOfViewOne;
	}
}
