using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000368 RID: 872
	public struct MMPaniniProjectionShakeEvent_URP
	{
		// Token: 0x1400004C RID: 76
		// (add) Token: 0x060017FD RID: 6141 RVA: 0x00065E3C File Offset: 0x0006403C
		// (remove) Token: 0x060017FE RID: 6142 RVA: 0x00065E70 File Offset: 0x00064070
		private static event MMPaniniProjectionShakeEvent_URP.Delegate OnEvent;

		// Token: 0x060017FF RID: 6143 RVA: 0x00065EA3 File Offset: 0x000640A3
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPaniniProjectionShakeEvent_URP.OnEvent = null;
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00065EAB File Offset: 0x000640AB
		public static void Register(MMPaniniProjectionShakeEvent_URP.Delegate callback)
		{
			MMPaniniProjectionShakeEvent_URP.OnEvent += callback;
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x00065EB3 File Offset: 0x000640B3
		public static void Unregister(MMPaniniProjectionShakeEvent_URP.Delegate callback)
		{
			MMPaniniProjectionShakeEvent_URP.OnEvent -= callback;
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x00065EBC File Offset: 0x000640BC
		public static void Trigger(AnimationCurve distance, float duration, float remapMin, float remapMax, bool relativeDistance = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMPaniniProjectionShakeEvent_URP.Delegate onEvent = MMPaniniProjectionShakeEvent_URP.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(distance, duration, remapMin, remapMax, relativeDistance, attenuation, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000591 RID: 1425
		// (Invoke) Token: 0x06001DA8 RID: 7592
		public delegate void Delegate(AnimationCurve distance, float duration, float remapMin, float remapMax, bool relativeDistance = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
