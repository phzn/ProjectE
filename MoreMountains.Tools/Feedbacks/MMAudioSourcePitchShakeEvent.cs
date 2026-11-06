using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000231 RID: 561
	public struct MMAudioSourcePitchShakeEvent
	{
		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000F7E RID: 3966 RVA: 0x000453CC File Offset: 0x000435CC
		// (remove) Token: 0x06000F7F RID: 3967 RVA: 0x00045400 File Offset: 0x00043600
		private static event MMAudioSourcePitchShakeEvent.Delegate OnEvent;

		// Token: 0x06000F80 RID: 3968 RVA: 0x00045433 File Offset: 0x00043633
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMAudioSourcePitchShakeEvent.OnEvent = null;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0004543B File Offset: 0x0004363B
		public static void Register(MMAudioSourcePitchShakeEvent.Delegate callback)
		{
			MMAudioSourcePitchShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x00045443 File Offset: 0x00043643
		public static void Unregister(MMAudioSourcePitchShakeEvent.Delegate callback)
		{
			MMAudioSourcePitchShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0004544C File Offset: 0x0004364C
		public static void Trigger(AnimationCurve pitchCurve, float duration, float remapMin, float remapMax, bool relativePitch = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMAudioSourcePitchShakeEvent.Delegate onEvent = MMAudioSourcePitchShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(pitchCurve, duration, remapMin, remapMax, relativePitch, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000510 RID: 1296
		// (Invoke) Token: 0x06001C4F RID: 7247
		public delegate void Delegate(AnimationCurve pitchCurve, float duration, float remapMin, float remapMax, bool relativePitch = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
