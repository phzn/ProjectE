using System;

namespace Steamworks
{
	// Token: 0x020001A5 RID: 421
	[Serializable]
	public struct HSteamListenSocket : IEquatable<HSteamListenSocket>, IComparable<HSteamListenSocket>
	{
		// Token: 0x06000A5A RID: 2650 RVA: 0x0000FEA0 File Offset: 0x0000E0A0
		public HSteamListenSocket(uint value)
		{
			this.m_HSteamListenSocket = value;
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0000FEA9 File Offset: 0x0000E0A9
		public override string ToString()
		{
			return this.m_HSteamListenSocket.ToString();
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0000FEB6 File Offset: 0x0000E0B6
		public override bool Equals(object other)
		{
			return other is HSteamListenSocket && this == (HSteamListenSocket)other;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0000FED3 File Offset: 0x0000E0D3
		public override int GetHashCode()
		{
			return this.m_HSteamListenSocket.GetHashCode();
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0000FEE0 File Offset: 0x0000E0E0
		public static bool operator ==(HSteamListenSocket x, HSteamListenSocket y)
		{
			return x.m_HSteamListenSocket == y.m_HSteamListenSocket;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0000FEF0 File Offset: 0x0000E0F0
		public static bool operator !=(HSteamListenSocket x, HSteamListenSocket y)
		{
			return !(x == y);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0000FEFC File Offset: 0x0000E0FC
		public static explicit operator HSteamListenSocket(uint value)
		{
			return new HSteamListenSocket(value);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0000FF04 File Offset: 0x0000E104
		public static explicit operator uint(HSteamListenSocket that)
		{
			return that.m_HSteamListenSocket;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0000FF0C File Offset: 0x0000E10C
		public bool Equals(HSteamListenSocket other)
		{
			return this.m_HSteamListenSocket == other.m_HSteamListenSocket;
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0000FF1C File Offset: 0x0000E11C
		public int CompareTo(HSteamListenSocket other)
		{
			return this.m_HSteamListenSocket.CompareTo(other.m_HSteamListenSocket);
		}

		// Token: 0x04000B9D RID: 2973
		public static readonly HSteamListenSocket Invalid = new HSteamListenSocket(0U);

		// Token: 0x04000B9E RID: 2974
		public uint m_HSteamListenSocket;
	}
}
