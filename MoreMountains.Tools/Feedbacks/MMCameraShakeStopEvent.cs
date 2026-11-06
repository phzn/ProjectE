using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000243 RID: 579
	public struct MMCameraShakeStopEvent
	{
		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06000FF7 RID: 4087 RVA: 0x00046FD4 File Offset: 0x000451D4
		// (remove) Token: 0x06000FF8 RID: 4088 RVA: 0x00047008 File Offset: 0x00045208
		private static event MMCameraShakeStopEvent.Delegate OnEvent;

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0004703B File Offset: 0x0004523B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMCameraShakeStopEvent.OnEvent = null;
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00047043 File Offset: 0x00045243
		public static void Register(MMCameraShakeStopEvent.Delegate callback)
		{
			MMCameraShakeStopEvent.OnEvent += callback;
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0004704B File Offset: 0x0004524B
		public static void Unregister(MMCameraShakeStopEvent.Delegate callback)
		{
			MMCameraShakeStopEvent.OnEvent -= callback;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00047053 File Offset: 0x00045253
		public static void Trigger(MMChannelData channelData)
		{
			MMCameraShakeStopEvent.Delegate onEvent = MMCameraShakeStopEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channelData);
		}

		// Token: 0x0200051A RID: 1306
		// (Invoke) Token: 0x06001C6F RID: 7279
		public delegate void Delegate(MMChannelData channelData);
	}
}
