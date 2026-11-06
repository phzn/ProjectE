using System;

namespace Steamworks
{
	// Token: 0x0200001E RID: 30
	public static class SteamRemotePlay
	{
		// Token: 0x06000360 RID: 864 RVA: 0x00008FDE File Offset: 0x000071DE
		public static uint GetSessionCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_GetSessionCount(CSteamAPIContext.GetSteamRemotePlay());
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00008FEF File Offset: 0x000071EF
		public static RemotePlaySessionID_t GetSessionID(int iSessionIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (RemotePlaySessionID_t)NativeMethods.ISteamRemotePlay_GetSessionID(CSteamAPIContext.GetSteamRemotePlay(), iSessionIndex);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00009006 File Offset: 0x00007206
		public static bool BSessionRemotePlayTogether(RemotePlaySessionID_t unSessionID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_BSessionRemotePlayTogether(CSteamAPIContext.GetSteamRemotePlay(), unSessionID);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00009018 File Offset: 0x00007218
		public static CSteamID GetSessionSteamID(RemotePlaySessionID_t unSessionID)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamRemotePlay_GetSessionSteamID(CSteamAPIContext.GetSteamRemotePlay(), unSessionID);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000902F File Offset: 0x0000722F
		public static uint GetSessionGuestID(RemotePlaySessionID_t unSessionID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_GetSessionGuestID(CSteamAPIContext.GetSteamRemotePlay(), unSessionID);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00009041 File Offset: 0x00007241
		public static int GetSmallSessionAvatar(RemotePlaySessionID_t unSessionID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_GetSmallSessionAvatar(CSteamAPIContext.GetSteamRemotePlay(), unSessionID);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00009053 File Offset: 0x00007253
		public static int GetMediumSessionAvatar(RemotePlaySessionID_t unSessionID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_GetMediumSessionAvatar(CSteamAPIContext.GetSteamRemotePlay(), unSessionID);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00009065 File Offset: 0x00007265
		public static int GetLargeSessionAvatar(RemotePlaySessionID_t unSessionID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_GetLargeSessionAvatar(CSteamAPIContext.GetSteamRemotePlay(), unSessionID);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00009077 File Offset: 0x00007277
		public static string GetSessionClientName(RemotePlaySessionID_t unSessionID)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamRemotePlay_GetSessionClientName(CSteamAPIContext.GetSteamRemotePlay(), unSessionID));
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000908E File Offset: 0x0000728E
		public static ESteamDeviceFormFactor GetSessionClientFormFactor(RemotePlaySessionID_t unSessionID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_GetSessionClientFormFactor(CSteamAPIContext.GetSteamRemotePlay(), unSessionID);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000090A0 File Offset: 0x000072A0
		public static bool BGetSessionClientResolution(RemotePlaySessionID_t unSessionID, out int pnResolutionX, out int pnResolutionY)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_BGetSessionClientResolution(CSteamAPIContext.GetSteamRemotePlay(), unSessionID, out pnResolutionX, out pnResolutionY);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x000090B4 File Offset: 0x000072B4
		public static bool ShowRemotePlayTogetherUI()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_ShowRemotePlayTogetherUI(CSteamAPIContext.GetSteamRemotePlay());
		}

		// Token: 0x0600036C RID: 876 RVA: 0x000090C5 File Offset: 0x000072C5
		public static bool BSendRemotePlayTogetherInvite(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_BSendRemotePlayTogetherInvite(CSteamAPIContext.GetSteamRemotePlay(), steamIDFriend);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x000090D7 File Offset: 0x000072D7
		public static bool BEnableRemotePlayTogetherDirectInput()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_BEnableRemotePlayTogetherDirectInput(CSteamAPIContext.GetSteamRemotePlay());
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000090E8 File Offset: 0x000072E8
		public static void DisableRemotePlayTogetherDirectInput()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamRemotePlay_DisableRemotePlayTogetherDirectInput(CSteamAPIContext.GetSteamRemotePlay());
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000090F9 File Offset: 0x000072F9
		public static uint GetInput(RemotePlayInput_t[] pInput, uint unMaxEvents)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemotePlay_GetInput(CSteamAPIContext.GetSteamRemotePlay(), pInput, unMaxEvents);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000910C File Offset: 0x0000730C
		public static void SetMouseVisibility(RemotePlaySessionID_t unSessionID, bool bVisible)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamRemotePlay_SetMouseVisibility(CSteamAPIContext.GetSteamRemotePlay(), unSessionID, bVisible);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000911F File Offset: 0x0000731F
		public static void SetMousePosition(RemotePlaySessionID_t unSessionID, float flNormalizedX, float flNormalizedY)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamRemotePlay_SetMousePosition(CSteamAPIContext.GetSteamRemotePlay(), unSessionID, flNormalizedX, flNormalizedY);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00009133 File Offset: 0x00007333
		public static RemotePlayCursorID_t CreateMouseCursor(int nWidth, int nHeight, int nHotX, int nHotY, IntPtr pBGRA, int nPitch = 0)
		{
			InteropHelp.TestIfAvailableClient();
			return (RemotePlayCursorID_t)NativeMethods.ISteamRemotePlay_CreateMouseCursor(CSteamAPIContext.GetSteamRemotePlay(), nWidth, nHeight, nHotX, nHotY, pBGRA, nPitch);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00009151 File Offset: 0x00007351
		public static void SetMouseCursor(RemotePlaySessionID_t unSessionID, RemotePlayCursorID_t unCursorID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamRemotePlay_SetMouseCursor(CSteamAPIContext.GetSteamRemotePlay(), unSessionID, unCursorID);
		}
	}
}
