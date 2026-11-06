using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000365 RID: 869
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMMotionBlurShaker_URP")]
	public class MMMotionBlurShaker_URP : MMShaker
	{
		// Token: 0x060017E5 RID: 6117 RVA: 0x00065876 File Offset: 0x00063A76
		protected override void Initialization()
		{
			base.Initialization();
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.profile.TryGet<MotionBlur>(out this._motionBlur);
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x000658A8 File Offset: 0x00063AA8
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeIntensity, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, this._initialIntensity);
			this._motionBlur.intensity.Override(num);
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x000658EB File Offset: 0x00063AEB
		protected override void GrabInitialValues()
		{
			this._initialIntensity = this._motionBlur.intensity.value;
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00065904 File Offset: 0x00063B04
		public virtual void OnMotionBlurShakeEvent(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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

		// Token: 0x060017E9 RID: 6121 RVA: 0x000659EA File Offset: 0x00063BEA
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._motionBlur.intensity.Override(this._initialIntensity);
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00065A08 File Offset: 0x00063C08
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeIntensity = this._originalShakeIntensity;
			this.RemapIntensityZero = this._originalRemapIntensityZero;
			this.RemapIntensityOne = this._originalRemapIntensityOne;
			this.RelativeIntensity = this._originalRelativeIntensity;
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00065A57 File Offset: 0x00063C57
		public override void StartListening()
		{
			base.StartListening();
			MMMotionBlurShakeEvent_URP.Register(new MMMotionBlurShakeEvent_URP.Delegate(this.OnMotionBlurShakeEvent));
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00065A71 File Offset: 0x00063C71
		public override void StopListening()
		{
			base.StopListening();
			MMMotionBlurShakeEvent_URP.Unregister(new MMMotionBlurShakeEvent_URP.Delegate(this.OnMotionBlurShakeEvent));
		}

		// Token: 0x04001BA2 RID: 7074
		[MMInspectorGroup("Motion Blur Intensity", true, 61, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity;

		// Token: 0x04001BA3 RID: 7075
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001BA4 RID: 7076
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001BA5 RID: 7077
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001BA6 RID: 7078
		protected Volume _volume;

		// Token: 0x04001BA7 RID: 7079
		protected MotionBlur _motionBlur;

		// Token: 0x04001BA8 RID: 7080
		protected float _initialIntensity;

		// Token: 0x04001BA9 RID: 7081
		protected float _originalShakeDuration;

		// Token: 0x04001BAA RID: 7082
		protected AnimationCurve _originalShakeIntensity;

		// Token: 0x04001BAB RID: 7083
		protected float _originalRemapIntensityZero;

		// Token: 0x04001BAC RID: 7084
		protected float _originalRemapIntensityOne;

		// Token: 0x04001BAD RID: 7085
		protected bool _originalRelativeIntensity;
	}
}
