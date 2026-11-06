using System;

namespace Steamworks
{
	// Token: 0x020001C2 RID: 450
	[Serializable]
	public struct SteamLeaderboard_t : IEquatable<SteamLeaderboard_t>, IComparable<SteamLeaderboard_t>
	{
		// Token: 0x06000B73 RID: 2931 RVA: 0x00011037 File Offset: 0x0000F237
		public SteamLeaderboard_t(ulong value)
		{
			this.m_SteamLeaderboard = value;
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00011040 File Offset: 0x0000F240
		public override string ToString()
		{
			return this.m_SteamLeaderboard.ToString();
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0001104D File Offset: 0x0000F24D
		public override bool Equals(object other)
		{
			return other is SteamLeaderboard_t && this == (SteamLeaderboard_t)other;
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0001106A File Offset: 0x0000F26A
		public override int GetHashCode()
		{
			return this.m_SteamLeaderboard.GetHashCode();
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00011077 File Offset: 0x0000F277
		public static bool operator ==(SteamLeaderboard_t x, SteamLeaderboard_t y)
		{
			return x.m_SteamLeaderboard == y.m_SteamLeaderboard;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00011087 File Offset: 0x0000F287
		public static bool operator !=(SteamLeaderboard_t x, SteamLeaderboard_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00011093 File Offset: 0x0000F293
		public static explicit operator SteamLeaderboard_t(ulong value)
		{
			return new SteamLeaderboard_t(value);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0001109B File Offset: 0x0000F29B
		public static explicit operator ulong(SteamLeaderboard_t that)
		{
			return that.m_SteamLeaderboard;
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x000110A3 File Offset: 0x0000F2A3
		public bool Equals(SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard == other.m_SteamLeaderboard;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x000110B3 File Offset: 0x0000F2B3
		public int CompareTo(SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard.CompareTo(other.m_SteamLeaderboard);
		}

		// Token: 0x04000C03 RID: 3075
		public ulong m_SteamLeaderboard;
	}
}
