using System;

namespace Steamworks
{
	// Token: 0x020001BC RID: 444
	[Serializable]
	public struct RTime32 : IEquatable<RTime32>, IComparable<RTime32>
	{
		// Token: 0x06000B39 RID: 2873 RVA: 0x00010B61 File Offset: 0x0000ED61
		public RTime32(uint value)
		{
			this.m_RTime32 = value;
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00010B6A File Offset: 0x0000ED6A
		public override string ToString()
		{
			return this.m_RTime32.ToString();
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00010B77 File Offset: 0x0000ED77
		public override bool Equals(object other)
		{
			return other is RTime32 && this == (RTime32)other;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00010B94 File Offset: 0x0000ED94
		public override int GetHashCode()
		{
			return this.m_RTime32.GetHashCode();
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00010BA1 File Offset: 0x0000EDA1
		public static bool operator ==(RTime32 x, RTime32 y)
		{
			return x.m_RTime32 == y.m_RTime32;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00010BB1 File Offset: 0x0000EDB1
		public static bool operator !=(RTime32 x, RTime32 y)
		{
			return !(x == y);
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00010BBD File Offset: 0x0000EDBD
		public static explicit operator RTime32(uint value)
		{
			return new RTime32(value);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00010BC5 File Offset: 0x0000EDC5
		public static explicit operator uint(RTime32 that)
		{
			return that.m_RTime32;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00010BCD File Offset: 0x0000EDCD
		public bool Equals(RTime32 other)
		{
			return this.m_RTime32 == other.m_RTime32;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00010BDD File Offset: 0x0000EDDD
		public int CompareTo(RTime32 other)
		{
			return this.m_RTime32.CompareTo(other.m_RTime32);
		}

		// Token: 0x04000BF8 RID: 3064
		public uint m_RTime32;
	}
}
