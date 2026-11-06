using System;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x020000F1 RID: 241
	internal struct MultipleEntitiesCommand
	{
		// Token: 0x04000279 RID: 633
		public BasicCommand Header;

		// Token: 0x0400027A RID: 634
		public EntityNode Entities;

		// Token: 0x0400027B RID: 635
		public int EntitiesCount;

		// Token: 0x0400027C RID: 636
		public AllocatorManager.AllocatorHandle Allocator;

		// Token: 0x0400027D RID: 637
		public int SkipDeferredEntityLookup;
	}
}
