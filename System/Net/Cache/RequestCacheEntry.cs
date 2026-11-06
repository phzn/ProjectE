using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace System.Net.Cache
{
	// Token: 0x02000783 RID: 1923
	internal class RequestCacheEntry
	{
		// Token: 0x06003C9D RID: 15517 RVA: 0x000CE668 File Offset: 0x000CC868
		internal RequestCacheEntry()
		{
			this.m_ExpiresUtc = (this.m_LastAccessedUtc = (this.m_LastModifiedUtc = (this.m_LastSynchronizedUtc = DateTime.MinValue)));
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06003C9E RID: 15518 RVA: 0x000CE6A1 File Offset: 0x000CC8A1
		// (set) Token: 0x06003C9F RID: 15519 RVA: 0x000CE6A9 File Offset: 0x000CC8A9
		internal bool IsPrivateEntry
		{
			get
			{
				return this.m_IsPrivateEntry;
			}
			set
			{
				this.m_IsPrivateEntry = value;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06003CA0 RID: 15520 RVA: 0x000CE6B2 File Offset: 0x000CC8B2
		// (set) Token: 0x06003CA1 RID: 15521 RVA: 0x000CE6BA File Offset: 0x000CC8BA
		internal long StreamSize
		{
			get
			{
				return this.m_StreamSize;
			}
			set
			{
				this.m_StreamSize = value;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06003CA2 RID: 15522 RVA: 0x000CE6C3 File Offset: 0x000CC8C3
		// (set) Token: 0x06003CA3 RID: 15523 RVA: 0x000CE6CB File Offset: 0x000CC8CB
		internal DateTime ExpiresUtc
		{
			get
			{
				return this.m_ExpiresUtc;
			}
			set
			{
				this.m_ExpiresUtc = value;
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x06003CA4 RID: 15524 RVA: 0x000CE6D4 File Offset: 0x000CC8D4
		// (set) Token: 0x06003CA5 RID: 15525 RVA: 0x000CE6DC File Offset: 0x000CC8DC
		internal DateTime LastAccessedUtc
		{
			get
			{
				return this.m_LastAccessedUtc;
			}
			set
			{
				this.m_LastAccessedUtc = value;
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06003CA6 RID: 15526 RVA: 0x000CE6E5 File Offset: 0x000CC8E5
		// (set) Token: 0x06003CA7 RID: 15527 RVA: 0x000CE6ED File Offset: 0x000CC8ED
		internal DateTime LastModifiedUtc
		{
			get
			{
				return this.m_LastModifiedUtc;
			}
			set
			{
				this.m_LastModifiedUtc = value;
			}
		}

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x000CE6F6 File Offset: 0x000CC8F6
		// (set) Token: 0x06003CA9 RID: 15529 RVA: 0x000CE6FE File Offset: 0x000CC8FE
		internal DateTime LastSynchronizedUtc
		{
			get
			{
				return this.m_LastSynchronizedUtc;
			}
			set
			{
				this.m_LastSynchronizedUtc = value;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06003CAA RID: 15530 RVA: 0x000CE707 File Offset: 0x000CC907
		// (set) Token: 0x06003CAB RID: 15531 RVA: 0x000CE70F File Offset: 0x000CC90F
		internal TimeSpan MaxStale
		{
			get
			{
				return this.m_MaxStale;
			}
			set
			{
				this.m_MaxStale = value;
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06003CAC RID: 15532 RVA: 0x000CE718 File Offset: 0x000CC918
		// (set) Token: 0x06003CAD RID: 15533 RVA: 0x000CE720 File Offset: 0x000CC920
		internal int HitCount
		{
			get
			{
				return this.m_HitCount;
			}
			set
			{
				this.m_HitCount = value;
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06003CAE RID: 15534 RVA: 0x000CE729 File Offset: 0x000CC929
		// (set) Token: 0x06003CAF RID: 15535 RVA: 0x000CE731 File Offset: 0x000CC931
		internal int UsageCount
		{
			get
			{
				return this.m_UsageCount;
			}
			set
			{
				this.m_UsageCount = value;
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06003CB0 RID: 15536 RVA: 0x000CE73A File Offset: 0x000CC93A
		// (set) Token: 0x06003CB1 RID: 15537 RVA: 0x000CE742 File Offset: 0x000CC942
		internal bool IsPartialEntry
		{
			get
			{
				return this.m_IsPartialEntry;
			}
			set
			{
				this.m_IsPartialEntry = value;
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06003CB2 RID: 15538 RVA: 0x000CE74B File Offset: 0x000CC94B
		// (set) Token: 0x06003CB3 RID: 15539 RVA: 0x000CE753 File Offset: 0x000CC953
		internal StringCollection EntryMetadata
		{
			get
			{
				return this.m_EntryMetadata;
			}
			set
			{
				this.m_EntryMetadata = value;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06003CB4 RID: 15540 RVA: 0x000CE75C File Offset: 0x000CC95C
		// (set) Token: 0x06003CB5 RID: 15541 RVA: 0x000CE764 File Offset: 0x000CC964
		internal StringCollection SystemMetadata
		{
			get
			{
				return this.m_SystemMetadata;
			}
			set
			{
				this.m_SystemMetadata = value;
			}
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x000CE770 File Offset: 0x000CC970
		internal virtual string ToString(bool verbose)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			stringBuilder.Append("\r\nIsPrivateEntry   = ").Append(this.IsPrivateEntry);
			stringBuilder.Append("\r\nIsPartialEntry   = ").Append(this.IsPartialEntry);
			stringBuilder.Append("\r\nStreamSize       = ").Append(this.StreamSize);
			stringBuilder.Append("\r\nExpires          = ").Append((this.ExpiresUtc == DateTime.MinValue) ? "" : this.ExpiresUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastAccessed     = ").Append((this.LastAccessedUtc == DateTime.MinValue) ? "" : this.LastAccessedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastModified     = ").Append((this.LastModifiedUtc == DateTime.MinValue) ? "" : this.LastModifiedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastSynchronized = ").Append((this.LastSynchronizedUtc == DateTime.MinValue) ? "" : this.LastSynchronizedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nMaxStale(sec)    = ").Append((this.MaxStale == TimeSpan.MinValue) ? "" : ((int)this.MaxStale.TotalSeconds).ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\nHitCount         = ").Append(this.HitCount.ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\nUsageCount       = ").Append(this.UsageCount.ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\n");
			if (verbose)
			{
				stringBuilder.Append("EntryMetadata:\r\n");
				if (this.m_EntryMetadata != null)
				{
					foreach (string text in this.m_EntryMetadata)
					{
						stringBuilder.Append(text).Append("\r\n");
					}
				}
				stringBuilder.Append("---\r\nSystemMetadata:\r\n");
				if (this.m_SystemMetadata != null)
				{
					foreach (string text2 in this.m_SystemMetadata)
					{
						stringBuilder.Append(text2).Append("\r\n");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040023D5 RID: 9173
		private bool m_IsPrivateEntry;

		// Token: 0x040023D6 RID: 9174
		private long m_StreamSize;

		// Token: 0x040023D7 RID: 9175
		private DateTime m_ExpiresUtc;

		// Token: 0x040023D8 RID: 9176
		private int m_HitCount;

		// Token: 0x040023D9 RID: 9177
		private DateTime m_LastAccessedUtc;

		// Token: 0x040023DA RID: 9178
		private DateTime m_LastModifiedUtc;

		// Token: 0x040023DB RID: 9179
		private DateTime m_LastSynchronizedUtc;

		// Token: 0x040023DC RID: 9180
		private TimeSpan m_MaxStale;

		// Token: 0x040023DD RID: 9181
		private int m_UsageCount;

		// Token: 0x040023DE RID: 9182
		private bool m_IsPartialEntry;

		// Token: 0x040023DF RID: 9183
		private StringCollection m_EntryMetadata;

		// Token: 0x040023E0 RID: 9184
		private StringCollection m_SystemMetadata;
	}
}
