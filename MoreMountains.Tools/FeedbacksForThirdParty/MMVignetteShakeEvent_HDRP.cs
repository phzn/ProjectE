using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000311 RID: 785
	public struct MMVignetteShakeEvent_HDRP
	{
		// Token: 0x1400003C RID: 60
		// (add) Token: 0x060015DF RID: 5599 RVA: 0x0005D0B8 File Offset: 0x0005B2B8
		// (remove) Token: 0x060015E0 RID: 5600 RVA: 0x0005D0EC File Offset: 0x0005B2EC
		private static event MMVignetteShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x060015E1 RID: 5601 RVA: 0x0005D11F File Offset: 0x0005B31F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMVignetteShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x0005D127 File Offset: 0x0005B327
		public static void Register(MMVignetteShakeEvent_HDRP.Delegate callback)
		{
			MMVignetteShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x0005D12F File Offset: 0x0005B32F
		public static void Unregister(MMVignetteShakeEvent_HDRP.Delegate callback)
		{
			MMVignetteShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x0005D138 File Offset: 0x0005B338
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false, bool interpolateColor = false, AnimationCurve colorCurve = null, float remapColorZero = 0f, float remapColorOne = 1f, Color targetColor = default(Color))
		{
			MMVignetteShakeEvent_HDRP.Delegate onEvent = MMVignetteShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore, interpolateColor, colorCurve, remapColorZero, remapColorOne, targetColor);
		}

		// Token: 0x0200056B RID: 1387
		// (Invoke) Token: 0x06001D4A RID: 7498
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false, bool interpolateColor = false, AnimationCurve colorCurve = null, float remapColorZero = 0f, float remapColorOne = 1f, Color targetColor = default(Color));
	}
}
