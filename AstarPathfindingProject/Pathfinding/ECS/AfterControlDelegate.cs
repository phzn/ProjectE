using System;
using Unity.Entities;
using Unity.Transforms;

namespace Pathfinding.ECS
{
	// Token: 0x020002F5 RID: 757
	// (Invoke) Token: 0x060012DD RID: 4829
	public delegate void AfterControlDelegate(Entity entity, float dt, ref LocalTransform localTransform, ref AgentCylinderShape shape, ref AgentMovementPlane movementPlane, ref DestinationPoint destination, ref MovementState movementState, ref MovementSettings movementSettings, ref MovementControl movementControl);
}
