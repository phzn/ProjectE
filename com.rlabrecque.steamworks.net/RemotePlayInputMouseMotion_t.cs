using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000166 RID: 358
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemotePlayInputMouseMotion_t
	{
		// Token: 0x04000AAC RID: 2732
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAbsolute;

		// Token: 0x04000AAD RID: 2733
		public float m_flNormalizedX;

		// Token: 0x04000AAE RID: 2734
		public float m_flNormalizedY;

		// Token: 0x04000AAF RID: 2735
		public int m_nDeltaX;

		// Token: 0x04000AB0 RID: 2736
		public int m_nDeltaY;
	}
}
