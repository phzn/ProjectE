using System;

namespace Unity.Entities
{
	// Token: 0x020000F6 RID: 246
	internal struct EntityComponentCommand
	{
		// Token: 0x0400028E RID: 654
		public EntityCommand Header;

		// Token: 0x0400028F RID: 655
		public TypeIndex ComponentTypeIndex;

		// Token: 0x04000290 RID: 656
		public short ComponentSize;

		// Token: 0x04000291 RID: 657
		public byte ValueRequiresEntityFixup;
	}
}
