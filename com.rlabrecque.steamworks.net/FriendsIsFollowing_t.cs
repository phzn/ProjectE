using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200003C RID: 60
	[CallbackIdentity(345)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsIsFollowing_t
	{
		// Token: 0x0400004A RID: 74
		public const int k_iCallback = 345;

		// Token: 0x0400004B RID: 75
		public EResult m_eResult;

		// Token: 0x0400004C RID: 76
		public CSteamID m_steamID;

		// Token: 0x0400004D RID: 77
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bIsFollowing;
	}
}
