using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001AE RID: 430
	public struct MMSetFeedbackRangeCenterEvent
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000A5B RID: 2651 RVA: 0x0002F42C File Offset: 0x0002D62C
		// (remove) Token: 0x06000A5C RID: 2652 RVA: 0x0002F460 File Offset: 0x0002D660
		private static event MMSetFeedbackRangeCenterEvent.Delegate OnEvent;

		// Token: 0x06000A5D RID: 2653 RVA: 0x0002F493 File Offset: 0x0002D693
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMSetFeedbackRangeCenterEvent.OnEvent = null;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0002F49B File Offset: 0x0002D69B
		public static void Register(MMSetFeedbackRangeCenterEvent.Delegate callback)
		{
			MMSetFeedbackRangeCenterEvent.OnEvent += callback;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0002F4A3 File Offset: 0x0002D6A3
		public static void Unregister(MMSetFeedbackRangeCenterEvent.Delegate callback)
		{
			MMSetFeedbackRangeCenterEvent.OnEvent -= callback;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0002F4AB File Offset: 0x0002D6AB
		public static void Trigger(Transform newCenter)
		{
			MMSetFeedbackRangeCenterEvent.Delegate onEvent = MMSetFeedbackRangeCenterEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(newCenter);
		}

		// Token: 0x0200048D RID: 1165
		// (Invoke) Token: 0x06001B0F RID: 6927
		public delegate void Delegate(Transform newCenter);
	}
}
