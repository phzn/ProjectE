using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200012F RID: 303
	public struct UntypedUnsafeParallelHashMap
	{
		// Token: 0x0400050A RID: 1290
		[NativeDisableUnsafePtrRestriction]
		private unsafe UnsafeParallelHashMapData* m_Buffer;

		// Token: 0x0400050B RID: 1291
		private AllocatorManager.AllocatorHandle m_AllocatorLabel;
	}
}
