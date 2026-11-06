using System;

namespace Steamworks
{
	// Token: 0x020001BA RID: 442
	[Serializable]
	public struct DepotId_t : IEquatable<DepotId_t>, IComparable<DepotId_t>
	{
		// Token: 0x06000B23 RID: 2851 RVA: 0x00010A28 File Offset: 0x0000EC28
		public DepotId_t(uint value)
		{
			this.m_DepotId = value;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00010A31 File Offset: 0x0000EC31
		public override string ToString()
		{
			return this.m_DepotId.ToString();
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00010A3E File Offset: 0x0000EC3E
		public override bool Equals(object other)
		{
			return other is DepotId_t && this == (DepotId_t)other;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00010A5B File Offset: 0x0000EC5B
		public override int GetHashCode()
		{
			return this.m_DepotId.GetHashCode();
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00010A68 File Offset: 0x0000EC68
		public static bool operator ==(DepotId_t x, DepotId_t y)
		{
			return x.m_DepotId == y.m_DepotId;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00010A78 File Offset: 0x0000EC78
		public static bool operator !=(DepotId_t x, DepotId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00010A84 File Offset: 0x0000EC84
		public static explicit operator DepotId_t(uint value)
		{
			return new DepotId_t(value);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00010A8C File Offset: 0x0000EC8C
		public static explicit operator uint(DepotId_t that)
		{
			return that.m_DepotId;
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00010A94 File Offset: 0x0000EC94
		public bool Equals(DepotId_t other)
		{
			return this.m_DepotId == other.m_DepotId;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00010AA4 File Offset: 0x0000ECA4
		public int CompareTo(DepotId_t other)
		{
			return this.m_DepotId.CompareTo(other.m_DepotId);
		}

		// Token: 0x04000BF4 RID: 3060
		public static readonly DepotId_t Invalid = new DepotId_t(0U);

		// Token: 0x04000BF5 RID: 3061
		public uint m_DepotId;
	}
}
