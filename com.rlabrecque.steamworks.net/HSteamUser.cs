using System;

namespace Steamworks
{
	// Token: 0x020001C4 RID: 452
	[Serializable]
	public struct HSteamUser : IEquatable<HSteamUser>, IComparable<HSteamUser>
	{
		// Token: 0x06000B87 RID: 2951 RVA: 0x00011155 File Offset: 0x0000F355
		public HSteamUser(int value)
		{
			this.m_HSteamUser = value;
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0001115E File Offset: 0x0000F35E
		public override string ToString()
		{
			return this.m_HSteamUser.ToString();
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0001116B File Offset: 0x0000F36B
		public override bool Equals(object other)
		{
			return other is HSteamUser && this == (HSteamUser)other;
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00011188 File Offset: 0x0000F388
		public override int GetHashCode()
		{
			return this.m_HSteamUser.GetHashCode();
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00011195 File Offset: 0x0000F395
		public static bool operator ==(HSteamUser x, HSteamUser y)
		{
			return x.m_HSteamUser == y.m_HSteamUser;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x000111A5 File Offset: 0x0000F3A5
		public static bool operator !=(HSteamUser x, HSteamUser y)
		{
			return !(x == y);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x000111B1 File Offset: 0x0000F3B1
		public static explicit operator HSteamUser(int value)
		{
			return new HSteamUser(value);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x000111B9 File Offset: 0x0000F3B9
		public static explicit operator int(HSteamUser that)
		{
			return that.m_HSteamUser;
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x000111C1 File Offset: 0x0000F3C1
		public bool Equals(HSteamUser other)
		{
			return this.m_HSteamUser == other.m_HSteamUser;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x000111D1 File Offset: 0x0000F3D1
		public int CompareTo(HSteamUser other)
		{
			return this.m_HSteamUser.CompareTo(other.m_HSteamUser);
		}

		// Token: 0x04000C05 RID: 3077
		public int m_HSteamUser;
	}
}
