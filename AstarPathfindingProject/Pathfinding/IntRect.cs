using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public struct IntRect
	{
		// Token: 0x06000274 RID: 628 RVA: 0x0000B673 File Offset: 0x00009873
		public IntRect(int xmin, int ymin, int xmax, int ymax)
		{
			this.xmin = xmin;
			this.xmax = xmax;
			this.ymin = ymin;
			this.ymax = ymax;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000B692 File Offset: 0x00009892
		public bool Contains(int x, int y)
		{
			return x >= this.xmin && y >= this.ymin && x <= this.xmax && y <= this.ymax;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000B6BD File Offset: 0x000098BD
		public bool Contains(IntRect other)
		{
			return this.xmin <= other.xmin && this.xmax >= other.xmax && this.ymin <= other.ymin && this.ymax >= other.ymax;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000B6FC File Offset: 0x000098FC
		public Vector2Int Min
		{
			get
			{
				return new Vector2Int(this.xmin, this.ymin);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000B70F File Offset: 0x0000990F
		public Vector2Int Max
		{
			get
			{
				return new Vector2Int(this.xmax, this.ymax);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000B722 File Offset: 0x00009922
		public int Width
		{
			get
			{
				return this.xmax - this.xmin + 1;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000B733 File Offset: 0x00009933
		public int Height
		{
			get
			{
				return this.ymax - this.ymin + 1;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000B744 File Offset: 0x00009944
		public int Area
		{
			get
			{
				return this.Width * this.Height;
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000B753 File Offset: 0x00009953
		public bool IsValid()
		{
			return this.xmin <= this.xmax && this.ymin <= this.ymax;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000B776 File Offset: 0x00009976
		public static bool operator ==(IntRect a, IntRect b)
		{
			return a.xmin == b.xmin && a.xmax == b.xmax && a.ymin == b.ymin && a.ymax == b.ymax;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000B7B2 File Offset: 0x000099B2
		public static bool operator !=(IntRect a, IntRect b)
		{
			return a.xmin != b.xmin || a.xmax != b.xmax || a.ymin != b.ymin || a.ymax != b.ymax;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000B7F1 File Offset: 0x000099F1
		public static explicit operator Rect(IntRect r)
		{
			return new Rect((float)r.xmin, (float)r.ymin, (float)r.Width, (float)r.Height);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000B818 File Offset: 0x00009A18
		public override bool Equals(object obj)
		{
			if (!(obj is IntRect))
			{
				return false;
			}
			IntRect intRect = (IntRect)obj;
			return this.xmin == intRect.xmin && this.xmax == intRect.xmax && this.ymin == intRect.ymin && this.ymax == intRect.ymax;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000B870 File Offset: 0x00009A70
		public override int GetHashCode()
		{
			return (this.xmin * 131071) ^ (this.xmax * 3571) ^ (this.ymin * 3109) ^ (this.ymax * 7);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000B8A4 File Offset: 0x00009AA4
		public static IntRect Intersection(IntRect a, IntRect b)
		{
			return new IntRect(Math.Max(a.xmin, b.xmin), Math.Max(a.ymin, b.ymin), Math.Min(a.xmax, b.xmax), Math.Min(a.ymax, b.ymax));
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000B8FA File Offset: 0x00009AFA
		public static bool Intersects(IntRect a, IntRect b)
		{
			return a.xmin <= b.xmax && a.ymin <= b.ymax && a.xmax >= b.xmin && a.ymax >= b.ymin;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000B93C File Offset: 0x00009B3C
		public static IntRect Union(IntRect a, IntRect b)
		{
			return new IntRect(Math.Min(a.xmin, b.xmin), Math.Min(a.ymin, b.ymin), Math.Max(a.xmax, b.xmax), Math.Max(a.ymax, b.ymax));
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000B994 File Offset: 0x00009B94
		public static IntRect Exclude(IntRect a, IntRect b)
		{
			if (!b.IsValid() || !a.IsValid())
			{
				return a;
			}
			IntRect intRect = IntRect.Intersection(a, b);
			if (!intRect.IsValid())
			{
				return a;
			}
			if (a.xmin == intRect.xmin && a.xmax == intRect.xmax)
			{
				if (a.ymin == intRect.ymin)
				{
					a.ymin = intRect.ymax + 1;
					return a;
				}
				if (a.ymax == intRect.ymax)
				{
					a.ymax = intRect.ymin - 1;
					return a;
				}
				throw new ArgumentException("B splits A into two disjoint parts");
			}
			else
			{
				if (a.ymin != intRect.ymin || a.ymax != intRect.ymax)
				{
					throw new ArgumentException("B covers either a corner of A, or does not touch the edges of A at all");
				}
				if (a.xmin == intRect.xmin)
				{
					a.xmin = intRect.xmax + 1;
					return a;
				}
				if (a.xmax == intRect.xmax)
				{
					a.xmax = intRect.xmin - 1;
					return a;
				}
				throw new ArgumentException("B splits A into two disjoint parts");
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000BA9C File Offset: 0x00009C9C
		public IntRect ExpandToContain(int x, int y)
		{
			return new IntRect(Math.Min(this.xmin, x), Math.Min(this.ymin, y), Math.Max(this.xmax, x), Math.Max(this.ymax, y));
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000BAD3 File Offset: 0x00009CD3
		public IntRect Offset(Vector2Int offset)
		{
			return new IntRect(this.xmin + offset.x, this.ymin + offset.y, this.xmax + offset.x, this.ymax + offset.y);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000BB12 File Offset: 0x00009D12
		public IntRect Expand(int range)
		{
			return new IntRect(this.xmin - range, this.ymin - range, this.xmax + range, this.ymax + range);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000BB3C File Offset: 0x00009D3C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[x: ",
				this.xmin.ToString(),
				"...",
				this.xmax.ToString(),
				", y: ",
				this.ymin.ToString(),
				"...",
				this.ymax.ToString(),
				"]"
			});
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000BBB8 File Offset: 0x00009DB8
		public List<Vector2Int> GetInnerCoordinates()
		{
			List<Vector2Int> list = ListPool<Vector2Int>.Claim(this.Width * this.Height);
			for (int i = this.ymin; i <= this.ymax; i++)
			{
				for (int j = this.xmin; j <= this.xmax; j++)
				{
					list.Add(new Vector2Int(j, i));
				}
			}
			return list;
		}

		// Token: 0x04000185 RID: 389
		public int xmin;

		// Token: 0x04000186 RID: 390
		public int ymin;

		// Token: 0x04000187 RID: 391
		public int xmax;

		// Token: 0x04000188 RID: 392
		public int ymax;
	}
}
