using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200022E RID: 558
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Audio/MMAudioFilterReverbShaker")]
	[RequireComponent(typeof(AudioReverbFilter))]
	public class MMAudioFilterReverbShaker : MMShaker
	{
		// Token: 0x06000F64 RID: 3940 RVA: 0x00044E22 File Offset: 0x00043022
		protected override void Initialization()
		{
			base.Initialization();
			this._targetAudioReverbFilter = base.gameObject.GetComponent<AudioReverbFilter>();
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00044E3B File Offset: 0x0004303B
		protected virtual void Reset()
		{
			this.ShakeDuration = 2f;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00044E48 File Offset: 0x00043048
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeReverb, this.RemapReverbZero, this.RemapReverbOne, this.RelativeReverb, this._initialReverb);
			this._targetAudioReverbFilter.reverbLevel = num;
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00044E86 File Offset: 0x00043086
		protected override void GrabInitialValues()
		{
			this._initialReverb = this._targetAudioReverbFilter.reverbLevel;
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x00044E9C File Offset: 0x0004309C
		public virtual void OnMMAudioFilterReverbShakeEvent(AnimationCurve reverbCurve, float duration, float remapMin, float remapMax, bool relativeReverb = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeReverb = this.ShakeReverb;
				this._originalRemapReverbZero = this.RemapReverbZero;
				this._originalRemapReverbOne = this.RemapReverbOne;
				this._originalRelativeReverb = this.RelativeReverb;
			}
			if (restore)
			{
				this.ResetTargetValues();
				return;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeReverb = reverbCurve;
				this.RemapReverbZero = remapMin * feedbacksIntensity;
				this.RemapReverbOne = remapMax * feedbacksIntensity;
				this.RelativeReverb = relativeReverb;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x00044F8D File Offset: 0x0004318D
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetAudioReverbFilter.reverbLevel = this._initialReverb;
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00044FA8 File Offset: 0x000431A8
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeReverb = this._originalShakeReverb;
			this.RemapReverbZero = this._originalRemapReverbZero;
			this.RemapReverbOne = this._originalRemapReverbOne;
			this.RelativeReverb = this._originalRelativeReverb;
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00044FF7 File Offset: 0x000431F7
		public override void StartListening()
		{
			base.StartListening();
			MMAudioFilterReverbShakeEvent.Register(new MMAudioFilterReverbShakeEvent.Delegate(this.OnMMAudioFilterReverbShakeEvent));
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x00045011 File Offset: 0x00043211
		public override void StopListening()
		{
			base.StopListening();
			MMAudioFilterReverbShakeEvent.Unregister(new MMAudioFilterReverbShakeEvent.Delegate(this.OnMMAudioFilterReverbShakeEvent));
		}

		// Token: 0x040011E8 RID: 4584
		[MMInspectorGroup("Reverb", true, 55, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeReverb;

		// Token: 0x040011E9 RID: 4585
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeReverb = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040011EA RID: 4586
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-10000f, 2000f)]
		public float RemapReverbZero = -10000f;

		// Token: 0x040011EB RID: 4587
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-10000f, 2000f)]
		public float RemapReverbOne = 2000f;

		// Token: 0x040011EC RID: 4588
		protected AudioReverbFilter _targetAudioReverbFilter;

		// Token: 0x040011ED RID: 4589
		protected float _initialReverb;

		// Token: 0x040011EE RID: 4590
		protected float _originalShakeDuration;

		// Token: 0x040011EF RID: 4591
		protected bool _originalRelativeReverb;

		// Token: 0x040011F0 RID: 4592
		protected AnimationCurve _originalShakeReverb;

		// Token: 0x040011F1 RID: 4593
		protected float _originalRemapReverbZero;

		// Token: 0x040011F2 RID: 4594
		protected float _originalRemapReverbOne;
	}
}
