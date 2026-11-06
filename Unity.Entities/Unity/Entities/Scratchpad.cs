using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200027A RID: 634
	[BurstCompile]
	[GenerateTestsForBurstCompatibility]
	public struct Scratchpad
	{
		// Token: 0x06001103 RID: 4355 RVA: 0x000483A3 File Offset: 0x000465A3
		public unsafe ref ScratchpadAllocator GetScratchpadAllocator()
		{
			return this.m_allocatorHelpers[(IntPtr)this.m_ThreadIndex * (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>) / (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>)].Allocator;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000483BF File Offset: 0x000465BF
		public unsafe ref ScratchpadAllocator GetScratchpadAllocator(int threadIndex)
		{
			return this.m_allocatorHelpers[(IntPtr)threadIndex * (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>) / (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>)].Allocator;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000483D6 File Offset: 0x000465D6
		[ExcludeFromBurstCompatTesting("Accesses managed delegates")]
		public Scratchpad(int numScratchpadAllocators, int initialSizeInBytes = 32768, bool isGlobal = false, int globalIndex = 0)
		{
			this = default(Scratchpad);
			this.Initialize(numScratchpadAllocators, initialSizeInBytes, isGlobal, globalIndex);
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000483EC File Offset: 0x000465EC
		[ExcludeFromBurstCompatTesting("Accesses managed delegate")]
		public unsafe void Initialize(int numScratchpadAllocators, int initialSizeInBytes = 32768, bool isGlobal = false, int globalIndex = 0)
		{
			this.m_numAllocatorHelpers = numScratchpadAllocators;
			this.m_allocatorHelpers = (AllocatorHelper<ScratchpadAllocator>*)Memory.Unmanaged.Allocate((long)(sizeof(AllocatorHelper<ScratchpadAllocator>) * this.m_numAllocatorHelpers), 64, Allocator.Persistent);
			for (int i = 0; i < this.m_numAllocatorHelpers; i++)
			{
				this.m_allocatorHelpers[(IntPtr)i * (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>) / (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>)] = new AllocatorHelper<ScratchpadAllocator>(Allocator.Persistent, isGlobal, globalIndex + i);
				this.m_allocatorHelpers[(IntPtr)i * (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>) / (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>)].Allocator.Initialize(initialSizeInBytes);
			}
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00048474 File Offset: 0x00046674
		[ExcludeFromBurstCompatTesting("Accesses managed delegate")]
		public unsafe void Dispose()
		{
			if (JobsUtility.IsExecutingJob)
			{
				throw new InvalidOperationException("You cannot Dispose a Scratchpad from a Job.");
			}
			for (int i = 0; i < this.m_numAllocatorHelpers; i++)
			{
				this.m_allocatorHelpers[(IntPtr)i * (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>) / (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>)].Allocator.Dispose();
				this.m_allocatorHelpers[(IntPtr)i * (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>) / (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>)].Dispose();
			}
			Memory.Unmanaged.Free<AllocatorHelper<ScratchpadAllocator>>(this.m_allocatorHelpers, Allocator.Persistent);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x000484E4 File Offset: 0x000466E4
		public unsafe void Rewind()
		{
			if (JobsUtility.IsExecutingJob)
			{
				throw new InvalidOperationException("You cannot Rewind a whole Scratchpad from a Job.");
			}
			for (int i = 0; i < this.m_numAllocatorHelpers; i++)
			{
				this.m_allocatorHelpers[(IntPtr)i * (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>) / (IntPtr)sizeof(AllocatorHelper<ScratchpadAllocator>)].Allocator.Rewind();
			}
		}

		// Token: 0x04000684 RID: 1668
		[NativeDisableUnsafePtrRestriction]
		private unsafe AllocatorHelper<ScratchpadAllocator>* m_allocatorHelpers;

		// Token: 0x04000685 RID: 1669
		private int m_numAllocatorHelpers;

		// Token: 0x04000686 RID: 1670
		[NativeSetThreadIndex]
		internal int m_ThreadIndex;
	}
}
