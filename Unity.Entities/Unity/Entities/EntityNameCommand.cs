using System;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x020000F9 RID: 249
	internal struct EntityNameCommand
	{
		// Token: 0x04000296 RID: 662
		public EntityCommand Header;

		// Token: 0x04000297 RID: 663
		public FixedString64Bytes Name;
	}
}
