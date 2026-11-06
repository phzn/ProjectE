using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200003D RID: 61
	[CallbackIdentity(346)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsEnumerateFollowingList_t
	{
		// Token: 0x0400004E RID: 78
		public const int k_iCallback = 346;

		// Token: 0x0400004F RID: 79
		public EResult m_eResult;

		// Token: 0x04000050 RID: 80
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public CSteamID[] m_rgSteamID;

		// Token: 0x04000051 RID: 81
		public int m_nResultsReturned;

		// Token: 0x04000052 RID: 82
		public int m_nTotalResultCount;
	}
}
