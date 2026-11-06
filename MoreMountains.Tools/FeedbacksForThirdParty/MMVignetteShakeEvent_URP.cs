using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200036A RID: 874
	public struct MMVignetteShakeEvent_URP
	{
		// Token: 0x1400004D RID: 77
		// (add) Token: 0x0600180C RID: 6156 RVA: 0x000662E4 File Offset: 0x000644E4
		// (remove) Token: 0x0600180D RID: 6157 RVA: 0x00066318 File Offset: 0x00064518
		private static event MMVignetteShakeEvent_URP.Delegate OnEvent;

		// Token: 0x0600180E RID: 6158 RVA: 0x0006634B File Offset: 0x0006454B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMVignetteShakeEvent_URP.OnEvent = null;
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x00066353 File Offset: 0x00064553
		public static void Register(MMVignetteShakeEvent_URP.Delegate callback)
		{
			MMVignetteShakeEvent_URP.OnEvent += callback;
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x0006635B File Offset: 0x0006455B
		public static void Unregister(MMVignetteShakeEvent_URP.Delegate callback)
		{
			MMVignetteShakeEvent_URP.OnEvent -= callback;
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x00066364 File Offset: 0x00064564
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false, bool interpolateColor = false, AnimationCurve colorCurve = null, float remapColorZero = 0f, float remapColorOne = 1f, Color targetColor = default(Color))
		{
			MMVignetteShakeEvent_URP.Delegate onEvent = MMVignetteShakeEvent_URP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore, interpolateColor, colorCurve, remapColorZero, remapColorOne, targetColor);
		}

		// Token: 0x02000592 RID: 1426
		// (Invoke) Token: 0x06001DAC RID: 7596
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false, bool interpolateColor = false, AnimationCurve colorCurve = null, float remapColorZero = 0f, float remapColorOne = 1f, Color targetColor = default(Color));
	}
}
