using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200022B RID: 555
	public struct MMAudioFilterHighPassShakeEvent
	{
		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000F4E RID: 3918 RVA: 0x00044A4C File Offset: 0x00042C4C
		// (remove) Token: 0x06000F4F RID: 3919 RVA: 0x00044A80 File Offset: 0x00042C80
		private static event MMAudioFilterHighPassShakeEvent.Delegate OnEvent;

		// Token: 0x06000F50 RID: 3920 RVA: 0x00044AB3 File Offset: 0x00042CB3
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMAudioFilterHighPassShakeEvent.OnEvent = null;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00044ABB File Offset: 0x00042CBB
		public static void Register(MMAudioFilterHighPassShakeEvent.Delegate callback)
		{
			MMAudioFilterHighPassShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00044AC3 File Offset: 0x00042CC3
		public static void Unregister(MMAudioFilterHighPassShakeEvent.Delegate callback)
		{
			MMAudioFilterHighPassShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00044ACC File Offset: 0x00042CCC
		public static void Trigger(AnimationCurve highPassCurve, float duration, float remapMin, float remapMax, bool relativeHighPass = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMAudioFilterHighPassShakeEvent.Delegate onEvent = MMAudioFilterHighPassShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(highPassCurve, duration, remapMin, remapMax, relativeHighPass, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200050D RID: 1293
		// (Invoke) Token: 0x06001C43 RID: 7235
		public delegate void Delegate(AnimationCurve highPassCurve, float duration, float remapMin, float remapMax, bool relativeHighPass = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
