using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000094 RID: 148
	[CallbackIdentity(5704)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamRemotePlaySessionAvatarLoaded_t
	{
		// Token: 0x04000193 RID: 403
		public const int k_iCallback = 5704;

		// Token: 0x04000194 RID: 404
		public RemotePlaySessionID_t m_unSessionID;

		// Token: 0x04000195 RID: 405
		public int m_iImage;

		// Token: 0x04000196 RID: 406
		public int m_iWide;

		// Token: 0x04000197 RID: 407
		public int m_iTall;
	}
}
