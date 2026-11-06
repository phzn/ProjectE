using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200036C RID: 876
	public struct MMWhiteBalanceShakeEvent_URP
	{
		// Token: 0x1400004E RID: 78
		// (add) Token: 0x0600181B RID: 6171 RVA: 0x00066750 File Offset: 0x00064950
		// (remove) Token: 0x0600181C RID: 6172 RVA: 0x00066784 File Offset: 0x00064984
		private static event MMWhiteBalanceShakeEvent_URP.Delegate OnEvent;

		// Token: 0x0600181D RID: 6173 RVA: 0x000667B7 File Offset: 0x000649B7
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMWhiteBalanceShakeEvent_URP.OnEvent = null;
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x000667BF File Offset: 0x000649BF
		public static void Register(MMWhiteBalanceShakeEvent_URP.Delegate callback)
		{
			MMWhiteBalanceShakeEvent_URP.OnEvent += callback;
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x000667C7 File Offset: 0x000649C7
		public static void Unregister(MMWhiteBalanceShakeEvent_URP.Delegate callback)
		{
			MMWhiteBalanceShakeEvent_URP.OnEvent -= callback;
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x000667D0 File Offset: 0x000649D0
		public static void Trigger(AnimationCurve temperature, float duration, float remapTemperatureMin, float remapTemperatureMax, AnimationCurve tint, float remapTintMin, float remapTintMax, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMWhiteBalanceShakeEvent_URP.Delegate onEvent = MMWhiteBalanceShakeEvent_URP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(temperature, duration, remapTemperatureMin, remapTemperatureMax, tint, remapTintMin, remapTintMax, relativeValues, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000593 RID: 1427
		// (Invoke) Token: 0x06001DB0 RID: 7600
		public delegate void Delegate(AnimationCurve temperature, float duration, float remapTemperatureMin, float remapTemperatureMax, AnimationCurve tint, float remapTintMin, float remapTintMax, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
