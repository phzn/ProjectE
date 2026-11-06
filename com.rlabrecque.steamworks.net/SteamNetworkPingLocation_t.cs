using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000171 RID: 369
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetworkPingLocation_t
	{
		// Token: 0x04000B00 RID: 2816
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
		public byte[] m_data;
	}
}
