using System;

namespace Steamworks
{
	// Token: 0x0200019B RID: 411
	[Serializable]
	public struct SteamInventoryUpdateHandle_t : IEquatable<SteamInventoryUpdateHandle_t>, IComparable<SteamInventoryUpdateHandle_t>
	{
		// Token: 0x06000A09 RID: 2569 RVA: 0x0000FA5F File Offset: 0x0000DC5F
		public SteamInventoryUpdateHandle_t(ulong value)
		{
			this.m_SteamInventoryUpdateHandle = value;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0000FA68 File Offset: 0x0000DC68
		public override string ToString()
		{
			return this.m_SteamInventoryUpdateHandle.ToString();
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0000FA75 File Offset: 0x0000DC75
		public override bool Equals(object other)
		{
			return other is SteamInventoryUpdateHandle_t && this == (SteamInventoryUpdateHandle_t)other;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0000FA92 File Offset: 0x0000DC92
		public override int GetHashCode()
		{
			return this.m_SteamInventoryUpdateHandle.GetHashCode();
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0000FA9F File Offset: 0x0000DC9F
		public static bool operator ==(SteamInventoryUpdateHandle_t x, SteamInventoryUpdateHandle_t y)
		{
			return x.m_SteamInventoryUpdateHandle == y.m_SteamInventoryUpdateHandle;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0000FAAF File Offset: 0x0000DCAF
		public static bool operator !=(SteamInventoryUpdateHandle_t x, SteamInventoryUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0000FABB File Offset: 0x0000DCBB
		public static explicit operator SteamInventoryUpdateHandle_t(ulong value)
		{
			return new SteamInventoryUpdateHandle_t(value);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0000FAC3 File Offset: 0x0000DCC3
		public static explicit operator ulong(SteamInventoryUpdateHandle_t that)
		{
			return that.m_SteamInventoryUpdateHandle;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0000FACB File Offset: 0x0000DCCB
		public bool Equals(SteamInventoryUpdateHandle_t other)
		{
			return this.m_SteamInventoryUpdateHandle == other.m_SteamInventoryUpdateHandle;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0000FADB File Offset: 0x0000DCDB
		public int CompareTo(SteamInventoryUpdateHandle_t other)
		{
			return this.m_SteamInventoryUpdateHandle.CompareTo(other.m_SteamInventoryUpdateHandle);
		}

		// Token: 0x04000B92 RID: 2962
		public static readonly SteamInventoryUpdateHandle_t Invalid = new SteamInventoryUpdateHandle_t(ulong.MaxValue);

		// Token: 0x04000B93 RID: 2963
		public ulong m_SteamInventoryUpdateHandle;
	}
}
