using System;

namespace Unity.Entities.Content
{
	// Token: 0x02000481 RID: 1153
	[Serializable]
	internal struct ContentFileLocation
	{
		// Token: 0x04000A09 RID: 2569
		public ContentFileId FileId;

		// Token: 0x04000A0A RID: 2570
		public int ArchiveIndex;

		// Token: 0x04000A0B RID: 2571
		public int DependencyIndex;
	}
}
