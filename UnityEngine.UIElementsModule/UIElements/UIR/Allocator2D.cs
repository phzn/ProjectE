using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000426 RID: 1062
	internal class Allocator2D
	{
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x0007F8EB File Offset: 0x0007DAEB
		public Vector2Int minSize
		{
			get
			{
				return this.m_MinSize;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060021DD RID: 8669 RVA: 0x0007F8F3 File Offset: 0x0007DAF3
		public Vector2Int maxSize
		{
			get
			{
				return this.m_MaxSize;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x0007F8FB File Offset: 0x0007DAFB
		public Vector2Int maxAllocSize
		{
			get
			{
				return this.m_MaxAllocSize;
			}
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x0007F903 File Offset: 0x0007DB03
		public Allocator2D(int minSize, int maxSize, int rowHeightBias)
			: this(new Vector2Int(minSize, minSize), new Vector2Int(maxSize, maxSize), rowHeightBias)
		{
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x0007F91C File Offset: 0x0007DB1C
		public Allocator2D(Vector2Int minSize, Vector2Int maxSize, int rowHeightBias)
		{
			Debug.Assert(minSize.x > 0 && minSize.x <= maxSize.x && minSize.y > 0 && minSize.y <= maxSize.y);
			Debug.Assert(minSize.x == UIRUtility.GetNextPow2(minSize.x) && minSize.y == UIRUtility.GetNextPow2(minSize.y) && maxSize.x == UIRUtility.GetNextPow2(maxSize.x) && maxSize.y == UIRUtility.GetNextPow2(maxSize.y));
			Debug.Assert(rowHeightBias >= 0);
			this.m_MinSize = minSize;
			this.m_MaxSize = maxSize;
			this.m_RowHeightBias = rowHeightBias;
			Allocator2D.BuildAreas(this.m_Areas, minSize, maxSize);
			this.m_MaxAllocSize = Allocator2D.ComputeMaxAllocSize(this.m_Areas, rowHeightBias);
			this.m_Rows = Allocator2D.BuildRowArray(this.m_MaxAllocSize.y, rowHeightBias);
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x0007FA34 File Offset: 0x0007DC34
		public bool TryAllocate(int width, int height, out Allocator2D.Alloc2D alloc2D)
		{
			bool flag = width < 1 || width > this.m_MaxAllocSize.x || height < 1 || height > this.m_MaxAllocSize.y;
			bool flag2;
			if (flag)
			{
				alloc2D = default(Allocator2D.Alloc2D);
				flag2 = false;
			}
			else
			{
				int nextPow2Exp = UIRUtility.GetNextPow2Exp(Mathf.Max(height - this.m_RowHeightBias, 1));
				for (Allocator2D.Row row = this.m_Rows[nextPow2Exp]; row != null; row = row.next)
				{
					bool flag3 = row.rect.width >= width;
					if (flag3)
					{
						Alloc alloc = row.allocator.Allocate((uint)width);
						bool flag4 = alloc.size > 0U;
						if (flag4)
						{
							alloc2D = new Allocator2D.Alloc2D(row, alloc, width, height);
							return true;
						}
					}
				}
				int num = (1 << nextPow2Exp) + this.m_RowHeightBias;
				Debug.Assert(num >= height);
				for (int i = 0; i < this.m_Areas.Count; i++)
				{
					Allocator2D.Area area = this.m_Areas[i];
					bool flag5 = area.rect.height >= num && area.rect.width >= width;
					if (flag5)
					{
						Alloc alloc2 = area.allocator.Allocate((uint)num);
						bool flag6 = alloc2.size > 0U;
						if (flag6)
						{
							Allocator2D.Row row = Allocator2D.Row.pool.Get();
							row.alloc = alloc2;
							row.allocator = new BestFitAllocator((uint)area.rect.width);
							row.area = area;
							row.next = this.m_Rows[nextPow2Exp];
							row.rect = new RectInt(area.rect.xMin, area.rect.yMin + (int)alloc2.start, area.rect.width, num);
							this.m_Rows[nextPow2Exp] = row;
							Alloc alloc3 = row.allocator.Allocate((uint)width);
							Debug.Assert(alloc3.size > 0U);
							alloc2D = new Allocator2D.Alloc2D(row, alloc3, width, height);
							return true;
						}
					}
				}
				alloc2D = default(Allocator2D.Alloc2D);
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x0007FC70 File Offset: 0x0007DE70
		public void Free(Allocator2D.Alloc2D alloc2D)
		{
			bool flag = alloc2D.alloc.size == 0U;
			if (!flag)
			{
				Allocator2D.Row row = alloc2D.row;
				row.allocator.Free(alloc2D.alloc);
				bool flag2 = row.allocator.highWatermark == 0U;
				if (flag2)
				{
					row.area.allocator.Free(row.alloc);
					int nextPow2Exp = UIRUtility.GetNextPow2Exp(row.rect.height - this.m_RowHeightBias);
					Allocator2D.Row row2 = this.m_Rows[nextPow2Exp];
					bool flag3 = row2 == row;
					if (flag3)
					{
						this.m_Rows[nextPow2Exp] = row.next;
					}
					else
					{
						Allocator2D.Row row3 = row2;
						while (row3.next != row)
						{
							row3 = row3.next;
						}
						row3.next = row.next;
					}
					Allocator2D.Row.pool.Return(row);
				}
			}
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x0007FD58 File Offset: 0x0007DF58
		private static void BuildAreas(List<Allocator2D.Area> areas, Vector2Int minSize, Vector2Int maxSize)
		{
			int num = Mathf.Min(minSize.x, minSize.y);
			int num2 = num;
			areas.Add(new Allocator2D.Area(new RectInt(0, 0, num, num2)));
			while (num < maxSize.x || num2 < maxSize.y)
			{
				bool flag = num < maxSize.x;
				if (flag)
				{
					areas.Add(new Allocator2D.Area(new RectInt(num, 0, num, num2)));
					num *= 2;
				}
				bool flag2 = num2 < maxSize.y;
				if (flag2)
				{
					areas.Add(new Allocator2D.Area(new RectInt(0, num2, num, num2)));
					num2 *= 2;
				}
			}
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x0007FE04 File Offset: 0x0007E004
		private static Vector2Int ComputeMaxAllocSize(List<Allocator2D.Area> areas, int rowHeightBias)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < areas.Count; i++)
			{
				Allocator2D.Area area = areas[i];
				num = Mathf.Max(area.rect.width, num);
				num2 = Mathf.Max(area.rect.height, num2);
			}
			return new Vector2Int(num, UIRUtility.GetPrevPow2(num2 - rowHeightBias) + rowHeightBias);
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x0007FE74 File Offset: 0x0007E074
		private static Allocator2D.Row[] BuildRowArray(int maxRowHeight, int rowHeightBias)
		{
			int num = UIRUtility.GetNextPow2Exp(maxRowHeight - rowHeightBias) + 1;
			return new Allocator2D.Row[num];
		}

		// Token: 0x04000E67 RID: 3687
		private readonly Vector2Int m_MinSize;

		// Token: 0x04000E68 RID: 3688
		private readonly Vector2Int m_MaxSize;

		// Token: 0x04000E69 RID: 3689
		private readonly Vector2Int m_MaxAllocSize;

		// Token: 0x04000E6A RID: 3690
		private readonly int m_RowHeightBias;

		// Token: 0x04000E6B RID: 3691
		private readonly Allocator2D.Row[] m_Rows;

		// Token: 0x04000E6C RID: 3692
		private readonly List<Allocator2D.Area> m_Areas = new List<Allocator2D.Area>();

		// Token: 0x02000427 RID: 1063
		public class Area
		{
			// Token: 0x060021E6 RID: 8678 RVA: 0x0007FE97 File Offset: 0x0007E097
			public Area(RectInt rect)
			{
				this.rect = rect;
				this.allocator = new BestFitAllocator((uint)rect.height);
			}

			// Token: 0x04000E6D RID: 3693
			public RectInt rect;

			// Token: 0x04000E6E RID: 3694
			public BestFitAllocator allocator;
		}

		// Token: 0x02000428 RID: 1064
		public class Row : LinkedPoolItem<Allocator2D.Row>
		{
			// Token: 0x060021E7 RID: 8679 RVA: 0x0007FEBA File Offset: 0x0007E0BA
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static Allocator2D.Row Create()
			{
				return new Allocator2D.Row();
			}

			// Token: 0x060021E8 RID: 8680 RVA: 0x0007FEC1 File Offset: 0x0007E0C1
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void Reset(Allocator2D.Row row)
			{
				row.rect = default(RectInt);
				row.area = null;
				row.allocator = null;
				row.alloc = default(Alloc);
				row.next = null;
			}

			// Token: 0x04000E6F RID: 3695
			public RectInt rect;

			// Token: 0x04000E70 RID: 3696
			public Allocator2D.Area area;

			// Token: 0x04000E71 RID: 3697
			public BestFitAllocator allocator;

			// Token: 0x04000E72 RID: 3698
			public Alloc alloc;

			// Token: 0x04000E73 RID: 3699
			public Allocator2D.Row next;

			// Token: 0x04000E74 RID: 3700
			public static readonly LinkedPool<Allocator2D.Row> pool = new LinkedPool<Allocator2D.Row>(new Func<Allocator2D.Row>(Allocator2D.Row.Create), new Action<Allocator2D.Row>(Allocator2D.Row.Reset), 256);
		}

		// Token: 0x02000429 RID: 1065
		public struct Alloc2D
		{
			// Token: 0x060021EB RID: 8683 RVA: 0x0007FF23 File Offset: 0x0007E123
			public Alloc2D(Allocator2D.Row row, Alloc alloc, int width, int height)
			{
				this.alloc = alloc;
				this.row = row;
				this.rect = new RectInt(row.rect.xMin + (int)alloc.start, row.rect.yMin, width, height);
			}

			// Token: 0x04000E75 RID: 3701
			public RectInt rect;

			// Token: 0x04000E76 RID: 3702
			public Allocator2D.Row row;

			// Token: 0x04000E77 RID: 3703
			public Alloc alloc;
		}
	}
}
