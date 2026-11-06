using System;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x02000071 RID: 113
	internal struct BlobAssetCache : IDisposable
	{
		// Token: 0x060002A6 RID: 678 RVA: 0x0000C26B File Offset: 0x0000A46B
		public BlobAssetCache(AllocatorManager.AllocatorHandle allocator)
		{
			this.BlobAssetBatch = DynamicBlobAssetBatch.Allocate(allocator);
			this.BlobAssetRemap = new NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr>(1, allocator);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000C286 File Offset: 0x0000A486
		public void Dispose()
		{
			DynamicBlobAssetBatch.Free(this.BlobAssetBatch);
			this.BlobAssetRemap.Dispose();
			this.BlobAssetBatch = null;
		}

		// Token: 0x040000B3 RID: 179
		public NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr> BlobAssetRemap;

		// Token: 0x040000B4 RID: 180
		public unsafe DynamicBlobAssetBatch* BlobAssetBatch;
	}
}
