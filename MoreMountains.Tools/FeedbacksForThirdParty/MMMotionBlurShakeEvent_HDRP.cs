using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200030D RID: 781
	public struct MMMotionBlurShakeEvent_HDRP
	{
		// Token: 0x1400003A RID: 58
		// (add) Token: 0x060015D1 RID: 5585 RVA: 0x0005CDEC File Offset: 0x0005AFEC
		// (remove) Token: 0x060015D2 RID: 5586 RVA: 0x0005CE20 File Offset: 0x0005B020
		private static event MMMotionBlurShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x060015D3 RID: 5587 RVA: 0x0005CE53 File Offset: 0x0005B053
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMMotionBlurShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0005CE5B File Offset: 0x0005B05B
		public static void Register(MMMotionBlurShakeEvent_HDRP.Delegate callback)
		{
			MMMotionBlurShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0005CE63 File Offset: 0x0005B063
		public static void Unregister(MMMotionBlurShakeEvent_HDRP.Delegate callback)
		{
			MMMotionBlurShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x0005CE6C File Offset: 0x0005B06C
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMMotionBlurShakeEvent_HDRP.Delegate onEvent = MMMotionBlurShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000569 RID: 1385
		// (Invoke) Token: 0x06001D42 RID: 7490
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
