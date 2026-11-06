using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x0200011A RID: 282
	public static class NativeMultiHashMapExtensions
	{
		// Token: 0x060005BE RID: 1470 RVA: 0x0002FAFC File Offset: 0x0002DCFC
		public static bool MC2Contains<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(this NativeParallelMultiHashMap<TKey, TValue> map, TKey key, TValue value) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType, IEquatable<TValue>
		{
			foreach (TValue tvalue in map.GetValuesForKey(key))
			{
				if (tvalue.Equals(value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0002FB64 File Offset: 0x0002DD64
		public static void MC2UniqueAdd<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(this NativeParallelMultiHashMap<TKey, TValue> map, TKey key, TValue value) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType, IEquatable<TValue>
		{
			if (!(ref map).MC2Contains(key, value))
			{
				map.Add(key, value);
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0002FB78 File Offset: 0x0002DD78
		public static bool MC2RemoveValue<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(this NativeParallelMultiHashMap<TKey, TValue> map, TKey key, TValue value) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType, IEquatable<TValue>
		{
			TValue tvalue;
			NativeParallelMultiHashMapIterator<TKey> nativeParallelMultiHashMapIterator;
			if (map.TryGetFirstValue(key, out tvalue, out nativeParallelMultiHashMapIterator))
			{
				while (!tvalue.Equals(value))
				{
					if (!map.TryGetNextValue(out tvalue, ref nativeParallelMultiHashMapIterator))
					{
						return false;
					}
				}
				map.Remove(nativeParallelMultiHashMapIterator);
				return true;
			}
			return false;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0002FBB8 File Offset: 0x0002DDB8
		public static FixedList512Bytes<TValue> MC2ToFixedList512Bytes<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(this NativeParallelMultiHashMap<TKey, TValue> map, TKey key) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType, IEquatable<TValue>
		{
			FixedList512Bytes<TValue> fixedList512Bytes = default(FixedList512Bytes<TValue>);
			if (map.ContainsKey(key))
			{
				foreach (TValue tvalue in map.GetValuesForKey(key))
				{
					fixedList512Bytes.Add(in tvalue);
				}
			}
			return fixedList512Bytes;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0002FC24 File Offset: 0x0002DE24
		public static FixedList128Bytes<TValue> MC2ToFixedList128Bytes<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(this NativeParallelMultiHashMap<TKey, TValue> map, TKey key) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType, IEquatable<TValue>
		{
			FixedList128Bytes<TValue> fixedList128Bytes = default(FixedList128Bytes<TValue>);
			if (map.ContainsKey(key))
			{
				foreach (TValue tvalue in map.GetValuesForKey(key))
				{
					fixedList128Bytes.Add(in tvalue);
				}
			}
			return fixedList128Bytes;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0002FC90 File Offset: 0x0002DE90
		public static ValueTuple<TKey[], TValue[]> MC2Serialize<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(this NativeParallelMultiHashMap<TKey, TValue> map) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
		{
			if (!map.IsCreated || map.Count() == 0 || map.IsEmpty)
			{
				return new ValueTuple<TKey[], TValue[]>(null, null);
			}
			ValueTuple<TKey[], TValue[]> valueTuple;
			using (NativeArray<TKey> keyArray = map.GetKeyArray(Allocator.Persistent))
			{
				using (NativeArray<TValue> valueArray = map.GetValueArray(Allocator.Persistent))
				{
					valueTuple = new ValueTuple<TKey[], TValue[]>(keyArray.ToArray(), valueArray.ToArray());
				}
			}
			return valueTuple;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0002FD28 File Offset: 0x0002DF28
		public static NativeParallelMultiHashMap<int2, ushort> MC2Deserialize(int2[] keyArray, ushort[] valueArray)
		{
			int num = ((keyArray != null) ? keyArray.Length : 0);
			int num2 = ((valueArray != null) ? valueArray.Length : 0);
			NativeParallelMultiHashMap<int2, ushort> nativeParallelMultiHashMap = new NativeParallelMultiHashMap<int2, ushort>(num, Allocator.Persistent);
			if (num > 0 && num2 > 0)
			{
				using (NativeArray<int2> nativeArray = new NativeArray<int2>(keyArray, Allocator.Persistent))
				{
					using (NativeArray<ushort> nativeArray2 = new NativeArray<ushort>(valueArray, Allocator.Persistent))
					{
						new NativeMultiHashMapExtensions.SetParallelMultiHashMapJob<int2, ushort>
						{
							map = nativeParallelMultiHashMap,
							keyArray = nativeArray,
							valueArray = nativeArray2
						}.Run<NativeMultiHashMapExtensions.SetParallelMultiHashMapJob<int2, ushort>>();
					}
				}
			}
			return nativeParallelMultiHashMap;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0002FDD4 File Offset: 0x0002DFD4
		public static void MC2DisposeSafe<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(this NativeParallelMultiHashMap<TKey, TValue> map) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType, IEquatable<TValue>
		{
			if (map.IsCreated)
			{
				map.Dispose();
			}
		}

		// Token: 0x0200011B RID: 283
		[BurstCompile]
		private struct SetParallelMultiHashMapJob<[IsUnmanaged] TKey, [IsUnmanaged] TValue> : IJob where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
		{
			// Token: 0x060005C6 RID: 1478 RVA: 0x0002FDE4 File Offset: 0x0002DFE4
			public void Execute()
			{
				int length = this.keyArray.Length;
				for (int i = 0; i < length; i++)
				{
					this.map.Add(this.keyArray[i], this.valueArray[i]);
				}
			}

			// Token: 0x040008B4 RID: 2228
			public NativeParallelMultiHashMap<TKey, TValue> map;

			// Token: 0x040008B5 RID: 2229
			[ReadOnly]
			public NativeArray<TKey> keyArray;

			// Token: 0x040008B6 RID: 2230
			[ReadOnly]
			public NativeArray<TValue> valueArray;
		}
	}
}
