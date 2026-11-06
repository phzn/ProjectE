using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000069 RID: 105
	[CallbackIdentity(2102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestHeadersReceived_t
	{
		// Token: 0x04000104 RID: 260
		public const int k_iCallback = 2102;

		// Token: 0x04000105 RID: 261
		public HTTPRequestHandle m_hRequest;

		// Token: 0x04000106 RID: 262
		public ulong m_ulContextValue;
	}
}
