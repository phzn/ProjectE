using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000142 RID: 322
	[GenerateTestsForBurstCompatibility]
	internal struct UnsafeStreamBlockData
	{
		// Token: 0x06000D94 RID: 3476 RVA: 0x000299FC File Offset: 0x00027BFC
		internal unsafe UnsafeStreamBlock* Allocate(UnsafeStreamBlock* oldBlock, int threadIndex)
		{
			UnsafeStreamBlock* ptr = (UnsafeStreamBlock*)Memory.Unmanaged.Array.Resize(null, 0L, 4096L, this.Allocator, 1L, 16);
			ptr->Next = null;
			if (oldBlock == null)
			{
				ptr->Next = *(IntPtr*)(this.Blocks + (IntPtr)threadIndex * (IntPtr)sizeof(UnsafeStreamBlock*) / (IntPtr)sizeof(UnsafeStreamBlock*));
				*(IntPtr*)(this.Blocks + (IntPtr)threadIndex * (IntPtr)sizeof(UnsafeStreamBlock*) / (IntPtr)sizeof(UnsafeStreamBlock*)) = ptr;
			}
			else
			{
				ptr->Next = oldBlock->Next;
				oldBlock->Next = ptr;
			}
			return ptr;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00029A6F File Offset: 0x00027C6F
		internal unsafe void Free(UnsafeStreamBlock* oldBlock)
		{
			Memory.Unmanaged.Array.Resize((void*)oldBlock, 4096L, 0L, this.Allocator, 1L, 16);
		}

		// Token: 0x0400052F RID: 1327
		internal const int AllocationSize = 4096;

		// Token: 0x04000530 RID: 1328
		internal AllocatorManager.AllocatorHandle Allocator;

		// Token: 0x04000531 RID: 1329
		internal unsafe UnsafeStreamBlock** Blocks;

		// Token: 0x04000532 RID: 1330
		internal int BlockCount;

		// Token: 0x04000533 RID: 1331
		internal AllocatorManager.Block Ranges;

		// Token: 0x04000534 RID: 1332
		internal int RangeCount;
	}
}
