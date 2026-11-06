using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000438 RID: 1080
	internal class BestFitAllocator
	{
		// Token: 0x06002227 RID: 8743 RVA: 0x00082AC8 File Offset: 0x00080CC8
		public BestFitAllocator(uint size)
		{
			this.totalSize = size;
			this.m_FirstBlock = (this.m_FirstAvailableBlock = this.m_BlockPool.Get());
			this.m_FirstAvailableBlock.end = size;
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x00082B15 File Offset: 0x00080D15
		public uint totalSize { get; }

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06002229 RID: 8745 RVA: 0x00082B20 File Offset: 0x00080D20
		public uint highWatermark
		{
			get
			{
				return this.m_HighWatermark;
			}
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x00082B38 File Offset: 0x00080D38
		public Alloc Allocate(uint size)
		{
			BestFitAllocator.Block block = this.BestFitFindAvailableBlock(size);
			bool flag = block == null;
			Alloc alloc;
			if (flag)
			{
				alloc = default(Alloc);
			}
			else
			{
				Debug.Assert(block.size >= size);
				Debug.Assert(!block.allocated);
				bool flag2 = size != block.size;
				if (flag2)
				{
					this.SplitBlock(block, size);
				}
				Debug.Assert(block.size == size);
				bool flag3 = block.end > this.m_HighWatermark;
				if (flag3)
				{
					this.m_HighWatermark = block.end;
				}
				bool flag4 = block == this.m_FirstAvailableBlock;
				if (flag4)
				{
					this.m_FirstAvailableBlock = this.m_FirstAvailableBlock.nextAvailable;
				}
				bool flag5 = block.prevAvailable != null;
				if (flag5)
				{
					block.prevAvailable.nextAvailable = block.nextAvailable;
				}
				bool flag6 = block.nextAvailable != null;
				if (flag6)
				{
					block.nextAvailable.prevAvailable = block.prevAvailable;
				}
				block.allocated = true;
				block.prevAvailable = (block.nextAvailable = null);
				alloc = new Alloc
				{
					start = block.start,
					size = block.size,
					handle = block
				};
			}
			return alloc;
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x00082C78 File Offset: 0x00080E78
		public void Free(Alloc alloc)
		{
			BestFitAllocator.Block block = (BestFitAllocator.Block)alloc.handle;
			bool flag = !block.allocated;
			if (flag)
			{
				Debug.Assert(false, "Severe error: UIR allocation double-free");
			}
			else
			{
				Debug.Assert(block.allocated);
				Debug.Assert(block.start == alloc.start);
				Debug.Assert(block.size == alloc.size);
				bool flag2 = block.end == this.m_HighWatermark;
				if (flag2)
				{
					bool flag3 = block.prev != null;
					if (flag3)
					{
						this.m_HighWatermark = (block.prev.allocated ? block.prev.end : block.prev.start);
					}
					else
					{
						this.m_HighWatermark = 0U;
					}
				}
				block.allocated = false;
				BestFitAllocator.Block block2 = this.m_FirstAvailableBlock;
				BestFitAllocator.Block block3 = null;
				while (block2 != null && block2.start < block.start)
				{
					block3 = block2;
					block2 = block2.nextAvailable;
				}
				bool flag4 = block3 == null;
				if (flag4)
				{
					Debug.Assert(block.prevAvailable == null);
					block.nextAvailable = this.m_FirstAvailableBlock;
					this.m_FirstAvailableBlock = block;
				}
				else
				{
					block.prevAvailable = block3;
					block.nextAvailable = block3.nextAvailable;
					block3.nextAvailable = block;
				}
				bool flag5 = block.nextAvailable != null;
				if (flag5)
				{
					block.nextAvailable.prevAvailable = block;
				}
				bool flag6 = block.prevAvailable == block.prev && block.prev != null;
				if (flag6)
				{
					block = this.CoalesceBlockWithPrevious(block);
				}
				bool flag7 = block.nextAvailable == block.next && block.next != null;
				if (flag7)
				{
					block = this.CoalesceBlockWithPrevious(block.next);
				}
			}
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x00082E34 File Offset: 0x00081034
		private BestFitAllocator.Block CoalesceBlockWithPrevious(BestFitAllocator.Block block)
		{
			Debug.Assert(block.prevAvailable.end == block.start);
			Debug.Assert(block.prev.nextAvailable == block);
			BestFitAllocator.Block prev = block.prev;
			prev.next = block.next;
			bool flag = block.next != null;
			if (flag)
			{
				block.next.prev = prev;
			}
			prev.nextAvailable = block.nextAvailable;
			bool flag2 = block.nextAvailable != null;
			if (flag2)
			{
				block.nextAvailable.prevAvailable = block.prevAvailable;
			}
			prev.end = block.end;
			this.m_BlockPool.Return(block);
			return prev;
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x00082EE4 File Offset: 0x000810E4
		internal HeapStatistics GatherStatistics()
		{
			HeapStatistics heapStatistics = default(HeapStatistics);
			for (BestFitAllocator.Block block = this.m_FirstBlock; block != null; block = block.next)
			{
				bool allocated = block.allocated;
				if (allocated)
				{
					heapStatistics.numAllocs += 1U;
					heapStatistics.allocatedSize += block.size;
				}
				else
				{
					heapStatistics.freeSize += block.size;
					heapStatistics.availableBlocksCount += 1U;
					heapStatistics.largestAvailableBlock = Math.Max(heapStatistics.largestAvailableBlock, block.size);
				}
				heapStatistics.blockCount += 1U;
			}
			heapStatistics.totalSize = this.totalSize;
			heapStatistics.highWatermark = this.m_HighWatermark;
			bool flag = heapStatistics.freeSize > 0U;
			if (flag)
			{
				heapStatistics.fragmentation = (float)((heapStatistics.freeSize - heapStatistics.largestAvailableBlock) / heapStatistics.freeSize) * 100f;
			}
			return heapStatistics;
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x00082FD8 File Offset: 0x000811D8
		private BestFitAllocator.Block BestFitFindAvailableBlock(uint size)
		{
			BestFitAllocator.Block block = this.m_FirstAvailableBlock;
			BestFitAllocator.Block block2 = null;
			uint num = uint.MaxValue;
			while (block != null)
			{
				bool flag = block.size >= size && num > block.size;
				if (flag)
				{
					block2 = block;
					num = block.size;
				}
				block = block.nextAvailable;
			}
			return block2;
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x00083034 File Offset: 0x00081234
		private void SplitBlock(BestFitAllocator.Block block, uint size)
		{
			Debug.Assert(block.size > size);
			BestFitAllocator.Block block2 = this.m_BlockPool.Get();
			block2.next = block.next;
			block2.nextAvailable = block.nextAvailable;
			block2.prev = block;
			block2.prevAvailable = block;
			block2.start = block.start + size;
			block2.end = block.end;
			bool flag = block2.next != null;
			if (flag)
			{
				block2.next.prev = block2;
			}
			bool flag2 = block2.nextAvailable != null;
			if (flag2)
			{
				block2.nextAvailable.prevAvailable = block2;
			}
			block.next = block2;
			block.nextAvailable = block2;
			block.end = block2.start;
		}

		// Token: 0x04000EE5 RID: 3813
		private BestFitAllocator.Block m_FirstBlock;

		// Token: 0x04000EE6 RID: 3814
		private BestFitAllocator.Block m_FirstAvailableBlock;

		// Token: 0x04000EE7 RID: 3815
		private BestFitAllocator.BlockPool m_BlockPool = new BestFitAllocator.BlockPool();

		// Token: 0x04000EE8 RID: 3816
		private uint m_HighWatermark;

		// Token: 0x02000439 RID: 1081
		private class BlockPool : LinkedPool<BestFitAllocator.Block>
		{
			// Token: 0x06002230 RID: 8752 RVA: 0x000830EC File Offset: 0x000812EC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static BestFitAllocator.Block CreateBlock()
			{
				return new BestFitAllocator.Block();
			}

			// Token: 0x06002231 RID: 8753 RVA: 0x00003CD2 File Offset: 0x00001ED2
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void ResetBlock(BestFitAllocator.Block block)
			{
			}

			// Token: 0x06002232 RID: 8754 RVA: 0x00083103 File Offset: 0x00081303
			public BlockPool()
				: base(new Func<BestFitAllocator.Block>(BestFitAllocator.BlockPool.CreateBlock), new Action<BestFitAllocator.Block>(BestFitAllocator.BlockPool.ResetBlock), 10000)
			{
			}
		}

		// Token: 0x0200043A RID: 1082
		private class Block : LinkedPoolItem<BestFitAllocator.Block>
		{
			// Token: 0x170007D4 RID: 2004
			// (get) Token: 0x06002233 RID: 8755 RVA: 0x0008312C File Offset: 0x0008132C
			public uint size
			{
				get
				{
					return this.end - this.start;
				}
			}

			// Token: 0x04000EE9 RID: 3817
			public uint start;

			// Token: 0x04000EEA RID: 3818
			public uint end;

			// Token: 0x04000EEB RID: 3819
			public BestFitAllocator.Block prev;

			// Token: 0x04000EEC RID: 3820
			public BestFitAllocator.Block next;

			// Token: 0x04000EED RID: 3821
			public BestFitAllocator.Block prevAvailable;

			// Token: 0x04000EEE RID: 3822
			public BestFitAllocator.Block nextAvailable;

			// Token: 0x04000EEF RID: 3823
			public bool allocated;
		}
	}
}
