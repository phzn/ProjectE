using System;
using Unity.Entities;

namespace Unity.Transforms
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public struct Parent : IComponentData, IQueryTypeParameter
	{
		// Token: 0x0400004B RID: 75
		public Entity Value;
	}
}
