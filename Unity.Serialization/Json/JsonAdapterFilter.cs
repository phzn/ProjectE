using System;

namespace Unity.Serialization.Json
{
	// Token: 0x0200001E RID: 30
	[Flags]
	internal enum JsonAdapterFilter
	{
		// Token: 0x04000016 RID: 22
		None = 0,
		// Token: 0x04000017 RID: 23
		UserDefined = 1,
		// Token: 0x04000018 RID: 24
		Global = 2,
		// Token: 0x04000019 RID: 25
		Internal = 4,
		// Token: 0x0400001A RID: 26
		All = -1
	}
}
