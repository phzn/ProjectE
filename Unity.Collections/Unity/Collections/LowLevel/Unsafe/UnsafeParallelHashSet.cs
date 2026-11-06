using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000130 RID: 304
	[DebuggerTypeProxy(typeof(UnsafeParallelHashSetDebuggerTypeProxy<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct UnsafeParallelHashSet<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable, IEnumerable<T>, IEnumerable where T : struct, ValueType, IEquatable<T>
	{
		// Token: 0x06000C9E RID: 3230 RVA: 0x00025DF0 File Offset: 0x00023FF0
		public UnsafeParallelHashSet(int capacity, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_Data = new UnsafeParallelHashMap<T, bool>(capacity, allocator);
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x00025DFF File Offset: 0x00023FFF
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.IsEmpty;
			}
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00025E0C File Offset: 0x0002400C
		public int Count()
		{
			return this.m_Data.Count();
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x00025E19 File Offset: 0x00024019
		// (set) Token: 0x06000CA2 RID: 3234 RVA: 0x00025E26 File Offset: 0x00024026
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.m_Data.Capacity;
			}
			set
			{
				this.m_Data.Capacity = value;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x00025E34 File Offset: 0x00024034
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.IsCreated;
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00025E41 File Offset: 0x00024041
		public void Dispose()
		{
			this.m_Data.Dispose();
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00025E4E File Offset: 0x0002404E
		public JobHandle Dispose(JobHandle inputDeps)
		{
			return this.m_Data.Dispose(inputDeps);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00025E5C File Offset: 0x0002405C
		public void Clear()
		{
			this.m_Data.Clear();
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x00025E69 File Offset: 0x00024069
		public bool Add(T item)
		{
			return this.m_Data.TryAdd(item, false);
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00025E78 File Offset: 0x00024078
		public bool Remove(T item)
		{
			return this.m_Data.Remove(item);
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00025E86 File Offset: 0x00024086
		public bool Contains(T item)
		{
			return this.m_Data.ContainsKey(item);
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00025E94 File Offset: 0x00024094
		public NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_Data.GetKeyArray(allocator);
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00025EA4 File Offset: 0x000240A4
		public UnsafeParallelHashSet<T>.ParallelWriter AsParallelWriter()
		{
			return new UnsafeParallelHashSet<T>.ParallelWriter
			{
				m_Data = this.m_Data.AsParallelWriter()
			};
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00025ECC File Offset: 0x000240CC
		public UnsafeParallelHashSet<T>.Enumerator GetEnumerator()
		{
			return new UnsafeParallelHashSet<T>.Enumerator
			{
				m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_Data.m_Buffer)
			};
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00025EF9 File Offset: 0x000240F9
		public UnsafeParallelHashSet<T>.ReadOnly AsReadOnly()
		{
			return new UnsafeParallelHashSet<T>.ReadOnly(ref this);
		}

		// Token: 0x0400050C RID: 1292
		internal UnsafeParallelHashMap<T, bool> m_Data;

		// Token: 0x02000131 RID: 305
		[NativeContainerIsAtomicWriteOnly]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ParallelWriter
		{
			// Token: 0x17000182 RID: 386
			// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x00025F01 File Offset: 0x00024101
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.Capacity;
				}
			}

			// Token: 0x06000CB1 RID: 3249 RVA: 0x00025F0E File Offset: 0x0002410E
			public bool Add(T item)
			{
				return this.m_Data.TryAdd(item, false);
			}

			// Token: 0x06000CB2 RID: 3250 RVA: 0x00025F1D File Offset: 0x0002411D
			public bool Add(T item, int threadIndexOverride)
			{
				return this.m_Data.TryAdd(item, false, threadIndexOverride);
			}

			// Token: 0x0400050D RID: 1293
			internal UnsafeParallelHashMap<T, bool>.ParallelWriter m_Data;
		}

		// Token: 0x02000132 RID: 306
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06000CB3 RID: 3251 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000CB4 RID: 3252 RVA: 0x00025F2D File Offset: 0x0002412D
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000CB5 RID: 3253 RVA: 0x00025F3A File Offset: 0x0002413A
			public void Reset()
			{
				this.m_Enumerator.Reset();
			}

			// Token: 0x17000183 RID: 387
			// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00025F47 File Offset: 0x00024147
			public T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Enumerator.GetCurrentKey<T>();
				}
			}

			// Token: 0x17000184 RID: 388
			// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00025F54 File Offset: 0x00024154
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0400050E RID: 1294
			internal UnsafeParallelHashMapDataEnumerator m_Enumerator;
		}

		// Token: 0x02000133 RID: 307
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ReadOnly : IEnumerable<T>, IEnumerable
		{
			// Token: 0x06000CB8 RID: 3256 RVA: 0x00025F61 File Offset: 0x00024161
			internal ReadOnly(ref UnsafeParallelHashSet<T> data)
			{
				this.m_Data = data.m_Data;
			}

			// Token: 0x17000185 RID: 389
			// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00025F6F File Offset: 0x0002416F
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.IsCreated;
				}
			}

			// Token: 0x17000186 RID: 390
			// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00025F7C File Offset: 0x0002417C
			public readonly bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return !this.m_Data.IsCreated || this.m_Data.IsEmpty;
				}
			}

			// Token: 0x06000CBB RID: 3259 RVA: 0x00025F98 File Offset: 0x00024198
			public readonly int Count()
			{
				return this.m_Data.Count();
			}

			// Token: 0x17000187 RID: 391
			// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00025FA5 File Offset: 0x000241A5
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.Capacity;
				}
			}

			// Token: 0x06000CBD RID: 3261 RVA: 0x00025FB4 File Offset: 0x000241B4
			public readonly bool Contains(T item)
			{
				UnsafeParallelHashMap<T, bool> data = this.m_Data;
				return data.ContainsKey(item);
			}

			// Token: 0x06000CBE RID: 3262 RVA: 0x00025FD0 File Offset: 0x000241D0
			public readonly NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
			{
				UnsafeParallelHashMap<T, bool> data = this.m_Data;
				return data.GetKeyArray(allocator);
			}

			// Token: 0x06000CBF RID: 3263 RVA: 0x00025FEC File Offset: 0x000241EC
			public readonly UnsafeParallelHashSet<T>.Enumerator GetEnumerator()
			{
				return new UnsafeParallelHashSet<T>.Enumerator
				{
					m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_Data.m_Buffer)
				};
			}

			// Token: 0x06000CC0 RID: 3264 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000CC1 RID: 3265 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x0400050F RID: 1295
			internal UnsafeParallelHashMap<T, bool> m_Data;
		}
	}
}
