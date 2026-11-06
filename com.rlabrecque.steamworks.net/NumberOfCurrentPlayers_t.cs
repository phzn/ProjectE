using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DA RID: 218
	[CallbackIdentity(1107)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct NumberOfCurrentPlayers_t
	{
		// Token: 0x040002BE RID: 702
		public const int k_iCallback = 1107;

		// Token: 0x040002BF RID: 703
		public byte m_bSuccess;

		// Token: 0x040002C0 RID: 704
		public int m_cPlayers;
	}
}
