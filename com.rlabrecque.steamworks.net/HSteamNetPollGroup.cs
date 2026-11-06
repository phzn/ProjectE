using System;

namespace Steamworks
{
	// Token: 0x020001A7 RID: 423
	[Serializable]
	public struct HSteamNetPollGroup : IEquatable<HSteamNetPollGroup>, IComparable<HSteamNetPollGroup>
	{
		// Token: 0x06000A70 RID: 2672 RVA: 0x0000FFD8 File Offset: 0x0000E1D8
		public HSteamNetPollGroup(uint value)
		{
			this.m_HSteamNetPollGroup = value;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0000FFE1 File Offset: 0x0000E1E1
		public override string ToString()
		{
			return this.m_HSteamNetPollGroup.ToString();
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0000FFEE File Offset: 0x0000E1EE
		public override bool Equals(object other)
		{
			return other is HSteamNetPollGroup && this == (HSteamNetPollGroup)other;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0001000B File Offset: 0x0000E20B
		public override int GetHashCode()
		{
			return this.m_HSteamNetPollGroup.GetHashCode();
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00010018 File Offset: 0x0000E218
		public static bool operator ==(HSteamNetPollGroup x, HSteamNetPollGroup y)
		{
			return x.m_HSteamNetPollGroup == y.m_HSteamNetPollGroup;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00010028 File Offset: 0x0000E228
		public static bool operator !=(HSteamNetPollGroup x, HSteamNetPollGroup y)
		{
			return !(x == y);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00010034 File Offset: 0x0000E234
		public static explicit operator HSteamNetPollGroup(uint value)
		{
			return new HSteamNetPollGroup(value);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0001003C File Offset: 0x0000E23C
		public static explicit operator uint(HSteamNetPollGroup that)
		{
			return that.m_HSteamNetPollGroup;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00010044 File Offset: 0x0000E244
		public bool Equals(HSteamNetPollGroup other)
		{
			return this.m_HSteamNetPollGroup == other.m_HSteamNetPollGroup;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00010054 File Offset: 0x0000E254
		public int CompareTo(HSteamNetPollGroup other)
		{
			return this.m_HSteamNetPollGroup.CompareTo(other.m_HSteamNetPollGroup);
		}

		// Token: 0x04000BA1 RID: 2977
		public static readonly HSteamNetPollGroup Invalid = new HSteamNetPollGroup(0U);

		// Token: 0x04000BA2 RID: 2978
		public uint m_HSteamNetPollGroup;
	}
}
