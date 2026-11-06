using System;

namespace Unity.Entities
{
	// Token: 0x020000FE RID: 254
	internal struct EntityMoveManagedComponentCommand
	{
		// Token: 0x040002A4 RID: 676
		public EntityCommand Header;

		// Token: 0x040002A5 RID: 677
		public Entity SrcEntity;

		// Token: 0x040002A6 RID: 678
		public TypeIndex ComponentTypeIndex;
	}
}
