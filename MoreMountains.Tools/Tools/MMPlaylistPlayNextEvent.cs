using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000BB RID: 187
	public struct MMPlaylistPlayNextEvent
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060003AF RID: 943 RVA: 0x00012404 File Offset: 0x00010604
		// (remove) Token: 0x060003B0 RID: 944 RVA: 0x00012438 File Offset: 0x00010638
		private static event MMPlaylistPlayNextEvent.Delegate OnEvent;

		// Token: 0x060003B1 RID: 945 RVA: 0x0001246B File Offset: 0x0001066B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPlaylistPlayNextEvent.OnEvent = null;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00012473 File Offset: 0x00010673
		public static void Register(MMPlaylistPlayNextEvent.Delegate callback)
		{
			MMPlaylistPlayNextEvent.OnEvent += callback;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001247B File Offset: 0x0001067B
		public static void Unregister(MMPlaylistPlayNextEvent.Delegate callback)
		{
			MMPlaylistPlayNextEvent.OnEvent -= callback;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00012483 File Offset: 0x00010683
		public static void Trigger(int channel)
		{
			MMPlaylistPlayNextEvent.Delegate onEvent = MMPlaylistPlayNextEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channel);
		}

		// Token: 0x02000402 RID: 1026
		// (Invoke) Token: 0x0600196B RID: 6507
		public delegate void Delegate(int channel);
	}
}
