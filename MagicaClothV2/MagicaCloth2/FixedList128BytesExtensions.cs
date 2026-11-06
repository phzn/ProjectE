using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000114 RID: 276
	public static class FixedList128BytesExtensions
	{
		// Token: 0x06000592 RID: 1426 RVA: 0x0002F5A0 File Offset: 0x0002D7A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool MC2IsCapacity<[IsUnmanaged] T>(this FixedList128Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			return fixedList.Length >= fixedList.Capacity;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0002F5B3 File Offset: 0x0002D7B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Set<[IsUnmanaged] T>(this FixedList128Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			if (!(ref fixedList).Contains(item))
			{
				fixedList.Add(in item);
			}
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0002F5C6 File Offset: 0x0002D7C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2SetLimit<[IsUnmanaged] T>(this FixedList128Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			if (fixedList.Length >= fixedList.Capacity)
			{
				Debug.LogWarning(string.Format("FixedSet128.Limit!:{0}", fixedList.Capacity));
				return;
			}
			if (!(ref fixedList).Contains(item))
			{
				fixedList.Add(in item);
			}
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0002F604 File Offset: 0x0002D804
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2RemoveItemAtSwapBack<[IsUnmanaged] T>(this FixedList128Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000596 RID: 1430 RVA: 0x0002F63F File Offset: 0x0002D83F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Push<[IsUnmanaged] T>(this FixedList128Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			fixedList.Add(in item);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0002F64C File Offset: 0x0002D84C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MC2Pop<[IsUnmanaged] T>(this FixedList128Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			int num = fixedList.Length - 1;
			T t = fixedList[num];
			fixedList.RemoveAt(num);
			return t;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0002F63F File Offset: 0x0002D83F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Enqueue<[IsUnmanaged] T>(this FixedList128Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			fixedList.Add(in item);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0002F670 File Offset: 0x0002D870
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MC2Dequque<[IsUnmanaged] T>(this FixedList128Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			T t = fixedList[0];
			fixedList.RemoveAt(0);
			return t;
		}
	}
}
