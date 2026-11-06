using System;

namespace Steamworks
{
	// Token: 0x02000142 RID: 322
	public enum EAuthSessionResponse
	{
		// Token: 0x040008F2 RID: 2290
		k_EAuthSessionResponseOK,
		// Token: 0x040008F3 RID: 2291
		k_EAuthSessionResponseUserNotConnectedToSteam,
		// Token: 0x040008F4 RID: 2292
		k_EAuthSessionResponseNoLicenseOrExpired,
		// Token: 0x040008F5 RID: 2293
		k_EAuthSessionResponseVACBanned,
		// Token: 0x040008F6 RID: 2294
		k_EAuthSessionResponseLoggedInElseWhere,
		// Token: 0x040008F7 RID: 2295
		k_EAuthSessionResponseVACCheckTimedOut,
		// Token: 0x040008F8 RID: 2296
		k_EAuthSessionResponseAuthTicketCanceled,
		// Token: 0x040008F9 RID: 2297
		k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed,
		// Token: 0x040008FA RID: 2298
		k_EAuthSessionResponseAuthTicketInvalid,
		// Token: 0x040008FB RID: 2299
		k_EAuthSessionResponsePublisherIssuedBan,
		// Token: 0x040008FC RID: 2300
		k_EAuthSessionResponseAuthTicketNetworkIdentityFailure
	}
}
