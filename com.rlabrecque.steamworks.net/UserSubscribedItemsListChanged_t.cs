using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C3 RID: 195
	[CallbackIdentity(3418)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserSubscribedItemsListChanged_t
	{
		// Token: 0x04000266 RID: 614
		public const int k_iCallback = 3418;

		// Token: 0x04000267 RID: 615
		public AppId_t m_nAppID;
	}
}
