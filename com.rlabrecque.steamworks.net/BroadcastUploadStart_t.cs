using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EB RID: 235
	[CallbackIdentity(4604)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct BroadcastUploadStart_t
	{
		// Token: 0x040002EA RID: 746
		public const int k_iCallback = 4604;

		// Token: 0x040002EB RID: 747
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bIsRTMP;
	}
}
