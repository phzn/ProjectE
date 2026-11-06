using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007F RID: 127
	[CallbackIdentity(516)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FavoritesListAccountsUpdated_t
	{
		// Token: 0x0400015D RID: 349
		public const int k_iCallback = 516;

		// Token: 0x0400015E RID: 350
		public EResult m_eResult;
	}
}
