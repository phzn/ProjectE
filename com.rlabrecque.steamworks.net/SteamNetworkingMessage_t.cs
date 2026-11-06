using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AC RID: 428
	[Serializable]
	public struct SteamNetworkingMessage_t
	{
		// Token: 0x06000AA1 RID: 2721 RVA: 0x000102F9 File Offset: 0x0000E4F9
		public void Release()
		{
			throw new NotImplementedException("Please use the static Release function instead which takes an IntPtr.");
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00010305 File Offset: 0x0000E505
		public static void Release(IntPtr pointer)
		{
			NativeMethods.SteamAPI_SteamNetworkingMessage_t_Release(pointer);
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0001030D File Offset: 0x0000E50D
		public static SteamNetworkingMessage_t FromIntPtr(IntPtr pointer)
		{
			return Marshal.PtrToStructure<SteamNetworkingMessage_t>(pointer);
		}

		// Token: 0x04000BD0 RID: 3024
		public IntPtr m_pData;

		// Token: 0x04000BD1 RID: 3025
		public int m_cbSize;

		// Token: 0x04000BD2 RID: 3026
		public HSteamNetConnection m_conn;

		// Token: 0x04000BD3 RID: 3027
		public SteamNetworkingIdentity m_identityPeer;

		// Token: 0x04000BD4 RID: 3028
		public long m_nConnUserData;

		// Token: 0x04000BD5 RID: 3029
		public SteamNetworkingMicroseconds m_usecTimeReceived;

		// Token: 0x04000BD6 RID: 3030
		public long m_nMessageNumber;

		// Token: 0x04000BD7 RID: 3031
		public IntPtr m_pfnFreeData;

		// Token: 0x04000BD8 RID: 3032
		internal IntPtr m_pfnRelease;

		// Token: 0x04000BD9 RID: 3033
		public int m_nChannel;

		// Token: 0x04000BDA RID: 3034
		public int m_nFlags;

		// Token: 0x04000BDB RID: 3035
		public long m_nUserData;

		// Token: 0x04000BDC RID: 3036
		public ushort m_idxLane;

		// Token: 0x04000BDD RID: 3037
		public ushort _pad1__;
	}
}
