using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000028 RID: 40
	[CallbackIdentity(1005)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DlcInstalled_t
	{
		// Token: 0x04000003 RID: 3
		public const int k_iCallback = 1005;

		// Token: 0x04000004 RID: 4
		public AppId_t m_nAppID;
	}
}
