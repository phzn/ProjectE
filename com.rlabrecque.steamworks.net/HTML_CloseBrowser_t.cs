using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000054 RID: 84
	[CallbackIdentity(4504)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_CloseBrowser_t
	{
		// Token: 0x040000AB RID: 171
		public const int k_iCallback = 4504;

		// Token: 0x040000AC RID: 172
		public HHTMLBrowser unBrowserHandle;
	}
}
