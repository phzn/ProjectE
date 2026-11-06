using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000BF RID: 191
	[NativeContainer]
	[DebuggerDisplay("Length = {Length}, Capacity = {Capacity}, IsCreated = {IsCreated}, IsEmpty = {IsEmpty}")]
	[DebuggerTypeProxy(typeof(NativeRingQueueDebugView<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct NativeRingQueue<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable where T : struct, ValueType
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x0001B041 File Offset: 0x00019241
		public unsafe readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_RingQueue != null && this.m_RingQueue->IsCreated;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x0001B05A File Offset: 0x0001925A
		public unsafe readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_RingQueue == null || this.m_RingQueue->Length == 0;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x0001B076 File Offset: 0x00019276
		public unsafe readonly int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return CollectionHelper.AssumePositive(this.m_RingQueue->Length);
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x0001B088 File Offset: 0x00019288
		public unsafe readonly int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return CollectionHelper.AssumePositive(this.m_RingQueue->Capacity);
			}
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0001B09A File Offset: 0x0001929A
		public unsafe NativeRingQueue(int capacity, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
		{
			this.m_RingQueue = UnsafeRingQueue<T>.Alloc(allocator);
			*this.m_RingQueue = new UnsafeRingQueue<T>(capacity, allocator, options);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0001B0BB File Offset: 0x000192BB
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			UnsafeRingQueue<T>.Free(this.m_RingQueue);
			this.m_RingQueue = null;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0001B0DC File Offset: 0x000192DC
		public unsafe JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new NativeRingQueueDisposeJob
			{
				Data = new NativeRingQueueDispose
				{
					m_QueueData = (UnsafeRingQueue<int>*)this.m_RingQueue
				}
			}.Schedule(inputDeps);
			this.m_RingQueue = null;
			return jobHandle;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0001B127 File Offset: 0x00019327
		public unsafe bool TryEnqueue(T value)
		{
			return this.m_RingQueue->TryEnqueue(value);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0001B135 File Offset: 0x00019335
		public unsafe void Enqueue(T value)
		{
			this.m_RingQueue->Enqueue(value);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0001B143 File Offset: 0x00019343
		public unsafe bool TryDequeue(out T item)
		{
			return this.m_RingQueue->TryDequeue(out item);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0001B151 File Offset: 0x00019351
		public unsafe T Dequeue()
		{
			return this.m_RingQueue->Dequeue();
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void CheckRead()
		{
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void CheckWrite()
		{
		}

		// Token: 0x040003ED RID: 1005
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeRingQueue<T>* m_RingQueue;
	}
}
