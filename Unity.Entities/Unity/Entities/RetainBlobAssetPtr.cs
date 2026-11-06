using System;

namespace Unity.Entities
{
	// Token: 0x0200001A RID: 26
	internal struct RetainBlobAssetPtr : ICleanupComponentData, IComponentData, IQueryTypeParameter
	{
		// Token: 0x04000021 RID: 33
		public unsafe BlobAssetHeader* BlobAsset;
	}
}
