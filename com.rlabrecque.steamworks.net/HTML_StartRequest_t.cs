using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000053 RID: 83
	[CallbackIdentity(4503)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_StartRequest_t
	{
		// Token: 0x040000A5 RID: 165
		public const int k_iCallback = 4503;

		// Token: 0x040000A6 RID: 166
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000A7 RID: 167
		public string pchURL;

		// Token: 0x040000A8 RID: 168
		public string pchTarget;

		// Token: 0x040000A9 RID: 169
		public string pchPostData;

		// Token: 0x040000AA RID: 170
		[MarshalAs(UnmanagedType.I1)]
		public bool bIsRedirect;
	}
}
