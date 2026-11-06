using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Pathfinding.ECS.RVO;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding.ECS
{
	// Token: 0x0200036D RID: 877
	[BurstCompile]
	[UpdateAfter(typeof(FollowerControlSystem))]
	[UpdateAfter(typeof(RVOSystem))]
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[RequireMatchingQueriesForUpdate]
	[CompilerGenerated]
	public struct FallbackResolveMovementSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x060015FD RID: 5629 RVA: 0x0007B768 File Offset: 0x00079968
		public void OnUpdate(ref SystemState systemState)
		{
			systemState.Dependency = this.__ScheduleViaJobChunkExtension_0(default(FallbackResolveMovementSystem.CopyJob), this.__TypeHandle.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, systemState.Dependency, ref systemState, false);
			systemState.Dependency = this.__ScheduleViaJobChunkExtension_1(default(FallbackResolveMovementSystem.CopyRotationJob), this.__TypeHandle.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyRotationJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, systemState.Dependency, ref systemState, false);
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x0007B7D0 File Offset: 0x000799D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_0(FallbackResolveMovementSystem.CopyJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyJob_WithDefaultQuery_JobEntityTypeHandle.Schedule(ref job, query, dependency);
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0007B83C File Offset: 0x00079A3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_1(FallbackResolveMovementSystem.CopyRotationJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyRotationJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyRotationJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyRotationJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyRotationJob_WithDefaultQuery_JobEntityTypeHandle.Schedule(ref job, query, dependency);
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0007B8A8 File Offset: 0x00079AA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			entityQueryBuilder.Dispose();
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0007B8C9 File Offset: 0x00079AC9
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0007B8DE File Offset: 0x00079ADE
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((FallbackResolveMovementSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0007B8F7 File Offset: 0x00079AF7
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((FallbackResolveMovementSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000F1B RID: 3867
		private FallbackResolveMovementSystem.TypeHandle __TypeHandle;

		// Token: 0x0200036E RID: 878
		[BurstCompile]
		[WithAll(new Type[] { typeof(SimulateMovement) })]
		[WithOptions(EntityQueryOptions.FilterWriteGroup)]
		[CompilerGenerated]
		public struct CopyJob : IJobEntity, IJobChunk
		{
			// Token: 0x06001604 RID: 5636 RVA: 0x0007B910 File Offset: 0x00079B10
			public void Execute(in MovementControl control, ref ResolvedMovement resolved)
			{
				resolved.targetPoint = control.targetPoint;
				resolved.speed = control.speed;
				resolved.turningRadiusMultiplier = 1f;
			}

			// Token: 0x06001605 RID: 5637 RVA: 0x0007B938 File Offset: 0x00079B38
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementControl>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<ResolvedMovement>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						ref MovementControl ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr, i);
						ref ResolvedMovement ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr2, i);
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
							ref MovementControl ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr, j);
							ref ResolvedMovement ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr2, j);
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
						ref MovementControl ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr, k);
						ref ResolvedMovement ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr2, k);
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
						ref MovementControl ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr, l);
						ref ResolvedMovement ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr2, l);
						this.Execute(in ptr7, ref ptr8);
						num++;
					}
					num3 >>= 1;
				}
			}

			// Token: 0x06001606 RID: 5638 RVA: 0x0007BAD0 File Offset: 0x00079CD0
			private JobHandle __ThrowCodeGenException()
			{
				throw new Exception("This method should have been replaced by source gen.");
			}

			// Token: 0x06001607 RID: 5639 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void Run()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001608 RID: 5640 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void RunByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001609 RID: 5641 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void Run(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600160A RID: 5642 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void RunByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600160B RID: 5643 RVA: 0x0007BAF0 File Offset: 0x00079CF0
			public JobHandle Schedule(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600160C RID: 5644 RVA: 0x0007BAF0 File Offset: 0x00079CF0
			public JobHandle ScheduleByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600160D RID: 5645 RVA: 0x0007BAF0 File Offset: 0x00079CF0
			public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600160E RID: 5646 RVA: 0x0007BAF0 File Offset: 0x00079CF0
			public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600160F RID: 5647 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void Schedule()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001610 RID: 5648 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void ScheduleByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001611 RID: 5649 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void Schedule(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001612 RID: 5650 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void ScheduleByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001613 RID: 5651 RVA: 0x0007BAF0 File Offset: 0x00079CF0
			public JobHandle ScheduleParallel(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001614 RID: 5652 RVA: 0x0007BAF0 File Offset: 0x00079CF0
			public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001615 RID: 5653 RVA: 0x0007BAF0 File Offset: 0x00079CF0
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001616 RID: 5654 RVA: 0x0007BAF0 File Offset: 0x00079CF0
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001617 RID: 5655 RVA: 0x0007BAF0 File Offset: 0x00079CF0
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001618 RID: 5656 RVA: 0x0007BAF0 File Offset: 0x00079CF0
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001619 RID: 5657 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void ScheduleParallel()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600161A RID: 5658 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void ScheduleParallelByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600161B RID: 5659 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void ScheduleParallel(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600161C RID: 5660 RVA: 0x0007BAE7 File Offset: 0x00079CE7
			public void ScheduleParallelByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600161D RID: 5661 RVA: 0x0007BAF8 File Offset: 0x00079CF8
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000F1C RID: 3868
			private FallbackResolveMovementSystem.CopyJob.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x0200036F RID: 879
			public struct InternalCompilerQueryAndHandleData
			{
				// Token: 0x0600161E RID: 5662 RVA: 0x0007BB08 File Offset: 0x00079D08
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void __AssignQueries(ref SystemState state)
				{
					EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
					this.DefaultQuery = entityQueryBuilder.WithAll<MovementControl>().WithAll<SimulateMovement>().WithAllRW<ResolvedMovement>()
						.WithOptions(EntityQueryOptions.FilterWriteGroup)
						.Build(ref state);
					entityQueryBuilder.Reset();
					entityQueryBuilder.Dispose();
				}

				// Token: 0x0600161F RID: 5663 RVA: 0x0007BB60 File Offset: 0x00079D60
				public void Init(ref SystemState state, bool assignDefaultQuery)
				{
					if (assignDefaultQuery)
					{
						this.__AssignQueries(ref state);
					}
					this.__TypeHandle.__AssignHandles(ref state);
				}

				// Token: 0x06001620 RID: 5664 RVA: 0x0007BB78 File Offset: 0x00079D78
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Run(ref FallbackResolveMovementSystem.CopyJob job, EntityQuery query)
				{
					job.__TypeHandle = this.__TypeHandle;
					(ref job).RunByRef<FallbackResolveMovementSystem.CopyJob>(query);
				}

				// Token: 0x06001621 RID: 5665 RVA: 0x0007BB8D File Offset: 0x00079D8D
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle Schedule(ref FallbackResolveMovementSystem.CopyJob job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleByRef<FallbackResolveMovementSystem.CopyJob>(query, dependency);
				}

				// Token: 0x06001622 RID: 5666 RVA: 0x0007BBA3 File Offset: 0x00079DA3
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle ScheduleParallel(ref FallbackResolveMovementSystem.CopyJob job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleParallelByRef<FallbackResolveMovementSystem.CopyJob>(query, dependency);
				}

				// Token: 0x06001623 RID: 5667 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void UpdateBaseEntityIndexArray(ref FallbackResolveMovementSystem.CopyJob job, EntityQuery query, ref SystemState state)
				{
				}

				// Token: 0x06001624 RID: 5668 RVA: 0x000743B8 File Offset: 0x000725B8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle UpdateBaseEntityIndexArray(ref FallbackResolveMovementSystem.CopyJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
				{
					return dependency;
				}

				// Token: 0x06001625 RID: 5669 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void AssignEntityManager(ref FallbackResolveMovementSystem.CopyJob job, EntityManager entityManager)
				{
				}

				// Token: 0x04000F1D RID: 3869
				public FallbackResolveMovementSystem.CopyJob.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

				// Token: 0x04000F1E RID: 3870
				public EntityQuery DefaultQuery;

				// Token: 0x02000370 RID: 880
				public struct TypeHandle
				{
					// Token: 0x06001626 RID: 5670 RVA: 0x0007BBB9 File Offset: 0x00079DB9
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public void __AssignHandles(ref SystemState state)
					{
						this.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementControl>(true);
						this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ResolvedMovement>(false);
					}

					// Token: 0x06001627 RID: 5671 RVA: 0x0007BBD5 File Offset: 0x00079DD5
					public void Update(ref SystemState state)
					{
						this.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle.Update(ref state);
					}

					// Token: 0x04000F1F RID: 3871
					[ReadOnly]
					public ComponentTypeHandle<MovementControl> __Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle;

					// Token: 0x04000F20 RID: 3872
					public ComponentTypeHandle<ResolvedMovement> __Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle;
				}
			}

			// Token: 0x02000371 RID: 881
			public struct InternalCompiler
			{
				// Token: 0x06001628 RID: 5672 RVA: 0x00003788 File Offset: 0x00001988
				[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void CheckForErrors(int scheduleType)
				{
				}
			}
		}

		// Token: 0x02000372 RID: 882
		[BurstCompile]
		[WithAll(new Type[] { typeof(SimulateMovement) })]
		[CompilerGenerated]
		public struct CopyRotationJob : IJobEntity, IJobChunk
		{
			// Token: 0x06001629 RID: 5673 RVA: 0x0007BBEF File Offset: 0x00079DEF
			public void Execute(in MovementControl control, ref ResolvedMovement resolved)
			{
				resolved.targetRotation = control.targetRotation;
				resolved.targetRotationHint = control.targetRotationHint;
				resolved.targetRotationOffset = control.targetRotationOffset;
				resolved.rotationSpeed = control.rotationSpeed;
			}

			// Token: 0x0600162A RID: 5674 RVA: 0x0007BC24 File Offset: 0x00079E24
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementControl>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<ResolvedMovement>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						ref MovementControl ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr, i);
						ref ResolvedMovement ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr2, i);
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
							ref MovementControl ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr, j);
							ref ResolvedMovement ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr2, j);
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
						ref MovementControl ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr, k);
						ref ResolvedMovement ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr2, k);
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
						ref MovementControl ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr, l);
						ref ResolvedMovement ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr2, l);
						this.Execute(in ptr7, ref ptr8);
						num++;
					}
					num3 >>= 1;
				}
			}

			// Token: 0x0600162B RID: 5675 RVA: 0x0007BDBC File Offset: 0x00079FBC
			private JobHandle __ThrowCodeGenException()
			{
				throw new Exception("This method should have been replaced by source gen.");
			}

			// Token: 0x0600162C RID: 5676 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void Run()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600162D RID: 5677 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void RunByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600162E RID: 5678 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void Run(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600162F RID: 5679 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void RunByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001630 RID: 5680 RVA: 0x0007BDDC File Offset: 0x00079FDC
			public JobHandle Schedule(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001631 RID: 5681 RVA: 0x0007BDDC File Offset: 0x00079FDC
			public JobHandle ScheduleByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001632 RID: 5682 RVA: 0x0007BDDC File Offset: 0x00079FDC
			public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001633 RID: 5683 RVA: 0x0007BDDC File Offset: 0x00079FDC
			public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001634 RID: 5684 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void Schedule()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001635 RID: 5685 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void ScheduleByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001636 RID: 5686 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void Schedule(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001637 RID: 5687 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void ScheduleByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001638 RID: 5688 RVA: 0x0007BDDC File Offset: 0x00079FDC
			public JobHandle ScheduleParallel(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001639 RID: 5689 RVA: 0x0007BDDC File Offset: 0x00079FDC
			public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600163A RID: 5690 RVA: 0x0007BDDC File Offset: 0x00079FDC
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600163B RID: 5691 RVA: 0x0007BDDC File Offset: 0x00079FDC
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600163C RID: 5692 RVA: 0x0007BDDC File Offset: 0x00079FDC
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600163D RID: 5693 RVA: 0x0007BDDC File Offset: 0x00079FDC
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600163E RID: 5694 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void ScheduleParallel()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600163F RID: 5695 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void ScheduleParallelByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001640 RID: 5696 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void ScheduleParallel(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001641 RID: 5697 RVA: 0x0007BDD3 File Offset: 0x00079FD3
			public void ScheduleParallelByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001642 RID: 5698 RVA: 0x0007BDE4 File Offset: 0x00079FE4
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000F21 RID: 3873
			private FallbackResolveMovementSystem.CopyRotationJob.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x02000373 RID: 883
			public struct InternalCompilerQueryAndHandleData
			{
				// Token: 0x06001643 RID: 5699 RVA: 0x0007BDF4 File Offset: 0x00079FF4
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void __AssignQueries(ref SystemState state)
				{
					EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
					this.DefaultQuery = entityQueryBuilder.WithAll<MovementControl>().WithAll<SimulateMovement>().WithAllRW<ResolvedMovement>()
						.Build(ref state);
					entityQueryBuilder.Reset();
					entityQueryBuilder.Dispose();
				}

				// Token: 0x06001644 RID: 5700 RVA: 0x0007BE43 File Offset: 0x0007A043
				public void Init(ref SystemState state, bool assignDefaultQuery)
				{
					if (assignDefaultQuery)
					{
						this.__AssignQueries(ref state);
					}
					this.__TypeHandle.__AssignHandles(ref state);
				}

				// Token: 0x06001645 RID: 5701 RVA: 0x0007BE5B File Offset: 0x0007A05B
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Run(ref FallbackResolveMovementSystem.CopyRotationJob job, EntityQuery query)
				{
					job.__TypeHandle = this.__TypeHandle;
					(ref job).RunByRef<FallbackResolveMovementSystem.CopyRotationJob>(query);
				}

				// Token: 0x06001646 RID: 5702 RVA: 0x0007BE70 File Offset: 0x0007A070
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle Schedule(ref FallbackResolveMovementSystem.CopyRotationJob job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleByRef<FallbackResolveMovementSystem.CopyRotationJob>(query, dependency);
				}

				// Token: 0x06001647 RID: 5703 RVA: 0x0007BE86 File Offset: 0x0007A086
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle ScheduleParallel(ref FallbackResolveMovementSystem.CopyRotationJob job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleParallelByRef<FallbackResolveMovementSystem.CopyRotationJob>(query, dependency);
				}

				// Token: 0x06001648 RID: 5704 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void UpdateBaseEntityIndexArray(ref FallbackResolveMovementSystem.CopyRotationJob job, EntityQuery query, ref SystemState state)
				{
				}

				// Token: 0x06001649 RID: 5705 RVA: 0x000743B8 File Offset: 0x000725B8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle UpdateBaseEntityIndexArray(ref FallbackResolveMovementSystem.CopyRotationJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
				{
					return dependency;
				}

				// Token: 0x0600164A RID: 5706 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void AssignEntityManager(ref FallbackResolveMovementSystem.CopyRotationJob job, EntityManager entityManager)
				{
				}

				// Token: 0x04000F22 RID: 3874
				public FallbackResolveMovementSystem.CopyRotationJob.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

				// Token: 0x04000F23 RID: 3875
				public EntityQuery DefaultQuery;

				// Token: 0x02000374 RID: 884
				public struct TypeHandle
				{
					// Token: 0x0600164B RID: 5707 RVA: 0x0007BE9C File Offset: 0x0007A09C
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public void __AssignHandles(ref SystemState state)
					{
						this.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementControl>(true);
						this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ResolvedMovement>(false);
					}

					// Token: 0x0600164C RID: 5708 RVA: 0x0007BEB8 File Offset: 0x0007A0B8
					public void Update(ref SystemState state)
					{
						this.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle.Update(ref state);
					}

					// Token: 0x04000F24 RID: 3876
					[ReadOnly]
					public ComponentTypeHandle<MovementControl> __Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle;

					// Token: 0x04000F25 RID: 3877
					public ComponentTypeHandle<ResolvedMovement> __Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle;
				}
			}

			// Token: 0x02000375 RID: 885
			public struct InternalCompiler
			{
				// Token: 0x0600164D RID: 5709 RVA: 0x00003788 File Offset: 0x00001988
				[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void CheckForErrors(int scheduleType)
				{
				}
			}
		}

		// Token: 0x02000376 RID: 886
		private struct TypeHandle
		{
			// Token: 0x0600164E RID: 5710 RVA: 0x0007BED2 File Offset: 0x0007A0D2
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
				this.__Pathfinding_ECS_FallbackResolveMovementSystem_CopyRotationJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
			}

			// Token: 0x04000F26 RID: 3878
			public FallbackResolveMovementSystem.CopyJob.InternalCompilerQueryAndHandleData __Pathfinding_ECS_FallbackResolveMovementSystem_CopyJob_WithDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000F27 RID: 3879
			public FallbackResolveMovementSystem.CopyRotationJob.InternalCompilerQueryAndHandleData __Pathfinding_ECS_FallbackResolveMovementSystem_CopyRotationJob_WithDefaultQuery_JobEntityTypeHandle;
		}
	}
}
