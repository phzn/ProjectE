using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000077 RID: 119
	[CallbackIdentity(504)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyEnter_t
	{
		// Token: 0x0400013C RID: 316
		public const int k_iCallback = 504;

		// Token: 0x0400013D RID: 317
		public ulong m_ulSteamIDLobby;

		// Token: 0x0400013E RID: 318
		public uint m_rgfChatPermissions;

		// Token: 0x0400013F RID: 319
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLocked;

		// Token: 0x04000140 RID: 320
		public uint m_EChatRoomEnterResponse;
	}
}
