using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200030B RID: 779
	public struct MMLensDistortionShakeEvent_HDRP
	{
		// Token: 0x14000039 RID: 57
		// (add) Token: 0x060015CA RID: 5578 RVA: 0x0005CCC4 File Offset: 0x0005AEC4
		// (remove) Token: 0x060015CB RID: 5579 RVA: 0x0005CCF8 File Offset: 0x0005AEF8
		private static event MMLensDistortionShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x060015CC RID: 5580 RVA: 0x0005CD2B File Offset: 0x0005AF2B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMLensDistortionShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x0005CD33 File Offset: 0x0005AF33
		public static void Register(MMLensDistortionShakeEvent_HDRP.Delegate callback)
		{
			MMLensDistortionShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x0005CD3B File Offset: 0x0005AF3B
		public static void Unregister(MMLensDistortionShakeEvent_HDRP.Delegate callback)
		{
			MMLensDistortionShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x0005CD44 File Offset: 0x0005AF44
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMLensDistortionShakeEvent_HDRP.Delegate onEvent = MMLensDistortionShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000568 RID: 1384
		// (Invoke) Token: 0x06001D3E RID: 7486
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
