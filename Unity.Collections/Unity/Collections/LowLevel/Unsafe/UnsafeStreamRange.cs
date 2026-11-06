using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000141 RID: 321
	[GenerateTestsForBurstCompatibility]
	internal struct UnsafeStreamRange
	{
		// Token: 0x0400052A RID: 1322
		internal unsafe UnsafeStreamBlock* Block;

		// Token: 0x0400052B RID: 1323
		internal int OffsetInFirstBlock;

		// Token: 0x0400052C RID: 1324
		internal int ElementCount;

		// Token: 0x0400052D RID: 1325
		internal int LastOffset;

		// Token: 0x0400052E RID: 1326
		internal int NumberOfBlocks;
	}
}
