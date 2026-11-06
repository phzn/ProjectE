using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000303 RID: 771
	public struct MMColorAdjustmentsShakeEvent_HDRP
	{
		// Token: 0x14000035 RID: 53
		// (add) Token: 0x060015AE RID: 5550 RVA: 0x0005C5CC File Offset: 0x0005A7CC
		// (remove) Token: 0x060015AF RID: 5551 RVA: 0x0005C600 File Offset: 0x0005A800
		private static event MMColorAdjustmentsShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x060015B0 RID: 5552 RVA: 0x0005C633 File Offset: 0x0005A833
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMColorAdjustmentsShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0005C63B File Offset: 0x0005A83B
		public static void Register(MMColorAdjustmentsShakeEvent_HDRP.Delegate callback)
		{
			MMColorAdjustmentsShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0005C643 File Offset: 0x0005A843
		public static void Unregister(MMColorAdjustmentsShakeEvent_HDRP.Delegate callback)
		{
			MMColorAdjustmentsShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0005C64C File Offset: 0x0005A84C
		public static void Trigger(AnimationCurve shakePostExposure, float remapPostExposureZero, float remapPostExposureOne, AnimationCurve shakeHueShift, float remapHueShiftZero, float remapHueShiftOne, AnimationCurve shakeSaturation, float remapSaturationZero, float remapSaturationOne, AnimationCurve shakeContrast, float remapContrastZero, float remapContrastOne, MMColorAdjustmentsShaker_HDRP.ColorFilterModes colorFilterMode, Gradient colorFilterGradient, Color colorFilterDestination, AnimationCurve colorFilterCurve, float duration, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMColorAdjustmentsShakeEvent_HDRP.Delegate onEvent = MMColorAdjustmentsShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(shakePostExposure, remapPostExposureZero, remapPostExposureOne, shakeHueShift, remapHueShiftZero, remapHueShiftOne, shakeSaturation, remapSaturationZero, remapSaturationOne, shakeContrast, remapContrastZero, remapContrastOne, colorFilterMode, colorFilterGradient, colorFilterDestination, colorFilterCurve, duration, relativeValues, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000564 RID: 1380
		// (Invoke) Token: 0x06001D2E RID: 7470
		public delegate void Delegate(AnimationCurve shakePostExposure, float remapPostExposureZero, float remapPostExposureOne, AnimationCurve shakeHueShift, float remapHueShiftZero, float remapHueShiftOne, AnimationCurve shakeSaturation, float remapSaturationZero, float remapSaturationOne, AnimationCurve shakeContrast, float remapContrastZero, float remapContrastOne, MMColorAdjustmentsShaker_HDRP.ColorFilterModes colorFilterMode, Gradient colorFilterGradient, Color colorFilterDestination, AnimationCurve colorFilterCurve, float duration, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
