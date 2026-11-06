using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AC RID: 172
	[CallbackIdentity(1332)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileReadAsyncComplete_t
	{
		// Token: 0x0400020D RID: 525
		public const int k_iCallback = 1332;

		// Token: 0x0400020E RID: 526
		public SteamAPICall_t m_hFileReadAsync;

		// Token: 0x0400020F RID: 527
		public EResult m_eResult;

		// Token: 0x04000210 RID: 528
		public uint m_nOffset;

		// Token: 0x04000211 RID: 529
		public uint m_cubRead;
	}
}
