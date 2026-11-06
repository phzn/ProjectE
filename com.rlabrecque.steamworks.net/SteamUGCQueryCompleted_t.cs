using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B2 RID: 178
	[CallbackIdentity(3401)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCQueryCompleted_t
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x0000C3AC File Offset: 0x0000A5AC
		// (set) Token: 0x06000879 RID: 2169 RVA: 0x0000C3B9 File Offset: 0x0000A5B9
		public string m_rgchNextCursor
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchNextCursor_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchNextCursor_, 256);
			}
		}

		// Token: 0x04000220 RID: 544
		public const int k_iCallback = 3401;

		// Token: 0x04000221 RID: 545
		public UGCQueryHandle_t m_handle;

		// Token: 0x04000222 RID: 546
		public EResult m_eResult;

		// Token: 0x04000223 RID: 547
		public uint m_unNumResultsReturned;

		// Token: 0x04000224 RID: 548
		public uint m_unTotalMatchingResults;

		// Token: 0x04000225 RID: 549
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;

		// Token: 0x04000226 RID: 550
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_rgchNextCursor_;
	}
}
