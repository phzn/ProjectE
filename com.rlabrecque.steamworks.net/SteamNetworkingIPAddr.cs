using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AB RID: 427
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct SteamNetworkingIPAddr : IEquatable<SteamNetworkingIPAddr>
	{
		// Token: 0x06000A94 RID: 2708 RVA: 0x00010228 File Offset: 0x0000E428
		public void Clear()
		{
			NativeMethods.SteamAPI_SteamNetworkingIPAddr_Clear(ref this);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00010230 File Offset: 0x0000E430
		public bool IsIPv6AllZeros()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_IsIPv6AllZeros(ref this);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00010238 File Offset: 0x0000E438
		public void SetIPv6(byte[] ipv6, ushort nPort)
		{
			NativeMethods.SteamAPI_SteamNetworkingIPAddr_SetIPv6(ref this, ipv6, nPort);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00010242 File Offset: 0x0000E442
		public void SetIPv4(uint nIP, ushort nPort)
		{
			NativeMethods.SteamAPI_SteamNetworkingIPAddr_SetIPv4(ref this, nIP, nPort);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0001024C File Offset: 0x0000E44C
		public bool IsIPv4()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_IsIPv4(ref this);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00010254 File Offset: 0x0000E454
		public uint GetIPv4()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_GetIPv4(ref this);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0001025C File Offset: 0x0000E45C
		public void SetIPv6LocalHost(ushort nPort = 0)
		{
			NativeMethods.SteamAPI_SteamNetworkingIPAddr_SetIPv6LocalHost(ref this, nPort);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00010265 File Offset: 0x0000E465
		public bool IsLocalHost()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_IsLocalHost(ref this);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00010270 File Offset: 0x0000E470
		public void ToString(out string buf, bool bWithPort)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(48);
			NativeMethods.SteamAPI_SteamNetworkingIPAddr_ToString(ref this, intPtr, 48U, bWithPort);
			buf = InteropHelp.PtrToStringUTF8(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x000102A0 File Offset: 0x0000E4A0
		public bool ParseString(string pszStr)
		{
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszStr))
			{
				flag = NativeMethods.SteamAPI_SteamNetworkingIPAddr_ParseString(ref this, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x000102DC File Offset: 0x0000E4DC
		public bool Equals(SteamNetworkingIPAddr x)
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_IsEqualTo(ref this, ref x);
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x000102E6 File Offset: 0x0000E4E6
		public ESteamNetworkingFakeIPType GetFakeIPType()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_GetFakeIPType(ref this);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x000102EE File Offset: 0x0000E4EE
		public bool IsFakeIP()
		{
			return this.GetFakeIPType() > ESteamNetworkingFakeIPType.k_ESteamNetworkingFakeIPType_NotFake;
		}

		// Token: 0x04000BCD RID: 3021
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] m_ipv6;

		// Token: 0x04000BCE RID: 3022
		public ushort m_port;

		// Token: 0x04000BCF RID: 3023
		public const int k_cchMaxString = 48;
	}
}
