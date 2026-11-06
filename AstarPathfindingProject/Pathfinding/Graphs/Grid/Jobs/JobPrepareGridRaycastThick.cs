using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000231 RID: 561
	[BurstCompile]
	public struct JobPrepareGridRaycastThick : IJob
	{
		// Token: 0x06000E41 RID: 3649 RVA: 0x000583E0 File Offset: 0x000565E0
		public void Execute()
		{
			float magnitude = this.raycastDirection.magnitude;
			int3 size = this.bounds.size;
			Vector3 normalized = this.raycastDirection.normalized;
			UnsafeSpan<SpherecastCommand> unsafeSpan = this.raycastCommands.AsUnsafeSpan<SpherecastCommand>();
			QueryParameters queryParameters = new QueryParameters(this.raycastMask, false, QueryTriggerInteraction.Ignore, false);
			unsafeSpan.Fill(new SpherecastCommand(this.physicsScene, Vector3.zero, this.radius, normalized, queryParameters, magnitude));
			for (int i = 0; i < size.z; i++)
			{
				int num = i * size.x;
				for (int j = 0; j < size.x; j++)
				{
					int num2 = num + j;
					Vector3 vector = JobNodeGridLayout.NodePosition(this.graphToWorld, j + this.bounds.min.x, i + this.bounds.min.z, 0f);
					unsafeSpan[num2].origin = vector + this.raycastOffset;
				}
			}
		}

		// Token: 0x04000A92 RID: 2706
		public Matrix4x4 graphToWorld;

		// Token: 0x04000A93 RID: 2707
		public IntBounds bounds;

		// Token: 0x04000A94 RID: 2708
		public Vector3 raycastOffset;

		// Token: 0x04000A95 RID: 2709
		public Vector3 raycastDirection;

		// Token: 0x04000A96 RID: 2710
		public LayerMask raycastMask;

		// Token: 0x04000A97 RID: 2711
		public PhysicsScene physicsScene;

		// Token: 0x04000A98 RID: 2712
		public float radius;

		// Token: 0x04000A99 RID: 2713
		[WriteOnly]
		public NativeArray<SpherecastCommand> raycastCommands;
	}
}
