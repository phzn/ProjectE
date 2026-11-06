using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000232 RID: 562
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Audio/MMAudioSourceStereoPanShaker")]
	[RequireComponent(typeof(AudioSource))]
	public class MMAudioSourceStereoPanShaker : MMShaker
	{
		// Token: 0x06000F84 RID: 3972 RVA: 0x0004547E File Offset: 0x0004367E
		protected override void Initialization()
		{
			base.Initialization();
			this._targetAudioSource = base.gameObject.GetComponent<AudioSource>();
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00045497 File Offset: 0x00043697
		protected virtual void Reset()
		{
			this.ShakeDuration = 2f;
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x000454A4 File Offset: 0x000436A4
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeStereoPan, this.RemapStereoPanZero, this.RemapStereoPanOne, this.RelativeStereoPan, this._initialStereoPan);
			this._targetAudioSource.panStereo = num;
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x000454E2 File Offset: 0x000436E2
		protected override void GrabInitialValues()
		{
			this._initialStereoPan = this._targetAudioSource.panStereo;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x000454F8 File Offset: 0x000436F8
		public virtual void OnMMAudioSourceStereoPanShakeEvent(AnimationCurve stereoPanCurve, float duration, float remapMin, float remapMax, bool relativeStereoPan = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeStereoPan = this.ShakeStereoPan;
				this._originalRemapStereoPanZero = this.RemapStereoPanZero;
				this._originalRemapStereoPanOne = this.RemapStereoPanOne;
				this._originalRelativeValues = this.RelativeStereoPan;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeStereoPan = stereoPanCurve;
				this.RemapStereoPanZero = remapMin * feedbacksIntensity;
				this.RemapStereoPanOne = remapMax * feedbacksIntensity;
				this.RelativeStereoPan = relativeStereoPan;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x000455DE File Offset: 0x000437DE
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetAudioSource.panStereo = this._initialStereoPan;
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x000455F8 File Offset: 0x000437F8
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeStereoPan = this._originalShakeStereoPan;
			this.RemapStereoPanZero = this._originalRemapStereoPanZero;
			this.RemapStereoPanOne = this._originalRemapStereoPanOne;
			this.RelativeStereoPan = this._originalRelativeValues;
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x00045647 File Offset: 0x00043847
		public override void StartListening()
		{
			base.StartListening();
			MMAudioSourceStereoPanShakeEvent.Register(new MMAudioSourceStereoPanShakeEvent.Delegate(this.OnMMAudioSourceStereoPanShakeEvent));
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x00045661 File Offset: 0x00043861
		public override void StopListening()
		{
			base.StopListening();
			MMAudioSourceStereoPanShakeEvent.Unregister(new MMAudioSourceStereoPanShakeEvent.Delegate(this.OnMMAudioSourceStereoPanShakeEvent));
		}

		// Token: 0x04001200 RID: 4608
		[MMInspectorGroup("Stereo Pan", true, 57, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeStereoPan;

		// Token: 0x04001201 RID: 4609
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeStereoPan = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(0.6f, -1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001202 RID: 4610
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-1f, 1f)]
		public float RemapStereoPanZero;

		// Token: 0x04001203 RID: 4611
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-1f, 1f)]
		public float RemapStereoPanOne = 1f;

		// Token: 0x04001204 RID: 4612
		protected AudioSource _targetAudioSource;

		// Token: 0x04001205 RID: 4613
		protected float _initialStereoPan;

		// Token: 0x04001206 RID: 4614
		protected float _originalShakeDuration;

		// Token: 0x04001207 RID: 4615
		protected bool _originalRelativeValues;

		// Token: 0x04001208 RID: 4616
		protected AnimationCurve _originalShakeStereoPan;

		// Token: 0x04001209 RID: 4617
		protected float _originalRemapStereoPanZero;

		// Token: 0x0400120A RID: 4618
		protected float _originalRemapStereoPanOne;
	}
}
