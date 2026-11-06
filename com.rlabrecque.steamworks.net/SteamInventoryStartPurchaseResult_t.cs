using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000073 RID: 115
	[CallbackIdentity(4704)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryStartPurchaseResult_t
	{
		// Token: 0x04000129 RID: 297
		public const int k_iCallback = 4704;

		// Token: 0x0400012A RID: 298
		public EResult m_result;

		// Token: 0x0400012B RID: 299
		public ulong m_ulOrderID;

		// Token: 0x0400012C RID: 300
		public ulong m_ulTransID;
	}
}
