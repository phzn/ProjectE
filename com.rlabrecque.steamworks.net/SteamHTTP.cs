using System;

namespace Steamworks
{
	// Token: 0x02000012 RID: 18
	public static class SteamHTTP
	{
		// Token: 0x06000239 RID: 569 RVA: 0x00006DBC File Offset: 0x00004FBC
		public static HTTPRequestHandle CreateHTTPRequest(EHTTPMethod eHTTPRequestMethod, string pchAbsoluteURL)
		{
			InteropHelp.TestIfAvailableClient();
			HTTPRequestHandle httprequestHandle;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchAbsoluteURL))
			{
				httprequestHandle = (HTTPRequestHandle)NativeMethods.ISteamHTTP_CreateHTTPRequest(CSteamAPIContext.GetSteamHTTP(), eHTTPRequestMethod, utf8StringHandle);
			}
			return httprequestHandle;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00006E04 File Offset: 0x00005004
		public static bool SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SetHTTPRequestContextValue(CSteamAPIContext.GetSteamHTTP(), hRequest, ulContextValue);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00006E17 File Offset: 0x00005017
		public static bool SetHTTPRequestNetworkActivityTimeout(HTTPRequestHandle hRequest, uint unTimeoutSeconds)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SetHTTPRequestNetworkActivityTimeout(CSteamAPIContext.GetSteamHTTP(), hRequest, unTimeoutSeconds);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00006E2C File Offset: 0x0000502C
		public static bool SetHTTPRequestHeaderValue(HTTPRequestHandle hRequest, string pchHeaderName, string pchHeaderValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchHeaderValue))
				{
					flag = NativeMethods.ISteamHTTP_SetHTTPRequestHeaderValue(CSteamAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00006E90 File Offset: 0x00005090
		public static bool SetHTTPRequestGetOrPostParameter(HTTPRequestHandle hRequest, string pchParamName, string pchParamValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchParamName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchParamValue))
				{
					flag = NativeMethods.ISteamHTTP_SetHTTPRequestGetOrPostParameter(CSteamAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00006EF4 File Offset: 0x000050F4
		public static bool SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SendHTTPRequest(CSteamAPIContext.GetSteamHTTP(), hRequest, out pCallHandle);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00006F07 File Offset: 0x00005107
		public static bool SendHTTPRequestAndStreamResponse(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SendHTTPRequestAndStreamResponse(CSteamAPIContext.GetSteamHTTP(), hRequest, out pCallHandle);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00006F1A File Offset: 0x0000511A
		public static bool DeferHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_DeferHTTPRequest(CSteamAPIContext.GetSteamHTTP(), hRequest);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00006F2C File Offset: 0x0000512C
		public static bool PrioritizeHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_PrioritizeHTTPRequest(CSteamAPIContext.GetSteamHTTP(), hRequest);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00006F40 File Offset: 0x00005140
		public static bool GetHTTPResponseHeaderSize(HTTPRequestHandle hRequest, string pchHeaderName, out uint unResponseHeaderSize)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				flag = NativeMethods.ISteamHTTP_GetHTTPResponseHeaderSize(CSteamAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle, out unResponseHeaderSize);
			}
			return flag;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00006F84 File Offset: 0x00005184
		public static bool GetHTTPResponseHeaderValue(HTTPRequestHandle hRequest, string pchHeaderName, byte[] pHeaderValueBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				flag = NativeMethods.ISteamHTTP_GetHTTPResponseHeaderValue(CSteamAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle, pHeaderValueBuffer, unBufferSize);
			}
			return flag;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00006FCC File Offset: 0x000051CC
		public static bool GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_GetHTTPResponseBodySize(CSteamAPIContext.GetSteamHTTP(), hRequest, out unBodySize);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00006FDF File Offset: 0x000051DF
		public static bool GetHTTPResponseBodyData(HTTPRequestHandle hRequest, byte[] pBodyDataBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_GetHTTPResponseBodyData(CSteamAPIContext.GetSteamHTTP(), hRequest, pBodyDataBuffer, unBufferSize);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00006FF3 File Offset: 0x000051F3
		public static bool GetHTTPStreamingResponseBodyData(HTTPRequestHandle hRequest, uint cOffset, byte[] pBodyDataBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_GetHTTPStreamingResponseBodyData(CSteamAPIContext.GetSteamHTTP(), hRequest, cOffset, pBodyDataBuffer, unBufferSize);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00007008 File Offset: 0x00005208
		public static bool ReleaseHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_ReleaseHTTPRequest(CSteamAPIContext.GetSteamHTTP(), hRequest);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000701A File Offset: 0x0000521A
		public static bool GetHTTPDownloadProgressPct(HTTPRequestHandle hRequest, out float pflPercentOut)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_GetHTTPDownloadProgressPct(CSteamAPIContext.GetSteamHTTP(), hRequest, out pflPercentOut);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00007030 File Offset: 0x00005230
		public static bool SetHTTPRequestRawPostBody(HTTPRequestHandle hRequest, string pchContentType, byte[] pubBody, uint unBodyLen)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchContentType))
			{
				flag = NativeMethods.ISteamHTTP_SetHTTPRequestRawPostBody(CSteamAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle, pubBody, unBodyLen);
			}
			return flag;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00007078 File Offset: 0x00005278
		public static HTTPCookieContainerHandle CreateCookieContainer(bool bAllowResponsesToModify)
		{
			InteropHelp.TestIfAvailableClient();
			return (HTTPCookieContainerHandle)NativeMethods.ISteamHTTP_CreateCookieContainer(CSteamAPIContext.GetSteamHTTP(), bAllowResponsesToModify);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000708F File Offset: 0x0000528F
		public static bool ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_ReleaseCookieContainer(CSteamAPIContext.GetSteamHTTP(), hCookieContainer);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x000070A4 File Offset: 0x000052A4
		public static bool SetCookie(HTTPCookieContainerHandle hCookieContainer, string pchHost, string pchUrl, string pchCookie)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHost))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchUrl))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchCookie))
					{
						flag = NativeMethods.ISteamHTTP_SetCookie(CSteamAPIContext.GetSteamHTTP(), hCookieContainer, utf8StringHandle, utf8StringHandle2, utf8StringHandle3);
					}
				}
			}
			return flag;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00007124 File Offset: 0x00005324
		public static bool SetHTTPRequestCookieContainer(HTTPRequestHandle hRequest, HTTPCookieContainerHandle hCookieContainer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SetHTTPRequestCookieContainer(CSteamAPIContext.GetSteamHTTP(), hRequest, hCookieContainer);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00007138 File Offset: 0x00005338
		public static bool SetHTTPRequestUserAgentInfo(HTTPRequestHandle hRequest, string pchUserAgentInfo)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchUserAgentInfo))
			{
				flag = NativeMethods.ISteamHTTP_SetHTTPRequestUserAgentInfo(CSteamAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000717C File Offset: 0x0000537C
		public static bool SetHTTPRequestRequiresVerifiedCertificate(HTTPRequestHandle hRequest, bool bRequireVerifiedCertificate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate(CSteamAPIContext.GetSteamHTTP(), hRequest, bRequireVerifiedCertificate);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000718F File Offset: 0x0000538F
		public static bool SetHTTPRequestAbsoluteTimeoutMS(HTTPRequestHandle hRequest, uint unMilliseconds)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS(CSteamAPIContext.GetSteamHTTP(), hRequest, unMilliseconds);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000071A2 File Offset: 0x000053A2
		public static bool GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_GetHTTPRequestWasTimedOut(CSteamAPIContext.GetSteamHTTP(), hRequest, out pbWasTimedOut);
		}
	}
}
