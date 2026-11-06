using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200022F RID: 559
	public struct MMAudioFilterReverbShakeEvent
	{
		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000F6E RID: 3950 RVA: 0x000450A8 File Offset: 0x000432A8
		// (remove) Token: 0x06000F6F RID: 3951 RVA: 0x000450DC File Offset: 0x000432DC
		private static event MMAudioFilterReverbShakeEvent.Delegate OnEvent;

		// Token: 0x06000F70 RID: 3952 RVA: 0x0004510F File Offset: 0x0004330F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMAudioFilterReverbShakeEvent.OnEvent = null;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00045117 File Offset: 0x00043317
		public static void Register(MMAudioFilterReverbShakeEvent.Delegate callback)
		{
			MMAudioFilterReverbShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0004511F File Offset: 0x0004331F
		public static void Unregister(MMAudioFilterReverbShakeEvent.Delegate callback)
		{
			MMAudioFilterReverbShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00045128 File Offset: 0x00043328
		public static void Trigger(AnimationCurve reverbCurve, float duration, float remapMin, float remapMax, bool relativeReverb = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMAudioFilterReverbShakeEvent.Delegate onEvent = MMAudioFilterReverbShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(reverbCurve, duration, remapMin, remapMax, relativeReverb, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200050F RID: 1295
		// (Invoke) Token: 0x06001C4B RID: 7243
		public delegate void Delegate(AnimationCurve reverbCurve, float duration, float remapMin, float remapMax, bool relativeReverb = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
