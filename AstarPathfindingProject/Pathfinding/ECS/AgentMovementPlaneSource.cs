using System;
using Unity.Entities;

namespace Pathfinding.ECS
{
	// Token: 0x020002E6 RID: 742
	[Serializable]
	public struct AgentMovementPlaneSource : ISharedComponentData, IQueryTypeParameter
	{
		// Token: 0x04000DA2 RID: 3490
		public MovementPlaneSource value;
	}
}
