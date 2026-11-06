using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200032E RID: 814
	public struct MMLensDistortionShakeEvent
	{
		// Token: 0x14000042 RID: 66
		// (add) Token: 0x0600166B RID: 5739 RVA: 0x0005F17C File Offset: 0x0005D37C
		// (remove) Token: 0x0600166C RID: 5740 RVA: 0x0005F1B0 File Offset: 0x0005D3B0
		private static event MMLensDistortionShakeEvent.Delegate OnEvent;

		// Token: 0x0600166D RID: 5741 RVA: 0x0005F1E3 File Offset: 0x0005D3E3
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMLensDistortionShakeEvent.OnEvent = null;
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x0005F1EB File Offset: 0x0005D3EB
		public static void Register(MMLensDistortionShakeEvent.Delegate callback)
		{
			MMLensDistortionShakeEvent.OnEvent += callback;
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x0005F1F3 File Offset: 0x0005D3F3
		public static void Unregister(MMLensDistortionShakeEvent.Delegate callback)
		{
			MMLensDistortionShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x0005F1FC File Offset: 0x0005D3FC
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMLensDistortionShakeEvent.Delegate onEvent = MMLensDistortionShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000577 RID: 1399
		// (Invoke) Token: 0x06001D68 RID: 7528
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
