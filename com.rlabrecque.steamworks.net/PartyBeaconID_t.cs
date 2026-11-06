using System;

namespace Steamworks
{
	// Token: 0x020001BB RID: 443
	[Serializable]
	public struct PartyBeaconID_t : IEquatable<PartyBeaconID_t>, IComparable<PartyBeaconID_t>
	{
		// Token: 0x06000B2E RID: 2862 RVA: 0x00010AC4 File Offset: 0x0000ECC4
		public PartyBeaconID_t(ulong value)
		{
			this.m_PartyBeaconID = value;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00010ACD File Offset: 0x0000ECCD
		public override string ToString()
		{
			return this.m_PartyBeaconID.ToString();
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00010ADA File Offset: 0x0000ECDA
		public override bool Equals(object other)
		{
			return other is PartyBeaconID_t && this == (PartyBeaconID_t)other;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00010AF7 File Offset: 0x0000ECF7
		public override int GetHashCode()
		{
			return this.m_PartyBeaconID.GetHashCode();
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00010B04 File Offset: 0x0000ED04
		public static bool operator ==(PartyBeaconID_t x, PartyBeaconID_t y)
		{
			return x.m_PartyBeaconID == y.m_PartyBeaconID;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00010B14 File Offset: 0x0000ED14
		public static bool operator !=(PartyBeaconID_t x, PartyBeaconID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00010B20 File Offset: 0x0000ED20
		public static explicit operator PartyBeaconID_t(ulong value)
		{
			return new PartyBeaconID_t(value);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00010B28 File Offset: 0x0000ED28
		public static explicit operator ulong(PartyBeaconID_t that)
		{
			return that.m_PartyBeaconID;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00010B30 File Offset: 0x0000ED30
		public bool Equals(PartyBeaconID_t other)
		{
			return this.m_PartyBeaconID == other.m_PartyBeaconID;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00010B40 File Offset: 0x0000ED40
		public int CompareTo(PartyBeaconID_t other)
		{
			return this.m_PartyBeaconID.CompareTo(other.m_PartyBeaconID);
		}

		// Token: 0x04000BF6 RID: 3062
		public static readonly PartyBeaconID_t Invalid = new PartyBeaconID_t(0UL);

		// Token: 0x04000BF7 RID: 3063
		public ulong m_PartyBeaconID;
	}
}
