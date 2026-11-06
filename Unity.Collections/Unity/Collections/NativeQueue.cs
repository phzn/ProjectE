using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000B5 RID: 181
	[NativeContainer]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct NativeQueue<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable where T : struct, ValueType
	{
		// Token: 0x060008C4 RID: 2244 RVA: 0x0001ABE2 File Offset: 0x00018DE2
		public unsafe NativeQueue(AllocatorManager.AllocatorHandle allocator)
		{
			this.m_Queue = UnsafeQueue<T>.Alloc(allocator);
			*this.m_Queue = new UnsafeQueue<T>(allocator);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0001AC01 File Offset: 0x00018E01
		public unsafe readonly bool IsEmpty()
		{
			return !this.IsCreated || this.m_Queue->IsEmpty();
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x0001AC18 File Offset: 0x00018E18
		public unsafe readonly int Count
		{
			get
			{
				return this.m_Queue->Count;
			}
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0001AC25 File Offset: 0x00018E25
		public unsafe T Peek()
		{
			return this.m_Queue->Peek();
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001AC32 File Offset: 0x00018E32
		public unsafe void Enqueue(T value)
		{
			this.m_Queue->Enqueue(value);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0001AC40 File Offset: 0x00018E40
		public unsafe T Dequeue()
		{
			return this.m_Queue->Dequeue();
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0001AC4D File Offset: 0x00018E4D
		public unsafe bool TryDequeue(out T item)
		{
			return this.m_Queue->TryDequeue(out item);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0001AC5B File Offset: 0x00018E5B
		public unsafe NativeArray<T> ToArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_Queue->ToArray(allocator);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0001AC69 File Offset: 0x00018E69
		public unsafe void Clear()
		{
			this.m_Queue->Clear();
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0001AC76 File Offset: 0x00018E76
		public unsafe readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Queue != null && this.m_Queue->IsCreated;
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0001AC8F File Offset: 0x00018E8F
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			UnsafeQueue<T>.Free(this.m_Queue);
			this.m_Queue = null;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0001ACB0 File Offset: 0x00018EB0
		public unsafe JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new NativeQueueDisposeJob
			{
				Data = new NativeQueueDispose
				{
					m_QueueData = (UnsafeQueue<int>*)this.m_Queue
				}
			}.Schedule(inputDeps);
			this.m_Queue = null;
			return jobHandle;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0001ACFB File Offset: 0x00018EFB
		public NativeQueue<T>.ReadOnly AsReadOnly()
		{
			return new NativeQueue<T>.ReadOnly(ref this);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0001AD04 File Offset: 0x00018F04
		public unsafe NativeQueue<T>.ParallelWriter AsParallelWriter()
		{
			NativeQueue<T>.ParallelWriter parallelWriter;
			parallelWriter.unsafeWriter = this.m_Queue->AsParallelWriter();
			return parallelWriter;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void CheckRead()
		{
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckWrite()
		{
		}

		// Token: 0x040003E1 RID: 993
		[NativeDisableUnsafePtrRestriction]
		private unsafe UnsafeQueue<T>* m_Queue;

		// Token: 0x020000B6 RID: 182
		[NativeContainer]
		[NativeContainerIsReadOnly]
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x060008D4 RID: 2260 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x060008D5 RID: 2261 RVA: 0x0001AD24 File Offset: 0x00018F24
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x060008D6 RID: 2262 RVA: 0x0001AD31 File Offset: 0x00018F31
			public void Reset()
			{
				this.m_Enumerator.Reset();
			}

			// Token: 0x170000FF RID: 255
			// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0001AD3E File Offset: 0x00018F3E
			public T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Enumerator.Current;
				}
			}

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0001AD4B File Offset: 0x00018F4B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040003E2 RID: 994
			internal UnsafeQueue<T>.Enumerator m_Enumerator;
		}

		// Token: 0x020000B7 RID: 183
		[NativeContainer]
		[NativeContainerIsReadOnly]
		public struct ReadOnly : IEnumerable<T>, IEnumerable
		{
			// Token: 0x060008D9 RID: 2265 RVA: 0x0001AD58 File Offset: 0x00018F58
			internal unsafe ReadOnly(ref NativeQueue<T> data)
			{
				this.m_ReadOnly = new UnsafeQueue<T>.ReadOnly(ref *data.m_Queue);
			}

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x060008DA RID: 2266 RVA: 0x0001AD6B File Offset: 0x00018F6B
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_ReadOnly.IsCreated;
				}
			}

			// Token: 0x060008DB RID: 2267 RVA: 0x0001AD78 File Offset: 0x00018F78
			public readonly bool IsEmpty()
			{
				return this.m_ReadOnly.IsEmpty();
			}

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x060008DC RID: 2268 RVA: 0x0001AD85 File Offset: 0x00018F85
			public readonly int Count
			{
				get
				{
					return this.m_ReadOnly.Count;
				}
			}

			// Token: 0x17000103 RID: 259
			public readonly T this[int index]
			{
				get
				{
					return this.m_ReadOnly[index];
				}
			}

			// Token: 0x060008DE RID: 2270 RVA: 0x0001ADA0 File Offset: 0x00018FA0
			public readonly NativeQueue<T>.Enumerator GetEnumerator()
			{
				return new NativeQueue<T>.Enumerator
				{
					m_Enumerator = this.m_ReadOnly.GetEnumerator()
				};
			}

			// Token: 0x060008DF RID: 2271 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E0 RID: 2272 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E1 RID: 2273 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private readonly void CheckRead()
			{
			}

			// Token: 0x040003E3 RID: 995
			private UnsafeQueue<T>.ReadOnly m_ReadOnly;
		}

		// Token: 0x020000B8 RID: 184
		[NativeContainer]
		[NativeContainerIsAtomicWriteOnly]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ParallelWriter
		{
			// Token: 0x060008E2 RID: 2274 RVA: 0x0001ADC8 File Offset: 0x00018FC8
			public void Enqueue(T value)
			{
				this.unsafeWriter.Enqueue(value);
			}

			// Token: 0x060008E3 RID: 2275 RVA: 0x0001ADD6 File Offset: 0x00018FD6
			public void Enqueue(T value, int threadIndexOverride)
			{
				this.unsafeWriter.Enqueue(value, threadIndexOverride);
			}

			// Token: 0x040003E4 RID: 996
			internal UnsafeQueue<T>.ParallelWriter unsafeWriter;
		}
	}
}
