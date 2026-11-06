using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000313 RID: 787
	public struct MMWhiteBalanceShakeEvent_HDRP
	{
		// Token: 0x1400003D RID: 61
		// (add) Token: 0x060015E6 RID: 5606 RVA: 0x0005D24C File Offset: 0x0005B44C
		// (remove) Token: 0x060015E7 RID: 5607 RVA: 0x0005D280 File Offset: 0x0005B480
		private static event MMWhiteBalanceShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x060015E8 RID: 5608 RVA: 0x0005D2B3 File Offset: 0x0005B4B3
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMWhiteBalanceShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x0005D2BB File Offset: 0x0005B4BB
		public static void Register(MMWhiteBalanceShakeEvent_HDRP.Delegate callback)
		{
			MMWhiteBalanceShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x0005D2C3 File Offset: 0x0005B4C3
		public static void Unregister(MMWhiteBalanceShakeEvent_HDRP.Delegate callback)
		{
			MMWhiteBalanceShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x0005D2CC File Offset: 0x0005B4CC
		public static void Trigger(AnimationCurve temperature, float duration, float remapTemperatureMin, float remapTemperatureMax, AnimationCurve tint, float remapTintMin, float remapTintMax, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMWhiteBalanceShakeEvent_HDRP.Delegate onEvent = MMWhiteBalanceShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(temperature, duration, remapTemperatureMin, remapTemperatureMax, tint, remapTintMin, remapTintMax, relativeValues, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200056C RID: 1388
		// (Invoke) Token: 0x06001D4E RID: 7502
		public delegate void Delegate(AnimationCurve temperature, float duration, float remapTemperatureMin, float remapTemperatureMax, AnimationCurve tint, float remapTintMin, float remapTintMax, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
