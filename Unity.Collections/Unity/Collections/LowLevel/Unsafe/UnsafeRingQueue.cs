using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200013C RID: 316
	[DebuggerDisplay("Length = {Length}, Capacity = {Capacity}, IsCreated = {IsCreated}, IsEmpty = {IsEmpty}")]
	[DebuggerTypeProxy(typeof(UnsafeRingQueueDebugView<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct UnsafeRingQueue<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable where T : struct, ValueType
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x000295E0 File Offset: 0x000277E0
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Filled == 0;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x000295EB File Offset: 0x000277EB
		public readonly int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Filled;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x000295F3 File Offset: 0x000277F3
		public readonly int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Capacity;
			}
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x000295FB File Offset: 0x000277FB
		public unsafe UnsafeRingQueue(T* ptr, int capacity)
		{
			this.Ptr = ptr;
			this.Allocator = AllocatorManager.None;
			this.m_Capacity = capacity;
			this.m_Filled = 0;
			this.m_Write = 0;
			this.m_Read = 0;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0002962C File Offset: 0x0002782C
		public unsafe UnsafeRingQueue(int capacity, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
		{
			this.Allocator = allocator;
			this.m_Capacity = capacity;
			this.m_Filled = 0;
			this.m_Write = 0;
			this.m_Read = 0;
			int num = capacity * UnsafeUtility.SizeOf<T>();
			this.Ptr = (T*)Memory.Unmanaged.Allocate((long)num, 16, allocator);
			if (options == NativeArrayOptions.ClearMemory)
			{
				UnsafeUtility.MemClear((void*)this.Ptr, (long)num);
			}
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00029688 File Offset: 0x00027888
		internal unsafe static UnsafeRingQueue<T>* Alloc(AllocatorManager.AllocatorHandle allocator)
		{
			return (UnsafeRingQueue<T>*)Memory.Unmanaged.Allocate((long)sizeof(UnsafeRingQueue<T>), UnsafeUtility.AlignOf<UnsafeRingQueue<T>>(), allocator);
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000296AC File Offset: 0x000278AC
		internal unsafe static void Free(UnsafeRingQueue<T>* data)
		{
			if (data == null)
			{
				throw new InvalidOperationException("UnsafeRingQueue has yet to be created or has been destroyed!");
			}
			AllocatorManager.AllocatorHandle allocator = data->Allocator;
			data->Dispose();
			Memory.Unmanaged.Free<UnsafeRingQueue<T>>(data, allocator);
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x000296DD File Offset: 0x000278DD
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Ptr != null;
			}
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000296EC File Offset: 0x000278EC
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			if (CollectionHelper.ShouldDeallocate(this.Allocator))
			{
				Memory.Unmanaged.Free<T>(this.Ptr, this.Allocator);
				this.Allocator = AllocatorManager.Invalid;
			}
			this.Ptr = null;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00029728 File Offset: 0x00027928
		public unsafe JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			if (CollectionHelper.ShouldDeallocate(this.Allocator))
			{
				JobHandle jobHandle = new UnsafeDisposeJob
				{
					Ptr = (void*)this.Ptr,
					Allocator = this.Allocator
				}.Schedule(inputDeps);
				this.Ptr = null;
				this.Allocator = AllocatorManager.Invalid;
				return jobHandle;
			}
			this.Ptr = null;
			return inputDeps;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00029794 File Offset: 0x00027994
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe bool TryEnqueueInternal(T value)
		{
			if (this.m_Filled == this.m_Capacity)
			{
				return false;
			}
			this.Ptr[(IntPtr)this.m_Write * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
			this.m_Write++;
			if (this.m_Write == this.m_Capacity)
			{
				this.m_Write = 0;
			}
			this.m_Filled++;
			return true;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x000297FE File Offset: 0x000279FE
		public bool TryEnqueue(T value)
		{
			return this.TryEnqueueInternal(value);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00029807 File Offset: 0x00027A07
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void ThrowQueueFull()
		{
			throw new InvalidOperationException("Trying to enqueue into full queue.");
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00029813 File Offset: 0x00027A13
		public void Enqueue(T value)
		{
			this.TryEnqueueInternal(value);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00029820 File Offset: 0x00027A20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe bool TryDequeueInternal(out T item)
		{
			item = this.Ptr[(IntPtr)this.m_Read * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			if (this.m_Filled == 0)
			{
				return false;
			}
			this.m_Read++;
			if (this.m_Read == this.m_Capacity)
			{
				this.m_Read = 0;
			}
			this.m_Filled--;
			return true;
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00029889 File Offset: 0x00027A89
		public bool TryDequeue(out T item)
		{
			return this.TryDequeueInternal(out item);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00029892 File Offset: 0x00027A92
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void ThrowQueueEmpty()
		{
			throw new InvalidOperationException("Trying to dequeue from an empty queue");
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000298A0 File Offset: 0x00027AA0
		public T Dequeue()
		{
			T t;
			this.TryDequeueInternal(out t);
			return t;
		}

		// Token: 0x0400051D RID: 1309
		[NativeDisableUnsafePtrRestriction]
		public unsafe T* Ptr;

		// Token: 0x0400051E RID: 1310
		public AllocatorManager.AllocatorHandle Allocator;

		// Token: 0x0400051F RID: 1311
		internal readonly int m_Capacity;

		// Token: 0x04000520 RID: 1312
		internal int m_Filled;

		// Token: 0x04000521 RID: 1313
		internal int m_Write;

		// Token: 0x04000522 RID: 1314
		internal int m_Read;
	}
}
