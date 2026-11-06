using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005F RID: 95
	[CallbackIdentity(4515)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_JSConfirm_t
	{
		// Token: 0x040000DE RID: 222
		public const int k_iCallback = 4515;

		// Token: 0x040000DF RID: 223
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000E0 RID: 224
		public string pchMessage;
	}
}
