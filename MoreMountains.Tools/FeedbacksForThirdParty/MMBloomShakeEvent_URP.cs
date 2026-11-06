using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000358 RID: 856
	public struct MMBloomShakeEvent_URP
	{
		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06001781 RID: 6017 RVA: 0x00063994 File Offset: 0x00061B94
		// (remove) Token: 0x06001782 RID: 6018 RVA: 0x000639C8 File Offset: 0x00061BC8
		private static event MMBloomShakeEvent_URP.Delegate OnEvent;

		// Token: 0x06001783 RID: 6019 RVA: 0x000639FB File Offset: 0x00061BFB
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMBloomShakeEvent_URP.OnEvent = null;
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00063A03 File Offset: 0x00061C03
		public static void Register(MMBloomShakeEvent_URP.Delegate callback)
		{
			MMBloomShakeEvent_URP.OnEvent += callback;
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00063A0B File Offset: 0x00061C0B
		public static void Unregister(MMBloomShakeEvent_URP.Delegate callback)
		{
			MMBloomShakeEvent_URP.OnEvent -= callback;
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00063A14 File Offset: 0x00061C14
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, AnimationCurve threshold, float remapThresholdMin, float remapThresholdMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMBloomShakeEvent_URP.Delegate onEvent = MMBloomShakeEvent_URP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, threshold, remapThresholdMin, remapThresholdMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000588 RID: 1416
		// (Invoke) Token: 0x06001D88 RID: 7560
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, AnimationCurve threshold, float remapThresholdMin, float remapThresholdMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
