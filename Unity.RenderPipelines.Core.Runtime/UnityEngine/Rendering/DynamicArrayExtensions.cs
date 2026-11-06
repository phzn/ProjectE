using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000040 RID: 64
	public static class DynamicArrayExtensions
	{
		// Token: 0x06000254 RID: 596 RVA: 0x0000B648 File Offset: 0x00009848
		private static int Partition<T>(T[] data, int left, int right) where T : IComparable<T>, new()
		{
			T t = data[left];
			left--;
			right++;
			for (;;)
			{
				T t2 = default(T);
				int num;
				do
				{
					left++;
					t2 = data[left];
					num = t2.CompareTo(t);
				}
				while (num < 0);
				T t3 = default(T);
				do
				{
					right--;
					t3 = data[right];
					num = t3.CompareTo(t);
				}
				while (num > 0);
				if (left >= right)
				{
					break;
				}
				data[right] = t2;
				data[left] = t3;
			}
			return right;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000B6D0 File Offset: 0x000098D0
		private static void QuickSort<T>(T[] data, int left, int right) where T : IComparable<T>, new()
		{
			if (left < right)
			{
				int num = DynamicArrayExtensions.Partition<T>(data, left, right);
				if (num >= 1)
				{
					DynamicArrayExtensions.QuickSort<T>(data, left, num);
				}
				if (num + 1 < right)
				{
					DynamicArrayExtensions.QuickSort<T>(data, num + 1, right);
				}
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000B706 File Offset: 0x00009906
		public static void QuickSort<T>(this DynamicArray<T> array) where T : IComparable<T>, new()
		{
			DynamicArrayExtensions.QuickSort<T>(array, 0, array.size - 1);
			array.BumpVersion();
		}
	}
}
