using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000118 RID: 280
	public static class FixedList64BytesExtensions
	{
		// Token: 0x060005B2 RID: 1458 RVA: 0x0002F920 File Offset: 0x0002DB20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool MC2IsCapacity<[IsUnmanaged] T>(this FixedList64Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			return fixedList.Length >= fixedList.Capacity;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0002F933 File Offset: 0x0002DB33
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Set<[IsUnmanaged] T>(this FixedList64Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			if (!(ref fixedList).Contains(item))
			{
				fixedList.Add(in item);
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0002F946 File Offset: 0x0002DB46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2SetLimit<[IsUnmanaged] T>(this FixedList64Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			if (fixedList.Length >= fixedList.Capacity)
			{
				Debug.LogWarning(string.Format("FixedSet64.Limit!:{0}", fixedList.Capacity));
				return;
			}
			if (!(ref fixedList).Contains(item))
			{
				fixedList.Add(in item);
			}
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0002F984 File Offset: 0x0002DB84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2RemoveItemAtSwapBack<[IsUnmanaged] T>(this FixedList64Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			for (int i = 0; i < fixedList.Length; i++)
			{
				if (fixedList.ElementAt(i).Equals(item))
				{
					fixedList.RemoveAtSwapBack(i);
					return;
				}
			}
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0002F9BF File Offset: 0x0002DBBF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Push<[IsUnmanaged] T>(this FixedList64Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			fixedList.Add(in item);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0002F9CC File Offset: 0x0002DBCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MC2Pop<[IsUnmanaged] T>(this FixedList64Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			int num = fixedList.Length - 1;
			T t = fixedList[num];
			fixedList.RemoveAt(num);
			return t;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0002F9BF File Offset: 0x0002DBBF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Enqueue<[IsUnmanaged] T>(this FixedList64Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			fixedList.Add(in item);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0002F9F0 File Offset: 0x0002DBF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MC2Dequque<[IsUnmanaged] T>(this FixedList64Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			T t = fixedList[0];
			fixedList.RemoveAt(0);
			return t;
		}
	}
}
