using System;

namespace Unity.Entities
{
	// Token: 0x020000F2 RID: 242
	internal struct MultipleEntitiesComponentCommand
	{
		// Token: 0x0400027E RID: 638
		public MultipleEntitiesCommand Header;

		// Token: 0x0400027F RID: 639
		public TypeIndex ComponentTypeIndex;

		// Token: 0x04000280 RID: 640
		public short ComponentSize;

		// Token: 0x04000281 RID: 641
		public byte ValueRequiresEntityFixup;
	}
}
