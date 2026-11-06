using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000040 RID: 64
	[CallbackIdentity(350)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct EquippedProfileItemsChanged_t
	{
		// Token: 0x04000056 RID: 86
		public const int k_iCallback = 350;

		// Token: 0x04000057 RID: 87
		public CSteamID m_steamID;
	}
}
