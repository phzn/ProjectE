using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000369 RID: 873
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMVignetteShaker_URP")]
	public class MMVignetteShaker_URP : MMShaker
	{
		// Token: 0x06001803 RID: 6147 RVA: 0x00065EEE File Offset: 0x000640EE
		protected override void Initialization()
		{
			base.Initialization();
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.profile.TryGet<Vignette>(out this._vignette);
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00065F20 File Offset: 0x00064120
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeIntensity, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, this._initialIntensity);
			this._vignette.intensity.Override(num);
			if (this.InterpolateColor)
			{
				float num2 = this.ShakeFloat(this.ColorCurve, this.RemapColorZero, this.RemapColorOne, this.RelativeIntensity, 0f);
				this._vignette.color.Override(Color.Lerp(this._initialColor, this.TargetColor, num2));
			}
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00065FB1 File Offset: 0x000641B1
		protected override void GrabInitialValues()
		{
			this._initialIntensity = this._vignette.intensity.value;
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00065FCC File Offset: 0x000641CC
		public virtual void OnVignetteShakeEvent(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false, bool interpolateColor = false, AnimationCurve colorCurve = null, float remapColorZero = 0f, float remapColorOne = 1f, Color targetColor = default(Color))
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
				this._originalInterpolateColor = this.InterpolateColor;
				this._originalColorCurve = this.ColorCurve;
				this._originalRemapColorZero = this.RemapColorZero;
				this._originalRemapColorOne = this.RemapColorOne;
				this._originalTargetColor = this.TargetColor;
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
				this.InterpolateColor = interpolateColor;
				this.ColorCurve = colorCurve;
				this.RemapColorZero = remapColorZero;
				this.RemapColorOne = remapColorOne;
				this.TargetColor = targetColor;
			}
			this.Play();
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00066116 File Offset: 0x00064316
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._vignette.intensity.Override(this._initialIntensity);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00066134 File Offset: 0x00064334
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeIntensity = this._originalShakeIntensity;
			this.RemapIntensityZero = this._originalRemapIntensityZero;
			this.RemapIntensityOne = this._originalRemapIntensityOne;
			this.RelativeIntensity = this._originalRelativeIntensity;
			this.InterpolateColor = this._originalInterpolateColor;
			this.ColorCurve = this._originalColorCurve;
			this.RemapColorZero = this._originalRemapColorZero;
			this.RemapColorOne = this._originalRemapColorOne;
			this.TargetColor = this._originalTargetColor;
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x000661BF File Offset: 0x000643BF
		public override void StartListening()
		{
			base.StartListening();
			MMVignetteShakeEvent_URP.Register(new MMVignetteShakeEvent_URP.Delegate(this.OnVignetteShakeEvent));
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x000661D9 File Offset: 0x000643D9
		public override void StopListening()
		{
			base.StopListening();
			MMVignetteShakeEvent_URP.Unregister(new MMVignetteShakeEvent_URP.Delegate(this.OnVignetteShakeEvent));
		}

		// Token: 0x04001BBC RID: 7100
		[MMInspectorGroup("Vignette Intensity", true, 63, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity;

		// Token: 0x04001BBD RID: 7101
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001BBE RID: 7102
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001BBF RID: 7103
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001BC0 RID: 7104
		[MMFInspectorGroup("Vignette Color", true, 60, false, false)]
		[Tooltip("whether or not to also animate the vignette's color")]
		public bool InterpolateColor;

		// Token: 0x04001BC1 RID: 7105
		[Tooltip("the curve to animate the color on")]
		public AnimationCurve ColorCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.05f, 1f),
			new Keyframe(0.95f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001BC2 RID: 7106
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapColorZero;

		// Token: 0x04001BC3 RID: 7107
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapColorOne = 1f;

		// Token: 0x04001BC4 RID: 7108
		[Tooltip("the color to lerp towards")]
		public Color TargetColor = Color.red;

		// Token: 0x04001BC5 RID: 7109
		protected Volume _volume;

		// Token: 0x04001BC6 RID: 7110
		protected Vignette _vignette;

		// Token: 0x04001BC7 RID: 7111
		protected float _initialIntensity;

		// Token: 0x04001BC8 RID: 7112
		protected float _originalShakeDuration;

		// Token: 0x04001BC9 RID: 7113
		protected AnimationCurve _originalShakeIntensity;

		// Token: 0x04001BCA RID: 7114
		protected float _originalRemapIntensityZero;

		// Token: 0x04001BCB RID: 7115
		protected float _originalRemapIntensityOne;

		// Token: 0x04001BCC RID: 7116
		protected bool _originalRelativeIntensity;

		// Token: 0x04001BCD RID: 7117
		protected bool _originalInterpolateColor;

		// Token: 0x04001BCE RID: 7118
		protected AnimationCurve _originalColorCurve;

		// Token: 0x04001BCF RID: 7119
		protected float _originalRemapColorZero;

		// Token: 0x04001BD0 RID: 7120
		protected float _originalRemapColorOne;

		// Token: 0x04001BD1 RID: 7121
		protected Color _originalTargetColor;

		// Token: 0x04001BD2 RID: 7122
		protected Color _initialColor;
	}
}
