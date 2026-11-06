using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000233 RID: 563
	public struct MMAudioSourceStereoPanShakeEvent
	{
		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000F8E RID: 3982 RVA: 0x00045704 File Offset: 0x00043904
		// (remove) Token: 0x06000F8F RID: 3983 RVA: 0x00045738 File Offset: 0x00043938
		private static event MMAudioSourceStereoPanShakeEvent.Delegate OnEvent;

		// Token: 0x06000F90 RID: 3984 RVA: 0x0004576B File Offset: 0x0004396B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMAudioSourceStereoPanShakeEvent.OnEvent = null;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00045773 File Offset: 0x00043973
		public static void Register(MMAudioSourceStereoPanShakeEvent.Delegate callback)
		{
			MMAudioSourceStereoPanShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0004577B File Offset: 0x0004397B
		public static void Unregister(MMAudioSourceStereoPanShakeEvent.Delegate callback)
		{
			MMAudioSourceStereoPanShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00045784 File Offset: 0x00043984
		public static void Trigger(AnimationCurve stereoPanCurve, float duration, float remapMin, float remapMax, bool relativeStereoPan = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMAudioSourceStereoPanShakeEvent.Delegate onEvent = MMAudioSourceStereoPanShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(stereoPanCurve, duration, remapMin, remapMax, relativeStereoPan, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000511 RID: 1297
		// (Invoke) Token: 0x06001C53 RID: 7251
		public delegate void Delegate(AnimationCurve stereoPanCurve, float duration, float remapMin, float remapMax, bool relativeStereoPan = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
