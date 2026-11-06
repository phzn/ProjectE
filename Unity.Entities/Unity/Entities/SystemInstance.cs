using System;

namespace Unity.Entities
{
	// Token: 0x02000365 RID: 869
	internal struct SystemInstance : IComponentData, IQueryTypeParameter
	{
		// Token: 0x040008CE RID: 2254
		internal unsafe SystemState* state;
	}
}
