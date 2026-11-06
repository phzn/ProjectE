using System;
using System.Collections.Generic;
using UnityEngine.Localization.Metadata;
using UnityEngine.Serialization;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x0200002F RID: 47
	public abstract class LocalizationTable : ScriptableObject, IMetadataCollection, IComparable<LocalizationTable>
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00005E3F File Offset: 0x0000403F
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00005E47 File Offset: 0x00004047
		public LocaleIdentifier LocaleIdentifier
		{
			get
			{
				return this.m_LocaleId;
			}
			set
			{
				this.m_LocaleId = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00005E50 File Offset: 0x00004050
		public string TableCollectionName
		{
			get
			{
				this.VerifySharedTableDataIsNotNull();
				return this.SharedData.TableCollectionName;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00005E63 File Offset: 0x00004063
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00005E6B File Offset: 0x0000406B
		public SharedTableData SharedData
		{
			get
			{
				return this.m_SharedData;
			}
			set
			{
				this.m_SharedData = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00005E74 File Offset: 0x00004074
		internal List<TableEntryData> TableData
		{
			get
			{
				return this.m_TableData;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00005E7C File Offset: 0x0000407C
		public IList<IMetadata> MetadataEntries
		{
			get
			{
				return this.m_Metadata.MetadataEntries;
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00005E89 File Offset: 0x00004089
		public TObject GetMetadata<TObject>() where TObject : IMetadata
		{
			return this.m_Metadata.GetMetadata<TObject>();
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00005E96 File Offset: 0x00004096
		public void GetMetadatas<TObject>(IList<TObject> foundItems) where TObject : IMetadata
		{
			this.m_Metadata.GetMetadatas<TObject>(foundItems);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00005EA4 File Offset: 0x000040A4
		public IList<TObject> GetMetadatas<TObject>() where TObject : IMetadata
		{
			return this.m_Metadata.GetMetadatas<TObject>();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00005EB1 File Offset: 0x000040B1
		public void AddMetadata(IMetadata md)
		{
			this.m_Metadata.AddMetadata(md);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00005EBF File Offset: 0x000040BF
		public bool RemoveMetadata(IMetadata md)
		{
			return this.m_Metadata.RemoveMetadata(md);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00005ECD File Offset: 0x000040CD
		public bool Contains(IMetadata md)
		{
			return this.m_Metadata.Contains(md);
		}

		// Token: 0x06000187 RID: 391
		public abstract void CreateEmpty(TableEntryReference entryReference);

		// Token: 0x06000188 RID: 392 RVA: 0x00005EDB File Offset: 0x000040DB
		protected long FindKeyId(string key, bool addKey)
		{
			this.VerifySharedTableDataIsNotNull();
			return this.SharedData.GetId(key, addKey);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00005EF0 File Offset: 0x000040F0
		private void VerifySharedTableDataIsNotNull()
		{
			if (this.SharedData == null)
			{
				throw new NullReferenceException("The Table \"" + base.name + "\" does not have a SharedTableData.");
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00005F1B File Offset: 0x0000411B
		public override string ToString()
		{
			return string.Format("{0}({1})", this.TableCollectionName, this.LocaleIdentifier);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00005F38 File Offset: 0x00004138
		public int CompareTo(LocalizationTable other)
		{
			if (other == null)
			{
				return 1;
			}
			return this.LocaleIdentifier.CompareTo(other.LocaleIdentifier);
		}

		// Token: 0x04000059 RID: 89
		[SerializeField]
		private LocaleIdentifier m_LocaleId;

		// Token: 0x0400005A RID: 90
		[FormerlySerializedAs("m_KeyDatabase")]
		[SerializeField]
		[HideInInspector]
		private SharedTableData m_SharedData;

		// Token: 0x0400005B RID: 91
		[SerializeField]
		private MetadataCollection m_Metadata = new MetadataCollection();

		// Token: 0x0400005C RID: 92
		[SerializeField]
		private List<TableEntryData> m_TableData = new List<TableEntryData>();
	}
}
