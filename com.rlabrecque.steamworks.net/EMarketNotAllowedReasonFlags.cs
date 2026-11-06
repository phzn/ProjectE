using System;

namespace Steamworks
{
	// Token: 0x0200014A RID: 330
	[Flags]
	public enum EMarketNotAllowedReasonFlags
	{
		// Token: 0x0400094D RID: 2381
		k_EMarketNotAllowedReason_None = 0,
		// Token: 0x0400094E RID: 2382
		k_EMarketNotAllowedReason_TemporaryFailure = 1,
		// Token: 0x0400094F RID: 2383
		k_EMarketNotAllowedReason_AccountDisabled = 2,
		// Token: 0x04000950 RID: 2384
		k_EMarketNotAllowedReason_AccountLockedDown = 4,
		// Token: 0x04000951 RID: 2385
		k_EMarketNotAllowedReason_AccountLimited = 8,
		// Token: 0x04000952 RID: 2386
		k_EMarketNotAllowedReason_TradeBanned = 16,
		// Token: 0x04000953 RID: 2387
		k_EMarketNotAllowedReason_AccountNotTrusted = 32,
		// Token: 0x04000954 RID: 2388
		k_EMarketNotAllowedReason_SteamGuardNotEnabled = 64,
		// Token: 0x04000955 RID: 2389
		k_EMarketNotAllowedReason_SteamGuardOnlyRecentlyEnabled = 128,
		// Token: 0x04000956 RID: 2390
		k_EMarketNotAllowedReason_RecentPasswordReset = 256,
		// Token: 0x04000957 RID: 2391
		k_EMarketNotAllowedReason_NewPaymentMethod = 512,
		// Token: 0x04000958 RID: 2392
		k_EMarketNotAllowedReason_InvalidCookie = 1024,
		// Token: 0x04000959 RID: 2393
		k_EMarketNotAllowedReason_UsingNewDevice = 2048,
		// Token: 0x0400095A RID: 2394
		k_EMarketNotAllowedReason_RecentSelfRefund = 4096,
		// Token: 0x0400095B RID: 2395
		k_EMarketNotAllowedReason_NewPaymentMethodCannotBeVerified = 8192,
		// Token: 0x0400095C RID: 2396
		k_EMarketNotAllowedReason_NoRecentPurchases = 16384,
		// Token: 0x0400095D RID: 2397
		k_EMarketNotAllowedReason_AcceptedWalletGift = 32768,
		// Token: 0x0400095E RID: 2398
		k_EMarketNotAllowedReason_TradeCooldown = 65536
	}
}
