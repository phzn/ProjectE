using System;
using System.Runtime.CompilerServices;
using Pathfinding.Drawing;
using Pathfinding.Sync;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Profiling;
using Unity.Transforms;

namespace Pathfinding.ECS
{
	// Token: 0x02000377 RID: 887
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[BurstCompile]
	[CompilerGenerated]
	public struct FollowerControlSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x0600164F RID: 5711 RVA: 0x0007BEF0 File Offset: 0x0007A0F0
		public void OnCreate(ref SystemState state)
		{
			this.redrawScope = DrawingManager.GetRedrawScope(null);
			this.entityQueryControlManaged = state.GetEntityQuery(new ComponentType[]
			{
				ComponentType.ReadWrite<ManagedMovementOverrideBeforeControl>(),
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
			this.entityQueryControlManaged2 = state.GetEntityQuery(new ComponentType[]
			{
				ComponentType.ReadWrite<ManagedMovementOverrideAfterControl>(),
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

		// Token: 0x06001650 RID: 5712 RVA: 0x0007C089 File Offset: 0x0007A289
		public void OnDestroy(ref SystemState state)
		{
			this.redrawScope.Dispose();
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x0007C096 File Offset: 0x0007A296
		public void OnUpdate(ref SystemState systemState)
		{
			if (AstarPath.active == null)
			{
				return;
			}
			if (this.__query_248260819_0.IsEmptyIgnoreFilter)
			{
				return;
			}
			this.ProcessControlLoop(ref systemState, AIMovementSystemGroup.TimeScaledRateManager.CheapStepDeltaTime);
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x0007C0C0 File Offset: 0x0007A2C0
		private void ProcessControlLoop(ref SystemState systemState, float dt)
		{
			if (!this.entityQueryControlManaged.IsEmpty)
			{
				systemState.Dependency.Complete();
				this.__ScheduleViaJobChunkExtension_0(new JobManagedMovementOverrideBeforeControl
				{
					dt = dt
				}, this.entityQueryControlManaged, systemState.Dependency, ref systemState, true);
			}
			this.redrawScope.Rewind();
			CommandBuilder builder = DrawingManager.GetBuilder(this.redrawScope, false);
			RWLock.CombinedReadLockAsync combinedReadLockAsync;
			NavmeshEdges.NavmeshBorderData navmeshBorderData = AstarPath.active.GetNavmeshBorderData(out combinedReadLockAsync);
			systemState.Dependency = this.__ScheduleViaJobChunkExtension_1(new JobControl
			{
				navmeshEdgeData = navmeshBorderData,
				draw = builder,
				dt = dt
			}, this.__TypeHandle.__Pathfinding_ECS_JobControl_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, JobHandle.CombineDependencies(systemState.Dependency, combinedReadLockAsync.dependency), ref systemState, false);
			combinedReadLockAsync.UnlockAfter(systemState.Dependency);
			builder.DisposeAfter(systemState.Dependency, AllowedDelay.EndOfFrame);
			if (!this.entityQueryControlManaged2.IsEmpty)
			{
				systemState.Dependency.Complete();
				this.__ScheduleViaJobChunkExtension_2(new JobManagedMovementOverrideAfterControl
				{
					dt = dt
				}, this.entityQueryControlManaged2, systemState.Dependency, ref systemState, true);
			}
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x0007C1E0 File Offset: 0x0007A3E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __ScheduleViaJobChunkExtension_0(JobManagedMovementOverrideBeforeControl job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideBeforeControl_WithoutDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideBeforeControl_WithoutDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideBeforeControl_WithoutDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideBeforeControl_WithoutDefaultQuery_JobEntityTypeHandle.Run(ref job, query);
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x0007C248 File Offset: 0x0007A448
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_1(JobControl job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_JobControl_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobControl_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobControl_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_JobControl_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0007C2B4 File Offset: 0x0007A4B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __ScheduleViaJobChunkExtension_2(JobManagedMovementOverrideAfterControl job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideAfterControl_WithoutDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideAfterControl_WithoutDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideAfterControl_WithoutDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedMovementOverrideAfterControl_WithoutDefaultQuery_JobEntityTypeHandle.Run(ref job, query);
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x0007C31C File Offset: 0x0007A51C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			this.__query_248260819_0 = entityQueryBuilder.WithAll<MovementState>().Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x0007C35B File Offset: 0x0007A55B
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0007C390 File Offset: 0x0007A590
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
		{
			((FollowerControlSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x0007C3A9 File Offset: 0x0007A5A9
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((FollowerControlSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0007C3C2 File Offset: 0x0007A5C2
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
		{
			((FollowerControlSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x0007C3DB File Offset: 0x0007A5DB
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((FollowerControlSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000F28 RID: 3880
		private EntityQuery entityQueryControlManaged;

		// Token: 0x04000F29 RID: 3881
		private EntityQuery entityQueryControlManaged2;

		// Token: 0x04000F2A RID: 3882
		private RedrawScope redrawScope;

		// Token: 0x04000F2B RID: 3883
		private static readonly ProfilerMarker MarkerMovementOverrideBeforeControl = new ProfilerMarker("MovementOverrideBeforeControl");

		// Token: 0x04000F2C RID: 3884
		private static readonly ProfilerMarker MarkerMovementOverrideAfterControl = new ProfilerMarker("MovementOverrideAfterControl");

		// Token: 0x04000F2D RID: 3885
		private FollowerControlSystem.TypeHandle __TypeHandle;

		// Token: 0x04000F2E RID: 3886
		private EntityQuery __query_248260819_0;

		// Token: 0x02000378 RID: 888
		private struct TypeHandle
		{
			// Token: 0x0600165D RID: 5725 RVA: 0x0007C3F4 File Offset: 0x0007A5F4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Pathfinding_ECS_JobManagedMovementOverrideBeforeControl_WithoutDefaultQuery_JobEntityTypeHandle.Init(ref state, false);
				this.__Pathfinding_ECS_JobControl_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
				this.__Pathfinding_ECS_JobManagedMovementOverrideAfterControl_WithoutDefaultQuery_JobEntityTypeHandle.Init(ref state, false);
			}

			// Token: 0x04000F2F RID: 3887
			public JobManagedMovementOverrideBeforeControl.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobManagedMovementOverrideBeforeControl_WithoutDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000F30 RID: 3888
			public JobControl.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobControl_WithDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000F31 RID: 3889
			public JobManagedMovementOverrideAfterControl.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobManagedMovementOverrideAfterControl_WithoutDefaultQuery_JobEntityTypeHandle;
		}
	}
}
