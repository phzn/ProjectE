using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000A1 RID: 161
	[NativeContainer]
	[DebuggerDisplay("Count = {m_HashMapData.Count()}, Capacity = {m_HashMapData.Capacity}, IsCreated = {m_HashMapData.IsCreated}, IsEmpty = {IsEmpty}")]
	[DebuggerTypeProxy(typeof(NativeParallelHashMapDebuggerTypeProxy<, >))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
	{
		typeof(int),
		typeof(int)
	})]
	public struct NativeParallelHashMap<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> : INativeDisposable, IDisposable, IEnumerable<KeyValue<TKey, TValue>>, IEnumerable where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x060007DC RID: 2012 RVA: 0x00018526 File Offset: 0x00016726
		public NativeParallelHashMap(int capacity, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_HashMapData = new UnsafeParallelHashMap<TKey, TValue>(capacity, allocator);
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00018535 File Offset: 0x00016735
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return !this.IsCreated || this.m_HashMapData.IsEmpty;
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001854C File Offset: 0x0001674C
		public int Count()
		{
			return this.m_HashMapData.Count();
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x00018559 File Offset: 0x00016759
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x00018566 File Offset: 0x00016766
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.m_HashMapData.Capacity;
			}
			set
			{
				this.m_HashMapData.Capacity = value;
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00018574 File Offset: 0x00016774
		public void Clear()
		{
			this.m_HashMapData.Clear();
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00018581 File Offset: 0x00016781
		public bool TryAdd(TKey key, TValue item)
		{
			return UnsafeParallelHashMapBase<TKey, TValue>.TryAdd(this.m_HashMapData.m_Buffer, key, item, false, this.m_HashMapData.m_AllocatorLabel);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000185A1 File Offset: 0x000167A1
		public void Add(TKey key, TValue item)
		{
			UnsafeParallelHashMapBase<TKey, TValue>.TryAdd(this.m_HashMapData.m_Buffer, key, item, false, this.m_HashMapData.m_AllocatorLabel);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000185C2 File Offset: 0x000167C2
		public bool Remove(TKey key)
		{
			return this.m_HashMapData.Remove(key);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x000185D0 File Offset: 0x000167D0
		public bool TryGetValue(TKey key, out TValue item)
		{
			return this.m_HashMapData.TryGetValue(key, out item);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x000185DF File Offset: 0x000167DF
		public bool ContainsKey(TKey key)
		{
			return this.m_HashMapData.ContainsKey(key);
		}

		// Token: 0x170000DA RID: 218
		public TValue this[TKey key]
		{
			get
			{
				TValue tvalue;
				if (this.m_HashMapData.TryGetValue(key, out tvalue))
				{
					return tvalue;
				}
				return default(TValue);
			}
			set
			{
				this.m_HashMapData[key] = value;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00018627 File Offset: 0x00016827
		public readonly bool IsCreated
		{
			get
			{
				return this.m_HashMapData.IsCreated;
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00018634 File Offset: 0x00016834
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			this.m_HashMapData.Dispose();
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001864C File Offset: 0x0001684C
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
					m_Buffer = this.m_HashMapData.m_Buffer,
					m_AllocatorLabel = this.m_HashMapData.m_AllocatorLabel
				}
			}.Schedule(inputDeps);
			this.m_HashMapData.m_Buffer = null;
			return jobHandle;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000186B3 File Offset: 0x000168B3
		public NativeArray<TKey> GetKeyArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_HashMapData.GetKeyArray(allocator);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x000186C1 File Offset: 0x000168C1
		public NativeArray<TValue> GetValueArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_HashMapData.GetValueArray(allocator);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x000186CF File Offset: 0x000168CF
		public NativeKeyValueArrays<TKey, TValue> GetKeyValueArrays(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_HashMapData.GetKeyValueArrays(allocator);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x000186E0 File Offset: 0x000168E0
		public NativeParallelHashMap<TKey, TValue>.ParallelWriter AsParallelWriter()
		{
			NativeParallelHashMap<TKey, TValue>.ParallelWriter parallelWriter;
			parallelWriter.m_Writer = this.m_HashMapData.AsParallelWriter();
			return parallelWriter;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00018700 File Offset: 0x00016900
		public NativeParallelHashMap<TKey, TValue>.ReadOnly AsReadOnly()
		{
			return new NativeParallelHashMap<TKey, TValue>.ReadOnly(this.m_HashMapData);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00018710 File Offset: 0x00016910
		public NativeParallelHashMap<TKey, TValue>.Enumerator GetEnumerator()
		{
			return new NativeParallelHashMap<TKey, TValue>.Enumerator
			{
				m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_HashMapData.m_Buffer)
			};
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<KeyValue<TKey, TValue>> IEnumerable<KeyValue<TKey, TValue>>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void CheckRead()
		{
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckWrite()
		{
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0001873D File Offset: 0x0001693D
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ThrowKeyNotPresent(TKey key)
		{
			throw new ArgumentException(string.Format("Key: {0} is not present in the NativeParallelHashMap.", key));
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00018754 File Offset: 0x00016954
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ThrowKeyAlreadyAdded(TKey key)
		{
			throw new ArgumentException("An item with the same key has already been added", "key");
		}

		// Token: 0x040003CA RID: 970
		internal UnsafeParallelHashMap<TKey, TValue> m_HashMapData;

		// Token: 0x020000A2 RID: 162
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
			// Token: 0x060007F8 RID: 2040 RVA: 0x00018765 File Offset: 0x00016965
			internal ReadOnly(UnsafeParallelHashMap<TKey, TValue> hashMapData)
			{
				this.m_HashMapData = hashMapData;
			}

			// Token: 0x170000DC RID: 220
			// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0001876E File Offset: 0x0001696E
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_HashMapData.IsCreated;
				}
			}

			// Token: 0x170000DD RID: 221
			// (get) Token: 0x060007FA RID: 2042 RVA: 0x0001877B File Offset: 0x0001697B
			public readonly bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return !this.IsCreated || this.m_HashMapData.IsEmpty;
				}
			}

			// Token: 0x060007FB RID: 2043 RVA: 0x00018792 File Offset: 0x00016992
			public readonly int Count()
			{
				return this.m_HashMapData.Count();
			}

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x060007FC RID: 2044 RVA: 0x0001879F File Offset: 0x0001699F
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_HashMapData.Capacity;
				}
			}

			// Token: 0x060007FD RID: 2045 RVA: 0x000187AC File Offset: 0x000169AC
			public readonly bool TryGetValue(TKey key, out TValue item)
			{
				UnsafeParallelHashMap<TKey, TValue> hashMapData = this.m_HashMapData;
				return hashMapData.TryGetValue(key, out item);
			}

			// Token: 0x060007FE RID: 2046 RVA: 0x000187CC File Offset: 0x000169CC
			public readonly bool ContainsKey(TKey key)
			{
				UnsafeParallelHashMap<TKey, TValue> hashMapData = this.m_HashMapData;
				return hashMapData.ContainsKey(key);
			}

			// Token: 0x170000DF RID: 223
			public readonly TValue this[TKey key]
			{
				get
				{
					UnsafeParallelHashMap<TKey, TValue> hashMapData = this.m_HashMapData;
					TValue tvalue;
					if (hashMapData.TryGetValue(key, out tvalue))
					{
						return tvalue;
					}
					return default(TValue);
				}
			}

			// Token: 0x06000800 RID: 2048 RVA: 0x00018814 File Offset: 0x00016A14
			public readonly NativeArray<TKey> GetKeyArray(AllocatorManager.AllocatorHandle allocator)
			{
				UnsafeParallelHashMap<TKey, TValue> hashMapData = this.m_HashMapData;
				return hashMapData.GetKeyArray(allocator);
			}

			// Token: 0x06000801 RID: 2049 RVA: 0x00018830 File Offset: 0x00016A30
			public readonly NativeArray<TValue> GetValueArray(AllocatorManager.AllocatorHandle allocator)
			{
				UnsafeParallelHashMap<TKey, TValue> hashMapData = this.m_HashMapData;
				return hashMapData.GetValueArray(allocator);
			}

			// Token: 0x06000802 RID: 2050 RVA: 0x0001884C File Offset: 0x00016A4C
			public readonly NativeKeyValueArrays<TKey, TValue> GetKeyValueArrays(AllocatorManager.AllocatorHandle allocator)
			{
				UnsafeParallelHashMap<TKey, TValue> hashMapData = this.m_HashMapData;
				return hashMapData.GetKeyValueArrays(allocator);
			}

			// Token: 0x06000803 RID: 2051 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private readonly void CheckRead()
			{
			}

			// Token: 0x06000804 RID: 2052 RVA: 0x0001873D File Offset: 0x0001693D
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private readonly void ThrowKeyNotPresent(TKey key)
			{
				throw new ArgumentException(string.Format("Key: {0} is not present in the NativeParallelHashMap.", key));
			}

			// Token: 0x06000805 RID: 2053 RVA: 0x00018868 File Offset: 0x00016A68
			public readonly NativeParallelHashMap<TKey, TValue>.Enumerator GetEnumerator()
			{
				return new NativeParallelHashMap<TKey, TValue>.Enumerator
				{
					m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_HashMapData.m_Buffer)
				};
			}

			// Token: 0x06000806 RID: 2054 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<KeyValue<TKey, TValue>> IEnumerable<KeyValue<TKey, TValue>>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000807 RID: 2055 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x040003CB RID: 971
			internal UnsafeParallelHashMap<TKey, TValue> m_HashMapData;
		}

		// Token: 0x020000A3 RID: 163
		[NativeContainer]
		[NativeContainerIsAtomicWriteOnly]
		[DebuggerDisplay("Capacity = {m_Writer.Capacity}")]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public struct ParallelWriter
		{
			// Token: 0x170000E0 RID: 224
			// (get) Token: 0x06000808 RID: 2056 RVA: 0x00018895 File Offset: 0x00016A95
			public int ThreadIndex
			{
				get
				{
					return this.m_Writer.m_ThreadIndex;
				}
			}

			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x06000809 RID: 2057 RVA: 0x00018895 File Offset: 0x00016A95
			[Obsolete("'m_ThreadIndex' has been deprecated; use 'ThreadIndex' instead. (UnityUpgradable) -> ThreadIndex")]
			public int m_ThreadIndex
			{
				get
				{
					return this.m_Writer.m_ThreadIndex;
				}
			}

			// Token: 0x170000E2 RID: 226
			// (get) Token: 0x0600080A RID: 2058 RVA: 0x000188A2 File Offset: 0x00016AA2
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Writer.Capacity;
				}
			}

			// Token: 0x0600080B RID: 2059 RVA: 0x000188AF File Offset: 0x00016AAF
			public bool TryAdd(TKey key, TValue item)
			{
				return this.m_Writer.TryAdd(key, item);
			}

			// Token: 0x0600080C RID: 2060 RVA: 0x000188BE File Offset: 0x00016ABE
			public bool TryAdd(TKey key, TValue item, int threadIndexOverride)
			{
				return this.m_Writer.TryAdd(key, item, threadIndexOverride);
			}

			// Token: 0x040003CC RID: 972
			internal UnsafeParallelHashMap<TKey, TValue>.ParallelWriter m_Writer;
		}

		// Token: 0x020000A4 RID: 164
		[NativeContainer]
		[NativeContainerIsReadOnly]
		public struct Enumerator : IEnumerator<KeyValue<TKey, TValue>>, IEnumerator, IDisposable
		{
			// Token: 0x0600080D RID: 2061 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x0600080E RID: 2062 RVA: 0x000188CE File Offset: 0x00016ACE
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x0600080F RID: 2063 RVA: 0x000188DB File Offset: 0x00016ADB
			public void Reset()
			{
				this.m_Enumerator.Reset();
			}

			// Token: 0x170000E3 RID: 227
			// (get) Token: 0x06000810 RID: 2064 RVA: 0x000188E8 File Offset: 0x00016AE8
			public KeyValue<TKey, TValue> Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Enumerator.GetCurrent<TKey, TValue>();
				}
			}

			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x06000811 RID: 2065 RVA: 0x000188F5 File Offset: 0x00016AF5
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040003CD RID: 973
			internal UnsafeParallelHashMapDataEnumerator m_Enumerator;
		}
	}
}
