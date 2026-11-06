using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000307 RID: 775
	public struct MMExposureShakeEvent_HDRP
	{
		// Token: 0x14000037 RID: 55
		// (add) Token: 0x060015BC RID: 5564 RVA: 0x0005C9D8 File Offset: 0x0005ABD8
		// (remove) Token: 0x060015BD RID: 5565 RVA: 0x0005CA0C File Offset: 0x0005AC0C
		private static event MMExposureShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x060015BE RID: 5566 RVA: 0x0005CA3F File Offset: 0x0005AC3F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMExposureShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x0005CA47 File Offset: 0x0005AC47
		public static void Register(MMExposureShakeEvent_HDRP.Delegate callback)
		{
			MMExposureShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0005CA4F File Offset: 0x0005AC4F
		public static void Unregister(MMExposureShakeEvent_HDRP.Delegate callback)
		{
			MMExposureShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0005CA58 File Offset: 0x0005AC58
		public static void Trigger(AnimationCurve fixedExposure, float duration, float remapMin, float remapMax, bool relativeFixedExposure = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMExposureShakeEvent_HDRP.Delegate onEvent = MMExposureShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(fixedExposure, duration, remapMin, remapMax, relativeFixedExposure, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000566 RID: 1382
		// (Invoke) Token: 0x06001D36 RID: 7478
		public delegate void Delegate(AnimationCurve fixedExposure, float duration, float remapMin, float remapMax, bool relativeFixedExposure = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
