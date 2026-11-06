using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000229 RID: 553
	public struct MMAudioFilterEchoShakeEvent
	{
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000F3E RID: 3902 RVA: 0x00044728 File Offset: 0x00042928
		// (remove) Token: 0x06000F3F RID: 3903 RVA: 0x0004475C File Offset: 0x0004295C
		private static event MMAudioFilterEchoShakeEvent.Delegate OnEvent;

		// Token: 0x06000F40 RID: 3904 RVA: 0x0004478F File Offset: 0x0004298F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMAudioFilterEchoShakeEvent.OnEvent = null;
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00044797 File Offset: 0x00042997
		public static void Register(MMAudioFilterEchoShakeEvent.Delegate callback)
		{
			MMAudioFilterEchoShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0004479F File Offset: 0x0004299F
		public static void Unregister(MMAudioFilterEchoShakeEvent.Delegate callback)
		{
			MMAudioFilterEchoShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x000447A8 File Offset: 0x000429A8
		public static void Trigger(AnimationCurve echoCurve, float duration, float remapMin, float remapMax, bool relativeEcho = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMAudioFilterEchoShakeEvent.Delegate onEvent = MMAudioFilterEchoShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(echoCurve, duration, remapMin, remapMax, relativeEcho, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200050C RID: 1292
		// (Invoke) Token: 0x06001C3F RID: 7231
		public delegate void Delegate(AnimationCurve echoCurve, float duration, float remapMin, float remapMax, bool relativeEcho = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
