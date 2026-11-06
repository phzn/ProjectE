using System;

namespace Unity.Entities
{
	// Token: 0x020002EF RID: 751
	[Flags]
	internal enum ChunkFlags
	{
		// Token: 0x0400076B RID: 1899
		None = 0,
		// Token: 0x0400076C RID: 1900
		Unused0 = 1,
		// Token: 0x0400076D RID: 1901
		Unused1 = 2,
		// Token: 0x0400076E RID: 1902
		TempAssertWillDestroyAllInLinkedEntityGroup = 4
	}
}
