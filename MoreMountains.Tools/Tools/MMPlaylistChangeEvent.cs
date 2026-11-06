using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000C0 RID: 192
	public struct MMPlaylistChangeEvent
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060003CD RID: 973 RVA: 0x000126E8 File Offset: 0x000108E8
		// (remove) Token: 0x060003CE RID: 974 RVA: 0x0001271C File Offset: 0x0001091C
		private static event MMPlaylistChangeEvent.Delegate OnEvent;

		// Token: 0x060003CF RID: 975 RVA: 0x0001274F File Offset: 0x0001094F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPlaylistChangeEvent.OnEvent = null;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00012757 File Offset: 0x00010957
		public static void Register(MMPlaylistChangeEvent.Delegate callback)
		{
			MMPlaylistChangeEvent.OnEvent += callback;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001275F File Offset: 0x0001095F
		public static void Unregister(MMPlaylistChangeEvent.Delegate callback)
		{
			MMPlaylistChangeEvent.OnEvent -= callback;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00012767 File Offset: 0x00010967
		public static void Trigger(int channel, MMSMPlaylist newPlaylist, bool andPlay)
		{
			MMPlaylistChangeEvent.Delegate onEvent = MMPlaylistChangeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channel, newPlaylist, andPlay);
		}

		// Token: 0x02000407 RID: 1031
		// (Invoke) Token: 0x0600197F RID: 6527
		public delegate void Delegate(int channel, MMSMPlaylist newPlaylist, bool andPlay);
	}
}
