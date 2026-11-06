using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200035C RID: 860
	public struct MMChromaticAberrationShakeEvent_URP
	{
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x060017A0 RID: 6048 RVA: 0x00064288 File Offset: 0x00062488
		// (remove) Token: 0x060017A1 RID: 6049 RVA: 0x000642BC File Offset: 0x000624BC
		private static event MMChromaticAberrationShakeEvent_URP.Delegate OnEvent;

		// Token: 0x060017A2 RID: 6050 RVA: 0x000642EF File Offset: 0x000624EF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMChromaticAberrationShakeEvent_URP.OnEvent = null;
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x000642F7 File Offset: 0x000624F7
		public static void Register(MMChromaticAberrationShakeEvent_URP.Delegate callback)
		{
			MMChromaticAberrationShakeEvent_URP.OnEvent += callback;
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x000642FF File Offset: 0x000624FF
		public static void Unregister(MMChromaticAberrationShakeEvent_URP.Delegate callback)
		{
			MMChromaticAberrationShakeEvent_URP.OnEvent -= callback;
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x00064308 File Offset: 0x00062508
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMChromaticAberrationShakeEvent_URP.Delegate onEvent = MMChromaticAberrationShakeEvent_URP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200058A RID: 1418
		// (Invoke) Token: 0x06001D90 RID: 7568
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
