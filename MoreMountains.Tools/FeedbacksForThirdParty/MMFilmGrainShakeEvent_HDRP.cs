using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000309 RID: 777
	public struct MMFilmGrainShakeEvent_HDRP
	{
		// Token: 0x14000038 RID: 56
		// (add) Token: 0x060015C3 RID: 5571 RVA: 0x0005CB00 File Offset: 0x0005AD00
		// (remove) Token: 0x060015C4 RID: 5572 RVA: 0x0005CB34 File Offset: 0x0005AD34
		private static event MMFilmGrainShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x060015C5 RID: 5573 RVA: 0x0005CB67 File Offset: 0x0005AD67
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMFilmGrainShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x0005CB6F File Offset: 0x0005AD6F
		public static void Register(MMFilmGrainShakeEvent_HDRP.Delegate callback)
		{
			MMFilmGrainShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0005CB77 File Offset: 0x0005AD77
		public static void Unregister(MMFilmGrainShakeEvent_HDRP.Delegate callback)
		{
			MMFilmGrainShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0005CB80 File Offset: 0x0005AD80
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMFilmGrainShakeEvent_HDRP.Delegate onEvent = MMFilmGrainShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000567 RID: 1383
		// (Invoke) Token: 0x06001D3A RID: 7482
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
