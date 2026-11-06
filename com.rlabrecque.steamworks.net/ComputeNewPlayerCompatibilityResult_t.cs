using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004D RID: 77
	[CallbackIdentity(211)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ComputeNewPlayerCompatibilityResult_t
	{
		// Token: 0x04000088 RID: 136
		public const int k_iCallback = 211;

		// Token: 0x04000089 RID: 137
		public EResult m_eResult;

		// Token: 0x0400008A RID: 138
		public int m_cPlayersThatDontLikeCandidate;

		// Token: 0x0400008B RID: 139
		public int m_cPlayersThatCandidateDoesntLike;

		// Token: 0x0400008C RID: 140
		public int m_cClanPlayersThatDontLikeCandidate;

		// Token: 0x0400008D RID: 141
		public CSteamID m_SteamIDCandidate;
	}
}
