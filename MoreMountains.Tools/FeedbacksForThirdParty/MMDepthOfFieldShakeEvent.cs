using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200032C RID: 812
	public struct MMDepthOfFieldShakeEvent
	{
		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06001664 RID: 5732 RVA: 0x0005EFAC File Offset: 0x0005D1AC
		// (remove) Token: 0x06001665 RID: 5733 RVA: 0x0005EFE0 File Offset: 0x0005D1E0
		private static event MMDepthOfFieldShakeEvent.Delegate OnEvent;

		// Token: 0x06001666 RID: 5734 RVA: 0x0005F013 File Offset: 0x0005D213
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMDepthOfFieldShakeEvent.OnEvent = null;
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x0005F01B File Offset: 0x0005D21B
		public static void Register(MMDepthOfFieldShakeEvent.Delegate callback)
		{
			MMDepthOfFieldShakeEvent.OnEvent += callback;
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0005F023 File Offset: 0x0005D223
		public static void Unregister(MMDepthOfFieldShakeEvent.Delegate callback)
		{
			MMDepthOfFieldShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x0005F02C File Offset: 0x0005D22C
		public static void Trigger(AnimationCurve focusDistance, float duration, float remapFocusDistanceMin, float remapFocusDistanceMax, AnimationCurve aperture, float remapApertureMin, float remapApertureMax, AnimationCurve focalLength, float remapFocalLengthMin, float remapFocalLengthMax, bool relativeValues = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMDepthOfFieldShakeEvent.Delegate onEvent = MMDepthOfFieldShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(focusDistance, duration, remapFocusDistanceMin, remapFocusDistanceMax, aperture, remapApertureMin, remapApertureMax, focalLength, remapFocalLengthMin, remapFocalLengthMax, relativeValues, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000576 RID: 1398
		// (Invoke) Token: 0x06001D64 RID: 7524
		public delegate void Delegate(AnimationCurve focusDistance, float duration, float remapFocusDistanceMin, float remapFocusDistanceMax, AnimationCurve aperture, float remapApertureMin, float remapApertureMax, AnimationCurve focalLength, float remapFocalLengthMin, float remapFocalLengthMax, bool relativeValues = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
