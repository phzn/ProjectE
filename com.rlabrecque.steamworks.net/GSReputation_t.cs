using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004B RID: 75
	[CallbackIdentity(209)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSReputation_t
	{
		// Token: 0x0400007E RID: 126
		public const int k_iCallback = 209;

		// Token: 0x0400007F RID: 127
		public EResult m_eResult;

		// Token: 0x04000080 RID: 128
		public uint m_unReputationScore;

		// Token: 0x04000081 RID: 129
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x04000082 RID: 130
		public uint m_unBannedIP;

		// Token: 0x04000083 RID: 131
		public ushort m_usBannedPort;

		// Token: 0x04000084 RID: 132
		public ulong m_ulBannedGameID;

		// Token: 0x04000085 RID: 133
		public uint m_unBanExpires;
	}
}
