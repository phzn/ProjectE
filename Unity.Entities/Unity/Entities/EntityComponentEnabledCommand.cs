using System;

namespace Unity.Entities
{
	// Token: 0x020000F8 RID: 248
	internal struct EntityComponentEnabledCommand
	{
		// Token: 0x04000294 RID: 660
		public EntityEnabledCommand Header;

		// Token: 0x04000295 RID: 661
		public TypeIndex ComponentTypeIndex;
	}
}
