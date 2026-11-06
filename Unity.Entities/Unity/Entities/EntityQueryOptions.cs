using System;

namespace Unity.Entities
{
	// Token: 0x02000212 RID: 530
	[Flags]
	public enum EntityQueryOptions
	{
		// Token: 0x04000570 RID: 1392
		Default = 0,
		// Token: 0x04000571 RID: 1393
		IncludePrefab = 1,
		// Token: 0x04000572 RID: 1394
		IncludeDisabledEntities = 2,
		// Token: 0x04000573 RID: 1395
		[Obsolete("This enum value has been renamed to IncludeDisabledEntities. (RemovedAfter Entities 1.0) (UnityUpgradable) -> IncludeDisabledEntities", false)]
		IncludeDisabled = 2,
		// Token: 0x04000574 RID: 1396
		FilterWriteGroup = 4,
		// Token: 0x04000575 RID: 1397
		IgnoreComponentEnabledState = 8,
		// Token: 0x04000576 RID: 1398
		IncludeSystems = 16,
		// Token: 0x04000577 RID: 1399
		IncludeMetaChunks = 32
	}
}
