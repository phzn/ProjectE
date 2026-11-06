using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000234 RID: 564
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Audio/MMAudioSourceVolumeShaker")]
	[RequireComponent(typeof(AudioSource))]
	public class MMAudioSourceVolumeShaker : MMShaker
	{
		// Token: 0x06000F94 RID: 3988 RVA: 0x000457B6 File Offset: 0x000439B6
		protected override void Initialization()
		{
			base.Initialization();
			this._targetAudioSource = base.gameObject.GetComponent<AudioSource>();
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x000457CF File Offset: 0x000439CF
		protected virtual void Reset()
		{
			this.ShakeDuration = 2f;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x000457DC File Offset: 0x000439DC
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeVolume, this.RemapVolumeZero, this.RemapVolumeOne, this.RelativeVolume, this._initialVolume);
			this._targetAudioSource.volume = num;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0004581A File Offset: 0x00043A1A
		protected override void GrabInitialValues()
		{
			this._initialVolume = this._targetAudioSource.volume;
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00045830 File Offset: 0x00043A30
		public virtual void OnMMAudioSourceVolumeShakeEvent(AnimationCurve volumeCurve, float duration, float remapMin, float remapMax, bool relativeVolume = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeVolume = this.ShakeVolume;
				this._originalRemapVolumeZero = this.RemapVolumeZero;
				this._originalRemapVolumeOne = this.RemapVolumeOne;
				this._originalRelativeValues = this.RelativeVolume;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeVolume = volumeCurve;
				this.RemapVolumeZero = remapMin * feedbacksIntensity;
				this.RemapVolumeOne = remapMax * feedbacksIntensity;
				this.RelativeVolume = relativeVolume;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00045916 File Offset: 0x00043B16
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetAudioSource.volume = this._initialVolume;
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00045930 File Offset: 0x00043B30
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeVolume = this._originalShakeVolume;
			this.RemapVolumeZero = this._originalRemapVolumeZero;
			this.RemapVolumeOne = this._originalRemapVolumeOne;
			this.RelativeVolume = this._originalRelativeValues;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0004597F File Offset: 0x00043B7F
		public override void StartListening()
		{
			base.StartListening();
			MMAudioSourceVolumeShakeEvent.Register(new MMAudioSourceVolumeShakeEvent.Delegate(this.OnMMAudioSourceVolumeShakeEvent));
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00045999 File Offset: 0x00043B99
		public override void StopListening()
		{
			base.StopListening();
			MMAudioSourceVolumeShakeEvent.Unregister(new MMAudioSourceVolumeShakeEvent.Delegate(this.OnMMAudioSourceVolumeShakeEvent));
		}

		// Token: 0x0400120C RID: 4620
		[MMInspectorGroup("Volume", true, 59, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeVolume;

		// Token: 0x0400120D RID: 4621
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeVolume = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(0.5f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x0400120E RID: 4622
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-1f, 1f)]
		public float RemapVolumeZero;

		// Token: 0x0400120F RID: 4623
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-1f, 1f)]
		public float RemapVolumeOne = 1f;

		// Token: 0x04001210 RID: 4624
		protected AudioSource _targetAudioSource;

		// Token: 0x04001211 RID: 4625
		protected float _initialVolume;

		// Token: 0x04001212 RID: 4626
		protected float _originalShakeDuration;

		// Token: 0x04001213 RID: 4627
		protected bool _originalRelativeValues;

		// Token: 0x04001214 RID: 4628
		protected AnimationCurve _originalShakeVolume;

		// Token: 0x04001215 RID: 4629
		protected float _originalRemapVolumeZero;

		// Token: 0x04001216 RID: 4630
		protected float _originalRemapVolumeOne;
	}
}
