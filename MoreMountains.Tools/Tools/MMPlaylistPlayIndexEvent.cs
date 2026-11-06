using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000BD RID: 189
	public struct MMPlaylistPlayIndexEvent
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060003BB RID: 955 RVA: 0x0001252C File Offset: 0x0001072C
		// (remove) Token: 0x060003BC RID: 956 RVA: 0x00012560 File Offset: 0x00010760
		private static event MMPlaylistPlayIndexEvent.Delegate OnEvent;

		// Token: 0x060003BD RID: 957 RVA: 0x00012593 File Offset: 0x00010793
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPlaylistPlayIndexEvent.OnEvent = null;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001259B File Offset: 0x0001079B
		public static void Register(MMPlaylistPlayIndexEvent.Delegate callback)
		{
			MMPlaylistPlayIndexEvent.OnEvent += callback;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000125A3 File Offset: 0x000107A3
		public static void Unregister(MMPlaylistPlayIndexEvent.Delegate callback)
		{
			MMPlaylistPlayIndexEvent.OnEvent -= callback;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000125AB File Offset: 0x000107AB
		public static void Trigger(int channel, int index)
		{
			MMPlaylistPlayIndexEvent.Delegate onEvent = MMPlaylistPlayIndexEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channel, index);
		}

		// Token: 0x02000404 RID: 1028
		// (Invoke) Token: 0x06001973 RID: 6515
		public delegate void Delegate(int channel, int index);
	}
}
