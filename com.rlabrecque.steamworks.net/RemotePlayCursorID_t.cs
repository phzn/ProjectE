using System;

namespace Steamworks
{
	// Token: 0x020001AF RID: 431
	[Serializable]
	public struct RemotePlayCursorID_t : IEquatable<RemotePlayCursorID_t>, IComparable<RemotePlayCursorID_t>
	{
		// Token: 0x06000AB8 RID: 2744 RVA: 0x00010433 File Offset: 0x0000E633
		public RemotePlayCursorID_t(uint value)
		{
			this.m_RemotePlayCursorID = value;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0001043C File Offset: 0x0000E63C
		public override string ToString()
		{
			return this.m_RemotePlayCursorID.ToString();
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00010449 File Offset: 0x0000E649
		public override bool Equals(object other)
		{
			return other is RemotePlayCursorID_t && this == (RemotePlayCursorID_t)other;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00010466 File Offset: 0x0000E666
		public override int GetHashCode()
		{
			return this.m_RemotePlayCursorID.GetHashCode();
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00010473 File Offset: 0x0000E673
		public static bool operator ==(RemotePlayCursorID_t x, RemotePlayCursorID_t y)
		{
			return x.m_RemotePlayCursorID == y.m_RemotePlayCursorID;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00010483 File Offset: 0x0000E683
		public static bool operator !=(RemotePlayCursorID_t x, RemotePlayCursorID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0001048F File Offset: 0x0000E68F
		public static explicit operator RemotePlayCursorID_t(uint value)
		{
			return new RemotePlayCursorID_t(value);
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00010497 File Offset: 0x0000E697
		public static explicit operator uint(RemotePlayCursorID_t that)
		{
			return that.m_RemotePlayCursorID;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0001049F File Offset: 0x0000E69F
		public bool Equals(RemotePlayCursorID_t other)
		{
			return this.m_RemotePlayCursorID == other.m_RemotePlayCursorID;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x000104AF File Offset: 0x0000E6AF
		public int CompareTo(RemotePlayCursorID_t other)
		{
			return this.m_RemotePlayCursorID.CompareTo(other.m_RemotePlayCursorID);
		}

		// Token: 0x04000BE0 RID: 3040
		public uint m_RemotePlayCursorID;
	}
}
