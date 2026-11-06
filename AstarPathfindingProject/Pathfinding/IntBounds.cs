using System;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x02000062 RID: 98
	public struct IntBounds
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x00011B12 File Offset: 0x0000FD12
		public IntBounds(int xmin, int ymin, int zmin, int xmax, int ymax, int zmax)
		{
			this.min = new int3(xmin, ymin, zmin);
			this.max = new int3(xmax, ymax, zmax);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00011B33 File Offset: 0x0000FD33
		public IntBounds(int3 min, int3 max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00011B43 File Offset: 0x0000FD43
		public int3 size
		{
			get
			{
				return this.max - this.min;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00011B58 File Offset: 0x0000FD58
		public int volume
		{
			get
			{
				int3 size = this.size;
				return size.x * size.y * size.z;
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00011B80 File Offset: 0x0000FD80
		public static IntBounds Intersection(IntBounds a, IntBounds b)
		{
			return new IntBounds(math.max(a.min, b.min), math.min(a.max, b.max));
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00011BA9 File Offset: 0x0000FDA9
		public static bool Intersects(IntBounds a, IntBounds b)
		{
			return math.all((a.min < b.max) & (a.max > b.min));
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00011BD7 File Offset: 0x0000FDD7
		public IntBounds Offset(int3 offset)
		{
			return new IntBounds(this.min + offset, this.max + offset);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00011BF6 File Offset: 0x0000FDF6
		public bool Contains(IntBounds other)
		{
			return math.all((other.min >= this.min) & (other.max <= this.max));
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00011C24 File Offset: 0x0000FE24
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.min.ToString(),
				" <= x < ",
				this.max.ToString(),
				")"
			});
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00011C7C File Offset: 0x0000FE7C
		public override bool Equals(object _b)
		{
			IntBounds intBounds = (IntBounds)_b;
			return this == intBounds;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00011C9C File Offset: 0x0000FE9C
		public override int GetHashCode()
		{
			return this.min.GetHashCode() ^ (this.max.GetHashCode() << 2);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00011CC3 File Offset: 0x0000FEC3
		public static bool operator ==(IntBounds a, IntBounds b)
		{
			return math.all((a.min == b.min) & (a.max == b.max));
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00011CF1 File Offset: 0x0000FEF1
		public static bool operator !=(IntBounds a, IntBounds b)
		{
			return !(a == b);
		}

		// Token: 0x04000222 RID: 546
		public int3 min;

		// Token: 0x04000223 RID: 547
		public int3 max;
	}
}
