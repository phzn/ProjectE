using System;

namespace Steamworks
{
	// Token: 0x0200011A RID: 282
	[Flags]
	public enum ERemoteStoragePlatform
	{
		// Token: 0x04000755 RID: 1877
		k_ERemoteStoragePlatformNone = 0,
		// Token: 0x04000756 RID: 1878
		k_ERemoteStoragePlatformWindows = 1,
		// Token: 0x04000757 RID: 1879
		k_ERemoteStoragePlatformOSX = 2,
		// Token: 0x04000758 RID: 1880
		k_ERemoteStoragePlatformPS3 = 4,
		// Token: 0x04000759 RID: 1881
		k_ERemoteStoragePlatformLinux = 8,
		// Token: 0x0400075A RID: 1882
		k_ERemoteStoragePlatformSwitch = 16,
		// Token: 0x0400075B RID: 1883
		k_ERemoteStoragePlatformAndroid = 32,
		// Token: 0x0400075C RID: 1884
		k_ERemoteStoragePlatformIOS = 64,
		// Token: 0x0400075D RID: 1885
		k_ERemoteStoragePlatformAll = -1
	}
}
