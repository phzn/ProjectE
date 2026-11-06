using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding.ECS
{
	// Token: 0x0200033D RID: 829
	[CompilerGenerated]
	public struct JobManagedOffMeshLinkTransitionCleanup : IJobEntity, IJobChunk
	{
		// Token: 0x060014A5 RID: 5285 RVA: 0x00077E09 File Offset: 0x00076009
		public void Execute(ManagedAgentOffMeshLinkTraversal managedLinkInfo)
		{
			if (managedLinkInfo.stateMachine != null)
			{
				managedLinkInfo.stateMachine.OnAbortTraversingOffMeshLink();
			}
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x00077E20 File Offset: 0x00076020
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			ManagedComponentAccessor<ManagedAgentOffMeshLinkTraversal> managedComponentAccessor = chunk.GetManagedComponentAccessor<ManagedAgentOffMeshLinkTraversal>(ref this.__TypeHandle.__Pathfinding_ECS_ManagedAgentOffMeshLinkTraversal_RW_ComponentTypeHandle, this.__EntityManager);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ManagedAgentOffMeshLinkTraversal managedAgentOffMeshLinkTraversal = managedComponentAccessor[i];
					this.Execute(managedAgentOffMeshLinkTraversal);
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
						ManagedAgentOffMeshLinkTraversal managedAgentOffMeshLinkTraversal2 = managedComponentAccessor[j];
						this.Execute(managedAgentOffMeshLinkTraversal2);
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
					ManagedAgentOffMeshLinkTraversal managedAgentOffMeshLinkTraversal3 = managedComponentAccessor[k];
					this.Execute(managedAgentOffMeshLinkTraversal3);
					num++;
				}
				num3 >>= 1;
			}
			num3 = chunkEnabledMask.ULong1;
			for (int l = 64; l < count; l++)
			{
				if ((num3 & 1UL) != 0UL)
				{
					ManagedAgentOffMeshLinkTraversal managedAgentOffMeshLinkTraversal4 = managedComponentAccessor[l];
					this.Execute(managedAgentOffMeshLinkTraversal4);
					num++;
				}
				num3 >>= 1;
			}
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x00077F74 File Offset: 0x00076174
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00077F8B File Offset: 0x0007618B
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00077F8B File Offset: 0x0007618B
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x00077F8B File Offset: 0x0007618B
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x00077F8B File Offset: 0x0007618B
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x00077F94 File Offset: 0x00076194
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x00077F94 File Offset: 0x00076194
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00077F94 File Offset: 0x00076194
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00077F94 File Offset: 0x00076194
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00077F8B File Offset: 0x0007618B
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x00077F8B File Offset: 0x0007618B
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x00077F8B File Offset: 0x0007618B
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00077F8B File Offset: 0x0007618B
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x00077F94 File Offset: 0x00076194
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x00077F94 File Offset: 0x00076194
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x00077F94 File Offset: 0x00076194
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x00077F94 File Offset: 0x00076194
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x00077F94 File Offset: 0x00076194
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x00077F94 File Offset: 0x00076194
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00077F8B File Offset: 0x0007618B
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x00077F8B File Offset: 0x0007618B
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00077F8B File Offset: 0x0007618B
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x00077F8B File Offset: 0x0007618B
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x00077F9C File Offset: 0x0007619C
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000E97 RID: 3735
		private JobManagedOffMeshLinkTransitionCleanup.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x04000E98 RID: 3736
		public EntityManager __EntityManager;

		// Token: 0x0200033E RID: 830
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x060014BF RID: 5311 RVA: 0x00077FAC File Offset: 0x000761AC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithAllRW<ManagedAgentOffMeshLinkTraversal>().Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x060014C0 RID: 5312 RVA: 0x00077FEB File Offset: 0x000761EB
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x060014C1 RID: 5313 RVA: 0x00078003 File Offset: 0x00076203
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobManagedOffMeshLinkTransitionCleanup job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				InternalCompilerInterface.JobChunkInterface.RunByRefWithoutJobs<JobManagedOffMeshLinkTransitionCleanup>(ref job, query);
			}

			// Token: 0x060014C2 RID: 5314 RVA: 0x00078018 File Offset: 0x00076218
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobManagedOffMeshLinkTransitionCleanup job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobManagedOffMeshLinkTransitionCleanup>(query, dependency);
			}

			// Token: 0x060014C3 RID: 5315 RVA: 0x0007802E File Offset: 0x0007622E
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobManagedOffMeshLinkTransitionCleanup job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleParallelByRef<JobManagedOffMeshLinkTransitionCleanup>(query, dependency);
			}

			// Token: 0x060014C4 RID: 5316 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobManagedOffMeshLinkTransitionCleanup job, EntityQuery query, ref SystemState state)
			{
			}

			// Token: 0x060014C5 RID: 5317 RVA: 0x000743B8 File Offset: 0x000725B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobManagedOffMeshLinkTransitionCleanup job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			// Token: 0x060014C6 RID: 5318 RVA: 0x00078044 File Offset: 0x00076244
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobManagedOffMeshLinkTransitionCleanup job, EntityManager entityManager)
			{
				job.__EntityManager = entityManager;
			}

			// Token: 0x04000E99 RID: 3737
			public JobManagedOffMeshLinkTransitionCleanup.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000E9A RID: 3738
			public EntityQuery DefaultQuery;

			// Token: 0x0200033F RID: 831
			public struct TypeHandle
			{
				// Token: 0x060014C7 RID: 5319 RVA: 0x00078050 File Offset: 0x00076250
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Pathfinding_ECS_ManagedAgentOffMeshLinkTraversal_RW_ComponentTypeHandle = state.EntityManager.GetComponentTypeHandle<ManagedAgentOffMeshLinkTraversal>(false);
				}

				// Token: 0x060014C8 RID: 5320 RVA: 0x00078072 File Offset: 0x00076272
				public void Update(ref SystemState state)
				{
					this.__Pathfinding_ECS_ManagedAgentOffMeshLinkTraversal_RW_ComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000E9B RID: 3739
				public ComponentTypeHandle<ManagedAgentOffMeshLinkTraversal> __Pathfinding_ECS_ManagedAgentOffMeshLinkTraversal_RW_ComponentTypeHandle;
			}
		}

		// Token: 0x02000340 RID: 832
		public struct InternalCompiler
		{
			// Token: 0x060014C9 RID: 5321 RVA: 0x000768D5 File Offset: 0x00074AD5
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
}
