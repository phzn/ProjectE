using System;

namespace Steamworks
{
	// Token: 0x020001A1 RID: 417
	[Serializable]
	public struct SNetSocket_t : IEquatable<SNetSocket_t>, IComparable<SNetSocket_t>
	{
		// Token: 0x06000A48 RID: 2632 RVA: 0x0000FDE6 File Offset: 0x0000DFE6
		public SNetSocket_t(uint value)
		{
			this.m_SNetSocket = value;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0000FDEF File Offset: 0x0000DFEF
		public override string ToString()
		{
			return this.m_SNetSocket.ToString();
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0000FDFC File Offset: 0x0000DFFC
		public override bool Equals(object other)
		{
			return other is SNetSocket_t && this == (SNetSocket_t)other;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0000FE19 File Offset: 0x0000E019
		public override int GetHashCode()
		{
			return this.m_SNetSocket.GetHashCode();
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0000FE26 File Offset: 0x0000E026
		public static bool operator ==(SNetSocket_t x, SNetSocket_t y)
		{
			return x.m_SNetSocket == y.m_SNetSocket;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0000FE36 File Offset: 0x0000E036
		public static bool operator !=(SNetSocket_t x, SNetSocket_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0000FE42 File Offset: 0x0000E042
		public static explicit operator SNetSocket_t(uint value)
		{
			return new SNetSocket_t(value);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0000FE4A File Offset: 0x0000E04A
		public static explicit operator uint(SNetSocket_t that)
		{
			return that.m_SNetSocket;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0000FE52 File Offset: 0x0000E052
		public bool Equals(SNetSocket_t other)
		{
			return this.m_SNetSocket == other.m_SNetSocket;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0000FE62 File Offset: 0x0000E062
		public int CompareTo(SNetSocket_t other)
		{
			return this.m_SNetSocket.CompareTo(other.m_SNetSocket);
		}

		// Token: 0x04000B9C RID: 2972
		public uint m_SNetSocket;
	}
}
