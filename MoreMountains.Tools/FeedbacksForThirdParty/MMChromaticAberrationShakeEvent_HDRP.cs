using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000301 RID: 769
	public struct MMChromaticAberrationShakeEvent_HDRP
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x060015A7 RID: 5543 RVA: 0x0005C328 File Offset: 0x0005A528
		// (remove) Token: 0x060015A8 RID: 5544 RVA: 0x0005C35C File Offset: 0x0005A55C
		private static event MMChromaticAberrationShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x060015A9 RID: 5545 RVA: 0x0005C38F File Offset: 0x0005A58F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMChromaticAberrationShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0005C397 File Offset: 0x0005A597
		public static void Register(MMChromaticAberrationShakeEvent_HDRP.Delegate callback)
		{
			MMChromaticAberrationShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0005C39F File Offset: 0x0005A59F
		public static void Unregister(MMChromaticAberrationShakeEvent_HDRP.Delegate callback)
		{
			MMChromaticAberrationShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0005C3A8 File Offset: 0x0005A5A8
		public static void Trigger(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMChromaticAberrationShakeEvent_HDRP.Delegate onEvent = MMChromaticAberrationShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(intensity, duration, remapMin, remapMax, relativeIntensity, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000562 RID: 1378
		// (Invoke) Token: 0x06001D2A RID: 7466
		public delegate void Delegate(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
