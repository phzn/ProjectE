using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000064 RID: 100
	[CallbackIdentity(4524)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_ShowToolTip_t
	{
		// Token: 0x040000F3 RID: 243
		public const int k_iCallback = 4524;

		// Token: 0x040000F4 RID: 244
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000F5 RID: 245
		public string pchMsg;
	}
}
