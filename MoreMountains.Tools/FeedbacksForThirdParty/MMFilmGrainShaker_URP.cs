using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000361 RID: 865
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMFilmGrainShaker_URP")]
	public class MMFilmGrainShaker_URP : MMShaker
	{
		// Token: 0x060017C6 RID: 6086 RVA: 0x00065156 File Offset: 0x00063356
		protected override void Initialization()
		{
			base.Initialization();
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.profile.TryGet<FilmGrain>(out this._filmGrain);
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00065188 File Offset: 0x00063388
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeIntensity, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, this._initialIntensity);
			this._filmGrain.intensity.Override(num);
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x000651CB File Offset: 0x000633CB
		protected override void GrabInitialValues()
		{
			this._initialIntensity = this._filmGrain.intensity.value;
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x000651E4 File Offset: 0x000633E4
		public virtual void OnFilmGrainShakeEvent(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeIntensity = this.ShakeIntensity;
				this._originalRemapIntensityZero = this.RemapIntensityZero;
				this._originalRemapIntensityOne = this.RemapIntensityOne;
				this._originalRelativeIntensity = this.RelativeIntensity;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeIntensity = intensity;
				this.RemapIntensityZero = remapMin * attenuation;
				this.RemapIntensityOne = remapMax * attenuation;
				this.RelativeIntensity = relativeIntensity;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x000652CA File Offset: 0x000634CA
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._filmGrain.intensity.Override(this._initialIntensity);
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x000652E8 File Offset: 0x000634E8
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeIntensity = this._originalShakeIntensity;
			this.RemapIntensityZero = this._originalRemapIntensityZero;
			this.RemapIntensityOne = this._originalRemapIntensityOne;
			this.RelativeIntensity = this._originalRelativeIntensity;
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x00065337 File Offset: 0x00063537
		public override void StartListening()
		{
			base.StartListening();
			MMFilmGrainShakeEvent_URP.Register(new MMFilmGrainShakeEvent_URP.Delegate(this.OnFilmGrainShakeEvent));
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00065351 File Offset: 0x00063551
		public override void StopListening()
		{
			base.StopListening();
			MMFilmGrainShakeEvent_URP.Unregister(new MMFilmGrainShakeEvent_URP.Delegate(this.OnFilmGrainShakeEvent));
		}

		// Token: 0x04001B88 RID: 7048
		[MMInspectorGroup("Film Grain Intensity", true, 51, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity;

		// Token: 0x04001B89 RID: 7049
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B8A RID: 7050
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001B8B RID: 7051
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001B8C RID: 7052
		protected Volume _volume;

		// Token: 0x04001B8D RID: 7053
		protected FilmGrain _filmGrain;

		// Token: 0x04001B8E RID: 7054
		protected float _initialIntensity;

		// Token: 0x04001B8F RID: 7055
		protected float _originalShakeDuration;

		// Token: 0x04001B90 RID: 7056
		protected AnimationCurve _originalShakeIntensity;

		// Token: 0x04001B91 RID: 7057
		protected float _originalRemapIntensityZero;

		// Token: 0x04001B92 RID: 7058
		protected float _originalRemapIntensityOne;

		// Token: 0x04001B93 RID: 7059
		protected bool _originalRelativeIntensity;
	}
}
