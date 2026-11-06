using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000362 RID: 866
	public struct MMFilmGrainShakeEvent_URP
	{
		// Token: 0x14000049 RID: 73
		// (add) Token: 0x060017CF RID: 6095 RVA: 0x000653E0 File Offset: 0x000635E0
		// (remove) Token: 0x060017D0 RID: 6096 RVA: 0x00065414 File Offset: 0x00063614
		private static event MMFilmGrainShakeEvent_URP.Delegate OnEvent;

		// Token: 0x060017D1 RID: 6097 RVA: 0x00065447 File Offset: 0x00063647
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMFilmGrainShakeEvent_URP.OnEvent = null;
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x0006544F File Offset: 0x0006364F
		public static void Register(MMFilmGrainShakeEvent_URP.Delegate callback)
		{
			MMFilmGrainShakeEvent_URP.OnEvent += callback;
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x00065457 File Offset: 0x00063657
		public static void Unregister(MMFilmGrainShakeEvent_URP.Delegate callback)
		{
			MMFilmGrainShakeEvent_URP.OnEvent -= callback;
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00065460 File Offset: 0x00063660
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMFilmGrainShakeEvent_URP.Delegate onEvent = MMFilmGrainShakeEvent_URP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200058E RID: 1422
		// (Invoke) Token: 0x06001D9C RID: 7580
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
