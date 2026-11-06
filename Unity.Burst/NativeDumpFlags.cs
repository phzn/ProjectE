using System;

namespace Unity.Burst
{
	// Token: 0x02000017 RID: 23
	[Flags]
	internal enum NativeDumpFlags
	{
		// Token: 0x040000BB RID: 187
		None = 0,
		// Token: 0x040000BC RID: 188
		IL = 1,
		// Token: 0x040000BD RID: 189
		Unused = 2,
		// Token: 0x040000BE RID: 190
		IR = 4,
		// Token: 0x040000BF RID: 191
		IROptimized = 8,
		// Token: 0x040000C0 RID: 192
		Asm = 16,
		// Token: 0x040000C1 RID: 193
		Function = 32,
		// Token: 0x040000C2 RID: 194
		Analysis = 64,
		// Token: 0x040000C3 RID: 195
		IRPassAnalysis = 128,
		// Token: 0x040000C4 RID: 196
		ILPre = 256,
		// Token: 0x040000C5 RID: 197
		IRPerEntryPoint = 512,
		// Token: 0x040000C6 RID: 198
		All = 1021
	}
}
