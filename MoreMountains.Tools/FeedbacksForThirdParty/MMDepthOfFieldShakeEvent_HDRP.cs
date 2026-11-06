using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000305 RID: 773
	public struct MMDepthOfFieldShakeEvent_HDRP
	{
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x060015B5 RID: 5557 RVA: 0x0005C888 File Offset: 0x0005AA88
		// (remove) Token: 0x060015B6 RID: 5558 RVA: 0x0005C8BC File Offset: 0x0005AABC
		private static event MMDepthOfFieldShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x060015B7 RID: 5559 RVA: 0x0005C8EF File Offset: 0x0005AAEF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMDepthOfFieldShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0005C8F7 File Offset: 0x0005AAF7
		public static void Register(MMDepthOfFieldShakeEvent_HDRP.Delegate callback)
		{
			MMDepthOfFieldShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0005C8FF File Offset: 0x0005AAFF
		public static void Unregister(MMDepthOfFieldShakeEvent_HDRP.Delegate callback)
		{
			MMDepthOfFieldShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0005C908 File Offset: 0x0005AB08
		public static void Trigger(float duration, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false, bool animateFocusDistance = false, AnimationCurve shakeFocusDistance = null, float remapFocusDistanceZero = 0f, float remapFocusDistanceOne = 1f, bool animateNearRangeStart = false, AnimationCurve shakeNearRangeStart = null, float remapNearRangeStartZero = 0f, float remapNearRangeStartOne = 0f, bool animateNearRangeEnd = false, AnimationCurve shakeNearRangeEnd = null, float remapNearRangeEndZero = 0f, float remapNearRangeEndOne = 0f, bool animateFarRangeStart = false, AnimationCurve shakeFarRangeStart = null, float remapFarRangeStartZero = 0f, float remapFarRangeStartOne = 0f, bool animateFarRangeEnd = false, AnimationCurve shakeFarRangeEnd = null, float remapFarRangeEndZero = 0f, float remapFarRangeEndOne = 0f)
		{
			MMDepthOfFieldShakeEvent_HDRP.Delegate onEvent = MMDepthOfFieldShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(duration, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore, animateFocusDistance, shakeFocusDistance, remapFocusDistanceZero, remapFocusDistanceOne, animateNearRangeStart, shakeNearRangeStart, remapNearRangeStartZero, remapNearRangeStartOne, animateNearRangeEnd, shakeNearRangeEnd, remapNearRangeEndZero, remapNearRangeEndOne, animateFarRangeStart, shakeFarRangeStart, remapFarRangeStartZero, remapFarRangeStartOne, animateFarRangeEnd, shakeFarRangeEnd, remapFarRangeEndZero, remapFarRangeEndOne);
		}

		// Token: 0x02000565 RID: 1381
		// (Invoke) Token: 0x06001D32 RID: 7474
		public delegate void Delegate(float duration, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false, bool animateFocusDistance = false, AnimationCurve shakeFocusDistance = null, float remapFocusDistanceZero = 0f, float remapFocusDistanceOne = 1f, bool animateNearRangeStart = false, AnimationCurve shakeNearRangeStart = null, float remapNearRangeStartZero = 0f, float remapNearRangeStartOne = 0f, bool animateNearRangeEnd = false, AnimationCurve shakeNearRangeEnd = null, float remapNearRangeEndZero = 0f, float remapNearRangeEndOne = 0f, bool animateFarRangeStart = false, AnimationCurve shakeFarRangeStart = null, float remapFarRangeStartZero = 0f, float remapFarRangeStartOne = 0f, bool animateFarRangeEnd = false, AnimationCurve shakeFarRangeEnd = null, float remapFarRangeEndZero = 0f, float remapFarRangeEndOne = 0f);
	}
}
