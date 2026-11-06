using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000044 RID: 68
	[CallbackIdentity(201)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSClientApprove_t
	{
		// Token: 0x04000064 RID: 100
		public const int k_iCallback = 201;

		// Token: 0x04000065 RID: 101
		public CSteamID m_SteamID;

		// Token: 0x04000066 RID: 102
		public CSteamID m_OwnerSteamID;
	}
}
