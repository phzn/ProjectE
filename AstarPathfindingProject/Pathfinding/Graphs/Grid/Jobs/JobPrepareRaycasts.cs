using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000232 RID: 562
	[BurstCompile]
	public struct JobPrepareRaycasts : IJob
	{
		// Token: 0x06000E42 RID: 3650 RVA: 0x000584E8 File Offset: 0x000566E8
		public void Execute()
		{
			Vector3 normalized = this.direction.normalized;
			UnsafeSpan<RaycastCommand> unsafeSpan = this.raycastCommands.AsUnsafeSpan<RaycastCommand>();
			QueryParameters queryParameters = new QueryParameters(this.mask, false, QueryTriggerInteraction.Ignore, false);
			RaycastCommand raycastCommand = new RaycastCommand(this.physicsScene, Vector3.zero, normalized, queryParameters, this.distance);
			unsafeSpan.Fill(raycastCommand);
			for (int i = 0; i < this.raycastCommands.Length; i++)
			{
				unsafeSpan[i].from = this.origins[i] + this.originOffset;
			}
		}

		// Token: 0x04000A9A RID: 2714
		public Vector3 direction;

		// Token: 0x04000A9B RID: 2715
		public Vector3 originOffset;

		// Token: 0x04000A9C RID: 2716
		public float distance;

		// Token: 0x04000A9D RID: 2717
		public LayerMask mask;

		// Token: 0x04000A9E RID: 2718
		public PhysicsScene physicsScene;

		// Token: 0x04000A9F RID: 2719
		[ReadOnly]
		public NativeArray<Vector3> origins;

		// Token: 0x04000AA0 RID: 2720
		[WriteOnly]
		public NativeArray<RaycastCommand> raycastCommands;
	}
}
