using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Pathfinding.ECS
{
	// Token: 0x020002FE RID: 766
	public struct MovementControl : IComponentData, IQueryTypeParameter
	{
		// Token: 0x04000DD2 RID: 3538
		public float3 targetPoint;

		// Token: 0x04000DD3 RID: 3539
		public float3 endOfPath;

		// Token: 0x04000DD4 RID: 3540
		public float speed;

		// Token: 0x04000DD5 RID: 3541
		public float maxSpeed;

		// Token: 0x04000DD6 RID: 3542
		public int hierarchicalNodeIndex;

		// Token: 0x04000DD7 RID: 3543
		public float targetRotation;

		// Token: 0x04000DD8 RID: 3544
		public float targetRotationHint;

		// Token: 0x04000DD9 RID: 3545
		public float targetRotationOffset;

		// Token: 0x04000DDA RID: 3546
		public float rotationSpeed;

		// Token: 0x04000DDB RID: 3547
		public bool overrideLocalAvoidance;
	}
}
