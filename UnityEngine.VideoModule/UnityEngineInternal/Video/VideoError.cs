using System;
using UnityEngine.Scripting;

namespace UnityEngineInternal.Video
{
	// Token: 0x02000012 RID: 18
	[UsedByNativeCode]
	internal enum VideoError
	{
		// Token: 0x0400002F RID: 47
		NoErr,
		// Token: 0x04000030 RID: 48
		OutOfMemoryErr,
		// Token: 0x04000031 RID: 49
		CantReadFile,
		// Token: 0x04000032 RID: 50
		CantWriteFile,
		// Token: 0x04000033 RID: 51
		BadParams,
		// Token: 0x04000034 RID: 52
		NoData,
		// Token: 0x04000035 RID: 53
		BadPermissions,
		// Token: 0x04000036 RID: 54
		DeviceNotAvailable,
		// Token: 0x04000037 RID: 55
		ResourceNotAvailable,
		// Token: 0x04000038 RID: 56
		NetworkErr
	}
}
