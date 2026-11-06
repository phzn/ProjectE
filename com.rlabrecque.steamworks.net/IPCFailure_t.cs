using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C9 RID: 201
	[CallbackIdentity(117)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct IPCFailure_t
	{
		// Token: 0x0400027B RID: 635
		public const int k_iCallback = 117;

		// Token: 0x0400027C RID: 636
		public byte m_eFailureType;
	}
}
