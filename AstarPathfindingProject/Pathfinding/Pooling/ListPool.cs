using System;
using System.Collections.Generic;

namespace Pathfinding.Pooling
{
	// Token: 0x02000256 RID: 598
	public static class ListPool<T>
	{
		// Token: 0x06000EE4 RID: 3812 RVA: 0x0005BF28 File Offset: 0x0005A128
		public static List<T> Claim()
		{
			List<List<T>> list = ListPool<T>.pool;
			List<T> list3;
			lock (list)
			{
				if (ListPool<T>.pool.Count > 0)
				{
					List<T> list2 = ListPool<T>.pool[ListPool<T>.pool.Count - 1];
					ListPool<T>.pool.RemoveAt(ListPool<T>.pool.Count - 1);
					ListPool<T>.inPool.Remove(list2);
					list3 = list2;
				}
				else
				{
					list3 = new List<T>();
				}
			}
			return list3;
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0005BFB4 File Offset: 0x0005A1B4
		private static int FindCandidate(List<List<T>> pool, int capacity)
		{
			List<T> list = null;
			int num = -1;
			int num2 = 0;
			while (num2 < pool.Count && num2 < 8)
			{
				List<T> list2 = pool[pool.Count - 1 - num2];
				if ((list == null || list2.Capacity > list.Capacity) && list2.Capacity < capacity * 16)
				{
					list = list2;
					num = pool.Count - 1 - num2;
					if (list.Capacity >= capacity)
					{
						return num;
					}
				}
				num2++;
			}
			return num;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0005C024 File Offset: 0x0005A224
		public static List<T> Claim(int capacity)
		{
			List<List<T>> list = ListPool<T>.pool;
			List<T> list3;
			lock (list)
			{
				List<List<T>> list2 = ListPool<T>.pool;
				int num = ListPool<T>.FindCandidate(ListPool<T>.pool, capacity);
				if (capacity > 5000)
				{
					int num2 = ListPool<T>.FindCandidate(ListPool<T>.largePool, capacity);
					if (num2 != -1)
					{
						list2 = ListPool<T>.largePool;
						num = num2;
					}
				}
				if (num == -1)
				{
					list3 = new List<T>(capacity);
				}
				else
				{
					List<T> list4 = list2[num];
					ListPool<T>.inPool.Remove(list4);
					list2[num] = list2[list2.Count - 1];
					list2.RemoveAt(list2.Count - 1);
					list3 = list4;
				}
			}
			return list3;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0005C0E0 File Offset: 0x0005A2E0
		public static void Warmup(int count, int size)
		{
			List<List<T>> list = ListPool<T>.pool;
			lock (list)
			{
				List<T>[] array = new List<T>[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = ListPool<T>.Claim(size);
				}
				for (int j = 0; j < count; j++)
				{
					ListPool<T>.Release(array[j]);
				}
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0005C150 File Offset: 0x0005A350
		public static void Release(ref List<T> list)
		{
			ListPool<T>.Release(list);
			list = null;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0005C15C File Offset: 0x0005A35C
		public static void Release(List<T> list)
		{
			list.ClearFast<T>();
			List<List<T>> list2 = ListPool<T>.pool;
			lock (list2)
			{
				if (list.Capacity > 5000)
				{
					ListPool<T>.largePool.Add(list);
					if (ListPool<T>.largePool.Count > 8)
					{
						ListPool<T>.largePool.RemoveAt(0);
					}
				}
				else
				{
					ListPool<T>.pool.Add(list);
				}
			}
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0005C1D8 File Offset: 0x0005A3D8
		public static void Clear()
		{
			List<List<T>> list = ListPool<T>.pool;
			lock (list)
			{
				ListPool<T>.pool.Clear();
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0005C21C File Offset: 0x0005A41C
		public static int GetSize()
		{
			return ListPool<T>.pool.Count;
		}

		// Token: 0x04000B26 RID: 2854
		private static readonly List<List<T>> pool = new List<List<T>>();

		// Token: 0x04000B27 RID: 2855
		private static readonly List<List<T>> largePool = new List<List<T>>();

		// Token: 0x04000B28 RID: 2856
		private static readonly HashSet<List<T>> inPool = new HashSet<List<T>>();

		// Token: 0x04000B29 RID: 2857
		private const int MaxCapacitySearchLength = 8;

		// Token: 0x04000B2A RID: 2858
		private const int LargeThreshold = 5000;

		// Token: 0x04000B2B RID: 2859
		private const int MaxLargePoolSize = 8;
	}
}
