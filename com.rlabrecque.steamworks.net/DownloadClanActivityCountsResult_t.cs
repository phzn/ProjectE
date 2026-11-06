using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000038 RID: 56
	[CallbackIdentity(341)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DownloadClanActivityCountsResult_t
	{
		// Token: 0x0400003E RID: 62
		public const int k_iCallback = 341;

		// Token: 0x0400003F RID: 63
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSuccess;
	}
}
