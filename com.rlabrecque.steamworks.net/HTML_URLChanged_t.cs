using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000055 RID: 85
	[CallbackIdentity(4505)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_URLChanged_t
	{
		// Token: 0x040000AD RID: 173
		public const int k_iCallback = 4505;

		// Token: 0x040000AE RID: 174
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000AF RID: 175
		public string pchURL;

		// Token: 0x040000B0 RID: 176
		public string pchPostData;

		// Token: 0x040000B1 RID: 177
		[MarshalAs(UnmanagedType.I1)]
		public bool bIsRedirect;

		// Token: 0x040000B2 RID: 178
		public string pchPageTitle;

		// Token: 0x040000B3 RID: 179
		[MarshalAs(UnmanagedType.I1)]
		public bool bNewNavigation;
	}
}
