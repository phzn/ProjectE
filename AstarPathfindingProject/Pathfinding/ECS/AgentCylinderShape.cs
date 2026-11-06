using System;
using Unity.Entities;

namespace Pathfinding.ECS
{
	// Token: 0x020002E4 RID: 740
	[Serializable]
	public struct AgentCylinderShape : IComponentData, IQueryTypeParameter
	{
		// Token: 0x04000D9F RID: 3487
		public float radius;

		// Token: 0x04000DA0 RID: 3488
		public float height;
	}
}
