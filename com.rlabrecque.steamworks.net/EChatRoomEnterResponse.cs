using System;

namespace Steamworks
{
	// Token: 0x02000146 RID: 326
	public enum EChatRoomEnterResponse
	{
		// Token: 0x0400091C RID: 2332
		k_EChatRoomEnterResponseSuccess = 1,
		// Token: 0x0400091D RID: 2333
		k_EChatRoomEnterResponseDoesntExist,
		// Token: 0x0400091E RID: 2334
		k_EChatRoomEnterResponseNotAllowed,
		// Token: 0x0400091F RID: 2335
		k_EChatRoomEnterResponseFull,
		// Token: 0x04000920 RID: 2336
		k_EChatRoomEnterResponseError,
		// Token: 0x04000921 RID: 2337
		k_EChatRoomEnterResponseBanned,
		// Token: 0x04000922 RID: 2338
		k_EChatRoomEnterResponseLimited,
		// Token: 0x04000923 RID: 2339
		k_EChatRoomEnterResponseClanDisabled,
		// Token: 0x04000924 RID: 2340
		k_EChatRoomEnterResponseCommunityBan,
		// Token: 0x04000925 RID: 2341
		k_EChatRoomEnterResponseMemberBlockedYou,
		// Token: 0x04000926 RID: 2342
		k_EChatRoomEnterResponseYouBlockedMember,
		// Token: 0x04000927 RID: 2343
		k_EChatRoomEnterResponseRatelimitExceeded = 15
	}
}
