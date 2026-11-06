using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000363 RID: 867
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMLensDistortionShaker_URP")]
	public class MMLensDistortionShaker_URP : MMShaker
	{
		// Token: 0x060017D5 RID: 6101 RVA: 0x00065492 File Offset: 0x00063692
		protected override void Initialization()
		{
			base.Initialization();
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.profile.TryGet<LensDistortion>(out this._lensDistortion);
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x000654C2 File Offset: 0x000636C2
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.8f;
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x000654D0 File Offset: 0x000636D0
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeIntensity, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, this._initialIntensity);
			this._lensDistortion.intensity.Override(num);
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00065513 File Offset: 0x00063713
		protected override void GrabInitialValues()
		{
			this._initialIntensity = this._lensDistortion.intensity.value;
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x0006552C File Offset: 0x0006372C
		public virtual void OnMMLensDistortionShakeEvent(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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

		// Token: 0x060017DA RID: 6106 RVA: 0x00065612 File Offset: 0x00063812
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._lensDistortion.intensity.Override(this._initialIntensity);
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00065630 File Offset: 0x00063830
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeIntensity = this._originalShakeIntensity;
			this.RemapIntensityZero = this._originalRemapIntensityZero;
			this.RemapIntensityOne = this._originalRemapIntensityOne;
			this.RelativeIntensity = this._originalRelativeIntensity;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x0006567F File Offset: 0x0006387F
		public override void StartListening()
		{
			base.StartListening();
			MMLensDistortionShakeEvent_URP.Register(new MMLensDistortionShakeEvent_URP.Delegate(this.OnMMLensDistortionShakeEvent));
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00065699 File Offset: 0x00063899
		public override void StopListening()
		{
			base.StopListening();
			MMLensDistortionShakeEvent_URP.Unregister(new MMLensDistortionShakeEvent_URP.Delegate(this.OnMMLensDistortionShakeEvent));
		}

		// Token: 0x04001B95 RID: 7061
		[MMInspectorGroup("Lens Distortion Intensity", true, 51, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity;

		// Token: 0x04001B96 RID: 7062
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.2f, 1f),
			new Keyframe(0.25f, -1f),
			new Keyframe(0.35f, 0.7f),
			new Keyframe(0.4f, -0.7f),
			new Keyframe(0.6f, 0.3f),
			new Keyframe(0.65f, -0.3f),
			new Keyframe(0.8f, 0.1f),
			new Keyframe(0.85f, -0.1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B97 RID: 7063
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapIntensityZero;

		// Token: 0x04001B98 RID: 7064
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapIntensityOne = 0.5f;

		// Token: 0x04001B99 RID: 7065
		protected Volume _volume;

		// Token: 0x04001B9A RID: 7066
		protected LensDistortion _lensDistortion;

		// Token: 0x04001B9B RID: 7067
		protected float _initialIntensity;

		// Token: 0x04001B9C RID: 7068
		protected float _originalShakeDuration;

		// Token: 0x04001B9D RID: 7069
		protected AnimationCurve _originalShakeIntensity;

		// Token: 0x04001B9E RID: 7070
		protected float _originalRemapIntensityZero;

		// Token: 0x04001B9F RID: 7071
		protected float _originalRemapIntensityOne;

		// Token: 0x04001BA0 RID: 7072
		protected bool _originalRelativeIntensity;
	}
}
