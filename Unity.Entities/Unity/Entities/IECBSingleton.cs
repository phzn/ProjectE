using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200013A RID: 314
	public interface IECBSingleton
	{
		// Token: 0x06000649 RID: 1609
		void SetPendingBufferList(ref UnsafeList<EntityCommandBuffer> buffers);

		// Token: 0x0600064A RID: 1610
		void SetAllocator(Allocator allocatorIn);

		// Token: 0x0600064B RID: 1611 RVA: 0x000215F8 File Offset: 0x0001F7F8
		void SetAllocator(AllocatorManager.AllocatorHandle allocatorIn)
		{
			this.SetAllocator(allocatorIn.ToAllocator);
		}
	}
}
