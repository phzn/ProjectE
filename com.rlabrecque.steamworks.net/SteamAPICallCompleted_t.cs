using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E2 RID: 226
	[CallbackIdentity(703)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamAPICallCompleted_t
	{
		// Token: 0x040002D4 RID: 724
		public const int k_iCallback = 703;

		// Token: 0x040002D5 RID: 725
		public SteamAPICall_t m_hAsyncCall;

		// Token: 0x040002D6 RID: 726
		public int m_iCallback;

		// Token: 0x040002D7 RID: 727
		public uint m_cubParam;
	}
}
