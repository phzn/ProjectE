using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000AE RID: 174
	[NativeContainer]
	[DebuggerTypeProxy(typeof(NativeParallelMultiHashMapDebuggerTypeProxy<, >))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
	{
		typeof(int),
		typeof(int)
	})]
	public struct NativeParallelMultiHashMap<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> : INativeDisposable, IDisposable, IEnumerable<KeyValue<TKey, TValue>>, IEnumerable where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x06000885 RID: 2181 RVA: 0x0001A658 File Offset: 0x00018858
		public NativeParallelMultiHashMap(int capacity, AllocatorManager.AllocatorHandle allocator)
		{
			this = default(NativeParallelMultiHashMap<TKey, TValue>);
			this.Initialize<AllocatorManager.AllocatorHandle>(capacity, ref allocator);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0001A66A File Offset: 0x0001886A
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(AllocatorManager.AllocatorHandle) })]
		internal void Initialize<[global::System.Runtime.CompilerServices.IsUnmanaged] U>(int capacity, ref U allocator) where U : struct, ValueType, AllocatorManager.IAllocator
		{
			this.m_MultiHashMapData = new UnsafeParallelMultiHashMap<TKey, TValue>(capacity, allocator.Handle);
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x0001A684 File Offset: 0x00018884
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_MultiHashMapData.IsEmpty;
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001A691 File Offset: 0x00018891
		public readonly int Count()
		{
			return this.m_MultiHashMapData.Count();
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x0001A69E File Offset: 0x0001889E
		// (set) Token: 0x0600088A RID: 2186 RVA: 0x0001A6AB File Offset: 0x000188AB
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.m_MultiHashMapData.Capacity;
			}
			set
			{
				this.m_MultiHashMapData.Capacity = value;
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0001A6B9 File Offset: 0x000188B9
		public void Clear()
		{
			this.m_MultiHashMapData.Clear();
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0001A6C6 File Offset: 0x000188C6
		public void Add(TKey key, TValue item)
		{
			this.m_MultiHashMapData.Add(key, item);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001A6D5 File Offset: 0x000188D5
		public int Remove(TKey key)
		{
			return this.m_MultiHashMapData.Remove(key);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001A6E3 File Offset: 0x000188E3
		public void Remove(NativeParallelMultiHashMapIterator<TKey> it)
		{
			this.m_MultiHashMapData.Remove(it);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0001A6F1 File Offset: 0x000188F1
		public bool TryGetFirstValue(TKey key, out TValue item, out NativeParallelMultiHashMapIterator<TKey> it)
		{
			return this.m_MultiHashMapData.TryGetFirstValue(key, out item, out it);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0001A701 File Offset: 0x00018901
		public bool TryGetNextValue(out TValue item, ref NativeParallelMultiHashMapIterator<TKey> it)
		{
			return this.m_MultiHashMapData.TryGetNextValue(out item, ref it);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0001A710 File Offset: 0x00018910
		public bool ContainsKey(TKey key)
		{
			TValue tvalue;
			NativeParallelMultiHashMapIterator<TKey> nativeParallelMultiHashMapIterator;
			return this.TryGetFirstValue(key, out tvalue, out nativeParallelMultiHashMapIterator);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0001A728 File Offset: 0x00018928
		public int CountValuesForKey(TKey key)
		{
			TValue tvalue;
			NativeParallelMultiHashMapIterator<TKey> nativeParallelMultiHashMapIterator;
			if (!this.TryGetFirstValue(key, out tvalue, out nativeParallelMultiHashMapIterator))
			{
				return 0;
			}
			int num = 1;
			while (this.TryGetNextValue(out tvalue, ref nativeParallelMultiHashMapIterator))
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0001A759 File Offset: 0x00018959
		public bool SetValue(TValue item, NativeParallelMultiHashMapIterator<TKey> it)
		{
			return this.m_MultiHashMapData.SetValue(item, it);
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x0001A768 File Offset: 0x00018968
		public readonly bool IsCreated
		{
			get
			{
				return this.m_MultiHashMapData.IsCreated;
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001A775 File Offset: 0x00018975
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			this.m_MultiHashMapData.Dispose();
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001A78C File Offset: 0x0001898C
		public JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new UnsafeParallelHashMapDataDisposeJob
			{
				Data = new UnsafeParallelHashMapDataDispose
				{
					m_Buffer = this.m_MultiHashMapData.m_Buffer,
					m_AllocatorLabel = this.m_MultiHashMapData.m_AllocatorLabel
				}
			}.Schedule(inputDeps);
			this.m_MultiHashMapData.m_Buffer = null;
			return jobHandle;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001A7F3 File Offset: 0x000189F3
		public NativeArray<TKey> GetKeyArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_MultiHashMapData.GetKeyArray(allocator);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001A801 File Offset: 0x00018A01
		public NativeArray<TValue> GetValueArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_MultiHashMapData.GetValueArray(allocator);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001A80F File Offset: 0x00018A0F
		public NativeKeyValueArrays<TKey, TValue> GetKeyValueArrays(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_MultiHashMapData.GetKeyValueArrays(allocator);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0001A820 File Offset: 0x00018A20
		public NativeParallelMultiHashMap<TKey, TValue>.ParallelWriter AsParallelWriter()
		{
			NativeParallelMultiHashMap<TKey, TValue>.ParallelWriter parallelWriter;
			parallelWriter.m_Writer = this.m_MultiHashMapData.AsParallelWriter();
			return parallelWriter;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001A840 File Offset: 0x00018A40
		public NativeParallelMultiHashMap<TKey, TValue>.Enumerator GetValuesForKey(TKey key)
		{
			return new NativeParallelMultiHashMap<TKey, TValue>.Enumerator
			{
				hashmap = this,
				key = key,
				isFirst = 1
			};
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001A874 File Offset: 0x00018A74
		public NativeParallelMultiHashMap<TKey, TValue>.KeyValueEnumerator GetEnumerator()
		{
			return new NativeParallelMultiHashMap<TKey, TValue>.KeyValueEnumerator
			{
				m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_MultiHashMapData.m_Buffer)
			};
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<KeyValue<TKey, TValue>> IEnumerable<KeyValue<TKey, TValue>>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001A8A1 File Offset: 0x00018AA1
		public NativeParallelMultiHashMap<TKey, TValue>.ReadOnly AsReadOnly()
		{
			return new NativeParallelMultiHashMap<TKey, TValue>.ReadOnly(this.m_MultiHashMapData);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void CheckRead()
		{
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckWrite()
		{
		}

		// Token: 0x040003D7 RID: 983
		internal UnsafeParallelMultiHashMap<TKey, TValue> m_MultiHashMapData;

		// Token: 0x020000AF RID: 175
		[NativeContainer]
		[NativeContainerIsAtomicWriteOnly]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public struct ParallelWriter
		{
			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x060008A2 RID: 2210 RVA: 0x0001A8AE File Offset: 0x00018AAE
			public int m_ThreadIndex
			{
				get
				{
					return this.m_Writer.m_ThreadIndex;
				}
			}

			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0001A8BB File Offset: 0x00018ABB
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Writer.Capacity;
				}
			}

			// Token: 0x060008A4 RID: 2212 RVA: 0x0001A8C8 File Offset: 0x00018AC8
			public void Add(TKey key, TValue item)
			{
				this.m_Writer.Add(key, item);
			}

			// Token: 0x060008A5 RID: 2213 RVA: 0x0001A8D7 File Offset: 0x00018AD7
			public void Add(TKey key, TValue item, int threadIndexOverride)
			{
				this.m_Writer.Add(key, item, threadIndexOverride);
			}

			// Token: 0x040003D8 RID: 984
			internal UnsafeParallelMultiHashMap<TKey, TValue>.ParallelWriter m_Writer;
		}

		// Token: 0x020000B0 RID: 176
		public struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
		{
			// Token: 0x060008A6 RID: 2214 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x060008A7 RID: 2215 RVA: 0x0001A8E8 File Offset: 0x00018AE8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				if (this.isFirst == 1)
				{
					this.isFirst = 0;
					return this.hashmap.TryGetFirstValue(this.key, out this.value, out this.iterator);
				}
				return this.hashmap.TryGetNextValue(out this.value, ref this.iterator);
			}

			// Token: 0x060008A8 RID: 2216 RVA: 0x0001A93A File Offset: 0x00018B3A
			public void Reset()
			{
				this.isFirst = 1;
			}

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0001A943 File Offset: 0x00018B43
			public TValue Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.value;
				}
			}

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x060008AA RID: 2218 RVA: 0x0001A94B File Offset: 0x00018B4B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060008AB RID: 2219 RVA: 0x0001A958 File Offset: 0x00018B58
			public NativeParallelMultiHashMap<TKey, TValue>.Enumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x040003D9 RID: 985
			internal NativeParallelMultiHashMap<TKey, TValue> hashmap;

			// Token: 0x040003DA RID: 986
			internal TKey key;

			// Token: 0x040003DB RID: 987
			internal byte isFirst;

			// Token: 0x040003DC RID: 988
			private TValue value;

			// Token: 0x040003DD RID: 989
			private NativeParallelMultiHashMapIterator<TKey> iterator;
		}

		// Token: 0x020000B1 RID: 177
		[NativeContainer]
		[NativeContainerIsReadOnly]
		public struct KeyValueEnumerator : IEnumerator<KeyValue<TKey, TValue>>, IEnumerator, IDisposable
		{
			// Token: 0x060008AC RID: 2220 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x060008AD RID: 2221 RVA: 0x0001A960 File Offset: 0x00018B60
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x060008AE RID: 2222 RVA: 0x0001A96D File Offset: 0x00018B6D
			public void Reset()
			{
				this.m_Enumerator.Reset();
			}

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x060008AF RID: 2223 RVA: 0x0001A97C File Offset: 0x00018B7C
			public readonly KeyValue<TKey, TValue> Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					UnsafeParallelHashMapDataEnumerator enumerator = this.m_Enumerator;
					return enumerator.GetCurrent<TKey, TValue>();
				}
			}

			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0001A997 File Offset: 0x00018B97
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040003DE RID: 990
			internal UnsafeParallelHashMapDataEnumerator m_Enumerator;
		}

		// Token: 0x020000B2 RID: 178
		[NativeContainer]
		[NativeContainerIsReadOnly]
		[DebuggerTypeProxy(typeof(NativeParallelHashMapDebuggerTypeProxy<, >))]
		[DebuggerDisplay("Count = {m_HashMapData.Count()}, Capacity = {m_HashMapData.Capacity}, IsCreated = {m_HashMapData.IsCreated}, IsEmpty = {IsEmpty}")]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public struct ReadOnly : IEnumerable<KeyValue<TKey, TValue>>, IEnumerable
		{
			// Token: 0x060008B1 RID: 2225 RVA: 0x0001A9A4 File Offset: 0x00018BA4
			internal ReadOnly(UnsafeParallelMultiHashMap<TKey, TValue> container)
			{
				this.m_MultiHashMapData = container;
			}

			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0001A9AD File Offset: 0x00018BAD
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_MultiHashMapData.IsCreated;
				}
			}

			// Token: 0x170000FA RID: 250
			// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0001A9BA File Offset: 0x00018BBA
			public readonly bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return !this.IsCreated || this.m_MultiHashMapData.IsEmpty;
				}
			}

			// Token: 0x060008B4 RID: 2228 RVA: 0x0001A9D1 File Offset: 0x00018BD1
			public readonly int Count()
			{
				return this.m_MultiHashMapData.Count();
			}

			// Token: 0x170000FB RID: 251
			// (get) Token: 0x060008B5 RID: 2229 RVA: 0x0001A9DE File Offset: 0x00018BDE
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_MultiHashMapData.Capacity;
				}
			}

			// Token: 0x060008B6 RID: 2230 RVA: 0x0001A9EB File Offset: 0x00018BEB
			public readonly bool TryGetFirstValue(TKey key, out TValue item, out NativeParallelMultiHashMapIterator<TKey> it)
			{
				return this.m_MultiHashMapData.TryGetFirstValue(key, out item, out it);
			}

			// Token: 0x060008B7 RID: 2231 RVA: 0x0001A9FB File Offset: 0x00018BFB
			public readonly bool TryGetNextValue(out TValue item, ref NativeParallelMultiHashMapIterator<TKey> it)
			{
				return this.m_MultiHashMapData.TryGetNextValue(out item, ref it);
			}

			// Token: 0x060008B8 RID: 2232 RVA: 0x0001AA0A File Offset: 0x00018C0A
			public readonly bool ContainsKey(TKey key)
			{
				return this.m_MultiHashMapData.ContainsKey(key);
			}

			// Token: 0x060008B9 RID: 2233 RVA: 0x0001AA18 File Offset: 0x00018C18
			public readonly NativeArray<TKey> GetKeyArray(AllocatorManager.AllocatorHandle allocator)
			{
				return this.m_MultiHashMapData.GetKeyArray(allocator);
			}

			// Token: 0x060008BA RID: 2234 RVA: 0x0001AA26 File Offset: 0x00018C26
			public readonly NativeArray<TValue> GetValueArray(AllocatorManager.AllocatorHandle allocator)
			{
				return this.m_MultiHashMapData.GetValueArray(allocator);
			}

			// Token: 0x060008BB RID: 2235 RVA: 0x0001AA34 File Offset: 0x00018C34
			public readonly NativeKeyValueArrays<TKey, TValue> GetKeyValueArrays(AllocatorManager.AllocatorHandle allocator)
			{
				return this.m_MultiHashMapData.GetKeyValueArrays(allocator);
			}

			// Token: 0x060008BC RID: 2236 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private readonly void CheckRead()
			{
			}

			// Token: 0x060008BD RID: 2237 RVA: 0x0001873D File Offset: 0x0001693D
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private readonly void ThrowKeyNotPresent(TKey key)
			{
				throw new ArgumentException(string.Format("Key: {0} is not present in the NativeParallelHashMap.", key));
			}

			// Token: 0x060008BE RID: 2238 RVA: 0x0001AA44 File Offset: 0x00018C44
			public NativeParallelMultiHashMap<TKey, TValue>.KeyValueEnumerator GetEnumerator()
			{
				return new NativeParallelMultiHashMap<TKey, TValue>.KeyValueEnumerator
				{
					m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_MultiHashMapData.m_Buffer)
				};
			}

			// Token: 0x060008BF RID: 2239 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<KeyValue<TKey, TValue>> IEnumerable<KeyValue<TKey, TValue>>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008C0 RID: 2240 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x040003DF RID: 991
			internal UnsafeParallelMultiHashMap<TKey, TValue> m_MultiHashMapData;
		}
	}
}
