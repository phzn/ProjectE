using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AA RID: 426
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct SteamNetworkingIdentity : IEquatable<SteamNetworkingIdentity>
	{
		// Token: 0x06000A7B RID: 2683 RVA: 0x00010074 File Offset: 0x0000E274
		public void Clear()
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_Clear(ref this);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0001007C File Offset: 0x0000E27C
		public bool IsInvalid()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_IsInvalid(ref this);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00010084 File Offset: 0x0000E284
		public void SetSteamID(CSteamID steamID)
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetSteamID(ref this, (ulong)steamID);
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00010092 File Offset: 0x0000E292
		public CSteamID GetSteamID()
		{
			return (CSteamID)NativeMethods.SteamAPI_SteamNetworkingIdentity_GetSteamID(ref this);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0001009F File Offset: 0x0000E29F
		public void SetSteamID64(ulong steamID)
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetSteamID64(ref this, steamID);
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x000100A8 File Offset: 0x0000E2A8
		public ulong GetSteamID64()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_GetSteamID64(ref this);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x000100B0 File Offset: 0x0000E2B0
		public bool SetXboxPairwiseID(string pszString)
		{
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszString))
			{
				flag = NativeMethods.SteamAPI_SteamNetworkingIdentity_SetXboxPairwiseID(ref this, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x000100EC File Offset: 0x0000E2EC
		public string GetXboxPairwiseID()
		{
			return InteropHelp.PtrToStringUTF8(NativeMethods.SteamAPI_SteamNetworkingIdentity_GetXboxPairwiseID(ref this));
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x000100F9 File Offset: 0x0000E2F9
		public void SetPSNID(ulong id)
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetPSNID(ref this, id);
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00010102 File Offset: 0x0000E302
		public ulong GetPSNID()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_GetPSNID(ref this);
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0001010A File Offset: 0x0000E30A
		public void SetIPAddr(SteamNetworkingIPAddr addr)
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetIPAddr(ref this, ref addr);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00010115 File Offset: 0x0000E315
		public SteamNetworkingIPAddr GetIPAddr()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0001011C File Offset: 0x0000E31C
		public void SetIPv4Addr(uint nIPv4, ushort nPort)
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetIPv4Addr(ref this, nIPv4, nPort);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00010126 File Offset: 0x0000E326
		public uint GetIPv4()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_GetIPv4(ref this);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0001012E File Offset: 0x0000E32E
		public ESteamNetworkingFakeIPType GetFakeIPType()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_GetFakeIPType(ref this);
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00010136 File Offset: 0x0000E336
		public bool IsFakeIP()
		{
			return this.GetFakeIPType() > ESteamNetworkingFakeIPType.k_ESteamNetworkingFakeIPType_NotFake;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00010141 File Offset: 0x0000E341
		public void SetLocalHost()
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetLocalHost(ref this);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00010149 File Offset: 0x0000E349
		public bool IsLocalHost()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_IsLocalHost(ref this);
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00010154 File Offset: 0x0000E354
		public bool SetGenericString(string pszString)
		{
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszString))
			{
				flag = NativeMethods.SteamAPI_SteamNetworkingIdentity_SetGenericString(ref this, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00010190 File Offset: 0x0000E390
		public string GetGenericString()
		{
			return InteropHelp.PtrToStringUTF8(NativeMethods.SteamAPI_SteamNetworkingIdentity_GetGenericString(ref this));
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0001019D File Offset: 0x0000E39D
		public bool SetGenericBytes(byte[] data, uint cbLen)
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_SetGenericBytes(ref this, data, cbLen);
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x000101A7 File Offset: 0x0000E3A7
		public byte[] GetGenericBytes(out int cbLen)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x000101AE File Offset: 0x0000E3AE
		public bool Equals(SteamNetworkingIdentity x)
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_IsEqualTo(ref this, ref x);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x000101B8 File Offset: 0x0000E3B8
		public void ToString(out string buf)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(128);
			NativeMethods.SteamAPI_SteamNetworkingIdentity_ToString(ref this, intPtr, 128U);
			buf = InteropHelp.PtrToStringUTF8(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x000101EC File Offset: 0x0000E3EC
		public bool ParseString(string pszStr)
		{
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszStr))
			{
				flag = NativeMethods.SteamAPI_SteamNetworkingIdentity_ParseString(ref this, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x04000BA7 RID: 2983
		public ESteamNetworkingIdentityType m_eType;

		// Token: 0x04000BA8 RID: 2984
		private int m_cbSize;

		// Token: 0x04000BA9 RID: 2985
		private uint m_reserved0;

		// Token: 0x04000BAA RID: 2986
		private uint m_reserved1;

		// Token: 0x04000BAB RID: 2987
		private uint m_reserved2;

		// Token: 0x04000BAC RID: 2988
		private uint m_reserved3;

		// Token: 0x04000BAD RID: 2989
		private uint m_reserved4;

		// Token: 0x04000BAE RID: 2990
		private uint m_reserved5;

		// Token: 0x04000BAF RID: 2991
		private uint m_reserved6;

		// Token: 0x04000BB0 RID: 2992
		private uint m_reserved7;

		// Token: 0x04000BB1 RID: 2993
		private uint m_reserved8;

		// Token: 0x04000BB2 RID: 2994
		private uint m_reserved9;

		// Token: 0x04000BB3 RID: 2995
		private uint m_reserved10;

		// Token: 0x04000BB4 RID: 2996
		private uint m_reserved11;

		// Token: 0x04000BB5 RID: 2997
		private uint m_reserved12;

		// Token: 0x04000BB6 RID: 2998
		private uint m_reserved13;

		// Token: 0x04000BB7 RID: 2999
		private uint m_reserved14;

		// Token: 0x04000BB8 RID: 3000
		private uint m_reserved15;

		// Token: 0x04000BB9 RID: 3001
		private uint m_reserved16;

		// Token: 0x04000BBA RID: 3002
		private uint m_reserved17;

		// Token: 0x04000BBB RID: 3003
		private uint m_reserved18;

		// Token: 0x04000BBC RID: 3004
		private uint m_reserved19;

		// Token: 0x04000BBD RID: 3005
		private uint m_reserved20;

		// Token: 0x04000BBE RID: 3006
		private uint m_reserved21;

		// Token: 0x04000BBF RID: 3007
		private uint m_reserved22;

		// Token: 0x04000BC0 RID: 3008
		private uint m_reserved23;

		// Token: 0x04000BC1 RID: 3009
		private uint m_reserved24;

		// Token: 0x04000BC2 RID: 3010
		private uint m_reserved25;

		// Token: 0x04000BC3 RID: 3011
		private uint m_reserved26;

		// Token: 0x04000BC4 RID: 3012
		private uint m_reserved27;

		// Token: 0x04000BC5 RID: 3013
		private uint m_reserved28;

		// Token: 0x04000BC6 RID: 3014
		private uint m_reserved29;

		// Token: 0x04000BC7 RID: 3015
		private uint m_reserved30;

		// Token: 0x04000BC8 RID: 3016
		private uint m_reserved31;

		// Token: 0x04000BC9 RID: 3017
		public const int k_cchMaxString = 128;

		// Token: 0x04000BCA RID: 3018
		public const int k_cchMaxGenericString = 32;

		// Token: 0x04000BCB RID: 3019
		public const int k_cchMaxXboxPairwiseID = 33;

		// Token: 0x04000BCC RID: 3020
		public const int k_cbMaxGenericBytes = 32;
	}
}
