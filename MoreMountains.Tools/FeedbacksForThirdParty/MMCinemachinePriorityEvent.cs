using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002ED RID: 749
	public struct MMCinemachinePriorityEvent
	{
		// Token: 0x14000031 RID: 49
		// (add) Token: 0x0600150D RID: 5389 RVA: 0x00059D30 File Offset: 0x00057F30
		// (remove) Token: 0x0600150E RID: 5390 RVA: 0x00059D64 File Offset: 0x00057F64
		private static event MMCinemachinePriorityEvent.Delegate OnEvent;

		// Token: 0x0600150F RID: 5391 RVA: 0x00059D97 File Offset: 0x00057F97
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMCinemachinePriorityEvent.OnEvent = null;
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00059D9F File Offset: 0x00057F9F
		public static void Register(MMCinemachinePriorityEvent.Delegate callback)
		{
			MMCinemachinePriorityEvent.OnEvent += callback;
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x00059DA7 File Offset: 0x00057FA7
		public static void Unregister(MMCinemachinePriorityEvent.Delegate callback)
		{
			MMCinemachinePriorityEvent.OnEvent -= callback;
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x00059DB0 File Offset: 0x00057FB0
		public static void Trigger(MMChannelData channelData, bool forceMaxPriority, int newPriority, bool forceTransition, CinemachineBlendDefinition blendDefinition, bool resetValuesAfterTransition, TimescaleModes timescaleMode, bool restore = false)
		{
			MMCinemachinePriorityEvent.Delegate onEvent = MMCinemachinePriorityEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channelData, forceMaxPriority, newPriority, forceTransition, blendDefinition, resetValuesAfterTransition, timescaleMode, restore);
		}

		// Token: 0x0200055F RID: 1375
		// (Invoke) Token: 0x06001D1E RID: 7454
		public delegate void Delegate(MMChannelData channelData, bool forceMaxPriority, int newPriority, bool forceTransition, CinemachineBlendDefinition blendDefinition, bool resetValuesAfterTransition, TimescaleModes timescaleMode, bool restore = false);
	}
}
