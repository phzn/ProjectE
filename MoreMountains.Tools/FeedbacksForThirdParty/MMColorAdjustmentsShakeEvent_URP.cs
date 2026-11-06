using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200035E RID: 862
	public struct MMColorAdjustmentsShakeEvent_URP
	{
		// Token: 0x14000047 RID: 71
		// (add) Token: 0x060017B0 RID: 6064 RVA: 0x00064AF0 File Offset: 0x00062CF0
		// (remove) Token: 0x060017B1 RID: 6065 RVA: 0x00064B24 File Offset: 0x00062D24
		private static event MMColorAdjustmentsShakeEvent_URP.Delegate OnEvent;

		// Token: 0x060017B2 RID: 6066 RVA: 0x00064B57 File Offset: 0x00062D57
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMColorAdjustmentsShakeEvent_URP.OnEvent = null;
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x00064B5F File Offset: 0x00062D5F
		public static void Register(MMColorAdjustmentsShakeEvent_URP.Delegate callback)
		{
			MMColorAdjustmentsShakeEvent_URP.OnEvent += callback;
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x00064B67 File Offset: 0x00062D67
		public static void Unregister(MMColorAdjustmentsShakeEvent_URP.Delegate callback)
		{
			MMColorAdjustmentsShakeEvent_URP.OnEvent -= callback;
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00064B70 File Offset: 0x00062D70
		public static void Trigger(AnimationCurve shakePostExposure, float remapPostExposureZero, float remapPostExposureOne, AnimationCurve shakeHueShift, float remapHueShiftZero, float remapHueShiftOne, AnimationCurve shakeSaturation, float remapSaturationZero, float remapSaturationOne, AnimationCurve shakeContrast, float remapContrastZero, float remapContrastOne, MMColorAdjustmentsShaker_URP.ColorFilterModes colorFilterMode, Gradient colorFilterGradient, Color colorFilterDestination, AnimationCurve colorFilterCurve, float duration, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMColorAdjustmentsShakeEvent_URP.Delegate onEvent = MMColorAdjustmentsShakeEvent_URP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(shakePostExposure, remapPostExposureZero, remapPostExposureOne, shakeHueShift, remapHueShiftZero, remapHueShiftOne, shakeSaturation, remapSaturationZero, remapSaturationOne, shakeContrast, remapContrastZero, remapContrastOne, colorFilterMode, colorFilterGradient, colorFilterDestination, colorFilterCurve, duration, relativeValues, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200058C RID: 1420
		// (Invoke) Token: 0x06001D94 RID: 7572
		public delegate void Delegate(AnimationCurve shakePostExposure, float remapPostExposureZero, float remapPostExposureOne, AnimationCurve shakeHueShift, float remapHueShiftZero, float remapHueShiftOne, AnimationCurve shakeSaturation, float remapSaturationZero, float remapSaturationOne, AnimationCurve shakeContrast, float remapContrastZero, float remapContrastOne, MMColorAdjustmentsShaker_URP.ColorFilterModes colorFilterMode, Gradient colorFilterGradient, Color colorFilterDestination, AnimationCurve colorFilterCurve, float duration, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
