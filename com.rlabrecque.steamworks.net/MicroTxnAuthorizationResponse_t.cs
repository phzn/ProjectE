using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CC RID: 204
	[CallbackIdentity(152)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MicroTxnAuthorizationResponse_t
	{
		// Token: 0x04000282 RID: 642
		public const int k_iCallback = 152;

		// Token: 0x04000283 RID: 643
		public uint m_unAppID;

		// Token: 0x04000284 RID: 644
		public ulong m_ulOrderID;

		// Token: 0x04000285 RID: 645
		public byte m_bAuthorized;
	}
}
