using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000046 RID: 70
	[CallbackIdentity(203)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSClientKick_t
	{
		// Token: 0x0400006B RID: 107
		public const int k_iCallback = 203;

		// Token: 0x0400006C RID: 108
		public CSteamID m_SteamID;

		// Token: 0x0400006D RID: 109
		public EDenyReason m_eDenyReason;
	}
}
