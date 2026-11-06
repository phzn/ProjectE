using System;

namespace Steamworks
{
	// Token: 0x0200019A RID: 410
	[Serializable]
	public struct SteamInventoryResult_t : IEquatable<SteamInventoryResult_t>, IComparable<SteamInventoryResult_t>
	{
		// Token: 0x060009FE RID: 2558 RVA: 0x0000F9C3 File Offset: 0x0000DBC3
		public SteamInventoryResult_t(int value)
		{
			this.m_SteamInventoryResult = value;
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0000F9CC File Offset: 0x0000DBCC
		public override string ToString()
		{
			return this.m_SteamInventoryResult.ToString();
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0000F9D9 File Offset: 0x0000DBD9
		public override bool Equals(object other)
		{
			return other is SteamInventoryResult_t && this == (SteamInventoryResult_t)other;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0000F9F6 File Offset: 0x0000DBF6
		public override int GetHashCode()
		{
			return this.m_SteamInventoryResult.GetHashCode();
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0000FA03 File Offset: 0x0000DC03
		public static bool operator ==(SteamInventoryResult_t x, SteamInventoryResult_t y)
		{
			return x.m_SteamInventoryResult == y.m_SteamInventoryResult;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0000FA13 File Offset: 0x0000DC13
		public static bool operator !=(SteamInventoryResult_t x, SteamInventoryResult_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0000FA1F File Offset: 0x0000DC1F
		public static explicit operator SteamInventoryResult_t(int value)
		{
			return new SteamInventoryResult_t(value);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0000FA27 File Offset: 0x0000DC27
		public static explicit operator int(SteamInventoryResult_t that)
		{
			return that.m_SteamInventoryResult;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0000FA2F File Offset: 0x0000DC2F
		public bool Equals(SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult == other.m_SteamInventoryResult;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0000FA3F File Offset: 0x0000DC3F
		public int CompareTo(SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult.CompareTo(other.m_SteamInventoryResult);
		}

		// Token: 0x04000B90 RID: 2960
		public static readonly SteamInventoryResult_t Invalid = new SteamInventoryResult_t(-1);

		// Token: 0x04000B91 RID: 2961
		public int m_SteamInventoryResult;
	}
}
