using System;

namespace Steamworks
{
	// Token: 0x020001C1 RID: 449
	[Serializable]
	public struct SteamLeaderboardEntries_t : IEquatable<SteamLeaderboardEntries_t>, IComparable<SteamLeaderboardEntries_t>
	{
		// Token: 0x06000B69 RID: 2921 RVA: 0x00010FA8 File Offset: 0x0000F1A8
		public SteamLeaderboardEntries_t(ulong value)
		{
			this.m_SteamLeaderboardEntries = value;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00010FB1 File Offset: 0x0000F1B1
		public override string ToString()
		{
			return this.m_SteamLeaderboardEntries.ToString();
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00010FBE File Offset: 0x0000F1BE
		public override bool Equals(object other)
		{
			return other is SteamLeaderboardEntries_t && this == (SteamLeaderboardEntries_t)other;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00010FDB File Offset: 0x0000F1DB
		public override int GetHashCode()
		{
			return this.m_SteamLeaderboardEntries.GetHashCode();
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00010FE8 File Offset: 0x0000F1E8
		public static bool operator ==(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
		{
			return x.m_SteamLeaderboardEntries == y.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00010FF8 File Offset: 0x0000F1F8
		public static bool operator !=(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00011004 File Offset: 0x0000F204
		public static explicit operator SteamLeaderboardEntries_t(ulong value)
		{
			return new SteamLeaderboardEntries_t(value);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0001100C File Offset: 0x0000F20C
		public static explicit operator ulong(SteamLeaderboardEntries_t that)
		{
			return that.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00011014 File Offset: 0x0000F214
		public bool Equals(SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries == other.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00011024 File Offset: 0x0000F224
		public int CompareTo(SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries.CompareTo(other.m_SteamLeaderboardEntries);
		}

		// Token: 0x04000C02 RID: 3074
		public ulong m_SteamLeaderboardEntries;
	}
}
