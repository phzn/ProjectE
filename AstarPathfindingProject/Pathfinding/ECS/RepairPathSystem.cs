using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Pathfinding.ECS
{
	// Token: 0x02000393 RID: 915
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[UpdateBefore(typeof(FollowerControlSystem))]
	[BurstCompile]
	[CompilerGenerated]
	public struct RepairPathSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x060016FA RID: 5882 RVA: 0x0007DF54 File Offset: 0x0007C154
		public void OnCreate(ref SystemState state)
		{
			this.jobRepairPathScheduler = new JobRepairPath.Scheduler(ref state);
			this.entityQueryPrepare = this.jobRepairPathScheduler.GetEntityQuery(Allocator.Temp).WithAll<SimulateMovement, SimulateMovementRepair>().Build(ref state);
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x0007DF90 File Offset: 0x0007C190
		public void OnDestroy(ref SystemState state)
		{
			this.jobRepairPathScheduler.Dispose();
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x0007DF9D File Offset: 0x0007C19D
		public void OnUpdate(ref SystemState systemState)
		{
			if (AstarPath.active == null)
			{
				return;
			}
			if (this.__query_943785559_0.IsEmptyIgnoreFilter)
			{
				return;
			}
			systemState.Dependency = this.jobRepairPathScheduler.ScheduleParallel(ref systemState, this.entityQueryPrepare, systemState.Dependency);
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x0007DFD9 File Offset: 0x0007C1D9
		[Obsolete("Use TraverseOffMeshLinkSystem.NextLinkToTraverse instead")]
		public static OffMeshLinks.OffMeshLinkTracer NextLinkToTraverse(ManagedState state)
		{
			return TraverseOffMeshLinkSystem.NextLinkToTraverse(state);
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x0007DFE1 File Offset: 0x0007C1E1
		[Obsolete("Use TraverseOffMeshLinkSystem.ResolveOffMeshLinkHandler instead")]
		public static IOffMeshLinkHandler ResolveOffMeshLinkHandler(ManagedSettings settings, AgentOffMeshLinkTraversalContext ctx)
		{
			return TraverseOffMeshLinkSystem.ResolveOffMeshLinkHandler(settings, ctx);
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x0007DFEC File Offset: 0x0007C1EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			this.__query_943785559_0 = entityQueryBuilder.WithAll<MovementState>().Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x0007E02B File Offset: 0x0007C22B
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x0007E040 File Offset: 0x0007C240
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
		{
			((RepairPathSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x0007E059 File Offset: 0x0007C259
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((RepairPathSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x0007E072 File Offset: 0x0007C272
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
		{
			((RepairPathSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x0007E08B File Offset: 0x0007C28B
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((RepairPathSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000F6A RID: 3946
		private EntityQuery entityQueryPrepare;

		// Token: 0x04000F6B RID: 3947
		private JobRepairPath.Scheduler jobRepairPathScheduler;

		// Token: 0x04000F6C RID: 3948
		private RepairPathSystem.TypeHandle __TypeHandle;

		// Token: 0x04000F6D RID: 3949
		private EntityQuery __query_943785559_0;

		// Token: 0x02000394 RID: 916
		private struct TypeHandle
		{
			// Token: 0x06001705 RID: 5893 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
			}
		}
	}
}
