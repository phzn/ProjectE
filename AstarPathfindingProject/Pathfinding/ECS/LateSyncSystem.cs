using System;
using System.Runtime.CompilerServices;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine.Jobs;

namespace Pathfinding.ECS
{
	// Token: 0x0200037B RID: 891
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[UpdateAfter(typeof(AIMoveSystem))]
	[UpdateAfter(typeof(MovementStatisticsSystem))]
	[RequireMatchingQueriesForUpdate]
	[CompilerGenerated]
	public struct LateSyncSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x06001664 RID: 5732 RVA: 0x0007C554 File Offset: 0x0007A754
		public void OnCreate(ref SystemState systemState)
		{
			this.jobRepairPathScheduler = new JobRepairPath.Scheduler(ref systemState);
			this.entityQueryPrepareMovement = this.jobRepairPathScheduler.GetEntityQuery(Allocator.Temp).WithAll<SimulateMovement, SimulateMovementRepair>().Build(ref systemState);
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0007C590 File Offset: 0x0007A790
		public void OnDestroy(ref SystemState systemState)
		{
			this.jobRepairPathScheduler.Dispose();
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x0007C5A0 File Offset: 0x0007A7A0
		public void OnUpdate(ref SystemState systemState)
		{
			systemState.Dependency = this.ScheduleRepairPaths(ref systemState, systemState.Dependency);
			systemState.Dependency = this.ScheduleSyncEntitiesToTransforms(ref systemState, systemState.Dependency);
			systemState.Dependency = this.__ScheduleViaJobChunkExtension_0(default(JobClearTemporaryData), this.__TypeHandle.__Pathfinding_ECS_JobClearTemporaryData_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, systemState.Dependency, ref systemState, false);
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x0007C600 File Offset: 0x0007A800
		private JobHandle ScheduleRepairPaths(ref SystemState systemState, JobHandle dependency)
		{
			dependency = this.jobRepairPathScheduler.ScheduleParallel(ref systemState, this.entityQueryPrepareMovement, dependency);
			return dependency;
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0007C618 File Offset: 0x0007A818
		private JobHandle ScheduleSyncEntitiesToTransforms(ref SystemState systemState, JobHandle dependency)
		{
			TransformAccessArray transformAccessArray;
			FollowerEntity[] array;
			int components = BatchedEvents.GetComponents<FollowerEntity>(BatchedEvents.Event.None, out transformAccessArray, out array);
			if (components > 0)
			{
				NativeArray<Entity> nativeArray = CollectionHelper.CreateNativeArray<Entity>(components, systemState.WorldUpdateAllocator, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < components; i++)
				{
					nativeArray[i] = array[i].entity;
				}
				dependency = new JobSyncEntitiesToTransforms
				{
					entities = nativeArray,
					syncPositionWithTransform = InternalCompilerInterface.GetComponentLookup<SyncPositionWithTransform>(ref this.__TypeHandle.__Pathfinding_ECS_SyncPositionWithTransform_RO_ComponentLookup, ref systemState),
					syncRotationWithTransform = InternalCompilerInterface.GetComponentLookup<SyncRotationWithTransform>(ref this.__TypeHandle.__Pathfinding_ECS_SyncRotationWithTransform_RO_ComponentLookup, ref systemState),
					orientationYAxisForward = InternalCompilerInterface.GetComponentLookup<OrientationYAxisForward>(ref this.__TypeHandle.__Pathfinding_ECS_OrientationYAxisForward_RO_ComponentLookup, ref systemState),
					entityPositions = InternalCompilerInterface.GetComponentLookup<LocalTransform>(ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref systemState),
					movementState = InternalCompilerInterface.GetComponentLookup<MovementState>(ref this.__TypeHandle.__Pathfinding_ECS_MovementState_RO_ComponentLookup, ref systemState)
				}.Schedule(transformAccessArray, dependency);
			}
			return dependency;
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x0007C700 File Offset: 0x0007A900
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_0(JobClearTemporaryData job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_JobClearTemporaryData_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobClearTemporaryData_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobClearTemporaryData_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_JobClearTemporaryData_WithDefaultQuery_JobEntityTypeHandle.Schedule(ref job, query, dependency);
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x0007C76C File Offset: 0x0007A96C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			entityQueryBuilder.Dispose();
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x0007C78D File Offset: 0x0007A98D
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x0007C7A2 File Offset: 0x0007A9A2
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
		{
			((LateSyncSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x0007C7BB File Offset: 0x0007A9BB
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((LateSyncSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x0007C7D4 File Offset: 0x0007A9D4
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
		{
			((LateSyncSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x0007C7ED File Offset: 0x0007A9ED
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((LateSyncSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000F34 RID: 3892
		private JobRepairPath.Scheduler jobRepairPathScheduler;

		// Token: 0x04000F35 RID: 3893
		private EntityQuery entityQueryPrepareMovement;

		// Token: 0x04000F36 RID: 3894
		private LateSyncSystem.TypeHandle __TypeHandle;

		// Token: 0x0200037C RID: 892
		private struct TypeHandle
		{
			// Token: 0x06001670 RID: 5744 RVA: 0x0007C808 File Offset: 0x0007AA08
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Pathfinding_ECS_SyncPositionWithTransform_RO_ComponentLookup = state.GetComponentLookup<SyncPositionWithTransform>(true);
				this.__Pathfinding_ECS_SyncRotationWithTransform_RO_ComponentLookup = state.GetComponentLookup<SyncRotationWithTransform>(true);
				this.__Pathfinding_ECS_OrientationYAxisForward_RO_ComponentLookup = state.GetComponentLookup<OrientationYAxisForward>(true);
				this.__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(true);
				this.__Pathfinding_ECS_MovementState_RO_ComponentLookup = state.GetComponentLookup<MovementState>(true);
				this.__Pathfinding_ECS_JobClearTemporaryData_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
			}

			// Token: 0x04000F37 RID: 3895
			[ReadOnly]
			public ComponentLookup<SyncPositionWithTransform> __Pathfinding_ECS_SyncPositionWithTransform_RO_ComponentLookup;

			// Token: 0x04000F38 RID: 3896
			[ReadOnly]
			public ComponentLookup<SyncRotationWithTransform> __Pathfinding_ECS_SyncRotationWithTransform_RO_ComponentLookup;

			// Token: 0x04000F39 RID: 3897
			[ReadOnly]
			public ComponentLookup<OrientationYAxisForward> __Pathfinding_ECS_OrientationYAxisForward_RO_ComponentLookup;

			// Token: 0x04000F3A RID: 3898
			[ReadOnly]
			public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

			// Token: 0x04000F3B RID: 3899
			[ReadOnly]
			public ComponentLookup<MovementState> __Pathfinding_ECS_MovementState_RO_ComponentLookup;

			// Token: 0x04000F3C RID: 3900
			public JobClearTemporaryData.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobClearTemporaryData_WithDefaultQuery_JobEntityTypeHandle;
		}
	}
}
