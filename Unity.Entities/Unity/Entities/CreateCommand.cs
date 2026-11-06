using System;

namespace Unity.Entities
{
	// Token: 0x020000EA RID: 234
	internal struct CreateCommand
	{
		// Token: 0x04000263 RID: 611
		public BasicCommand Header;

		// Token: 0x04000264 RID: 612
		public EntityArchetype Archetype;

		// Token: 0x04000265 RID: 613
		public int IdentityIndex;

		// Token: 0x04000266 RID: 614
		public int BatchCount;
	}
}
