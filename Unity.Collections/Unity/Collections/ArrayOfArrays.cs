using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Collections
{
	// Token: 0x02000033 RID: 51
	internal struct ArrayOfArrays<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IDisposable where T : struct, ValueType
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00003B19 File Offset: 0x00001D19
		private int BlockSizeInElements
		{
			get
			{
				return 1 << this.m_log2BlockSizeInElements;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00003B26 File Offset: 0x00001D26
		private int BlockSizeInBytes
		{
			get
			{
				return this.BlockSizeInElements * sizeof(T);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00003B35 File Offset: 0x00001D35
		private int BlockMask
		{
			get
			{
				return this.BlockSizeInElements - 1;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00003B3F File Offset: 0x00001D3F
		public int Length
		{
			get
			{
				return this.m_lengthInElements;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00003B47 File Offset: 0x00001D47
		public int Capacity
		{
			get
			{
				return this.m_capacityInElements;
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00003B50 File Offset: 0x00001D50
		public unsafe ArrayOfArrays(int capacityInElements, AllocatorManager.AllocatorHandle backingAllocatorHandle, int log2BlockSizeInElements = 12)
		{
			this = default(ArrayOfArrays<T>);
			this.m_backingAllocatorHandle = backingAllocatorHandle;
			this.m_lengthInElements = 0;
			this.m_capacityInElements = capacityInElements;
			this.m_log2BlockSizeInElements = log2BlockSizeInElements;
			this.m_blocks = capacityInElements + this.BlockMask >> this.m_log2BlockSizeInElements;
			this.m_block = (IntPtr*)Memory.Unmanaged.Allocate((long)(sizeof(IntPtr) * this.m_blocks), 16, this.m_backingAllocatorHandle);
			UnsafeUtility.MemSet((void*)this.m_block, 0, (long)(sizeof(IntPtr) * this.m_blocks));
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00003BD4 File Offset: 0x00001DD4
		public unsafe void LockfreeAdd(T t)
		{
			int num = Interlocked.Increment(ref this.m_lengthInElements) - 1;
			int num2 = this.BlockIndexOfElement(num);
			if (this.m_block[(IntPtr)num2 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] == IntPtr.Zero)
			{
				void* ptr = Memory.Unmanaged.Allocate((long)this.BlockSizeInBytes, 16, this.m_backingAllocatorHandle);
				int num3 = math.min(this.m_blocks, num2 + 4);
				while (num2 < num3 && !(IntPtr.Zero == Interlocked.CompareExchange(ref this.m_block[(IntPtr)num2 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], (IntPtr)ptr, IntPtr.Zero)))
				{
					num2++;
				}
				if (num2 == num3)
				{
					Memory.Unmanaged.Free(ptr, this.m_backingAllocatorHandle);
				}
			}
			*this[num] = t;
		}

		// Token: 0x17000024 RID: 36
		public unsafe ref T this[int elementIndex]
		{
			get
			{
				int num = this.BlockIndexOfElement(elementIndex);
				IntPtr intPtr = this.m_block[(IntPtr)num * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				int num2 = elementIndex & this.BlockMask;
				T* ptr = (T*)(void*)intPtr;
				return ref ptr[(IntPtr)num2 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00003CCC File Offset: 0x00001ECC
		public void Rewind()
		{
			this.m_lengthInElements = 0;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00003CD8 File Offset: 0x00001ED8
		public unsafe void Clear()
		{
			this.Rewind();
			for (int i = 0; i < this.m_blocks; i++)
			{
				if (this.m_block[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] != IntPtr.Zero)
				{
					Memory.Unmanaged.Free((void*)this.m_block[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], this.m_backingAllocatorHandle);
					this.m_block[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = IntPtr.Zero;
				}
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00003D50 File Offset: 0x00001F50
		public void Dispose()
		{
			this.Clear();
			Memory.Unmanaged.Free<IntPtr>(this.m_block, this.m_backingAllocatorHandle);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00003D69 File Offset: 0x00001F69
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckElementIndex(int elementIndex)
		{
			if (elementIndex >= this.m_lengthInElements)
			{
				throw new ArgumentException(string.Format("Element index {0} must be less than length in elements {1}.", elementIndex, this.m_lengthInElements));
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00003D95 File Offset: 0x00001F95
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckBlockIndex(int blockIndex)
		{
			if (blockIndex >= this.m_blocks)
			{
				throw new ArgumentException(string.Format("Block index {0} must be less than number of blocks {1}.", blockIndex, this.m_blocks));
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00003DC1 File Offset: 0x00001FC1
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe void CheckBlockIsNotNull(int blockIndex)
		{
			if (this.m_block[(IntPtr)blockIndex * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] == IntPtr.Zero)
			{
				throw new ArgumentException(string.Format("Block index {0} is a null pointer.", blockIndex));
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00003DF6 File Offset: 0x00001FF6
		public unsafe void RemoveAtSwapBack(int elementIndex)
		{
			*this[elementIndex] = *this[this.Length - 1];
			this.m_lengthInElements--;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00003E25 File Offset: 0x00002025
		private int BlockIndexOfElement(int elementIndex)
		{
			return elementIndex >> this.m_log2BlockSizeInElements;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00003E34 File Offset: 0x00002034
		public unsafe void TrimExcess()
		{
			for (int i = this.BlockIndexOfElement(this.m_lengthInElements + this.BlockMask); i < this.m_blocks; i++)
			{
				if (this.m_block[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] != IntPtr.Zero)
				{
					Memory.Unmanaged.Free((void*)this.m_block[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], this.m_backingAllocatorHandle);
					this.m_block[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = IntPtr.Zero;
				}
			}
		}

		// Token: 0x04000079 RID: 121
		private AllocatorManager.AllocatorHandle m_backingAllocatorHandle;

		// Token: 0x0400007A RID: 122
		private int m_lengthInElements;

		// Token: 0x0400007B RID: 123
		private int m_capacityInElements;

		// Token: 0x0400007C RID: 124
		private int m_log2BlockSizeInElements;

		// Token: 0x0400007D RID: 125
		private int m_blocks;

		// Token: 0x0400007E RID: 126
		private unsafe IntPtr* m_block;
	}
}
