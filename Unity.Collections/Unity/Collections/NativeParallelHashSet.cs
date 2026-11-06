using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000A7 RID: 167
	[DebuggerTypeProxy(typeof(NativeParallelHashSetDebuggerTypeProxy<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct NativeParallelHashSet<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable, IEnumerable<T>, IEnumerable where T : struct, ValueType, IEquatable<T>
	{
		// Token: 0x0600081B RID: 2075 RVA: 0x00018A9C File Offset: 0x00016C9C
		public NativeParallelHashSet(int capacity, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_Data = new NativeParallelHashMap<T, bool>(capacity, allocator);
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x00018AAB File Offset: 0x00016CAB
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.IsEmpty;
			}
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00018AB8 File Offset: 0x00016CB8
		public int Count()
		{
			return this.m_Data.Count();
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x00018AC5 File Offset: 0x00016CC5
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x00018AD2 File Offset: 0x00016CD2
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00018AE0 File Offset: 0x00016CE0
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.IsCreated;
			}
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00018AED File Offset: 0x00016CED
		public void Dispose()
		{
			this.m_Data.Dispose();
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00018AFA File Offset: 0x00016CFA
		public JobHandle Dispose(JobHandle inputDeps)
		{
			return this.m_Data.Dispose(inputDeps);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00018B08 File Offset: 0x00016D08
		public void Clear()
		{
			this.m_Data.Clear();
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00018B15 File Offset: 0x00016D15
		public bool Add(T item)
		{
			return this.m_Data.TryAdd(item, false);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00018B24 File Offset: 0x00016D24
		public bool Remove(T item)
		{
			return this.m_Data.Remove(item);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00018B32 File Offset: 0x00016D32
		public bool Contains(T item)
		{
			return this.m_Data.ContainsKey(item);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00018B40 File Offset: 0x00016D40
		public NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_Data.GetKeyArray(allocator);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00018B50 File Offset: 0x00016D50
		public NativeParallelHashSet<T>.ParallelWriter AsParallelWriter()
		{
			NativeParallelHashSet<T>.ParallelWriter parallelWriter;
			parallelWriter.m_Data = this.m_Data.AsParallelWriter();
			return parallelWriter;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00018B70 File Offset: 0x00016D70
		public NativeParallelHashSet<T>.Enumerator GetEnumerator()
		{
			return new NativeParallelHashSet<T>.Enumerator
			{
				m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_Data.m_HashMapData.m_Buffer)
			};
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00018BA2 File Offset: 0x00016DA2
		public NativeParallelHashSet<T>.ReadOnly AsReadOnly()
		{
			return new NativeParallelHashSet<T>.ReadOnly(ref this);
		}

		// Token: 0x040003CF RID: 975
		internal NativeParallelHashMap<T, bool> m_Data;

		// Token: 0x020000A8 RID: 168
		[NativeContainerIsAtomicWriteOnly]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ParallelWriter
		{
			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x0600082D RID: 2093 RVA: 0x00018BAA File Offset: 0x00016DAA
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.Capacity;
				}
			}

			// Token: 0x0600082E RID: 2094 RVA: 0x00018BB7 File Offset: 0x00016DB7
			public bool Add(T item)
			{
				return this.m_Data.TryAdd(item, false);
			}

			// Token: 0x0600082F RID: 2095 RVA: 0x00018BC6 File Offset: 0x00016DC6
			public bool Add(T item, int threadIndexOverride)
			{
				return this.m_Data.TryAdd(item, false, threadIndexOverride);
			}

			// Token: 0x040003D0 RID: 976
			internal NativeParallelHashMap<T, bool>.ParallelWriter m_Data;
		}

		// Token: 0x020000A9 RID: 169
		[NativeContainer]
		[NativeContainerIsReadOnly]
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06000830 RID: 2096 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000831 RID: 2097 RVA: 0x00018BD6 File Offset: 0x00016DD6
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000832 RID: 2098 RVA: 0x00018BE3 File Offset: 0x00016DE3
			public void Reset()
			{
				this.m_Enumerator.Reset();
			}

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x06000833 RID: 2099 RVA: 0x00018BF0 File Offset: 0x00016DF0
			public T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Enumerator.GetCurrentKey<T>();
				}
			}

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x06000834 RID: 2100 RVA: 0x00018BFD File Offset: 0x00016DFD
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040003D1 RID: 977
			internal UnsafeParallelHashMapDataEnumerator m_Enumerator;
		}

		// Token: 0x020000AA RID: 170
		[NativeContainer]
		[NativeContainerIsReadOnly]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ReadOnly : IEnumerable<T>, IEnumerable
		{
			// Token: 0x06000835 RID: 2101 RVA: 0x00018C0A File Offset: 0x00016E0A
			internal ReadOnly(ref NativeParallelHashSet<T> data)
			{
				this.m_Data = data.m_Data.m_HashMapData;
			}

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x06000836 RID: 2102 RVA: 0x00018C1D File Offset: 0x00016E1D
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.IsCreated;
				}
			}

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x06000837 RID: 2103 RVA: 0x00018C2A File Offset: 0x00016E2A
			public readonly bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return !this.IsCreated || this.m_Data.IsEmpty;
				}
			}

			// Token: 0x06000838 RID: 2104 RVA: 0x00018C41 File Offset: 0x00016E41
			public readonly int Count()
			{
				return this.m_Data.Count();
			}

			// Token: 0x170000EE RID: 238
			// (get) Token: 0x06000839 RID: 2105 RVA: 0x00018C4E File Offset: 0x00016E4E
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.Capacity;
				}
			}

			// Token: 0x0600083A RID: 2106 RVA: 0x00018C5C File Offset: 0x00016E5C
			public readonly bool Contains(T item)
			{
				UnsafeParallelHashMap<T, bool> data = this.m_Data;
				return data.ContainsKey(item);
			}

			// Token: 0x0600083B RID: 2107 RVA: 0x00018C78 File Offset: 0x00016E78
			public readonly NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
			{
				UnsafeParallelHashMap<T, bool> data = this.m_Data;
				return data.GetKeyArray(allocator);
			}

			// Token: 0x0600083C RID: 2108 RVA: 0x00018C94 File Offset: 0x00016E94
			public readonly NativeParallelHashSet<T>.Enumerator GetEnumerator()
			{
				return new NativeParallelHashSet<T>.Enumerator
				{
					m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_Data.m_Buffer)
				};
			}

			// Token: 0x0600083D RID: 2109 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600083E RID: 2110 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600083F RID: 2111 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private readonly void CheckRead()
			{
			}

			// Token: 0x040003D2 RID: 978
			internal UnsafeParallelHashMap<T, bool> m_Data;
		}
	}
}
