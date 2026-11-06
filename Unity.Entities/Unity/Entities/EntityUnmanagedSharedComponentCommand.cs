using System;

namespace Unity.Entities
{
	// Token: 0x020000FF RID: 255
	internal struct EntityUnmanagedSharedComponentCommand
	{
		// Token: 0x040002A7 RID: 679
		public EntityCommand Header;

		// Token: 0x040002A8 RID: 680
		public TypeIndex ComponentTypeIndex;

		// Token: 0x040002A9 RID: 681
		public int HashCode;

		// Token: 0x040002AA RID: 682
		public byte IsDefault;

		// Token: 0x040002AB RID: 683
		public byte ValueRequiresEntityFixup;
	}
}
