using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000230 RID: 560
	[BurstCompile]
	public struct JobPrepareGridRaycast : IJob
	{
		// Token: 0x06000E40 RID: 3648 RVA: 0x000582DC File Offset: 0x000564DC
		public void Execute()
		{
			float magnitude = this.raycastDirection.magnitude;
			int3 size = this.bounds.size;
			Vector3 normalized = this.raycastDirection.normalized;
			UnsafeSpan<RaycastCommand> unsafeSpan = this.raycastCommands.AsUnsafeSpan<RaycastCommand>();
			QueryParameters queryParameters = new QueryParameters(this.raycastMask, false, QueryTriggerInteraction.Ignore, false);
			unsafeSpan.Fill(new RaycastCommand(this.physicsScene, Vector3.zero, normalized, queryParameters, magnitude));
			for (int i = 0; i < size.z; i++)
			{
				int num = i * size.x;
				for (int j = 0; j < size.x; j++)
				{
					int num2 = num + j;
					Vector3 vector = JobNodeGridLayout.NodePosition(this.graphToWorld, j + this.bounds.min.x, i + this.bounds.min.z, 0f);
					unsafeSpan[num2].from = vector + this.raycastOffset;
				}
			}
		}

		// Token: 0x04000A8B RID: 2699
		public Matrix4x4 graphToWorld;

		// Token: 0x04000A8C RID: 2700
		public IntBounds bounds;

		// Token: 0x04000A8D RID: 2701
		public Vector3 raycastOffset;

		// Token: 0x04000A8E RID: 2702
		public Vector3 raycastDirection;

		// Token: 0x04000A8F RID: 2703
		public LayerMask raycastMask;

		// Token: 0x04000A90 RID: 2704
		public PhysicsScene physicsScene;

		// Token: 0x04000A91 RID: 2705
		[WriteOnly]
		public NativeArray<RaycastCommand> raycastCommands;
	}
}
