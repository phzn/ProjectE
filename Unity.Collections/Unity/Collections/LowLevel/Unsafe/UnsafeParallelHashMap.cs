using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000129 RID: 297
	[DebuggerDisplay("Count = {Count()}, Capacity = {Capacity}, IsCreated = {IsCreated}, IsEmpty = {IsEmpty}")]
	[DebuggerTypeProxy(typeof(UnsafeParallelHashMapDebuggerTypeProxy<, >))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
	{
		typeof(int),
		typeof(int)
	})]
	public struct UnsafeParallelHashMap<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> : INativeDisposable, IDisposable, IEnumerable<KeyValue<TKey, TValue>>, IEnumerable where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x06000C6B RID: 3179 RVA: 0x000258F2 File Offset: 0x00023AF2
		public UnsafeParallelHashMap(int capacity, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_AllocatorLabel = allocator;
			UnsafeParallelHashMapData.AllocateHashMap<TKey, TValue>(capacity, capacity * 2, allocator, out this.m_Buffer);
			this.Clear();
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x00025911 File Offset: 0x00023B11
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Buffer != null;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x00025920 File Offset: 0x00023B20
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return !this.IsCreated || UnsafeParallelHashMapData.IsEmpty(this.m_Buffer);
			}
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00025937 File Offset: 0x00023B37
		public readonly int Count()
		{
			return UnsafeParallelHashMapData.GetCount(this.m_Buffer);
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x00025944 File Offset: 0x00023B44
		// (set) Token: 0x06000C70 RID: 3184 RVA: 0x00025951 File Offset: 0x00023B51
		public unsafe int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.m_Buffer->keyCapacity;
			}
			set
			{
				UnsafeParallelHashMapData.ReallocateHashMap<TKey, TValue>(this.m_Buffer, value, UnsafeParallelHashMapData.GetBucketSize(value), this.m_AllocatorLabel);
			}
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0002596B File Offset: 0x00023B6B
		public void Clear()
		{
			UnsafeParallelHashMapBase<TKey, TValue>.Clear(this.m_Buffer);
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00025978 File Offset: 0x00023B78
		public bool TryAdd(TKey key, TValue item)
		{
			return UnsafeParallelHashMapBase<TKey, TValue>.TryAdd(this.m_Buffer, key, item, false, this.m_AllocatorLabel);
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0002598E File Offset: 0x00023B8E
		public void Add(TKey key, TValue item)
		{
			UnsafeParallelHashMapBase<TKey, TValue>.TryAdd(this.m_Buffer, key, item, false, this.m_AllocatorLabel);
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000259A5 File Offset: 0x00023BA5
		public bool Remove(TKey key)
		{
			return UnsafeParallelHashMapBase<TKey, TValue>.Remove(this.m_Buffer, key, false) != 0;
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x000259B8 File Offset: 0x00023BB8
		public bool TryGetValue(TKey key, out TValue item)
		{
			NativeParallelMultiHashMapIterator<TKey> nativeParallelMultiHashMapIterator;
			return UnsafeParallelHashMapBase<TKey, TValue>.TryGetFirstValueAtomic(this.m_Buffer, key, out item, out nativeParallelMultiHashMapIterator);
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x000259D4 File Offset: 0x00023BD4
		public bool ContainsKey(TKey key)
		{
			TValue tvalue;
			NativeParallelMultiHashMapIterator<TKey> nativeParallelMultiHashMapIterator;
			return UnsafeParallelHashMapBase<TKey, TValue>.TryGetFirstValueAtomic(this.m_Buffer, key, out tvalue, out nativeParallelMultiHashMapIterator);
		}

		// Token: 0x17000175 RID: 373
		public TValue this[TKey key]
		{
			get
			{
				TValue tvalue;
				this.TryGetValue(key, out tvalue);
				return tvalue;
			}
			set
			{
				TValue tvalue;
				NativeParallelMultiHashMapIterator<TKey> nativeParallelMultiHashMapIterator;
				if (UnsafeParallelHashMapBase<TKey, TValue>.TryGetFirstValueAtomic(this.m_Buffer, key, out tvalue, out nativeParallelMultiHashMapIterator))
				{
					UnsafeParallelHashMapBase<TKey, TValue>.SetValue(this.m_Buffer, ref nativeParallelMultiHashMapIterator, ref value);
					return;
				}
				UnsafeParallelHashMapBase<TKey, TValue>.TryAdd(this.m_Buffer, key, value, false, this.m_AllocatorLabel);
			}
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00025A51 File Offset: 0x00023C51
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			UnsafeParallelHashMapData.DeallocateHashMap(this.m_Buffer, this.m_AllocatorLabel);
			this.m_Buffer = null;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00025A78 File Offset: 0x00023C78
		public JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new UnsafeParallelHashMapDisposeJob
			{
				Data = this.m_Buffer,
				Allocator = this.m_AllocatorLabel
			}.Schedule(inputDeps);
			this.m_Buffer = null;
			return jobHandle;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00025AC0 File Offset: 0x00023CC0
		public NativeArray<TKey> GetKeyArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<TKey> nativeArray = CollectionHelper.CreateNativeArray<TKey>(UnsafeParallelHashMapData.GetCount(this.m_Buffer), allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeParallelHashMapData.GetKeyArray<TKey>(this.m_Buffer, nativeArray);
			return nativeArray;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00025AF0 File Offset: 0x00023CF0
		public NativeArray<TValue> GetValueArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<TValue> nativeArray = CollectionHelper.CreateNativeArray<TValue>(UnsafeParallelHashMapData.GetCount(this.m_Buffer), allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeParallelHashMapData.GetValueArray<TValue>(this.m_Buffer, nativeArray);
			return nativeArray;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00025B20 File Offset: 0x00023D20
		public NativeKeyValueArrays<TKey, TValue> GetKeyValueArrays(AllocatorManager.AllocatorHandle allocator)
		{
			NativeKeyValueArrays<TKey, TValue> nativeKeyValueArrays = new NativeKeyValueArrays<TKey, TValue>(UnsafeParallelHashMapData.GetCount(this.m_Buffer), allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeParallelHashMapData.GetKeyValueArrays<TKey, TValue>(this.m_Buffer, nativeKeyValueArrays);
			return nativeKeyValueArrays;
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00025B50 File Offset: 0x00023D50
		public UnsafeParallelHashMap<TKey, TValue>.ParallelWriter AsParallelWriter()
		{
			UnsafeParallelHashMap<TKey, TValue>.ParallelWriter parallelWriter;
			parallelWriter.m_ThreadIndex = 0;
			parallelWriter.m_Buffer = this.m_Buffer;
			return parallelWriter;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00025B73 File Offset: 0x00023D73
		public UnsafeParallelHashMap<TKey, TValue>.ReadOnly AsReadOnly()
		{
			return new UnsafeParallelHashMap<TKey, TValue>.ReadOnly(this);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00025B80 File Offset: 0x00023D80
		public UnsafeParallelHashMap<TKey, TValue>.Enumerator GetEnumerator()
		{
			return new UnsafeParallelHashMap<TKey, TValue>.Enumerator
			{
				m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_Buffer)
			};
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<KeyValue<TKey, TValue>> IEnumerable<KeyValue<TKey, TValue>>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000501 RID: 1281
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeParallelHashMapData* m_Buffer;

		// Token: 0x04000502 RID: 1282
		internal AllocatorManager.AllocatorHandle m_AllocatorLabel;

		// Token: 0x0200012A RID: 298
		[DebuggerDisplay("Count = {m_HashMapData.Count()}, Capacity = {m_HashMapData.Capacity}, IsCreated = {m_HashMapData.IsCreated}, IsEmpty = {IsEmpty}")]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public struct ReadOnly : IEnumerable<KeyValue<TKey, TValue>>, IEnumerable
		{
			// Token: 0x06000C83 RID: 3203 RVA: 0x00025BA8 File Offset: 0x00023DA8
			internal ReadOnly(UnsafeParallelHashMap<TKey, TValue> hashMapData)
			{
				this.m_HashMapData = hashMapData;
			}

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x06000C84 RID: 3204 RVA: 0x00025BB1 File Offset: 0x00023DB1
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_HashMapData.IsCreated;
				}
			}

			// Token: 0x17000177 RID: 375
			// (get) Token: 0x06000C85 RID: 3205 RVA: 0x00025BBE File Offset: 0x00023DBE
			public readonly bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return !this.IsCreated || this.m_HashMapData.IsEmpty;
				}
			}

			// Token: 0x06000C86 RID: 3206 RVA: 0x00025BD5 File Offset: 0x00023DD5
			public readonly int Count()
			{
				return this.m_HashMapData.Count();
			}

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00025BE2 File Offset: 0x00023DE2
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_HashMapData.Capacity;
				}
			}

			// Token: 0x06000C88 RID: 3208 RVA: 0x00025BF0 File Offset: 0x00023DF0
			public readonly bool TryGetValue(TKey key, out TValue item)
			{
				UnsafeParallelHashMap<TKey, TValue> hashMapData = this.m_HashMapData;
				return hashMapData.TryGetValue(key, out item);
			}

			// Token: 0x06000C89 RID: 3209 RVA: 0x00025C10 File Offset: 0x00023E10
			public readonly bool ContainsKey(TKey key)
			{
				UnsafeParallelHashMap<TKey, TValue> hashMapData = this.m_HashMapData;
				return hashMapData.ContainsKey(key);
			}

			// Token: 0x17000179 RID: 377
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

			// Token: 0x06000C8B RID: 3211 RVA: 0x00025C58 File Offset: 0x00023E58
			public readonly NativeArray<TKey> GetKeyArray(AllocatorManager.AllocatorHandle allocator)
			{
				UnsafeParallelHashMap<TKey, TValue> hashMapData = this.m_HashMapData;
				return hashMapData.GetKeyArray(allocator);
			}

			// Token: 0x06000C8C RID: 3212 RVA: 0x00025C74 File Offset: 0x00023E74
			public readonly NativeArray<TValue> GetValueArray(AllocatorManager.AllocatorHandle allocator)
			{
				UnsafeParallelHashMap<TKey, TValue> hashMapData = this.m_HashMapData;
				return hashMapData.GetValueArray(allocator);
			}

			// Token: 0x06000C8D RID: 3213 RVA: 0x00025C90 File Offset: 0x00023E90
			public readonly NativeKeyValueArrays<TKey, TValue> GetKeyValueArrays(AllocatorManager.AllocatorHandle allocator)
			{
				UnsafeParallelHashMap<TKey, TValue> hashMapData = this.m_HashMapData;
				return hashMapData.GetKeyValueArrays(allocator);
			}

			// Token: 0x06000C8E RID: 3214 RVA: 0x0001873D File Offset: 0x0001693D
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private readonly void ThrowKeyNotPresent(TKey key)
			{
				throw new ArgumentException(string.Format("Key: {0} is not present in the NativeParallelHashMap.", key));
			}

			// Token: 0x06000C8F RID: 3215 RVA: 0x00025CAC File Offset: 0x00023EAC
			public readonly UnsafeParallelHashMap<TKey, TValue>.Enumerator GetEnumerator()
			{
				return new UnsafeParallelHashMap<TKey, TValue>.Enumerator
				{
					m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_HashMapData.m_Buffer)
				};
			}

			// Token: 0x06000C90 RID: 3216 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<KeyValue<TKey, TValue>> IEnumerable<KeyValue<TKey, TValue>>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000C91 RID: 3217 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x04000503 RID: 1283
			internal UnsafeParallelHashMap<TKey, TValue> m_HashMapData;
		}

		// Token: 0x0200012B RID: 299
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public struct ParallelWriter
		{
			// Token: 0x1700017A RID: 378
			// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00025CD9 File Offset: 0x00023ED9
			public int ThreadIndex
			{
				get
				{
					return this.m_ThreadIndex;
				}
			}

			// Token: 0x1700017B RID: 379
			// (get) Token: 0x06000C93 RID: 3219 RVA: 0x00025CE1 File Offset: 0x00023EE1
			public unsafe readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Buffer->keyCapacity;
				}
			}

			// Token: 0x06000C94 RID: 3220 RVA: 0x00025CEE File Offset: 0x00023EEE
			public bool TryAdd(TKey key, TValue item)
			{
				return UnsafeParallelHashMapBase<TKey, TValue>.TryAddAtomic(this.m_Buffer, key, item, this.m_ThreadIndex);
			}

			// Token: 0x06000C95 RID: 3221 RVA: 0x00025D03 File Offset: 0x00023F03
			public bool TryAdd(TKey key, TValue item, int threadIndexOverride)
			{
				return UnsafeParallelHashMapBase<TKey, TValue>.TryAddAtomic(this.m_Buffer, key, item, threadIndexOverride);
			}

			// Token: 0x04000504 RID: 1284
			[NativeDisableUnsafePtrRestriction]
			internal unsafe UnsafeParallelHashMapData* m_Buffer;

			// Token: 0x04000505 RID: 1285
			[NativeSetThreadIndex]
			internal int m_ThreadIndex;
		}

		// Token: 0x0200012C RID: 300
		public struct Enumerator : IEnumerator<KeyValue<TKey, TValue>>, IEnumerator, IDisposable
		{
			// Token: 0x06000C96 RID: 3222 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000C97 RID: 3223 RVA: 0x00025D13 File Offset: 0x00023F13
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000C98 RID: 3224 RVA: 0x00025D20 File Offset: 0x00023F20
			public void Reset()
			{
				this.m_Enumerator.Reset();
			}

			// Token: 0x1700017C RID: 380
			// (get) Token: 0x06000C99 RID: 3225 RVA: 0x00025D2D File Offset: 0x00023F2D
			public KeyValue<TKey, TValue> Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Enumerator.GetCurrent<TKey, TValue>();
				}
			}

			// Token: 0x1700017D RID: 381
			// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00025D3A File Offset: 0x00023F3A
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04000506 RID: 1286
			internal UnsafeParallelHashMapDataEnumerator m_Enumerator;
		}
	}
}
