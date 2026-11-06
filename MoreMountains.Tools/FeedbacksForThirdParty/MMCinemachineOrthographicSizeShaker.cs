using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002EA RID: 746
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Cinemachine/MMCinemachineOrthographicSizeShaker")]
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class MMCinemachineOrthographicSizeShaker : MMShaker
	{
		// Token: 0x060014F3 RID: 5363 RVA: 0x000598A5 File Offset: 0x00057AA5
		protected override void Initialization()
		{
			base.Initialization();
			this._targetCamera = base.gameObject.GetComponent<CinemachineVirtualCamera>();
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x000598BE File Offset: 0x00057ABE
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.5f;
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x000598CC File Offset: 0x00057ACC
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeOrthographicSize, this.RemapOrthographicSizeZero, this.RemapOrthographicSizeOne, this.RelativeOrthographicSize, this._initialOrthographicSize);
			this._targetCamera.m_Lens.OrthographicSize = num;
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0005990F File Offset: 0x00057B0F
		protected override void GrabInitialValues()
		{
			this._initialOrthographicSize = this._targetCamera.m_Lens.OrthographicSize;
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00059928 File Offset: 0x00057B28
		public virtual void OnMMCameraOrthographicSizeShakeEvent(AnimationCurve distortionCurve, float duration, float remapMin, float remapMax, bool relativeDistortion = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeOrthographicSize = this.ShakeOrthographicSize;
				this._originalRemapOrthographicSizeZero = this.RemapOrthographicSizeZero;
				this._originalRemapOrthographicSizeOne = this.RemapOrthographicSizeOne;
				this._originalRelativeOrthographicSize = this.RelativeOrthographicSize;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeOrthographicSize = distortionCurve;
				this.RemapOrthographicSizeZero = remapMin * feedbacksIntensity;
				this.RemapOrthographicSizeOne = remapMax * feedbacksIntensity;
				this.RelativeOrthographicSize = relativeDistortion;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00059A0F File Offset: 0x00057C0F
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetCamera.m_Lens.OrthographicSize = this._initialOrthographicSize;
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00059A30 File Offset: 0x00057C30
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeOrthographicSize = this._originalShakeOrthographicSize;
			this.RemapOrthographicSizeZero = this._originalRemapOrthographicSizeZero;
			this.RemapOrthographicSizeOne = this._originalRemapOrthographicSizeOne;
			this.RelativeOrthographicSize = this._originalRelativeOrthographicSize;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00059A7F File Offset: 0x00057C7F
		public override void StartListening()
		{
			base.StartListening();
			MMCameraOrthographicSizeShakeEvent.Register(new MMCameraOrthographicSizeShakeEvent.Delegate(this.OnMMCameraOrthographicSizeShakeEvent));
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00059A99 File Offset: 0x00057C99
		public override void StopListening()
		{
			base.StopListening();
			MMCameraOrthographicSizeShakeEvent.Unregister(new MMCameraOrthographicSizeShakeEvent.Delegate(this.OnMMCameraOrthographicSizeShakeEvent));
		}

		// Token: 0x040017FE RID: 6142
		[MMInspectorGroup("Orthographic Size", true, 43, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeOrthographicSize;

		// Token: 0x040017FF RID: 6143
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeOrthographicSize = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001800 RID: 6144
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapOrthographicSizeZero = 5f;

		// Token: 0x04001801 RID: 6145
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapOrthographicSizeOne = 10f;

		// Token: 0x04001802 RID: 6146
		protected CinemachineVirtualCamera _targetCamera;

		// Token: 0x04001803 RID: 6147
		protected float _initialOrthographicSize;

		// Token: 0x04001804 RID: 6148
		protected float _originalShakeDuration;

		// Token: 0x04001805 RID: 6149
		protected bool _originalRelativeOrthographicSize;

		// Token: 0x04001806 RID: 6150
		protected AnimationCurve _originalShakeOrthographicSize;

		// Token: 0x04001807 RID: 6151
		protected float _originalRemapOrthographicSizeZero;

		// Token: 0x04001808 RID: 6152
		protected float _originalRemapOrthographicSizeOne;
	}
}
