using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005A RID: 90
	[CallbackIdentity(4510)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_CanGoBackAndForward_t
	{
		// Token: 0x040000C2 RID: 194
		public const int k_iCallback = 4510;

		// Token: 0x040000C3 RID: 195
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000C4 RID: 196
		[MarshalAs(UnmanagedType.I1)]
		public bool bCanGoBack;

		// Token: 0x040000C5 RID: 197
		[MarshalAs(UnmanagedType.I1)]
		public bool bCanGoForward;
	}
}
