using System;

namespace Unity.Entities
{
	// Token: 0x020002DD RID: 733
	[Flags]
	internal enum ArchetypeFlags : ushort
	{
		// Token: 0x04000720 RID: 1824
		CleanupComplete = 1,
		// Token: 0x04000721 RID: 1825
		CleanupNeeded = 2,
		// Token: 0x04000722 RID: 1826
		Disabled = 4,
		// Token: 0x04000723 RID: 1827
		Prefab = 8,
		// Token: 0x04000724 RID: 1828
		HasChunkHeader = 16,
		// Token: 0x04000725 RID: 1829
		HasBlobAssetRefs = 32,
		// Token: 0x04000726 RID: 1830
		HasCompanionComponents = 64,
		// Token: 0x04000727 RID: 1831
		HasBufferComponents = 128,
		// Token: 0x04000728 RID: 1832
		HasManagedComponents = 256,
		// Token: 0x04000729 RID: 1833
		HasManagedEntityRefs = 512,
		// Token: 0x0400072A RID: 1834
		HasWeakAssetRefs = 1024,
		// Token: 0x0400072B RID: 1835
		HasSystemInstanceComponents = 2048,
		// Token: 0x0400072C RID: 1836
		HasUnityObjectRefs = 4096
	}
}
