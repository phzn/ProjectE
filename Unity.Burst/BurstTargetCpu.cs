using System;

namespace Unity.Burst
{
	// Token: 0x02000016 RID: 22
	internal enum BurstTargetCpu
	{
		// Token: 0x040000AD RID: 173
		Auto,
		// Token: 0x040000AE RID: 174
		X86_SSE2,
		// Token: 0x040000AF RID: 175
		X86_SSE4,
		// Token: 0x040000B0 RID: 176
		X64_SSE2,
		// Token: 0x040000B1 RID: 177
		X64_SSE4,
		// Token: 0x040000B2 RID: 178
		AVX,
		// Token: 0x040000B3 RID: 179
		AVX2,
		// Token: 0x040000B4 RID: 180
		WASM32,
		// Token: 0x040000B5 RID: 181
		ARMV7A_NEON32,
		// Token: 0x040000B6 RID: 182
		ARMV8A_AARCH64,
		// Token: 0x040000B7 RID: 183
		THUMB2_NEON32,
		// Token: 0x040000B8 RID: 184
		ARMV8A_AARCH64_HALFFP,
		// Token: 0x040000B9 RID: 185
		ARMV9A
	}
}
