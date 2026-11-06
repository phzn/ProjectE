using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B8 RID: 184
	[CallbackIdentity(3407)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserFavoriteItemsListChanged_t
	{
		// Token: 0x0400023B RID: 571
		public const int k_iCallback = 3407;

		// Token: 0x0400023C RID: 572
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400023D RID: 573
		public EResult m_eResult;

		// Token: 0x0400023E RID: 574
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bWasAddRequest;
	}
}
