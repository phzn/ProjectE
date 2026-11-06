using System;

namespace Steamworks
{
	// Token: 0x0200019E RID: 414
	[Serializable]
	public struct HServerListRequest : IEquatable<HServerListRequest>
	{
		// Token: 0x06000A29 RID: 2601 RVA: 0x0000FC28 File Offset: 0x0000DE28
		public HServerListRequest(IntPtr value)
		{
			this.m_HServerListRequest = value;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0000FC31 File Offset: 0x0000DE31
		public override string ToString()
		{
			return this.m_HServerListRequest.ToString();
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0000FC3E File Offset: 0x0000DE3E
		public override bool Equals(object other)
		{
			return other is HServerListRequest && this == (HServerListRequest)other;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0000FC5B File Offset: 0x0000DE5B
		public override int GetHashCode()
		{
			return this.m_HServerListRequest.GetHashCode();
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0000FC68 File Offset: 0x0000DE68
		public static bool operator ==(HServerListRequest x, HServerListRequest y)
		{
			return x.m_HServerListRequest == y.m_HServerListRequest;
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0000FC7B File Offset: 0x0000DE7B
		public static bool operator !=(HServerListRequest x, HServerListRequest y)
		{
			return !(x == y);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0000FC87 File Offset: 0x0000DE87
		public static explicit operator HServerListRequest(IntPtr value)
		{
			return new HServerListRequest(value);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0000FC8F File Offset: 0x0000DE8F
		public static explicit operator IntPtr(HServerListRequest that)
		{
			return that.m_HServerListRequest;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0000FC97 File Offset: 0x0000DE97
		public bool Equals(HServerListRequest other)
		{
			return this.m_HServerListRequest == other.m_HServerListRequest;
		}

		// Token: 0x04000B97 RID: 2967
		public static readonly HServerListRequest Invalid = new HServerListRequest(IntPtr.Zero);

		// Token: 0x04000B98 RID: 2968
		public IntPtr m_HServerListRequest;
	}
}
