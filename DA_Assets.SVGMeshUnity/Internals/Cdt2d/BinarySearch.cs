using System;

namespace DA_Assets.SVGMeshUnity.Internals.Cdt2d
{
	// Token: 0x0200000D RID: 13
	public static class BinarySearch
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00004160 File Offset: 0x00002360
		public static int GE<G, E>(G[] a, E y, BinarySearch.IComparer<G, E> c, int l, int h)
		{
			int num = h + 1;
			while (l <= h)
			{
				int num2 = l + h >> 1;
				G g = a[num2];
				if (c.Compare(g, y) >= 0)
				{
					num = num2;
					h = num2 - 1;
				}
				else
				{
					l = num2 + 1;
				}
			}
			return num;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000041A4 File Offset: 0x000023A4
		public static int GE<G>(G[] a, G y, int l, int h) where G : IComparable<G>
		{
			int num = h + 1;
			while (l <= h)
			{
				int num2 = l + h >> 1;
				G g = a[num2];
				if (g.CompareTo(y) >= 0)
				{
					num = num2;
					h = num2 - 1;
				}
				else
				{
					l = num2 + 1;
				}
			}
			return num;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000041EC File Offset: 0x000023EC
		public static int GT<G, E>(G[] a, E y, BinarySearch.IComparer<G, E> c, int l, int h)
		{
			int num = h + 1;
			while (l <= h)
			{
				int num2 = l + h >> 1;
				G g = a[num2];
				if (c.Compare(g, y) > 0)
				{
					num = num2;
					h = num2 - 1;
				}
				else
				{
					l = num2 + 1;
				}
			}
			return num;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004230 File Offset: 0x00002430
		public static int GT<G>(G[] a, G y, int l, int h) where G : IComparable<G>
		{
			int num = h + 1;
			while (l <= h)
			{
				int num2 = l + h >> 1;
				G g = a[num2];
				if (g.CompareTo(y) > 0)
				{
					num = num2;
					h = num2 - 1;
				}
				else
				{
					l = num2 + 1;
				}
			}
			return num;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004278 File Offset: 0x00002478
		public static int LT<G, E>(G[] a, E y, BinarySearch.IComparer<G, E> c, int l, int h)
		{
			int num = l - 1;
			while (l <= h)
			{
				int num2 = l + h >> 1;
				G g = a[num2];
				if (c.Compare(g, y) < 0)
				{
					num = num2;
					l = num2 + 1;
				}
				else
				{
					h = num2 - 1;
				}
			}
			return num;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000042BC File Offset: 0x000024BC
		public static int LT<G>(G[] a, G y, int l, int h) where G : IComparable<G>
		{
			int num = l - 1;
			while (l <= h)
			{
				int num2 = l + h >> 1;
				G g = a[num2];
				if (g.CompareTo(y) < 0)
				{
					num = num2;
					l = num2 + 1;
				}
				else
				{
					h = num2 - 1;
				}
			}
			return num;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004304 File Offset: 0x00002504
		public static int LE<G, E>(G[] a, E y, BinarySearch.IComparer<G, E> c, int l, int h)
		{
			int num = l - 1;
			while (l <= h)
			{
				int num2 = l + h >> 1;
				G g = a[num2];
				if (c.Compare(g, y) <= 0)
				{
					num = num2;
					l = num2 + 1;
				}
				else
				{
					h = num2 - 1;
				}
			}
			return num;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004348 File Offset: 0x00002548
		public static int LE<G>(G[] a, G y, int l, int h) where G : IComparable<G>
		{
			int num = l - 1;
			while (l <= h)
			{
				int num2 = l + h >> 1;
				G g = a[num2];
				if (g.CompareTo(y) <= 0)
				{
					num = num2;
					l = num2 + 1;
				}
				else
				{
					h = num2 - 1;
				}
			}
			return num;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004390 File Offset: 0x00002590
		public static int EQ<G, E>(G[] a, E y, BinarySearch.IComparer<G, E> c, int l, int h)
		{
			while (l <= h)
			{
				int num = l + h >> 1;
				G g = a[num];
				int num2 = c.Compare(g, y);
				if (num2 == 0)
				{
					return num;
				}
				if (num2 <= 0)
				{
					l = num + 1;
				}
				else
				{
					h = num - 1;
				}
			}
			return -1;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000043D4 File Offset: 0x000025D4
		public static int EQ<G>(G[] a, G y, int l, int h) where G : IComparable<G>
		{
			while (l <= h)
			{
				int num = l + h >> 1;
				G g = a[num];
				int num2 = g.CompareTo(y);
				if (num2 == 0)
				{
					return num;
				}
				if (num2 <= 0)
				{
					l = num + 1;
				}
				else
				{
					h = num - 1;
				}
			}
			return -1;
		}

		// Token: 0x02000019 RID: 25
		public interface IComparer<G, E>
		{
			// Token: 0x060000A4 RID: 164
			int Compare(G x, E y);
		}
	}
}
