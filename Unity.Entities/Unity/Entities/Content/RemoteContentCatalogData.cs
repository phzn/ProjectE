using System;
using Unity.Collections;

namespace Unity.Entities.Content
{
	// Token: 0x0200046F RID: 1135
	[Serializable]
	internal struct RemoteContentCatalogData
	{
		// Token: 0x040009E5 RID: 2533
		public BlobArray<RemoteContentCatalogData.RemoteContentLocationData> RemoteContentLocations;

		// Token: 0x040009E6 RID: 2534
		public BlobArray<RemoteContentCatalogData.RemoteContentSetData> ContentSets;

		// Token: 0x02000470 RID: 1136
		[Serializable]
		public struct RemoteContentLocationData
		{
			// Token: 0x040009E7 RID: 2535
			public RemoteContentId identifier;

			// Token: 0x040009E8 RID: 2536
			public RemoteContentLocation location;
		}

		// Token: 0x02000471 RID: 1137
		[Serializable]
		public struct RemoteContentSetData
		{
			// Token: 0x040009E9 RID: 2537
			public FixedString512Bytes Name;

			// Token: 0x040009EA RID: 2538
			public BlobArray<int> Ids;
		}
	}
}
