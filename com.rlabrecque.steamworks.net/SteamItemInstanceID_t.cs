using System;

namespace Steamworks
{
	// Token: 0x0200019D RID: 413
	[Serializable]
	public struct SteamItemInstanceID_t : IEquatable<SteamItemInstanceID_t>, IComparable<SteamItemInstanceID_t>
	{
		// Token: 0x06000A1E RID: 2590 RVA: 0x0000FB8B File Offset: 0x0000DD8B
		public SteamItemInstanceID_t(ulong value)
		{
			this.m_SteamItemInstanceID = value;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0000FB94 File Offset: 0x0000DD94
		public override string ToString()
		{
			return this.m_SteamItemInstanceID.ToString();
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0000FBA1 File Offset: 0x0000DDA1
		public override bool Equals(object other)
		{
			return other is SteamItemInstanceID_t && this == (SteamItemInstanceID_t)other;
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0000FBBE File Offset: 0x0000DDBE
		public override int GetHashCode()
		{
			return this.m_SteamItemInstanceID.GetHashCode();
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0000FBCB File Offset: 0x0000DDCB
		public static bool operator ==(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return x.m_SteamItemInstanceID == y.m_SteamItemInstanceID;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0000FBDB File Offset: 0x0000DDDB
		public static bool operator !=(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0000FBE7 File Offset: 0x0000DDE7
		public static explicit operator SteamItemInstanceID_t(ulong value)
		{
			return new SteamItemInstanceID_t(value);
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0000FBEF File Offset: 0x0000DDEF
		public static explicit operator ulong(SteamItemInstanceID_t that)
		{
			return that.m_SteamItemInstanceID;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0000FBF7 File Offset: 0x0000DDF7
		public bool Equals(SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID == other.m_SteamItemInstanceID;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0000FC07 File Offset: 0x0000DE07
		public int CompareTo(SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID.CompareTo(other.m_SteamItemInstanceID);
		}

		// Token: 0x04000B95 RID: 2965
		public static readonly SteamItemInstanceID_t Invalid = new SteamItemInstanceID_t(ulong.MaxValue);

		// Token: 0x04000B96 RID: 2966
		public ulong m_SteamItemInstanceID;
	}
}
