using System;

namespace UnityEngine.InputSystem.Users
{
	// Token: 0x02000083 RID: 131
	[Flags]
	public enum InputUserPairingOptions
	{
		// Token: 0x040003D1 RID: 977
		None = 0,
		// Token: 0x040003D2 RID: 978
		ForcePlatformUserAccountSelection = 1,
		// Token: 0x040003D3 RID: 979
		ForceNoPlatformUserAccountSelection = 2,
		// Token: 0x040003D4 RID: 980
		UnpairCurrentDevicesFromUser = 8
	}
}
