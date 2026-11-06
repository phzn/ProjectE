using System;
using System.Collections.Generic;

namespace UnityEngine.Splines
{
	// Token: 0x02000003 RID: 3
	internal static class ArrayUtility
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public static void RemoveAt<T>(ref T[] array, int index)
		{
			if (index < 0 || index >= array.Length)
			{
				throw new IndexOutOfRangeException();
			}
			Array.Copy(array, index + 1, array, index, array.Length - index - 1);
			Array.Resize<T>(ref array, array.Length - 1);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020F4 File Offset: 0x000002F4
		public static void RemoveAt<T>(ref T[] array, IEnumerable<int> indices)
		{
			List<int> list = new List<int>(indices);
			list.Sort();
			ArrayUtility.SortedRemoveAt<T>(ref array, list);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002118 File Offset: 0x00000318
		public static void SortedRemoveAt<T>(ref T[] array, IList<int> sorted)
		{
			int count = sorted.Count;
			int num = array.Length;
			T[] array2 = new T[num - count];
			int i = 0;
			for (int j = 0; j < num; j++)
			{
				if (i < count && sorted[i] == j)
				{
					while (i < count)
					{
						if (sorted[i] != j)
						{
							break;
						}
						i++;
					}
				}
				else
				{
					array2[j - i] = array[j];
				}
			}
			array = array2;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002188 File Offset: 0x00000388
		public static void Remove<T>(ref T[] array, T element)
		{
			int num = Array.IndexOf<T>(array, element);
			if (num >= 0)
			{
				ArrayUtility.RemoveAt<T>(ref array, num);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021A9 File Offset: 0x000003A9
		public static void Add<T>(ref T[] array, T element)
		{
			Array.Resize<T>(ref array, array.Length + 1);
			array[array.Length - 1] = element;
		}
	}
}
