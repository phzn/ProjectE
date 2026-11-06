using System;

namespace Steamworks
{
	// Token: 0x020001C3 RID: 451
	[Serializable]
	public struct HSteamPipe : IEquatable<HSteamPipe>, IComparable<HSteamPipe>
	{
		// Token: 0x06000B7D RID: 2941 RVA: 0x000110C6 File Offset: 0x0000F2C6
		public HSteamPipe(int value)
		{
			this.m_HSteamPipe = value;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x000110CF File Offset: 0x0000F2CF
		public override string ToString()
		{
			return this.m_HSteamPipe.ToString();
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x000110DC File Offset: 0x0000F2DC
		public override bool Equals(object other)
		{
			return other is HSteamPipe && this == (HSteamPipe)other;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x000110F9 File Offset: 0x0000F2F9
		public override int GetHashCode()
		{
			return this.m_HSteamPipe.GetHashCode();
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00011106 File Offset: 0x0000F306
		public static bool operator ==(HSteamPipe x, HSteamPipe y)
		{
			return x.m_HSteamPipe == y.m_HSteamPipe;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00011116 File Offset: 0x0000F316
		public static bool operator !=(HSteamPipe x, HSteamPipe y)
		{
			return !(x == y);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00011122 File Offset: 0x0000F322
		public static explicit operator HSteamPipe(int value)
		{
			return new HSteamPipe(value);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0001112A File Offset: 0x0000F32A
		public static explicit operator int(HSteamPipe that)
		{
			return that.m_HSteamPipe;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00011132 File Offset: 0x0000F332
		public bool Equals(HSteamPipe other)
		{
			return this.m_HSteamPipe == other.m_HSteamPipe;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00011142 File Offset: 0x0000F342
		public int CompareTo(HSteamPipe other)
		{
			return this.m_HSteamPipe.CompareTo(other.m_HSteamPipe);
		}

		// Token: 0x04000C04 RID: 3076
		public int m_HSteamPipe;
	}
}
