using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000226 RID: 550
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Audio/MMAudioFilterDistortionShaker")]
	[RequireComponent(typeof(AudioDistortionFilter))]
	public class MMAudioFilterDistortionShaker : MMShaker
	{
		// Token: 0x06000F24 RID: 3876 RVA: 0x00044191 File Offset: 0x00042391
		protected override void Initialization()
		{
			base.Initialization();
			this._targetAudioDistortionFilter = base.gameObject.GetComponent<AudioDistortionFilter>();
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x000441AA File Offset: 0x000423AA
		protected virtual void Reset()
		{
			this.ShakeDuration = 2f;
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x000441B8 File Offset: 0x000423B8
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeDistortion, this.RemapDistortionZero, this.RemapDistortionOne, this.RelativeDistortion, this._initialDistortion);
			this._targetAudioDistortionFilter.distortionLevel = num;
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x000441F6 File Offset: 0x000423F6
		protected override void GrabInitialValues()
		{
			this._initialDistortion = this._targetAudioDistortionFilter.distortionLevel;
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0004420C File Offset: 0x0004240C
		public virtual void OnMMAudioFilterDistortionShakeEvent(AnimationCurve distortionCurve, float duration, float remapMin, float remapMax, bool relativeDistortion = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeDistortion = this.ShakeDistortion;
				this._originalRemapDistortionZero = this.RemapDistortionZero;
				this._originalRemapDistortionOne = this.RemapDistortionOne;
				this._originalRelativeDistortion = this.RelativeDistortion;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeDistortion = distortionCurve;
				this.RemapDistortionZero = remapMin * feedbacksIntensity;
				this.RemapDistortionOne = remapMax * feedbacksIntensity;
				this.RelativeDistortion = relativeDistortion;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x000442F2 File Offset: 0x000424F2
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetAudioDistortionFilter.distortionLevel = this._initialDistortion;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x0004430C File Offset: 0x0004250C
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeDistortion = this._originalShakeDistortion;
			this.RemapDistortionZero = this._originalRemapDistortionZero;
			this.RemapDistortionOne = this._originalRemapDistortionOne;
			this.RelativeDistortion = this._originalRelativeDistortion;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0004435B File Offset: 0x0004255B
		public override void StartListening()
		{
			base.StartListening();
			MMAudioFilterDistortionShakeEvent.Register(new MMAudioFilterDistortionShakeEvent.Delegate(this.OnMMAudioFilterDistortionShakeEvent));
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00044375 File Offset: 0x00042575
		public override void StopListening()
		{
			base.StopListening();
			MMAudioFilterDistortionShakeEvent.Unregister(new MMAudioFilterDistortionShakeEvent.Delegate(this.OnMMAudioFilterDistortionShakeEvent));
		}

		// Token: 0x040011B8 RID: 4536
		[MMInspectorGroup("Distortion", true, 51, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeDistortion;

		// Token: 0x040011B9 RID: 4537
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeDistortion = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040011BA RID: 4538
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapDistortionZero;

		// Token: 0x040011BB RID: 4539
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapDistortionOne = 1f;

		// Token: 0x040011BC RID: 4540
		protected AudioDistortionFilter _targetAudioDistortionFilter;

		// Token: 0x040011BD RID: 4541
		protected float _initialDistortion;

		// Token: 0x040011BE RID: 4542
		protected float _originalShakeDuration;

		// Token: 0x040011BF RID: 4543
		protected bool _originalRelativeDistortion;

		// Token: 0x040011C0 RID: 4544
		protected AnimationCurve _originalShakeDistortion;

		// Token: 0x040011C1 RID: 4545
		protected float _originalRemapDistortionZero;

		// Token: 0x040011C2 RID: 4546
		protected float _originalRemapDistortionOne;
	}
}
