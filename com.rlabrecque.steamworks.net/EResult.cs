using System;

namespace Steamworks
{
	// Token: 0x0200013E RID: 318
	public enum EResult
	{
		// Token: 0x0400084B RID: 2123
		k_EResultNone,
		// Token: 0x0400084C RID: 2124
		k_EResultOK,
		// Token: 0x0400084D RID: 2125
		k_EResultFail,
		// Token: 0x0400084E RID: 2126
		k_EResultNoConnection,
		// Token: 0x0400084F RID: 2127
		k_EResultInvalidPassword = 5,
		// Token: 0x04000850 RID: 2128
		k_EResultLoggedInElsewhere,
		// Token: 0x04000851 RID: 2129
		k_EResultInvalidProtocolVer,
		// Token: 0x04000852 RID: 2130
		k_EResultInvalidParam,
		// Token: 0x04000853 RID: 2131
		k_EResultFileNotFound,
		// Token: 0x04000854 RID: 2132
		k_EResultBusy,
		// Token: 0x04000855 RID: 2133
		k_EResultInvalidState,
		// Token: 0x04000856 RID: 2134
		k_EResultInvalidName,
		// Token: 0x04000857 RID: 2135
		k_EResultInvalidEmail,
		// Token: 0x04000858 RID: 2136
		k_EResultDuplicateName,
		// Token: 0x04000859 RID: 2137
		k_EResultAccessDenied,
		// Token: 0x0400085A RID: 2138
		k_EResultTimeout,
		// Token: 0x0400085B RID: 2139
		k_EResultBanned,
		// Token: 0x0400085C RID: 2140
		k_EResultAccountNotFound,
		// Token: 0x0400085D RID: 2141
		k_EResultInvalidSteamID,
		// Token: 0x0400085E RID: 2142
		k_EResultServiceUnavailable,
		// Token: 0x0400085F RID: 2143
		k_EResultNotLoggedOn,
		// Token: 0x04000860 RID: 2144
		k_EResultPending,
		// Token: 0x04000861 RID: 2145
		k_EResultEncryptionFailure,
		// Token: 0x04000862 RID: 2146
		k_EResultInsufficientPrivilege,
		// Token: 0x04000863 RID: 2147
		k_EResultLimitExceeded,
		// Token: 0x04000864 RID: 2148
		k_EResultRevoked,
		// Token: 0x04000865 RID: 2149
		k_EResultExpired,
		// Token: 0x04000866 RID: 2150
		k_EResultAlreadyRedeemed,
		// Token: 0x04000867 RID: 2151
		k_EResultDuplicateRequest,
		// Token: 0x04000868 RID: 2152
		k_EResultAlreadyOwned,
		// Token: 0x04000869 RID: 2153
		k_EResultIPNotFound,
		// Token: 0x0400086A RID: 2154
		k_EResultPersistFailed,
		// Token: 0x0400086B RID: 2155
		k_EResultLockingFailed,
		// Token: 0x0400086C RID: 2156
		k_EResultLogonSessionReplaced,
		// Token: 0x0400086D RID: 2157
		k_EResultConnectFailed,
		// Token: 0x0400086E RID: 2158
		k_EResultHandshakeFailed,
		// Token: 0x0400086F RID: 2159
		k_EResultIOFailure,
		// Token: 0x04000870 RID: 2160
		k_EResultRemoteDisconnect,
		// Token: 0x04000871 RID: 2161
		k_EResultShoppingCartNotFound,
		// Token: 0x04000872 RID: 2162
		k_EResultBlocked,
		// Token: 0x04000873 RID: 2163
		k_EResultIgnored,
		// Token: 0x04000874 RID: 2164
		k_EResultNoMatch,
		// Token: 0x04000875 RID: 2165
		k_EResultAccountDisabled,
		// Token: 0x04000876 RID: 2166
		k_EResultServiceReadOnly,
		// Token: 0x04000877 RID: 2167
		k_EResultAccountNotFeatured,
		// Token: 0x04000878 RID: 2168
		k_EResultAdministratorOK,
		// Token: 0x04000879 RID: 2169
		k_EResultContentVersion,
		// Token: 0x0400087A RID: 2170
		k_EResultTryAnotherCM,
		// Token: 0x0400087B RID: 2171
		k_EResultPasswordRequiredToKickSession,
		// Token: 0x0400087C RID: 2172
		k_EResultAlreadyLoggedInElsewhere,
		// Token: 0x0400087D RID: 2173
		k_EResultSuspended,
		// Token: 0x0400087E RID: 2174
		k_EResultCancelled,
		// Token: 0x0400087F RID: 2175
		k_EResultDataCorruption,
		// Token: 0x04000880 RID: 2176
		k_EResultDiskFull,
		// Token: 0x04000881 RID: 2177
		k_EResultRemoteCallFailed,
		// Token: 0x04000882 RID: 2178
		k_EResultPasswordUnset,
		// Token: 0x04000883 RID: 2179
		k_EResultExternalAccountUnlinked,
		// Token: 0x04000884 RID: 2180
		k_EResultPSNTicketInvalid,
		// Token: 0x04000885 RID: 2181
		k_EResultExternalAccountAlreadyLinked,
		// Token: 0x04000886 RID: 2182
		k_EResultRemoteFileConflict,
		// Token: 0x04000887 RID: 2183
		k_EResultIllegalPassword,
		// Token: 0x04000888 RID: 2184
		k_EResultSameAsPreviousValue,
		// Token: 0x04000889 RID: 2185
		k_EResultAccountLogonDenied,
		// Token: 0x0400088A RID: 2186
		k_EResultCannotUseOldPassword,
		// Token: 0x0400088B RID: 2187
		k_EResultInvalidLoginAuthCode,
		// Token: 0x0400088C RID: 2188
		k_EResultAccountLogonDeniedNoMail,
		// Token: 0x0400088D RID: 2189
		k_EResultHardwareNotCapableOfIPT,
		// Token: 0x0400088E RID: 2190
		k_EResultIPTInitError,
		// Token: 0x0400088F RID: 2191
		k_EResultParentalControlRestricted,
		// Token: 0x04000890 RID: 2192
		k_EResultFacebookQueryError,
		// Token: 0x04000891 RID: 2193
		k_EResultExpiredLoginAuthCode,
		// Token: 0x04000892 RID: 2194
		k_EResultIPLoginRestrictionFailed,
		// Token: 0x04000893 RID: 2195
		k_EResultAccountLockedDown,
		// Token: 0x04000894 RID: 2196
		k_EResultAccountLogonDeniedVerifiedEmailRequired,
		// Token: 0x04000895 RID: 2197
		k_EResultNoMatchingURL,
		// Token: 0x04000896 RID: 2198
		k_EResultBadResponse,
		// Token: 0x04000897 RID: 2199
		k_EResultRequirePasswordReEntry,
		// Token: 0x04000898 RID: 2200
		k_EResultValueOutOfRange,
		// Token: 0x04000899 RID: 2201
		k_EResultUnexpectedError,
		// Token: 0x0400089A RID: 2202
		k_EResultDisabled,
		// Token: 0x0400089B RID: 2203
		k_EResultInvalidCEGSubmission,
		// Token: 0x0400089C RID: 2204
		k_EResultRestrictedDevice,
		// Token: 0x0400089D RID: 2205
		k_EResultRegionLocked,
		// Token: 0x0400089E RID: 2206
		k_EResultRateLimitExceeded,
		// Token: 0x0400089F RID: 2207
		k_EResultAccountLoginDeniedNeedTwoFactor,
		// Token: 0x040008A0 RID: 2208
		k_EResultItemDeleted,
		// Token: 0x040008A1 RID: 2209
		k_EResultAccountLoginDeniedThrottle,
		// Token: 0x040008A2 RID: 2210
		k_EResultTwoFactorCodeMismatch,
		// Token: 0x040008A3 RID: 2211
		k_EResultTwoFactorActivationCodeMismatch,
		// Token: 0x040008A4 RID: 2212
		k_EResultAccountAssociatedToMultiplePartners,
		// Token: 0x040008A5 RID: 2213
		k_EResultNotModified,
		// Token: 0x040008A6 RID: 2214
		k_EResultNoMobileDevice,
		// Token: 0x040008A7 RID: 2215
		k_EResultTimeNotSynced,
		// Token: 0x040008A8 RID: 2216
		k_EResultSmsCodeFailed,
		// Token: 0x040008A9 RID: 2217
		k_EResultAccountLimitExceeded,
		// Token: 0x040008AA RID: 2218
		k_EResultAccountActivityLimitExceeded,
		// Token: 0x040008AB RID: 2219
		k_EResultPhoneActivityLimitExceeded,
		// Token: 0x040008AC RID: 2220
		k_EResultRefundToWallet,
		// Token: 0x040008AD RID: 2221
		k_EResultEmailSendFailure,
		// Token: 0x040008AE RID: 2222
		k_EResultNotSettled,
		// Token: 0x040008AF RID: 2223
		k_EResultNeedCaptcha,
		// Token: 0x040008B0 RID: 2224
		k_EResultGSLTDenied,
		// Token: 0x040008B1 RID: 2225
		k_EResultGSOwnerDenied,
		// Token: 0x040008B2 RID: 2226
		k_EResultInvalidItemType,
		// Token: 0x040008B3 RID: 2227
		k_EResultIPBanned,
		// Token: 0x040008B4 RID: 2228
		k_EResultGSLTExpired,
		// Token: 0x040008B5 RID: 2229
		k_EResultInsufficientFunds,
		// Token: 0x040008B6 RID: 2230
		k_EResultTooManyPending,
		// Token: 0x040008B7 RID: 2231
		k_EResultNoSiteLicensesFound,
		// Token: 0x040008B8 RID: 2232
		k_EResultWGNetworkSendExceeded,
		// Token: 0x040008B9 RID: 2233
		k_EResultAccountNotFriends,
		// Token: 0x040008BA RID: 2234
		k_EResultLimitedUserAccount,
		// Token: 0x040008BB RID: 2235
		k_EResultCantRemoveItem,
		// Token: 0x040008BC RID: 2236
		k_EResultAccountDeleted,
		// Token: 0x040008BD RID: 2237
		k_EResultExistingUserCancelledLicense,
		// Token: 0x040008BE RID: 2238
		k_EResultCommunityCooldown,
		// Token: 0x040008BF RID: 2239
		k_EResultNoLauncherSpecified,
		// Token: 0x040008C0 RID: 2240
		k_EResultMustAgreeToSSA,
		// Token: 0x040008C1 RID: 2241
		k_EResultLauncherMigrated,
		// Token: 0x040008C2 RID: 2242
		k_EResultSteamRealmMismatch,
		// Token: 0x040008C3 RID: 2243
		k_EResultInvalidSignature,
		// Token: 0x040008C4 RID: 2244
		k_EResultParseFailure,
		// Token: 0x040008C5 RID: 2245
		k_EResultNoVerifiedPhone,
		// Token: 0x040008C6 RID: 2246
		k_EResultInsufficientBattery,
		// Token: 0x040008C7 RID: 2247
		k_EResultChargerRequired,
		// Token: 0x040008C8 RID: 2248
		k_EResultCachedCredentialInvalid,
		// Token: 0x040008C9 RID: 2249
		K_EResultPhoneNumberIsVOIP,
		// Token: 0x040008CA RID: 2250
		k_EResultNotSupported,
		// Token: 0x040008CB RID: 2251
		k_EResultFamilySizeLimitExceeded,
		// Token: 0x040008CC RID: 2252
		k_EResultOfflineAppCacheInvalid,
		// Token: 0x040008CD RID: 2253
		k_EResultTryLater
	}
}
