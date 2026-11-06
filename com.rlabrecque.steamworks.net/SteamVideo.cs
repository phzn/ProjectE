using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000026 RID: 38
	public static class SteamVideo
	{
		// Token: 0x0600049F RID: 1183 RVA: 0x0000C0C6 File Offset: 0x0000A2C6
		public static void GetVideoURL(AppId_t unVideoAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamVideo_GetVideoURL(CSteamAPIContext.GetSteamVideo(), unVideoAppID);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
		public static bool IsBroadcasting(out int pnNumViewers)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamVideo_IsBroadcasting(CSteamAPIContext.GetSteamVideo(), out pnNumViewers);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000C0EA File Offset: 0x0000A2EA
		public static void GetOPFSettings(AppId_t unVideoAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamVideo_GetOPFSettings(CSteamAPIContext.GetSteamVideo(), unVideoAppID);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000C0FC File Offset: 0x0000A2FC
		public static bool GetOPFStringForApp(AppId_t unVideoAppID, out string pchBuffer, ref int pnBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(pnBufferSize);
			bool flag = NativeMethods.ISteamVideo_GetOPFStringForApp(CSteamAPIContext.GetSteamVideo(), unVideoAppID, intPtr, ref pnBufferSize);
			pchBuffer = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}
	}
}
