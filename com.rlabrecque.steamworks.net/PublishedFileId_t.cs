using System;

namespace Steamworks
{
	// Token: 0x020001B2 RID: 434
	[Serializable]
	public struct PublishedFileId_t : IEquatable<PublishedFileId_t>, IComparable<PublishedFileId_t>
	{
		// Token: 0x06000ACC RID: 2764 RVA: 0x00010551 File Offset: 0x0000E751
		public PublishedFileId_t(ulong value)
		{
			this.m_PublishedFileId = value;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0001055A File Offset: 0x0000E75A
		public override string ToString()
		{
			return this.m_PublishedFileId.ToString();
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00010567 File Offset: 0x0000E767
		public override bool Equals(object other)
		{
			return other is PublishedFileId_t && this == (PublishedFileId_t)other;
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00010584 File Offset: 0x0000E784
		public override int GetHashCode()
		{
			return this.m_PublishedFileId.GetHashCode();
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00010591 File Offset: 0x0000E791
		public static bool operator ==(PublishedFileId_t x, PublishedFileId_t y)
		{
			return x.m_PublishedFileId == y.m_PublishedFileId;
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x000105A1 File Offset: 0x0000E7A1
		public static bool operator !=(PublishedFileId_t x, PublishedFileId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x000105AD File Offset: 0x0000E7AD
		public static explicit operator PublishedFileId_t(ulong value)
		{
			return new PublishedFileId_t(value);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x000105B5 File Offset: 0x0000E7B5
		public static explicit operator ulong(PublishedFileId_t that)
		{
			return that.m_PublishedFileId;
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x000105BD File Offset: 0x0000E7BD
		public bool Equals(PublishedFileId_t other)
		{
			return this.m_PublishedFileId == other.m_PublishedFileId;
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000105CD File Offset: 0x0000E7CD
		public int CompareTo(PublishedFileId_t other)
		{
			return this.m_PublishedFileId.CompareTo(other.m_PublishedFileId);
		}

		// Token: 0x04000BE5 RID: 3045
		public static readonly PublishedFileId_t Invalid = new PublishedFileId_t(0UL);

		// Token: 0x04000BE6 RID: 3046
		public ulong m_PublishedFileId;
	}
}
