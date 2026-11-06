using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000247 RID: 583
	public struct MMFeedbacksShakeEvent
	{
		// Token: 0x14000027 RID: 39
		// (add) Token: 0x0600100C RID: 4108 RVA: 0x0004746C File Offset: 0x0004566C
		// (remove) Token: 0x0600100D RID: 4109 RVA: 0x000474A0 File Offset: 0x000456A0
		private static event MMFeedbacksShakeEvent.Delegate OnEvent;

		// Token: 0x0600100E RID: 4110 RVA: 0x000474D3 File Offset: 0x000456D3
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMFeedbacksShakeEvent.OnEvent = null;
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x000474DB File Offset: 0x000456DB
		public static void Register(MMFeedbacksShakeEvent.Delegate callback)
		{
			MMFeedbacksShakeEvent.OnEvent += callback;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x000474E3 File Offset: 0x000456E3
		public static void Unregister(MMFeedbacksShakeEvent.Delegate callback)
		{
			MMFeedbacksShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x000474EB File Offset: 0x000456EB
		public static void Trigger(MMChannelData channelData = null, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3))
		{
			MMFeedbacksShakeEvent.Delegate onEvent = MMFeedbacksShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channelData, useRange, eventRange, eventOriginPosition);
		}

		// Token: 0x0200051B RID: 1307
		// (Invoke) Token: 0x06001C73 RID: 7283
		public delegate void Delegate(MMChannelData channelData = null, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3));
	}
}
