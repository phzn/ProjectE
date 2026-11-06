using System;

namespace Steamworks
{
	// Token: 0x02000190 RID: 400
	[Serializable]
	public struct FriendsGroupID_t : IEquatable<FriendsGroupID_t>, IComparable<FriendsGroupID_t>
	{
		// Token: 0x060009A6 RID: 2470 RVA: 0x0000F517 File Offset: 0x0000D717
		public FriendsGroupID_t(short value)
		{
			this.m_FriendsGroupID = value;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0000F520 File Offset: 0x0000D720
		public override string ToString()
		{
			return this.m_FriendsGroupID.ToString();
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0000F52D File Offset: 0x0000D72D
		public override bool Equals(object other)
		{
			return other is FriendsGroupID_t && this == (FriendsGroupID_t)other;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0000F54A File Offset: 0x0000D74A
		public override int GetHashCode()
		{
			return this.m_FriendsGroupID.GetHashCode();
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0000F557 File Offset: 0x0000D757
		public static bool operator ==(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return x.m_FriendsGroupID == y.m_FriendsGroupID;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0000F567 File Offset: 0x0000D767
		public static bool operator !=(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return !(x == y);
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0000F573 File Offset: 0x0000D773
		public static explicit operator FriendsGroupID_t(short value)
		{
			return new FriendsGroupID_t(value);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0000F57B File Offset: 0x0000D77B
		public static explicit operator short(FriendsGroupID_t that)
		{
			return that.m_FriendsGroupID;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0000F583 File Offset: 0x0000D783
		public bool Equals(FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID == other.m_FriendsGroupID;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0000F593 File Offset: 0x0000D793
		public int CompareTo(FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID.CompareTo(other.m_FriendsGroupID);
		}

		// Token: 0x04000B81 RID: 2945
		public static readonly FriendsGroupID_t Invalid = new FriendsGroupID_t(-1);

		// Token: 0x04000B82 RID: 2946
		public short m_FriendsGroupID;
	}
}
