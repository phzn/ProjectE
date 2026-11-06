using System;
using Pathfinding.PID;
using Unity.Entities;
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x02000300 RID: 768
	[Serializable]
	public struct MovementSettings : IComponentData, IQueryTypeParameter
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001306 RID: 4870 RVA: 0x000738E1 File Offset: 0x00071AE1
		// (set) Token: 0x06001307 RID: 4871 RVA: 0x000738E9 File Offset: 0x00071AE9
		[Obsolete("Use the AgentMovementPlaneSource component instead, or the movementPlaneSource property on the FollowerEntity component", true)]
		public MovementPlaneSource movementPlaneSource { readonly get; set; }

		// Token: 0x04000DE0 RID: 3552
		public PIDMovement follower;

		// Token: 0x04000DE1 RID: 3553
		public PIDMovement.DebugFlags debugFlags;

		// Token: 0x04000DE2 RID: 3554
		public float stopDistance;

		// Token: 0x04000DE3 RID: 3555
		public float rotationSmoothing;

		// Token: 0x04000DE4 RID: 3556
		public float positionSmoothing;

		// Token: 0x04000DE5 RID: 3557
		public LayerMask groundMask;

		// Token: 0x04000DE7 RID: 3559
		public bool isStopped;
	}
}
