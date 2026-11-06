using System;

namespace DA_Assets.SVGMeshUnity.Internals
{
	// Token: 0x0200000A RID: 10
	public static class Sort<T> where T : IComparable<T>
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00003C84 File Offset: 0x00001E84
		public static void QuickSort(T[] elements, int left, int right)
		{
			int i = left;
			int num = right;
			T t = elements[left + right >> 1];
			while (i <= num)
			{
				while (elements[i].CompareTo(t) < 0)
				{
					i++;
				}
				while (elements[num].CompareTo(t) > 0)
				{
					num--;
				}
				if (i <= num)
				{
					T t2 = elements[i];
					elements[i] = elements[num];
					elements[num] = t2;
					i++;
					num--;
				}
			}
			if (left < num)
			{
				Sort<T>.QuickSort(elements, left, num);
			}
			if (i < right)
			{
				Sort<T>.QuickSort(elements, i, right);
			}
		}
	}
}
