using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000228 RID: 552
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Audio/MMAudioFilterEchoShaker")]
	[RequireComponent(typeof(AudioEchoFilter))]
	public class MMAudioFilterEchoShaker : MMShaker
	{
		// Token: 0x06000F34 RID: 3892 RVA: 0x000444B6 File Offset: 0x000426B6
		protected override void Initialization()
		{
			base.Initialization();
			this._targetAudioEchoFilter = base.gameObject.GetComponent<AudioEchoFilter>();
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x000444CF File Offset: 0x000426CF
		protected virtual void Reset()
		{
			this.ShakeDuration = 2f;
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x000444DC File Offset: 0x000426DC
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeEcho, this.RemapEchoZero, this.RemapEchoOne, this.RelativeEcho, this._initialEcho);
			this._targetAudioEchoFilter.wetMix = num;
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0004451A File Offset: 0x0004271A
		protected override void GrabInitialValues()
		{
			this._initialEcho = this._targetAudioEchoFilter.wetMix;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00044530 File Offset: 0x00042730
		public virtual void OnMMAudioFilterEchoShakeEvent(AnimationCurve echoCurve, float duration, float remapMin, float remapMax, bool relativeEcho = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeEcho = this.ShakeEcho;
				this._originalRemapEchoZero = this.RemapEchoZero;
				this._originalRemapEchoOne = this.RemapEchoOne;
				this._originalRelativeEcho = this.RelativeEcho;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeEcho = echoCurve;
				this.RemapEchoZero = remapMin * feedbacksIntensity;
				this.RemapEchoOne = remapMax * feedbacksIntensity;
				this.RelativeEcho = relativeEcho;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00044616 File Offset: 0x00042816
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetAudioEchoFilter.wetMix = this._initialEcho;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00044630 File Offset: 0x00042830
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeEcho = this._originalShakeEcho;
			this.RemapEchoZero = this._originalRemapEchoZero;
			this.RemapEchoOne = this._originalRemapEchoOne;
			this.RelativeEcho = this._originalRelativeEcho;
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0004467F File Offset: 0x0004287F
		public override void StartListening()
		{
			base.StartListening();
			MMAudioFilterEchoShakeEvent.Register(new MMAudioFilterEchoShakeEvent.Delegate(this.OnMMAudioFilterEchoShakeEvent));
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00044699 File Offset: 0x00042899
		public override void StopListening()
		{
			base.StopListening();
			MMAudioFilterEchoShakeEvent.Unregister(new MMAudioFilterEchoShakeEvent.Delegate(this.OnMMAudioFilterEchoShakeEvent));
		}

		// Token: 0x040011C4 RID: 4548
		[MMInspectorGroup("Echo", true, 52, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeEcho;

		// Token: 0x040011C5 RID: 4549
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeEcho = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040011C6 RID: 4550
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapEchoZero;

		// Token: 0x040011C7 RID: 4551
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapEchoOne = 1f;

		// Token: 0x040011C8 RID: 4552
		protected AudioEchoFilter _targetAudioEchoFilter;

		// Token: 0x040011C9 RID: 4553
		protected float _initialEcho;

		// Token: 0x040011CA RID: 4554
		protected float _originalShakeDuration;

		// Token: 0x040011CB RID: 4555
		protected bool _originalRelativeEcho;

		// Token: 0x040011CC RID: 4556
		protected AnimationCurve _originalShakeEcho;

		// Token: 0x040011CD RID: 4557
		protected float _originalRemapEchoZero;

		// Token: 0x040011CE RID: 4558
		protected float _originalRemapEchoOne;
	}
}
