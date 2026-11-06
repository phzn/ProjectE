using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200022F RID: 559
	[BurstCompile]
	public struct JobPrepareCapsuleCommands : IJob
	{
		// Token: 0x06000E3F RID: 3647 RVA: 0x00058238 File Offset: 0x00056438
		public void Execute()
		{
			UnsafeSpan<OverlapCapsuleCommand> unsafeSpan = this.commands.AsUnsafeSpan<OverlapCapsuleCommand>();
			QueryParameters queryParameters = new QueryParameters(this.mask, false, QueryTriggerInteraction.Ignore, false);
			unsafeSpan.Fill(new OverlapCapsuleCommand(this.physicsScene, Vector3.zero, Vector3.zero, this.radius, queryParameters));
			for (int i = 0; i < unsafeSpan.Length; i++)
			{
				Vector3 vector = this.origins[i] + this.originOffset;
				unsafeSpan[i].point0 = vector;
				unsafeSpan[i].point1 = vector + this.direction;
			}
		}

		// Token: 0x04000A84 RID: 2692
		public Vector3 direction;

		// Token: 0x04000A85 RID: 2693
		public Vector3 originOffset;

		// Token: 0x04000A86 RID: 2694
		public float radius;

		// Token: 0x04000A87 RID: 2695
		public LayerMask mask;

		// Token: 0x04000A88 RID: 2696
		public PhysicsScene physicsScene;

		// Token: 0x04000A89 RID: 2697
		[ReadOnly]
		public NativeArray<Vector3> origins;

		// Token: 0x04000A8A RID: 2698
		[WriteOnly]
		public NativeArray<OverlapCapsuleCommand> commands;
	}
}
