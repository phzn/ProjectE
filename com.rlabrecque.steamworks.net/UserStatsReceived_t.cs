using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D4 RID: 212
	[CallbackIdentity(1101)]
	[StructLayout(LayoutKind.Explicit, Pack = 8)]
	public struct UserStatsReceived_t
	{
		// Token: 0x040002A3 RID: 675
		public const int k_iCallback = 1101;

		// Token: 0x040002A4 RID: 676
		[FieldOffset(0)]
		public ulong m_nGameID;

		// Token: 0x040002A5 RID: 677
		[FieldOffset(8)]
		public EResult m_eResult;

		// Token: 0x040002A6 RID: 678
		[FieldOffset(12)]
		public CSteamID m_steamIDUser;
	}
}
