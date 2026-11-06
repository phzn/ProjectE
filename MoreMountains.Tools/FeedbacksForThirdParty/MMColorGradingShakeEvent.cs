using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200032A RID: 810
	public struct MMColorGradingShakeEvent
	{
		// Token: 0x14000040 RID: 64
		// (add) Token: 0x0600165D RID: 5725 RVA: 0x0005EDC4 File Offset: 0x0005CFC4
		// (remove) Token: 0x0600165E RID: 5726 RVA: 0x0005EDF8 File Offset: 0x0005CFF8
		private static event MMColorGradingShakeEvent.Delegate OnEvent;

		// Token: 0x0600165F RID: 5727 RVA: 0x0005EE2B File Offset: 0x0005D02B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMColorGradingShakeEvent.OnEvent = null;
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x0005EE33 File Offset: 0x0005D033
		public static void Register(MMColorGradingShakeEvent.Delegate callback)
		{
			MMColorGradingShakeEvent.OnEvent += callback;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x0005EE3B File Offset: 0x0005D03B
		public static void Unregister(MMColorGradingShakeEvent.Delegate callback)
		{
			MMColorGradingShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x0005EE44 File Offset: 0x0005D044
		public static void Trigger(AnimationCurve shakePostExposure, float remapPostExposureZero, float remapPostExposureOne, AnimationCurve shakeHueShift, float remapHueShiftZero, float remapHueShiftOne, AnimationCurve shakeSaturation, float remapSaturationZero, float remapSaturationOne, AnimationCurve shakeContrast, float remapContrastZero, float remapContrastOne, bool shakeColorFilter, Gradient colorFilterGradient, float duration, bool relativeValues = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMColorGradingShakeEvent.Delegate onEvent = MMColorGradingShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(shakePostExposure, remapPostExposureZero, remapPostExposureOne, shakeHueShift, remapHueShiftZero, remapHueShiftOne, shakeSaturation, remapSaturationZero, remapSaturationOne, shakeContrast, remapContrastZero, remapContrastOne, shakeColorFilter, colorFilterGradient, duration, relativeValues, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000575 RID: 1397
		// (Invoke) Token: 0x06001D60 RID: 7520
		public delegate void Delegate(AnimationCurve shakePostExposure, float remapPostExposureZero, float remapPostExposureOne, AnimationCurve shakeHueShift, float remapHueShiftZero, float remapHueShiftOne, AnimationCurve shakeSaturation, float remapSaturationZero, float remapSaturationOne, AnimationCurve shakeContrast, float remapContrastZero, float remapContrastOne, bool shakeColorFilter, Gradient colorFilterGradient, float duration, bool relativeValues = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
