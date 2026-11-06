using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000BE RID: 190
	public struct MMPlaylistVolumeMultiplierEvent
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060003C1 RID: 961 RVA: 0x000125C0 File Offset: 0x000107C0
		// (remove) Token: 0x060003C2 RID: 962 RVA: 0x000125F4 File Offset: 0x000107F4
		private static event MMPlaylistVolumeMultiplierEvent.Delegate OnEvent;

		// Token: 0x060003C3 RID: 963 RVA: 0x00012627 File Offset: 0x00010827
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPlaylistVolumeMultiplierEvent.OnEvent = null;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0001262F File Offset: 0x0001082F
		public static void Register(MMPlaylistVolumeMultiplierEvent.Delegate callback)
		{
			MMPlaylistVolumeMultiplierEvent.OnEvent += callback;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00012637 File Offset: 0x00010837
		public static void Unregister(MMPlaylistVolumeMultiplierEvent.Delegate callback)
		{
			MMPlaylistVolumeMultiplierEvent.OnEvent -= callback;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001263F File Offset: 0x0001083F
		public static void Trigger(int channel, float newVolumeMultiplier, bool applyVolumeMultiplierInstantly = false)
		{
			MMPlaylistVolumeMultiplierEvent.Delegate onEvent = MMPlaylistVolumeMultiplierEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channel, newVolumeMultiplier, applyVolumeMultiplierInstantly);
		}

		// Token: 0x02000405 RID: 1029
		// (Invoke) Token: 0x06001977 RID: 6519
		public delegate void Delegate(int channel, float newVolumeMultiplier, bool applyVolumeMultiplierInstantly = false);
	}
}
