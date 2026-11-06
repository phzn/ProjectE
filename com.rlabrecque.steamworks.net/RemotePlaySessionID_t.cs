using System;

namespace Steamworks
{
	// Token: 0x020001B1 RID: 433
	[Serializable]
	public struct RemotePlaySessionID_t : IEquatable<RemotePlaySessionID_t>, IComparable<RemotePlaySessionID_t>
	{
		// Token: 0x06000AC2 RID: 2754 RVA: 0x000104C2 File Offset: 0x0000E6C2
		public RemotePlaySessionID_t(uint value)
		{
			this.m_RemotePlaySessionID = value;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x000104CB File Offset: 0x0000E6CB
		public override string ToString()
		{
			return this.m_RemotePlaySessionID.ToString();
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x000104D8 File Offset: 0x0000E6D8
		public override bool Equals(object other)
		{
			return other is RemotePlaySessionID_t && this == (RemotePlaySessionID_t)other;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x000104F5 File Offset: 0x0000E6F5
		public override int GetHashCode()
		{
			return this.m_RemotePlaySessionID.GetHashCode();
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00010502 File Offset: 0x0000E702
		public static bool operator ==(RemotePlaySessionID_t x, RemotePlaySessionID_t y)
		{
			return x.m_RemotePlaySessionID == y.m_RemotePlaySessionID;
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00010512 File Offset: 0x0000E712
		public static bool operator !=(RemotePlaySessionID_t x, RemotePlaySessionID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0001051E File Offset: 0x0000E71E
		public static explicit operator RemotePlaySessionID_t(uint value)
		{
			return new RemotePlaySessionID_t(value);
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00010526 File Offset: 0x0000E726
		public static explicit operator uint(RemotePlaySessionID_t that)
		{
			return that.m_RemotePlaySessionID;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0001052E File Offset: 0x0000E72E
		public bool Equals(RemotePlaySessionID_t other)
		{
			return this.m_RemotePlaySessionID == other.m_RemotePlaySessionID;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0001053E File Offset: 0x0000E73E
		public int CompareTo(RemotePlaySessionID_t other)
		{
			return this.m_RemotePlaySessionID.CompareTo(other.m_RemotePlaySessionID);
		}

		// Token: 0x04000BE4 RID: 3044
		public uint m_RemotePlaySessionID;
	}
}
