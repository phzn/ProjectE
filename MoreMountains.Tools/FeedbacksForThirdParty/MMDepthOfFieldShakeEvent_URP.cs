using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000360 RID: 864
	public struct MMDepthOfFieldShakeEvent_URP
	{
		// Token: 0x14000048 RID: 72
		// (add) Token: 0x060017C0 RID: 6080 RVA: 0x00065098 File Offset: 0x00063298
		// (remove) Token: 0x060017C1 RID: 6081 RVA: 0x000650CC File Offset: 0x000632CC
		private static event MMDepthOfFieldShakeEvent_URP.Delegate OnEvent;

		// Token: 0x060017C2 RID: 6082 RVA: 0x000650FF File Offset: 0x000632FF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMDepthOfFieldShakeEvent_URP.OnEvent = null;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x00065107 File Offset: 0x00063307
		public static void Register(MMDepthOfFieldShakeEvent_URP.Delegate callback)
		{
			MMDepthOfFieldShakeEvent_URP.OnEvent += callback;
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0006510F File Offset: 0x0006330F
		public static void Unregister(MMDepthOfFieldShakeEvent_URP.Delegate callback)
		{
			MMDepthOfFieldShakeEvent_URP.OnEvent -= callback;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x00065118 File Offset: 0x00063318
		public static void Trigger(AnimationCurve focusDistance, float duration, float remapFocusDistanceMin, float remapFocusDistanceMax, AnimationCurve aperture, float remapApertureMin, float remapApertureMax, AnimationCurve focalLength, float remapFocalLengthMin, float remapFocalLengthMax, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMDepthOfFieldShakeEvent_URP.Delegate onEvent = MMDepthOfFieldShakeEvent_URP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(focusDistance, duration, remapFocusDistanceMin, remapFocusDistanceMax, aperture, remapApertureMin, remapApertureMax, focalLength, remapFocalLengthMin, remapFocalLengthMax, relativeValues, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200058D RID: 1421
		// (Invoke) Token: 0x06001D98 RID: 7576
		public delegate void Delegate(AnimationCurve focusDistance, float duration, float remapFocusDistanceMin, float remapFocusDistanceMax, AnimationCurve aperture, float remapApertureMin, float remapApertureMax, AnimationCurve focalLength, float remapFocalLengthMin, float remapFocalLengthMax, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
