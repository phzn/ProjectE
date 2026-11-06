using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

namespace Pathfinding.ECS
{
	// Token: 0x02000351 RID: 849
	[BurstCompile]
	internal struct JobSyncEntitiesToTransforms : IJobParallelForTransform
	{
		// Token: 0x0600153A RID: 5434 RVA: 0x00079AE0 File Offset: 0x00077CE0
		public void Execute(int index, TransformAccess transform)
		{
			Entity entity = this.entities[index];
			if (!this.entityPositions.HasComponent(entity))
			{
				return;
			}
			float3 @float = float3.zero;
			MovementState movementState;
			if (this.movementState.TryGetComponent(entity, out movementState))
			{
				@float = movementState.positionOffset;
			}
			RefRO<LocalTransform> refRO = this.entityPositions.GetRefRO(entity);
			if (this.syncPositionWithTransform.HasComponent(entity))
			{
				transform.position = refRO.ValueRO.Position + @float;
			}
			if (this.syncRotationWithTransform.HasComponent(entity))
			{
				if (this.orientationYAxisForward.HasComponent(entity))
				{
					transform.rotation = math.mul(refRO.ValueRO.Rotation, SyncTransformsToEntitiesSystem.ZAxisForwardToYAxisForward);
					return;
				}
				transform.rotation = refRO.ValueRO.Rotation;
			}
		}

		// Token: 0x04000ED8 RID: 3800
		[ReadOnly]
		public NativeArray<Entity> entities;

		// Token: 0x04000ED9 RID: 3801
		[ReadOnly]
		public ComponentLookup<LocalTransform> entityPositions;

		// Token: 0x04000EDA RID: 3802
		[ReadOnly]
		public ComponentLookup<MovementState> movementState;

		// Token: 0x04000EDB RID: 3803
		[ReadOnly]
		public ComponentLookup<SyncPositionWithTransform> syncPositionWithTransform;

		// Token: 0x04000EDC RID: 3804
		[ReadOnly]
		public ComponentLookup<SyncRotationWithTransform> syncRotationWithTransform;

		// Token: 0x04000EDD RID: 3805
		[ReadOnly]
		public ComponentLookup<OrientationYAxisForward> orientationYAxisForward;
	}
}
