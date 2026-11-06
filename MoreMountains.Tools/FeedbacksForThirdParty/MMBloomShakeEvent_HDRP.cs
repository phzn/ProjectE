using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002FD RID: 765
	public struct MMBloomShakeEvent_HDRP
	{
		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06001599 RID: 5529 RVA: 0x0005C008 File Offset: 0x0005A208
		// (remove) Token: 0x0600159A RID: 5530 RVA: 0x0005C03C File Offset: 0x0005A23C
		private static event MMBloomShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x0600159B RID: 5531 RVA: 0x0005C06F File Offset: 0x0005A26F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMBloomShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x0005C077 File Offset: 0x0005A277
		public static void Register(MMBloomShakeEvent_HDRP.Delegate callback)
		{
			MMBloomShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x0005C07F File Offset: 0x0005A27F
		public static void Unregister(MMBloomShakeEvent_HDRP.Delegate callback)
		{
			MMBloomShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x0005C088 File Offset: 0x0005A288
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, AnimationCurve threshold, float remapThresholdMin, float remapThresholdMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMBloomShakeEvent_HDRP.Delegate onEvent = MMBloomShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, threshold, remapThresholdMin, remapThresholdMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000560 RID: 1376
		// (Invoke) Token: 0x06001D22 RID: 7458
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, AnimationCurve threshold, float remapThresholdMin, float remapThresholdMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
