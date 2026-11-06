using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000039 RID: 57
	[CallbackIdentity(342)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct JoinClanChatRoomCompletionResult_t
	{
		// Token: 0x04000040 RID: 64
		public const int k_iCallback = 342;

		// Token: 0x04000041 RID: 65
		public CSteamID m_steamIDClanChat;

		// Token: 0x04000042 RID: 66
		public EChatRoomEnterResponse m_eChatRoomEnterResponse;
	}
}
