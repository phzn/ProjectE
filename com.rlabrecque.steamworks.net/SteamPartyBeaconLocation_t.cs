using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000164 RID: 356
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamPartyBeaconLocation_t
	{
		// Token: 0x04000AA2 RID: 2722
		public ESteamPartyBeaconLocationType m_eType;

		// Token: 0x04000AA3 RID: 2723
		public ulong m_ulLocationID;
	}
}
