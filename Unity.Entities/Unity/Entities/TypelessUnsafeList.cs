using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001D6 RID: 470
	[NativeContainer]
	internal struct TypelessUnsafeList
	{
		// Token: 0x0400050A RID: 1290
		[NativeDisableUnsafePtrRestriction]
		public unsafe byte* Ptr;

		// Token: 0x0400050B RID: 1291
		[NativeDisableUnsafePtrRestriction]
		public unsafe int* Length;

		// Token: 0x0400050C RID: 1292
		public int Capacity;
	}
}
