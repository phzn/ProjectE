using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000066 RID: 102
	[CallbackIdentity(4526)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_HideToolTip_t
	{
		// Token: 0x040000F9 RID: 249
		public const int k_iCallback = 4526;

		// Token: 0x040000FA RID: 250
		public HHTMLBrowser unBrowserHandle;
	}
}
