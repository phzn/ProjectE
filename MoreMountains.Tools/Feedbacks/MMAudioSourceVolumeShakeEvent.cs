using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000235 RID: 565
	public struct MMAudioSourceVolumeShakeEvent
	{
		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000F9E RID: 3998 RVA: 0x00045A28 File Offset: 0x00043C28
		// (remove) Token: 0x06000F9F RID: 3999 RVA: 0x00045A5C File Offset: 0x00043C5C
		private static event MMAudioSourceVolumeShakeEvent.Delegate OnEvent;

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00045A8F File Offset: 0x00043C8F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMAudioSourceVolumeShakeEvent.OnEvent = null;
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00045A97 File Offset: 0x00043C97
		public static void Register(MMAudioSourceVolumeShakeEvent.Delegate callback)
		{
			MMAudioSourceVolumeShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00045A9F File Offset: 0x00043C9F
		public static void Unregister(MMAudioSourceVolumeShakeEvent.Delegate callback)
		{
			MMAudioSourceVolumeShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00045AA8 File Offset: 0x00043CA8
		public static void Trigger(AnimationCurve volumeCurve, float duration, float remapMin, float remapMax, bool relativeVolume = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMAudioSourceVolumeShakeEvent.Delegate onEvent = MMAudioSourceVolumeShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(volumeCurve, duration, remapMin, remapMax, relativeVolume, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000512 RID: 1298
		// (Invoke) Token: 0x06001C57 RID: 7255
		public delegate void Delegate(AnimationCurve volumeCurve, float duration, float remapMin, float remapMax, bool relativeVolume = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
