using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000227 RID: 551
	public struct MMAudioFilterDistortionShakeEvent
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000F2E RID: 3886 RVA: 0x00044404 File Offset: 0x00042604
		// (remove) Token: 0x06000F2F RID: 3887 RVA: 0x00044438 File Offset: 0x00042638
		private static event MMAudioFilterDistortionShakeEvent.Delegate OnEvent;

		// Token: 0x06000F30 RID: 3888 RVA: 0x0004446B File Offset: 0x0004266B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMAudioFilterDistortionShakeEvent.OnEvent = null;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00044473 File Offset: 0x00042673
		public static void Register(MMAudioFilterDistortionShakeEvent.Delegate callback)
		{
			MMAudioFilterDistortionShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0004447B File Offset: 0x0004267B
		public static void Unregister(MMAudioFilterDistortionShakeEvent.Delegate callback)
		{
			MMAudioFilterDistortionShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00044484 File Offset: 0x00042684
		public static void Trigger(AnimationCurve distortionCurve, float duration, float remapMin, float remapMax, bool relativeDistortion = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMAudioFilterDistortionShakeEvent.Delegate onEvent = MMAudioFilterDistortionShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(distortionCurve, duration, remapMin, remapMax, relativeDistortion, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200050B RID: 1291
		// (Invoke) Token: 0x06001C3B RID: 7227
		public delegate void Delegate(AnimationCurve distortionCurve, float duration, float remapMin, float remapMax, bool relativeDistortion = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
