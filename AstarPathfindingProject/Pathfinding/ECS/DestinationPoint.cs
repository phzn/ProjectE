using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Pathfinding.ECS
{
	// Token: 0x020002F2 RID: 754
	public struct DestinationPoint : IComponentData, IQueryTypeParameter
	{
		// Token: 0x04000DC2 RID: 3522
		public float3 destination;

		// Token: 0x04000DC3 RID: 3523
		public float3 facingDirection;
	}
}
