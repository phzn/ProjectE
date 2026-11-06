using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Pathfinding.ECS
{
	// Token: 0x0200038A RID: 906
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[UpdateAfter(typeof(AIMoveSystem))]
	[RequireMatchingQueriesForUpdate]
	[CompilerGenerated]
	public struct MovementStatisticsSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x060016DA RID: 5850 RVA: 0x0007DA30 File Offset: 0x0007BC30
		public void OnUpdate(ref SystemState state)
		{
			state.Dependency = this.__ScheduleViaJobChunkExtension_0(new JobUpdateMovementStatistics
			{
				dt = AIMovementSystemGroup.TimeScaledRateManager.CheapStepDeltaTime
			}, this.__TypeHandle.__Pathfinding_ECS_JobUpdateMovementStatistics_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, false);
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x0007DA78 File Offset: 0x0007BC78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_0(JobUpdateMovementStatistics job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_JobUpdateMovementStatistics_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobUpdateMovementStatistics_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobUpdateMovementStatistics_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_JobUpdateMovementStatistics_WithDefaultQuery_JobEntityTypeHandle.Schedule(ref job, query, dependency);
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x0007DAE4 File Offset: 0x0007BCE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			entityQueryBuilder.Dispose();
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x0007DB05 File Offset: 0x0007BD05
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x0007DB1A File Offset: 0x0007BD1A
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((MovementStatisticsSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x0007DB33 File Offset: 0x0007BD33
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((MovementStatisticsSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000F58 RID: 3928
		private MovementStatisticsSystem.TypeHandle __TypeHandle;

		// Token: 0x0200038B RID: 907
		private struct TypeHandle
		{
			// Token: 0x060016E0 RID: 5856 RVA: 0x0007DB4C File Offset: 0x0007BD4C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Pathfinding_ECS_JobUpdateMovementStatistics_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
			}

			// Token: 0x04000F59 RID: 3929
			public JobUpdateMovementStatistics.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobUpdateMovementStatistics_WithDefaultQuery_JobEntityTypeHandle;
		}
	}
}
