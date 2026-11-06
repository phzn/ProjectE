using System;
using Unity.Entities;

namespace Pathfinding.ECS
{
	// Token: 0x020002F3 RID: 755
	public struct GravityState : IComponentData, IQueryTypeParameter, IEnableableComponent
	{
		// Token: 0x04000DC4 RID: 3524
		public float verticalVelocity;
	}
}
