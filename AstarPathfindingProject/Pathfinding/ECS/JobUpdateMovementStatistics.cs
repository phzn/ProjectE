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
using Unity.Transforms;

namespace Pathfinding.ECS
{
	// Token: 0x02000352 RID: 850
	[BurstCompile]
	[WithAll(new Type[] { typeof(SimulateMovement) })]
	[CompilerGenerated]
	public struct JobUpdateMovementStatistics : IJobEntity, IJobChunk
	{
		// Token: 0x0600153B RID: 5435 RVA: 0x00079BB6 File Offset: 0x00077DB6
		private static void UpdateStatistics(in LocalTransform transform, ref MovementStatistics movementStatistics, float dt)
		{
			if (dt > 1E-06f)
			{
				movementStatistics.estimatedVelocity = (transform.Position - movementStatistics.lastPosition) / dt;
			}
			movementStatistics.lastPosition = transform.Position;
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x00079BE9 File Offset: 0x00077DE9
		public void Execute(in LocalTransform transform, ref MovementStatistics movementStatistics)
		{
			JobUpdateMovementStatistics.UpdateStatistics(in transform, ref movementStatistics, this.dt);
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x00079BF8 File Offset: 0x00077DF8
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
			IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementStatistics>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, i);
					ref MovementStatistics ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementStatistics>(intPtr2, i);
					this.Execute(in ptr, ref ptr2);
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
						ref LocalTransform ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, j);
						ref MovementStatistics ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementStatistics>(intPtr2, j);
						this.Execute(in ptr3, ref ptr4);
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
					ref LocalTransform ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, k);
					ref MovementStatistics ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementStatistics>(intPtr2, k);
					this.Execute(in ptr5, ref ptr6);
					num++;
				}
				num3 >>= 1;
			}
			num3 = chunkEnabledMask.ULong1;
			for (int l = 64; l < count; l++)
			{
				if ((num3 & 1UL) != 0UL)
				{
					ref LocalTransform ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, l);
					ref MovementStatistics ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementStatistics>(intPtr2, l);
					this.Execute(in ptr7, ref ptr8);
					num++;
				}
				num3 >>= 1;
			}
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x00079D90 File Offset: 0x00077F90
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00079DB0 File Offset: 0x00077FB0
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00079DB0 File Offset: 0x00077FB0
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00079DB0 File Offset: 0x00077FB0
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x00079DB0 File Offset: 0x00077FB0
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x00079DB0 File Offset: 0x00077FB0
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00079DB0 File Offset: 0x00077FB0
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x00079DB0 File Offset: 0x00077FB0
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x00079DB0 File Offset: 0x00077FB0
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x00079DB0 File Offset: 0x00077FB0
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x00079DB0 File Offset: 0x00077FB0
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x00079DA7 File Offset: 0x00077FA7
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00079DB8 File Offset: 0x00077FB8
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000EDE RID: 3806
		public float dt;

		// Token: 0x04000EDF RID: 3807
		private JobUpdateMovementStatistics.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x02000353 RID: 851
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x06001556 RID: 5462 RVA: 0x00079DC8 File Offset: 0x00077FC8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithAll<LocalTransform>().WithAll<SimulateMovement>().WithAllRW<MovementStatistics>()
					.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x06001557 RID: 5463 RVA: 0x00079E17 File Offset: 0x00078017
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x06001558 RID: 5464 RVA: 0x00079E2F File Offset: 0x0007802F
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobUpdateMovementStatistics job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				(ref job).RunByRef<JobUpdateMovementStatistics>(query);
			}

			// Token: 0x06001559 RID: 5465 RVA: 0x00079E44 File Offset: 0x00078044
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobUpdateMovementStatistics job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobUpdateMovementStatistics>(query, dependency);
			}

			// Token: 0x0600155A RID: 5466 RVA: 0x00079E5A File Offset: 0x0007805A
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobUpdateMovementStatistics job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleParallelByRef<JobUpdateMovementStatistics>(query, dependency);
			}

			// Token: 0x0600155B RID: 5467 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobUpdateMovementStatistics job, EntityQuery query, ref SystemState state)
			{
			}

			// Token: 0x0600155C RID: 5468 RVA: 0x000743B8 File Offset: 0x000725B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobUpdateMovementStatistics job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			// Token: 0x0600155D RID: 5469 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobUpdateMovementStatistics job, EntityManager entityManager)
			{
			}

			// Token: 0x04000EE0 RID: 3808
			public JobUpdateMovementStatistics.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000EE1 RID: 3809
			public EntityQuery DefaultQuery;

			// Token: 0x02000354 RID: 852
			public struct TypeHandle
			{
				// Token: 0x0600155E RID: 5470 RVA: 0x00079E70 File Offset: 0x00078070
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(true);
					this.__Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementStatistics>(false);
				}

				// Token: 0x0600155F RID: 5471 RVA: 0x00079E8C File Offset: 0x0007808C
				public void Update(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000EE2 RID: 3810
				[ReadOnly]
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

				// Token: 0x04000EE3 RID: 3811
				public ComponentTypeHandle<MovementStatistics> __Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle;
			}
		}

		// Token: 0x02000355 RID: 853
		public struct InternalCompiler
		{
			// Token: 0x06001560 RID: 5472 RVA: 0x00003788 File Offset: 0x00001988
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void CheckForErrors(int scheduleType)
			{
			}
		}
	}
}
