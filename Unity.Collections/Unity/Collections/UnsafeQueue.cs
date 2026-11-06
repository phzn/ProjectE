using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000EC RID: 236
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct UnsafeQueue<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable where T : struct, ValueType
	{
		// Token: 0x06000A4F RID: 2639 RVA: 0x0001EB8D File Offset: 0x0001CD8D
		public UnsafeQueue(AllocatorManager.AllocatorHandle allocator)
		{
			this.m_AllocatorLabel = allocator;
			UnsafeQueueData.AllocateQueue<T>(allocator, out this.m_Buffer);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0001EBA4 File Offset: 0x0001CDA4
		internal unsafe static UnsafeQueue<T>* Alloc(AllocatorManager.AllocatorHandle allocator)
		{
			return (UnsafeQueue<T>*)Memory.Unmanaged.Allocate((long)sizeof(UnsafeQueue<T>), UnsafeUtility.AlignOf<UnsafeQueue<T>>(), allocator);
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0001EBC8 File Offset: 0x0001CDC8
		internal unsafe static void Free(UnsafeQueue<T>* data)
		{
			if (data == null)
			{
				throw new InvalidOperationException("UnsafeQueue has yet to be created or has been destroyed!");
			}
			AllocatorManager.AllocatorHandle allocatorLabel = data->m_AllocatorLabel;
			data->Dispose();
			Memory.Unmanaged.Free<UnsafeQueue<T>>(data, allocatorLabel);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0001EBFC File Offset: 0x0001CDFC
		public unsafe readonly bool IsEmpty()
		{
			if (this.IsCreated)
			{
				int num = 0;
				int currentRead = this.m_Buffer->m_CurrentRead;
				for (UnsafeQueueBlockHeader* ptr = (UnsafeQueueBlockHeader*)(void*)this.m_Buffer->m_FirstBlock; ptr != null; ptr = ptr->m_NextBlock)
				{
					num += ptr->m_NumItems;
					if (num > currentRead)
					{
						return false;
					}
				}
				return num == currentRead;
			}
			return true;
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0001EC54 File Offset: 0x0001CE54
		public unsafe readonly int Count
		{
			get
			{
				int num = 0;
				for (UnsafeQueueBlockHeader* ptr = (UnsafeQueueBlockHeader*)(void*)this.m_Buffer->m_FirstBlock; ptr != null; ptr = ptr->m_NextBlock)
				{
					num += ptr->m_NumItems;
				}
				return num - this.m_Buffer->m_CurrentRead;
			}
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0001EC98 File Offset: 0x0001CE98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe T Peek()
		{
			UnsafeQueueBlockHeader* ptr = (UnsafeQueueBlockHeader*)(void*)this.m_Buffer->m_FirstBlock;
			return UnsafeUtility.ReadArrayElement<T>((void*)(ptr + 1), this.m_Buffer->m_CurrentRead);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0001ECD0 File Offset: 0x0001CED0
		public unsafe void Enqueue(T value)
		{
			UnsafeQueueBlockHeader* ptr = UnsafeQueueData.AllocateWriteBlockMT<T>(this.m_Buffer, this.m_AllocatorLabel, 0);
			UnsafeUtility.WriteArrayElement<T>((void*)(ptr + 1), ptr->m_NumItems, value);
			ptr->m_NumItems++;
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0001ED10 File Offset: 0x0001CF10
		public T Dequeue()
		{
			T t;
			this.TryDequeue(out t);
			return t;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0001ED28 File Offset: 0x0001CF28
		public unsafe bool TryDequeue(out T item)
		{
			UnsafeQueueBlockHeader* ptr = (UnsafeQueueBlockHeader*)(void*)this.m_Buffer->m_FirstBlock;
			if (ptr != null)
			{
				UnsafeQueueData* buffer = this.m_Buffer;
				int currentRead = buffer->m_CurrentRead;
				buffer->m_CurrentRead = currentRead + 1;
				int num = currentRead;
				int numItems = ptr->m_NumItems;
				item = UnsafeUtility.ReadArrayElement<T>((void*)(ptr + 1), num);
				if (num + 1 >= numItems)
				{
					this.m_Buffer->m_CurrentRead = 0;
					this.m_Buffer->m_FirstBlock = (IntPtr)((void*)ptr->m_NextBlock);
					if (this.m_Buffer->m_FirstBlock == IntPtr.Zero)
					{
						this.m_Buffer->m_LastBlock = IntPtr.Zero;
					}
					int threadIndexCount = JobsUtility.ThreadIndexCount;
					for (int i = 0; i < threadIndexCount; i++)
					{
						if (this.m_Buffer->GetCurrentWriteBlockTLS(i) == ptr)
						{
							this.m_Buffer->SetCurrentWriteBlockTLS(i, null);
						}
					}
					Memory.Unmanaged.Free<UnsafeQueueBlockHeader>(ptr, this.m_AllocatorLabel);
				}
				return true;
			}
			item = default(T);
			return false;
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0001EE1C File Offset: 0x0001D01C
		public unsafe NativeArray<T> ToArray(AllocatorManager.AllocatorHandle allocator)
		{
			UnsafeQueueBlockHeader* ptr = (UnsafeQueueBlockHeader*)(void*)this.m_Buffer->m_FirstBlock;
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(this.Count, allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeQueueBlockHeader* ptr2 = ptr;
			byte* unsafePtr = (byte*)nativeArray.GetUnsafePtr<T>();
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = 0;
			int num3 = this.m_Buffer->m_CurrentRead * num;
			int num4 = this.m_Buffer->m_CurrentRead;
			while (ptr2 != null)
			{
				int num5 = (ptr2->m_NumItems - num4) * num;
				UnsafeUtility.MemCpy((void*)(unsafePtr + num2), (void*)(ptr2 + 1 + num3 / sizeof(UnsafeQueueBlockHeader)), (long)num5);
				num4 = (num3 = 0);
				num2 += num5;
				ptr2 = ptr2->m_NextBlock;
			}
			return nativeArray;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0001EEBC File Offset: 0x0001D0BC
		public unsafe void Clear()
		{
			UnsafeQueueBlockHeader* nextBlock;
			for (UnsafeQueueBlockHeader* ptr = (UnsafeQueueBlockHeader*)(void*)this.m_Buffer->m_FirstBlock; ptr != null; ptr = nextBlock)
			{
				nextBlock = ptr->m_NextBlock;
				Memory.Unmanaged.Free<UnsafeQueueBlockHeader>(ptr, this.m_AllocatorLabel);
			}
			this.m_Buffer->m_FirstBlock = IntPtr.Zero;
			this.m_Buffer->m_LastBlock = IntPtr.Zero;
			this.m_Buffer->m_CurrentRead = 0;
			int threadIndexCount = JobsUtility.ThreadIndexCount;
			for (int i = 0; i < threadIndexCount; i++)
			{
				this.m_Buffer->SetCurrentWriteBlockTLS(i, null);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0001EF40 File Offset: 0x0001D140
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Buffer != null;
			}
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0001EF4F File Offset: 0x0001D14F
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			UnsafeQueueData.DeallocateQueue(this.m_Buffer, this.m_AllocatorLabel);
			this.m_Buffer = null;
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0001EF74 File Offset: 0x0001D174
		public JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new UnsafeQueueDisposeJob
			{
				Data = new UnsafeQueueDispose
				{
					m_Buffer = this.m_Buffer,
					m_AllocatorLabel = this.m_AllocatorLabel
				}
			}.Schedule(inputDeps);
			this.m_Buffer = null;
			return jobHandle;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0001EFCC File Offset: 0x0001D1CC
		public UnsafeQueue<T>.ReadOnly AsReadOnly()
		{
			return new UnsafeQueue<T>.ReadOnly(ref this);
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0001EFD4 File Offset: 0x0001D1D4
		public UnsafeQueue<T>.ParallelWriter AsParallelWriter()
		{
			UnsafeQueue<T>.ParallelWriter parallelWriter;
			parallelWriter.m_Buffer = this.m_Buffer;
			parallelWriter.m_AllocatorLabel = this.m_AllocatorLabel;
			parallelWriter.m_ThreadIndex = 0;
			return parallelWriter;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0001F004 File Offset: 0x0001D204
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void CheckNotEmpty()
		{
			this.m_Buffer->m_FirstBlock == (IntPtr)0;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0001F01D File Offset: 0x0001D21D
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void ThrowEmpty()
		{
			throw new InvalidOperationException("Trying to read from an empty queue.");
		}

		// Token: 0x04000463 RID: 1123
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeQueueData* m_Buffer;

		// Token: 0x04000464 RID: 1124
		[NativeDisableUnsafePtrRestriction]
		internal AllocatorManager.AllocatorHandle m_AllocatorLabel;

		// Token: 0x020000ED RID: 237
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06000A61 RID: 2657 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000A62 RID: 2658 RVA: 0x0001F02C File Offset: 0x0001D22C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe bool MoveNext()
			{
				this.m_Index++;
				while (this.m_Block != null)
				{
					int numItems = this.m_Block->m_NumItems;
					if (this.m_Index < numItems)
					{
						this.value = UnsafeUtility.ReadArrayElement<T>((void*)(this.m_Block + 1), this.m_Index);
						return true;
					}
					this.m_Index -= numItems;
					this.m_Block = this.m_Block->m_NextBlock;
				}
				this.value = default(T);
				return false;
			}

			// Token: 0x06000A63 RID: 2659 RVA: 0x0001F0B4 File Offset: 0x0001D2B4
			public void Reset()
			{
				this.m_Block = this.m_FirstBlock;
				this.m_Index = -1;
			}

			// Token: 0x17000131 RID: 305
			// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0001F0C9 File Offset: 0x0001D2C9
			public T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.value;
				}
			}

			// Token: 0x17000132 RID: 306
			// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0001F0D1 File Offset: 0x0001D2D1
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04000465 RID: 1125
			[NativeDisableUnsafePtrRestriction]
			internal unsafe UnsafeQueueBlockHeader* m_FirstBlock;

			// Token: 0x04000466 RID: 1126
			[NativeDisableUnsafePtrRestriction]
			internal unsafe UnsafeQueueBlockHeader* m_Block;

			// Token: 0x04000467 RID: 1127
			internal int m_Index;

			// Token: 0x04000468 RID: 1128
			private T value;
		}

		// Token: 0x020000EE RID: 238
		public struct ReadOnly : IEnumerable<T>, IEnumerable
		{
			// Token: 0x06000A66 RID: 2662 RVA: 0x0001F0DE File Offset: 0x0001D2DE
			internal ReadOnly(ref UnsafeQueue<T> data)
			{
				this.m_Buffer = data.m_Buffer;
			}

			// Token: 0x17000133 RID: 307
			// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0001F0EC File Offset: 0x0001D2EC
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Buffer != null;
				}
			}

			// Token: 0x06000A68 RID: 2664 RVA: 0x0001F0FC File Offset: 0x0001D2FC
			public unsafe readonly bool IsEmpty()
			{
				int num = 0;
				int currentRead = this.m_Buffer->m_CurrentRead;
				for (UnsafeQueueBlockHeader* ptr = (UnsafeQueueBlockHeader*)(void*)this.m_Buffer->m_FirstBlock; ptr != null; ptr = ptr->m_NextBlock)
				{
					num += ptr->m_NumItems;
					if (num > currentRead)
					{
						return false;
					}
				}
				return num == currentRead;
			}

			// Token: 0x17000134 RID: 308
			// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0001F14C File Offset: 0x0001D34C
			public unsafe readonly int Count
			{
				get
				{
					int num = 0;
					for (UnsafeQueueBlockHeader* ptr = (UnsafeQueueBlockHeader*)(void*)this.m_Buffer->m_FirstBlock; ptr != null; ptr = ptr->m_NextBlock)
					{
						num += ptr->m_NumItems;
					}
					return num - this.m_Buffer->m_CurrentRead;
				}
			}

			// Token: 0x17000135 RID: 309
			public readonly T this[int index]
			{
				get
				{
					T t;
					this.TryGetValue(index, out t);
					return t;
				}
			}

			// Token: 0x06000A6B RID: 2667 RVA: 0x0001F1A8 File Offset: 0x0001D3A8
			private unsafe readonly bool TryGetValue(int index, out T item)
			{
				if (index >= 0)
				{
					int num = index;
					for (UnsafeQueueBlockHeader* ptr = (UnsafeQueueBlockHeader*)(void*)this.m_Buffer->m_FirstBlock; ptr != null; ptr = ptr->m_NextBlock)
					{
						int numItems = ptr->m_NumItems;
						if (num < numItems)
						{
							item = UnsafeUtility.ReadArrayElement<T>((void*)(ptr + 1), num);
							return true;
						}
						num -= numItems;
					}
				}
				item = default(T);
				return false;
			}

			// Token: 0x06000A6C RID: 2668 RVA: 0x0001F208 File Offset: 0x0001D408
			public unsafe readonly UnsafeQueue<T>.Enumerator GetEnumerator()
			{
				return new UnsafeQueue<T>.Enumerator
				{
					m_FirstBlock = (UnsafeQueueBlockHeader*)(void*)this.m_Buffer->m_FirstBlock,
					m_Block = (UnsafeQueueBlockHeader*)(void*)this.m_Buffer->m_FirstBlock,
					m_Index = -1
				};
			}

			// Token: 0x06000A6D RID: 2669 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000A6E RID: 2670 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000A6F RID: 2671 RVA: 0x0001F254 File Offset: 0x0001D454
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private readonly void ThrowIndexOutOfRangeException(int index)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of bounds [0-{1}].", index, this.Count));
			}

			// Token: 0x04000469 RID: 1129
			[NativeDisableUnsafePtrRestriction]
			private unsafe UnsafeQueueData* m_Buffer;
		}

		// Token: 0x020000EF RID: 239
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ParallelWriter
		{
			// Token: 0x06000A70 RID: 2672 RVA: 0x0001F278 File Offset: 0x0001D478
			public unsafe void Enqueue(T value)
			{
				UnsafeQueueBlockHeader* ptr = UnsafeQueueData.AllocateWriteBlockMT<T>(this.m_Buffer, this.m_AllocatorLabel, this.m_ThreadIndex);
				UnsafeUtility.WriteArrayElement<T>((void*)(ptr + 1), ptr->m_NumItems, value);
				ptr->m_NumItems++;
			}

			// Token: 0x06000A71 RID: 2673 RVA: 0x0001F2BC File Offset: 0x0001D4BC
			public unsafe void Enqueue(T value, int threadIndexOverride)
			{
				UnsafeQueueBlockHeader* ptr = UnsafeQueueData.AllocateWriteBlockMT<T>(this.m_Buffer, this.m_AllocatorLabel, threadIndexOverride);
				UnsafeUtility.WriteArrayElement<T>((void*)(ptr + 1), ptr->m_NumItems, value);
				ptr->m_NumItems++;
			}

			// Token: 0x0400046A RID: 1130
			[NativeDisableUnsafePtrRestriction]
			internal unsafe UnsafeQueueData* m_Buffer;

			// Token: 0x0400046B RID: 1131
			internal AllocatorManager.AllocatorHandle m_AllocatorLabel;

			// Token: 0x0400046C RID: 1132
			[NativeSetThreadIndex]
			internal int m_ThreadIndex;
		}
	}
}
