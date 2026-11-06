using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000115 RID: 277
	public static class FixedList32BytesExtensions
	{
		// Token: 0x0600059A RID: 1434 RVA: 0x0002F680 File Offset: 0x0002D880
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool MC2IsCapacity<[IsUnmanaged] T>(this FixedList32Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			return fixedList.Length >= fixedList.Capacity;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0002F693 File Offset: 0x0002D893
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Set<[IsUnmanaged] T>(this FixedList32Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			if (!(ref fixedList).Contains(item))
			{
				fixedList.Add(in item);
			}
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0002F6A6 File Offset: 0x0002D8A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2SetLimit<[IsUnmanaged] T>(this FixedList32Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			if (fixedList.Length >= fixedList.Capacity)
			{
				Debug.LogWarning(string.Format("FixedSet32.Limit!:{0}", fixedList.Capacity));
				return;
			}
			if (!(ref fixedList).Contains(item))
			{
				fixedList.Add(in item);
			}
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0002F6E4 File Offset: 0x0002D8E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2RemoveItemAtSwapBack<[IsUnmanaged] T>(this FixedList32Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x0600059E RID: 1438 RVA: 0x0002F71F File Offset: 0x0002D91F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Push<[IsUnmanaged] T>(this FixedList32Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			fixedList.Add(in item);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0002F72C File Offset: 0x0002D92C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MC2Pop<[IsUnmanaged] T>(this FixedList32Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			int num = fixedList.Length - 1;
			T t = fixedList[num];
			fixedList.RemoveAt(num);
			return t;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0002F71F File Offset: 0x0002D91F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2Enqueue<[IsUnmanaged] T>(this FixedList32Bytes<T> fixedList, T item) where T : struct, ValueType, IEquatable<T>
		{
			fixedList.Add(in item);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0002F750 File Offset: 0x0002D950
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MC2Dequque<[IsUnmanaged] T>(this FixedList32Bytes<T> fixedList) where T : struct, ValueType, IEquatable<T>
		{
			T t = fixedList[0];
			fixedList.RemoveAt(0);
			return t;
		}
	}
}
