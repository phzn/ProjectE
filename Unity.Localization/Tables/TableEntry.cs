using System;
using System.Collections.Generic;
using UnityEngine.Localization.Metadata;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x02000029 RID: 41
	public class TableEntry : IMetadataCollection
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000050C6 File Offset: 0x000032C6
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000050CE File Offset: 0x000032CE
		public LocalizationTable Table { get; internal set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600012F RID: 303 RVA: 0x000050D7 File Offset: 0x000032D7
		// (set) Token: 0x06000130 RID: 304 RVA: 0x000050DF File Offset: 0x000032DF
		internal TableEntryData Data { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000131 RID: 305 RVA: 0x000050E8 File Offset: 0x000032E8
		public SharedTableData.SharedTableEntry SharedEntry
		{
			get
			{
				if (this.m_SharedTableEntry == null)
				{
					this.m_SharedTableEntry = this.Table.SharedData.GetEntry(this.KeyId);
				}
				return this.m_SharedTableEntry;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00005114 File Offset: 0x00003314
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00005127 File Offset: 0x00003327
		public string Key
		{
			get
			{
				SharedTableData.SharedTableEntry sharedEntry = this.SharedEntry;
				if (sharedEntry == null)
				{
					return null;
				}
				return sharedEntry.Key;
			}
			set
			{
				this.Table.SharedData.RenameKey(this.KeyId, value);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00005140 File Offset: 0x00003340
		public long KeyId
		{
			get
			{
				return this.Data.Id;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000514D File Offset: 0x0000334D
		public string LocalizedValue
		{
			get
			{
				return this.Data.Localized;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000136 RID: 310 RVA: 0x0000515A File Offset: 0x0000335A
		public IList<IMetadata> MetadataEntries
		{
			get
			{
				return this.Data.Metadata.MetadataEntries;
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000516C File Offset: 0x0000336C
		public TObject GetMetadata<TObject>() where TObject : IMetadata
		{
			return this.Data.Metadata.GetMetadata<TObject>();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000517E File Offset: 0x0000337E
		public void GetMetadatas<TObject>(IList<TObject> foundItems) where TObject : IMetadata
		{
			this.Data.Metadata.GetMetadatas<TObject>(foundItems);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005191 File Offset: 0x00003391
		public IList<TObject> GetMetadatas<TObject>() where TObject : IMetadata
		{
			return this.Data.Metadata.GetMetadatas<TObject>();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000051A3 File Offset: 0x000033A3
		public bool HasTagMetadata<TShared>() where TShared : SharedTableEntryMetadata
		{
			TShared tshared = this.Table.GetMetadata<TShared>();
			return tshared != null && tshared.IsRegistered(this);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000051C4 File Offset: 0x000033C4
		public void AddTagMetadata<TShared>() where TShared : SharedTableEntryMetadata, new()
		{
			TShared tshared = default(TShared);
			foreach (IMetadata metadata in this.Table.MetadataEntries)
			{
				TShared tshared2 = metadata as TShared;
				if (tshared2 != null)
				{
					tshared = tshared2;
					if (tshared.IsRegistered(this))
					{
						return;
					}
					break;
				}
			}
			if (tshared == null)
			{
				tshared = new TShared();
				this.Table.AddMetadata(tshared);
			}
			tshared.Register(this);
			this.AddMetadata(tshared);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00005274 File Offset: 0x00003474
		public void AddSharedMetadata(SharedTableEntryMetadata md)
		{
			if (!this.Table.Contains(md))
			{
				this.Table.AddMetadata(md);
			}
			if (md.IsRegistered(this))
			{
				return;
			}
			md.Register(this);
			this.AddMetadata(md);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000052A8 File Offset: 0x000034A8
		public void AddSharedMetadata(SharedTableCollectionMetadata md)
		{
			if (!this.Table.SharedData.Metadata.Contains(md))
			{
				this.Table.SharedData.Metadata.AddMetadata(md);
			}
			md.AddEntry(this.Data.Id, this.Table.LocaleIdentifier.Code);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00005307 File Offset: 0x00003507
		public void AddMetadata(IMetadata md)
		{
			this.Data.Metadata.AddMetadata(md);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000531C File Offset: 0x0000351C
		public void RemoveTagMetadata<TShared>() where TShared : SharedTableEntryMetadata
		{
			IList<IMetadata> metadataEntries = this.Table.MetadataEntries;
			IList<IMetadata> metadataEntries2 = this.Data.Metadata.MetadataEntries;
			for (int i = metadataEntries2.Count - 1; i >= 0; i--)
			{
				TShared tshared = metadataEntries2[i] as TShared;
				if (tshared != null)
				{
					tshared.Unregister(this);
					metadataEntries2.RemoveAt(i);
				}
			}
			for (int j = metadataEntries.Count - 1; j >= 0; j--)
			{
				TShared tshared2 = metadataEntries[j] as TShared;
				if (tshared2 != null)
				{
					tshared2.Unregister(this);
					if (tshared2.Count == 0)
					{
						metadataEntries.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000053DD File Offset: 0x000035DD
		public void RemoveSharedMetadata(SharedTableEntryMetadata md)
		{
			md.Unregister(this);
			this.RemoveMetadata(md);
			if (md.Count == 0 && this.Table.Contains(md))
			{
				this.Table.RemoveMetadata(md);
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005414 File Offset: 0x00003614
		public void RemoveSharedMetadata(SharedTableCollectionMetadata md)
		{
			md.RemoveEntry(this.Data.Id, this.Table.LocaleIdentifier.Code);
			if (md.IsEmpty)
			{
				this.Table.SharedData.Metadata.RemoveMetadata(md);
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005464 File Offset: 0x00003664
		public bool RemoveMetadata(IMetadata md)
		{
			return this.Data.Metadata.RemoveMetadata(md);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005477 File Offset: 0x00003677
		public bool Contains(IMetadata md)
		{
			return this.Data.Metadata.Contains(md);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000548A File Offset: 0x0000368A
		public override string ToString()
		{
			return string.Format("{0} - {1}", this.KeyId, this.LocalizedValue);
		}

		// Token: 0x04000047 RID: 71
		private SharedTableData.SharedTableEntry m_SharedTableEntry;
	}
}
