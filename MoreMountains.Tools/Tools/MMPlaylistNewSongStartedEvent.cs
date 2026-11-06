using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000C1 RID: 193
	public struct MMPlaylistNewSongStartedEvent
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060003D3 RID: 979 RVA: 0x0001277C File Offset: 0x0001097C
		// (remove) Token: 0x060003D4 RID: 980 RVA: 0x000127B0 File Offset: 0x000109B0
		private static event MMPlaylistNewSongStartedEvent.Delegate OnEvent;

		// Token: 0x060003D5 RID: 981 RVA: 0x000127E3 File Offset: 0x000109E3
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPlaylistNewSongStartedEvent.OnEvent = null;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000127EB File Offset: 0x000109EB
		public static void Register(MMPlaylistNewSongStartedEvent.Delegate callback)
		{
			MMPlaylistNewSongStartedEvent.OnEvent += callback;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000127F3 File Offset: 0x000109F3
		public static void Unregister(MMPlaylistNewSongStartedEvent.Delegate callback)
		{
			MMPlaylistNewSongStartedEvent.OnEvent -= callback;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000127FB File Offset: 0x000109FB
		public static void Trigger(int channel)
		{
			MMPlaylistNewSongStartedEvent.Delegate onEvent = MMPlaylistNewSongStartedEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channel);
		}

		// Token: 0x02000408 RID: 1032
		// (Invoke) Token: 0x06001983 RID: 6531
		public delegate void Delegate(int channel);
	}
}
