using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000063 RID: 99
	[CallbackIdentity(4523)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_StatusText_t
	{
		// Token: 0x040000F0 RID: 240
		public const int k_iCallback = 4523;

		// Token: 0x040000F1 RID: 241
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000F2 RID: 242
		public string pchMsg;
	}
}
