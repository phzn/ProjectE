using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200000F RID: 15
	public static class SteamGameServerUGC
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00005693 File Offset: 0x00003893
		public static UGCQueryHandle_t CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryUserUGCRequest(CSteamGameServerAPIContext.GetSteamUGC(), unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000056B3 File Offset: 0x000038B3
		public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryAllUGCRequestPage(CSteamGameServerAPIContext.GetSteamUGC(), eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000056D0 File Offset: 0x000038D0
		public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, string pchCursor = null)
		{
			InteropHelp.TestIfAvailableGameServer();
			UGCQueryHandle_t ugcqueryHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchCursor))
			{
				ugcqueryHandle_t = (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryAllUGCRequestCursor(CSteamGameServerAPIContext.GetSteamUGC(), eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, utf8StringHandle);
			}
			return ugcqueryHandle_t;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000571C File Offset: 0x0000391C
		public static UGCQueryHandle_t CreateQueryUGCDetailsRequest(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryUGCDetailsRequest(CSteamGameServerAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00005734 File Offset: 0x00003934
		public static SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SendQueryUGCRequest(CSteamGameServerAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000574B File Offset: 0x0000394B
		public static bool GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCResult(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, out pDetails);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000575F File Offset: 0x0000395F
		public static uint GetQueryUGCNumTags(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCNumTags(CSteamGameServerAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00005774 File Offset: 0x00003974
		public static bool GetQueryUGCTag(UGCQueryHandle_t handle, uint index, uint indexTag, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, indexTag, intPtr, cchValueSize);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000057B4 File Offset: 0x000039B4
		public static bool GetQueryUGCTagDisplayName(UGCQueryHandle_t handle, uint index, uint indexTag, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCTagDisplayName(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, indexTag, intPtr, cchValueSize);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000057F4 File Offset: 0x000039F4
		public static bool GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCPreviewURL(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, intPtr, cchURLSize);
			pchURL = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00005834 File Offset: 0x00003A34
		public static bool GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchMetadatasize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCMetadata(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, intPtr, cchMetadatasize);
			pchMetadata = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00005871 File Offset: 0x00003A71
		public static bool GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCChildren(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, pvecPublishedFileID, cMaxEntries);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00005886 File Offset: 0x00003A86
		public static bool GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCStatistic(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, eStatType, out pStatValue);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000589B File Offset: 0x00003A9B
		public static uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCNumAdditionalPreviews(CSteamGameServerAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000058B0 File Offset: 0x00003AB0
		public static bool GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out string pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchOriginalFileNameSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCAdditionalPreview(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, previewIndex, intPtr, cchURLSize, intPtr2, cchOriginalFileNameSize, out pPreviewType);
			pchURLOrVideoID = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchOriginalFileName = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00005912 File Offset: 0x00003B12
		public static uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCNumKeyValueTags(CSteamGameServerAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00005928 File Offset: 0x00003B28
		public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchKeySize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCKeyValueTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, keyValueTagIndex, intPtr, cchKeySize, intPtr2, cchValueSize);
			pchKey = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00005988 File Offset: 0x00003B88
		public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, string pchKey, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag2;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				bool flag = NativeMethods.ISteamUGC_GetQueryFirstUGCKeyValueTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, utf8StringHandle, intPtr, cchValueSize);
				pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
				Marshal.FreeHGlobal(intPtr);
				flag2 = flag;
			}
			return flag2;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000059F0 File Offset: 0x00003BF0
		public static uint GetNumSupportedGameVersions(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetNumSupportedGameVersions(CSteamGameServerAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00005A04 File Offset: 0x00003C04
		public static bool GetSupportedGameVersionData(UGCQueryHandle_t handle, uint index, uint versionIndex, out string pchGameBranchMin, out string pchGameBranchMax, uint cchGameBranchSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchGameBranchSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchGameBranchSize);
			bool flag = NativeMethods.ISteamUGC_GetSupportedGameVersionData(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, versionIndex, intPtr, intPtr2, cchGameBranchSize);
			pchGameBranchMin = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchGameBranchMax = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00005A62 File Offset: 0x00003C62
		public static uint GetQueryUGCContentDescriptors(UGCQueryHandle_t handle, uint index, EUGCContentDescriptorID[] pvecDescriptors, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableGameServer();
			if (pvecDescriptors != null && (long)pvecDescriptors.Length != (long)((ulong)cMaxEntries))
			{
				throw new ArgumentException("pvecDescriptors must be the same size as cMaxEntries!");
			}
			return NativeMethods.ISteamUGC_GetQueryUGCContentDescriptors(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, pvecDescriptors, cMaxEntries);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00005A8D File Offset: 0x00003C8D
		public static bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_ReleaseQueryUGCRequest(CSteamGameServerAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00005AA0 File Offset: 0x00003CA0
		public static bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				flag = NativeMethods.ISteamUGC_AddRequiredTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00005AE4 File Offset: 0x00003CE4
		public static bool AddRequiredTagGroup(UGCQueryHandle_t handle, IList<string> pTagGroups)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_AddRequiredTagGroup(CSteamGameServerAPIContext.GetSteamUGC(), handle, new InteropHelp.SteamParamStringArray(pTagGroups));
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00005B04 File Offset: 0x00003D04
		public static bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				flag = NativeMethods.ISteamUGC_AddExcludedTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00005B48 File Offset: 0x00003D48
		public static bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnOnlyIDs(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnOnlyIDs);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00005B5B File Offset: 0x00003D5B
		public static bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnKeyValueTags(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnKeyValueTags);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00005B6E File Offset: 0x00003D6E
		public static bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnLongDescription(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnLongDescription);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00005B81 File Offset: 0x00003D81
		public static bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnMetadata(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnMetadata);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00005B94 File Offset: 0x00003D94
		public static bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnChildren(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnChildren);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00005BA7 File Offset: 0x00003DA7
		public static bool SetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool bReturnAdditionalPreviews)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnAdditionalPreviews(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnAdditionalPreviews);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00005BBA File Offset: 0x00003DBA
		public static bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnTotalOnly(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnTotalOnly);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00005BCD File Offset: 0x00003DCD
		public static bool SetReturnPlaytimeStats(UGCQueryHandle_t handle, uint unDays)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnPlaytimeStats(CSteamGameServerAPIContext.GetSteamUGC(), handle, unDays);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00005BE0 File Offset: 0x00003DE0
		public static bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				flag = NativeMethods.ISteamUGC_SetLanguage(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00005C24 File Offset: 0x00003E24
		public static bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetAllowCachedResponse(CSteamGameServerAPIContext.GetSteamUGC(), handle, unMaxAgeSeconds);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00005C37 File Offset: 0x00003E37
		public static bool SetAdminQuery(UGCUpdateHandle_t handle, bool bAdminQuery)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetAdminQuery(CSteamGameServerAPIContext.GetSteamUGC(), handle, bAdminQuery);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00005C4C File Offset: 0x00003E4C
		public static bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pMatchCloudFileName))
			{
				flag = NativeMethods.ISteamUGC_SetCloudFileNameFilter(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00005C90 File Offset: 0x00003E90
		public static bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetMatchAnyTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, bMatchAnyTag);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public static bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pSearchText))
			{
				flag = NativeMethods.ISteamUGC_SetSearchText(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00005CE8 File Offset: 0x00003EE8
		public static bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetRankedByTrendDays(CSteamGameServerAPIContext.GetSteamUGC(), handle, unDays);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00005CFB File Offset: 0x00003EFB
		public static bool SetTimeCreatedDateRange(UGCQueryHandle_t handle, uint rtStart, uint rtEnd)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetTimeCreatedDateRange(CSteamGameServerAPIContext.GetSteamUGC(), handle, rtStart, rtEnd);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00005D0F File Offset: 0x00003F0F
		public static bool SetTimeUpdatedDateRange(UGCQueryHandle_t handle, uint rtStart, uint rtEnd)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetTimeUpdatedDateRange(CSteamGameServerAPIContext.GetSteamUGC(), handle, rtStart, rtEnd);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00005D24 File Offset: 0x00003F24
		public static bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pValue))
				{
					flag = NativeMethods.ISteamUGC_AddRequiredKeyValueTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00005D88 File Offset: 0x00003F88
		public static SteamAPICall_t RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RequestUGCDetails(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, unMaxAgeSeconds);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00005DA0 File Offset: 0x00003FA0
		public static SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_CreateItem(CSteamGameServerAPIContext.GetSteamUGC(), nConsumerAppId, eFileType);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00005DB8 File Offset: 0x00003FB8
		public static UGCUpdateHandle_t StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCUpdateHandle_t)NativeMethods.ISteamUGC_StartItemUpdate(CSteamGameServerAPIContext.GetSteamUGC(), nConsumerAppId, nPublishedFileID);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00005DD0 File Offset: 0x00003FD0
		public static bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				flag = NativeMethods.ISteamUGC_SetItemTitle(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00005E14 File Offset: 0x00004014
		public static bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				flag = NativeMethods.ISteamUGC_SetItemDescription(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00005E58 File Offset: 0x00004058
		public static bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				flag = NativeMethods.ISteamUGC_SetItemUpdateLanguage(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00005E9C File Offset: 0x0000409C
		public static bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchMetaData))
			{
				flag = NativeMethods.ISteamUGC_SetItemMetadata(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00005EE0 File Offset: 0x000040E0
		public static bool SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetItemVisibility(CSteamGameServerAPIContext.GetSteamUGC(), handle, eVisibility);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00005EF3 File Offset: 0x000040F3
		public static bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags, bool bAllowAdminTags = false)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetItemTags(CSteamGameServerAPIContext.GetSteamUGC(), updateHandle, new InteropHelp.SteamParamStringArray(pTags), bAllowAdminTags);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00005F14 File Offset: 0x00004114
		public static bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszContentFolder))
			{
				flag = NativeMethods.ISteamUGC_SetItemContent(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00005F58 File Offset: 0x00004158
		public static bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamUGC_SetItemPreview(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00005F9C File Offset: 0x0000419C
		public static bool SetAllowLegacyUpload(UGCUpdateHandle_t handle, bool bAllowLegacyUpload)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetAllowLegacyUpload(CSteamGameServerAPIContext.GetSteamUGC(), handle, bAllowLegacyUpload);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00005FAF File Offset: 0x000041AF
		public static bool RemoveAllItemKeyValueTags(UGCUpdateHandle_t handle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_RemoveAllItemKeyValueTags(CSteamGameServerAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00005FC4 File Offset: 0x000041C4
		public static bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				flag = NativeMethods.ISteamUGC_RemoveItemKeyValueTags(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00006008 File Offset: 0x00004208
		public static bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					flag = NativeMethods.ISteamUGC_AddItemKeyValueTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000606C File Offset: 0x0000426C
		public static bool AddItemPreviewFile(UGCUpdateHandle_t handle, string pszPreviewFile, EItemPreviewType type)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamUGC_AddItemPreviewFile(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle, type);
			}
			return flag;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000060B0 File Offset: 0x000042B0
		public static bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				flag = NativeMethods.ISteamUGC_AddItemPreviewVideo(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000060F4 File Offset: 0x000042F4
		public static bool UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamUGC_UpdateItemPreviewFile(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00006138 File Offset: 0x00004338
		public static bool UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string pszVideoID)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				flag = NativeMethods.ISteamUGC_UpdateItemPreviewVideo(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000617C File Offset: 0x0000437C
		public static bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_RemoveItemPreview(CSteamGameServerAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000618F File Offset: 0x0000438F
		public static bool AddContentDescriptor(UGCUpdateHandle_t handle, EUGCContentDescriptorID descid)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_AddContentDescriptor(CSteamGameServerAPIContext.GetSteamUGC(), handle, descid);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000061A2 File Offset: 0x000043A2
		public static bool RemoveContentDescriptor(UGCUpdateHandle_t handle, EUGCContentDescriptorID descid)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_RemoveContentDescriptor(CSteamGameServerAPIContext.GetSteamUGC(), handle, descid);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000061B8 File Offset: 0x000043B8
		public static bool SetRequiredGameVersions(UGCUpdateHandle_t handle, string pszGameBranchMin, string pszGameBranchMax)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszGameBranchMin))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pszGameBranchMax))
				{
					flag = NativeMethods.ISteamUGC_SetRequiredGameVersions(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000621C File Offset: 0x0000441C
		public static SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
		{
			InteropHelp.TestIfAvailableGameServer();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchChangeNote))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamUGC_SubmitItemUpdate(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00006264 File Offset: 0x00004464
		public static EItemUpdateStatus GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetItemUpdateProgress(CSteamGameServerAPIContext.GetSteamUGC(), handle, out punBytesProcessed, out punBytesTotal);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00006278 File Offset: 0x00004478
		public static SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SetUserItemVote(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, bVoteUp);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00006290 File Offset: 0x00004490
		public static SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetUserItemVote(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000062A7 File Offset: 0x000044A7
		public static SteamAPICall_t AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddItemToFavorites(CSteamGameServerAPIContext.GetSteamUGC(), nAppId, nPublishedFileID);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000062BF File Offset: 0x000044BF
		public static SteamAPICall_t RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveItemFromFavorites(CSteamGameServerAPIContext.GetSteamUGC(), nAppId, nPublishedFileID);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000062D7 File Offset: 0x000044D7
		public static SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SubscribeItem(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000062EE File Offset: 0x000044EE
		public static SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_UnsubscribeItem(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00006305 File Offset: 0x00004505
		public static uint GetNumSubscribedItems(bool bIncludeLocallyDisabled = false)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetNumSubscribedItems(CSteamGameServerAPIContext.GetSteamUGC(), bIncludeLocallyDisabled);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00006317 File Offset: 0x00004517
		public static uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries, bool bIncludeLocallyDisabled = false)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetSubscribedItems(CSteamGameServerAPIContext.GetSteamUGC(), pvecPublishedFileID, cMaxEntries, bIncludeLocallyDisabled);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000632B File Offset: 0x0000452B
		public static uint GetItemState(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetItemState(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00006340 File Offset: 0x00004540
		public static bool GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchFolderSize);
			bool flag = NativeMethods.ISteamUGC_GetItemInstallInfo(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, out punSizeOnDisk, intPtr, cchFolderSize, out punTimeStamp);
			pchFolder = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000637F File Offset: 0x0000457F
		public static bool GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetItemDownloadInfo(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00006393 File Offset: 0x00004593
		public static bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_DownloadItem(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, bHighPriority);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000063A8 File Offset: 0x000045A8
		public static bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszFolder))
			{
				flag = NativeMethods.ISteamUGC_BInitWorkshopForGameServer(CSteamGameServerAPIContext.GetSteamUGC(), unWorkshopDepotID, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000063EC File Offset: 0x000045EC
		public static void SuspendDownloads(bool bSuspend)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamUGC_SuspendDownloads(CSteamGameServerAPIContext.GetSteamUGC(), bSuspend);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000063FE File Offset: 0x000045FE
		public static SteamAPICall_t StartPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StartPlaytimeTracking(CSteamGameServerAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00006416 File Offset: 0x00004616
		public static SteamAPICall_t StopPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StopPlaytimeTracking(CSteamGameServerAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000642E File Offset: 0x0000462E
		public static SteamAPICall_t StopPlaytimeTrackingForAllItems()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StopPlaytimeTrackingForAllItems(CSteamGameServerAPIContext.GetSteamUGC());
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00006444 File Offset: 0x00004644
		public static SteamAPICall_t AddDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddDependency(CSteamGameServerAPIContext.GetSteamUGC(), nParentPublishedFileID, nChildPublishedFileID);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000645C File Offset: 0x0000465C
		public static SteamAPICall_t RemoveDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveDependency(CSteamGameServerAPIContext.GetSteamUGC(), nParentPublishedFileID, nChildPublishedFileID);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00006474 File Offset: 0x00004674
		public static SteamAPICall_t AddAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddAppDependency(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, nAppID);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000648C File Offset: 0x0000468C
		public static SteamAPICall_t RemoveAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveAppDependency(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, nAppID);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000064A4 File Offset: 0x000046A4
		public static SteamAPICall_t GetAppDependencies(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetAppDependencies(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000064BB File Offset: 0x000046BB
		public static SteamAPICall_t DeleteItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_DeleteItem(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000064D2 File Offset: 0x000046D2
		public static bool ShowWorkshopEULA()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_ShowWorkshopEULA(CSteamGameServerAPIContext.GetSteamUGC());
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000064E3 File Offset: 0x000046E3
		public static SteamAPICall_t GetWorkshopEULAStatus()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetWorkshopEULAStatus(CSteamGameServerAPIContext.GetSteamUGC());
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000064F9 File Offset: 0x000046F9
		public static uint GetUserContentDescriptorPreferences(EUGCContentDescriptorID[] pvecDescriptors, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetUserContentDescriptorPreferences(CSteamGameServerAPIContext.GetSteamUGC(), pvecDescriptors, cMaxEntries);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000650C File Offset: 0x0000470C
		public static bool SetItemsDisabledLocally(PublishedFileId_t[] pvecPublishedFileIDs, uint unNumPublishedFileIDs, bool bDisabledLocally)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetItemsDisabledLocally(CSteamGameServerAPIContext.GetSteamUGC(), pvecPublishedFileIDs, unNumPublishedFileIDs, bDisabledLocally);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00006520 File Offset: 0x00004720
		public static bool SetSubscriptionsLoadOrder(PublishedFileId_t[] pvecPublishedFileIDs, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetSubscriptionsLoadOrder(CSteamGameServerAPIContext.GetSteamUGC(), pvecPublishedFileIDs, unNumPublishedFileIDs);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00006533 File Offset: 0x00004733
		public static bool MarkDownloadedItemAsUnused(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_MarkDownloadedItemAsUnused(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00006545 File Offset: 0x00004745
		public static uint GetNumDownloadedItems()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetNumDownloadedItems(CSteamGameServerAPIContext.GetSteamUGC());
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00006556 File Offset: 0x00004756
		public static uint GetDownloadedItems(out PublishedFileId_t pvecPublishedFileIDs, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetDownloadedItems(CSteamGameServerAPIContext.GetSteamUGC(), out pvecPublishedFileIDs, cMaxEntries);
		}
	}
}
