using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200043B RID: 1083
	internal class GPUBufferAllocator
	{
		// Token: 0x06002235 RID: 8757 RVA: 0x00083154 File Offset: 0x00081354
		public GPUBufferAllocator(uint maxSize)
		{
			this.m_Low = new BestFitAllocator(maxSize);
			this.m_High = new BestFitAllocator(maxSize);
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x00083178 File Offset: 0x00081378
		public Alloc Allocate(uint size, bool shortLived)
		{
			bool flag = !shortLived;
			Alloc alloc;
			if (flag)
			{
				alloc = this.m_Low.Allocate(size);
			}
			else
			{
				alloc = this.m_High.Allocate(size);
				alloc.start = this.m_High.totalSize - alloc.start - alloc.size;
			}
			alloc.shortLived = shortLived;
			bool flag2 = this.HighLowCollide() && alloc.size > 0U;
			Alloc alloc2;
			if (flag2)
			{
				this.Free(alloc);
				alloc2 = default(Alloc);
			}
			else
			{
				alloc2 = alloc;
			}
			return alloc2;
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x0008320C File Offset: 0x0008140C
		public void Free(Alloc alloc)
		{
			bool flag = !alloc.shortLived;
			if (flag)
			{
				this.m_Low.Free(alloc);
			}
			else
			{
				alloc.start = this.m_High.totalSize - alloc.start - alloc.size;
				this.m_High.Free(alloc);
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06002238 RID: 8760 RVA: 0x00083268 File Offset: 0x00081468
		public bool isEmpty
		{
			get
			{
				return this.m_Low.highWatermark == 0U && this.m_High.highWatermark == 0U;
			}
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x00083298 File Offset: 0x00081498
		public HeapStatistics GatherStatistics()
		{
			HeapStatistics heapStatistics = default(HeapStatistics);
			heapStatistics.subAllocators = new HeapStatistics[]
			{
				this.m_Low.GatherStatistics(),
				this.m_High.GatherStatistics()
			};
			heapStatistics.largestAvailableBlock = uint.MaxValue;
			for (int i = 0; i < 2; i++)
			{
				heapStatistics.numAllocs += heapStatistics.subAllocators[i].numAllocs;
				heapStatistics.totalSize = Math.Max(heapStatistics.totalSize, heapStatistics.subAllocators[i].totalSize);
				heapStatistics.allocatedSize += heapStatistics.subAllocators[i].allocatedSize;
				heapStatistics.largestAvailableBlock = Math.Min(heapStatistics.largestAvailableBlock, heapStatistics.subAllocators[i].largestAvailableBlock);
				heapStatistics.availableBlocksCount += heapStatistics.subAllocators[i].availableBlocksCount;
				heapStatistics.blockCount += heapStatistics.subAllocators[i].blockCount;
				heapStatistics.highWatermark = Math.Max(heapStatistics.highWatermark, heapStatistics.subAllocators[i].highWatermark);
				heapStatistics.fragmentation = Math.Max(heapStatistics.fragmentation, heapStatistics.subAllocators[i].fragmentation);
			}
			heapStatistics.freeSize = heapStatistics.totalSize - heapStatistics.allocatedSize;
			return heapStatistics;
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x00083414 File Offset: 0x00081614
		private bool HighLowCollide()
		{
			return this.m_Low.highWatermark + this.m_High.highWatermark > this.m_Low.totalSize;
		}

		// Token: 0x04000EF0 RID: 3824
		private BestFitAllocator m_Low;

		// Token: 0x04000EF1 RID: 3825
		private BestFitAllocator m_High;
	}
}
