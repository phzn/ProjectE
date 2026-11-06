using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000037 RID: 55
	[CallbackIdentity(340)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct GameConnectedChatLeave_t
	{
		// Token: 0x04000039 RID: 57
		public const int k_iCallback = 340;

		// Token: 0x0400003A RID: 58
		public CSteamID m_steamIDClanChat;

		// Token: 0x0400003B RID: 59
		public CSteamID m_steamIDUser;

		// Token: 0x0400003C RID: 60
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bKicked;

		// Token: 0x0400003D RID: 61
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bDropped;
	}
}
