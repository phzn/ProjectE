using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000089 RID: 137
	[CallbackIdentity(1203)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct P2PSessionConnectFail_t
	{
		// Token: 0x04000173 RID: 371
		public const int k_iCallback = 1203;

		// Token: 0x04000174 RID: 372
		public CSteamID m_steamIDRemote;

		// Token: 0x04000175 RID: 373
		public byte m_eP2PSessionError;
	}
}
