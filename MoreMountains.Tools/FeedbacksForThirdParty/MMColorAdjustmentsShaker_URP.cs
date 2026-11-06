using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200035D RID: 861
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMColorAdjustmentsShaker_URP")]
	public class MMColorAdjustmentsShaker_URP : MMShaker
	{
		// Token: 0x060017A6 RID: 6054 RVA: 0x0006433A File Offset: 0x0006253A
		protected override void Initialization()
		{
			base.Initialization();
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.profile.TryGet<ColorAdjustments>(out this._colorAdjustments);
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x0006436A File Offset: 0x0006256A
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.8f;
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00064378 File Offset: 0x00062578
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakePostExposure, this.RemapPostExposureZero, this.RemapPostExposureOne, this.RelativeValues, this._initialPostExposure);
			this._colorAdjustments.postExposure.Override(num);
			float num2 = this.ShakeFloat(this.ShakeHueShift, this.RemapHueShiftZero, this.RemapHueShiftOne, this.RelativeValues, this._initialHueShift);
			this._colorAdjustments.hueShift.Override(num2);
			float num3 = this.ShakeFloat(this.ShakeSaturation, this.RemapSaturationZero, this.RemapSaturationOne, this.RelativeValues, this._initialSaturation);
			this._colorAdjustments.saturation.Override(num3);
			float num4 = this.ShakeFloat(this.ShakeContrast, this.RemapContrastZero, this.RemapContrastOne, this.RelativeValues, this._initialContrast);
			this._colorAdjustments.contrast.Override(num4);
			this._remappedTimeSinceStart = MMFeedbacksHelpers.Remap(Time.time - this._shakeStartedTimestamp, 0f, this.ShakeDuration, 0f, 1f);
			if (this.ColorFilterMode == MMColorAdjustmentsShaker_URP.ColorFilterModes.Gradient)
			{
				this._colorAdjustments.colorFilter.Override(this.ColorFilterGradient.Evaluate(this._remappedTimeSinceStart));
				return;
			}
			if (this.ColorFilterMode == MMColorAdjustmentsShaker_URP.ColorFilterModes.Interpolate)
			{
				float num5 = this.ColorFilterCurve.Evaluate(this._remappedTimeSinceStart);
				this._colorAdjustments.colorFilter.Override(Color.LerpUnclamped(this._initialColorFilterColor, this.ColorFilterDestination, num5));
			}
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x000644F4 File Offset: 0x000626F4
		protected override void GrabInitialValues()
		{
			this._initialPostExposure = this._colorAdjustments.postExposure.value;
			this._initialHueShift = this._colorAdjustments.hueShift.value;
			this._initialSaturation = this._colorAdjustments.saturation.value;
			this._initialContrast = this._colorAdjustments.contrast.value;
			this._initialColorFilterColor = this._colorAdjustments.colorFilter.value;
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x00064570 File Offset: 0x00062770
		public virtual void OnMMColorGradingShakeEvent(AnimationCurve shakePostExposure, float remapPostExposureZero, float remapPostExposureOne, AnimationCurve shakeHueShift, float remapHueShiftZero, float remapHueShiftOne, AnimationCurve shakeSaturation, float remapSaturationZero, float remapSaturationOne, AnimationCurve shakeContrast, float remapContrastZero, float remapContrastOne, MMColorAdjustmentsShaker_URP.ColorFilterModes colorFilterMode, Gradient colorFilterGradient, Color colorFilterDestination, AnimationCurve colorFilterCurve, float duration, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalRelativeValues = this.RelativeValues;
				this._originalShakePostExposure = this.ShakePostExposure;
				this._originalRemapPostExposureZero = this.RemapPostExposureZero;
				this._originalRemapPostExposureOne = this.RemapPostExposureOne;
				this._originalShakeHueShift = this.ShakeHueShift;
				this._originalRemapHueShiftZero = this.RemapHueShiftZero;
				this._originalRemapHueShiftOne = this.RemapHueShiftOne;
				this._originalShakeSaturation = this.ShakeSaturation;
				this._originalRemapSaturationZero = this.RemapSaturationZero;
				this._originalRemapSaturationOne = this.RemapSaturationOne;
				this._originalShakeContrast = this.ShakeContrast;
				this._originalRemapContrastZero = this.RemapContrastZero;
				this._originalRemapContrastOne = this.RemapContrastOne;
				this._originalColorFilterMode = this.ColorFilterMode;
				this._originalColorFilterGradient = this.ColorFilterGradient;
				this._originalColorFilterDestination = this.ColorFilterDestination;
				this._originalColorFilterCurve = this.ColorFilterCurve;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.RelativeValues = relativeValues;
				this.ShakePostExposure = shakePostExposure;
				this.RemapPostExposureZero = remapPostExposureZero;
				this.RemapPostExposureOne = remapPostExposureOne;
				this.ShakeHueShift = shakeHueShift;
				this.RemapHueShiftZero = remapHueShiftZero;
				this.RemapHueShiftOne = remapHueShiftOne;
				this.ShakeSaturation = shakeSaturation;
				this.RemapSaturationZero = remapSaturationZero;
				this.RemapSaturationOne = remapSaturationOne;
				this.ShakeContrast = shakeContrast;
				this.RemapContrastZero = remapContrastZero;
				this.RemapContrastOne = remapContrastOne;
				this.ColorFilterMode = colorFilterMode;
				this.ColorFilterGradient = colorFilterGradient;
				this.ColorFilterDestination = colorFilterDestination;
				this.ColorFilterCurve = colorFilterCurve;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x0006475C File Offset: 0x0006295C
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._colorAdjustments.postExposure.Override(this._initialPostExposure);
			this._colorAdjustments.hueShift.Override(this._initialHueShift);
			this._colorAdjustments.saturation.Override(this._initialSaturation);
			this._colorAdjustments.contrast.Override(this._initialContrast);
			this._colorAdjustments.colorFilter.Override(this._initialColorFilterColor);
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x000647E0 File Offset: 0x000629E0
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.RelativeValues = this._originalRelativeValues;
			this.ShakePostExposure = this._originalShakePostExposure;
			this.RemapPostExposureZero = this._originalRemapPostExposureZero;
			this.RemapPostExposureOne = this._originalRemapPostExposureOne;
			this.ShakeHueShift = this._originalShakeHueShift;
			this.RemapHueShiftZero = this._originalRemapHueShiftZero;
			this.RemapHueShiftOne = this._originalRemapHueShiftOne;
			this.ShakeSaturation = this._originalShakeSaturation;
			this.RemapSaturationZero = this._originalRemapSaturationZero;
			this.RemapSaturationOne = this._originalRemapSaturationOne;
			this.ShakeContrast = this._originalShakeContrast;
			this.RemapContrastZero = this._originalRemapContrastZero;
			this.RemapContrastOne = this._originalRemapContrastOne;
			this.ColorFilterMode = this._originalColorFilterMode;
			this.ColorFilterGradient = this._originalColorFilterGradient;
			this.ColorFilterDestination = this._originalColorFilterDestination;
			this.ColorFilterCurve = this._originalColorFilterCurve;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x000648CB File Offset: 0x00062ACB
		public override void StartListening()
		{
			base.StartListening();
			MMColorAdjustmentsShakeEvent_URP.Register(new MMColorAdjustmentsShakeEvent_URP.Delegate(this.OnMMColorGradingShakeEvent));
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x000648E5 File Offset: 0x00062AE5
		public override void StopListening()
		{
			base.StopListening();
			MMColorAdjustmentsShakeEvent_URP.Unregister(new MMColorAdjustmentsShakeEvent_URP.Delegate(this.OnMMColorGradingShakeEvent));
		}

		// Token: 0x04001B42 RID: 6978
		public bool RelativeValues = true;

		// Token: 0x04001B43 RID: 6979
		[MMInspectorGroup("Post Exposure", true, 43, false)]
		[Tooltip("the curve used to animate the focus distance value on")]
		public AnimationCurve ShakePostExposure = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B44 RID: 6980
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapPostExposureZero;

		// Token: 0x04001B45 RID: 6981
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapPostExposureOne = 1f;

		// Token: 0x04001B46 RID: 6982
		[MMInspectorGroup("Hue Shift", true, 44, false)]
		[Tooltip("the curve used to animate the aperture value on")]
		public AnimationCurve ShakeHueShift = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B47 RID: 6983
		[Range(-180f, 180f)]
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapHueShiftZero;

		// Token: 0x04001B48 RID: 6984
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-180f, 180f)]
		public float RemapHueShiftOne = 180f;

		// Token: 0x04001B49 RID: 6985
		[MMInspectorGroup("Saturation", true, 45, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeSaturation = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B4A RID: 6986
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationZero;

		// Token: 0x04001B4B RID: 6987
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationOne = 100f;

		// Token: 0x04001B4C RID: 6988
		[MMInspectorGroup("Contrast", true, 47, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeContrast = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B4D RID: 6989
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapContrastZero;

		// Token: 0x04001B4E RID: 6990
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapContrastOne = 100f;

		// Token: 0x04001B4F RID: 6991
		[MMInspectorGroup("Color Filter", true, 48, false)]
		[Tooltip("the color filter mode to work with (none, over a gradient, or interpolate to a destination color")]
		public MMColorAdjustmentsShaker_URP.ColorFilterModes ColorFilterMode;

		// Token: 0x04001B50 RID: 6992
		[Tooltip("the gradient over which to modify the color filter")]
		[MMFEnumCondition("ColorFilterMode", new int[] { 1 })]
		[GradientUsage(true)]
		public Gradient ColorFilterGradient;

		// Token: 0x04001B51 RID: 6993
		[Tooltip("the destination color to match when in Interpolate mode")]
		[MMFEnumCondition("ColorFilterMode", new int[] { 2 })]
		public Color ColorFilterDestination = Color.yellow;

		// Token: 0x04001B52 RID: 6994
		[Tooltip("the curve over which to interpolate the color filter")]
		[MMFEnumCondition("ColorFilterMode", new int[] { 2 })]
		public AnimationCurve ColorFilterCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B53 RID: 6995
		protected Volume _volume;

		// Token: 0x04001B54 RID: 6996
		protected ColorAdjustments _colorAdjustments;

		// Token: 0x04001B55 RID: 6997
		protected float _initialPostExposure;

		// Token: 0x04001B56 RID: 6998
		protected float _initialHueShift;

		// Token: 0x04001B57 RID: 6999
		protected float _initialSaturation;

		// Token: 0x04001B58 RID: 7000
		protected float _initialContrast;

		// Token: 0x04001B59 RID: 7001
		protected Color _initialColorFilterColor;

		// Token: 0x04001B5A RID: 7002
		protected float _originalShakeDuration;

		// Token: 0x04001B5B RID: 7003
		protected bool _originalRelativeValues;

		// Token: 0x04001B5C RID: 7004
		protected AnimationCurve _originalShakePostExposure;

		// Token: 0x04001B5D RID: 7005
		protected float _originalRemapPostExposureZero;

		// Token: 0x04001B5E RID: 7006
		protected float _originalRemapPostExposureOne;

		// Token: 0x04001B5F RID: 7007
		protected AnimationCurve _originalShakeHueShift;

		// Token: 0x04001B60 RID: 7008
		protected float _originalRemapHueShiftZero;

		// Token: 0x04001B61 RID: 7009
		protected float _originalRemapHueShiftOne;

		// Token: 0x04001B62 RID: 7010
		protected AnimationCurve _originalShakeSaturation;

		// Token: 0x04001B63 RID: 7011
		protected float _originalRemapSaturationZero;

		// Token: 0x04001B64 RID: 7012
		protected float _originalRemapSaturationOne;

		// Token: 0x04001B65 RID: 7013
		protected AnimationCurve _originalShakeContrast;

		// Token: 0x04001B66 RID: 7014
		protected float _originalRemapContrastZero;

		// Token: 0x04001B67 RID: 7015
		protected float _originalRemapContrastOne;

		// Token: 0x04001B68 RID: 7016
		protected MMColorAdjustmentsShaker_URP.ColorFilterModes _originalColorFilterMode;

		// Token: 0x04001B69 RID: 7017
		protected Gradient _originalColorFilterGradient;

		// Token: 0x04001B6A RID: 7018
		protected Color _originalColorFilterDestination;

		// Token: 0x04001B6B RID: 7019
		protected AnimationCurve _originalColorFilterCurve;

		// Token: 0x0200058B RID: 1419
		public enum ColorFilterModes
		{
			// Token: 0x040024EA RID: 9450
			None,
			// Token: 0x040024EB RID: 9451
			Gradient,
			// Token: 0x040024EC RID: 9452
			Interpolate
		}
	}
}
