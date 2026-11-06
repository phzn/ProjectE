using System;
using System.Runtime.CompilerServices;
using Pathfinding.ECS.RVO;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Profiling;
using Unity.Transforms;
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x0200035B RID: 859
	[BurstCompile]
	[UpdateAfter(typeof(FollowerControlSystem))]
	[UpdateAfter(typeof(RVOSystem))]
	[UpdateAfter(typeof(FallbackResolveMovementSystem))]
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[RequireMatchingQueriesForUpdate]
	[CompilerGenerated]
	public struct AIMoveSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x0600157F RID: 5503 RVA: 0x0007A6BC File Offset: 0x000788BC
		public void OnCreate(ref SystemState state)
		{
			this.entityQueryWithGravity = state.GetEntityQuery(new ComponentType[]
			{
				ComponentType.ReadWrite<LocalTransform>(),
				ComponentType.ReadOnly<AgentCylinderShape>(),
				ComponentType.ReadWrite<AgentMovementPlane>(),
				ComponentType.ReadWrite<MovementState>(),
				ComponentType.ReadOnly<MovementSettings>(),
				ComponentType.ReadWrite<ResolvedMovement>(),
				ComponentType.ReadWrite<MovementStatistics>(),
				ComponentType.ReadOnly<MovementControl>(),
				ComponentType.ReadWrite<GravityState>(),
				ComponentType.ReadOnly<PhysicsSceneRef>(),
				ComponentType.Exclude<OrientationYAxisForward>(),
				ComponentType.Exclude<AgentOffMeshLinkMovementDisabled>(),
				ComponentType.ReadOnly<AgentMovementPlaneSource>(),
				ComponentType.ReadOnly<SimulateMovement>(),
				ComponentType.ReadOnly<SimulateMovementFinalize>()
			});
			this.entityQueryMovementOverride = state.GetEntityQuery(new ComponentType[]
			{
				ComponentType.ReadWrite<ManagedMovementOverrideBeforeMovement>(),
				ComponentType.ReadWrite<LocalTransform>(),
				ComponentType.ReadWrite<AgentCylinderShape>(),
				ComponentType.ReadWrite<AgentMovementPlane>(),
				ComponentType.ReadWrite<DestinationPoint>(),
				ComponentType.ReadWrite<MovementState>(),
				ComponentType.ReadWrite<MovementStatistics>(),
				ComponentType.ReadWrite<ManagedState>(),
				ComponentType.ReadWrite<MovementSettings>(),
				ComponentType.ReadWrite<ResolvedMovement>(),
				ComponentType.ReadWrite<MovementControl>(),
				ComponentType.Exclude<AgentOffMeshLinkTraversal>(),
				ComponentType.ReadOnly<SimulateMovement>(),
				ComponentType.ReadOnly<SimulateMovementControl>()
			});
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0007A858 File Offset: 0x00078A58
		public void OnUpdate(ref SystemState systemState)
		{
			float cheapStepDeltaTime = AIMovementSystemGroup.TimeScaledRateManager.CheapStepDeltaTime;
			systemState.Dependency = this.__ScheduleViaJobChunkExtension_0(new JobAlignAgentWithMovementDirection
			{
				dt = cheapStepDeltaTime
			}, this.__TypeHandle.__Pathfinding_ECS_JobAlignAgentWithMovementDirection_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, systemState.Dependency, ref systemState, false);
			this.RunMovementOverrideBeforeMovement(ref systemState, cheapStepDeltaTime);
			systemState.Dependency = this.__ScheduleViaJobChunkExtension_1(new JobMoveAgent
			{
				dt = cheapStepDeltaTime
			}, this.__TypeHandle.__Pathfinding_ECS_JobMoveAgent_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, systemState.Dependency, ref systemState, false);
			this.ScheduleApplyGravity(ref systemState, cheapStepDeltaTime);
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0007A8E8 File Offset: 0x00078AE8
		private void ScheduleApplyGravity(ref SystemState systemState, float dt)
		{
			int num = this.entityQueryWithGravity.CalculateEntityCount();
			NativeArray<RaycastCommand> nativeArray = CollectionHelper.CreateNativeArray<RaycastCommand>(num, systemState.WorldUpdateAllocator, NativeArrayOptions.UninitializedMemory);
			NativeArray<RaycastHit> nativeArray2 = CollectionHelper.CreateNativeArray<RaycastHit>(num, systemState.WorldUpdateAllocator, NativeArrayOptions.UninitializedMemory);
			systemState.Dependency = this.__ScheduleViaJobChunkExtension_2(new JobPrepareAgentRaycasts
			{
				raycastQueryParameters = new QueryParameters(-1, false, QueryTriggerInteraction.Ignore, false),
				raycastCommands = nativeArray,
				dt = dt,
				gravity = Physics.gravity.y
			}, this.entityQueryWithGravity, systemState.Dependency, ref systemState, true);
			JobHandle jobHandle = RaycastCommand.ScheduleBatch(nativeArray, nativeArray2, 32, 1, systemState.Dependency);
			systemState.Dependency = this.__ScheduleViaJobChunkExtension_3(new JobApplyGravity
			{
				raycastHits = nativeArray2,
				raycastCommands = nativeArray,
				dt = dt
			}, this.entityQueryWithGravity, JobHandle.CombineDependencies(systemState.Dependency, jobHandle), ref systemState, true);
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0007A9CC File Offset: 0x00078BCC
		private void RunMovementOverrideBeforeMovement(ref SystemState systemState, float dt)
		{
			if (!this.entityQueryMovementOverride.IsEmptyIgnoreFilter)
			{
				systemState.CompleteDependency();
				this.__ScheduleViaJobChunkExtension_4(new JobManagedMovementOverrideBeforeMovement
				{
					dt = dt
				}, this.entityQueryMovementOverride, systemState.Dependency, ref systemState, true);
			}
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0007AA14 File Offset: 0x00078C14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_0(JobAlignAgentWithMovementDirection job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_JobAlignAgentWithMovementDirection_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobAlignAgentWithMovementDirection_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobAlignAgentWithMovementDirection_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_JobAlignAgentWithMovementDirection_WithDefaultQuery_JobEntityTypeHandle.Schedule(ref job, query, dependency);
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0007AA80 File Offset: 0x00078C80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_1(JobMoveAgent job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_JobMoveAgent_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobMoveAgent_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobMoveAgent_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_JobMoveAgent_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0007AAEC File Offset: 0x00078CEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_2(JobPrepareAgentRaycasts job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_JobPrepareAgentRaycasts_WithoutDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobPrepareAgentRaycasts_WithoutDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobPrepareAgentRaycasts_WithoutDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_JobPrepareAgentRaycasts_WithoutDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0007AB58 File Offset: 0x00078D58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_3(JobApplyGravity job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_JobApplyGravity_WithoutDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobApplyGravity_WithoutDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobApplyGravity_WithoutDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_JobApplyGravity_WithoutDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0007ABC4 File Offset: 0x00078DC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __ScheduleViaJobChunkExtension_4(JobManagedMovementOverrideBeforeMovement job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideBeforeMovement_WithoutDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideBeforeMovement_WithoutDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideBeforeMovement_WithoutDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideBeforeMovement_WithoutDefaultQuery_JobEntityTypeHandle.Run(ref job, query);
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0007AC2C File Offset: 0x00078E2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			entityQueryBuilder.Dispose();
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0007AC4D File Offset: 0x00078E4D
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0007AC73 File Offset: 0x00078E73
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
		{
			((AIMoveSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0007AC8C File Offset: 0x00078E8C
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((AIMoveSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0007ACA5 File Offset: 0x00078EA5
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((AIMoveSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000EFB RID: 3835
		private EntityQuery entityQueryWithGravity;

		// Token: 0x04000EFC RID: 3836
		private EntityQuery entityQueryMovementOverride;

		// Token: 0x04000EFD RID: 3837
		private static readonly ProfilerMarker MarkerMovementOverride = new ProfilerMarker("MovementOverrideBeforeMovement");

		// Token: 0x04000EFE RID: 3838
		private AIMoveSystem.TypeHandle __TypeHandle;

		// Token: 0x0200035C RID: 860
		private struct TypeHandle
		{
			// Token: 0x0600158E RID: 5518 RVA: 0x0007ACC0 File Offset: 0x00078EC0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Pathfinding_ECS_JobAlignAgentWithMovementDirection_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
				this.__Pathfinding_ECS_JobMoveAgent_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
				this.__Pathfinding_ECS_JobPrepareAgentRaycasts_WithoutDefaultQuery_JobEntityTypeHandle.Init(ref state, false);
				this.__Pathfinding_ECS_JobApplyGravity_WithoutDefaultQuery_JobEntityTypeHandle.Init(ref state, false);
				this.__Pathfinding_ECS_JobManagedMovementOverrideBeforeMovement_WithoutDefaultQuery_JobEntityTypeHandle.Init(ref state, false);
			}

			// Token: 0x04000EFF RID: 3839
			public JobAlignAgentWithMovementDirection.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobAlignAgentWithMovementDirection_WithDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000F00 RID: 3840
			public JobMoveAgent.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobMoveAgent_WithDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000F01 RID: 3841
			public JobPrepareAgentRaycasts.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobPrepareAgentRaycasts_WithoutDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000F02 RID: 3842
			public JobApplyGravity.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobApplyGravity_WithoutDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000F03 RID: 3843
			public JobManagedMovementOverrideBeforeMovement.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobManagedMovementOverrideBeforeMovement_WithoutDefaultQuery_JobEntityTypeHandle;
		}
	}
}
