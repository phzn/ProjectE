using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding.ECS
{
	// Token: 0x0200031B RID: 795
	[BurstCompile]
	[CompilerGenerated]
	public struct JobClearTemporaryData : IJobEntity, IJobChunk
	{
		// Token: 0x06001379 RID: 4985 RVA: 0x00074A2F File Offset: 0x00072C2F
		public void Execute(ref MovementState movementState, ref MovementControl movementControl)
		{
			movementState.hierarchicalNodeIndex = -1;
			movementControl.hierarchicalNodeIndex = -1;
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00074A40 File Offset: 0x00072C40
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementState>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle);
			IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementControl>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref MovementState ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr, i);
					ref MovementControl ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr2, i);
					this.Execute(ref ptr, ref ptr2);
					num++;
				}
				return;
			}
			if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
			{
				int j = 0;
				int num2 = 0;
				while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num2, out j, out num2))
				{
					while (j < num2)
					{
						ref MovementState ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr, j);
						ref MovementControl ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr2, j);
						this.Execute(ref ptr3, ref ptr4);
						j++;
						num++;
					}
				}
				return;
			}
			ulong num3 = chunkEnabledMask.ULong0;
			int num4 = math.min(64, count);
			for (int k = 0; k < num4; k++)
			{
				if ((num3 & 1UL) != 0UL)
				{
					ref MovementState ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr, k);
					ref MovementControl ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr2, k);
					this.Execute(ref ptr5, ref ptr6);
					num++;
				}
				num3 >>= 1;
			}
			num3 = chunkEnabledMask.ULong1;
			for (int l = 64; l < count; l++)
			{
				if ((num3 & 1UL) != 0UL)
				{
					ref MovementState ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr, l);
					ref MovementControl ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr2, l);
					this.Execute(ref ptr7, ref ptr8);
					num++;
				}
				num3 >>= 1;
			}
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00074BD8 File Offset: 0x00072DD8
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00074BF8 File Offset: 0x00072DF8
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00074BF8 File Offset: 0x00072DF8
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00074BF8 File Offset: 0x00072DF8
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00074BF8 File Offset: 0x00072DF8
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00074BF8 File Offset: 0x00072DF8
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00074BF8 File Offset: 0x00072DF8
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x00074BF8 File Offset: 0x00072DF8
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x00074BF8 File Offset: 0x00072DF8
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00074BF8 File Offset: 0x00072DF8
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00074BF8 File Offset: 0x00072DF8
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00074BEF File Offset: 0x00072DEF
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00074C00 File Offset: 0x00072E00
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000E29 RID: 3625
		private JobClearTemporaryData.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x0200031C RID: 796
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x06001393 RID: 5011 RVA: 0x00074C10 File Offset: 0x00072E10
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithAllRW<MovementState>().WithAllRW<MovementControl>().Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x06001394 RID: 5012 RVA: 0x00074C57 File Offset: 0x00072E57
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x06001395 RID: 5013 RVA: 0x00074C6F File Offset: 0x00072E6F
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobClearTemporaryData job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				(ref job).RunByRef<JobClearTemporaryData>(query);
			}

			// Token: 0x06001396 RID: 5014 RVA: 0x00074C84 File Offset: 0x00072E84
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobClearTemporaryData job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobClearTemporaryData>(query, dependency);
			}

			// Token: 0x06001397 RID: 5015 RVA: 0x00074C9A File Offset: 0x00072E9A
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobClearTemporaryData job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleParallelByRef<JobClearTemporaryData>(query, dependency);
			}

			// Token: 0x06001398 RID: 5016 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobClearTemporaryData job, EntityQuery query, ref SystemState state)
			{
			}

			// Token: 0x06001399 RID: 5017 RVA: 0x000743B8 File Offset: 0x000725B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobClearTemporaryData job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			// Token: 0x0600139A RID: 5018 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobClearTemporaryData job, EntityManager entityManager)
			{
			}

			// Token: 0x04000E2A RID: 3626
			public JobClearTemporaryData.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000E2B RID: 3627
			public EntityQuery DefaultQuery;

			// Token: 0x0200031D RID: 797
			public struct TypeHandle
			{
				// Token: 0x0600139B RID: 5019 RVA: 0x00074CB0 File Offset: 0x00072EB0
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementState>(false);
					this.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementControl>(false);
				}

				// Token: 0x0600139C RID: 5020 RVA: 0x00074CCC File Offset: 0x00072ECC
				public void Update(ref SystemState state)
				{
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000E2C RID: 3628
				public ComponentTypeHandle<MovementState> __Pathfinding_ECS_MovementState_RW_ComponentTypeHandle;

				// Token: 0x04000E2D RID: 3629
				public ComponentTypeHandle<MovementControl> __Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle;
			}
		}

		// Token: 0x0200031E RID: 798
		public struct InternalCompiler
		{
			// Token: 0x0600139D RID: 5021 RVA: 0x00003788 File Offset: 0x00001988
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void CheckForErrors(int scheduleType)
			{
			}
		}
	}
}
