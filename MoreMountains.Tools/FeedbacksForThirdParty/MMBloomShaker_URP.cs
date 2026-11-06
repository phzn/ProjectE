using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000357 RID: 855
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMBloomShaker_URP")]
	public class MMBloomShaker_URP : MMShaker
	{
		// Token: 0x06001778 RID: 6008 RVA: 0x000635EF File Offset: 0x000617EF
		protected override void Initialization()
		{
			base.Initialization();
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.profile.TryGet<Bloom>(out this._bloom);
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x00063620 File Offset: 0x00061820
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeIntensity, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeValues, this._initialIntensity);
			this._bloom.intensity.Override(num);
			float num2 = this.ShakeFloat(this.ShakeThreshold, this.RemapThresholdZero, this.RemapThresholdOne, this.RelativeValues, this._initialThreshold);
			this._bloom.threshold.Override(num2);
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x00063699 File Offset: 0x00061899
		protected override void GrabInitialValues()
		{
			this._initialIntensity = this._bloom.intensity.value;
			this._initialThreshold = this._bloom.threshold.value;
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x000636C8 File Offset: 0x000618C8
		public virtual void OnBloomShakeEvent(AnimationCurve intensity, float duration, float remapMin, float remapMax, AnimationCurve threshold, float remapThresholdMin, float remapThresholdMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalRelativeIntensity = this.RelativeValues;
				this._originalShakeThreshold = this.ShakeThreshold;
				this._originalRemapThresholdZero = this.RemapThresholdZero;
				this._originalRemapThresholdOne = this.RemapThresholdOne;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeIntensity = intensity;
				this.RemapIntensityZero = remapMin * attenuation;
				this.RemapIntensityOne = remapMax * attenuation;
				this.RelativeValues = relativeIntensity;
				this.ShakeThreshold = threshold;
				this.RemapThresholdZero = remapThresholdMin;
				this.RemapThresholdOne = remapThresholdMax;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x000637EA File Offset: 0x000619EA
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._bloom.intensity.Override(this._initialIntensity);
			this._bloom.threshold.Override(this._initialThreshold);
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x00063820 File Offset: 0x00061A20
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeIntensity = this._originalShakeIntensity;
			this.RemapIntensityZero = this._originalRemapIntensityZero;
			this.RemapIntensityOne = this._originalRemapIntensityOne;
			this.RelativeValues = this._originalRelativeIntensity;
			this.ShakeThreshold = this._originalShakeThreshold;
			this.RemapThresholdZero = this._originalRemapThresholdZero;
			this.RemapThresholdOne = this._originalRemapThresholdOne;
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x00063893 File Offset: 0x00061A93
		public override void StartListening()
		{
			base.StartListening();
			MMBloomShakeEvent_URP.Register(new MMBloomShakeEvent_URP.Delegate(this.OnBloomShakeEvent));
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x000638AD File Offset: 0x00061AAD
		public override void StopListening()
		{
			base.StopListening();
			MMBloomShakeEvent_URP.Unregister(new MMBloomShakeEvent_URP.Delegate(this.OnBloomShakeEvent));
		}

		// Token: 0x04001B04 RID: 6916
		public bool RelativeValues = true;

		// Token: 0x04001B05 RID: 6917
		[MMInspectorGroup("Bloom Intensity", true, 51, false)]
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B06 RID: 6918
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapIntensityZero;

		// Token: 0x04001B07 RID: 6919
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001B08 RID: 6920
		[MMInspectorGroup("Bloom Threshold", true, 52, false)]
		[Tooltip("the curve used to animate the threshold value on")]
		public AnimationCurve ShakeThreshold = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B09 RID: 6921
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapThresholdZero;

		// Token: 0x04001B0A RID: 6922
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapThresholdOne;

		// Token: 0x04001B0B RID: 6923
		protected Volume _volume;

		// Token: 0x04001B0C RID: 6924
		protected Bloom _bloom;

		// Token: 0x04001B0D RID: 6925
		protected float _initialIntensity;

		// Token: 0x04001B0E RID: 6926
		protected float _initialThreshold;

		// Token: 0x04001B0F RID: 6927
		protected float _originalShakeDuration;

		// Token: 0x04001B10 RID: 6928
		protected bool _originalRelativeIntensity;

		// Token: 0x04001B11 RID: 6929
		protected AnimationCurve _originalShakeIntensity;

		// Token: 0x04001B12 RID: 6930
		protected float _originalRemapIntensityZero;

		// Token: 0x04001B13 RID: 6931
		protected float _originalRemapIntensityOne;

		// Token: 0x04001B14 RID: 6932
		protected AnimationCurve _originalShakeThreshold;

		// Token: 0x04001B15 RID: 6933
		protected float _originalRemapThresholdZero;

		// Token: 0x04001B16 RID: 6934
		protected float _originalRemapThresholdOne;
	}
}
