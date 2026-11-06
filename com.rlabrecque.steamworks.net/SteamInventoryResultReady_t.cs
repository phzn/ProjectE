using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006F RID: 111
	[CallbackIdentity(4700)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryResultReady_t
	{
		// Token: 0x0400011E RID: 286
		public const int k_iCallback = 4700;

		// Token: 0x0400011F RID: 287
		public SteamInventoryResult_t m_handle;

		// Token: 0x04000120 RID: 288
		public EResult m_result;
	}
}
