using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000248 RID: 584
	public struct MMFlashEvent
	{
		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06001012 RID: 4114 RVA: 0x00047500 File Offset: 0x00045700
		// (remove) Token: 0x06001013 RID: 4115 RVA: 0x00047534 File Offset: 0x00045734
		private static event MMFlashEvent.Delegate OnEvent;

		// Token: 0x06001014 RID: 4116 RVA: 0x00047567 File Offset: 0x00045767
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMFlashEvent.OnEvent = null;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0004756F File Offset: 0x0004576F
		public static void Register(MMFlashEvent.Delegate callback)
		{
			MMFlashEvent.OnEvent += callback;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00047577 File Offset: 0x00045777
		public static void Unregister(MMFlashEvent.Delegate callback)
		{
			MMFlashEvent.OnEvent -= callback;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0004757F File Offset: 0x0004577F
		public static void Trigger(Color flashColor, float duration, float alpha, int flashID, MMChannelData channelData, TimescaleModes timescaleMode, bool stop = false)
		{
			MMFlashEvent.Delegate onEvent = MMFlashEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(flashColor, duration, alpha, flashID, channelData, timescaleMode, stop);
		}

		// Token: 0x0200051C RID: 1308
		// (Invoke) Token: 0x06001C77 RID: 7287
		public delegate void Delegate(Color flashColor, float duration, float alpha, int flashID, MMChannelData channelData, TimescaleModes timescaleMode, bool stop = false);
	}
}
