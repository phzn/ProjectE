using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200022A RID: 554
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Audio/MMAudioFilterHighPassShaker")]
	[RequireComponent(typeof(AudioHighPassFilter))]
	public class MMAudioFilterHighPassShaker : MMShaker
	{
		// Token: 0x06000F44 RID: 3908 RVA: 0x000447DA File Offset: 0x000429DA
		protected override void Initialization()
		{
			base.Initialization();
			this._targetAudioHighPassFilter = base.gameObject.GetComponent<AudioHighPassFilter>();
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x000447F3 File Offset: 0x000429F3
		protected virtual void Reset()
		{
			this.ShakeDuration = 2f;
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x00044800 File Offset: 0x00042A00
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeHighPass, this.RemapHighPassZero, this.RemapHighPassOne, this.RelativeHighPass, this._initialHighPass);
			this._targetAudioHighPassFilter.cutoffFrequency = num;
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0004483E File Offset: 0x00042A3E
		protected override void GrabInitialValues()
		{
			this._initialHighPass = this._targetAudioHighPassFilter.cutoffFrequency;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00044854 File Offset: 0x00042A54
		public virtual void OnMMAudioFilterHighPassShakeEvent(AnimationCurve highPassCurve, float duration, float remapMin, float remapMax, bool relativeHighPass = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeHighPass = this.ShakeHighPass;
				this._originalRemapHighPassZero = this.RemapHighPassZero;
				this._originalRemapHighPassOne = this.RemapHighPassOne;
				this._originalRelativeHighPass = this.RelativeHighPass;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeHighPass = highPassCurve;
				this.RemapHighPassZero = remapMin * feedbacksIntensity;
				this.RemapHighPassOne = remapMax * feedbacksIntensity;
				this.RelativeHighPass = relativeHighPass;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0004493A File Offset: 0x00042B3A
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetAudioHighPassFilter.cutoffFrequency = this._initialHighPass;
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x00044954 File Offset: 0x00042B54
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeHighPass = this._originalShakeHighPass;
			this.RemapHighPassZero = this._originalRemapHighPassZero;
			this.RemapHighPassOne = this._originalRemapHighPassOne;
			this.RelativeHighPass = this._originalRelativeHighPass;
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x000449A3 File Offset: 0x00042BA3
		public override void StartListening()
		{
			base.StartListening();
			MMAudioFilterHighPassShakeEvent.Register(new MMAudioFilterHighPassShakeEvent.Delegate(this.OnMMAudioFilterHighPassShakeEvent));
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x000449BD File Offset: 0x00042BBD
		public override void StopListening()
		{
			base.StopListening();
			MMAudioFilterHighPassShakeEvent.Unregister(new MMAudioFilterHighPassShakeEvent.Delegate(this.OnMMAudioFilterHighPassShakeEvent));
		}

		// Token: 0x040011D0 RID: 4560
		[MMInspectorGroup("High Pass", true, 53, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeHighPass;

		// Token: 0x040011D1 RID: 4561
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeHighPass = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040011D2 RID: 4562
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(10f, 22000f)]
		public float RemapHighPassZero;

		// Token: 0x040011D3 RID: 4563
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(10f, 22000f)]
		public float RemapHighPassOne = 10000f;

		// Token: 0x040011D4 RID: 4564
		protected AudioHighPassFilter _targetAudioHighPassFilter;

		// Token: 0x040011D5 RID: 4565
		protected float _initialHighPass;

		// Token: 0x040011D6 RID: 4566
		protected float _originalShakeDuration;

		// Token: 0x040011D7 RID: 4567
		protected bool _originalRelativeHighPass;

		// Token: 0x040011D8 RID: 4568
		protected AnimationCurve _originalShakeHighPass;

		// Token: 0x040011D9 RID: 4569
		protected float _originalRemapHighPassZero;

		// Token: 0x040011DA RID: 4570
		protected float _originalRemapHighPassOne;
	}
}
