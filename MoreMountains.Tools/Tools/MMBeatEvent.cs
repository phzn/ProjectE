using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000B2 RID: 178
	public struct MMBeatEvent
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600037D RID: 893 RVA: 0x000118B8 File Offset: 0x0000FAB8
		// (remove) Token: 0x0600037E RID: 894 RVA: 0x000118EC File Offset: 0x0000FAEC
		private static event MMBeatEvent.Delegate OnEvent;

		// Token: 0x0600037F RID: 895 RVA: 0x0001191F File Offset: 0x0000FB1F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMBeatEvent.OnEvent = null;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00011927 File Offset: 0x0000FB27
		public static void Register(MMBeatEvent.Delegate callback)
		{
			MMBeatEvent.OnEvent += callback;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0001192F File Offset: 0x0000FB2F
		public static void Unregister(MMBeatEvent.Delegate callback)
		{
			MMBeatEvent.OnEvent -= callback;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00011937 File Offset: 0x0000FB37
		public static void Trigger(string name, float value)
		{
			MMBeatEvent.Delegate onEvent = MMBeatEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(name, value);
		}

		// Token: 0x020003F8 RID: 1016
		// (Invoke) Token: 0x0600194B RID: 6475
		public delegate void Delegate(string name, float value);
	}
}
