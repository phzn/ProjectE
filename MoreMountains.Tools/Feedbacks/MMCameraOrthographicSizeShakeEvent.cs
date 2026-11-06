using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200023E RID: 574
	public struct MMCameraOrthographicSizeShakeEvent
	{
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06000FE4 RID: 4068 RVA: 0x00046D98 File Offset: 0x00044F98
		// (remove) Token: 0x06000FE5 RID: 4069 RVA: 0x00046DCC File Offset: 0x00044FCC
		private static event MMCameraOrthographicSizeShakeEvent.Delegate OnEvent;

		// Token: 0x06000FE6 RID: 4070 RVA: 0x00046DFF File Offset: 0x00044FFF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMCameraOrthographicSizeShakeEvent.OnEvent = null;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00046E07 File Offset: 0x00045007
		public static void Register(MMCameraOrthographicSizeShakeEvent.Delegate callback)
		{
			MMCameraOrthographicSizeShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x00046E0F File Offset: 0x0004500F
		public static void Unregister(MMCameraOrthographicSizeShakeEvent.Delegate callback)
		{
			MMCameraOrthographicSizeShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00046E18 File Offset: 0x00045018
		public static void Trigger(AnimationCurve animCurve, float duration, float remapMin, float remapMax, bool relativeValue = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMCameraOrthographicSizeShakeEvent.Delegate onEvent = MMCameraOrthographicSizeShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(animCurve, duration, remapMin, remapMax, relativeValue, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000517 RID: 1303
		// (Invoke) Token: 0x06001C63 RID: 7267
		public delegate void Delegate(AnimationCurve animCurve, float duration, float remapMin, float remapMax, bool relativeValue = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
