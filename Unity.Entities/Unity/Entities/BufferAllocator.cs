using System;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x0200002D RID: 45
	[GenerateTestsForBurstCompatibility]
	internal struct BufferAllocator : IBufferAllocator, IDisposable
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x00003C2C File Offset: 0x00001E2C
		public BufferAllocator(int budgetBytes, int bufferBytes, AllocatorManager.AllocatorHandle handle)
		{
			this.Allocator = new BufferAllocatorHeap(budgetBytes, bufferBytes, handle);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003C3C File Offset: 0x00001E3C
		public void Dispose()
		{
			this.Allocator.Dispose();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003C49 File Offset: 0x00001E49
		public int Allocate()
		{
			return this.Allocator.Allocate();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003C56 File Offset: 0x00001E56
		public void Free(int index)
		{
			this.Allocator.Free(index);
		}

		// Token: 0x17000019 RID: 25
		public unsafe void* this[int index]
		{
			get
			{
				return this.Allocator[index];
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00003C72 File Offset: 0x00001E72
		public int BufferCapacity
		{
			get
			{
				return this.Allocator.BufferCapacity;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00003C7F File Offset: 0x00001E7F
		public bool IsEmpty
		{
			get
			{
				return this.Allocator.IsEmpty;
			}
		}

		// Token: 0x0400003E RID: 62
		private BufferAllocatorHeap Allocator;
	}
}
