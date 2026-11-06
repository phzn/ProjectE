using System;

namespace Steamworks
{
	// Token: 0x0200019C RID: 412
	[Serializable]
	public struct SteamItemDef_t : IEquatable<SteamItemDef_t>, IComparable<SteamItemDef_t>
	{
		// Token: 0x06000A14 RID: 2580 RVA: 0x0000FAFC File Offset: 0x0000DCFC
		public SteamItemDef_t(int value)
		{
			this.m_SteamItemDef = value;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0000FB05 File Offset: 0x0000DD05
		public override string ToString()
		{
			return this.m_SteamItemDef.ToString();
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0000FB12 File Offset: 0x0000DD12
		public override bool Equals(object other)
		{
			return other is SteamItemDef_t && this == (SteamItemDef_t)other;
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0000FB2F File Offset: 0x0000DD2F
		public override int GetHashCode()
		{
			return this.m_SteamItemDef.GetHashCode();
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0000FB3C File Offset: 0x0000DD3C
		public static bool operator ==(SteamItemDef_t x, SteamItemDef_t y)
		{
			return x.m_SteamItemDef == y.m_SteamItemDef;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0000FB4C File Offset: 0x0000DD4C
		public static bool operator !=(SteamItemDef_t x, SteamItemDef_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0000FB58 File Offset: 0x0000DD58
		public static explicit operator SteamItemDef_t(int value)
		{
			return new SteamItemDef_t(value);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0000FB60 File Offset: 0x0000DD60
		public static explicit operator int(SteamItemDef_t that)
		{
			return that.m_SteamItemDef;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0000FB68 File Offset: 0x0000DD68
		public bool Equals(SteamItemDef_t other)
		{
			return this.m_SteamItemDef == other.m_SteamItemDef;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0000FB78 File Offset: 0x0000DD78
		public int CompareTo(SteamItemDef_t other)
		{
			return this.m_SteamItemDef.CompareTo(other.m_SteamItemDef);
		}

		// Token: 0x04000B94 RID: 2964
		public int m_SteamItemDef;
	}
}
