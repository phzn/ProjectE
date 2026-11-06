using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005C RID: 92
	[CallbackIdentity(4512)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_VerticalScroll_t
	{
		// Token: 0x040000CD RID: 205
		public const int k_iCallback = 4512;

		// Token: 0x040000CE RID: 206
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000CF RID: 207
		public uint unScrollMax;

		// Token: 0x040000D0 RID: 208
		public uint unScrollCurrent;

		// Token: 0x040000D1 RID: 209
		public float flPageScale;

		// Token: 0x040000D2 RID: 210
		[MarshalAs(UnmanagedType.I1)]
		public bool bVisible;

		// Token: 0x040000D3 RID: 211
		public uint unPageSize;
	}
}
