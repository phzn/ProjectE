using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000255 RID: 597
	public struct MMSpriteRendererShakeEvent
	{
		// Token: 0x1400002D RID: 45
		// (add) Token: 0x0600108D RID: 4237 RVA: 0x00049F28 File Offset: 0x00048128
		// (remove) Token: 0x0600108E RID: 4238 RVA: 0x00049F5C File Offset: 0x0004815C
		private static event MMSpriteRendererShakeEvent.Delegate OnEvent;

		// Token: 0x0600108F RID: 4239 RVA: 0x00049F8F File Offset: 0x0004818F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMSpriteRendererShakeEvent.OnEvent = null;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00049F97 File Offset: 0x00048197
		public static void Register(MMSpriteRendererShakeEvent.Delegate callback)
		{
			MMSpriteRendererShakeEvent.OnEvent += callback;
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00049F9F File Offset: 0x0004819F
		public static void Unregister(MMSpriteRendererShakeEvent.Delegate callback)
		{
			MMSpriteRendererShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00049FA8 File Offset: 0x000481A8
		public static void Trigger(float shakeDuration, bool modifyColor, Gradient colorOverTime, bool flipX, bool flipY, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3))
		{
			MMSpriteRendererShakeEvent.Delegate onEvent = MMSpriteRendererShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(shakeDuration, modifyColor, colorOverTime, flipX, flipY, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, useRange, eventRange, eventOriginPosition);
		}

		// Token: 0x02000525 RID: 1317
		// (Invoke) Token: 0x06001C91 RID: 7313
		public delegate void Delegate(float shakeDuration, bool modifyColor, Gradient colorOverTime, bool flipX, bool flipY, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3));
	}
}
