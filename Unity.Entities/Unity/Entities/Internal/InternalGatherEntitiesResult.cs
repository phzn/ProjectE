using System;
using Unity.Collections;

namespace Unity.Entities.Internal
{
	// Token: 0x0200045B RID: 1115
	public struct InternalGatherEntitiesResult
	{
		// Token: 0x0400098D RID: 2445
		public int StartingOffset;

		// Token: 0x0400098E RID: 2446
		public int EntityCount;

		// Token: 0x0400098F RID: 2447
		public unsafe Entity* EntityBuffer;

		// Token: 0x04000990 RID: 2448
		public NativeArray<Entity> EntityArray;
	}
}
