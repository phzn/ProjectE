using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000364 RID: 868
	public struct MMLensDistortionShakeEvent_URP
	{
		// Token: 0x1400004A RID: 74
		// (add) Token: 0x060017DF RID: 6111 RVA: 0x000657C4 File Offset: 0x000639C4
		// (remove) Token: 0x060017E0 RID: 6112 RVA: 0x000657F8 File Offset: 0x000639F8
		private static event MMLensDistortionShakeEvent_URP.Delegate OnEvent;

		// Token: 0x060017E1 RID: 6113 RVA: 0x0006582B File Offset: 0x00063A2B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMLensDistortionShakeEvent_URP.OnEvent = null;
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00065833 File Offset: 0x00063A33
		public static void Register(MMLensDistortionShakeEvent_URP.Delegate callback)
		{
			MMLensDistortionShakeEvent_URP.OnEvent += callback;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0006583B File Offset: 0x00063A3B
		public static void Unregister(MMLensDistortionShakeEvent_URP.Delegate callback)
		{
			MMLensDistortionShakeEvent_URP.OnEvent -= callback;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00065844 File Offset: 0x00063A44
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMLensDistortionShakeEvent_URP.Delegate onEvent = MMLensDistortionShakeEvent_URP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200058F RID: 1423
		// (Invoke) Token: 0x06001DA0 RID: 7584
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
