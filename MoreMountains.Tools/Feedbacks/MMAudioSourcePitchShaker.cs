using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000230 RID: 560
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Audio/MMAudioSourcePitchShaker")]
	[RequireComponent(typeof(AudioSource))]
	public class MMAudioSourcePitchShaker : MMShaker
	{
		// Token: 0x06000F74 RID: 3956 RVA: 0x0004515A File Offset: 0x0004335A
		protected override void Initialization()
		{
			base.Initialization();
			this._targetAudioSource = base.gameObject.GetComponent<AudioSource>();
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00045173 File Offset: 0x00043373
		protected virtual void Reset()
		{
			this.ShakeDuration = 2f;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00045180 File Offset: 0x00043380
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakePitch, this.RemapPitchZero, this.RemapPitchOne, this.RelativePitch, this._initialPitch);
			this._targetAudioSource.pitch = num;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x000451BE File Offset: 0x000433BE
		protected override void GrabInitialValues()
		{
			this._initialPitch = this._targetAudioSource.pitch;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x000451D4 File Offset: 0x000433D4
		public virtual void OnMMAudioSourcePitchShakeEvent(AnimationCurve pitchCurve, float duration, float remapMin, float remapMax, bool relativePitch = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakePitch = this.ShakePitch;
				this._originalRemapPitchZero = this.RemapPitchZero;
				this._originalRemapPitchOne = this.RemapPitchOne;
				this._originalRelativePitch = this.RelativePitch;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakePitch = pitchCurve;
				this.RemapPitchZero = remapMin * feedbacksIntensity;
				this.RemapPitchOne = remapMax * feedbacksIntensity;
				this.RelativePitch = relativePitch;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x000452BA File Offset: 0x000434BA
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetAudioSource.pitch = this._initialPitch;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x000452D4 File Offset: 0x000434D4
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakePitch = this._originalShakePitch;
			this.RemapPitchZero = this._originalRemapPitchZero;
			this.RemapPitchOne = this._originalRemapPitchOne;
			this.RelativePitch = this._originalRelativePitch;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00045323 File Offset: 0x00043523
		public override void StartListening()
		{
			base.StartListening();
			MMAudioSourcePitchShakeEvent.Register(new MMAudioSourcePitchShakeEvent.Delegate(this.OnMMAudioSourcePitchShakeEvent));
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0004533D File Offset: 0x0004353D
		public override void StopListening()
		{
			base.StopListening();
			MMAudioSourcePitchShakeEvent.Unregister(new MMAudioSourcePitchShakeEvent.Delegate(this.OnMMAudioSourcePitchShakeEvent));
		}

		// Token: 0x040011F4 RID: 4596
		[MMInspectorGroup("Pitch", true, 57, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativePitch;

		// Token: 0x040011F5 RID: 4597
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakePitch = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(0.5f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040011F6 RID: 4598
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-3f, 3f)]
		public float RemapPitchZero;

		// Token: 0x040011F7 RID: 4599
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-3f, 3f)]
		public float RemapPitchOne = 1f;

		// Token: 0x040011F8 RID: 4600
		protected AudioSource _targetAudioSource;

		// Token: 0x040011F9 RID: 4601
		protected float _initialPitch;

		// Token: 0x040011FA RID: 4602
		protected float _originalShakeDuration;

		// Token: 0x040011FB RID: 4603
		protected bool _originalRelativePitch;

		// Token: 0x040011FC RID: 4604
		protected AnimationCurve _originalShakePitch;

		// Token: 0x040011FD RID: 4605
		protected float _originalRemapPitchZero;

		// Token: 0x040011FE RID: 4606
		protected float _originalRemapPitchOne;
	}
}
