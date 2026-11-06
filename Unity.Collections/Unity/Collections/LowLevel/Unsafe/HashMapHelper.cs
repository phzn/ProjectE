using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000109 RID: 265
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	internal struct HashMapHelper<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey> where TKey : struct, ValueType, IEquatable<TKey>
	{
		// Token: 0x06000B48 RID: 2888 RVA: 0x00022564 File Offset: 0x00020764
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal int CalcCapacityCeilPow2(int capacity)
		{
			capacity = math.max(math.max(1, this.Count), capacity);
			return math.ceilpow2(math.max(capacity, 1 << this.Log2MinGrowth));
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00022590 File Offset: 0x00020790
		internal static int GetBucketSize(int capacity)
		{
			return capacity * 2;
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00022595 File Offset: 0x00020795
		internal readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Ptr != null;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x000225A4 File Offset: 0x000207A4
		internal readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return !this.IsCreated || this.Count == 0;
			}
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x000225BC File Offset: 0x000207BC
		internal unsafe void Clear()
		{
			UnsafeUtility.MemSet((void*)this.Buckets, byte.MaxValue, (long)(this.BucketCapacity * 4));
			UnsafeUtility.MemSet((void*)this.Next, byte.MaxValue, (long)(this.Capacity * 4));
			this.Count = 0;
			this.FirstFreeIdx = -1;
			this.AllocatedIndex = 0;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00022610 File Offset: 0x00020810
		internal unsafe void Init(int capacity, int sizeOfValueT, int minGrowth, AllocatorManager.AllocatorHandle allocator)
		{
			this.Count = 0;
			this.Log2MinGrowth = (int)((byte)(32 - math.lzcnt(math.max(1, minGrowth) - 1)));
			capacity = this.CalcCapacityCeilPow2(capacity);
			this.Capacity = capacity;
			this.BucketCapacity = HashMapHelper<TKey>.GetBucketSize(capacity);
			this.Allocator = allocator;
			this.SizeOfTValue = sizeOfValueT;
			int num2;
			int num3;
			int num4;
			int num = HashMapHelper<TKey>.CalculateDataSize(capacity, this.BucketCapacity, sizeOfValueT, out num2, out num3, out num4);
			this.Ptr = (byte*)Memory.Unmanaged.Allocate((long)num, 64, allocator);
			this.Keys = (TKey*)(this.Ptr + num2);
			this.Next = (int*)(this.Ptr + num3);
			this.Buckets = (int*)(this.Ptr + num4);
			this.Clear();
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x000226BC File Offset: 0x000208BC
		internal void Dispose()
		{
			Memory.Unmanaged.Free<byte>(this.Ptr, this.Allocator);
			this.Ptr = null;
			this.Keys = null;
			this.Next = null;
			this.Buckets = null;
			this.Count = 0;
			this.BucketCapacity = 0;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00022708 File Offset: 0x00020908
		internal unsafe static HashMapHelper<TKey>* Alloc(int capacity, int sizeOfValueT, int minGrowth, AllocatorManager.AllocatorHandle allocator)
		{
			HashMapHelper<TKey>* ptr = (HashMapHelper<TKey>*)Memory.Unmanaged.Allocate((long)sizeof(HashMapHelper<TKey>), UnsafeUtility.AlignOf<HashMapHelper<TKey>>(), allocator);
			ptr->Init(capacity, sizeOfValueT, minGrowth, allocator);
			return ptr;
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00022733 File Offset: 0x00020933
		internal unsafe static void Free(HashMapHelper<TKey>* data)
		{
			if (data == null)
			{
				throw new InvalidOperationException("Hash based container has yet to be created or has been destroyed!");
			}
			data->Dispose();
			Memory.Unmanaged.Free<HashMapHelper<TKey>>(data, data->Allocator);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00022758 File Offset: 0x00020958
		internal void Resize(int newCapacity)
		{
			newCapacity = math.max(newCapacity, this.Count);
			int num = math.ceilpow2(HashMapHelper<TKey>.GetBucketSize(newCapacity));
			if (this.Capacity == newCapacity && this.BucketCapacity == num)
			{
				return;
			}
			this.ResizeExact(newCapacity, num);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0002279C File Offset: 0x0002099C
		internal unsafe void ResizeExact(int newCapacity, int newBucketCapacity)
		{
			int num2;
			int num3;
			int num4;
			int num = HashMapHelper<TKey>.CalculateDataSize(newCapacity, newBucketCapacity, this.SizeOfTValue, out num2, out num3, out num4);
			byte* ptr = this.Ptr;
			TKey* keys = this.Keys;
			int* next = this.Next;
			int* buckets = this.Buckets;
			int bucketCapacity = this.BucketCapacity;
			this.Ptr = (byte*)Memory.Unmanaged.Allocate((long)num, 64, this.Allocator);
			this.Keys = (TKey*)(this.Ptr + num2);
			this.Next = (int*)(this.Ptr + num3);
			this.Buckets = (int*)(this.Ptr + num4);
			this.Capacity = newCapacity;
			this.BucketCapacity = newBucketCapacity;
			this.Clear();
			int i = 0;
			int num5 = bucketCapacity;
			while (i < num5)
			{
				for (int num6 = buckets[i]; num6 != -1; num6 = next[num6])
				{
					int num7 = this.TryAdd(in keys[(IntPtr)num6 * (IntPtr)sizeof(TKey) / (IntPtr)sizeof(TKey)]);
					UnsafeUtility.MemCpy((void*)(this.Ptr + this.SizeOfTValue * num7), (void*)(ptr + this.SizeOfTValue * num6), (long)this.SizeOfTValue);
				}
				i++;
			}
			Memory.Unmanaged.Free<byte>(ptr, this.Allocator);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x000228B0 File Offset: 0x00020AB0
		internal void TrimExcess()
		{
			int num = this.CalcCapacityCeilPow2(this.Count);
			this.ResizeExact(num, HashMapHelper<TKey>.GetBucketSize(num));
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x000228D8 File Offset: 0x00020AD8
		internal static int CalculateDataSize(int capacity, int bucketCapacity, int sizeOfTValue, out int outKeyOffset, out int outNextOffset, out int outBucketOffset)
		{
			int num = sizeof(TKey);
			int num2 = 4;
			int num3 = sizeOfTValue * capacity;
			int num4 = num * capacity;
			int num5 = num2 * capacity;
			int num6 = num2 * bucketCapacity;
			int num7 = num3 + num4 + num5 + num6;
			outKeyOffset = num3;
			outNextOffset = outKeyOffset + num4;
			outBucketOffset = outNextOffset + num5;
			return num7;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00022918 File Offset: 0x00020B18
		internal unsafe readonly int GetCount()
		{
			if (this.AllocatedIndex <= 0)
			{
				return 0;
			}
			int num = 0;
			for (int i = this.FirstFreeIdx; i >= 0; i = this.Next[i])
			{
				num++;
			}
			return math.min(this.Capacity, this.AllocatedIndex) - num;
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00022964 File Offset: 0x00020B64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int GetBucket(in TKey key)
		{
			TKey tkey = key;
			return (int)((ulong)tkey.GetHashCode() & (ulong)((long)(this.BucketCapacity - 1)));
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00022994 File Offset: 0x00020B94
		internal unsafe int TryAdd(in TKey key)
		{
			if (-1 == this.Find(key))
			{
				if (this.AllocatedIndex >= this.Capacity && this.FirstFreeIdx < 0)
				{
					int num = this.CalcCapacityCeilPow2(this.Capacity + (1 << this.Log2MinGrowth));
					this.Resize(num);
				}
				int num2 = this.FirstFreeIdx;
				if (num2 >= 0)
				{
					this.FirstFreeIdx = this.Next[num2];
				}
				else
				{
					int allocatedIndex = this.AllocatedIndex;
					this.AllocatedIndex = allocatedIndex + 1;
					num2 = allocatedIndex;
				}
				UnsafeUtility.WriteArrayElement<TKey>((void*)this.Keys, num2, key);
				int bucket = this.GetBucket(in key);
				this.Next[num2] = this.Buckets[bucket];
				this.Buckets[bucket] = num2;
				this.Count++;
				return num2;
			}
			return -1;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00022A6C File Offset: 0x00020C6C
		internal unsafe int Find(TKey key)
		{
			if (this.AllocatedIndex > 0)
			{
				int bucket = this.GetBucket(in key);
				int num = this.Buckets[bucket];
				if (num < this.Capacity)
				{
					int* next = this.Next;
					do
					{
						TKey tkey = UnsafeUtility.ReadArrayElement<TKey>((void*)this.Keys, num);
						if (tkey.Equals(key))
						{
							return num;
						}
						num = next[num];
					}
					while (num < this.Capacity);
					return -1;
				}
			}
			return -1;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00022AE0 File Offset: 0x00020CE0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		internal unsafe bool TryGetValue<[global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(TKey key, out TValue item) where TValue : struct, ValueType
		{
			int num = this.Find(key);
			if (-1 != num)
			{
				item = UnsafeUtility.ReadArrayElement<TValue>((void*)this.Ptr, num);
				return true;
			}
			item = default(TValue);
			return false;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00022B18 File Offset: 0x00020D18
		internal unsafe int TryRemove(TKey key)
		{
			if (this.Capacity == 0)
			{
				return -1;
			}
			int num = 0;
			int bucket = this.GetBucket(in key);
			int num2 = -1;
			int num3 = this.Buckets[bucket];
			while (num3 >= 0 && num3 < this.Capacity)
			{
				TKey tkey = UnsafeUtility.ReadArrayElement<TKey>((void*)this.Keys, num3);
				if (tkey.Equals(key))
				{
					num++;
					if (num2 < 0)
					{
						this.Buckets[bucket] = this.Next[num3];
					}
					else
					{
						this.Next[num2] = this.Next[num3];
					}
					int num4 = this.Next[num3];
					this.Next[num3] = this.FirstFreeIdx;
					this.FirstFreeIdx = num3;
					break;
				}
				num2 = num3;
				num3 = this.Next[num3];
			}
			this.Count -= num;
			if (num == 0)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00022C04 File Offset: 0x00020E04
		internal unsafe bool MoveNextSearch(ref int bucketIndex, ref int nextIndex, out int index)
		{
			int i = bucketIndex;
			int bucketCapacity = this.BucketCapacity;
			while (i < bucketCapacity)
			{
				int num = this.Buckets[i];
				if (num != -1)
				{
					index = num;
					bucketIndex = i + 1;
					nextIndex = this.Next[num];
					return true;
				}
				i++;
			}
			index = -1;
			bucketIndex = this.BucketCapacity;
			nextIndex = -1;
			return false;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00022C5D File Offset: 0x00020E5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe bool MoveNext(ref int bucketIndex, ref int nextIndex, out int index)
		{
			if (nextIndex != -1)
			{
				index = nextIndex;
				nextIndex = this.Next[nextIndex];
				return true;
			}
			return this.MoveNextSearch(ref bucketIndex, ref nextIndex, out index);
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00022C84 File Offset: 0x00020E84
		internal unsafe NativeArray<TKey> GetKeyArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<TKey> nativeArray = CollectionHelper.CreateNativeArray<TKey>(this.Count, allocator, NativeArrayOptions.UninitializedMemory);
			int num = 0;
			int num2 = 0;
			int length = nativeArray.Length;
			int bucketCapacity = this.BucketCapacity;
			while (num < bucketCapacity && num2 < length)
			{
				for (int num3 = this.Buckets[num]; num3 != -1; num3 = this.Next[num3])
				{
					nativeArray[num2++] = UnsafeUtility.ReadArrayElement<TKey>((void*)this.Keys, num3);
				}
				num++;
			}
			return nativeArray;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00022D00 File Offset: 0x00020F00
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		internal unsafe NativeArray<TValue> GetValueArray<[global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(AllocatorManager.AllocatorHandle allocator) where TValue : struct, ValueType
		{
			NativeArray<TValue> nativeArray = CollectionHelper.CreateNativeArray<TValue>(this.Count, allocator, NativeArrayOptions.UninitializedMemory);
			int num = 0;
			int num2 = 0;
			int length = nativeArray.Length;
			int bucketCapacity = this.BucketCapacity;
			while (num < bucketCapacity && num2 < length)
			{
				for (int num3 = this.Buckets[num]; num3 != -1; num3 = this.Next[num3])
				{
					nativeArray[num2++] = UnsafeUtility.ReadArrayElement<TValue>((void*)this.Ptr, num3);
				}
				num++;
			}
			return nativeArray;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00022D7C File Offset: 0x00020F7C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		internal unsafe NativeKeyValueArrays<TKey, TValue> GetKeyValueArrays<[global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(AllocatorManager.AllocatorHandle allocator) where TValue : struct, ValueType
		{
			NativeKeyValueArrays<TKey, TValue> nativeKeyValueArrays = new NativeKeyValueArrays<TKey, TValue>(this.Count, allocator, NativeArrayOptions.UninitializedMemory);
			int num = 0;
			int num2 = 0;
			int length = nativeKeyValueArrays.Length;
			int bucketCapacity = this.BucketCapacity;
			while (num < bucketCapacity && num2 < length)
			{
				for (int num3 = this.Buckets[num]; num3 != -1; num3 = this.Next[num3])
				{
					nativeKeyValueArrays.Keys[num2] = UnsafeUtility.ReadArrayElement<TKey>((void*)this.Keys, num3);
					nativeKeyValueArrays.Values[num2] = UnsafeUtility.ReadArrayElement<TValue>((void*)this.Ptr, num3);
					num2++;
				}
				num++;
			}
			return nativeKeyValueArrays;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00022E18 File Offset: 0x00021018
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckIndexOutOfBounds(int idx)
		{
			if (idx >= this.Capacity)
			{
				throw new InvalidOperationException(string.Format("Internal HashMap error. idx {0}", idx));
			}
		}

		// Token: 0x040004AD RID: 1197
		[NativeDisableUnsafePtrRestriction]
		internal unsafe byte* Ptr;

		// Token: 0x040004AE RID: 1198
		[NativeDisableUnsafePtrRestriction]
		internal unsafe TKey* Keys;

		// Token: 0x040004AF RID: 1199
		[NativeDisableUnsafePtrRestriction]
		internal unsafe int* Next;

		// Token: 0x040004B0 RID: 1200
		[NativeDisableUnsafePtrRestriction]
		internal unsafe int* Buckets;

		// Token: 0x040004B1 RID: 1201
		internal int Count;

		// Token: 0x040004B2 RID: 1202
		internal int Capacity;

		// Token: 0x040004B3 RID: 1203
		internal int Log2MinGrowth;

		// Token: 0x040004B4 RID: 1204
		internal int BucketCapacity;

		// Token: 0x040004B5 RID: 1205
		internal int AllocatedIndex;

		// Token: 0x040004B6 RID: 1206
		internal int FirstFreeIdx;

		// Token: 0x040004B7 RID: 1207
		internal int SizeOfTValue;

		// Token: 0x040004B8 RID: 1208
		internal AllocatorManager.AllocatorHandle Allocator;

		// Token: 0x040004B9 RID: 1209
		internal const int kMinimumCapacity = 256;

		// Token: 0x0200010A RID: 266
		internal struct Enumerator
		{
			// Token: 0x06000B61 RID: 2913 RVA: 0x00022E39 File Offset: 0x00021039
			internal unsafe Enumerator(HashMapHelper<TKey>* data)
			{
				this.m_Data = data;
				this.m_Index = -1;
				this.m_BucketIndex = 0;
				this.m_NextIndex = -1;
			}

			// Token: 0x06000B62 RID: 2914 RVA: 0x00022E57 File Offset: 0x00021057
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe bool MoveNext()
			{
				return this.m_Data->MoveNext(ref this.m_BucketIndex, ref this.m_NextIndex, out this.m_Index);
			}

			// Token: 0x06000B63 RID: 2915 RVA: 0x00022E76 File Offset: 0x00021076
			internal void Reset()
			{
				this.m_Index = -1;
				this.m_BucketIndex = 0;
				this.m_NextIndex = -1;
			}

			// Token: 0x06000B64 RID: 2916 RVA: 0x00022E90 File Offset: 0x00021090
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal KVPair<TKey, TValue> GetCurrent<[global::System.Runtime.CompilerServices.IsUnmanaged] TValue>() where TValue : struct, ValueType
			{
				return new KVPair<TKey, TValue>
				{
					m_Data = this.m_Data,
					m_Index = this.m_Index
				};
			}

			// Token: 0x06000B65 RID: 2917 RVA: 0x00022EC0 File Offset: 0x000210C0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe TKey GetCurrentKey()
			{
				if (this.m_Index != -1)
				{
					return this.m_Data->Keys[(IntPtr)this.m_Index * (IntPtr)sizeof(TKey) / (IntPtr)sizeof(TKey)];
				}
				return default(TKey);
			}

			// Token: 0x040004BA RID: 1210
			[NativeDisableUnsafePtrRestriction]
			internal unsafe HashMapHelper<TKey>* m_Data;

			// Token: 0x040004BB RID: 1211
			internal int m_Index;

			// Token: 0x040004BC RID: 1212
			internal int m_BucketIndex;

			// Token: 0x040004BD RID: 1213
			internal int m_NextIndex;
		}
	}
}
