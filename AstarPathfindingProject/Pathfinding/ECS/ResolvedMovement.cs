using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Pathfinding.ECS
{
	// Token: 0x02000305 RID: 773
	public struct ResolvedMovement : IComponentData, IQueryTypeParameter
	{
		// Token: 0x04000DFF RID: 3583
		public float3 targetPoint;

		// Token: 0x04000E00 RID: 3584
		public float speed;

		// Token: 0x04000E01 RID: 3585
		public float turningRadiusMultiplier;

		// Token: 0x04000E02 RID: 3586
		public float targetRotation;

		// Token: 0x04000E03 RID: 3587
		public float targetRotationHint;

		// Token: 0x04000E04 RID: 3588
		public float targetRotationOffset;

		// Token: 0x04000E05 RID: 3589
		public float rotationSpeed;
	}
}
