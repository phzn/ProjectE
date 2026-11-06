using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000136 RID: 310
	[DebuggerTypeProxy(typeof(UnsafeParallelMultiHashMapDebuggerTypeProxy<, >))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
	{
		typeof(int),
		typeof(int)
	})]
	public struct UnsafeParallelMultiHashMap<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> : INativeDisposable, IDisposable, IEnumerable<KeyValue<TKey, TValue>>, IEnumerable where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x06000D42 RID: 3394 RVA: 0x00029078 File Offset: 0x00027278
		public UnsafeParallelMultiHashMap(int capacity, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_AllocatorLabel = allocator;
			UnsafeParallelHashMapData.AllocateHashMap<TKey, TValue>(capacity, capacity * 2, allocator, out this.m_Buffer);
			this.Clear();
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00029097 File Offset: 0x00027297
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return !this.IsCreated || UnsafeParallelHashMapData.IsEmpty(this.m_Buffer);
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x000290AE File Offset: 0x000272AE
		public unsafe readonly int Count()
		{
			if (this.m_Buffer->allocatedIndexLength <= 0)
			{
				return 0;
			}
			return UnsafeParallelHashMapData.GetCount(this.m_Buffer);
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x000290CB File Offset: 0x000272CB
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x000290D8 File Offset: 0x000272D8
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

		// Token: 0x06000D47 RID: 3399 RVA: 0x000290F2 File Offset: 0x000272F2
		public void Clear()
		{
			UnsafeParallelHashMapBase<TKey, TValue>.Clear(this.m_Buffer);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x000290FF File Offset: 0x000272FF
		public void Add(TKey key, TValue item)
		{
			UnsafeParallelHashMapBase<TKey, TValue>.TryAdd(this.m_Buffer, key, item, true, this.m_AllocatorLabel);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00029116 File Offset: 0x00027316
		public int Remove(TKey key)
		{
			return UnsafeParallelHashMapBase<TKey, TValue>.Remove(this.m_Buffer, key, true);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00029125 File Offset: 0x00027325
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public void Remove<[global::System.Runtime.CompilerServices.IsUnmanaged] TValueEQ>(TKey key, TValueEQ value) where TValueEQ : struct, ValueType, IEquatable<TValueEQ>
		{
			UnsafeParallelHashMapBase<TKey, TValueEQ>.RemoveKeyValue<TValueEQ>(this.m_Buffer, key, value);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00029134 File Offset: 0x00027334
		public void Remove(NativeParallelMultiHashMapIterator<TKey> it)
		{
			UnsafeParallelHashMapBase<TKey, TValue>.Remove(this.m_Buffer, it);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00029142 File Offset: 0x00027342
		public readonly bool TryGetFirstValue(TKey key, out TValue item, out NativeParallelMultiHashMapIterator<TKey> it)
		{
			return UnsafeParallelHashMapBase<TKey, TValue>.TryGetFirstValueAtomic(this.m_Buffer, key, out item, out it);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00029152 File Offset: 0x00027352
		public readonly bool TryGetNextValue(out TValue item, ref NativeParallelMultiHashMapIterator<TKey> it)
		{
			return UnsafeParallelHashMapBase<TKey, TValue>.TryGetNextValueAtomic(this.m_Buffer, out item, ref it);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00029164 File Offset: 0x00027364
		public readonly bool ContainsKey(TKey key)
		{
			TValue tvalue;
			NativeParallelMultiHashMapIterator<TKey> nativeParallelMultiHashMapIterator;
			return this.TryGetFirstValue(key, out tvalue, out nativeParallelMultiHashMapIterator);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0002917C File Offset: 0x0002737C
		public readonly int CountValuesForKey(TKey key)
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

		// Token: 0x06000D50 RID: 3408 RVA: 0x000291AD File Offset: 0x000273AD
		public bool SetValue(TValue item, NativeParallelMultiHashMapIterator<TKey> it)
		{
			return UnsafeParallelHashMapBase<TKey, TValue>.SetValue(this.m_Buffer, ref it, ref item);
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x000291BE File Offset: 0x000273BE
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Buffer != null;
			}
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x000291CD File Offset: 0x000273CD
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			UnsafeParallelHashMapData.DeallocateHashMap(this.m_Buffer, this.m_AllocatorLabel);
			this.m_Buffer = null;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x000291F4 File Offset: 0x000273F4
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

		// Token: 0x06000D54 RID: 3412 RVA: 0x0002923C File Offset: 0x0002743C
		public readonly NativeArray<TKey> GetKeyArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<TKey> nativeArray = CollectionHelper.CreateNativeArray<TKey>(this.Count(), allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeParallelHashMapData.GetKeyArray<TKey>(this.m_Buffer, nativeArray);
			return nativeArray;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00029264 File Offset: 0x00027464
		public readonly NativeArray<TValue> GetValueArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<TValue> nativeArray = CollectionHelper.CreateNativeArray<TValue>(this.Count(), allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeParallelHashMapData.GetValueArray<TValue>(this.m_Buffer, nativeArray);
			return nativeArray;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0002928C File Offset: 0x0002748C
		public readonly NativeKeyValueArrays<TKey, TValue> GetKeyValueArrays(AllocatorManager.AllocatorHandle allocator)
		{
			NativeKeyValueArrays<TKey, TValue> nativeKeyValueArrays = new NativeKeyValueArrays<TKey, TValue>(this.Count(), allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeParallelHashMapData.GetKeyValueArrays<TKey, TValue>(this.m_Buffer, nativeKeyValueArrays);
			return nativeKeyValueArrays;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x000292B8 File Offset: 0x000274B8
		public UnsafeParallelMultiHashMap<TKey, TValue>.Enumerator GetValuesForKey(TKey key)
		{
			return new UnsafeParallelMultiHashMap<TKey, TValue>.Enumerator
			{
				hashmap = this,
				key = key,
				isFirst = true
			};
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x000292EC File Offset: 0x000274EC
		public UnsafeParallelMultiHashMap<TKey, TValue>.ParallelWriter AsParallelWriter()
		{
			UnsafeParallelMultiHashMap<TKey, TValue>.ParallelWriter parallelWriter;
			parallelWriter.m_ThreadIndex = 0;
			parallelWriter.m_Buffer = this.m_Buffer;
			return parallelWriter;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00029310 File Offset: 0x00027510
		public UnsafeParallelMultiHashMap<TKey, TValue>.KeyValueEnumerator GetEnumerator()
		{
			return new UnsafeParallelMultiHashMap<TKey, TValue>.KeyValueEnumerator
			{
				m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_Buffer)
			};
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<KeyValue<TKey, TValue>> IEnumerable<KeyValue<TKey, TValue>>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00029338 File Offset: 0x00027538
		public UnsafeParallelMultiHashMap<TKey, TValue>.ReadOnly AsReadOnly()
		{
			return new UnsafeParallelMultiHashMap<TKey, TValue>.ReadOnly(this);
		}

		// Token: 0x04000511 RID: 1297
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeParallelHashMapData* m_Buffer;

		// Token: 0x04000512 RID: 1298
		internal AllocatorManager.AllocatorHandle m_AllocatorLabel;

		// Token: 0x02000137 RID: 311
		public struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
		{
			// Token: 0x06000D5D RID: 3421 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000D5E RID: 3422 RVA: 0x00029348 File Offset: 0x00027548
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				if (this.isFirst)
				{
					this.isFirst = false;
					return this.hashmap.TryGetFirstValue(this.key, out this.value, out this.iterator);
				}
				return this.hashmap.TryGetNextValue(out this.value, ref this.iterator);
			}

			// Token: 0x06000D5F RID: 3423 RVA: 0x00029399 File Offset: 0x00027599
			public void Reset()
			{
				this.isFirst = true;
			}

			// Token: 0x1700018C RID: 396
			// (get) Token: 0x06000D60 RID: 3424 RVA: 0x000293A2 File Offset: 0x000275A2
			public TValue Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.value;
				}
			}

			// Token: 0x1700018D RID: 397
			// (get) Token: 0x06000D61 RID: 3425 RVA: 0x000293AA File Offset: 0x000275AA
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000D62 RID: 3426 RVA: 0x000293B7 File Offset: 0x000275B7
			public UnsafeParallelMultiHashMap<TKey, TValue>.Enumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x04000513 RID: 1299
			internal UnsafeParallelMultiHashMap<TKey, TValue> hashmap;

			// Token: 0x04000514 RID: 1300
			internal TKey key;

			// Token: 0x04000515 RID: 1301
			internal bool isFirst;

			// Token: 0x04000516 RID: 1302
			private TValue value;

			// Token: 0x04000517 RID: 1303
			private NativeParallelMultiHashMapIterator<TKey> iterator;
		}

		// Token: 0x02000138 RID: 312
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public struct ParallelWriter
		{
			// Token: 0x1700018E RID: 398
			// (get) Token: 0x06000D63 RID: 3427 RVA: 0x000293BF File Offset: 0x000275BF
			public unsafe readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Buffer->keyCapacity;
				}
			}

			// Token: 0x06000D64 RID: 3428 RVA: 0x000293CC File Offset: 0x000275CC
			public void Add(TKey key, TValue item)
			{
				UnsafeParallelHashMapBase<TKey, TValue>.AddAtomicMulti(this.m_Buffer, key, item, this.m_ThreadIndex);
			}

			// Token: 0x06000D65 RID: 3429 RVA: 0x000293E1 File Offset: 0x000275E1
			public void Add(TKey key, TValue item, int threadIndexOverride)
			{
				UnsafeParallelHashMapBase<TKey, TValue>.AddAtomicMulti(this.m_Buffer, key, item, threadIndexOverride);
			}

			// Token: 0x04000518 RID: 1304
			[NativeDisableUnsafePtrRestriction]
			internal unsafe UnsafeParallelHashMapData* m_Buffer;

			// Token: 0x04000519 RID: 1305
			[NativeSetThreadIndex]
			internal int m_ThreadIndex;
		}

		// Token: 0x02000139 RID: 313
		public struct KeyValueEnumerator : IEnumerator<KeyValue<TKey, TValue>>, IEnumerator, IDisposable
		{
			// Token: 0x06000D66 RID: 3430 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000D67 RID: 3431 RVA: 0x000293F1 File Offset: 0x000275F1
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000D68 RID: 3432 RVA: 0x000293FE File Offset: 0x000275FE
			public void Reset()
			{
				this.m_Enumerator.Reset();
			}

			// Token: 0x1700018F RID: 399
			// (get) Token: 0x06000D69 RID: 3433 RVA: 0x0002940B File Offset: 0x0002760B
			public KeyValue<TKey, TValue> Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Enumerator.GetCurrent<TKey, TValue>();
				}
			}

			// Token: 0x17000190 RID: 400
			// (get) Token: 0x06000D6A RID: 3434 RVA: 0x00029418 File Offset: 0x00027618
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0400051A RID: 1306
			internal UnsafeParallelHashMapDataEnumerator m_Enumerator;
		}

		// Token: 0x0200013A RID: 314
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public struct ReadOnly : IEnumerable<KeyValue<TKey, TValue>>, IEnumerable
		{
			// Token: 0x06000D6B RID: 3435 RVA: 0x00029425 File Offset: 0x00027625
			internal ReadOnly(UnsafeParallelMultiHashMap<TKey, TValue> container)
			{
				this.m_MultiHashMapData = container;
			}

			// Token: 0x17000191 RID: 401
			// (get) Token: 0x06000D6C RID: 3436 RVA: 0x0002942E File Offset: 0x0002762E
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_MultiHashMapData.IsCreated;
				}
			}

			// Token: 0x17000192 RID: 402
			// (get) Token: 0x06000D6D RID: 3437 RVA: 0x0002943B File Offset: 0x0002763B
			public readonly bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return !this.IsCreated || this.m_MultiHashMapData.IsEmpty;
				}
			}

			// Token: 0x06000D6E RID: 3438 RVA: 0x00029452 File Offset: 0x00027652
			public readonly int Count()
			{
				return this.m_MultiHashMapData.Count();
			}

			// Token: 0x17000193 RID: 403
			// (get) Token: 0x06000D6F RID: 3439 RVA: 0x0002945F File Offset: 0x0002765F
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_MultiHashMapData.Capacity;
				}
			}

			// Token: 0x06000D70 RID: 3440 RVA: 0x0002946C File Offset: 0x0002766C
			public readonly bool TryGetFirstValue(TKey key, out TValue item, out NativeParallelMultiHashMapIterator<TKey> it)
			{
				return this.m_MultiHashMapData.TryGetFirstValue(key, out item, out it);
			}

			// Token: 0x06000D71 RID: 3441 RVA: 0x0002947C File Offset: 0x0002767C
			public readonly bool TryGetNextValue(out TValue item, ref NativeParallelMultiHashMapIterator<TKey> it)
			{
				return this.m_MultiHashMapData.TryGetNextValue(out item, ref it);
			}

			// Token: 0x06000D72 RID: 3442 RVA: 0x0002948B File Offset: 0x0002768B
			public readonly bool ContainsKey(TKey key)
			{
				return this.m_MultiHashMapData.ContainsKey(key);
			}

			// Token: 0x06000D73 RID: 3443 RVA: 0x00029499 File Offset: 0x00027699
			public readonly NativeArray<TKey> GetKeyArray(AllocatorManager.AllocatorHandle allocator)
			{
				return this.m_MultiHashMapData.GetKeyArray(allocator);
			}

			// Token: 0x06000D74 RID: 3444 RVA: 0x000294A7 File Offset: 0x000276A7
			public readonly NativeArray<TValue> GetValueArray(AllocatorManager.AllocatorHandle allocator)
			{
				return this.m_MultiHashMapData.GetValueArray(allocator);
			}

			// Token: 0x06000D75 RID: 3445 RVA: 0x000294B5 File Offset: 0x000276B5
			public readonly NativeKeyValueArrays<TKey, TValue> GetKeyValueArrays(AllocatorManager.AllocatorHandle allocator)
			{
				return this.m_MultiHashMapData.GetKeyValueArrays(allocator);
			}

			// Token: 0x06000D76 RID: 3446 RVA: 0x000294C4 File Offset: 0x000276C4
			public UnsafeParallelMultiHashMap<TKey, TValue>.KeyValueEnumerator GetEnumerator()
			{
				return new UnsafeParallelMultiHashMap<TKey, TValue>.KeyValueEnumerator
				{
					m_Enumerator = new UnsafeParallelHashMapDataEnumerator(this.m_MultiHashMapData.m_Buffer)
				};
			}

			// Token: 0x06000D77 RID: 3447 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<KeyValue<TKey, TValue>> IEnumerable<KeyValue<TKey, TValue>>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000D78 RID: 3448 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x0400051B RID: 1307
			internal UnsafeParallelMultiHashMap<TKey, TValue> m_MultiHashMapData;
		}
	}
}
