using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000058 RID: 88
	[CallbackIdentity(4508)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_ChangedTitle_t
	{
		// Token: 0x040000BB RID: 187
		public const int k_iCallback = 4508;

		// Token: 0x040000BC RID: 188
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000BD RID: 189
		public string pchTitle;
	}
}
