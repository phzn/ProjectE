using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002E8 RID: 744
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Cinemachine/MMCinemachineFieldOfViewShaker")]
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class MMCinemachineFieldOfViewShaker : MMShaker
	{
		// Token: 0x060014DC RID: 5340 RVA: 0x000592CD File Offset: 0x000574CD
		protected override void Initialization()
		{
			base.Initialization();
			this._targetCamera = base.gameObject.GetComponent<CinemachineVirtualCamera>();
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x000592E6 File Offset: 0x000574E6
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.5f;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x000592F4 File Offset: 0x000574F4
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeFieldOfView, this.RemapFieldOfViewZero, this.RemapFieldOfViewOne, this.RelativeFieldOfView, this._initialFieldOfView);
			this.SetFieldOfView(num);
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0005932D File Offset: 0x0005752D
		protected virtual void SetFieldOfView(float newFieldOfView)
		{
			this._targetCamera.m_Lens.FieldOfView = newFieldOfView;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00059340 File Offset: 0x00057540
		protected override void GrabInitialValues()
		{
			this._initialFieldOfView = this._targetCamera.m_Lens.FieldOfView;
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00059358 File Offset: 0x00057558
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

		// Token: 0x060014E2 RID: 5346 RVA: 0x0005943F File Offset: 0x0005763F
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this.SetFieldOfView(this._initialFieldOfView);
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00059454 File Offset: 0x00057654
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeFieldOfView = this._originalShakeFieldOfView;
			this.RemapFieldOfViewZero = this._originalRemapFieldOfViewZero;
			this.RemapFieldOfViewOne = this._originalRemapFieldOfViewOne;
			this.RelativeFieldOfView = this._originalRelativeFieldOfView;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x000594A3 File Offset: 0x000576A3
		public override void StartListening()
		{
			base.StartListening();
			MMCameraFieldOfViewShakeEvent.Register(new MMCameraFieldOfViewShakeEvent.Delegate(this.OnMMCameraFieldOfViewShakeEvent));
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x000594BD File Offset: 0x000576BD
		public override void StopListening()
		{
			base.StopListening();
			MMCameraFieldOfViewShakeEvent.Unregister(new MMCameraFieldOfViewShakeEvent.Delegate(this.OnMMCameraFieldOfViewShakeEvent));
		}

		// Token: 0x040017DB RID: 6107
		[MMInspectorGroup("Field of view", true, 41, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeFieldOfView;

		// Token: 0x040017DC RID: 6108
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeFieldOfView = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040017DD RID: 6109
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 179f)]
		public float RemapFieldOfViewZero = 60f;

		// Token: 0x040017DE RID: 6110
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 179f)]
		public float RemapFieldOfViewOne = 120f;

		// Token: 0x040017DF RID: 6111
		protected CinemachineVirtualCamera _targetCamera;

		// Token: 0x040017E0 RID: 6112
		protected float _initialFieldOfView;

		// Token: 0x040017E1 RID: 6113
		protected float _originalShakeDuration;

		// Token: 0x040017E2 RID: 6114
		protected bool _originalRelativeFieldOfView;

		// Token: 0x040017E3 RID: 6115
		protected AnimationCurve _originalShakeFieldOfView;

		// Token: 0x040017E4 RID: 6116
		protected float _originalRemapFieldOfViewZero;

		// Token: 0x040017E5 RID: 6117
		protected float _originalRemapFieldOfViewOne;
	}
}
