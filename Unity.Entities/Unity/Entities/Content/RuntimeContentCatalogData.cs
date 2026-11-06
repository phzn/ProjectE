using System;

namespace Unity.Entities.Content
{
	// Token: 0x02000485 RID: 1157
	[Serializable]
	internal struct RuntimeContentCatalogData
	{
		// Token: 0x04000A16 RID: 2582
		public BlobArray<ContentArchiveLocation> Archives;

		// Token: 0x04000A17 RID: 2583
		public BlobArray<ContentFileLocation> Files;

		// Token: 0x04000A18 RID: 2584
		public BlobArray<ContentObjectLocation> Objects;

		// Token: 0x04000A19 RID: 2585
		public BlobArray<ContentSceneLocation> Scenes;

		// Token: 0x04000A1A RID: 2586
		public BlobArray<BlobArray<int>> Dependencies;

		// Token: 0x04000A1B RID: 2587
		public BlobArray<ContentBlobLocation> Blobs;
	}
}
