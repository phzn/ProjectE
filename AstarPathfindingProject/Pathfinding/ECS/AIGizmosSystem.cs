using System;
using System.Runtime.CompilerServices;
using Pathfinding.Drawing;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Pathfinding.ECS
{
	// Token: 0x02000356 RID: 854
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[CompilerGenerated]
	public struct AIGizmosSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x06001561 RID: 5473 RVA: 0x00079EA8 File Offset: 0x000780A8
		public void OnCreate(ref SystemState state)
		{
			this.jobRepairPathScheduler = new JobRepairPath.Scheduler(ref state);
			this.MovementStateTypeHandleRO = state.GetComponentTypeHandle<MovementState>(true);
			this.ResolvedMovementHandleRO = state.GetComponentTypeHandle<ResolvedMovement>(true);
			DrawingManager.Register(new AIGizmosSystem.DrawerCallback(state.World), typeof(FollowerEntity));
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00079EF5 File Offset: 0x000780F5
		public void OnUpdate(ref SystemState systemState)
		{
			if (AIGizmosSystem.manuallyTriggered)
			{
				this.DrawGizmos(ref systemState);
			}
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00079F08 File Offset: 0x00078108
		private void DrawGizmos(ref SystemState systemState)
		{
			EntityQuery _query_2063717491_ = this.__query_2063717491_0;
			if (_query_2063717491_.IsEmptyIgnoreFilter)
			{
				return;
			}
			this.jobRepairPathScheduler.Update(ref systemState);
			this.MovementStateTypeHandleRO.Update(ref systemState);
			this.ResolvedMovementHandleRO.Update(ref systemState);
			CommandBuilder builder = DrawingManager.GetBuilder(false);
			JobHandle jobHandle = new JobDrawFollowerGizmos
			{
				draw = builder,
				entityManagerHandle = this.jobRepairPathScheduler.entityManagerHandle,
				LocalTransformTypeHandleRO = this.jobRepairPathScheduler.LocalTransformTypeHandleRO,
				AgentCylinderShapeHandleRO = this.jobRepairPathScheduler.AgentCylinderShapeTypeHandleRO,
				MovementSettingsHandleRO = this.jobRepairPathScheduler.MovementSettingsTypeHandleRO,
				AgentMovementPlaneHandleRO = this.jobRepairPathScheduler.AgentMovementPlaneTypeHandleRO,
				ManagedStateHandleRW = this.jobRepairPathScheduler.ManagedStateTypeHandleRW,
				MovementStateHandleRO = this.MovementStateTypeHandleRO,
				ResolvedMovementHandleRO = this.ResolvedMovementHandleRO
			}.ScheduleParallel(_query_2063717491_, systemState.Dependency);
			JobHandle jobHandle2 = this.__ScheduleViaJobChunkExtension_0(new JobDrawFollowerGizmosBase
			{
				draw = builder
			}, this.__TypeHandle.__Pathfinding_ECS_JobDrawFollowerGizmosBase_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, systemState.Dependency, ref systemState, false);
			systemState.Dependency = JobHandle.CombineDependencies(jobHandle, jobHandle2);
			builder.DisposeAfter(systemState.Dependency, AllowedDelay.EndOfFrame);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0007A044 File Offset: 0x00078244
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_0(JobDrawFollowerGizmosBase job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_JobDrawFollowerGizmosBase_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobDrawFollowerGizmosBase_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobDrawFollowerGizmosBase_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_JobDrawFollowerGizmosBase_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0007A0B0 File Offset: 0x000782B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			this.__query_2063717491_0 = entityQueryBuilder.WithAll<LocalTransform, AgentCylinderShape, MovementSettings, AgentMovementPlane>().WithAll<ManagedState, MovementState, ResolvedMovement>().WithAll<SimulateMovement>()
				.Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0007A0FF File Offset: 0x000782FF
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0007A114 File Offset: 0x00078314
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
		{
			((AIGizmosSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0007A12D File Offset: 0x0007832D
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((AIGizmosSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0007A146 File Offset: 0x00078346
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((AIGizmosSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000EE4 RID: 3812
		private static bool manuallyTriggered;

		// Token: 0x04000EE5 RID: 3813
		private JobRepairPath.Scheduler jobRepairPathScheduler;

		// Token: 0x04000EE6 RID: 3814
		private ComponentTypeHandle<MovementState> MovementStateTypeHandleRO;

		// Token: 0x04000EE7 RID: 3815
		private ComponentTypeHandle<ResolvedMovement> ResolvedMovementHandleRO;

		// Token: 0x04000EE8 RID: 3816
		private AIGizmosSystem.TypeHandle __TypeHandle;

		// Token: 0x04000EE9 RID: 3817
		private EntityQuery __query_2063717491_0;

		// Token: 0x02000357 RID: 855
		private class DrawerCallback : IDrawGizmos
		{
			// Token: 0x0600156A RID: 5482 RVA: 0x0007A15F File Offset: 0x0007835F
			public DrawerCallback(World world)
			{
				this.world = world;
			}

			// Token: 0x0600156B RID: 5483 RVA: 0x0007A170 File Offset: 0x00078370
			public void DrawGizmos()
			{
				if (!this.world.IsCreated)
				{
					return;
				}
				SystemHandle existingSystem = this.world.GetExistingSystem<AIGizmosSystem>();
				if (existingSystem != SystemHandle.Null)
				{
					AIGizmosSystem.manuallyTriggered = true;
					try
					{
						existingSystem.Update(this.world.Unmanaged);
					}
					finally
					{
						AIGizmosSystem.manuallyTriggered = false;
					}
				}
			}

			// Token: 0x1700031E RID: 798
			// (get) Token: 0x0600156C RID: 5484 RVA: 0x0007A1D8 File Offset: 0x000783D8
			public bool Exists
			{
				get
				{
					return this.world.IsCreated && this.world.GetExistingSystem<AIGizmosSystem>() != SystemHandle.Null;
				}
			}

			// Token: 0x04000EEA RID: 3818
			private World world;
		}

		// Token: 0x02000358 RID: 856
		private struct TypeHandle
		{
			// Token: 0x0600156D RID: 5485 RVA: 0x0007A1FE File Offset: 0x000783FE
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Pathfinding_ECS_JobDrawFollowerGizmosBase_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
			}

			// Token: 0x04000EEB RID: 3819
			public JobDrawFollowerGizmosBase.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobDrawFollowerGizmosBase_WithDefaultQuery_JobEntityTypeHandle;
		}
	}
}
