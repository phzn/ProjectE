using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004F RID: 79
	[CallbackIdentity(1801)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSStatsStored_t
	{
		// Token: 0x04000091 RID: 145
		public const int k_iCallback = 1801;

		// Token: 0x04000092 RID: 146
		public EResult m_eResult;

		// Token: 0x04000093 RID: 147
		public CSteamID m_steamIDUser;
	}
}
