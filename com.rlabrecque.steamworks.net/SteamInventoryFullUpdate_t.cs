using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000070 RID: 112
	[CallbackIdentity(4701)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryFullUpdate_t
	{
		// Token: 0x04000121 RID: 289
		public const int k_iCallback = 4701;

		// Token: 0x04000122 RID: 290
		public SteamInventoryResult_t m_handle;
	}
}
