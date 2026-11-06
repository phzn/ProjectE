using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200023A RID: 570
	public struct MMCameraClippingPlanesShakeEvent
	{
		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000FC4 RID: 4036 RVA: 0x00046738 File Offset: 0x00044938
		// (remove) Token: 0x06000FC5 RID: 4037 RVA: 0x0004676C File Offset: 0x0004496C
		private static event MMCameraClippingPlanesShakeEvent.Delegate OnEvent;

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0004679F File Offset: 0x0004499F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMCameraClippingPlanesShakeEvent.OnEvent = null;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x000467A7 File Offset: 0x000449A7
		public static void Register(MMCameraClippingPlanesShakeEvent.Delegate callback)
		{
			MMCameraClippingPlanesShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x000467AF File Offset: 0x000449AF
		public static void Unregister(MMCameraClippingPlanesShakeEvent.Delegate callback)
		{
			MMCameraClippingPlanesShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x000467B8 File Offset: 0x000449B8
		public static void Trigger(AnimationCurve animNearCurve, float duration, float remapNearMin, float remapNearMax, AnimationCurve animFarCurve, float remapFarMin, float remapFarMax, bool relativeValue = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMCameraClippingPlanesShakeEvent.Delegate onEvent = MMCameraClippingPlanesShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(animNearCurve, duration, remapNearMin, remapNearMax, animFarCurve, remapFarMin, remapFarMax, relativeValue, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000515 RID: 1301
		// (Invoke) Token: 0x06001C5B RID: 7259
		public delegate void Delegate(AnimationCurve animNearCurve, float duration, float remapNearMin, float remapNearMax, AnimationCurve animFarCurve, float remapFarMin, float remapFarMax, bool relativeValue = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
