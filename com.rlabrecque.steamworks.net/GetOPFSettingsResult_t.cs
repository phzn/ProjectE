using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EA RID: 234
	[CallbackIdentity(4624)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetOPFSettingsResult_t
	{
		// Token: 0x040002E7 RID: 743
		public const int k_iCallback = 4624;

		// Token: 0x040002E8 RID: 744
		public EResult m_eResult;

		// Token: 0x040002E9 RID: 745
		public AppId_t m_unVideoAppID;
	}
}
