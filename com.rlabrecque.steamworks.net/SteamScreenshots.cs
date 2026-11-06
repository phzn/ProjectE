using System;

namespace Steamworks
{
	// Token: 0x02000020 RID: 32
	public static class SteamScreenshots
	{
		// Token: 0x060003AF RID: 943 RVA: 0x00009BD0 File Offset: 0x00007DD0
		public static ScreenshotHandle WriteScreenshot(byte[] pubRGB, uint cubRGB, int nWidth, int nHeight)
		{
			InteropHelp.TestIfAvailableClient();
			return (ScreenshotHandle)NativeMethods.ISteamScreenshots_WriteScreenshot(CSteamAPIContext.GetSteamScreenshots(), pubRGB, cubRGB, nWidth, nHeight);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00009BEC File Offset: 0x00007DEC
		public static ScreenshotHandle AddScreenshotToLibrary(string pchFilename, string pchThumbnailFilename, int nWidth, int nHeight)
		{
			InteropHelp.TestIfAvailableClient();
			ScreenshotHandle screenshotHandle;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFilename))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchThumbnailFilename))
				{
					screenshotHandle = (ScreenshotHandle)NativeMethods.ISteamScreenshots_AddScreenshotToLibrary(CSteamAPIContext.GetSteamScreenshots(), utf8StringHandle, utf8StringHandle2, nWidth, nHeight);
				}
			}
			return screenshotHandle;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00009C54 File Offset: 0x00007E54
		public static void TriggerScreenshot()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamScreenshots_TriggerScreenshot(CSteamAPIContext.GetSteamScreenshots());
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00009C65 File Offset: 0x00007E65
		public static void HookScreenshots(bool bHook)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamScreenshots_HookScreenshots(CSteamAPIContext.GetSteamScreenshots(), bHook);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00009C78 File Offset: 0x00007E78
		public static bool SetLocation(ScreenshotHandle hScreenshot, string pchLocation)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLocation))
			{
				flag = NativeMethods.ISteamScreenshots_SetLocation(CSteamAPIContext.GetSteamScreenshots(), hScreenshot, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00009CBC File Offset: 0x00007EBC
		public static bool TagUser(ScreenshotHandle hScreenshot, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_TagUser(CSteamAPIContext.GetSteamScreenshots(), hScreenshot, steamID);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00009CCF File Offset: 0x00007ECF
		public static bool TagPublishedFile(ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_TagPublishedFile(CSteamAPIContext.GetSteamScreenshots(), hScreenshot, unPublishedFileID);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00009CE2 File Offset: 0x00007EE2
		public static bool IsScreenshotsHooked()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_IsScreenshotsHooked(CSteamAPIContext.GetSteamScreenshots());
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00009CF4 File Offset: 0x00007EF4
		public static ScreenshotHandle AddVRScreenshotToLibrary(EVRScreenshotType eType, string pchFilename, string pchVRFilename)
		{
			InteropHelp.TestIfAvailableClient();
			ScreenshotHandle screenshotHandle;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFilename))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchVRFilename))
				{
					screenshotHandle = (ScreenshotHandle)NativeMethods.ISteamScreenshots_AddVRScreenshotToLibrary(CSteamAPIContext.GetSteamScreenshots(), eType, utf8StringHandle, utf8StringHandle2);
				}
			}
			return screenshotHandle;
		}
	}
}
