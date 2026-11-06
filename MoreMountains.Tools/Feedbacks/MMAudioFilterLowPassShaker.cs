using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200022C RID: 556
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Audio/MMAudioFilterLowPassShaker")]
	[RequireComponent(typeof(AudioLowPassFilter))]
	public class MMAudioFilterLowPassShaker : MMShaker
	{
		// Token: 0x06000F54 RID: 3924 RVA: 0x00044AFE File Offset: 0x00042CFE
		protected override void Initialization()
		{
			base.Initialization();
			this._targetAudioLowPassFilter = base.gameObject.GetComponent<AudioLowPassFilter>();
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x00044B17 File Offset: 0x00042D17
		protected virtual void Reset()
		{
			this.ShakeDuration = 2f;
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00044B24 File Offset: 0x00042D24
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeLowPass, this.RemapLowPassZero, this.RemapLowPassOne, this.RelativeLowPass, this._initialLowPass);
			this._targetAudioLowPassFilter.cutoffFrequency = num;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00044B62 File Offset: 0x00042D62
		protected override void GrabInitialValues()
		{
			this._initialLowPass = this._targetAudioLowPassFilter.cutoffFrequency;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00044B78 File Offset: 0x00042D78
		public virtual void OnMMAudioFilterLowPassShakeEvent(AnimationCurve lowPassCurve, float duration, float remapMin, float remapMax, bool relativeLowPass = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			if (!this.CheckEventAllowed(channelData, false, 0f, default(Vector3)) || (!this.Interruptible && this.Shaking))
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
			this._resetShakerValuesAfterShake = resetShakerValuesAfterShake;
			this._resetTargetValuesAfterShake = resetTargetValuesAfterShake;
			if (resetShakerValuesAfterShake)
			{
				this._originalShakeDuration = this.ShakeDuration;
				this._originalShakeLowPass = this.ShakeLowPass;
				this._originalRemapLowPassZero = this.RemapLowPassZero;
				this._originalRemapLowPassOne = this.RemapLowPassOne;
				this._originalRelativeLowPass = this.RelativeLowPass;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeLowPass = lowPassCurve;
				this.RemapLowPassZero = remapMin * feedbacksIntensity;
				this.RemapLowPassOne = remapMax * feedbacksIntensity;
				this.RelativeLowPass = relativeLowPass;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x00044C5E File Offset: 0x00042E5E
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetAudioLowPassFilter.cutoffFrequency = this._initialLowPass;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00044C78 File Offset: 0x00042E78
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeLowPass = this._originalShakeLowPass;
			this.RemapLowPassZero = this._originalRemapLowPassZero;
			this.RemapLowPassOne = this._originalRemapLowPassOne;
			this.RelativeLowPass = this._originalRelativeLowPass;
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00044CC7 File Offset: 0x00042EC7
		public override void StartListening()
		{
			base.StartListening();
			MMAudioFilterLowPassShakeEvent.Register(new MMAudioFilterLowPassShakeEvent.Delegate(this.OnMMAudioFilterLowPassShakeEvent));
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00044CE1 File Offset: 0x00042EE1
		public override void StopListening()
		{
			base.StopListening();
			MMAudioFilterLowPassShakeEvent.Unregister(new MMAudioFilterLowPassShakeEvent.Delegate(this.OnMMAudioFilterLowPassShakeEvent));
		}

		// Token: 0x040011DC RID: 4572
		[MMInspectorGroup("Low Pass", true, 54, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeLowPass;

		// Token: 0x040011DD RID: 4573
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeLowPass = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(0.5f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040011DE RID: 4574
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(10f, 22000f)]
		public float RemapLowPassZero;

		// Token: 0x040011DF RID: 4575
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(10f, 22000f)]
		public float RemapLowPassOne = 10000f;

		// Token: 0x040011E0 RID: 4576
		protected AudioLowPassFilter _targetAudioLowPassFilter;

		// Token: 0x040011E1 RID: 4577
		protected float _initialLowPass;

		// Token: 0x040011E2 RID: 4578
		protected float _originalShakeDuration;

		// Token: 0x040011E3 RID: 4579
		protected bool _originalRelativeLowPass;

		// Token: 0x040011E4 RID: 4580
		protected AnimationCurve _originalShakeLowPass;

		// Token: 0x040011E5 RID: 4581
		protected float _originalRemapLowPassZero;

		// Token: 0x040011E6 RID: 4582
		protected float _originalRemapLowPassOne;
	}
}
