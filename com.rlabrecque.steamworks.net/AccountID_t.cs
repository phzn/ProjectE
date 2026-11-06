using System;

namespace Steamworks
{
	// Token: 0x020001B8 RID: 440
	[Serializable]
	public struct AccountID_t : IEquatable<AccountID_t>, IComparable<AccountID_t>
	{
		// Token: 0x06000B0D RID: 2829 RVA: 0x000108F0 File Offset: 0x0000EAF0
		public AccountID_t(uint value)
		{
			this.m_AccountID = value;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x000108F9 File Offset: 0x0000EAF9
		public override string ToString()
		{
			return this.m_AccountID.ToString();
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00010906 File Offset: 0x0000EB06
		public override bool Equals(object other)
		{
			return other is AccountID_t && this == (AccountID_t)other;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00010923 File Offset: 0x0000EB23
		public override int GetHashCode()
		{
			return this.m_AccountID.GetHashCode();
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00010930 File Offset: 0x0000EB30
		public static bool operator ==(AccountID_t x, AccountID_t y)
		{
			return x.m_AccountID == y.m_AccountID;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00010940 File Offset: 0x0000EB40
		public static bool operator !=(AccountID_t x, AccountID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0001094C File Offset: 0x0000EB4C
		public static explicit operator AccountID_t(uint value)
		{
			return new AccountID_t(value);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00010954 File Offset: 0x0000EB54
		public static explicit operator uint(AccountID_t that)
		{
			return that.m_AccountID;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0001095C File Offset: 0x0000EB5C
		public bool Equals(AccountID_t other)
		{
			return this.m_AccountID == other.m_AccountID;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0001096C File Offset: 0x0000EB6C
		public int CompareTo(AccountID_t other)
		{
			return this.m_AccountID.CompareTo(other.m_AccountID);
		}

		// Token: 0x04000BF0 RID: 3056
		public static readonly AccountID_t Invalid = new AccountID_t(0U);

		// Token: 0x04000BF1 RID: 3057
		public uint m_AccountID;
	}
}
