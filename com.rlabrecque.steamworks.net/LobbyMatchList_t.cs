using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007C RID: 124
	[CallbackIdentity(510)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyMatchList_t
	{
		// Token: 0x04000154 RID: 340
		public const int k_iCallback = 510;

		// Token: 0x04000155 RID: 341
		public uint m_nLobbiesMatching;
	}
}
