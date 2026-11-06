using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x0200009C RID: 156
	[NativeContainer]
	[GenerateTestsForBurstCompatibility]
	internal struct NativeListDispose
	{
		// Token: 0x060007CE RID: 1998 RVA: 0x000183A4 File Offset: 0x000165A4
		public unsafe void Dispose()
		{
			UnsafeList<int>* listData = (UnsafeList<int>*)this.m_ListData;
			UnsafeList<int>.Destroy(listData);
		}

		// Token: 0x040003C5 RID: 965
		[NativeDisableUnsafePtrRestriction]
		public unsafe UntypedUnsafeList* m_ListData;
	}
}
