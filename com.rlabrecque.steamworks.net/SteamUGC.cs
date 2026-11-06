using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000022 RID: 34
	public static class SteamUGC
	{
		// Token: 0x060003CA RID: 970 RVA: 0x0000A1FE File Offset: 0x000083FE
		public static UGCQueryHandle_t CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryUserUGCRequest(CSteamAPIContext.GetSteamUGC(), unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000A21E File Offset: 0x0000841E
		public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryAllUGCRequestPage(CSteamAPIContext.GetSteamUGC(), eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000A23C File Offset: 0x0000843C
		public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, string pchCursor = null)
		{
			InteropHelp.TestIfAvailableClient();
			UGCQueryHandle_t ugcqueryHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchCursor))
			{
				ugcqueryHandle_t = (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryAllUGCRequestCursor(CSteamAPIContext.GetSteamUGC(), eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, utf8StringHandle);
			}
			return ugcqueryHandle_t;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000A288 File Offset: 0x00008488
		public static UGCQueryHandle_t CreateQueryUGCDetailsRequest(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryUGCDetailsRequest(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000A2A0 File Offset: 0x000084A0
		public static SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SendQueryUGCRequest(CSteamAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000A2B7 File Offset: 0x000084B7
		public static bool GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCResult(CSteamAPIContext.GetSteamUGC(), handle, index, out pDetails);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000A2CB File Offset: 0x000084CB
		public static uint GetQueryUGCNumTags(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCNumTags(CSteamAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000A2E0 File Offset: 0x000084E0
		public static bool GetQueryUGCTag(UGCQueryHandle_t handle, uint index, uint indexTag, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCTag(CSteamAPIContext.GetSteamUGC(), handle, index, indexTag, intPtr, cchValueSize);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000A320 File Offset: 0x00008520
		public static bool GetQueryUGCTagDisplayName(UGCQueryHandle_t handle, uint index, uint indexTag, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCTagDisplayName(CSteamAPIContext.GetSteamUGC(), handle, index, indexTag, intPtr, cchValueSize);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000A360 File Offset: 0x00008560
		public static bool GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCPreviewURL(CSteamAPIContext.GetSteamUGC(), handle, index, intPtr, cchURLSize);
			pchURL = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000A3A0 File Offset: 0x000085A0
		public static bool GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchMetadatasize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCMetadata(CSteamAPIContext.GetSteamUGC(), handle, index, intPtr, cchMetadatasize);
			pchMetadata = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000A3DD File Offset: 0x000085DD
		public static bool GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCChildren(CSteamAPIContext.GetSteamUGC(), handle, index, pvecPublishedFileID, cMaxEntries);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000A3F2 File Offset: 0x000085F2
		public static bool GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCStatistic(CSteamAPIContext.GetSteamUGC(), handle, index, eStatType, out pStatValue);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000A407 File Offset: 0x00008607
		public static uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCNumAdditionalPreviews(CSteamAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000A41C File Offset: 0x0000861C
		public static bool GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out string pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchOriginalFileNameSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCAdditionalPreview(CSteamAPIContext.GetSteamUGC(), handle, index, previewIndex, intPtr, cchURLSize, intPtr2, cchOriginalFileNameSize, out pPreviewType);
			pchURLOrVideoID = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchOriginalFileName = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000A47E File Offset: 0x0000867E
		public static uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCNumKeyValueTags(CSteamAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000A494 File Offset: 0x00008694
		public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchKeySize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCKeyValueTag(CSteamAPIContext.GetSteamUGC(), handle, index, keyValueTagIndex, intPtr, cchKeySize, intPtr2, cchValueSize);
			pchKey = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000A4F4 File Offset: 0x000086F4
		public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, string pchKey, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag2;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				bool flag = NativeMethods.ISteamUGC_GetQueryFirstUGCKeyValueTag(CSteamAPIContext.GetSteamUGC(), handle, index, utf8StringHandle, intPtr, cchValueSize);
				pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
				Marshal.FreeHGlobal(intPtr);
				flag2 = flag;
			}
			return flag2;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000A55C File Offset: 0x0000875C
		public static uint GetNumSupportedGameVersions(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetNumSupportedGameVersions(CSteamAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000A570 File Offset: 0x00008770
		public static bool GetSupportedGameVersionData(UGCQueryHandle_t handle, uint index, uint versionIndex, out string pchGameBranchMin, out string pchGameBranchMax, uint cchGameBranchSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchGameBranchSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchGameBranchSize);
			bool flag = NativeMethods.ISteamUGC_GetSupportedGameVersionData(CSteamAPIContext.GetSteamUGC(), handle, index, versionIndex, intPtr, intPtr2, cchGameBranchSize);
			pchGameBranchMin = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchGameBranchMax = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000A5CE File Offset: 0x000087CE
		public static uint GetQueryUGCContentDescriptors(UGCQueryHandle_t handle, uint index, EUGCContentDescriptorID[] pvecDescriptors, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableClient();
			if (pvecDescriptors != null && (long)pvecDescriptors.Length != (long)((ulong)cMaxEntries))
			{
				throw new ArgumentException("pvecDescriptors must be the same size as cMaxEntries!");
			}
			return NativeMethods.ISteamUGC_GetQueryUGCContentDescriptors(CSteamAPIContext.GetSteamUGC(), handle, index, pvecDescriptors, cMaxEntries);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000A5F9 File Offset: 0x000087F9
		public static bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_ReleaseQueryUGCRequest(CSteamAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000A60C File Offset: 0x0000880C
		public static bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				flag = NativeMethods.ISteamUGC_AddRequiredTag(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000A650 File Offset: 0x00008850
		public static bool AddRequiredTagGroup(UGCQueryHandle_t handle, IList<string> pTagGroups)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_AddRequiredTagGroup(CSteamAPIContext.GetSteamUGC(), handle, new InteropHelp.SteamParamStringArray(pTagGroups));
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000A670 File Offset: 0x00008870
		public static bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				flag = NativeMethods.ISteamUGC_AddExcludedTag(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000A6B4 File Offset: 0x000088B4
		public static bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnOnlyIDs(CSteamAPIContext.GetSteamUGC(), handle, bReturnOnlyIDs);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000A6C7 File Offset: 0x000088C7
		public static bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnKeyValueTags(CSteamAPIContext.GetSteamUGC(), handle, bReturnKeyValueTags);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000A6DA File Offset: 0x000088DA
		public static bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnLongDescription(CSteamAPIContext.GetSteamUGC(), handle, bReturnLongDescription);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000A6ED File Offset: 0x000088ED
		public static bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnMetadata(CSteamAPIContext.GetSteamUGC(), handle, bReturnMetadata);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000A700 File Offset: 0x00008900
		public static bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnChildren(CSteamAPIContext.GetSteamUGC(), handle, bReturnChildren);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000A713 File Offset: 0x00008913
		public static bool SetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool bReturnAdditionalPreviews)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnAdditionalPreviews(CSteamAPIContext.GetSteamUGC(), handle, bReturnAdditionalPreviews);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000A726 File Offset: 0x00008926
		public static bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnTotalOnly(CSteamAPIContext.GetSteamUGC(), handle, bReturnTotalOnly);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000A739 File Offset: 0x00008939
		public static bool SetReturnPlaytimeStats(UGCQueryHandle_t handle, uint unDays)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnPlaytimeStats(CSteamAPIContext.GetSteamUGC(), handle, unDays);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000A74C File Offset: 0x0000894C
		public static bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				flag = NativeMethods.ISteamUGC_SetLanguage(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000A790 File Offset: 0x00008990
		public static bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetAllowCachedResponse(CSteamAPIContext.GetSteamUGC(), handle, unMaxAgeSeconds);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000A7A3 File Offset: 0x000089A3
		public static bool SetAdminQuery(UGCUpdateHandle_t handle, bool bAdminQuery)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetAdminQuery(CSteamAPIContext.GetSteamUGC(), handle, bAdminQuery);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000A7B8 File Offset: 0x000089B8
		public static bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pMatchCloudFileName))
			{
				flag = NativeMethods.ISteamUGC_SetCloudFileNameFilter(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000A7FC File Offset: 0x000089FC
		public static bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetMatchAnyTag(CSteamAPIContext.GetSteamUGC(), handle, bMatchAnyTag);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000A810 File Offset: 0x00008A10
		public static bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pSearchText))
			{
				flag = NativeMethods.ISteamUGC_SetSearchText(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000A854 File Offset: 0x00008A54
		public static bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetRankedByTrendDays(CSteamAPIContext.GetSteamUGC(), handle, unDays);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000A867 File Offset: 0x00008A67
		public static bool SetTimeCreatedDateRange(UGCQueryHandle_t handle, uint rtStart, uint rtEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetTimeCreatedDateRange(CSteamAPIContext.GetSteamUGC(), handle, rtStart, rtEnd);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000A87B File Offset: 0x00008A7B
		public static bool SetTimeUpdatedDateRange(UGCQueryHandle_t handle, uint rtStart, uint rtEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetTimeUpdatedDateRange(CSteamAPIContext.GetSteamUGC(), handle, rtStart, rtEnd);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000A890 File Offset: 0x00008A90
		public static bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pValue))
				{
					flag = NativeMethods.ISteamUGC_AddRequiredKeyValueTag(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000A8F4 File Offset: 0x00008AF4
		public static SteamAPICall_t RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RequestUGCDetails(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, unMaxAgeSeconds);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000A90C File Offset: 0x00008B0C
		public static SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_CreateItem(CSteamAPIContext.GetSteamUGC(), nConsumerAppId, eFileType);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000A924 File Offset: 0x00008B24
		public static UGCUpdateHandle_t StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCUpdateHandle_t)NativeMethods.ISteamUGC_StartItemUpdate(CSteamAPIContext.GetSteamUGC(), nConsumerAppId, nPublishedFileID);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000A93C File Offset: 0x00008B3C
		public static bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				flag = NativeMethods.ISteamUGC_SetItemTitle(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000A980 File Offset: 0x00008B80
		public static bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				flag = NativeMethods.ISteamUGC_SetItemDescription(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000A9C4 File Offset: 0x00008BC4
		public static bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				flag = NativeMethods.ISteamUGC_SetItemUpdateLanguage(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000AA08 File Offset: 0x00008C08
		public static bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchMetaData))
			{
				flag = NativeMethods.ISteamUGC_SetItemMetadata(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000AA4C File Offset: 0x00008C4C
		public static bool SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetItemVisibility(CSteamAPIContext.GetSteamUGC(), handle, eVisibility);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000AA5F File Offset: 0x00008C5F
		public static bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags, bool bAllowAdminTags = false)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetItemTags(CSteamAPIContext.GetSteamUGC(), updateHandle, new InteropHelp.SteamParamStringArray(pTags), bAllowAdminTags);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000AA80 File Offset: 0x00008C80
		public static bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszContentFolder))
			{
				flag = NativeMethods.ISteamUGC_SetItemContent(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000AAC4 File Offset: 0x00008CC4
		public static bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamUGC_SetItemPreview(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000AB08 File Offset: 0x00008D08
		public static bool SetAllowLegacyUpload(UGCUpdateHandle_t handle, bool bAllowLegacyUpload)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetAllowLegacyUpload(CSteamAPIContext.GetSteamUGC(), handle, bAllowLegacyUpload);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000AB1B File Offset: 0x00008D1B
		public static bool RemoveAllItemKeyValueTags(UGCUpdateHandle_t handle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_RemoveAllItemKeyValueTags(CSteamAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000AB30 File Offset: 0x00008D30
		public static bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				flag = NativeMethods.ISteamUGC_RemoveItemKeyValueTags(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000AB74 File Offset: 0x00008D74
		public static bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					flag = NativeMethods.ISteamUGC_AddItemKeyValueTag(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000ABD8 File Offset: 0x00008DD8
		public static bool AddItemPreviewFile(UGCUpdateHandle_t handle, string pszPreviewFile, EItemPreviewType type)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamUGC_AddItemPreviewFile(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle, type);
			}
			return flag;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000AC1C File Offset: 0x00008E1C
		public static bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				flag = NativeMethods.ISteamUGC_AddItemPreviewVideo(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000AC60 File Offset: 0x00008E60
		public static bool UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamUGC_UpdateItemPreviewFile(CSteamAPIContext.GetSteamUGC(), handle, index, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000ACA4 File Offset: 0x00008EA4
		public static bool UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string pszVideoID)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				flag = NativeMethods.ISteamUGC_UpdateItemPreviewVideo(CSteamAPIContext.GetSteamUGC(), handle, index, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000ACE8 File Offset: 0x00008EE8
		public static bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_RemoveItemPreview(CSteamAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000ACFB File Offset: 0x00008EFB
		public static bool AddContentDescriptor(UGCUpdateHandle_t handle, EUGCContentDescriptorID descid)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_AddContentDescriptor(CSteamAPIContext.GetSteamUGC(), handle, descid);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000AD0E File Offset: 0x00008F0E
		public static bool RemoveContentDescriptor(UGCUpdateHandle_t handle, EUGCContentDescriptorID descid)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_RemoveContentDescriptor(CSteamAPIContext.GetSteamUGC(), handle, descid);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000AD24 File Offset: 0x00008F24
		public static bool SetRequiredGameVersions(UGCUpdateHandle_t handle, string pszGameBranchMin, string pszGameBranchMax)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszGameBranchMin))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pszGameBranchMax))
				{
					flag = NativeMethods.ISteamUGC_SetRequiredGameVersions(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000AD88 File Offset: 0x00008F88
		public static SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchChangeNote))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamUGC_SubmitItemUpdate(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000ADD0 File Offset: 0x00008FD0
		public static EItemUpdateStatus GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetItemUpdateProgress(CSteamAPIContext.GetSteamUGC(), handle, out punBytesProcessed, out punBytesTotal);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000ADE4 File Offset: 0x00008FE4
		public static SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SetUserItemVote(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, bVoteUp);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000ADFC File Offset: 0x00008FFC
		public static SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetUserItemVote(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000AE13 File Offset: 0x00009013
		public static SteamAPICall_t AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddItemToFavorites(CSteamAPIContext.GetSteamUGC(), nAppId, nPublishedFileID);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000AE2B File Offset: 0x0000902B
		public static SteamAPICall_t RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveItemFromFavorites(CSteamAPIContext.GetSteamUGC(), nAppId, nPublishedFileID);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000AE43 File Offset: 0x00009043
		public static SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SubscribeItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000AE5A File Offset: 0x0000905A
		public static SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_UnsubscribeItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000AE71 File Offset: 0x00009071
		public static uint GetNumSubscribedItems(bool bIncludeLocallyDisabled = false)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetNumSubscribedItems(CSteamAPIContext.GetSteamUGC(), bIncludeLocallyDisabled);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000AE83 File Offset: 0x00009083
		public static uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries, bool bIncludeLocallyDisabled = false)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetSubscribedItems(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, cMaxEntries, bIncludeLocallyDisabled);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000AE97 File Offset: 0x00009097
		public static uint GetItemState(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetItemState(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000AEAC File Offset: 0x000090AC
		public static bool GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchFolderSize);
			bool flag = NativeMethods.ISteamUGC_GetItemInstallInfo(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, out punSizeOnDisk, intPtr, cchFolderSize, out punTimeStamp);
			pchFolder = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000AEEB File Offset: 0x000090EB
		public static bool GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetItemDownloadInfo(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000AEFF File Offset: 0x000090FF
		public static bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_DownloadItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, bHighPriority);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000AF14 File Offset: 0x00009114
		public static bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszFolder))
			{
				flag = NativeMethods.ISteamUGC_BInitWorkshopForGameServer(CSteamAPIContext.GetSteamUGC(), unWorkshopDepotID, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000AF58 File Offset: 0x00009158
		public static void SuspendDownloads(bool bSuspend)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUGC_SuspendDownloads(CSteamAPIContext.GetSteamUGC(), bSuspend);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000AF6A File Offset: 0x0000916A
		public static SteamAPICall_t StartPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StartPlaytimeTracking(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000AF82 File Offset: 0x00009182
		public static SteamAPICall_t StopPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StopPlaytimeTracking(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000AF9A File Offset: 0x0000919A
		public static SteamAPICall_t StopPlaytimeTrackingForAllItems()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StopPlaytimeTrackingForAllItems(CSteamAPIContext.GetSteamUGC());
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000AFB0 File Offset: 0x000091B0
		public static SteamAPICall_t AddDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddDependency(CSteamAPIContext.GetSteamUGC(), nParentPublishedFileID, nChildPublishedFileID);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000AFC8 File Offset: 0x000091C8
		public static SteamAPICall_t RemoveDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveDependency(CSteamAPIContext.GetSteamUGC(), nParentPublishedFileID, nChildPublishedFileID);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000AFE0 File Offset: 0x000091E0
		public static SteamAPICall_t AddAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddAppDependency(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, nAppID);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000AFF8 File Offset: 0x000091F8
		public static SteamAPICall_t RemoveAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveAppDependency(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, nAppID);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000B010 File Offset: 0x00009210
		public static SteamAPICall_t GetAppDependencies(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetAppDependencies(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000B027 File Offset: 0x00009227
		public static SteamAPICall_t DeleteItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_DeleteItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000B03E File Offset: 0x0000923E
		public static bool ShowWorkshopEULA()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_ShowWorkshopEULA(CSteamAPIContext.GetSteamUGC());
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000B04F File Offset: 0x0000924F
		public static SteamAPICall_t GetWorkshopEULAStatus()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetWorkshopEULAStatus(CSteamAPIContext.GetSteamUGC());
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000B065 File Offset: 0x00009265
		public static uint GetUserContentDescriptorPreferences(EUGCContentDescriptorID[] pvecDescriptors, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetUserContentDescriptorPreferences(CSteamAPIContext.GetSteamUGC(), pvecDescriptors, cMaxEntries);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000B078 File Offset: 0x00009278
		public static bool SetItemsDisabledLocally(PublishedFileId_t[] pvecPublishedFileIDs, uint unNumPublishedFileIDs, bool bDisabledLocally)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetItemsDisabledLocally(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileIDs, unNumPublishedFileIDs, bDisabledLocally);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000B08C File Offset: 0x0000928C
		public static bool SetSubscriptionsLoadOrder(PublishedFileId_t[] pvecPublishedFileIDs, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetSubscriptionsLoadOrder(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileIDs, unNumPublishedFileIDs);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000B09F File Offset: 0x0000929F
		public static bool MarkDownloadedItemAsUnused(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_MarkDownloadedItemAsUnused(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000B0B1 File Offset: 0x000092B1
		public static uint GetNumDownloadedItems()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetNumDownloadedItems(CSteamAPIContext.GetSteamUGC());
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000B0C2 File Offset: 0x000092C2
		public static uint GetDownloadedItems(out PublishedFileId_t pvecPublishedFileIDs, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetDownloadedItems(CSteamAPIContext.GetSteamUGC(), out pvecPublishedFileIDs, cMaxEntries);
		}
	}
}
