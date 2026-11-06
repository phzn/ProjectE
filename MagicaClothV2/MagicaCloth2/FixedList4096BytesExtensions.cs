using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000116 RID: 278
	public static class FixedList4096BytesExtensions
	{
		// Token: 0x060005A2 RID: 1442 RVA: 0x0002F760 File Offset: 0x0002D960
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool MC2IsCapacity<[IsUnmanaged] T>(this FixedList4096Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			return fixedList.Length >= fixedList.Capacity;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0002F773 File Offset: 0x0002D973
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Set<[IsUnmanaged] T>(this FixedList4096Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			if (!(ref fixedList).Contains(item))
			{
				fixedList.Add(in item);
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0002F786 File Offset: 0x0002D986
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2SetLimit<[IsUnmanaged] T>(this FixedList4096Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			if (fixedList.Length >= fixedList.Capacity)
			{
				Debug.LogWarning(string.Format("FixedSet4096.Limit!:{0}", fixedList.Capacity));
				return;
			}
			if (!(ref fixedList).Contains(item))
			{
				fixedList.Add(in item);
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0002F7C4 File Offset: 0x0002D9C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2RemoveItemAtSwapBack<[IsUnmanaged] T>(this FixedList4096Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x060005A6 RID: 1446 RVA: 0x0002F7FF File Offset: 0x0002D9FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Push<[IsUnmanaged] T>(this FixedList4096Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			fixedList.Add(in item);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0002F80C File Offset: 0x0002DA0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MC2Pop<[IsUnmanaged] T>(this FixedList4096Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			int num = fixedList.Length - 1;
			T t = fixedList[num];
			fixedList.RemoveAt(num);
			return t;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0002F7FF File Offset: 0x0002D9FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Enqueue<[IsUnmanaged] T>(this FixedList4096Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			fixedList.Add(in item);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0002F830 File Offset: 0x0002DA30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MC2Dequque<[IsUnmanaged] T>(this FixedList4096Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			T t = fixedList[0];
			fixedList.RemoveAt(0);
			return t;
		}
	}
}
