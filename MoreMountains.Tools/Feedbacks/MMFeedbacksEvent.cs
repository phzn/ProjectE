using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001AD RID: 429
	public struct MMFeedbacksEvent
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000A55 RID: 2645 RVA: 0x0002F398 File Offset: 0x0002D598
		// (remove) Token: 0x06000A56 RID: 2646 RVA: 0x0002F3CC File Offset: 0x0002D5CC
		private static event MMFeedbacksEvent.Delegate OnEvent;

		// Token: 0x06000A57 RID: 2647 RVA: 0x0002F3FF File Offset: 0x0002D5FF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMFeedbacksEvent.OnEvent = null;
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0002F407 File Offset: 0x0002D607
		public static void Register(MMFeedbacksEvent.Delegate callback)
		{
			MMFeedbacksEvent.OnEvent += callback;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0002F40F File Offset: 0x0002D60F
		public static void Unregister(MMFeedbacksEvent.Delegate callback)
		{
			MMFeedbacksEvent.OnEvent -= callback;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0002F417 File Offset: 0x0002D617
		public static void Trigger(MMFeedbacks source, MMFeedbacksEvent.EventTypes type)
		{
			MMFeedbacksEvent.Delegate onEvent = MMFeedbacksEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(source, type);
		}

		// Token: 0x0200048B RID: 1163
		public enum EventTypes
		{
			// Token: 0x04002134 RID: 8500
			Play,
			// Token: 0x04002135 RID: 8501
			Pause,
			// Token: 0x04002136 RID: 8502
			Resume,
			// Token: 0x04002137 RID: 8503
			Revert,
			// Token: 0x04002138 RID: 8504
			Complete,
			// Token: 0x04002139 RID: 8505
			SkipToTheEnd,
			// Token: 0x0400213A RID: 8506
			RestoreInitialValues,
			// Token: 0x0400213B RID: 8507
			Loop,
			// Token: 0x0400213C RID: 8508
			Enable,
			// Token: 0x0400213D RID: 8509
			Disable,
			// Token: 0x0400213E RID: 8510
			InitializationComplete
		}

		// Token: 0x0200048C RID: 1164
		// (Invoke) Token: 0x06001B0B RID: 6923
		public delegate void Delegate(MMFeedbacks source, MMFeedbacksEvent.EventTypes type);
	}
}
