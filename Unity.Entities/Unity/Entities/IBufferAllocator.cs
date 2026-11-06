using System;

namespace Unity.Entities
{
	// Token: 0x0200002C RID: 44
	internal interface IBufferAllocator : IDisposable
	{
		// Token: 0x060000BE RID: 190
		int Allocate();

		// Token: 0x060000BF RID: 191
		void Free(int index);

		// Token: 0x17000016 RID: 22
		unsafe void* this[int index] { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000C1 RID: 193
		int BufferCapacity { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000C2 RID: 194
		bool IsEmpty { get; }
	}
}
