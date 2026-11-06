using System;

namespace Unity.Entities
{
	// Token: 0x020000EB RID: 235
	internal struct EntityCommand
	{
		// Token: 0x04000267 RID: 615
		public BasicCommand Header;

		// Token: 0x04000268 RID: 616
		public Entity Entity;

		// Token: 0x04000269 RID: 617
		public int IdentityIndex;

		// Token: 0x0400026A RID: 618
		public int BatchCount;
	}
}
