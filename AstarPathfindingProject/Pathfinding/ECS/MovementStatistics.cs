using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Pathfinding.ECS
{
	// Token: 0x02000302 RID: 770
	public struct MovementStatistics : IComponentData, IQueryTypeParameter
	{
		// Token: 0x04000DFC RID: 3580
		public float3 estimatedVelocity;

		// Token: 0x04000DFD RID: 3581
		public float3 lastPosition;
	}
}
