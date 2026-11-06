using System;
using System.Collections.Generic;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000E0 RID: 224
	[Serializable]
	public class MetadataCollection : IMetadataCollection
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00014E07 File Offset: 0x00013007
		public IList<IMetadata> MetadataEntries
		{
			get
			{
				return this.m_Items;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x00014E0F File Offset: 0x0001300F
		public bool HasData
		{
			get
			{
				return this.MetadataEntries != null && this.MetadataEntries.Count > 0;
			}
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00014E29 File Offset: 0x00013029
		public bool HasMetadata<TObject>() where TObject : IMetadata
		{
			return this.GetMetadata<TObject>() != null;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00014E3C File Offset: 0x0001303C
		public TObject GetMetadata<TObject>() where TObject : IMetadata
		{
			foreach (IMetadata metadata in this.m_Items)
			{
				if (metadata is TObject)
				{
					return (TObject)((object)metadata);
				}
			}
			return default(TObject);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00014EA8 File Offset: 0x000130A8
		public void GetMetadatas<TObject>(IList<TObject> foundItems) where TObject : IMetadata
		{
			foundItems.Clear();
			foreach (IMetadata metadata in this.m_Items)
			{
				if (metadata is TObject)
				{
					TObject tobject = (TObject)((object)metadata);
					foundItems.Add(tobject);
				}
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00014F10 File Offset: 0x00013110
		public IList<TObject> GetMetadatas<TObject>() where TObject : IMetadata
		{
			List<TObject> list = new List<TObject>();
			this.GetMetadatas<TObject>(list);
			return list;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00014F2B File Offset: 0x0001312B
		public void AddMetadata(IMetadata md)
		{
			this.MetadataEntries.Add(md);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00014F39 File Offset: 0x00013139
		public bool RemoveMetadata(IMetadata md)
		{
			return this.m_Items.Remove(md);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00014F47 File Offset: 0x00013147
		public bool Contains(IMetadata md)
		{
			return this.m_Items.Contains(md);
		}

		// Token: 0x04000212 RID: 530
		[SerializeReference]
		private List<IMetadata> m_Items = new List<IMetadata>();
	}
}
