using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A9 RID: 425
	[Serializable]
	public struct SteamNetworkingErrMsg
	{
		// Token: 0x04000BA6 RID: 2982
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		public byte[] m_SteamNetworkingErrMsg;
	}
}
