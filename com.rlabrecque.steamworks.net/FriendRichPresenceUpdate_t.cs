using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000033 RID: 51
	[CallbackIdentity(336)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendRichPresenceUpdate_t
	{
		// Token: 0x0400002C RID: 44
		public const int k_iCallback = 336;

		// Token: 0x0400002D RID: 45
		public CSteamID m_steamIDFriend;

		// Token: 0x0400002E RID: 46
		public AppId_t m_nAppID;
	}
}
