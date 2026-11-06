using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002DC RID: 732
	public struct MMFreezeFrameEvent
	{
		// Token: 0x14000030 RID: 48
		// (add) Token: 0x0600147B RID: 5243 RVA: 0x00057C48 File Offset: 0x00055E48
		// (remove) Token: 0x0600147C RID: 5244 RVA: 0x00057C7C File Offset: 0x00055E7C
		private static event MMFreezeFrameEvent.Delegate OnEvent;

		// Token: 0x0600147D RID: 5245 RVA: 0x00057CAF File Offset: 0x00055EAF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMFreezeFrameEvent.OnEvent = null;
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x00057CB7 File Offset: 0x00055EB7
		public static void Register(MMFreezeFrameEvent.Delegate callback)
		{
			MMFreezeFrameEvent.OnEvent += callback;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00057CBF File Offset: 0x00055EBF
		public static void Unregister(MMFreezeFrameEvent.Delegate callback)
		{
			MMFreezeFrameEvent.OnEvent -= callback;
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00057CC7 File Offset: 0x00055EC7
		public static void Trigger(float duration)
		{
			MMFreezeFrameEvent.Delegate onEvent = MMFreezeFrameEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(duration);
		}

		// Token: 0x0200055A RID: 1370
		// (Invoke) Token: 0x06001D0C RID: 7436
		public delegate void Delegate(float duration);
	}
}
