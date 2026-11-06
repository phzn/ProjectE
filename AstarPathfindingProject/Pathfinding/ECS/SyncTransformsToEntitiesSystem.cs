using System;
using System.Runtime.CompilerServices;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

namespace Pathfinding.ECS
{
	// Token: 0x020003A9 RID: 937
	[UpdateBefore(typeof(TransformSystemGroup))]
	[UpdateBefore(typeof(AIMovementSystemGroup))]
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	[CompilerGenerated]
	public struct SyncTransformsToEntitiesSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x06001797 RID: 6039 RVA: 0x0007F464 File Offset: 0x0007D664
		public void OnUpdate(ref SystemState systemState)
		{
			TransformAccessArray transformAccessArray;
			FollowerEntity[] array;
			int components = BatchedEvents.GetComponents<FollowerEntity>(BatchedEvents.Event.None, out transformAccessArray, out array);
			if (components > 0)
			{
				NativeArray<Entity> nativeArray = new NativeArray<Entity>(components, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < components; i++)
				{
					nativeArray[i] = array[i].entity;
				}
				SyncTransformsToEntitiesSystem.SyncTransformsToEntitiesJob syncTransformsToEntitiesJob = default(SyncTransformsToEntitiesSystem.SyncTransformsToEntitiesJob);
				syncTransformsToEntitiesJob.entities = nativeArray;
				syncTransformsToEntitiesJob.entityPositions = InternalCompilerInterface.GetComponentLookup<LocalTransform>(ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref systemState);
				syncTransformsToEntitiesJob.syncPositionWithTransform = InternalCompilerInterface.GetComponentLookup<SyncPositionWithTransform>(ref this.__TypeHandle.__Pathfinding_ECS_SyncPositionWithTransform_RO_ComponentLookup, ref systemState);
				syncTransformsToEntitiesJob.syncRotationWithTransform = InternalCompilerInterface.GetComponentLookup<SyncRotationWithTransform>(ref this.__TypeHandle.__Pathfinding_ECS_SyncRotationWithTransform_RO_ComponentLookup, ref systemState);
				syncTransformsToEntitiesJob.orientationYAxisForward = InternalCompilerInterface.GetComponentLookup<OrientationYAxisForward>(ref this.__TypeHandle.__Pathfinding_ECS_OrientationYAxisForward_RO_ComponentLookup, ref systemState);
				syncTransformsToEntitiesJob.movementState = InternalCompilerInterface.GetComponentLookup<MovementState>(ref this.__TypeHandle.__Pathfinding_ECS_MovementState_RO_ComponentLookup, ref systemState);
				systemState.Dependency = syncTransformsToEntitiesJob.Schedule(transformAccessArray, systemState.Dependency);
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x0007F54C File Offset: 0x0007D74C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			entityQueryBuilder.Dispose();
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x0007F56D File Offset: 0x0007D76D
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x0007F5B8 File Offset: 0x0007D7B8
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((SyncTransformsToEntitiesSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x0007F5D1 File Offset: 0x0007D7D1
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((SyncTransformsToEntitiesSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000FA1 RID: 4001
		public static readonly quaternion ZAxisForwardToYAxisForward = quaternion.Euler(1.5707964f, 0f, 0f, math.RotationOrder.ZXY);

		// Token: 0x04000FA2 RID: 4002
		public static readonly quaternion YAxisForwardToZAxisForward = quaternion.Euler(-1.5707964f, 0f, 0f, math.RotationOrder.ZXY);

		// Token: 0x04000FA3 RID: 4003
		private SyncTransformsToEntitiesSystem.TypeHandle __TypeHandle;

		// Token: 0x020003AA RID: 938
		[BurstCompile]
		private struct SyncTransformsToEntitiesJob : IJobParallelForTransform
		{
			// Token: 0x0600179D RID: 6045 RVA: 0x0007F5EC File Offset: 0x0007D7EC
			public void Execute(int index, TransformAccess transform)
			{
				Entity entity = this.entities[index];
				if (this.entityPositions.HasComponent(entity))
				{
					ref LocalTransform valueRW = ref this.entityPositions.GetRefRW(entity).ValueRW;
					float3 @float = float3.zero;
					MovementState movementState;
					if (this.movementState.TryGetComponent(entity, out movementState))
					{
						@float = movementState.positionOffset;
					}
					if (this.syncPositionWithTransform.HasComponent(entity))
					{
						valueRW.Position = transform.position - @float;
					}
					if (this.syncRotationWithTransform.HasComponent(entity))
					{
						if (this.orientationYAxisForward.HasComponent(entity))
						{
							valueRW.Rotation = math.mul(transform.rotation, SyncTransformsToEntitiesSystem.YAxisForwardToZAxisForward);
						}
						else
						{
							valueRW.Rotation = transform.rotation;
						}
					}
					valueRW.Scale = transform.localScale.y;
				}
			}

			// Token: 0x04000FA4 RID: 4004
			[ReadOnly]
			[DeallocateOnJobCompletion]
			public NativeArray<Entity> entities;

			// Token: 0x04000FA5 RID: 4005
			[NativeDisableParallelForRestriction]
			public ComponentLookup<LocalTransform> entityPositions;

			// Token: 0x04000FA6 RID: 4006
			[ReadOnly]
			public ComponentLookup<SyncPositionWithTransform> syncPositionWithTransform;

			// Token: 0x04000FA7 RID: 4007
			[ReadOnly]
			public ComponentLookup<SyncRotationWithTransform> syncRotationWithTransform;

			// Token: 0x04000FA8 RID: 4008
			[ReadOnly]
			public ComponentLookup<OrientationYAxisForward> orientationYAxisForward;

			// Token: 0x04000FA9 RID: 4009
			[ReadOnly]
			public ComponentLookup<MovementState> movementState;
		}

		// Token: 0x020003AB RID: 939
		private struct TypeHandle
		{
			// Token: 0x0600179E RID: 6046 RVA: 0x0007F6D0 File Offset: 0x0007D8D0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>(false);
				this.__Pathfinding_ECS_SyncPositionWithTransform_RO_ComponentLookup = state.GetComponentLookup<SyncPositionWithTransform>(true);
				this.__Pathfinding_ECS_SyncRotationWithTransform_RO_ComponentLookup = state.GetComponentLookup<SyncRotationWithTransform>(true);
				this.__Pathfinding_ECS_OrientationYAxisForward_RO_ComponentLookup = state.GetComponentLookup<OrientationYAxisForward>(true);
				this.__Pathfinding_ECS_MovementState_RO_ComponentLookup = state.GetComponentLookup<MovementState>(true);
			}

			// Token: 0x04000FAA RID: 4010
			public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

			// Token: 0x04000FAB RID: 4011
			[ReadOnly]
			public ComponentLookup<SyncPositionWithTransform> __Pathfinding_ECS_SyncPositionWithTransform_RO_ComponentLookup;

			// Token: 0x04000FAC RID: 4012
			[ReadOnly]
			public ComponentLookup<SyncRotationWithTransform> __Pathfinding_ECS_SyncRotationWithTransform_RO_ComponentLookup;

			// Token: 0x04000FAD RID: 4013
			[ReadOnly]
			public ComponentLookup<OrientationYAxisForward> __Pathfinding_ECS_OrientationYAxisForward_RO_ComponentLookup;

			// Token: 0x04000FAE RID: 4014
			[ReadOnly]
			public ComponentLookup<MovementState> __Pathfinding_ECS_MovementState_RO_ComponentLookup;
		}
	}
}
