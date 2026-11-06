using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000060 RID: 96
	[CallbackIdentity(4516)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_FileOpenDialog_t
	{
		// Token: 0x040000E1 RID: 225
		public const int k_iCallback = 4516;

		// Token: 0x040000E2 RID: 226
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000E3 RID: 227
		public string pchTitle;

		// Token: 0x040000E4 RID: 228
		public string pchInitialFile;
	}
}
