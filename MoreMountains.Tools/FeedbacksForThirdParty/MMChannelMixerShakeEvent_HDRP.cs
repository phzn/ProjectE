using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002FF RID: 767
	public struct MMChannelMixerShakeEvent_HDRP
	{
		// Token: 0x14000033 RID: 51
		// (add) Token: 0x060015A0 RID: 5536 RVA: 0x0005C1F4 File Offset: 0x0005A3F4
		// (remove) Token: 0x060015A1 RID: 5537 RVA: 0x0005C228 File Offset: 0x0005A428
		private static event MMChannelMixerShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x060015A2 RID: 5538 RVA: 0x0005C25B File Offset: 0x0005A45B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMChannelMixerShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0005C263 File Offset: 0x0005A463
		public static void Register(MMChannelMixerShakeEvent_HDRP.Delegate callback)
		{
			MMChannelMixerShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0005C26B File Offset: 0x0005A46B
		public static void Unregister(MMChannelMixerShakeEvent_HDRP.Delegate callback)
		{
			MMChannelMixerShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x0005C274 File Offset: 0x0005A474
		public static void Trigger(AnimationCurve shakeRed, float remapRedZero, float remapRedOne, AnimationCurve shakeGreen, float remapGreenZero, float remapGreenOne, AnimationCurve shakeBlue, float remapBlueZero, float remapBlueOne, float duration, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMChannelMixerShakeEvent_HDRP.Delegate onEvent = MMChannelMixerShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(shakeRed, remapRedZero, remapRedOne, shakeGreen, remapGreenZero, remapGreenOne, shakeBlue, remapBlueZero, remapBlueOne, duration, relativeValues, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000561 RID: 1377
		// (Invoke) Token: 0x06001D26 RID: 7462
		public delegate void Delegate(AnimationCurve shakeRed, float remapRedZero, float remapRedOne, AnimationCurve shakeGreen, float remapGreenZero, float remapGreenOne, AnimationCurve shakeBlue, float remapBlueZero, float remapBlueOne, float duration, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
