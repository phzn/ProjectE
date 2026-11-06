using System;
using System.Collections.Generic;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000147 RID: 327
	public static class ReadOnlyArrayExtensions
	{
		// Token: 0x06001206 RID: 4614 RVA: 0x000559F0 File Offset: 0x00053BF0
		public static bool Contains<TValue>(this ReadOnlyArray<TValue> array, TValue value) where TValue : IComparable<TValue>
		{
			for (int i = 0; i < array.m_Length; i++)
			{
				if (array.m_Array[array.m_StartIndex + i].CompareTo(value) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x00055A34 File Offset: 0x00053C34
		public static bool ContainsReference<TValue>(this ReadOnlyArray<TValue> array, TValue value) where TValue : class
		{
			return array.IndexOfReference(value) != -1;
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x00055A44 File Offset: 0x00053C44
		public static int IndexOfReference<TValue>(this ReadOnlyArray<TValue> array, TValue value) where TValue : class
		{
			for (int i = 0; i < array.m_Length; i++)
			{
				if (array.m_Array[array.m_StartIndex + i] == value)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00055A88 File Offset: 0x00053C88
		internal static bool HaveEqualReferences<TValue>(this ReadOnlyArray<TValue> array1, IReadOnlyList<TValue> array2, int count = 2147483647)
		{
			int num = Math.Min(array1.Count, count);
			int num2 = Math.Min(array2.Count, count);
			if (num != num2)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (array1[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
