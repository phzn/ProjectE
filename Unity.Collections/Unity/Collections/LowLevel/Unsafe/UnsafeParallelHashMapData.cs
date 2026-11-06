using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000123 RID: 291
	[GenerateTestsForBurstCompatibility]
	[StructLayout(LayoutKind.Explicit)]
	internal struct UnsafeParallelHashMapData
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0002484D File Offset: 0x00022A4D
		internal unsafe int* firstFreeTLS
		{
			get
			{
				return (int*)((byte*)UnsafeUtility.AddressOf<UnsafeParallelHashMapData>(ref this) + 64);
			}
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00022590 File Offset: 0x00020790
		internal static int GetBucketSize(int capacity)
		{
			return capacity * 2;
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00024858 File Offset: 0x00022A58
		internal static int GrowCapacity(int capacity)
		{
			if (capacity == 0)
			{
				return 1;
			}
			return capacity * 2;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00024864 File Offset: 0x00022A64
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		internal unsafe static void AllocateHashMap<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(int length, int bucketLength, AllocatorManager.AllocatorHandle label, out UnsafeParallelHashMapData* outBuf) where TKey : struct, ValueType where TValue : struct, ValueType
		{
			int threadIndexCount = JobsUtility.ThreadIndexCount;
			UnsafeParallelHashMapData* ptr = (UnsafeParallelHashMapData*)Memory.Unmanaged.Allocate((long)(64 + 64 * threadIndexCount), 64, label);
			bucketLength = math.ceilpow2(bucketLength);
			ptr->keyCapacity = length;
			ptr->bucketCapacityMask = bucketLength - 1;
			int num2;
			int num3;
			int num4;
			int num = UnsafeParallelHashMapData.CalculateDataSize<TKey, TValue>(length, bucketLength, out num2, out num3, out num4);
			ptr->values = (byte*)Memory.Unmanaged.Allocate((long)num, 64, label);
			ptr->keys = ptr->values + num2;
			ptr->next = ptr->values + num3;
			ptr->buckets = ptr->values + num4;
			outBuf = ptr;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x000248F0 File Offset: 0x00022AF0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		internal unsafe static void ReallocateHashMap<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(UnsafeParallelHashMapData* data, int newCapacity, int newBucketCapacity, AllocatorManager.AllocatorHandle label) where TKey : struct, ValueType where TValue : struct, ValueType
		{
			newBucketCapacity = math.ceilpow2(newBucketCapacity);
			if (data->keyCapacity == newCapacity && data->bucketCapacityMask + 1 == newBucketCapacity)
			{
				return;
			}
			int num;
			int num2;
			int num3;
			byte* ptr = (byte*)Memory.Unmanaged.Allocate((long)UnsafeParallelHashMapData.CalculateDataSize<TKey, TValue>(newCapacity, newBucketCapacity, out num, out num2, out num3), 64, label);
			byte* ptr2 = ptr + num;
			byte* ptr3 = ptr + num2;
			byte* ptr4 = ptr + num3;
			UnsafeUtility.MemCpy((void*)ptr, (void*)data->values, (long)(data->keyCapacity * UnsafeUtility.SizeOf<TValue>()));
			UnsafeUtility.MemCpy((void*)ptr2, (void*)data->keys, (long)(data->keyCapacity * UnsafeUtility.SizeOf<TKey>()));
			UnsafeUtility.MemCpy((void*)ptr3, (void*)data->next, (long)(data->keyCapacity * UnsafeUtility.SizeOf<int>()));
			for (int i = data->keyCapacity; i < newCapacity; i++)
			{
				*(int*)(ptr3 + (IntPtr)i * 4) = -1;
			}
			for (int j = 0; j < newBucketCapacity; j++)
			{
				*(int*)(ptr4 + (IntPtr)j * 4) = -1;
			}
			for (int k = 0; k <= data->bucketCapacityMask; k++)
			{
				int* ptr5 = (int*)data->buckets;
				int* ptr6 = (int*)ptr3;
				while (ptr5[k] >= 0)
				{
					int num4 = ptr5[k];
					ptr5[k] = ptr6[num4];
					TKey tkey = UnsafeUtility.ReadArrayElement<TKey>((void*)data->keys, num4);
					int num5 = tkey.GetHashCode() & (newBucketCapacity - 1);
					ptr6[num4] = *(int*)(ptr4 + (IntPtr)num5 * 4);
					*(int*)(ptr4 + (IntPtr)num5 * 4) = num4;
				}
			}
			Memory.Unmanaged.Free<byte>(data->values, label);
			if (data->allocatedIndexLength > data->keyCapacity)
			{
				data->allocatedIndexLength = data->keyCapacity;
			}
			data->values = ptr;
			data->keys = ptr2;
			data->next = ptr3;
			data->buckets = ptr4;
			data->keyCapacity = newCapacity;
			data->bucketCapacityMask = newBucketCapacity - 1;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00024AA9 File Offset: 0x00022CA9
		internal unsafe static void DeallocateHashMap(UnsafeParallelHashMapData* data, AllocatorManager.AllocatorHandle allocator)
		{
			Memory.Unmanaged.Free<byte>(data->values, allocator);
			Memory.Unmanaged.Free<UnsafeParallelHashMapData>(data, allocator);
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00024AC0 File Offset: 0x00022CC0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		internal static int CalculateDataSize<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(int length, int bucketLength, out int keyOffset, out int nextOffset, out int bucketOffset) where TKey : struct, ValueType where TValue : struct, ValueType
		{
			int num = UnsafeUtility.SizeOf<TValue>();
			int num2 = UnsafeUtility.SizeOf<TKey>();
			int num3 = UnsafeUtility.SizeOf<int>();
			int num4 = CollectionHelper.Align(num * length, 64);
			int num5 = CollectionHelper.Align(num2 * length, 64);
			int num6 = CollectionHelper.Align(num3 * length, 64);
			int num7 = CollectionHelper.Align(num3 * bucketLength, 64);
			int num8 = num4 + num5 + num6 + num7;
			keyOffset = num4;
			nextOffset = keyOffset + num5;
			bucketOffset = nextOffset + num6;
			return num8;
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00024B28 File Offset: 0x00022D28
		internal unsafe static bool IsEmpty(UnsafeParallelHashMapData* data)
		{
			if (data->allocatedIndexLength <= 0)
			{
				return true;
			}
			int* ptr = (int*)data->buckets;
			int* ptr2 = (int*)data->next;
			int num = data->bucketCapacityMask;
			for (int i = 0; i <= num; i++)
			{
				if (ptr[i] != -1)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00024B70 File Offset: 0x00022D70
		internal unsafe static int GetCount(UnsafeParallelHashMapData* data)
		{
			if (data->allocatedIndexLength <= 0)
			{
				return 0;
			}
			int* ptr = (int*)data->next;
			int num = 0;
			int threadIndexCount = JobsUtility.ThreadIndexCount;
			for (int i = 0; i < threadIndexCount; i++)
			{
				for (int j = data->firstFreeTLS[i * 16]; j >= 0; j = ptr[j])
				{
					num++;
				}
			}
			return math.min(data->keyCapacity, data->allocatedIndexLength) - num;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00024BDC File Offset: 0x00022DDC
		internal unsafe static bool MoveNextSearch(UnsafeParallelHashMapData* data, ref int bucketIndex, ref int nextIndex, out int index)
		{
			int* ptr = (int*)data->buckets;
			int num = data->bucketCapacityMask;
			for (int i = bucketIndex; i <= num; i++)
			{
				int num2 = ptr[i];
				if (num2 != -1)
				{
					int* ptr2 = (int*)data->next;
					index = num2;
					bucketIndex = i + 1;
					nextIndex = ptr2[num2];
					return true;
				}
			}
			index = -1;
			bucketIndex = num + 1;
			nextIndex = -1;
			return false;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x00024C38 File Offset: 0x00022E38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static bool MoveNext(UnsafeParallelHashMapData* data, ref int bucketIndex, ref int nextIndex, out int index)
		{
			if (nextIndex != -1)
			{
				int* ptr = (int*)data->next;
				index = nextIndex;
				nextIndex = ptr[nextIndex];
				return true;
			}
			return UnsafeParallelHashMapData.MoveNextSearch(data, ref bucketIndex, ref nextIndex, out index);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00024C6C File Offset: 0x00022E6C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		internal unsafe static void GetKeyArray<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey>(UnsafeParallelHashMapData* data, NativeArray<TKey> result) where TKey : struct, ValueType
		{
			int* ptr = (int*)data->buckets;
			int* ptr2 = (int*)data->next;
			int num = 0;
			int num2 = 0;
			int length = result.Length;
			while (num <= data->bucketCapacityMask && num2 < length)
			{
				for (int num3 = ptr[num]; num3 != -1; num3 = ptr2[num3])
				{
					result[num2++] = UnsafeUtility.ReadArrayElement<TKey>((void*)data->keys, num3);
				}
				num++;
			}
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00024CDC File Offset: 0x00022EDC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		internal unsafe static void GetValueArray<[global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(UnsafeParallelHashMapData* data, NativeArray<TValue> result) where TValue : struct, ValueType
		{
			int* ptr = (int*)data->buckets;
			int* ptr2 = (int*)data->next;
			int num = 0;
			int num2 = 0;
			int length = result.Length;
			int num3 = data->bucketCapacityMask;
			while (num <= num3 && num2 < length)
			{
				for (int num4 = ptr[num]; num4 != -1; num4 = ptr2[num4])
				{
					result[num2++] = UnsafeUtility.ReadArrayElement<TValue>((void*)data->values, num4);
				}
				num++;
			}
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00024D50 File Offset: 0x00022F50
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		internal unsafe static void GetKeyValueArrays<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(UnsafeParallelHashMapData* data, NativeKeyValueArrays<TKey, TValue> result) where TKey : struct, ValueType where TValue : struct, ValueType
		{
			int* ptr = (int*)data->buckets;
			int* ptr2 = (int*)data->next;
			int num = 0;
			int num2 = 0;
			int length = result.Length;
			int num3 = data->bucketCapacityMask;
			while (num <= num3 && num2 < length)
			{
				for (int num4 = ptr[num]; num4 != -1; num4 = ptr2[num4])
				{
					result.Keys[num2] = UnsafeUtility.ReadArrayElement<TKey>((void*)data->keys, num4);
					result.Values[num2] = UnsafeUtility.ReadArrayElement<TValue>((void*)data->values, num4);
					num2++;
				}
				num++;
			}
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00024DE2 File Offset: 0x00022FE2
		internal UnsafeParallelHashMapBucketData GetBucketData()
		{
			return new UnsafeParallelHashMapBucketData(this.values, this.keys, this.next, this.buckets, this.bucketCapacityMask);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00024E07 File Offset: 0x00023007
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe static void CheckHashMapReallocateDoesNotShrink(UnsafeParallelHashMapData* data, int newCapacity)
		{
			if (data->keyCapacity > newCapacity)
			{
				throw new InvalidOperationException("Shrinking a hash map is not supported");
			}
		}

		// Token: 0x040004EC RID: 1260
		[FieldOffset(0)]
		internal unsafe byte* values;

		// Token: 0x040004ED RID: 1261
		[FieldOffset(8)]
		internal unsafe byte* keys;

		// Token: 0x040004EE RID: 1262
		[FieldOffset(16)]
		internal unsafe byte* next;

		// Token: 0x040004EF RID: 1263
		[FieldOffset(24)]
		internal unsafe byte* buckets;

		// Token: 0x040004F0 RID: 1264
		[FieldOffset(32)]
		internal int keyCapacity;

		// Token: 0x040004F1 RID: 1265
		[FieldOffset(36)]
		internal int bucketCapacityMask;

		// Token: 0x040004F2 RID: 1266
		[FieldOffset(40)]
		internal int allocatedIndexLength;

		// Token: 0x040004F3 RID: 1267
		private const int kFirstFreeTLSOffset = 64;

		// Token: 0x040004F4 RID: 1268
		internal const int IntsPerCacheLine = 16;
	}
}
