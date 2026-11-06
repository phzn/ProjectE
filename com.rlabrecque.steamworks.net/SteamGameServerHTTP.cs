using System;

namespace Steamworks
{
	// Token: 0x02000008 RID: 8
	public static class SteamGameServerHTTP
	{
		// Token: 0x060000DD RID: 221 RVA: 0x000040BC File Offset: 0x000022BC
		public static HTTPRequestHandle CreateHTTPRequest(EHTTPMethod eHTTPRequestMethod, string pchAbsoluteURL)
		{
			InteropHelp.TestIfAvailableGameServer();
			HTTPRequestHandle httprequestHandle;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchAbsoluteURL))
			{
				httprequestHandle = (HTTPRequestHandle)NativeMethods.ISteamHTTP_CreateHTTPRequest(CSteamGameServerAPIContext.GetSteamHTTP(), eHTTPRequestMethod, utf8StringHandle);
			}
			return httprequestHandle;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004104 File Offset: 0x00002304
		public static bool SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_SetHTTPRequestContextValue(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, ulContextValue);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004117 File Offset: 0x00002317
		public static bool SetHTTPRequestNetworkActivityTimeout(HTTPRequestHandle hRequest, uint unTimeoutSeconds)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_SetHTTPRequestNetworkActivityTimeout(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, unTimeoutSeconds);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000412C File Offset: 0x0000232C
		public static bool SetHTTPRequestHeaderValue(HTTPRequestHandle hRequest, string pchHeaderName, string pchHeaderValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchHeaderValue))
				{
					flag = NativeMethods.ISteamHTTP_SetHTTPRequestHeaderValue(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004190 File Offset: 0x00002390
		public static bool SetHTTPRequestGetOrPostParameter(HTTPRequestHandle hRequest, string pchParamName, string pchParamValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchParamName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchParamValue))
				{
					flag = NativeMethods.ISteamHTTP_SetHTTPRequestGetOrPostParameter(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000041F4 File Offset: 0x000023F4
		public static bool SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_SendHTTPRequest(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, out pCallHandle);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004207 File Offset: 0x00002407
		public static bool SendHTTPRequestAndStreamResponse(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_SendHTTPRequestAndStreamResponse(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, out pCallHandle);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000421A File Offset: 0x0000241A
		public static bool DeferHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_DeferHTTPRequest(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000422C File Offset: 0x0000242C
		public static bool PrioritizeHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_PrioritizeHTTPRequest(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004240 File Offset: 0x00002440
		public static bool GetHTTPResponseHeaderSize(HTTPRequestHandle hRequest, string pchHeaderName, out uint unResponseHeaderSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				flag = NativeMethods.ISteamHTTP_GetHTTPResponseHeaderSize(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle, out unResponseHeaderSize);
			}
			return flag;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004284 File Offset: 0x00002484
		public static bool GetHTTPResponseHeaderValue(HTTPRequestHandle hRequest, string pchHeaderName, byte[] pHeaderValueBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				flag = NativeMethods.ISteamHTTP_GetHTTPResponseHeaderValue(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle, pHeaderValueBuffer, unBufferSize);
			}
			return flag;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000042CC File Offset: 0x000024CC
		public static bool GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_GetHTTPResponseBodySize(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, out unBodySize);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000042DF File Offset: 0x000024DF
		public static bool GetHTTPResponseBodyData(HTTPRequestHandle hRequest, byte[] pBodyDataBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_GetHTTPResponseBodyData(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, pBodyDataBuffer, unBufferSize);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000042F3 File Offset: 0x000024F3
		public static bool GetHTTPStreamingResponseBodyData(HTTPRequestHandle hRequest, uint cOffset, byte[] pBodyDataBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_GetHTTPStreamingResponseBodyData(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, cOffset, pBodyDataBuffer, unBufferSize);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004308 File Offset: 0x00002508
		public static bool ReleaseHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_ReleaseHTTPRequest(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000431A File Offset: 0x0000251A
		public static bool GetHTTPDownloadProgressPct(HTTPRequestHandle hRequest, out float pflPercentOut)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_GetHTTPDownloadProgressPct(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, out pflPercentOut);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004330 File Offset: 0x00002530
		public static bool SetHTTPRequestRawPostBody(HTTPRequestHandle hRequest, string pchContentType, byte[] pubBody, uint unBodyLen)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchContentType))
			{
				flag = NativeMethods.ISteamHTTP_SetHTTPRequestRawPostBody(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle, pubBody, unBodyLen);
			}
			return flag;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004378 File Offset: 0x00002578
		public static HTTPCookieContainerHandle CreateCookieContainer(bool bAllowResponsesToModify)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (HTTPCookieContainerHandle)NativeMethods.ISteamHTTP_CreateCookieContainer(CSteamGameServerAPIContext.GetSteamHTTP(), bAllowResponsesToModify);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000438F File Offset: 0x0000258F
		public static bool ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_ReleaseCookieContainer(CSteamGameServerAPIContext.GetSteamHTTP(), hCookieContainer);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000043A4 File Offset: 0x000025A4
		public static bool SetCookie(HTTPCookieContainerHandle hCookieContainer, string pchHost, string pchUrl, string pchCookie)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHost))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchUrl))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchCookie))
					{
						flag = NativeMethods.ISteamHTTP_SetCookie(CSteamGameServerAPIContext.GetSteamHTTP(), hCookieContainer, utf8StringHandle, utf8StringHandle2, utf8StringHandle3);
					}
				}
			}
			return flag;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004424 File Offset: 0x00002624
		public static bool SetHTTPRequestCookieContainer(HTTPRequestHandle hRequest, HTTPCookieContainerHandle hCookieContainer)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_SetHTTPRequestCookieContainer(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, hCookieContainer);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004438 File Offset: 0x00002638
		public static bool SetHTTPRequestUserAgentInfo(HTTPRequestHandle hRequest, string pchUserAgentInfo)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchUserAgentInfo))
			{
				flag = NativeMethods.ISteamHTTP_SetHTTPRequestUserAgentInfo(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000447C File Offset: 0x0000267C
		public static bool SetHTTPRequestRequiresVerifiedCertificate(HTTPRequestHandle hRequest, bool bRequireVerifiedCertificate)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, bRequireVerifiedCertificate);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000448F File Offset: 0x0000268F
		public static bool SetHTTPRequestAbsoluteTimeoutMS(HTTPRequestHandle hRequest, uint unMilliseconds)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, unMilliseconds);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000044A2 File Offset: 0x000026A2
		public static bool GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamHTTP_GetHTTPRequestWasTimedOut(CSteamGameServerAPIContext.GetSteamHTTP(), hRequest, out pbWasTimedOut);
		}
	}
}
