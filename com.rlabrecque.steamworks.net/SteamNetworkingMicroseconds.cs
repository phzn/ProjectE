using System;

namespace Steamworks
{
	// Token: 0x020001AD RID: 429
	[Serializable]
	public struct SteamNetworkingMicroseconds : IEquatable<SteamNetworkingMicroseconds>, IComparable<SteamNetworkingMicroseconds>
	{
		// Token: 0x06000AA4 RID: 2724 RVA: 0x00010315 File Offset: 0x0000E515
		public SteamNetworkingMicroseconds(long value)
		{
			this.m_SteamNetworkingMicroseconds = value;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0001031E File Offset: 0x0000E51E
		public override string ToString()
		{
			return this.m_SteamNetworkingMicroseconds.ToString();
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0001032B File Offset: 0x0000E52B
		public override bool Equals(object other)
		{
			return other is SteamNetworkingMicroseconds && this == (SteamNetworkingMicroseconds)other;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00010348 File Offset: 0x0000E548
		public override int GetHashCode()
		{
			return this.m_SteamNetworkingMicroseconds.GetHashCode();
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00010355 File Offset: 0x0000E555
		public static bool operator ==(SteamNetworkingMicroseconds x, SteamNetworkingMicroseconds y)
		{
			return x.m_SteamNetworkingMicroseconds == y.m_SteamNetworkingMicroseconds;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00010365 File Offset: 0x0000E565
		public static bool operator !=(SteamNetworkingMicroseconds x, SteamNetworkingMicroseconds y)
		{
			return !(x == y);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00010371 File Offset: 0x0000E571
		public static explicit operator SteamNetworkingMicroseconds(long value)
		{
			return new SteamNetworkingMicroseconds(value);
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00010379 File Offset: 0x0000E579
		public static explicit operator long(SteamNetworkingMicroseconds that)
		{
			return that.m_SteamNetworkingMicroseconds;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00010381 File Offset: 0x0000E581
		public bool Equals(SteamNetworkingMicroseconds other)
		{
			return this.m_SteamNetworkingMicroseconds == other.m_SteamNetworkingMicroseconds;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00010391 File Offset: 0x0000E591
		public int CompareTo(SteamNetworkingMicroseconds other)
		{
			return this.m_SteamNetworkingMicroseconds.CompareTo(other.m_SteamNetworkingMicroseconds);
		}

		// Token: 0x04000BDE RID: 3038
		public long m_SteamNetworkingMicroseconds;
	}
}
