using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000D0 RID: 208
	[NativeContainer]
	[GenerateTestsForBurstCompatibility]
	internal struct NativeStreamDispose
	{
		// Token: 0x06000973 RID: 2419 RVA: 0x0001C5A8 File Offset: 0x0001A7A8
		public void Dispose()
		{
			this.m_StreamData.Dispose();
		}

		// Token: 0x0400040D RID: 1037
		public UnsafeStream m_StreamData;
	}
}
