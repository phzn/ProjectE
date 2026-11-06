using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000BF RID: 191
	public struct MMPlaylistPitchMultiplierEvent
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060003C7 RID: 967 RVA: 0x00012654 File Offset: 0x00010854
		// (remove) Token: 0x060003C8 RID: 968 RVA: 0x00012688 File Offset: 0x00010888
		private static event MMPlaylistPitchMultiplierEvent.Delegate OnEvent;

		// Token: 0x060003C9 RID: 969 RVA: 0x000126BB File Offset: 0x000108BB
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPlaylistPitchMultiplierEvent.OnEvent = null;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000126C3 File Offset: 0x000108C3
		public static void Register(MMPlaylistPitchMultiplierEvent.Delegate callback)
		{
			MMPlaylistPitchMultiplierEvent.OnEvent += callback;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000126CB File Offset: 0x000108CB
		public static void Unregister(MMPlaylistPitchMultiplierEvent.Delegate callback)
		{
			MMPlaylistPitchMultiplierEvent.OnEvent -= callback;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000126D3 File Offset: 0x000108D3
		public static void Trigger(int channel, float newPitchMultiplier, bool applyPitchMultiplierInstantly = false)
		{
			MMPlaylistPitchMultiplierEvent.Delegate onEvent = MMPlaylistPitchMultiplierEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channel, newPitchMultiplier, applyPitchMultiplierInstantly);
		}

		// Token: 0x02000406 RID: 1030
		// (Invoke) Token: 0x0600197B RID: 6523
		public delegate void Delegate(int channel, float newPitchMultiplier, bool applyPitchMultiplierInstantly = false);
	}
}
