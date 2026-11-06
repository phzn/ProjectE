using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000326 RID: 806
	public struct MMBloomShakeEvent
	{
		// Token: 0x1400003E RID: 62
		// (add) Token: 0x0600164F RID: 5711 RVA: 0x0005EA50 File Offset: 0x0005CC50
		// (remove) Token: 0x06001650 RID: 5712 RVA: 0x0005EA84 File Offset: 0x0005CC84
		private static event MMBloomShakeEvent.Delegate OnEvent;

		// Token: 0x06001651 RID: 5713 RVA: 0x0005EAB7 File Offset: 0x0005CCB7
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMBloomShakeEvent.OnEvent = null;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x0005EABF File Offset: 0x0005CCBF
		public static void Register(MMBloomShakeEvent.Delegate callback)
		{
			MMBloomShakeEvent.OnEvent += callback;
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x0005EAC7 File Offset: 0x0005CCC7
		public static void Unregister(MMBloomShakeEvent.Delegate callback)
		{
			MMBloomShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x0005EAD0 File Offset: 0x0005CCD0
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, AnimationCurve threshold, float remapThresholdMin, float remapThresholdMax, bool relativeIntensity = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMBloomShakeEvent.Delegate onEvent = MMBloomShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, threshold, remapThresholdMin, remapThresholdMax, relativeIntensity, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000573 RID: 1395
		// (Invoke) Token: 0x06001D58 RID: 7512
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, AnimationCurve threshold, float remapThresholdMin, float remapThresholdMax, bool relativeIntensity = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
