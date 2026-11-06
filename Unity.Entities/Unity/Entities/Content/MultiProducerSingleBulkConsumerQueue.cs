using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.LowLevel;

namespace Unity.Entities.Content
{
	// Token: 0x020004BA RID: 1210
	[BurstCompile]
	internal struct MultiProducerSingleBulkConsumerQueue<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IDisposable where T : struct, ValueType
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06001D24 RID: 7460 RVA: 0x00065E7A File Offset: 0x0006407A
		public int Length
		{
			get
			{
				this.spinLock.Acquire();
				int length = this.Values.Length;
				this.spinLock.Release();
				return length;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x00065E9D File Offset: 0x0006409D
		public bool IsCreated
		{
			get
			{
				return this.Values.IsCreated;
			}
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x00065EAA File Offset: 0x000640AA
		public MultiProducerSingleBulkConsumerQueue(int initialQueueSize)
		{
			this.spinLock = default(SpinLock);
			this.Values = new UnsafeRingQueue<T>(initialQueueSize, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x00065ECC File Offset: 0x000640CC
		public unsafe void Produce(T* vals, int count)
		{
			this.spinLock.Acquire();
			if (this.Values.Capacity - this.Values.Length < count)
			{
				UnsafeRingQueue<T> unsafeRingQueue = new UnsafeRingQueue<T>(this.Values.Length + count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				T t;
				while (this.Values.TryDequeue(out t))
				{
					unsafeRingQueue.Enqueue(t);
				}
				this.Values.Dispose();
				this.Values = unsafeRingQueue;
			}
			for (int i = 0; i < count; i++)
			{
				this.Values.TryEnqueue(vals[(IntPtr)i * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)]);
			}
			this.spinLock.Release();
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00065F78 File Offset: 0x00064178
		public void Produce(in T val)
		{
			this.spinLock.Acquire();
			if (!this.Values.TryEnqueue(val))
			{
				UnsafeRingQueue<T> unsafeRingQueue = new UnsafeRingQueue<T>(this.Values.Capacity * 2, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				T t;
				while (this.Values.TryDequeue(out t))
				{
					unsafeRingQueue.Enqueue(t);
				}
				this.Values.Dispose();
				this.Values = unsafeRingQueue;
				this.Values.TryEnqueue(val);
			}
			this.spinLock.Release();
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x00066008 File Offset: 0x00064208
		public unsafe bool ConsumeAll(out NativeArray<T> newIdContainer, AllocatorManager.AllocatorHandle allocator)
		{
			newIdContainer = default(NativeArray<T>);
			this.spinLock.Acquire();
			if (!this.Values.IsEmpty)
			{
				newIdContainer = new NativeArray<T>(this.Values.Length, allocator.ToAllocator, NativeArrayOptions.ClearMemory);
				UnsafeUtility.MemCpy(newIdContainer.GetUnsafePtr<T>(), (void*)this.Values.Ptr, (long)(sizeof(T) * this.Values.Length));
				int capacity = this.Values.Capacity;
				this.Values.Dispose();
				this.Values = new UnsafeRingQueue<T>(capacity, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
			this.spinLock.Release();
			return newIdContainer.IsCreated;
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x000660BB File Offset: 0x000642BB
		public void Dispose()
		{
			this.spinLock.Acquire();
			this.Values.Dispose();
			this.spinLock.Release();
		}

		// Token: 0x04000A7E RID: 2686
		private SpinLock spinLock;

		// Token: 0x04000A7F RID: 2687
		private UnsafeRingQueue<T> Values;
	}
}
