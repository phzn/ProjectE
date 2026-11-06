using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200021A RID: 538
	public struct MMFloatingTextSpawnEvent
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000ED0 RID: 3792 RVA: 0x00041F74 File Offset: 0x00040174
		// (remove) Token: 0x06000ED1 RID: 3793 RVA: 0x00041FA8 File Offset: 0x000401A8
		private static event MMFloatingTextSpawnEvent.Delegate OnEvent;

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00041FDB File Offset: 0x000401DB
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMFloatingTextSpawnEvent.OnEvent = null;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00041FE3 File Offset: 0x000401E3
		public static void Register(MMFloatingTextSpawnEvent.Delegate callback)
		{
			MMFloatingTextSpawnEvent.OnEvent += callback;
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00041FEB File Offset: 0x000401EB
		public static void Unregister(MMFloatingTextSpawnEvent.Delegate callback)
		{
			MMFloatingTextSpawnEvent.OnEvent -= callback;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00041FF4 File Offset: 0x000401F4
		public static void Trigger(MMChannelData channelData, Vector3 spawnPosition, string value, Vector3 direction, float intensity, bool forceLifetime = false, float lifetime = 1f, bool forceColor = false, Gradient animateColorGradient = null, bool useUnscaledTime = false)
		{
			MMFloatingTextSpawnEvent.Delegate onEvent = MMFloatingTextSpawnEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channelData, spawnPosition, value, direction, intensity, forceLifetime, lifetime, forceColor, animateColorGradient, useUnscaledTime);
		}

		// Token: 0x02000506 RID: 1286
		// (Invoke) Token: 0x06001C31 RID: 7217
		public delegate void Delegate(MMChannelData channelData, Vector3 spawnPosition, string value, Vector3 direction, float intensity, bool forceLifetime = false, float lifetime = 1f, bool forceColor = false, Gradient animateColorGradient = null, bool useUnscaledTime = false);
	}
}
