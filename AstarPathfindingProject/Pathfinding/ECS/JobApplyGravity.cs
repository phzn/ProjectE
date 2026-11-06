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
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x02000317 RID: 791
	[BurstCompile]
	[CompilerGenerated]
	public struct JobApplyGravity : IJobEntity, IJobChunk
	{
		// Token: 0x06001353 RID: 4947 RVA: 0x00074488 File Offset: 0x00072688
		private void ResolveGravity(RaycastHit hit, bool grounded, ref LocalTransform transform, in AgentMovementPlane movementPlane, ref GravityState gravityState)
		{
			float num;
			float2 @float = movementPlane.value.ToPlane(transform.Position, out num);
			if (grounded)
			{
				gravityState.verticalVelocity *= math.max(0f, 1f - 5f * this.dt);
				float num2;
				movementPlane.value.ToPlane(hit.point, out num2);
				float num3 = gravityState.verticalVelocity * this.dt;
				if (num2 > num)
				{
					num = Mathf.MoveTowards(num, num2, 6f * math.sqrt(math.abs(num2 - num)) * this.dt);
				}
				else
				{
					num = math.max(num2, num + num3);
				}
			}
			else
			{
				float num4 = gravityState.verticalVelocity * this.dt;
				num += num4;
			}
			transform.Position = movementPlane.value.ToWorld(@float, num);
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00074560 File Offset: 0x00072760
		public void Execute(ref LocalTransform transform, in MovementSettings movementSettings, ref AgentMovementPlane movementPlane, ref GravityState gravityState, in AgentMovementPlaneSource movementPlaneSource, [EntityIndexInQuery] int entityIndexInQuery)
		{
			RaycastHit raycastHit = this.raycastHits[entityIndexInQuery];
			bool flag = raycastHit.colliderInstanceID != 0;
			if (flag && movementPlaneSource.value == MovementPlaneSource.Raycast)
			{
				movementPlane.value = movementPlane.value.MatchUpDirection(raycastHit.normal);
			}
			this.ResolveGravity(raycastHit, flag, ref transform, in movementPlane, ref gravityState);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x000745BC File Offset: 0x000727BC
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementSettings>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle);
			IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle);
			IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<GravityState>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_GravityState_RW_ComponentTypeHandle);
			AgentMovementPlaneSource sharedComponent = chunk.GetSharedComponent<AgentMovementPlaneSource>(this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlaneSource_SharedComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, i);
					ref MovementSettings ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr2, i);
					ref AgentMovementPlane ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, i);
					ref GravityState ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GravityState>(intPtr4, i);
					int num2 = this.__ChunkBaseEntityIndices[chunkIndexInQuery] + num;
					this.Execute(ref ptr, in ptr2, ref ptr3, ref ptr4, in sharedComponent, num2);
					num++;
				}
				return;
			}
			if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
			{
				int j = 0;
				int num3 = 0;
				while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num3, out j, out num3))
				{
					while (j < num3)
					{
						ref LocalTransform ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, j);
						ref MovementSettings ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr2, j);
						ref AgentMovementPlane ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, j);
						ref GravityState ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GravityState>(intPtr4, j);
						int num4 = this.__ChunkBaseEntityIndices[chunkIndexInQuery] + num;
						this.Execute(ref ptr5, in ptr6, ref ptr7, ref ptr8, in sharedComponent, num4);
						j++;
						num++;
					}
				}
				return;
			}
			ulong num5 = chunkEnabledMask.ULong0;
			int num6 = math.min(64, count);
			for (int k = 0; k < num6; k++)
			{
				if ((num5 & 1UL) != 0UL)
				{
					ref LocalTransform ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, k);
					ref MovementSettings ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr2, k);
					ref AgentMovementPlane ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, k);
					ref GravityState ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GravityState>(intPtr4, k);
					int num7 = this.__ChunkBaseEntityIndices[chunkIndexInQuery] + num;
					this.Execute(ref ptr9, in ptr10, ref ptr11, ref ptr12, in sharedComponent, num7);
					num++;
				}
				num5 >>= 1;
			}
			num5 = chunkEnabledMask.ULong1;
			for (int l = 64; l < count; l++)
			{
				if ((num5 & 1UL) != 0UL)
				{
					ref LocalTransform ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, l);
					ref MovementSettings ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr2, l);
					ref AgentMovementPlane ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, l);
					ref GravityState ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GravityState>(intPtr4, l);
					int num8 = this.__ChunkBaseEntityIndices[chunkIndexInQuery] + num;
					this.Execute(ref ptr13, in ptr14, ref ptr15, ref ptr16, in sharedComponent, num8);
					num++;
				}
				num5 >>= 1;
			}
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00074858 File Offset: 0x00072A58
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0007486F File Offset: 0x00072A6F
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0007486F File Offset: 0x00072A6F
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0007486F File Offset: 0x00072A6F
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0007486F File Offset: 0x00072A6F
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00074878 File Offset: 0x00072A78
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00074878 File Offset: 0x00072A78
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00074878 File Offset: 0x00072A78
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00074878 File Offset: 0x00072A78
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0007486F File Offset: 0x00072A6F
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x0007486F File Offset: 0x00072A6F
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0007486F File Offset: 0x00072A6F
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0007486F File Offset: 0x00072A6F
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00074878 File Offset: 0x00072A78
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00074878 File Offset: 0x00072A78
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00074878 File Offset: 0x00072A78
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00074878 File Offset: 0x00072A78
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00074878 File Offset: 0x00072A78
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00074878 File Offset: 0x00072A78
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0007486F File Offset: 0x00072A6F
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0007486F File Offset: 0x00072A6F
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0007486F File Offset: 0x00072A6F
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0007486F File Offset: 0x00072A6F
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00074880 File Offset: 0x00072A80
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000E1D RID: 3613
		[ReadOnly]
		public NativeArray<RaycastHit> raycastHits;

		// Token: 0x04000E1E RID: 3614
		[ReadOnly]
		public NativeArray<RaycastCommand> raycastCommands;

		// Token: 0x04000E1F RID: 3615
		public float dt;

		// Token: 0x04000E20 RID: 3616
		private JobApplyGravity.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x04000E21 RID: 3617
		[ReadOnly]
		public NativeArray<int> __ChunkBaseEntityIndices;

		// Token: 0x02000318 RID: 792
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x0600136E RID: 4974 RVA: 0x00074890 File Offset: 0x00072A90
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithAll<MovementSettings>().WithAll<AgentMovementPlaneSource>().WithAllRW<LocalTransform>()
					.WithAllRW<AgentMovementPlane>()
					.WithAllRW<GravityState>()
					.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x0600136F RID: 4975 RVA: 0x000748EF File Offset: 0x00072AEF
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x06001370 RID: 4976 RVA: 0x00074907 File Offset: 0x00072B07
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobApplyGravity job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				(ref job).RunByRef<JobApplyGravity>(query);
			}

			// Token: 0x06001371 RID: 4977 RVA: 0x0007491C File Offset: 0x00072B1C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobApplyGravity job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobApplyGravity>(query, dependency);
			}

			// Token: 0x06001372 RID: 4978 RVA: 0x00074932 File Offset: 0x00072B32
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobApplyGravity job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return InternalCompilerInterface.JobChunkInterface.ScheduleParallelByRef<JobApplyGravity>(ref job, query, dependency, job.__ChunkBaseEntityIndices);
			}

			// Token: 0x06001373 RID: 4979 RVA: 0x00074950 File Offset: 0x00072B50
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobApplyGravity job, EntityQuery query, ref SystemState state)
			{
				NativeArray<int> nativeArray = query.CalculateBaseEntityIndexArray(state.WorldUpdateAllocator);
				job.__ChunkBaseEntityIndices = nativeArray;
			}

			// Token: 0x06001374 RID: 4980 RVA: 0x00074978 File Offset: 0x00072B78
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobApplyGravity job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				JobHandle jobHandle;
				NativeArray<int> nativeArray = query.CalculateBaseEntityIndexArrayAsync(state.WorldUpdateAllocator, dependency, out jobHandle);
				job.__ChunkBaseEntityIndices = nativeArray;
				return jobHandle;
			}

			// Token: 0x06001375 RID: 4981 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobApplyGravity job, EntityManager entityManager)
			{
			}

			// Token: 0x04000E22 RID: 3618
			public JobApplyGravity.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000E23 RID: 3619
			public EntityQuery DefaultQuery;

			// Token: 0x02000319 RID: 793
			public struct TypeHandle
			{
				// Token: 0x06001376 RID: 4982 RVA: 0x000749A4 File Offset: 0x00072BA4
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(false);
					this.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementSettings>(true);
					this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(false);
					this.__Pathfinding_ECS_GravityState_RW_ComponentTypeHandle = state.GetComponentTypeHandle<GravityState>(false);
					this.__Pathfinding_ECS_AgentMovementPlaneSource_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<AgentMovementPlaneSource>();
				}

				// Token: 0x06001377 RID: 4983 RVA: 0x000749F1 File Offset: 0x00072BF1
				public void Update(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_GravityState_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentMovementPlaneSource_SharedComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000E24 RID: 3620
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				// Token: 0x04000E25 RID: 3621
				[ReadOnly]
				public ComponentTypeHandle<MovementSettings> __Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle;

				// Token: 0x04000E26 RID: 3622
				public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle;

				// Token: 0x04000E27 RID: 3623
				public ComponentTypeHandle<GravityState> __Pathfinding_ECS_GravityState_RW_ComponentTypeHandle;

				// Token: 0x04000E28 RID: 3624
				[ReadOnly]
				public SharedComponentTypeHandle<AgentMovementPlaneSource> __Pathfinding_ECS_AgentMovementPlaneSource_SharedComponentTypeHandle;
			}
		}

		// Token: 0x0200031A RID: 794
		public struct InternalCompiler
		{
			// Token: 0x06001378 RID: 4984 RVA: 0x00003788 File Offset: 0x00001988
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void CheckForErrors(int scheduleType)
			{
			}
		}
	}
}
