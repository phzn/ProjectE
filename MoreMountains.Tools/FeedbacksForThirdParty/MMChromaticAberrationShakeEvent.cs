using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000328 RID: 808
	public struct MMChromaticAberrationShakeEvent
	{
		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06001656 RID: 5718 RVA: 0x0005EB7C File Offset: 0x0005CD7C
		// (remove) Token: 0x06001657 RID: 5719 RVA: 0x0005EBB0 File Offset: 0x0005CDB0
		private static event MMChromaticAberrationShakeEvent.Delegate OnEvent;

		// Token: 0x06001658 RID: 5720 RVA: 0x0005EBE3 File Offset: 0x0005CDE3
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMChromaticAberrationShakeEvent.OnEvent = null;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0005EBEB File Offset: 0x0005CDEB
		public static void Register(MMChromaticAberrationShakeEvent.Delegate callback)
		{
			MMChromaticAberrationShakeEvent.OnEvent += callback;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x0005EBF3 File Offset: 0x0005CDF3
		public static void Unregister(MMChromaticAberrationShakeEvent.Delegate callback)
		{
			MMChromaticAberrationShakeEvent.OnEvent -= callback;
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0005EBFC File Offset: 0x0005CDFC
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMChromaticAberrationShakeEvent.Delegate onEvent = MMChromaticAberrationShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000574 RID: 1396
		// (Invoke) Token: 0x06001D5C RID: 7516
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
