using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200023C RID: 572
	public struct MMCameraFieldOfViewShakeEvent
	{
		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06000FD4 RID: 4052 RVA: 0x00046A6C File Offset: 0x00044C6C
		// (remove) Token: 0x06000FD5 RID: 4053 RVA: 0x00046AA0 File Offset: 0x00044CA0
		private static event MMCameraFieldOfViewShakeEvent.Delegate OnEvent;

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00046AD3 File Offset: 0x00044CD3
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMCameraFieldOfViewShakeEvent.OnEvent = null;
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00046ADB File Offset: 0x00044CDB
		public static void Register(MMCameraFieldOfViewShakeEvent.Delegate callback)
		{
			MMCameraFieldOfViewShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00046AE3 File Offset: 0x00044CE3
		public static void Unregister(MMCameraFieldOfViewShakeEvent.Delegate callback)
		{
			MMCameraFieldOfViewShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x00046AEC File Offset: 0x00044CEC
		public static void Trigger(AnimationCurve animCurve, float duration, float remapMin, float remapMax, bool relativeValue = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMCameraFieldOfViewShakeEvent.Delegate onEvent = MMCameraFieldOfViewShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(animCurve, duration, remapMin, remapMax, relativeValue, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000516 RID: 1302
		// (Invoke) Token: 0x06001C5F RID: 7263
		public delegate void Delegate(AnimationCurve animCurve, float duration, float remapMin, float remapMax, bool relativeValue = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
