using System;
using System.Collections.Generic;
using Unity.Entities.Serialization;

namespace Unity.Entities.Content
{
	// Token: 0x02000476 RID: 1142
	internal interface IRuntimeCatalogDataSource
	{
		// Token: 0x06001C24 RID: 7204
		IEnumerable<ContentArchiveId> GetArchiveIds();

		// Token: 0x06001C25 RID: 7205
		IEnumerable<ContentFileId> GetFileIds(ContentArchiveId archiveId);

		// Token: 0x06001C26 RID: 7206
		IEnumerable<ContentFileId> GetDependencies(ContentFileId fileId);

		// Token: 0x06001C27 RID: 7207
		IEnumerable<ValueTuple<UntypedWeakReferenceId, long>> GetObjects(ContentFileId fileId);

		// Token: 0x06001C28 RID: 7208
		IEnumerable<ValueTuple<UntypedWeakReferenceId, string>> GetScenes(ContentFileId fileId);
	}
}
