using System;

namespace Steamworks
{
	// Token: 0x020001A6 RID: 422
	[Serializable]
	public struct HSteamNetConnection : IEquatable<HSteamNetConnection>, IComparable<HSteamNetConnection>
	{
		// Token: 0x06000A65 RID: 2661 RVA: 0x0000FF3C File Offset: 0x0000E13C
		public HSteamNetConnection(uint value)
		{
			this.m_HSteamNetConnection = value;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0000FF45 File Offset: 0x0000E145
		public override string ToString()
		{
			return this.m_HSteamNetConnection.ToString();
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0000FF52 File Offset: 0x0000E152
		public override bool Equals(object other)
		{
			return other is HSteamNetConnection && this == (HSteamNetConnection)other;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0000FF6F File Offset: 0x0000E16F
		public override int GetHashCode()
		{
			return this.m_HSteamNetConnection.GetHashCode();
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0000FF7C File Offset: 0x0000E17C
		public static bool operator ==(HSteamNetConnection x, HSteamNetConnection y)
		{
			return x.m_HSteamNetConnection == y.m_HSteamNetConnection;
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0000FF8C File Offset: 0x0000E18C
		public static bool operator !=(HSteamNetConnection x, HSteamNetConnection y)
		{
			return !(x == y);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0000FF98 File Offset: 0x0000E198
		public static explicit operator HSteamNetConnection(uint value)
		{
			return new HSteamNetConnection(value);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0000FFA0 File Offset: 0x0000E1A0
		public static explicit operator uint(HSteamNetConnection that)
		{
			return that.m_HSteamNetConnection;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0000FFA8 File Offset: 0x0000E1A8
		public bool Equals(HSteamNetConnection other)
		{
			return this.m_HSteamNetConnection == other.m_HSteamNetConnection;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
		public int CompareTo(HSteamNetConnection other)
		{
			return this.m_HSteamNetConnection.CompareTo(other.m_HSteamNetConnection);
		}

		// Token: 0x04000B9F RID: 2975
		public static readonly HSteamNetConnection Invalid = new HSteamNetConnection(0U);

		// Token: 0x04000BA0 RID: 2976
		public uint m_HSteamNetConnection;
	}
}
