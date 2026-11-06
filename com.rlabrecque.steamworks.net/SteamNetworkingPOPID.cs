using System;

namespace Steamworks
{
	// Token: 0x020001AE RID: 430
	[Serializable]
	public struct SteamNetworkingPOPID : IEquatable<SteamNetworkingPOPID>, IComparable<SteamNetworkingPOPID>
	{
		// Token: 0x06000AAE RID: 2734 RVA: 0x000103A4 File Offset: 0x0000E5A4
		public SteamNetworkingPOPID(uint value)
		{
			this.m_SteamNetworkingPOPID = value;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000103AD File Offset: 0x0000E5AD
		public override string ToString()
		{
			return this.m_SteamNetworkingPOPID.ToString();
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x000103BA File Offset: 0x0000E5BA
		public override bool Equals(object other)
		{
			return other is SteamNetworkingPOPID && this == (SteamNetworkingPOPID)other;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x000103D7 File Offset: 0x0000E5D7
		public override int GetHashCode()
		{
			return this.m_SteamNetworkingPOPID.GetHashCode();
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000103E4 File Offset: 0x0000E5E4
		public static bool operator ==(SteamNetworkingPOPID x, SteamNetworkingPOPID y)
		{
			return x.m_SteamNetworkingPOPID == y.m_SteamNetworkingPOPID;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x000103F4 File Offset: 0x0000E5F4
		public static bool operator !=(SteamNetworkingPOPID x, SteamNetworkingPOPID y)
		{
			return !(x == y);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00010400 File Offset: 0x0000E600
		public static explicit operator SteamNetworkingPOPID(uint value)
		{
			return new SteamNetworkingPOPID(value);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00010408 File Offset: 0x0000E608
		public static explicit operator uint(SteamNetworkingPOPID that)
		{
			return that.m_SteamNetworkingPOPID;
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00010410 File Offset: 0x0000E610
		public bool Equals(SteamNetworkingPOPID other)
		{
			return this.m_SteamNetworkingPOPID == other.m_SteamNetworkingPOPID;
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00010420 File Offset: 0x0000E620
		public int CompareTo(SteamNetworkingPOPID other)
		{
			return this.m_SteamNetworkingPOPID.CompareTo(other.m_SteamNetworkingPOPID);
		}

		// Token: 0x04000BDF RID: 3039
		public uint m_SteamNetworkingPOPID;
	}
}
