using System;

namespace Steamworks
{
	// Token: 0x020001B3 RID: 435
	[Serializable]
	public struct PublishedFileUpdateHandle_t : IEquatable<PublishedFileUpdateHandle_t>, IComparable<PublishedFileUpdateHandle_t>
	{
		// Token: 0x06000AD7 RID: 2775 RVA: 0x000105EE File Offset: 0x0000E7EE
		public PublishedFileUpdateHandle_t(ulong value)
		{
			this.m_PublishedFileUpdateHandle = value;
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x000105F7 File Offset: 0x0000E7F7
		public override string ToString()
		{
			return this.m_PublishedFileUpdateHandle.ToString();
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00010604 File Offset: 0x0000E804
		public override bool Equals(object other)
		{
			return other is PublishedFileUpdateHandle_t && this == (PublishedFileUpdateHandle_t)other;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00010621 File Offset: 0x0000E821
		public override int GetHashCode()
		{
			return this.m_PublishedFileUpdateHandle.GetHashCode();
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0001062E File Offset: 0x0000E82E
		public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
		{
			return x.m_PublishedFileUpdateHandle == y.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0001063E File Offset: 0x0000E83E
		public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0001064A File Offset: 0x0000E84A
		public static explicit operator PublishedFileUpdateHandle_t(ulong value)
		{
			return new PublishedFileUpdateHandle_t(value);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00010652 File Offset: 0x0000E852
		public static explicit operator ulong(PublishedFileUpdateHandle_t that)
		{
			return that.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0001065A File Offset: 0x0000E85A
		public bool Equals(PublishedFileUpdateHandle_t other)
		{
			return this.m_PublishedFileUpdateHandle == other.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0001066A File Offset: 0x0000E86A
		public int CompareTo(PublishedFileUpdateHandle_t other)
		{
			return this.m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);
		}

		// Token: 0x04000BE7 RID: 3047
		public static readonly PublishedFileUpdateHandle_t Invalid = new PublishedFileUpdateHandle_t(ulong.MaxValue);

		// Token: 0x04000BE8 RID: 3048
		public ulong m_PublishedFileUpdateHandle;
	}
}
