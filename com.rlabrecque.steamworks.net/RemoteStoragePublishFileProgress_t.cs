using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A9 RID: 169
	[CallbackIdentity(1329)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishFileProgress_t
	{
		// Token: 0x04000204 RID: 516
		public const int k_iCallback = 1329;

		// Token: 0x04000205 RID: 517
		public double m_dPercentFile;

		// Token: 0x04000206 RID: 518
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bPreview;
	}
}
