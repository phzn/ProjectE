using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200002B RID: 43
	[GenerateTestsForBurstCompatibility]
	internal struct BlockAllocator : IDisposable
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00003957 File Offset: 0x00001B57
		private int ms_BudgetInBytes
		{
			get
			{
				return this.m_bufferAllocator.BufferCapacity << 16;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003968 File Offset: 0x00001B68
		public BlockAllocator(AllocatorManager.AllocatorHandle handle, int budgetInBytes)
		{
			this.m_bufferAllocator = new BufferAllocator(budgetInBytes, 65536, handle);
			this.m_nextPtr = 0UL;
			int num = budgetInBytes + 65536 - 1 >> 16;
			this.m_allocations = new UnsafeList<int>(num, handle, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < num; i++)
			{
				int num2 = 0;
				this.m_allocations.Add(in num2);
			}
			this.m_currentBlockIndex = -1;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000039CC File Offset: 0x00001BCC
		public void Dispose()
		{
			this.m_bufferAllocator.Dispose();
			this.m_allocations.Dispose();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000039E4 File Offset: 0x00001BE4
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe void CheckBlockHasAllocations(int blockIndex)
		{
			if (this.m_allocations.Ptr[blockIndex] <= 0)
			{
				throw new ArgumentException("Cannot free this pointer from BlockAllocator: no more allocations to free in its block.");
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003A05 File Offset: 0x00001C05
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void ThrowCouldNotFindPointer()
		{
			throw new ArgumentException("Cannot free this pointer from BlockAllocator: can't be found in any block.");
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003A14 File Offset: 0x00001C14
		public unsafe void Free(void* pointer)
		{
			if (pointer == null)
			{
				return;
			}
			int length = this.m_allocations.Length;
			for (int i = length - 1; i >= 0; i--)
			{
				byte* ptr = (byte*)this.m_bufferAllocator[i];
				if (pointer >= (void*)ptr && pointer < (void*)(ptr + 65536))
				{
					int* ptr2 = this.m_allocations.Ptr + i;
					int num = *ptr2 - 1;
					*ptr2 = num;
					if (num == 0)
					{
						if (i == length - 1)
						{
							this.m_nextPtr = this.m_bufferAllocator[i];
							return;
						}
						this.m_bufferAllocator.Free(i);
						if (i == this.m_currentBlockIndex)
						{
							this.m_currentBlockIndex = -1;
						}
					}
					return;
				}
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003AB0 File Offset: 0x00001CB0
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckAllocationTooLarge(int bytesToAllocate, int alignment)
		{
			if (bytesToAllocate > 65536)
			{
				throw new ArgumentException(string.Format("Cannot allocate more than {0} in BlockAllocator. Requested: {1}", 65536, bytesToAllocate));
			}
			if (bytesToAllocate + alignment - 1 > 65536)
			{
				int num = 65536 - (alignment - 1);
				throw new ArgumentException(string.Format("Cannot guarantee allocation of {0} bytes. Allocation size must be <= {1} bytes to guarantee allocation.", bytesToAllocate, num));
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003B17 File Offset: 0x00001D17
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckExceededBudget()
		{
			if (this.m_bufferAllocator.IsEmpty)
			{
				throw new ArgumentException(string.Format("Cannot exceed budget of {0} in BlockAllocator.", this.ms_BudgetInBytes));
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003B44 File Offset: 0x00001D44
		public unsafe byte* Allocate(int bytesToAllocate, int alignment)
		{
			ulong num = CollectionHelper.Align(this.m_nextPtr, (ulong)((long)alignment));
			ulong num2 = num + (ulong)((long)bytesToAllocate);
			if (this.m_currentBlockIndex < 0 || num2 > (byte*)this.m_bufferAllocator[this.m_currentBlockIndex] + 65536L)
			{
				int num3 = this.m_bufferAllocator.Allocate();
				this.m_allocations.Ptr[num3] = 0;
				this.m_currentBlockIndex = num3;
				num = CollectionHelper.Align(this.m_bufferAllocator[this.m_currentBlockIndex], (ulong)((long)alignment));
				num2 = num + (ulong)((long)bytesToAllocate);
			}
			byte* ptr = num;
			this.m_nextPtr = num2;
			this.m_allocations.Ptr[this.m_currentBlockIndex]++;
			return ptr;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003BF2 File Offset: 0x00001DF2
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe T* Allocate<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int items = 1) where T : struct, ValueType
		{
			return (T*)this.Allocate(items * UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003C06 File Offset: 0x00001E06
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe byte* Construct(int size, int alignment, void* src)
		{
			byte* ptr = this.Allocate(size, alignment);
			UnsafeUtility.MemCpy((void*)ptr, src, (long)size);
			return ptr;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003C19 File Offset: 0x00001E19
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe T* Construct<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T* src) where T : struct, ValueType
		{
			return (T*)this.Construct(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), (void*)src);
		}

		// Token: 0x04000038 RID: 56
		private BufferAllocator m_bufferAllocator;

		// Token: 0x04000039 RID: 57
		private UnsafeList<int> m_allocations;

		// Token: 0x0400003A RID: 58
		private int m_currentBlockIndex;

		// Token: 0x0400003B RID: 59
		private ulong m_nextPtr;

		// Token: 0x0400003C RID: 60
		private const int ms_Log2BlockSize = 16;

		// Token: 0x0400003D RID: 61
		private const int ms_BlockSize = 65536;
	}
}
