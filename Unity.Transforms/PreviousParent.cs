using System;
using Unity.Entities;

namespace Unity.Transforms
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public struct PreviousParent : ICleanupComponentData, IComponentData, IQueryTypeParameter
	{
		// Token: 0x0400004C RID: 76
		public Entity Value;
	}
}
