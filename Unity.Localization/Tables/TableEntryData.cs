using System;
using UnityEngine.Localization.Metadata;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x02000033 RID: 51
	[Serializable]
	internal class TableEntryData
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00006AB5 File Offset: 0x00004CB5
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00006ABD File Offset: 0x00004CBD
		public long Id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00006AC6 File Offset: 0x00004CC6
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00006ACE File Offset: 0x00004CCE
		public string Localized
		{
			get
			{
				return this.m_Localized;
			}
			set
			{
				this.m_Localized = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00006AD7 File Offset: 0x00004CD7
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00006ADF File Offset: 0x00004CDF
		public MetadataCollection Metadata
		{
			get
			{
				return this.m_Metadata;
			}
			set
			{
				this.m_Metadata = value;
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00006AE8 File Offset: 0x00004CE8
		public TableEntryData()
		{
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00006AFB File Offset: 0x00004CFB
		public TableEntryData(long id)
		{
			this.Id = id;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00006B15 File Offset: 0x00004D15
		public TableEntryData(long id, string localized)
			: this(id)
		{
			this.Localized = localized;
		}

		// Token: 0x04000068 RID: 104
		[SerializeField]
		private long m_Id;

		// Token: 0x04000069 RID: 105
		[SerializeField]
		private string m_Localized;

		// Token: 0x0400006A RID: 106
		[SerializeField]
		private MetadataCollection m_Metadata = new MetadataCollection();
	}
}
