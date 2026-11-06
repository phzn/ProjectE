using System;

namespace Steamworks
{
	// Token: 0x020001B5 RID: 437
	[Serializable]
	public struct UGCHandle_t : IEquatable<UGCHandle_t>, IComparable<UGCHandle_t>
	{
		// Token: 0x06000AED RID: 2797 RVA: 0x00010728 File Offset: 0x0000E928
		public UGCHandle_t(ulong value)
		{
			this.m_UGCHandle = value;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x00010731 File Offset: 0x0000E931
		public override string ToString()
		{
			return this.m_UGCHandle.ToString();
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0001073E File Offset: 0x0000E93E
		public override bool Equals(object other)
		{
			return other is UGCHandle_t && this == (UGCHandle_t)other;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0001075B File Offset: 0x0000E95B
		public override int GetHashCode()
		{
			return this.m_UGCHandle.GetHashCode();
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00010768 File Offset: 0x0000E968
		public static bool operator ==(UGCHandle_t x, UGCHandle_t y)
		{
			return x.m_UGCHandle == y.m_UGCHandle;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00010778 File Offset: 0x0000E978
		public static bool operator !=(UGCHandle_t x, UGCHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00010784 File Offset: 0x0000E984
		public static explicit operator UGCHandle_t(ulong value)
		{
			return new UGCHandle_t(value);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0001078C File Offset: 0x0000E98C
		public static explicit operator ulong(UGCHandle_t that)
		{
			return that.m_UGCHandle;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00010794 File Offset: 0x0000E994
		public bool Equals(UGCHandle_t other)
		{
			return this.m_UGCHandle == other.m_UGCHandle;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x000107A4 File Offset: 0x0000E9A4
		public int CompareTo(UGCHandle_t other)
		{
			return this.m_UGCHandle.CompareTo(other.m_UGCHandle);
		}

		// Token: 0x04000BEB RID: 3051
		public static readonly UGCHandle_t Invalid = new UGCHandle_t(ulong.MaxValue);

		// Token: 0x04000BEC RID: 3052
		public ulong m_UGCHandle;
	}
}
