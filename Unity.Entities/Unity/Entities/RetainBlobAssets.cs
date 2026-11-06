using System;

namespace Unity.Entities
{
	// Token: 0x02000018 RID: 24
	internal struct RetainBlobAssets : ICleanupComponentData, IComponentData, IQueryTypeParameter
	{
		// Token: 0x0400001E RID: 30
		public BlobAssetReference<byte> DummyBlobAssetReference;

		// Token: 0x0400001F RID: 31
		public int FramesToRetainBlobAssets;
	}
}
