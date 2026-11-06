using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000BA RID: 186
	public struct MMPlaylistPauseEvent
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060003A9 RID: 937 RVA: 0x00012370 File Offset: 0x00010570
		// (remove) Token: 0x060003AA RID: 938 RVA: 0x000123A4 File Offset: 0x000105A4
		private static event MMPlaylistPauseEvent.Delegate OnEvent;

		// Token: 0x060003AB RID: 939 RVA: 0x000123D7 File Offset: 0x000105D7
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPlaylistPauseEvent.OnEvent = null;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000123DF File Offset: 0x000105DF
		public static void Register(MMPlaylistPauseEvent.Delegate callback)
		{
			MMPlaylistPauseEvent.OnEvent += callback;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000123E7 File Offset: 0x000105E7
		public static void Unregister(MMPlaylistPauseEvent.Delegate callback)
		{
			MMPlaylistPauseEvent.OnEvent -= callback;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x000123EF File Offset: 0x000105EF
		public static void Trigger(int channel)
		{
			MMPlaylistPauseEvent.Delegate onEvent = MMPlaylistPauseEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channel);
		}

		// Token: 0x02000401 RID: 1025
		// (Invoke) Token: 0x06001967 RID: 6503
		public delegate void Delegate(int channel);
	}
}
