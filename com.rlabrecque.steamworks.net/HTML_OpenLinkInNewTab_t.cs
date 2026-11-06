using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000057 RID: 87
	[CallbackIdentity(4507)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_OpenLinkInNewTab_t
	{
		// Token: 0x040000B8 RID: 184
		public const int k_iCallback = 4507;

		// Token: 0x040000B9 RID: 185
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000BA RID: 186
		public string pchURL;
	}
}
