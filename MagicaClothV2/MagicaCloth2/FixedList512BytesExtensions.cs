using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000117 RID: 279
	public static class FixedList512BytesExtensions
	{
		// Token: 0x060005AA RID: 1450 RVA: 0x0002F840 File Offset: 0x0002DA40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool MC2IsCapacity<[IsUnmanaged] T>(this FixedList512Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			return fixedList.Length >= fixedList.Capacity;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0002F853 File Offset: 0x0002DA53
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Set<[IsUnmanaged] T>(this FixedList512Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			if (!(ref fixedList).Contains(item))
			{
				fixedList.Add(in item);
			}
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0002F866 File Offset: 0x0002DA66
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2SetLimit<[IsUnmanaged] T>(this FixedList512Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			if (fixedList.Length >= fixedList.Capacity)
			{
				Debug.LogWarning(string.Format("FixedSet512.Limit!:{0}", fixedList.Capacity));
				return;
			}
			if (!(ref fixedList).Contains(item))
			{
				fixedList.Add(in item);
			}
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0002F8A4 File Offset: 0x0002DAA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2RemoveItemAtSwapBack<[IsUnmanaged] T>(this FixedList512Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x060005AE RID: 1454 RVA: 0x0002F8DF File Offset: 0x0002DADF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Push<[IsUnmanaged] T>(this FixedList512Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			fixedList.Add(in item);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0002F8EC File Offset: 0x0002DAEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MC2Pop<[IsUnmanaged] T>(this FixedList512Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			int num = fixedList.Length - 1;
			T t = fixedList[num];
			fixedList.RemoveAt(num);
			return t;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0002F8DF File Offset: 0x0002DADF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Enqueue<[IsUnmanaged] T>(this FixedList512Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			fixedList.Add(in item);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0002F910 File Offset: 0x0002DB10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MC2Dequque<[IsUnmanaged] T>(this FixedList512Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			T t = fixedList[0];
			fixedList.RemoveAt(0);
			return t;
		}
	}
}
