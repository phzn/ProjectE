using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AE RID: 174
	[CallbackIdentity(2301)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ScreenshotReady_t
	{
		// Token: 0x04000213 RID: 531
		public const int k_iCallback = 2301;

		// Token: 0x04000214 RID: 532
		public ScreenshotHandle m_hLocal;

		// Token: 0x04000215 RID: 533
		public EResult m_eResult;
	}
}
