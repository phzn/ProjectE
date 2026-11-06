using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000BC RID: 188
	public struct MMPlaylistPlayPreviousEvent
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060003B5 RID: 949 RVA: 0x00012498 File Offset: 0x00010698
		// (remove) Token: 0x060003B6 RID: 950 RVA: 0x000124CC File Offset: 0x000106CC
		private static event MMPlaylistPlayPreviousEvent.Delegate OnEvent;

		// Token: 0x060003B7 RID: 951 RVA: 0x000124FF File Offset: 0x000106FF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPlaylistPlayPreviousEvent.OnEvent = null;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00012507 File Offset: 0x00010707
		public static void Register(MMPlaylistPlayPreviousEvent.Delegate callback)
		{
			MMPlaylistPlayPreviousEvent.OnEvent += callback;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001250F File Offset: 0x0001070F
		public static void Unregister(MMPlaylistPlayPreviousEvent.Delegate callback)
		{
			MMPlaylistPlayPreviousEvent.OnEvent -= callback;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00012517 File Offset: 0x00010717
		public static void Trigger(int channel)
		{
			MMPlaylistPlayPreviousEvent.Delegate onEvent = MMPlaylistPlayPreviousEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channel);
		}

		// Token: 0x02000403 RID: 1027
		// (Invoke) Token: 0x0600196F RID: 6511
		public delegate void Delegate(int channel);
	}
}
