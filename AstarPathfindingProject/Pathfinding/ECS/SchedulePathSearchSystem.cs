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
using Unity.Profiling;
using Unity.Transforms;

namespace Pathfinding.ECS
{
	// Token: 0x02000395 RID: 917
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[UpdateBefore(typeof(RepairPathSystem))]
	[UpdateBefore(typeof(TraverseOffMeshLinkSystem))]
	[BurstCompile]
	[CompilerGenerated]
	public struct SchedulePathSearchSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x06001706 RID: 5894 RVA: 0x0007E0A4 File Offset: 0x0007C2A4
		public void OnUpdate(ref SystemState systemState)
		{
			if (AstarPath.active == null || !AIMovementSystemGroup.TimeScaledRateManager.IsFirstSubstep)
			{
				return;
			}
			if (this.__query_376638599_0.IsEmptyIgnoreFilter)
			{
				return;
			}
			NativeBitArray nativeBitArray = new NativeBitArray(512, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			systemState.CompleteDependency();
			PathProcessor.GraphUpdateLock graphUpdateLock = AstarPath.active.PausePathfindingSoon();
			SchedulePathSearchSystem.JobCheckStaleness jobCheckStaleness = default(SchedulePathSearchSystem.JobCheckStaleness);
			jobCheckStaleness.isPathStale = nativeBitArray;
			this.__ScheduleViaJobChunkExtension_0(jobCheckStaleness, this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobCheckStaleness_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, systemState.Dependency, ref systemState, false);
			SchedulePathSearchSystem.JobShouldRecalculatePaths jobShouldRecalculatePaths = default(SchedulePathSearchSystem.JobShouldRecalculatePaths);
			jobShouldRecalculatePaths.time = (float)systemState.WorldUnmanaged.Time.ElapsedTime;
			jobShouldRecalculatePaths.isPathStale = nativeBitArray;
			this.__ScheduleViaJobChunkExtension_1(jobShouldRecalculatePaths, this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobShouldRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, systemState.Dependency, ref systemState, false);
			SchedulePathSearchSystem.JobRecalculatePaths jobRecalculatePaths = default(SchedulePathSearchSystem.JobRecalculatePaths);
			jobRecalculatePaths.time = (float)systemState.WorldUnmanaged.Time.ElapsedTime;
			this.__ScheduleViaJobChunkExtension_2(jobRecalculatePaths, this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, systemState.Dependency, ref systemState, false);
			graphUpdateLock.Release();
			nativeBitArray.Dispose();
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x0007E1C4 File Offset: 0x0007C3C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __ScheduleViaJobChunkExtension_0(SchedulePathSearchSystem.JobCheckStaleness job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobCheckStaleness_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, ref state);
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobCheckStaleness_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobCheckStaleness_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobCheckStaleness_WithDefaultQuery_JobEntityTypeHandle.Run(ref job, query);
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x0007E22C File Offset: 0x0007C42C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __ScheduleViaJobChunkExtension_1(SchedulePathSearchSystem.JobShouldRecalculatePaths job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobShouldRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, ref state);
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobShouldRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobShouldRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobShouldRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.Run(ref job, query);
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x0007E294 File Offset: 0x0007C494
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __ScheduleViaJobChunkExtension_2(SchedulePathSearchSystem.JobRecalculatePaths job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, ref state);
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			this.__TypeHandle.__Pathfinding_ECS_SchedulePathSearchSystem_JobRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.Run(ref job, query);
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x0007E2FC File Offset: 0x0007C4FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			this.__query_376638599_0 = entityQueryBuilder.WithAll<ManagedState>().Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x0007E33B File Offset: 0x0007C53B
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x0007E38E File Offset: 0x0007C58E
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((SchedulePathSearchSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0007E3A7 File Offset: 0x0007C5A7
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((SchedulePathSearchSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000F6E RID: 3950
		private static readonly ProfilerMarker MarkerSchedulePathSearch = new ProfilerMarker("Schedule Path Search");

		// Token: 0x04000F6F RID: 3951
		private static readonly ProfilerMarker MarkerCheckStaleness = new ProfilerMarker("Check Path Staleness");

		// Token: 0x04000F70 RID: 3952
		private static readonly ProfilerMarker MarkerShouldRecalculatePaths = new ProfilerMarker("Check Should Recalculate Paths");

		// Token: 0x04000F71 RID: 3953
		private static readonly ProfilerMarker MarkerRecalculatePaths = new ProfilerMarker("Schedule Path Calculations");

		// Token: 0x04000F72 RID: 3954
		private SchedulePathSearchSystem.TypeHandle __TypeHandle;

		// Token: 0x04000F73 RID: 3955
		private EntityQuery __query_376638599_0;

		// Token: 0x02000396 RID: 918
		[WithAbsent(new Type[] { typeof(ManagedAgentOffMeshLinkTraversal) })]
		[WithPresent(new Type[] { typeof(AgentShouldRecalculatePath) })]
		[CompilerGenerated]
		private struct JobCheckStaleness : IJobEntity, IJobEntityChunkBeginEnd, IJobChunk
		{
			// Token: 0x0600170F RID: 5903 RVA: 0x0007E3C0 File Offset: 0x0007C5C0
			public void Execute(ManagedState state)
			{
				int num = this.index;
				this.index = num + 1;
				this.isPathStale.Set(num, state.pathTracer.isStale);
				num = this.index;
				this.index = num + 1;
				this.isPathStale.Set(num, state.pendingPath != null);
			}

			// Token: 0x06001710 RID: 5904 RVA: 0x0007E419 File Offset: 0x0007C619
			public bool OnChunkBegin(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				if (this.index + chunk.Count * 2 > this.isPathStale.Length)
				{
					this.isPathStale.Resize(math.ceilpow2(this.index + chunk.Count * 2), NativeArrayOptions.ClearMemory);
				}
				return true;
			}

			// Token: 0x06001711 RID: 5905 RVA: 0x00003788 File Offset: 0x00001988
			public void OnChunkEnd(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask, bool chunkWasExecuted)
			{
			}

			// Token: 0x06001712 RID: 5906 RVA: 0x0007E458 File Offset: 0x0007C658
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				bool flag = this.OnChunkBegin(in chunk, chunkIndexInQuery, useEnabledMask, in chunkEnabledMask);
				if (flag)
				{
					ManagedComponentAccessor<ManagedState> managedComponentAccessor = chunk.GetManagedComponentAccessor<ManagedState>(ref this.__TypeHandle.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle, this.__EntityManager);
					int count = chunk.Count;
					int num = 0;
					if (!useEnabledMask)
					{
						for (int i = 0; i < count; i++)
						{
							ManagedState managedState = managedComponentAccessor[i];
							this.Execute(managedState);
							num++;
						}
					}
					else if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
					{
						int j = 0;
						int num2 = 0;
						while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num2, out j, out num2))
						{
							while (j < num2)
							{
								ManagedState managedState2 = managedComponentAccessor[j];
								this.Execute(managedState2);
								j++;
								num++;
							}
						}
					}
					else
					{
						ulong num3 = chunkEnabledMask.ULong0;
						int num4 = math.min(64, count);
						for (int k = 0; k < num4; k++)
						{
							if ((num3 & 1UL) != 0UL)
							{
								ManagedState managedState3 = managedComponentAccessor[k];
								this.Execute(managedState3);
								num++;
							}
							num3 >>= 1;
						}
						num3 = chunkEnabledMask.ULong1;
						for (int l = 64; l < count; l++)
						{
							if ((num3 & 1UL) != 0UL)
							{
								ManagedState managedState4 = managedComponentAccessor[l];
								this.Execute(managedState4);
								num++;
							}
							num3 >>= 1;
						}
					}
				}
				this.OnChunkEnd(in chunk, chunkIndexInQuery, useEnabledMask, in chunkEnabledMask, flag);
			}

			// Token: 0x06001713 RID: 5907 RVA: 0x0007E5D4 File Offset: 0x0007C7D4
			private JobHandle __ThrowCodeGenException()
			{
				throw new Exception("This method should have been replaced by source gen.");
			}

			// Token: 0x06001714 RID: 5908 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void Run()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001715 RID: 5909 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void RunByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001716 RID: 5910 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void Run(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001717 RID: 5911 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void RunByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001718 RID: 5912 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
			public JobHandle Schedule(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001719 RID: 5913 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
			public JobHandle ScheduleByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600171A RID: 5914 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
			public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600171B RID: 5915 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
			public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600171C RID: 5916 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void Schedule()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600171D RID: 5917 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void ScheduleByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600171E RID: 5918 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void Schedule(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600171F RID: 5919 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void ScheduleByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001720 RID: 5920 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
			public JobHandle ScheduleParallel(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001721 RID: 5921 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
			public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001722 RID: 5922 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001723 RID: 5923 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001724 RID: 5924 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001725 RID: 5925 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001726 RID: 5926 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void ScheduleParallel()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001727 RID: 5927 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void ScheduleParallelByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001728 RID: 5928 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void ScheduleParallel(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001729 RID: 5929 RVA: 0x0007E5EB File Offset: 0x0007C7EB
			public void ScheduleParallelByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600172A RID: 5930 RVA: 0x0007E5FC File Offset: 0x0007C7FC
			bool IJobEntityChunkBeginEnd.OnChunkBegin(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				return this.OnChunkBegin(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x0600172B RID: 5931 RVA: 0x0007E609 File Offset: 0x0007C809
			void IJobEntityChunkBeginEnd.OnChunkEnd(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask, bool chunkWasExecuted)
			{
				this.OnChunkEnd(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask, chunkWasExecuted);
			}

			// Token: 0x0600172C RID: 5932 RVA: 0x0007E618 File Offset: 0x0007C818
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000F74 RID: 3956
			public NativeBitArray isPathStale;

			// Token: 0x04000F75 RID: 3957
			private int index;

			// Token: 0x04000F76 RID: 3958
			private SchedulePathSearchSystem.JobCheckStaleness.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000F77 RID: 3959
			public EntityManager __EntityManager;

			// Token: 0x02000397 RID: 919
			public struct InternalCompilerQueryAndHandleData
			{
				// Token: 0x0600172D RID: 5933 RVA: 0x0007E628 File Offset: 0x0007C828
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void __AssignQueries(ref SystemState state)
				{
					EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
					this.DefaultQuery = entityQueryBuilder.WithPresent<AgentShouldRecalculatePath>().WithAbsent<ManagedAgentOffMeshLinkTraversal>().WithAllRW<ManagedState>()
						.Build(ref state);
					entityQueryBuilder.Reset();
					entityQueryBuilder.Dispose();
				}

				// Token: 0x0600172E RID: 5934 RVA: 0x0007E677 File Offset: 0x0007C877
				public void Init(ref SystemState state, bool assignDefaultQuery)
				{
					if (assignDefaultQuery)
					{
						this.__AssignQueries(ref state);
					}
					this.__TypeHandle.__AssignHandles(ref state);
				}

				// Token: 0x0600172F RID: 5935 RVA: 0x0007E68F File Offset: 0x0007C88F
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Run(ref SchedulePathSearchSystem.JobCheckStaleness job, EntityQuery query)
				{
					job.__TypeHandle = this.__TypeHandle;
					InternalCompilerInterface.JobChunkInterface.RunByRefWithoutJobs<SchedulePathSearchSystem.JobCheckStaleness>(ref job, query);
				}

				// Token: 0x06001730 RID: 5936 RVA: 0x0007E6A4 File Offset: 0x0007C8A4
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle Schedule(ref SchedulePathSearchSystem.JobCheckStaleness job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleByRef<SchedulePathSearchSystem.JobCheckStaleness>(query, dependency);
				}

				// Token: 0x06001731 RID: 5937 RVA: 0x0007E6BA File Offset: 0x0007C8BA
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle ScheduleParallel(ref SchedulePathSearchSystem.JobCheckStaleness job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleParallelByRef<SchedulePathSearchSystem.JobCheckStaleness>(query, dependency);
				}

				// Token: 0x06001732 RID: 5938 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void UpdateBaseEntityIndexArray(ref SchedulePathSearchSystem.JobCheckStaleness job, EntityQuery query, ref SystemState state)
				{
				}

				// Token: 0x06001733 RID: 5939 RVA: 0x000743B8 File Offset: 0x000725B8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle UpdateBaseEntityIndexArray(ref SchedulePathSearchSystem.JobCheckStaleness job, EntityQuery query, JobHandle dependency, ref SystemState state)
				{
					return dependency;
				}

				// Token: 0x06001734 RID: 5940 RVA: 0x0007E6D0 File Offset: 0x0007C8D0
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void AssignEntityManager(ref SchedulePathSearchSystem.JobCheckStaleness job, EntityManager entityManager)
				{
					job.__EntityManager = entityManager;
				}

				// Token: 0x04000F78 RID: 3960
				public SchedulePathSearchSystem.JobCheckStaleness.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

				// Token: 0x04000F79 RID: 3961
				public EntityQuery DefaultQuery;

				// Token: 0x02000398 RID: 920
				public struct TypeHandle
				{
					// Token: 0x06001735 RID: 5941 RVA: 0x0007E6DC File Offset: 0x0007C8DC
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public void __AssignHandles(ref SystemState state)
					{
						this.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle = state.EntityManager.GetComponentTypeHandle<ManagedState>(false);
					}

					// Token: 0x06001736 RID: 5942 RVA: 0x0007E6FE File Offset: 0x0007C8FE
					public void Update(ref SystemState state)
					{
						this.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle.Update(ref state);
					}

					// Token: 0x04000F7A RID: 3962
					public ComponentTypeHandle<ManagedState> __Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle;
				}
			}

			// Token: 0x02000399 RID: 921
			public struct InternalCompiler
			{
				// Token: 0x06001737 RID: 5943 RVA: 0x000768D5 File Offset: 0x00074AD5
				[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void CheckForErrors(int scheduleType)
				{
					if (scheduleType == 2)
					{
						throw new InvalidOperationException("Tried to ScheduleParallel a job with a managed execute signature. Please use .Run or .Schedule instead.");
					}
				}
			}
		}

		// Token: 0x0200039A RID: 922
		[BurstCompile]
		[WithAbsent(new Type[] { typeof(ManagedAgentOffMeshLinkTraversal) })]
		[WithPresent(new Type[] { typeof(AgentShouldRecalculatePath) })]
		[CompilerGenerated]
		private struct JobShouldRecalculatePaths : IJobEntity, IJobChunk
		{
			// Token: 0x06001738 RID: 5944 RVA: 0x0007E70C File Offset: 0x0007C90C
			public void Execute(ref AutoRepathPolicy autoRepathPolicy, in LocalTransform transform, in AgentCylinderShape shape, in DestinationPoint destination, EnabledRefRW<AgentShouldRecalculatePath> shouldRecalculatePath)
			{
				int num = this.index;
				this.index = num + 1;
				bool flag = this.isPathStale.IsSet(num);
				num = this.index;
				this.index = num + 1;
				shouldRecalculatePath.ValueRW = this.isPathStale.IsSet(num) || autoRepathPolicy.ShouldRecalculatePath(transform.Position, shape.radius, destination.destination, this.time, flag);
			}

			// Token: 0x06001739 RID: 5945 RVA: 0x0007E780 File Offset: 0x0007C980
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AutoRepathPolicy>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AutoRepathPolicy_RW_ComponentTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
				IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle);
				IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<DestinationPoint>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle);
				EnabledMask enabledMask = chunk.GetEnabledMask<AgentShouldRecalculatePath>(ref this.__TypeHandle.__Pathfinding_ECS_AgentShouldRecalculatePath_RW_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						ref AutoRepathPolicy ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AutoRepathPolicy>(intPtr, i);
						ref LocalTransform ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, i);
						ref AgentCylinderShape ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, i);
						ref DestinationPoint ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr4, i);
						this.Execute(ref ptr, in ptr2, in ptr3, in ptr4, enabledMask.GetEnabledRefRW<AgentShouldRecalculatePath>(i));
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
							ref AutoRepathPolicy ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AutoRepathPolicy>(intPtr, j);
							ref LocalTransform ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, j);
							ref AgentCylinderShape ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, j);
							ref DestinationPoint ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr4, j);
							this.Execute(ref ptr5, in ptr6, in ptr7, in ptr8, enabledMask.GetEnabledRefRW<AgentShouldRecalculatePath>(j));
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
						ref AutoRepathPolicy ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AutoRepathPolicy>(intPtr, k);
						ref LocalTransform ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, k);
						ref AgentCylinderShape ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, k);
						ref DestinationPoint ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr4, k);
						this.Execute(ref ptr9, in ptr10, in ptr11, in ptr12, enabledMask.GetEnabledRefRW<AgentShouldRecalculatePath>(k));
						num++;
					}
					num3 >>= 1;
				}
				num3 = chunkEnabledMask.ULong1;
				for (int l = 64; l < count; l++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						ref AutoRepathPolicy ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AutoRepathPolicy>(intPtr, l);
						ref LocalTransform ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, l);
						ref AgentCylinderShape ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, l);
						ref DestinationPoint ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr4, l);
						this.Execute(ref ptr13, in ptr14, in ptr15, in ptr16, enabledMask.GetEnabledRefRW<AgentShouldRecalculatePath>(l));
						num++;
					}
					num3 >>= 1;
				}
			}

			// Token: 0x0600173A RID: 5946 RVA: 0x0007E9EC File Offset: 0x0007CBEC
			private JobHandle __ThrowCodeGenException()
			{
				throw new Exception("This method should have been replaced by source gen.");
			}

			// Token: 0x0600173B RID: 5947 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void Run()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600173C RID: 5948 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void RunByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600173D RID: 5949 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void Run(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600173E RID: 5950 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void RunByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600173F RID: 5951 RVA: 0x0007EA0C File Offset: 0x0007CC0C
			public JobHandle Schedule(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001740 RID: 5952 RVA: 0x0007EA0C File Offset: 0x0007CC0C
			public JobHandle ScheduleByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001741 RID: 5953 RVA: 0x0007EA0C File Offset: 0x0007CC0C
			public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001742 RID: 5954 RVA: 0x0007EA0C File Offset: 0x0007CC0C
			public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001743 RID: 5955 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void Schedule()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001744 RID: 5956 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void ScheduleByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001745 RID: 5957 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void Schedule(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001746 RID: 5958 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void ScheduleByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001747 RID: 5959 RVA: 0x0007EA0C File Offset: 0x0007CC0C
			public JobHandle ScheduleParallel(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001748 RID: 5960 RVA: 0x0007EA0C File Offset: 0x0007CC0C
			public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001749 RID: 5961 RVA: 0x0007EA0C File Offset: 0x0007CC0C
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600174A RID: 5962 RVA: 0x0007EA0C File Offset: 0x0007CC0C
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600174B RID: 5963 RVA: 0x0007EA0C File Offset: 0x0007CC0C
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600174C RID: 5964 RVA: 0x0007EA0C File Offset: 0x0007CC0C
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600174D RID: 5965 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void ScheduleParallel()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600174E RID: 5966 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void ScheduleParallelByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600174F RID: 5967 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void ScheduleParallel(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001750 RID: 5968 RVA: 0x0007EA03 File Offset: 0x0007CC03
			public void ScheduleParallelByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001751 RID: 5969 RVA: 0x0007EA14 File Offset: 0x0007CC14
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000F7B RID: 3963
			public float time;

			// Token: 0x04000F7C RID: 3964
			public NativeBitArray isPathStale;

			// Token: 0x04000F7D RID: 3965
			private int index;

			// Token: 0x04000F7E RID: 3966
			private SchedulePathSearchSystem.JobShouldRecalculatePaths.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x0200039B RID: 923
			public struct InternalCompilerQueryAndHandleData
			{
				// Token: 0x06001752 RID: 5970 RVA: 0x0007EA24 File Offset: 0x0007CC24
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void __AssignQueries(ref SystemState state)
				{
					EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
					this.DefaultQuery = entityQueryBuilder.WithPresentRW<AgentShouldRecalculatePath>().WithAbsent<ManagedAgentOffMeshLinkTraversal>().WithAll<LocalTransform>()
						.WithAll<AgentCylinderShape>()
						.WithAll<DestinationPoint>()
						.WithAllRW<AutoRepathPolicy>()
						.Build(ref state);
					entityQueryBuilder.Reset();
					entityQueryBuilder.Dispose();
				}

				// Token: 0x06001753 RID: 5971 RVA: 0x0007EA8B File Offset: 0x0007CC8B
				public void Init(ref SystemState state, bool assignDefaultQuery)
				{
					if (assignDefaultQuery)
					{
						this.__AssignQueries(ref state);
					}
					this.__TypeHandle.__AssignHandles(ref state);
				}

				// Token: 0x06001754 RID: 5972 RVA: 0x0007EAA3 File Offset: 0x0007CCA3
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Run(ref SchedulePathSearchSystem.JobShouldRecalculatePaths job, EntityQuery query)
				{
					job.__TypeHandle = this.__TypeHandle;
					(ref job).RunByRef<SchedulePathSearchSystem.JobShouldRecalculatePaths>(query);
				}

				// Token: 0x06001755 RID: 5973 RVA: 0x0007EAB8 File Offset: 0x0007CCB8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle Schedule(ref SchedulePathSearchSystem.JobShouldRecalculatePaths job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleByRef<SchedulePathSearchSystem.JobShouldRecalculatePaths>(query, dependency);
				}

				// Token: 0x06001756 RID: 5974 RVA: 0x0007EACE File Offset: 0x0007CCCE
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle ScheduleParallel(ref SchedulePathSearchSystem.JobShouldRecalculatePaths job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleParallelByRef<SchedulePathSearchSystem.JobShouldRecalculatePaths>(query, dependency);
				}

				// Token: 0x06001757 RID: 5975 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void UpdateBaseEntityIndexArray(ref SchedulePathSearchSystem.JobShouldRecalculatePaths job, EntityQuery query, ref SystemState state)
				{
				}

				// Token: 0x06001758 RID: 5976 RVA: 0x000743B8 File Offset: 0x000725B8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle UpdateBaseEntityIndexArray(ref SchedulePathSearchSystem.JobShouldRecalculatePaths job, EntityQuery query, JobHandle dependency, ref SystemState state)
				{
					return dependency;
				}

				// Token: 0x06001759 RID: 5977 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void AssignEntityManager(ref SchedulePathSearchSystem.JobShouldRecalculatePaths job, EntityManager entityManager)
				{
				}

				// Token: 0x04000F7F RID: 3967
				public SchedulePathSearchSystem.JobShouldRecalculatePaths.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

				// Token: 0x04000F80 RID: 3968
				public EntityQuery DefaultQuery;

				// Token: 0x0200039C RID: 924
				public struct TypeHandle
				{
					// Token: 0x0600175A RID: 5978 RVA: 0x0007EAE4 File Offset: 0x0007CCE4
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public void __AssignHandles(ref SystemState state)
					{
						this.__Pathfinding_ECS_AutoRepathPolicy_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AutoRepathPolicy>(false);
						this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(true);
						this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(true);
						this.__Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle = state.GetComponentTypeHandle<DestinationPoint>(true);
						this.__Pathfinding_ECS_AgentShouldRecalculatePath_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentShouldRecalculatePath>(false);
					}

					// Token: 0x0600175B RID: 5979 RVA: 0x0007EB32 File Offset: 0x0007CD32
					public void Update(ref SystemState state)
					{
						this.__Pathfinding_ECS_AutoRepathPolicy_RW_ComponentTypeHandle.Update(ref state);
						this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_AgentShouldRecalculatePath_RW_ComponentTypeHandle.Update(ref state);
					}

					// Token: 0x04000F81 RID: 3969
					public ComponentTypeHandle<AutoRepathPolicy> __Pathfinding_ECS_AutoRepathPolicy_RW_ComponentTypeHandle;

					// Token: 0x04000F82 RID: 3970
					[ReadOnly]
					public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

					// Token: 0x04000F83 RID: 3971
					[ReadOnly]
					public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle;

					// Token: 0x04000F84 RID: 3972
					[ReadOnly]
					public ComponentTypeHandle<DestinationPoint> __Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle;

					// Token: 0x04000F85 RID: 3973
					public ComponentTypeHandle<AgentShouldRecalculatePath> __Pathfinding_ECS_AgentShouldRecalculatePath_RW_ComponentTypeHandle;
				}
			}

			// Token: 0x0200039D RID: 925
			public struct InternalCompiler
			{
				// Token: 0x0600175C RID: 5980 RVA: 0x00003788 File Offset: 0x00001988
				[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void CheckForErrors(int scheduleType)
				{
				}
			}
		}

		// Token: 0x0200039E RID: 926
		[WithAbsent(new Type[] { typeof(ManagedAgentOffMeshLinkTraversal) })]
		[WithAll(new Type[] { typeof(AgentShouldRecalculatePath) })]
		[CompilerGenerated]
		public struct JobRecalculatePaths : IJobEntity, IJobChunk
		{
			// Token: 0x0600175D RID: 5981 RVA: 0x0007EB70 File Offset: 0x0007CD70
			public void Execute(ManagedState state, ManagedSettings settings, ref AutoRepathPolicy autoRepathPolicy, ref LocalTransform transform, ref DestinationPoint destination, ref AgentMovementPlane movementPlane)
			{
				SchedulePathSearchSystem.JobRecalculatePaths.MaybeRecalculatePath(state, settings, ref autoRepathPolicy, ref transform, ref destination, ref movementPlane, this.time, true);
			}

			// Token: 0x0600175E RID: 5982 RVA: 0x0007EB88 File Offset: 0x0007CD88
			public static void MaybeRecalculatePath(ManagedState state, ManagedSettings settings, ref AutoRepathPolicy autoRepathPolicy, ref LocalTransform transform, ref DestinationPoint destination, ref AgentMovementPlane movementPlane, float time, bool wantsToRecalculatePath)
			{
				if (wantsToRecalculatePath)
				{
					if (state.pendingPath == null)
					{
						ABPath abpath = ABPath.Construct(transform.Position, destination.destination, null);
						abpath.UseSettings(settings.pathfindingSettings);
						abpath.nearestNodeDistanceMetric = DistanceMetric.ClosestAsSeenFromAboveSoft(movementPlane.value.up);
						ManagedState.SetPath(abpath, state, in movementPlane, ref destination);
						autoRepathPolicy.OnScheduledPathRecalculation(destination.destination, time);
						return;
					}
					ABPath abpath2 = state.pendingPath as ABPath;
					if (abpath2 != null)
					{
						abpath2.RefreshPathEndpoints(transform.Position, destination.destination);
					}
				}
			}

			// Token: 0x0600175F RID: 5983 RVA: 0x0007EC30 File Offset: 0x0007CE30
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				ManagedComponentAccessor<ManagedState> managedComponentAccessor = chunk.GetManagedComponentAccessor<ManagedState>(ref this.__TypeHandle.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle, this.__EntityManager);
				ManagedComponentAccessor<ManagedSettings> managedComponentAccessor2 = chunk.GetManagedComponentAccessor<ManagedSettings>(ref this.__TypeHandle.__Pathfinding_ECS_ManagedSettings_RW_ComponentTypeHandle, this.__EntityManager);
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AutoRepathPolicy>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AutoRepathPolicy_RW_ComponentTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
				IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<DestinationPoint>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle);
				IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						ManagedState managedState = managedComponentAccessor[i];
						ManagedSettings managedSettings = managedComponentAccessor2[i];
						ref AutoRepathPolicy ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AutoRepathPolicy>(intPtr, i);
						ref LocalTransform ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, i);
						ref DestinationPoint ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, i);
						ref AgentMovementPlane ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, i);
						this.Execute(managedState, managedSettings, ref ptr, ref ptr2, ref ptr3, ref ptr4);
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
							ManagedState managedState2 = managedComponentAccessor[j];
							ManagedSettings managedSettings2 = managedComponentAccessor2[j];
							ref AutoRepathPolicy ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AutoRepathPolicy>(intPtr, j);
							ref LocalTransform ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, j);
							ref DestinationPoint ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, j);
							ref AgentMovementPlane ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, j);
							this.Execute(managedState2, managedSettings2, ref ptr5, ref ptr6, ref ptr7, ref ptr8);
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
						ManagedState managedState3 = managedComponentAccessor[k];
						ManagedSettings managedSettings3 = managedComponentAccessor2[k];
						ref AutoRepathPolicy ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AutoRepathPolicy>(intPtr, k);
						ref LocalTransform ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, k);
						ref DestinationPoint ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, k);
						ref AgentMovementPlane ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, k);
						this.Execute(managedState3, managedSettings3, ref ptr9, ref ptr10, ref ptr11, ref ptr12);
						num++;
					}
					num3 >>= 1;
				}
				num3 = chunkEnabledMask.ULong1;
				for (int l = 64; l < count; l++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						ManagedState managedState4 = managedComponentAccessor[l];
						ManagedSettings managedSettings4 = managedComponentAccessor2[l];
						ref AutoRepathPolicy ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AutoRepathPolicy>(intPtr, l);
						ref LocalTransform ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, l);
						ref DestinationPoint ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, l);
						ref AgentMovementPlane ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, l);
						this.Execute(managedState4, managedSettings4, ref ptr13, ref ptr14, ref ptr15, ref ptr16);
						num++;
					}
					num3 >>= 1;
				}
			}

			// Token: 0x06001760 RID: 5984 RVA: 0x0007EF08 File Offset: 0x0007D108
			private JobHandle __ThrowCodeGenException()
			{
				throw new Exception("This method should have been replaced by source gen.");
			}

			// Token: 0x06001761 RID: 5985 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void Run()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001762 RID: 5986 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void RunByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001763 RID: 5987 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void Run(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001764 RID: 5988 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void RunByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001765 RID: 5989 RVA: 0x0007EF28 File Offset: 0x0007D128
			public JobHandle Schedule(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001766 RID: 5990 RVA: 0x0007EF28 File Offset: 0x0007D128
			public JobHandle ScheduleByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001767 RID: 5991 RVA: 0x0007EF28 File Offset: 0x0007D128
			public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001768 RID: 5992 RVA: 0x0007EF28 File Offset: 0x0007D128
			public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001769 RID: 5993 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void Schedule()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600176A RID: 5994 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void ScheduleByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600176B RID: 5995 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void Schedule(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600176C RID: 5996 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void ScheduleByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600176D RID: 5997 RVA: 0x0007EF28 File Offset: 0x0007D128
			public JobHandle ScheduleParallel(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600176E RID: 5998 RVA: 0x0007EF28 File Offset: 0x0007D128
			public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600176F RID: 5999 RVA: 0x0007EF28 File Offset: 0x0007D128
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001770 RID: 6000 RVA: 0x0007EF28 File Offset: 0x0007D128
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001771 RID: 6001 RVA: 0x0007EF28 File Offset: 0x0007D128
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001772 RID: 6002 RVA: 0x0007EF28 File Offset: 0x0007D128
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001773 RID: 6003 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void ScheduleParallel()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001774 RID: 6004 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void ScheduleParallelByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001775 RID: 6005 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void ScheduleParallel(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001776 RID: 6006 RVA: 0x0007EF1F File Offset: 0x0007D11F
			public void ScheduleParallelByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001777 RID: 6007 RVA: 0x0007EF30 File Offset: 0x0007D130
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000F86 RID: 3974
			public float time;

			// Token: 0x04000F87 RID: 3975
			private SchedulePathSearchSystem.JobRecalculatePaths.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000F88 RID: 3976
			public EntityManager __EntityManager;

			// Token: 0x0200039F RID: 927
			public struct InternalCompilerQueryAndHandleData
			{
				// Token: 0x06001778 RID: 6008 RVA: 0x0007EF40 File Offset: 0x0007D140
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void __AssignQueries(ref SystemState state)
				{
					EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
					this.DefaultQuery = entityQueryBuilder.WithAbsent<ManagedAgentOffMeshLinkTraversal>().WithAll<AgentShouldRecalculatePath>().WithAllRW<ManagedState>()
						.WithAllRW<ManagedSettings>()
						.WithAllRW<AutoRepathPolicy>()
						.WithAllRW<LocalTransform>()
						.WithAllRW<DestinationPoint>()
						.WithAllRW<AgentMovementPlane>()
						.Build(ref state);
					entityQueryBuilder.Reset();
					entityQueryBuilder.Dispose();
				}

				// Token: 0x06001779 RID: 6009 RVA: 0x0007EFB7 File Offset: 0x0007D1B7
				public void Init(ref SystemState state, bool assignDefaultQuery)
				{
					if (assignDefaultQuery)
					{
						this.__AssignQueries(ref state);
					}
					this.__TypeHandle.__AssignHandles(ref state);
				}

				// Token: 0x0600177A RID: 6010 RVA: 0x0007EFCF File Offset: 0x0007D1CF
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Run(ref SchedulePathSearchSystem.JobRecalculatePaths job, EntityQuery query)
				{
					job.__TypeHandle = this.__TypeHandle;
					InternalCompilerInterface.JobChunkInterface.RunByRefWithoutJobs<SchedulePathSearchSystem.JobRecalculatePaths>(ref job, query);
				}

				// Token: 0x0600177B RID: 6011 RVA: 0x0007EFE4 File Offset: 0x0007D1E4
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle Schedule(ref SchedulePathSearchSystem.JobRecalculatePaths job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleByRef<SchedulePathSearchSystem.JobRecalculatePaths>(query, dependency);
				}

				// Token: 0x0600177C RID: 6012 RVA: 0x0007EFFA File Offset: 0x0007D1FA
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle ScheduleParallel(ref SchedulePathSearchSystem.JobRecalculatePaths job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleParallelByRef<SchedulePathSearchSystem.JobRecalculatePaths>(query, dependency);
				}

				// Token: 0x0600177D RID: 6013 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void UpdateBaseEntityIndexArray(ref SchedulePathSearchSystem.JobRecalculatePaths job, EntityQuery query, ref SystemState state)
				{
				}

				// Token: 0x0600177E RID: 6014 RVA: 0x000743B8 File Offset: 0x000725B8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle UpdateBaseEntityIndexArray(ref SchedulePathSearchSystem.JobRecalculatePaths job, EntityQuery query, JobHandle dependency, ref SystemState state)
				{
					return dependency;
				}

				// Token: 0x0600177F RID: 6015 RVA: 0x0007F010 File Offset: 0x0007D210
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void AssignEntityManager(ref SchedulePathSearchSystem.JobRecalculatePaths job, EntityManager entityManager)
				{
					job.__EntityManager = entityManager;
				}

				// Token: 0x04000F89 RID: 3977
				public SchedulePathSearchSystem.JobRecalculatePaths.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

				// Token: 0x04000F8A RID: 3978
				public EntityQuery DefaultQuery;

				// Token: 0x020003A0 RID: 928
				public struct TypeHandle
				{
					// Token: 0x06001780 RID: 6016 RVA: 0x0007F01C File Offset: 0x0007D21C
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public void __AssignHandles(ref SystemState state)
					{
						this.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle = state.EntityManager.GetComponentTypeHandle<ManagedState>(false);
						this.__Pathfinding_ECS_ManagedSettings_RW_ComponentTypeHandle = state.EntityManager.GetComponentTypeHandle<ManagedSettings>(false);
						this.__Pathfinding_ECS_AutoRepathPolicy_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AutoRepathPolicy>(false);
						this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(false);
						this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle = state.GetComponentTypeHandle<DestinationPoint>(false);
						this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(false);
					}

					// Token: 0x06001781 RID: 6017 RVA: 0x0007F088 File Offset: 0x0007D288
					public void Update(ref SystemState state)
					{
						this.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_ManagedSettings_RW_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_AutoRepathPolicy_RW_ComponentTypeHandle.Update(ref state);
						this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle.Update(ref state);
					}

					// Token: 0x04000F8B RID: 3979
					public ComponentTypeHandle<ManagedState> __Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle;

					// Token: 0x04000F8C RID: 3980
					public ComponentTypeHandle<ManagedSettings> __Pathfinding_ECS_ManagedSettings_RW_ComponentTypeHandle;

					// Token: 0x04000F8D RID: 3981
					public ComponentTypeHandle<AutoRepathPolicy> __Pathfinding_ECS_AutoRepathPolicy_RW_ComponentTypeHandle;

					// Token: 0x04000F8E RID: 3982
					public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

					// Token: 0x04000F8F RID: 3983
					public ComponentTypeHandle<DestinationPoint> __Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle;

					// Token: 0x04000F90 RID: 3984
					public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle;
				}
			}

			// Token: 0x020003A1 RID: 929
			public struct InternalCompiler
			{
				// Token: 0x06001782 RID: 6018 RVA: 0x000768D5 File Offset: 0x00074AD5
				[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void CheckForErrors(int scheduleType)
				{
					if (scheduleType == 2)
					{
						throw new InvalidOperationException("Tried to ScheduleParallel a job with a managed execute signature. Please use .Run or .Schedule instead.");
					}
				}
			}
		}

		// Token: 0x020003A2 RID: 930
		private struct TypeHandle
		{
			// Token: 0x06001783 RID: 6019 RVA: 0x0007F0DD File Offset: 0x0007D2DD
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Pathfinding_ECS_SchedulePathSearchSystem_JobCheckStaleness_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
				this.__Pathfinding_ECS_SchedulePathSearchSystem_JobShouldRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
				this.__Pathfinding_ECS_SchedulePathSearchSystem_JobRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
			}

			// Token: 0x04000F91 RID: 3985
			public SchedulePathSearchSystem.JobCheckStaleness.InternalCompilerQueryAndHandleData __Pathfinding_ECS_SchedulePathSearchSystem_JobCheckStaleness_WithDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000F92 RID: 3986
			public SchedulePathSearchSystem.JobShouldRecalculatePaths.InternalCompilerQueryAndHandleData __Pathfinding_ECS_SchedulePathSearchSystem_JobShouldRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000F93 RID: 3987
			public SchedulePathSearchSystem.JobRecalculatePaths.InternalCompilerQueryAndHandleData __Pathfinding_ECS_SchedulePathSearchSystem_JobRecalculatePaths_WithDefaultQuery_JobEntityTypeHandle;
		}
	}
}
