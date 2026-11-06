using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000233 RID: 563
	[BurstCompile]
	public struct JobPrepareSphereCommands : IJob
	{
		// Token: 0x06000E43 RID: 3651 RVA: 0x00058584 File Offset: 0x00056784
		public void Execute()
		{
			UnsafeSpan<OverlapSphereCommand> unsafeSpan = this.commands.AsUnsafeSpan<OverlapSphereCommand>();
			QueryParameters queryParameters = new QueryParameters(this.mask, false, QueryTriggerInteraction.Ignore, false);
			unsafeSpan.Fill(new OverlapSphereCommand(this.physicsScene, Vector3.zero, this.radius, queryParameters));
			for (int i = 0; i < unsafeSpan.Length; i++)
			{
				Vector3 vector = this.origins[i] + this.originOffset;
				unsafeSpan[i].point = vector;
			}
		}

		// Token: 0x04000AA1 RID: 2721
		public Vector3 originOffset;

		// Token: 0x04000AA2 RID: 2722
		public float radius;

		// Token: 0x04000AA3 RID: 2723
		public LayerMask mask;

		// Token: 0x04000AA4 RID: 2724
		public PhysicsScene physicsScene;

		// Token: 0x04000AA5 RID: 2725
		[ReadOnly]
		public NativeArray<Vector3> origins;

		// Token: 0x04000AA6 RID: 2726
		[WriteOnly]
		public NativeArray<OverlapSphereCommand> commands;
	}
}
