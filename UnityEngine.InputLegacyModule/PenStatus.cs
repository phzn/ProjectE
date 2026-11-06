using System;

namespace UnityEngine
{
	// Token: 0x02000006 RID: 6
	[Flags]
	public enum PenStatus
	{
		// Token: 0x0400001E RID: 30
		None = 0,
		// Token: 0x0400001F RID: 31
		Contact = 1,
		// Token: 0x04000020 RID: 32
		Barrel = 2,
		// Token: 0x04000021 RID: 33
		Inverted = 4,
		// Token: 0x04000022 RID: 34
		Eraser = 8
	}
}
