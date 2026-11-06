using System;

namespace Unity.Entities
{
	// Token: 0x020000F3 RID: 243
	internal struct MultipleEntitiesCommand_WithUnmanagedSharedComponent
	{
		// Token: 0x04000282 RID: 642
		public MultipleEntitiesCommand Header;

		// Token: 0x04000283 RID: 643
		public TypeIndex ComponentTypeIndex;

		// Token: 0x04000284 RID: 644
		public int ComponentSize;

		// Token: 0x04000285 RID: 645
		public int HashCode;

		// Token: 0x04000286 RID: 646
		public byte IsDefault;

		// Token: 0x04000287 RID: 647
		public byte ValueRequiresEntityFixup;
	}
}
