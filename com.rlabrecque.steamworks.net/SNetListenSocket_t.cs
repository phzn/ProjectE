using System;

namespace Steamworks
{
	// Token: 0x020001A0 RID: 416
	[Serializable]
	public struct SNetListenSocket_t : IEquatable<SNetListenSocket_t>, IComparable<SNetListenSocket_t>
	{
		// Token: 0x06000A3E RID: 2622 RVA: 0x0000FD57 File Offset: 0x0000DF57
		public SNetListenSocket_t(uint value)
		{
			this.m_SNetListenSocket = value;
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0000FD60 File Offset: 0x0000DF60
		public override string ToString()
		{
			return this.m_SNetListenSocket.ToString();
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0000FD6D File Offset: 0x0000DF6D
		public override bool Equals(object other)
		{
			return other is SNetListenSocket_t && this == (SNetListenSocket_t)other;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0000FD8A File Offset: 0x0000DF8A
		public override int GetHashCode()
		{
			return this.m_SNetListenSocket.GetHashCode();
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0000FD97 File Offset: 0x0000DF97
		public static bool operator ==(SNetListenSocket_t x, SNetListenSocket_t y)
		{
			return x.m_SNetListenSocket == y.m_SNetListenSocket;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0000FDA7 File Offset: 0x0000DFA7
		public static bool operator !=(SNetListenSocket_t x, SNetListenSocket_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0000FDB3 File Offset: 0x0000DFB3
		public static explicit operator SNetListenSocket_t(uint value)
		{
			return new SNetListenSocket_t(value);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0000FDBB File Offset: 0x0000DFBB
		public static explicit operator uint(SNetListenSocket_t that)
		{
			return that.m_SNetListenSocket;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0000FDC3 File Offset: 0x0000DFC3
		public bool Equals(SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket == other.m_SNetListenSocket;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0000FDD3 File Offset: 0x0000DFD3
		public int CompareTo(SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket.CompareTo(other.m_SNetListenSocket);
		}

		// Token: 0x04000B9B RID: 2971
		public uint m_SNetListenSocket;
	}
}
