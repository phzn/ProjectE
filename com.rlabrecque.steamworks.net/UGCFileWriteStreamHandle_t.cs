using System;

namespace Steamworks
{
	// Token: 0x020001B4 RID: 436
	[Serializable]
	public struct UGCFileWriteStreamHandle_t : IEquatable<UGCFileWriteStreamHandle_t>, IComparable<UGCFileWriteStreamHandle_t>
	{
		// Token: 0x06000AE2 RID: 2786 RVA: 0x0001068B File Offset: 0x0000E88B
		public UGCFileWriteStreamHandle_t(ulong value)
		{
			this.m_UGCFileWriteStreamHandle = value;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00010694 File Offset: 0x0000E894
		public override string ToString()
		{
			return this.m_UGCFileWriteStreamHandle.ToString();
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x000106A1 File Offset: 0x0000E8A1
		public override bool Equals(object other)
		{
			return other is UGCFileWriteStreamHandle_t && this == (UGCFileWriteStreamHandle_t)other;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x000106BE File Offset: 0x0000E8BE
		public override int GetHashCode()
		{
			return this.m_UGCFileWriteStreamHandle.GetHashCode();
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x000106CB File Offset: 0x0000E8CB
		public static bool operator ==(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
		{
			return x.m_UGCFileWriteStreamHandle == y.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x000106DB File Offset: 0x0000E8DB
		public static bool operator !=(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x000106E7 File Offset: 0x0000E8E7
		public static explicit operator UGCFileWriteStreamHandle_t(ulong value)
		{
			return new UGCFileWriteStreamHandle_t(value);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x000106EF File Offset: 0x0000E8EF
		public static explicit operator ulong(UGCFileWriteStreamHandle_t that)
		{
			return that.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x000106F7 File Offset: 0x0000E8F7
		public bool Equals(UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle == other.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00010707 File Offset: 0x0000E907
		public int CompareTo(UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle.CompareTo(other.m_UGCFileWriteStreamHandle);
		}

		// Token: 0x04000BE9 RID: 3049
		public static readonly UGCFileWriteStreamHandle_t Invalid = new UGCFileWriteStreamHandle_t(ulong.MaxValue);

		// Token: 0x04000BEA RID: 3050
		public ulong m_UGCFileWriteStreamHandle;
	}
}
