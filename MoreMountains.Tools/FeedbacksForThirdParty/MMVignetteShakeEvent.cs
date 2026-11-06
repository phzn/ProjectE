using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000330 RID: 816
	public struct MMVignetteShakeEvent
	{
		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06001672 RID: 5746 RVA: 0x0005F328 File Offset: 0x0005D528
		// (remove) Token: 0x06001673 RID: 5747 RVA: 0x0005F35C File Offset: 0x0005D55C
		private static event MMVignetteShakeEvent.Delegate OnEvent;

		// Token: 0x06001674 RID: 5748 RVA: 0x0005F38F File Offset: 0x0005D58F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMVignetteShakeEvent.OnEvent = null;
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x0005F397 File Offset: 0x0005D597
		public static void Register(MMVignetteShakeEvent.Delegate callback)
		{
			MMVignetteShakeEvent.OnEvent += callback;
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x0005F39F File Offset: 0x0005D59F
		public static void Unregister(MMVignetteShakeEvent.Delegate callback)
		{
			MMVignetteShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x0005F3A8 File Offset: 0x0005D5A8
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false, bool interpolateColor = false, AnimationCurve colorCurve = null, float remapColorZero = 0f, float remapColorOne = 1f, Color targetColor = default(Color))
		{
			MMVignetteShakeEvent.Delegate onEvent = MMVignetteShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore, interpolateColor, colorCurve, remapColorZero, remapColorOne, targetColor);
		}

		// Token: 0x02000578 RID: 1400
		// (Invoke) Token: 0x06001D6C RID: 7532
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false, bool interpolateColor = false, AnimationCurve colorCurve = null, float remapColorZero = 0f, float remapColorOne = 1f, Color targetColor = default(Color));
	}
}
