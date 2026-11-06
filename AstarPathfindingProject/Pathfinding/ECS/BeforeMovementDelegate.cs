using System;
using Unity.Entities;
using Unity.Transforms;

namespace Pathfinding.ECS
{
	// Token: 0x020002F6 RID: 758
	// (Invoke) Token: 0x060012E1 RID: 4833
	public delegate void BeforeMovementDelegate(Entity entity, float dt, ref LocalTransform localTransform, ref AgentCylinderShape shape, ref AgentMovementPlane movementPlane, ref DestinationPoint destination, ref MovementState movementState, ref MovementSettings movementSettings, ref MovementControl movementControl, ref ResolvedMovement resolvedMovement);
}
