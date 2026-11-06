using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200035A RID: 858
	public struct MMChannelMixerShakeEvent_URP
	{
		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06001791 RID: 6033 RVA: 0x00063F40 File Offset: 0x00062140
		// (remove) Token: 0x06001792 RID: 6034 RVA: 0x00063F74 File Offset: 0x00062174
		private static event MMChannelMixerShakeEvent_URP.Delegate OnEvent;

		// Token: 0x06001793 RID: 6035 RVA: 0x00063FA7 File Offset: 0x000621A7
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMChannelMixerShakeEvent_URP.OnEvent = null;
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x00063FAF File Offset: 0x000621AF
		public static void Register(MMChannelMixerShakeEvent_URP.Delegate callback)
		{
			MMChannelMixerShakeEvent_URP.OnEvent += callback;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x00063FB7 File Offset: 0x000621B7
		public static void Unregister(MMChannelMixerShakeEvent_URP.Delegate callback)
		{
			MMChannelMixerShakeEvent_URP.OnEvent -= callback;
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x00063FC0 File Offset: 0x000621C0
		public static void Trigger(AnimationCurve shakeRed, float remapRedZero, float remapRedOne, AnimationCurve shakeGreen, float remapGreenZero, float remapGreenOne, AnimationCurve shakeBlue, float remapBlueZero, float remapBlueOne, float duration, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMChannelMixerShakeEvent_URP.Delegate onEvent = MMChannelMixerShakeEvent_URP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(shakeRed, remapRedZero, remapRedOne, shakeGreen, remapGreenZero, remapGreenOne, shakeBlue, remapBlueZero, remapBlueOne, duration, relativeValues, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000589 RID: 1417
		// (Invoke) Token: 0x06001D8C RID: 7564
		public delegate void Delegate(AnimationCurve shakeRed, float remapRedZero, float remapRedOne, AnimationCurve shakeGreen, float remapGreenZero, float remapGreenOne, AnimationCurve shakeBlue, float remapBlueZero, float remapBlueOne, float duration, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
