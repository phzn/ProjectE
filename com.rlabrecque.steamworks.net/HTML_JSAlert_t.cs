using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005E RID: 94
	[CallbackIdentity(4514)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_JSAlert_t
	{
		// Token: 0x040000DB RID: 219
		public const int k_iCallback = 4514;

		// Token: 0x040000DC RID: 220
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000DD RID: 221
		public string pchMessage;
	}
}
