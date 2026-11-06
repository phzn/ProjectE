using System;

namespace Unity.Entities
{
	// Token: 0x020000B4 RID: 180
	internal struct DuplicateEntityGuid
	{
		// Token: 0x06000357 RID: 855 RVA: 0x00013C5D File Offset: 0x00011E5D
		public DuplicateEntityGuid(EntityGuid entityGuid, int duplicateCount)
		{
			this.EntityGuid = entityGuid;
			this.DuplicateCount = duplicateCount;
		}

		// Token: 0x040001DE RID: 478
		public EntityGuid EntityGuid;

		// Token: 0x040001DF RID: 479
		public int DuplicateCount;
	}
}
