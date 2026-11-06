using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000062 RID: 98
	[CallbackIdentity(4522)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_SetCursor_t
	{
		// Token: 0x040000ED RID: 237
		public const int k_iCallback = 4522;

		// Token: 0x040000EE RID: 238
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000EF RID: 239
		public uint eMouseCursor;
	}
}
