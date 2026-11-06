using System;
using System.Collections.Generic;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000DF RID: 223
	public interface IMetadataCollection
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000600 RID: 1536
		IList<IMetadata> MetadataEntries { get; }

		// Token: 0x06000601 RID: 1537
		TObject GetMetadata<TObject>() where TObject : IMetadata;

		// Token: 0x06000602 RID: 1538
		void GetMetadatas<TObject>(IList<TObject> foundItems) where TObject : IMetadata;

		// Token: 0x06000603 RID: 1539
		IList<TObject> GetMetadatas<TObject>() where TObject : IMetadata;

		// Token: 0x06000604 RID: 1540
		void AddMetadata(IMetadata md);

		// Token: 0x06000605 RID: 1541
		bool RemoveMetadata(IMetadata md);

		// Token: 0x06000606 RID: 1542
		bool Contains(IMetadata md);
	}
}
