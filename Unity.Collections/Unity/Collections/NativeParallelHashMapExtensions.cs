using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000A6 RID: 166
	[GenerateTestsForBurstCompatibility]
	public static class NativeParallelHashMapExtensions
	{
		// Token: 0x06000815 RID: 2069 RVA: 0x000189B0 File Offset: 0x00016BB0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static int Unique<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T> array) where T : struct, ValueType, IEquatable<T>
		{
			if (array.Length == 0)
			{
				return 0;
			}
			int num = 0;
			int length = array.Length;
			int num2 = num;
			while (++num != length)
			{
				T t = array[num2];
				if (!t.Equals(array[num]))
				{
					array[++num2] = array[num];
				}
			}
			return num2 + 1;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00018A18 File Offset: 0x00016C18
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static ValueTuple<NativeArray<TKey>, int> GetUniqueKeyArray<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(this UnsafeParallelMultiHashMap<TKey, TValue> container, AllocatorManager.AllocatorHandle allocator) where TKey : struct, ValueType, IEquatable<TKey>, IComparable<TKey> where TValue : struct, ValueType
		{
			NativeArray<TKey> keyArray = container.GetKeyArray(allocator);
			keyArray.Sort<TKey>();
			int num = keyArray.Unique<TKey>();
			return new ValueTuple<NativeArray<TKey>, int>(keyArray, num);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00018A40 File Offset: 0x00016C40
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static ValueTuple<NativeArray<TKey>, int> GetUniqueKeyArray<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(this NativeParallelMultiHashMap<TKey, TValue> container, AllocatorManager.AllocatorHandle allocator) where TKey : struct, ValueType, IEquatable<TKey>, IComparable<TKey> where TValue : struct, ValueType
		{
			NativeArray<TKey> keyArray = container.GetKeyArray(allocator);
			keyArray.Sort<TKey>();
			int num = keyArray.Unique<TKey>();
			return new ValueTuple<NativeArray<TKey>, int>(keyArray, num);
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00018A68 File Offset: 0x00016C68
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static UnsafeParallelHashMapBucketData GetUnsafeBucketData<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(this NativeParallelHashMap<TKey, TValue> container) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
		{
			return container.m_HashMapData.m_Buffer->GetBucketData();
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00018A7A File Offset: 0x00016C7A
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static UnsafeParallelHashMapBucketData GetUnsafeBucketData<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(this NativeParallelMultiHashMap<TKey, TValue> container) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
		{
			return container.m_MultiHashMapData.m_Buffer->GetBucketData();
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00018A8C File Offset: 0x00016C8C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static void Remove<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue>(this NativeParallelMultiHashMap<TKey, TValue> container, TKey key, TValue value) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType, IEquatable<TValue>
		{
			container.m_MultiHashMapData.Remove<TValue>(key, value);
		}
	}
}
