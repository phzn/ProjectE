using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200022D RID: 557
	public struct MMAudioFilterLowPassShakeEvent
	{
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000F5E RID: 3934 RVA: 0x00044D70 File Offset: 0x00042F70
		// (remove) Token: 0x06000F5F RID: 3935 RVA: 0x00044DA4 File Offset: 0x00042FA4
		private static event MMAudioFilterLowPassShakeEvent.Delegate OnEvent;

		// Token: 0x06000F60 RID: 3936 RVA: 0x00044DD7 File Offset: 0x00042FD7
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMAudioFilterLowPassShakeEvent.OnEvent = null;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x00044DDF File Offset: 0x00042FDF
		public static void Register(MMAudioFilterLowPassShakeEvent.Delegate callback)
		{
			MMAudioFilterLowPassShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00044DE7 File Offset: 0x00042FE7
		public static void Unregister(MMAudioFilterLowPassShakeEvent.Delegate callback)
		{
			MMAudioFilterLowPassShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00044DF0 File Offset: 0x00042FF0
		public static void Trigger(AnimationCurve lowPassCurve, float duration, float remapMin, float remapMax, bool relativeLowPass = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMAudioFilterLowPassShakeEvent.Delegate onEvent = MMAudioFilterLowPassShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(lowPassCurve, duration, remapMin, remapMax, relativeLowPass, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200050E RID: 1294
		// (Invoke) Token: 0x06001C47 RID: 7239
		public delegate void Delegate(AnimationCurve lowPassCurve, float duration, float remapMin, float remapMax, bool relativeLowPass = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
