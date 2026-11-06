using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000437 RID: 1079
	internal struct HeapStatistics
	{
		// Token: 0x04000EDA RID: 3802
		public uint numAllocs;

		// Token: 0x04000EDB RID: 3803
		public uint totalSize;

		// Token: 0x04000EDC RID: 3804
		public uint allocatedSize;

		// Token: 0x04000EDD RID: 3805
		public uint freeSize;

		// Token: 0x04000EDE RID: 3806
		public uint largestAvailableBlock;

		// Token: 0x04000EDF RID: 3807
		public uint availableBlocksCount;

		// Token: 0x04000EE0 RID: 3808
		public uint blockCount;

		// Token: 0x04000EE1 RID: 3809
		public uint highWatermark;

		// Token: 0x04000EE2 RID: 3810
		public float fragmentation;

		// Token: 0x04000EE3 RID: 3811
		public HeapStatistics[] subAllocators;
	}
}
