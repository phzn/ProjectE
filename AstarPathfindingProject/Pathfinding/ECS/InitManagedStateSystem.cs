using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;

namespace Pathfinding.ECS
{
	// Token: 0x02000379 RID: 889
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[UpdateBefore(typeof(MovementPlaneFromGraphSystem))]
	[UpdateBefore(typeof(SchedulePathSearchSystem))]
	[RequireMatchingQueriesForUpdate]
	[CompilerGenerated]
	public struct InitManagedStateSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x0600165E RID: 5726 RVA: 0x0007C420 File Offset: 0x0007A620
		public void OnUpdate(ref SystemState state)
		{
			EntityQuery _query_1374900497_ = this.__query_1374900497_0;
			NativeArray<Entity> nativeArray = _query_1374900497_.ToEntityArray(Allocator.Temp);
			state.EntityManager.AddComponent<ManagedState>(nativeArray);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				state.EntityManager.SetComponentData(nativeArray[i], new ManagedState
				{
					pathTracer = new PathTracer(Allocator.Persistent)
				});
			}
			for (int j = 0; j < nativeArray.Length; j++)
			{
				FollowerEntityProxy followerEntityProxy = new FollowerEntityProxy(state.World, nativeArray[j]);
				followerEntityProxy.Teleport(followerEntityProxy.position, false);
			}
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0007C4C4 File Offset: 0x0007A6C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			this.__query_1374900497_0 = entityQueryBuilder.WithAll<ManagedSettings>().WithNone<ManagedState>().Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x0007C50B File Offset: 0x0007A70B
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x0007C520 File Offset: 0x0007A720
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((InitManagedStateSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x0007C539 File Offset: 0x0007A739
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((InitManagedStateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000F32 RID: 3890
		private InitManagedStateSystem.TypeHandle __TypeHandle;

		// Token: 0x04000F33 RID: 3891
		private EntityQuery __query_1374900497_0;

		// Token: 0x0200037A RID: 890
		private struct TypeHandle
		{
			// Token: 0x06001663 RID: 5731 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
			}
		}
	}
}
