using System;

namespace Steamworks
{
	// Token: 0x0200019F RID: 415
	[Serializable]
	public struct HServerQuery : IEquatable<HServerQuery>, IComparable<HServerQuery>
	{
		// Token: 0x06000A33 RID: 2611 RVA: 0x0000FCBB File Offset: 0x0000DEBB
		public HServerQuery(int value)
		{
			this.m_HServerQuery = value;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0000FCC4 File Offset: 0x0000DEC4
		public override string ToString()
		{
			return this.m_HServerQuery.ToString();
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0000FCD1 File Offset: 0x0000DED1
		public override bool Equals(object other)
		{
			return other is HServerQuery && this == (HServerQuery)other;
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0000FCEE File Offset: 0x0000DEEE
		public override int GetHashCode()
		{
			return this.m_HServerQuery.GetHashCode();
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0000FCFB File Offset: 0x0000DEFB
		public static bool operator ==(HServerQuery x, HServerQuery y)
		{
			return x.m_HServerQuery == y.m_HServerQuery;
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0000FD0B File Offset: 0x0000DF0B
		public static bool operator !=(HServerQuery x, HServerQuery y)
		{
			return !(x == y);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0000FD17 File Offset: 0x0000DF17
		public static explicit operator HServerQuery(int value)
		{
			return new HServerQuery(value);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0000FD1F File Offset: 0x0000DF1F
		public static explicit operator int(HServerQuery that)
		{
			return that.m_HServerQuery;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0000FD27 File Offset: 0x0000DF27
		public bool Equals(HServerQuery other)
		{
			return this.m_HServerQuery == other.m_HServerQuery;
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0000FD37 File Offset: 0x0000DF37
		public int CompareTo(HServerQuery other)
		{
			return this.m_HServerQuery.CompareTo(other.m_HServerQuery);
		}

		// Token: 0x04000B99 RID: 2969
		public static readonly HServerQuery Invalid = new HServerQuery(-1);

		// Token: 0x04000B9A RID: 2970
		public int m_HServerQuery;
	}
}
