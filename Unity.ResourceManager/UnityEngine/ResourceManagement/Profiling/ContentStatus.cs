using System;

namespace UnityEngine.ResourceManagement.Profiling
{
	// Token: 0x0200004B RID: 75
	[Flags]
	internal enum ContentStatus
	{
		// Token: 0x040000AF RID: 175
		None = 0,
		// Token: 0x040000B0 RID: 176
		Queue = 2,
		// Token: 0x040000B1 RID: 177
		Downloading = 4,
		// Token: 0x040000B2 RID: 178
		Released = 16,
		// Token: 0x040000B3 RID: 179
		Loading = 64,
		// Token: 0x040000B4 RID: 180
		Active = 256
	}
}
