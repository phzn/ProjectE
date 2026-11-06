using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000042 RID: 66
	[CallbackIdentity(1701)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GCMessageAvailable_t
	{
		// Token: 0x04000061 RID: 97
		public const int k_iCallback = 1701;

		// Token: 0x04000062 RID: 98
		public uint m_nMessageSize;
	}
}
