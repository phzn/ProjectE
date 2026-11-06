using System;

namespace Steamworks
{
	// Token: 0x020001C0 RID: 448
	[Serializable]
	public struct UGCUpdateHandle_t : IEquatable<UGCUpdateHandle_t>, IComparable<UGCUpdateHandle_t>
	{
		// Token: 0x06000B5E RID: 2910 RVA: 0x00010F0B File Offset: 0x0000F10B
		public UGCUpdateHandle_t(ulong value)
		{
			this.m_UGCUpdateHandle = value;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00010F14 File Offset: 0x0000F114
		public override string ToString()
		{
			return this.m_UGCUpdateHandle.ToString();
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00010F21 File Offset: 0x0000F121
		public override bool Equals(object other)
		{
			return other is UGCUpdateHandle_t && this == (UGCUpdateHandle_t)other;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00010F3E File Offset: 0x0000F13E
		public override int GetHashCode()
		{
			return this.m_UGCUpdateHandle.GetHashCode();
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00010F4B File Offset: 0x0000F14B
		public static bool operator ==(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
		{
			return x.m_UGCUpdateHandle == y.m_UGCUpdateHandle;
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00010F5B File Offset: 0x0000F15B
		public static bool operator !=(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00010F67 File Offset: 0x0000F167
		public static explicit operator UGCUpdateHandle_t(ulong value)
		{
			return new UGCUpdateHandle_t(value);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00010F6F File Offset: 0x0000F16F
		public static explicit operator ulong(UGCUpdateHandle_t that)
		{
			return that.m_UGCUpdateHandle;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00010F77 File Offset: 0x0000F177
		public bool Equals(UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle == other.m_UGCUpdateHandle;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00010F87 File Offset: 0x0000F187
		public int CompareTo(UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle.CompareTo(other.m_UGCUpdateHandle);
		}

		// Token: 0x04000C00 RID: 3072
		public static readonly UGCUpdateHandle_t Invalid = new UGCUpdateHandle_t(ulong.MaxValue);

		// Token: 0x04000C01 RID: 3073
		public ulong m_UGCUpdateHandle;
	}
}
