using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000B9 RID: 185
	public struct MMPlaylistStopEvent
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060003A3 RID: 931 RVA: 0x000122DC File Offset: 0x000104DC
		// (remove) Token: 0x060003A4 RID: 932 RVA: 0x00012310 File Offset: 0x00010510
		private static event MMPlaylistStopEvent.Delegate OnEvent;

		// Token: 0x060003A5 RID: 933 RVA: 0x00012343 File Offset: 0x00010543
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPlaylistStopEvent.OnEvent = null;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001234B File Offset: 0x0001054B
		public static void Register(MMPlaylistStopEvent.Delegate callback)
		{
			MMPlaylistStopEvent.OnEvent += callback;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00012353 File Offset: 0x00010553
		public static void Unregister(MMPlaylistStopEvent.Delegate callback)
		{
			MMPlaylistStopEvent.OnEvent -= callback;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001235B File Offset: 0x0001055B
		public static void Trigger(int channel)
		{
			MMPlaylistStopEvent.Delegate onEvent = MMPlaylistStopEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channel);
		}

		// Token: 0x02000400 RID: 1024
		// (Invoke) Token: 0x06001963 RID: 6499
		public delegate void Delegate(int channel);
	}
}
