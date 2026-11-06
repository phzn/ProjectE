using System;

namespace Unity.Entities
{
	// Token: 0x020000EC RID: 236
	internal struct EntityQueryCommand
	{
		// Token: 0x0400026B RID: 619
		public BasicCommand Header;

		// Token: 0x0400026C RID: 620
		public unsafe EntityQueryImpl* QueryImpl;
	}
}
