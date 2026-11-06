using System;

namespace Steamworks
{
	// Token: 0x0200018D RID: 397
	[Serializable]
	public struct HAuthTicket : IEquatable<HAuthTicket>, IComparable<HAuthTicket>
	{
		// Token: 0x06000999 RID: 2457 RVA: 0x0000F460 File Offset: 0x0000D660
		public HAuthTicket(uint value)
		{
			this.m_HAuthTicket = value;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0000F469 File Offset: 0x0000D669
		public override string ToString()
		{
			return this.m_HAuthTicket.ToString();
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0000F476 File Offset: 0x0000D676
		public override bool Equals(object other)
		{
			return other is HAuthTicket && this == (HAuthTicket)other;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0000F493 File Offset: 0x0000D693
		public override int GetHashCode()
		{
			return this.m_HAuthTicket.GetHashCode();
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0000F4A0 File Offset: 0x0000D6A0
		public static bool operator ==(HAuthTicket x, HAuthTicket y)
		{
			return x.m_HAuthTicket == y.m_HAuthTicket;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0000F4B0 File Offset: 0x0000D6B0
		public static bool operator !=(HAuthTicket x, HAuthTicket y)
		{
			return !(x == y);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0000F4BC File Offset: 0x0000D6BC
		public static explicit operator HAuthTicket(uint value)
		{
			return new HAuthTicket(value);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0000F4C4 File Offset: 0x0000D6C4
		public static explicit operator uint(HAuthTicket that)
		{
			return that.m_HAuthTicket;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0000F4CC File Offset: 0x0000D6CC
		public bool Equals(HAuthTicket other)
		{
			return this.m_HAuthTicket == other.m_HAuthTicket;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0000F4DC File Offset: 0x0000D6DC
		public int CompareTo(HAuthTicket other)
		{
			return this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);
		}

		// Token: 0x04000B73 RID: 2931
		public static readonly HAuthTicket Invalid = new HAuthTicket(0U);

		// Token: 0x04000B74 RID: 2932
		public uint m_HAuthTicket;
	}
}
