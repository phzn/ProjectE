using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000168 RID: 360
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemotePlayInputKey_t
	{
		// Token: 0x04000AB3 RID: 2739
		public int m_eScancode;

		// Token: 0x04000AB4 RID: 2740
		public uint m_unModifiers;

		// Token: 0x04000AB5 RID: 2741
		public uint m_unKeycode;
	}
}
