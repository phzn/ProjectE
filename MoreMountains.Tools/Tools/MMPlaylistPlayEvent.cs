using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000B8 RID: 184
	public struct MMPlaylistPlayEvent
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600039D RID: 925 RVA: 0x00012248 File Offset: 0x00010448
		// (remove) Token: 0x0600039E RID: 926 RVA: 0x0001227C File Offset: 0x0001047C
		private static event MMPlaylistPlayEvent.Delegate OnEvent;

		// Token: 0x0600039F RID: 927 RVA: 0x000122AF File Offset: 0x000104AF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPlaylistPlayEvent.OnEvent = null;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x000122B7 File Offset: 0x000104B7
		public static void Register(MMPlaylistPlayEvent.Delegate callback)
		{
			MMPlaylistPlayEvent.OnEvent += callback;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000122BF File Offset: 0x000104BF
		public static void Unregister(MMPlaylistPlayEvent.Delegate callback)
		{
			MMPlaylistPlayEvent.OnEvent -= callback;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x000122C7 File Offset: 0x000104C7
		public static void Trigger(int channel)
		{
			MMPlaylistPlayEvent.Delegate onEvent = MMPlaylistPlayEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channel);
		}

		// Token: 0x020003FF RID: 1023
		// (Invoke) Token: 0x0600195F RID: 6495
		public delegate void Delegate(int channel);
	}
}
