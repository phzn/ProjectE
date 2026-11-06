using System;

namespace UnityEngine.Experimental.AI
{
	// Token: 0x02000004 RID: 4
	[Flags]
	public enum PathQueryStatus
	{
		// Token: 0x04000005 RID: 5
		Failure = -2147483648,
		// Token: 0x04000006 RID: 6
		Success = 1073741824,
		// Token: 0x04000007 RID: 7
		InProgress = 536870912,
		// Token: 0x04000008 RID: 8
		StatusDetailMask = 16777215,
		// Token: 0x04000009 RID: 9
		WrongMagic = 1,
		// Token: 0x0400000A RID: 10
		WrongVersion = 2,
		// Token: 0x0400000B RID: 11
		OutOfMemory = 4,
		// Token: 0x0400000C RID: 12
		InvalidParam = 8,
		// Token: 0x0400000D RID: 13
		BufferTooSmall = 16,
		// Token: 0x0400000E RID: 14
		OutOfNodes = 32,
		// Token: 0x0400000F RID: 15
		PartialResult = 64
	}
}
