using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200030F RID: 783
	public struct MMPaniniProjectionShakeEvent_HDRP
	{
		// Token: 0x1400003B RID: 59
		// (add) Token: 0x060015D8 RID: 5592 RVA: 0x0005CF14 File Offset: 0x0005B114
		// (remove) Token: 0x060015D9 RID: 5593 RVA: 0x0005CF48 File Offset: 0x0005B148
		private static event MMPaniniProjectionShakeEvent_HDRP.Delegate OnEvent;

		// Token: 0x060015DA RID: 5594 RVA: 0x0005CF7B File Offset: 0x0005B17B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPaniniProjectionShakeEvent_HDRP.OnEvent = null;
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x0005CF83 File Offset: 0x0005B183
		public static void Register(MMPaniniProjectionShakeEvent_HDRP.Delegate callback)
		{
			MMPaniniProjectionShakeEvent_HDRP.OnEvent += callback;
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x0005CF8B File Offset: 0x0005B18B
		public static void Unregister(MMPaniniProjectionShakeEvent_HDRP.Delegate callback)
		{
			MMPaniniProjectionShakeEvent_HDRP.OnEvent -= callback;
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x0005CF94 File Offset: 0x0005B194
		public static void Trigger(AnimationCurve distance, float duration, float remapMin, float remapMax, bool relativeDistance = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMPaniniProjectionShakeEvent_HDRP.Delegate onEvent = MMPaniniProjectionShakeEvent_HDRP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(distance, duration, remapMin, remapMax, relativeDistance, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x0200056A RID: 1386
		// (Invoke) Token: 0x06001D46 RID: 7494
		public delegate void Delegate(AnimationCurve distance, float duration, float remapMin, float remapMax, bool relativeDistance = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
