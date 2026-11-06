using System;

namespace Steamworks
{
	// Token: 0x020001BF RID: 447
	[Serializable]
	public struct UGCQueryHandle_t : IEquatable<UGCQueryHandle_t>, IComparable<UGCQueryHandle_t>
	{
		// Token: 0x06000B53 RID: 2899 RVA: 0x00010E6E File Offset: 0x0000F06E
		public UGCQueryHandle_t(ulong value)
		{
			this.m_UGCQueryHandle = value;
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00010E77 File Offset: 0x0000F077
		public override string ToString()
		{
			return this.m_UGCQueryHandle.ToString();
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00010E84 File Offset: 0x0000F084
		public override bool Equals(object other)
		{
			return other is UGCQueryHandle_t && this == (UGCQueryHandle_t)other;
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00010EA1 File Offset: 0x0000F0A1
		public override int GetHashCode()
		{
			return this.m_UGCQueryHandle.GetHashCode();
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00010EAE File Offset: 0x0000F0AE
		public static bool operator ==(UGCQueryHandle_t x, UGCQueryHandle_t y)
		{
			return x.m_UGCQueryHandle == y.m_UGCQueryHandle;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00010EBE File Offset: 0x0000F0BE
		public static bool operator !=(UGCQueryHandle_t x, UGCQueryHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00010ECA File Offset: 0x0000F0CA
		public static explicit operator UGCQueryHandle_t(ulong value)
		{
			return new UGCQueryHandle_t(value);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00010ED2 File Offset: 0x0000F0D2
		public static explicit operator ulong(UGCQueryHandle_t that)
		{
			return that.m_UGCQueryHandle;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00010EDA File Offset: 0x0000F0DA
		public bool Equals(UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle == other.m_UGCQueryHandle;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00010EEA File Offset: 0x0000F0EA
		public int CompareTo(UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);
		}

		// Token: 0x04000BFE RID: 3070
		public static readonly UGCQueryHandle_t Invalid = new UGCQueryHandle_t(ulong.MaxValue);

		// Token: 0x04000BFF RID: 3071
		public ulong m_UGCQueryHandle;
	}
}
