using System;

namespace Steamworks
{
	// Token: 0x02000011 RID: 17
	public static class SteamHTMLSurface
	{
		// Token: 0x06000214 RID: 532 RVA: 0x0000691E File Offset: 0x00004B1E
		public static bool Init()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTMLSurface_Init(CSteamAPIContext.GetSteamHTMLSurface());
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000692F File Offset: 0x00004B2F
		public static bool Shutdown()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTMLSurface_Shutdown(CSteamAPIContext.GetSteamHTMLSurface());
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00006940 File Offset: 0x00004B40
		public static SteamAPICall_t CreateBrowser(string pchUserAgent, string pchUserCSS)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchUserAgent))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchUserCSS))
				{
					steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamHTMLSurface_CreateBrowser(CSteamAPIContext.GetSteamHTMLSurface(), utf8StringHandle, utf8StringHandle2);
				}
			}
			return steamAPICall_t;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000069A8 File Offset: 0x00004BA8
		public static void RemoveBrowser(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_RemoveBrowser(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000069BC File Offset: 0x00004BBC
		public static void LoadURL(HHTMLBrowser unBrowserHandle, string pchURL, string pchPostData)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchURL))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchPostData))
				{
					NativeMethods.ISteamHTMLSurface_LoadURL(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00006A1C File Offset: 0x00004C1C
		public static void SetSize(HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetSize(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, unWidth, unHeight);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00006A30 File Offset: 0x00004C30
		public static void StopLoad(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_StopLoad(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00006A42 File Offset: 0x00004C42
		public static void Reload(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_Reload(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00006A54 File Offset: 0x00004C54
		public static void GoBack(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GoBack(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00006A66 File Offset: 0x00004C66
		public static void GoForward(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GoForward(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00006A78 File Offset: 0x00004C78
		public static void AddHeader(HHTMLBrowser unBrowserHandle, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					NativeMethods.ISteamHTMLSurface_AddHeader(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00006AD8 File Offset: 0x00004CD8
		public static void ExecuteJavascript(HHTMLBrowser unBrowserHandle, string pchScript)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchScript))
			{
				NativeMethods.ISteamHTMLSurface_ExecuteJavascript(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, utf8StringHandle);
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00006B1C File Offset: 0x00004D1C
		public static void MouseUp(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseUp(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, eMouseButton);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00006B2F File Offset: 0x00004D2F
		public static void MouseDown(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseDown(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, eMouseButton);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00006B42 File Offset: 0x00004D42
		public static void MouseDoubleClick(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseDoubleClick(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, eMouseButton);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00006B55 File Offset: 0x00004D55
		public static void MouseMove(HHTMLBrowser unBrowserHandle, int x, int y)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseMove(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, x, y);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00006B69 File Offset: 0x00004D69
		public static void MouseWheel(HHTMLBrowser unBrowserHandle, int nDelta)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseWheel(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, nDelta);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00006B7C File Offset: 0x00004D7C
		public static void KeyDown(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers, bool bIsSystemKey = false)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyDown(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers, bIsSystemKey);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00006B91 File Offset: 0x00004D91
		public static void KeyUp(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyUp(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00006BA5 File Offset: 0x00004DA5
		public static void KeyChar(HHTMLBrowser unBrowserHandle, uint cUnicodeChar, EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyChar(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, cUnicodeChar, eHTMLKeyModifiers);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00006BB9 File Offset: 0x00004DB9
		public static void SetHorizontalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetHorizontalScroll(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, nAbsolutePixelScroll);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00006BCC File Offset: 0x00004DCC
		public static void SetVerticalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetVerticalScroll(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, nAbsolutePixelScroll);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00006BDF File Offset: 0x00004DDF
		public static void SetKeyFocus(HHTMLBrowser unBrowserHandle, bool bHasKeyFocus)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetKeyFocus(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, bHasKeyFocus);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00006BF2 File Offset: 0x00004DF2
		public static void ViewSource(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_ViewSource(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00006C04 File Offset: 0x00004E04
		public static void CopyToClipboard(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_CopyToClipboard(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00006C16 File Offset: 0x00004E16
		public static void PasteFromClipboard(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_PasteFromClipboard(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00006C28 File Offset: 0x00004E28
		public static void Find(HHTMLBrowser unBrowserHandle, string pchSearchStr, bool bCurrentlyInFind, bool bReverse)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchSearchStr))
			{
				NativeMethods.ISteamHTMLSurface_Find(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, utf8StringHandle, bCurrentlyInFind, bReverse);
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00006C6C File Offset: 0x00004E6C
		public static void StopFind(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_StopFind(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00006C7E File Offset: 0x00004E7E
		public static void GetLinkAtPosition(HHTMLBrowser unBrowserHandle, int x, int y)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GetLinkAtPosition(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, x, y);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00006C94 File Offset: 0x00004E94
		public static void SetCookie(string pchHostname, string pchKey, string pchValue, string pchPath = "/", uint nExpires = 0U, bool bSecure = false, bool bHTTPOnly = false)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHostname))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchKey))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchValue))
					{
						using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle(pchPath))
						{
							NativeMethods.ISteamHTMLSurface_SetCookie(CSteamAPIContext.GetSteamHTMLSurface(), utf8StringHandle, utf8StringHandle2, utf8StringHandle3, utf8StringHandle4, nExpires, bSecure, bHTTPOnly);
						}
					}
				}
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00006D34 File Offset: 0x00004F34
		public static void SetPageScaleFactor(HHTMLBrowser unBrowserHandle, float flZoom, int nPointX, int nPointY)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetPageScaleFactor(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, flZoom, nPointX, nPointY);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00006D49 File Offset: 0x00004F49
		public static void SetBackgroundMode(HHTMLBrowser unBrowserHandle, bool bBackgroundMode)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetBackgroundMode(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, bBackgroundMode);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00006D5C File Offset: 0x00004F5C
		public static void SetDPIScalingFactor(HHTMLBrowser unBrowserHandle, float flDPIScaling)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetDPIScalingFactor(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, flDPIScaling);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00006D6F File Offset: 0x00004F6F
		public static void OpenDeveloperTools(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_OpenDeveloperTools(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00006D81 File Offset: 0x00004F81
		public static void AllowStartRequest(HHTMLBrowser unBrowserHandle, bool bAllowed)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_AllowStartRequest(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, bAllowed);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00006D94 File Offset: 0x00004F94
		public static void JSDialogResponse(HHTMLBrowser unBrowserHandle, bool bResult)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_JSDialogResponse(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, bResult);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00006DA7 File Offset: 0x00004FA7
		public static void FileLoadDialogResponse(HHTMLBrowser unBrowserHandle, IntPtr pchSelectedFiles)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_FileLoadDialogResponse(CSteamAPIContext.GetSteamHTMLSurface(), unBrowserHandle, pchSelectedFiles);
		}
	}
}
