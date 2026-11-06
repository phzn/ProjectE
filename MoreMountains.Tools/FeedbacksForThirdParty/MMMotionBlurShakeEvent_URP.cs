using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000366 RID: 870
	public struct MMMotionBlurShakeEvent_URP
	{
		// Token: 0x1400004B RID: 75
		// (add) Token: 0x060017EE RID: 6126 RVA: 0x00065B00 File Offset: 0x00063D00
		// (remove) Token: 0x060017EF RID: 6127 RVA: 0x00065B34 File Offset: 0x00063D34
		private static event MMMotionBlurShakeEvent_URP.Delegate OnEvent;

		// Token: 0x060017F0 RID: 6128 RVA: 0x00065B67 File Offset: 0x00063D67
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMMotionBlurShakeEvent_URP.OnEvent = null;
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x00065B6F File Offset: 0x00063D6F
		public static void Register(MMMotionBlurShakeEvent_URP.Delegate callback)
		{
			MMMotionBlurShakeEvent_URP.OnEvent += callback;
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00065B77 File Offset: 0x00063D77
		public static void Unregister(MMMotionBlurShakeEvent_URP.Delegate callback)
		{
			MMMotionBlurShakeEvent_URP.OnEvent -= callback;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00065B80 File Offset: 0x00063D80
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMMotionBlurShakeEvent_URP.Delegate onEvent = MMMotionBlurShakeEvent_URP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000590 RID: 1424
		// (Invoke) Token: 0x06001DA4 RID: 7588
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
