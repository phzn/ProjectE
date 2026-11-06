using System;
using Unity.Entities;
using Unity.Transforms;

namespace Pathfinding.ECS
{
	// Token: 0x020002F4 RID: 756
	// (Invoke) Token: 0x060012D9 RID: 4825
	public delegate void BeforeControlDelegate(Entity entity, float dt, ref LocalTransform localTransform, ref AgentCylinderShape shape, ref AgentMovementPlane movementPlane, ref DestinationPoint destination, ref MovementState movementState, ref MovementSettings movementSettings);
}
