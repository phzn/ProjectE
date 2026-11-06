using System;

namespace Unity.Entities
{
	// Token: 0x02000019 RID: 25
	internal struct RetainBlobAssetBatchPtr : ICleanupComponentData, IComponentData, IQueryTypeParameter
	{
		// Token: 0x04000020 RID: 32
		public unsafe BlobAssetBatch* BlobAssetBatchPtr;
	}
}
