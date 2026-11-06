using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200010B RID: 267
	[DebuggerTypeProxy(typeof(UnsafeHashMapDebuggerTypeProxy<, >))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
	{
		typeof(int),
		typeof(int)
	})]
	public struct UnsafeHashMap<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> : INativeDisposable, IDisposable, IEnumerable<KVPair<TKey, TValue>>, IEnumerable where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x06000B66 RID: 2918 RVA: 0x00022EFF File Offset: 0x000210FF
		public UnsafeHashMap(int initialCapacity, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_Data = default(HashMapHelper<TKey>);
			this.m_Data.Init(initialCapacity, sizeof(TValue), 256, allocator);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00022F25 File Offset: 0x00021125
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			this.m_Data.Dispose();
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00022F3C File Offset: 0x0002113C
		public unsafe JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new UnsafeDisposeJob
			{
				Ptr = (void*)this.m_Data.Ptr,
				Allocator = this.m_Data.Allocator
			}.Schedule(inputDeps);
			this.m_Data = default(HashMapHelper<TKey>);
			return jobHandle;
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00022F92 File Offset: 0x00021192
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.IsCreated;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00022F9F File Offset: 0x0002119F
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.IsEmpty;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x00022FAC File Offset: 0x000211AC
		public readonly int Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.Count;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x00022FB9 File Offset: 0x000211B9
		// (set) Token: 0x06000B6D RID: 2925 RVA: 0x00022FC6 File Offset: 0x000211C6
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.m_Data.Capacity;
			}
			set
			{
				this.m_Data.Resize(value);
			}
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00022FD4 File Offset: 0x000211D4
		public void Clear()
		{
			this.m_Data.Clear();
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00022FE4 File Offset: 0x000211E4
		public unsafe bool TryAdd(TKey key, TValue item)
		{
			int num = this.m_Data.TryAdd(in key);
			if (-1 != num)
			{
				UnsafeUtility.WriteArrayElement<TValue>((void*)this.m_Data.Ptr, num, item);
				return true;
			}
			return false;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00023018 File Offset: 0x00021218
		public void Add(TKey key, TValue item)
		{
			this.TryAdd(key, item);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00023023 File Offset: 0x00021223
		public bool Remove(TKey key)
		{
			return -1 != this.m_Data.TryRemove(key);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00023037 File Offset: 0x00021237
		public bool TryGetValue(TKey key, out TValue item)
		{
			return this.m_Data.TryGetValue<TValue>(key, out item);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00023046 File Offset: 0x00021246
		public bool ContainsKey(TKey key)
		{
			return -1 != this.m_Data.Find(key);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002305A File Offset: 0x0002125A
		public void TrimExcess()
		{
			this.m_Data.TrimExcess();
		}

		// Token: 0x17000147 RID: 327
		public unsafe TValue this[TKey key]
		{
			get
			{
				TValue tvalue;
				this.m_Data.TryGetValue<TValue>(key, out tvalue);
				return tvalue;
			}
			set
			{
				int num = this.m_Data.Find(key);
				if (-1 != num)
				{
					UnsafeUtility.WriteArrayElement<TValue>((void*)this.m_Data.Ptr, num, value);
					return;
				}
				this.TryAdd(key, value);
			}
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x000230C2 File Offset: 0x000212C2
		public NativeArray<TKey> GetKeyArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_Data.GetKeyArray(allocator);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000230D0 File Offset: 0x000212D0
		public NativeArray<TValue> GetValueArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_Data.GetValueArray<TValue>(allocator);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x000230DE File Offset: 0x000212DE
		public NativeKeyValueArrays<TKey, TValue> GetKeyValueArrays(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_Data.GetKeyValueArrays<TValue>(allocator);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x000230EC File Offset: 0x000212EC
		public unsafe UnsafeHashMap<TKey, TValue>.Enumerator GetEnumerator()
		{
			fixed (HashMapHelper<TKey>* ptr = &this.m_Data)
			{
				HashMapHelper<TKey>* ptr2 = ptr;
				return new UnsafeHashMap<TKey, TValue>.Enumerator
				{
					m_Enumerator = new HashMapHelper<TKey>.Enumerator(ptr2)
				};
			}
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<KVPair<TKey, TValue>> IEnumerable<KVPair<TKey, TValue>>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00023119 File Offset: 0x00021319
		public UnsafeHashMap<TKey, TValue>.ReadOnly AsReadOnly()
		{
			return new UnsafeHashMap<TKey, TValue>.ReadOnly(ref this.m_Data);
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00017958 File Offset: 0x00015B58
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ThrowKeyNotPresent(TKey key)
		{
			throw new ArgumentException(string.Format("Key: {0} is not present.", key));
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0001796F File Offset: 0x00015B6F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ThrowKeyAlreadyAdded(TKey key)
		{
			throw new ArgumentException(string.Format("An item with the same key has already been added: {0}", key));
		}

		// Token: 0x040004BE RID: 1214
		[NativeDisableUnsafePtrRestriction]
		internal HashMapHelper<TKey> m_Data;

		// Token: 0x0200010C RID: 268
		public struct Enumerator : IEnumerator<KVPair<TKey, TValue>>, IEnumerator, IDisposable
		{
			// Token: 0x06000B80 RID: 2944 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000B81 RID: 2945 RVA: 0x00023126 File Offset: 0x00021326
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000B82 RID: 2946 RVA: 0x00023133 File Offset: 0x00021333
			public void Reset()
			{
				this.m_Enumerator.Reset();
			}

			// Token: 0x17000148 RID: 328
			// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00023140 File Offset: 0x00021340
			public KVPair<TKey, TValue> Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Enumerator.GetCurrent<TValue>();
				}
			}

			// Token: 0x17000149 RID: 329
			// (get) Token: 0x06000B84 RID: 2948 RVA: 0x0002314D File Offset: 0x0002134D
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040004BF RID: 1215
			internal HashMapHelper<TKey>.Enumerator m_Enumerator;
		}

		// Token: 0x0200010D RID: 269
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public struct ReadOnly : IEnumerable<KVPair<TKey, TValue>>, IEnumerable
		{
			// Token: 0x06000B85 RID: 2949 RVA: 0x0002315A File Offset: 0x0002135A
			internal ReadOnly(ref HashMapHelper<TKey> data)
			{
				this.m_Data = data;
			}

			// Token: 0x1700014A RID: 330
			// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00023168 File Offset: 0x00021368
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.IsCreated;
				}
			}

			// Token: 0x1700014B RID: 331
			// (get) Token: 0x06000B87 RID: 2951 RVA: 0x00023175 File Offset: 0x00021375
			public readonly bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.IsEmpty;
				}
			}

			// Token: 0x1700014C RID: 332
			// (get) Token: 0x06000B88 RID: 2952 RVA: 0x00023182 File Offset: 0x00021382
			public readonly int Count
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.Count;
				}
			}

			// Token: 0x1700014D RID: 333
			// (get) Token: 0x06000B89 RID: 2953 RVA: 0x0002318F File Offset: 0x0002138F
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.Capacity;
				}
			}

			// Token: 0x06000B8A RID: 2954 RVA: 0x0002319C File Offset: 0x0002139C
			public readonly bool TryGetValue(TKey key, out TValue item)
			{
				HashMapHelper<TKey> data = this.m_Data;
				return data.TryGetValue<TValue>(key, out item);
			}

			// Token: 0x06000B8B RID: 2955 RVA: 0x000231BC File Offset: 0x000213BC
			public readonly bool ContainsKey(TKey key)
			{
				int num = -1;
				HashMapHelper<TKey> data = this.m_Data;
				return num != data.Find(key);
			}

			// Token: 0x1700014E RID: 334
			public readonly TValue this[TKey key]
			{
				get
				{
					HashMapHelper<TKey> data = this.m_Data;
					TValue tvalue;
					data.TryGetValue<TValue>(key, out tvalue);
					return tvalue;
				}
			}

			// Token: 0x06000B8D RID: 2957 RVA: 0x00023200 File Offset: 0x00021400
			public readonly NativeArray<TKey> GetKeyArray(AllocatorManager.AllocatorHandle allocator)
			{
				HashMapHelper<TKey> data = this.m_Data;
				return data.GetKeyArray(allocator);
			}

			// Token: 0x06000B8E RID: 2958 RVA: 0x0002321C File Offset: 0x0002141C
			public readonly NativeArray<TValue> GetValueArray(AllocatorManager.AllocatorHandle allocator)
			{
				HashMapHelper<TKey> data = this.m_Data;
				return data.GetValueArray<TValue>(allocator);
			}

			// Token: 0x06000B8F RID: 2959 RVA: 0x00023238 File Offset: 0x00021438
			public readonly NativeKeyValueArrays<TKey, TValue> GetKeyValueArrays(AllocatorManager.AllocatorHandle allocator)
			{
				HashMapHelper<TKey> data = this.m_Data;
				return data.GetKeyValueArrays<TValue>(allocator);
			}

			// Token: 0x06000B90 RID: 2960 RVA: 0x00023254 File Offset: 0x00021454
			public unsafe readonly UnsafeHashMap<TKey, TValue>.Enumerator GetEnumerator()
			{
				fixed (HashMapHelper<TKey>* ptr = &this.m_Data)
				{
					HashMapHelper<TKey>* ptr2 = ptr;
					return new UnsafeHashMap<TKey, TValue>.Enumerator
					{
						m_Enumerator = new HashMapHelper<TKey>.Enumerator(ptr2)
					};
				}
			}

			// Token: 0x06000B91 RID: 2961 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<KVPair<TKey, TValue>> IEnumerable<KVPair<TKey, TValue>>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000B92 RID: 2962 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x040004C0 RID: 1216
			[NativeDisableUnsafePtrRestriction]
			internal HashMapHelper<TKey> m_Data;
		}
	}
}
