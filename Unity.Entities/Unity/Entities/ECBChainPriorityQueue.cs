using System;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x0200010A RID: 266
	internal struct ECBChainPriorityQueue : IDisposable
	{
		// Token: 0x0600040D RID: 1037 RVA: 0x000172AC File Offset: 0x000154AC
		public unsafe ECBChainPriorityQueue(ECBChainPlaybackState* chainStates, int chainStateCount, AllocatorManager.AllocatorHandle alloc)
		{
			this.m_Size = chainStateCount;
			this.m_Allocator = alloc;
			this.m_Heap = (ECBChainHeapElement*)Memory.Unmanaged.Allocate((long)((this.m_Size + ECBChainPriorityQueue.BaseIndex) * sizeof(ECBChainHeapElement)), 64, this.m_Allocator);
			for (int i = this.m_Size - 1; i >= this.m_Size / 2; i--)
			{
				this.m_Heap[ECBChainPriorityQueue.BaseIndex + i].SortKey = chainStates[i].NextSortKey;
				this.m_Heap[ECBChainPriorityQueue.BaseIndex + i].ChainIndex = i;
			}
			for (int j = this.m_Size / 2 - 1; j >= 0; j--)
			{
				this.m_Heap[ECBChainPriorityQueue.BaseIndex + j].SortKey = chainStates[j].NextSortKey;
				this.m_Heap[ECBChainPriorityQueue.BaseIndex + j].ChainIndex = j;
				this.Heapify(ECBChainPriorityQueue.BaseIndex + j);
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000173B8 File Offset: 0x000155B8
		public void Dispose()
		{
			Memory.Unmanaged.Free<ECBChainHeapElement>(this.m_Heap, this.m_Allocator);
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x000173CB File Offset: 0x000155CB
		public bool Empty
		{
			get
			{
				return this.m_Size <= 0;
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x000173DC File Offset: 0x000155DC
		public unsafe ECBChainHeapElement Peek()
		{
			if (this.Empty)
			{
				return new ECBChainHeapElement
				{
					ChainIndex = -1,
					SortKey = -1
				};
			}
			return this.m_Heap[ECBChainPriorityQueue.BaseIndex];
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00017424 File Offset: 0x00015624
		public unsafe ECBChainHeapElement Pop()
		{
			if (this.Empty)
			{
				return new ECBChainHeapElement
				{
					ChainIndex = -1,
					SortKey = -1
				};
			}
			ECBChainHeapElement ecbchainHeapElement = this.Peek();
			ref ECBChainHeapElement ptr = ref this.m_Heap[ECBChainPriorityQueue.BaseIndex];
			IntPtr heap = this.m_Heap;
			int size = this.m_Size;
			this.m_Size = size - 1;
			ptr = *(heap + (IntPtr)size * (IntPtr)sizeof(ECBChainHeapElement));
			if (!this.Empty)
			{
				this.Heapify(ECBChainPriorityQueue.BaseIndex);
			}
			return ecbchainHeapElement;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000174AA File Offset: 0x000156AA
		public unsafe void ReplaceTop(ECBChainHeapElement value)
		{
			this.m_Heap[ECBChainPriorityQueue.BaseIndex] = value;
			this.Heapify(ECBChainPriorityQueue.BaseIndex);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x000174D4 File Offset: 0x000156D4
		private unsafe void Heapify(int i)
		{
			ECBChainHeapElement ecbchainHeapElement = this.m_Heap[i];
			while (i <= this.m_Size / 2)
			{
				int num = 2 * i;
				if (num < this.m_Size && this.m_Heap[num + 1].SortKey < this.m_Heap[num].SortKey)
				{
					num++;
				}
				if (ecbchainHeapElement.SortKey < this.m_Heap[num].SortKey)
				{
					break;
				}
				this.m_Heap[i] = this.m_Heap[num];
				i = num;
			}
			this.m_Heap[i] = ecbchainHeapElement;
		}

		// Token: 0x040002CD RID: 717
		private unsafe readonly ECBChainHeapElement* m_Heap;

		// Token: 0x040002CE RID: 718
		private int m_Size;

		// Token: 0x040002CF RID: 719
		private readonly AllocatorManager.AllocatorHandle m_Allocator;

		// Token: 0x040002D0 RID: 720
		private static readonly int BaseIndex = 1;
	}
}
