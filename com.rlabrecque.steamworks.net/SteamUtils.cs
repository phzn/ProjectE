using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000025 RID: 37
	public static class SteamUtils
	{
		// Token: 0x0600047A RID: 1146 RVA: 0x0000BD14 File Offset: 0x00009F14
		public static uint GetSecondsSinceAppActive()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetSecondsSinceAppActive(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000BD25 File Offset: 0x00009F25
		public static uint GetSecondsSinceComputerActive()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetSecondsSinceComputerActive(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000BD36 File Offset: 0x00009F36
		public static EUniverse GetConnectedUniverse()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetConnectedUniverse(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000BD47 File Offset: 0x00009F47
		public static uint GetServerRealTime()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetServerRealTime(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000BD58 File Offset: 0x00009F58
		public static string GetIPCountry()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetIPCountry(CSteamAPIContext.GetSteamUtils()));
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000BD6E File Offset: 0x00009F6E
		public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetImageSize(CSteamAPIContext.GetSteamUtils(), iImage, out pnWidth, out pnHeight);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000BD82 File Offset: 0x00009F82
		public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetImageRGBA(CSteamAPIContext.GetSteamUtils(), iImage, pubDest, nDestBufferSize);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000BD96 File Offset: 0x00009F96
		public static byte GetCurrentBatteryPower()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetCurrentBatteryPower(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000BDA7 File Offset: 0x00009FA7
		public static AppId_t GetAppID()
		{
			InteropHelp.TestIfAvailableClient();
			return (AppId_t)NativeMethods.ISteamUtils_GetAppID(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000BDBD File Offset: 0x00009FBD
		public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetOverlayNotificationPosition(CSteamAPIContext.GetSteamUtils(), eNotificationPosition);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000BDCF File Offset: 0x00009FCF
		public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsAPICallCompleted(CSteamAPIContext.GetSteamUtils(), hSteamAPICall, out pbFailed);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000BDE2 File Offset: 0x00009FE2
		public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetAPICallFailureReason(CSteamAPIContext.GetSteamUtils(), hSteamAPICall);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000BDF4 File Offset: 0x00009FF4
		public static bool GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetAPICallResult(CSteamAPIContext.GetSteamUtils(), hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000BE0B File Offset: 0x0000A00B
		public static uint GetIPCCallCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetIPCCallCount(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000BE1C File Offset: 0x0000A01C
		public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetWarningMessageHook(CSteamAPIContext.GetSteamUtils(), pFunction);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000BE2E File Offset: 0x0000A02E
		public static bool IsOverlayEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsOverlayEnabled(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000BE3F File Offset: 0x0000A03F
		public static bool BOverlayNeedsPresent()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_BOverlayNeedsPresent(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000BE50 File Offset: 0x0000A050
		public static SteamAPICall_t CheckFileSignature(string szFileName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(szFileName))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamUtils_CheckFileSignature(CSteamAPIContext.GetSteamUtils(), utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000BE98 File Offset: 0x0000A098
		public static bool ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchExistingText))
				{
					flag = NativeMethods.ISteamUtils_ShowGamepadTextInput(CSteamAPIContext.GetSteamUtils(), eInputMode, eLineInputMode, utf8StringHandle, unCharMax, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000BEFC File Offset: 0x0000A0FC
		public static uint GetEnteredGamepadTextLength()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetEnteredGamepadTextLength(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000BF10 File Offset: 0x0000A110
		public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchText);
			bool flag = NativeMethods.ISteamUtils_GetEnteredGamepadTextInput(CSteamAPIContext.GetSteamUtils(), intPtr, cchText);
			pchText = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000BF4B File Offset: 0x0000A14B
		public static string GetSteamUILanguage()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetSteamUILanguage(CSteamAPIContext.GetSteamUtils()));
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000BF61 File Offset: 0x0000A161
		public static bool IsSteamRunningInVR()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsSteamRunningInVR(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000BF72 File Offset: 0x0000A172
		public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetOverlayNotificationInset(CSteamAPIContext.GetSteamUtils(), nHorizontalInset, nVerticalInset);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000BF85 File Offset: 0x0000A185
		public static bool IsSteamInBigPictureMode()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsSteamInBigPictureMode(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000BF96 File Offset: 0x0000A196
		public static void StartVRDashboard()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_StartVRDashboard(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000BFA7 File Offset: 0x0000A1A7
		public static bool IsVRHeadsetStreamingEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsVRHeadsetStreamingEnabled(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000BFB8 File Offset: 0x0000A1B8
		public static void SetVRHeadsetStreamingEnabled(bool bEnabled)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetVRHeadsetStreamingEnabled(CSteamAPIContext.GetSteamUtils(), bEnabled);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000BFCA File Offset: 0x0000A1CA
		public static bool IsSteamChinaLauncher()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsSteamChinaLauncher(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000BFDB File Offset: 0x0000A1DB
		public static bool InitFilterText(uint unFilterOptions = 0U)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_InitFilterText(CSteamAPIContext.GetSteamUtils(), unFilterOptions);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000BFF0 File Offset: 0x0000A1F0
		public static int FilterText(ETextFilteringContext eContext, CSteamID sourceSteamID, string pchInputMessage, out string pchOutFilteredText, uint nByteSizeOutFilteredText)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)nByteSizeOutFilteredText);
			int num2;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchInputMessage))
			{
				int num = NativeMethods.ISteamUtils_FilterText(CSteamAPIContext.GetSteamUtils(), eContext, sourceSteamID, utf8StringHandle, intPtr, nByteSizeOutFilteredText);
				pchOutFilteredText = ((num != -1) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
				Marshal.FreeHGlobal(intPtr);
				num2 = num;
			}
			return num2;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000C058 File Offset: 0x0000A258
		public static ESteamIPv6ConnectivityState GetIPv6ConnectivityState(ESteamIPv6ConnectivityProtocol eProtocol)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetIPv6ConnectivityState(CSteamAPIContext.GetSteamUtils(), eProtocol);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000C06A File Offset: 0x0000A26A
		public static bool IsSteamRunningOnSteamDeck()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsSteamRunningOnSteamDeck(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000C07B File Offset: 0x0000A27B
		public static bool ShowFloatingGamepadTextInput(EFloatingGamepadTextInputMode eKeyboardMode, int nTextFieldXPosition, int nTextFieldYPosition, int nTextFieldWidth, int nTextFieldHeight)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_ShowFloatingGamepadTextInput(CSteamAPIContext.GetSteamUtils(), eKeyboardMode, nTextFieldXPosition, nTextFieldYPosition, nTextFieldWidth, nTextFieldHeight);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000C092 File Offset: 0x0000A292
		public static void SetGameLauncherMode(bool bLauncherMode)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetGameLauncherMode(CSteamAPIContext.GetSteamUtils(), bLauncherMode);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000C0A4 File Offset: 0x0000A2A4
		public static bool DismissFloatingGamepadTextInput()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_DismissFloatingGamepadTextInput(CSteamAPIContext.GetSteamUtils());
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000C0B5 File Offset: 0x0000A2B5
		public static bool DismissGamepadTextInput()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_DismissGamepadTextInput(CSteamAPIContext.GetSteamUtils());
		}
	}
}
