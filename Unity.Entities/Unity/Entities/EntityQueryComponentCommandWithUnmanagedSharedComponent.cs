using System;

namespace Unity.Entities
{
	// Token: 0x020000EF RID: 239
	internal struct EntityQueryComponentCommandWithUnmanagedSharedComponent
	{
		// Token: 0x04000271 RID: 625
		public EntityQueryComponentCommand Header;

		// Token: 0x04000272 RID: 626
		public int ComponentSize;

		// Token: 0x04000273 RID: 627
		public int HashCode;

		// Token: 0x04000274 RID: 628
		public byte IsDefault;

		// Token: 0x04000275 RID: 629
		public byte ValueRequiresEntityFixup;
	}
}
