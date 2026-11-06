using System;

namespace Steamworks
{
	// Token: 0x020001BD RID: 445
	[Serializable]
	public struct SteamAPICall_t : IEquatable<SteamAPICall_t>, IComparable<SteamAPICall_t>
	{
		// Token: 0x06000B43 RID: 2883 RVA: 0x00010BF0 File Offset: 0x0000EDF0
		public SteamAPICall_t(ulong value)
		{
			this.m_SteamAPICall = value;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00010BF9 File Offset: 0x0000EDF9
		public override string ToString()
		{
			return this.m_SteamAPICall.ToString();
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00010C06 File Offset: 0x0000EE06
		public override bool Equals(object other)
		{
			return other is SteamAPICall_t && this == (SteamAPICall_t)other;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00010C23 File Offset: 0x0000EE23
		public override int GetHashCode()
		{
			return this.m_SteamAPICall.GetHashCode();
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00010C30 File Offset: 0x0000EE30
		public static bool operator ==(SteamAPICall_t x, SteamAPICall_t y)
		{
			return x.m_SteamAPICall == y.m_SteamAPICall;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00010C40 File Offset: 0x0000EE40
		public static bool operator !=(SteamAPICall_t x, SteamAPICall_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00010C4C File Offset: 0x0000EE4C
		public static explicit operator SteamAPICall_t(ulong value)
		{
			return new SteamAPICall_t(value);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00010C54 File Offset: 0x0000EE54
		public static explicit operator ulong(SteamAPICall_t that)
		{
			return that.m_SteamAPICall;
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00010C5C File Offset: 0x0000EE5C
		public bool Equals(SteamAPICall_t other)
		{
			return this.m_SteamAPICall == other.m_SteamAPICall;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00010C6C File Offset: 0x0000EE6C
		public int CompareTo(SteamAPICall_t other)
		{
			return this.m_SteamAPICall.CompareTo(other.m_SteamAPICall);
		}

		// Token: 0x04000BF9 RID: 3065
		public static readonly SteamAPICall_t Invalid = new SteamAPICall_t(0UL);

		// Token: 0x04000BFA RID: 3066
		public ulong m_SteamAPICall;
	}
}
