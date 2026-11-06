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
	// Token: 0x0200035D RID: 861
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[UpdateBefore(typeof(SchedulePathSearchSystem))]
	[BurstCompile]
	[RequireMatchingQueriesForUpdate]
	[CompilerGenerated]
	public struct DestinationEntitySystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x0600158F RID: 5519 RVA: 0x0007AD10 File Offset: 0x00078F10
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			DestinationEntitySystem.UpdateDestinationJob updateDestinationJob = default(DestinationEntitySystem.UpdateDestinationJob);
			updateDestinationJob.TransformLookup = InternalCompilerInterface.GetComponentLookup<LocalToWorld>(ref this.__TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref state);
			state.Dependency = this.__ScheduleViaJobChunkExtension_0(updateDestinationJob, this.__TypeHandle.__Pathfinding_ECS_DestinationEntitySystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, false);
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x0007AD64 File Offset: 0x00078F64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_0(DestinationEntitySystem.UpdateDestinationJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_DestinationEntitySystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_DestinationEntitySystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_DestinationEntitySystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_DestinationEntitySystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.Schedule(ref job, query, dependency);
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0007ADD0 File Offset: 0x00078FD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			entityQueryBuilder.Dispose();
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0007ADF1 File Offset: 0x00078FF1
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0007AE06 File Offset: 0x00079006
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			DestinationEntitySystem.__codegen__OnUpdate_0000141C$BurstDirectCall.Invoke(self, state);
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0007AE0F File Offset: 0x0007900F
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((DestinationEntitySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0007AE28 File Offset: 0x00079028
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void __codegen__OnUpdate$BurstManaged(IntPtr self, IntPtr state)
		{
			((DestinationEntitySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000F04 RID: 3844
		private DestinationEntitySystem.TypeHandle __TypeHandle;

		// Token: 0x0200035E RID: 862
		[BurstCompile]
		[CompilerGenerated]
		private struct UpdateDestinationJob : IJobEntity, IJobChunk
		{
			// Token: 0x06001596 RID: 5526 RVA: 0x0007AE44 File Offset: 0x00079044
			public void Execute(ref DestinationPoint destPoint, in DestinationEntity destEntity)
			{
				if (this.TransformLookup.HasComponent(destEntity.destination))
				{
					LocalToWorld localToWorld = this.TransformLookup[destEntity.destination];
					destPoint.destination = localToWorld.Position;
					destPoint.facingDirection = (destEntity.useRotation ? localToWorld.Forward : float3.zero);
				}
			}

			// Token: 0x06001597 RID: 5527 RVA: 0x0007AEA0 File Offset: 0x000790A0
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<DestinationPoint>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<DestinationEntity>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_DestinationEntity_RO_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						ref DestinationPoint ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr, i);
						ref DestinationEntity ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationEntity>(intPtr2, i);
						this.Execute(ref ptr, in ptr2);
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
							ref DestinationPoint ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr, j);
							ref DestinationEntity ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationEntity>(intPtr2, j);
							this.Execute(ref ptr3, in ptr4);
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
						ref DestinationPoint ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr, k);
						ref DestinationEntity ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationEntity>(intPtr2, k);
						this.Execute(ref ptr5, in ptr6);
						num++;
					}
					num3 >>= 1;
				}
				num3 = chunkEnabledMask.ULong1;
				for (int l = 64; l < count; l++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						ref DestinationPoint ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr, l);
						ref DestinationEntity ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationEntity>(intPtr2, l);
						this.Execute(ref ptr7, in ptr8);
						num++;
					}
					num3 >>= 1;
				}
			}

			// Token: 0x06001598 RID: 5528 RVA: 0x0007B038 File Offset: 0x00079238
			private JobHandle __ThrowCodeGenException()
			{
				throw new Exception("This method should have been replaced by source gen.");
			}

			// Token: 0x06001599 RID: 5529 RVA: 0x0007B04F File Offset: 0x0007924F
			public void Run()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600159A RID: 5530 RVA: 0x0007B04F File Offset: 0x0007924F
			public void RunByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600159B RID: 5531 RVA: 0x0007B04F File Offset: 0x0007924F
			public void Run(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600159C RID: 5532 RVA: 0x0007B04F File Offset: 0x0007924F
			public void RunByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600159D RID: 5533 RVA: 0x0007B058 File Offset: 0x00079258
			public JobHandle Schedule(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600159E RID: 5534 RVA: 0x0007B058 File Offset: 0x00079258
			public JobHandle ScheduleByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600159F RID: 5535 RVA: 0x0007B058 File Offset: 0x00079258
			public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015A0 RID: 5536 RVA: 0x0007B058 File Offset: 0x00079258
			public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015A1 RID: 5537 RVA: 0x0007B04F File Offset: 0x0007924F
			public void Schedule()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015A2 RID: 5538 RVA: 0x0007B04F File Offset: 0x0007924F
			public void ScheduleByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015A3 RID: 5539 RVA: 0x0007B04F File Offset: 0x0007924F
			public void Schedule(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015A4 RID: 5540 RVA: 0x0007B04F File Offset: 0x0007924F
			public void ScheduleByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015A5 RID: 5541 RVA: 0x0007B058 File Offset: 0x00079258
			public JobHandle ScheduleParallel(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015A6 RID: 5542 RVA: 0x0007B058 File Offset: 0x00079258
			public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015A7 RID: 5543 RVA: 0x0007B058 File Offset: 0x00079258
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015A8 RID: 5544 RVA: 0x0007B058 File Offset: 0x00079258
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015A9 RID: 5545 RVA: 0x0007B058 File Offset: 0x00079258
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015AA RID: 5546 RVA: 0x0007B058 File Offset: 0x00079258
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015AB RID: 5547 RVA: 0x0007B04F File Offset: 0x0007924F
			public void ScheduleParallel()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015AC RID: 5548 RVA: 0x0007B04F File Offset: 0x0007924F
			public void ScheduleParallelByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015AD RID: 5549 RVA: 0x0007B04F File Offset: 0x0007924F
			public void ScheduleParallel(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015AE RID: 5550 RVA: 0x0007B04F File Offset: 0x0007924F
			public void ScheduleParallelByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015AF RID: 5551 RVA: 0x0007B060 File Offset: 0x00079260
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000F05 RID: 3845
			[ReadOnly]
			public ComponentLookup<LocalToWorld> TransformLookup;

			// Token: 0x04000F06 RID: 3846
			private DestinationEntitySystem.UpdateDestinationJob.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x0200035F RID: 863
			public struct InternalCompilerQueryAndHandleData
			{
				// Token: 0x060015B0 RID: 5552 RVA: 0x0007B070 File Offset: 0x00079270
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void __AssignQueries(ref SystemState state)
				{
					EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
					this.DefaultQuery = entityQueryBuilder.WithAll<DestinationEntity>().WithAllRW<DestinationPoint>().Build(ref state);
					entityQueryBuilder.Reset();
					entityQueryBuilder.Dispose();
				}

				// Token: 0x060015B1 RID: 5553 RVA: 0x0007B0B7 File Offset: 0x000792B7
				public void Init(ref SystemState state, bool assignDefaultQuery)
				{
					if (assignDefaultQuery)
					{
						this.__AssignQueries(ref state);
					}
					this.__TypeHandle.__AssignHandles(ref state);
				}

				// Token: 0x060015B2 RID: 5554 RVA: 0x0007B0CF File Offset: 0x000792CF
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Run(ref DestinationEntitySystem.UpdateDestinationJob job, EntityQuery query)
				{
					job.__TypeHandle = this.__TypeHandle;
					(ref job).RunByRef<DestinationEntitySystem.UpdateDestinationJob>(query);
				}

				// Token: 0x060015B3 RID: 5555 RVA: 0x0007B0E4 File Offset: 0x000792E4
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle Schedule(ref DestinationEntitySystem.UpdateDestinationJob job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleByRef<DestinationEntitySystem.UpdateDestinationJob>(query, dependency);
				}

				// Token: 0x060015B4 RID: 5556 RVA: 0x0007B0FA File Offset: 0x000792FA
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle ScheduleParallel(ref DestinationEntitySystem.UpdateDestinationJob job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleParallelByRef<DestinationEntitySystem.UpdateDestinationJob>(query, dependency);
				}

				// Token: 0x060015B5 RID: 5557 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void UpdateBaseEntityIndexArray(ref DestinationEntitySystem.UpdateDestinationJob job, EntityQuery query, ref SystemState state)
				{
				}

				// Token: 0x060015B6 RID: 5558 RVA: 0x000743B8 File Offset: 0x000725B8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle UpdateBaseEntityIndexArray(ref DestinationEntitySystem.UpdateDestinationJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
				{
					return dependency;
				}

				// Token: 0x060015B7 RID: 5559 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void AssignEntityManager(ref DestinationEntitySystem.UpdateDestinationJob job, EntityManager entityManager)
				{
				}

				// Token: 0x04000F07 RID: 3847
				public DestinationEntitySystem.UpdateDestinationJob.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

				// Token: 0x04000F08 RID: 3848
				public EntityQuery DefaultQuery;

				// Token: 0x02000360 RID: 864
				public struct TypeHandle
				{
					// Token: 0x060015B8 RID: 5560 RVA: 0x0007B110 File Offset: 0x00079310
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public void __AssignHandles(ref SystemState state)
					{
						this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle = state.GetComponentTypeHandle<DestinationPoint>(false);
						this.__Pathfinding_ECS_DestinationEntity_RO_ComponentTypeHandle = state.GetComponentTypeHandle<DestinationEntity>(true);
					}

					// Token: 0x060015B9 RID: 5561 RVA: 0x0007B12C File Offset: 0x0007932C
					public void Update(ref SystemState state)
					{
						this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_DestinationEntity_RO_ComponentTypeHandle.Update(ref state);
					}

					// Token: 0x04000F09 RID: 3849
					public ComponentTypeHandle<DestinationPoint> __Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle;

					// Token: 0x04000F0A RID: 3850
					[ReadOnly]
					public ComponentTypeHandle<DestinationEntity> __Pathfinding_ECS_DestinationEntity_RO_ComponentTypeHandle;
				}
			}

			// Token: 0x02000361 RID: 865
			public struct InternalCompiler
			{
				// Token: 0x060015BA RID: 5562 RVA: 0x00003788 File Offset: 0x00001988
				[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void CheckForErrors(int scheduleType)
				{
				}
			}
		}

		// Token: 0x02000362 RID: 866
		private struct TypeHandle
		{
			// Token: 0x060015BB RID: 5563 RVA: 0x0007B146 File Offset: 0x00079346
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Unity_Transforms_LocalToWorld_RO_ComponentLookup = state.GetComponentLookup<LocalToWorld>(true);
				this.__Pathfinding_ECS_DestinationEntitySystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
			}

			// Token: 0x04000F0B RID: 3851
			[ReadOnly]
			public ComponentLookup<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentLookup;

			// Token: 0x04000F0C RID: 3852
			public DestinationEntitySystem.UpdateDestinationJob.InternalCompilerQueryAndHandleData __Pathfinding_ECS_DestinationEntitySystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle;
		}

		// Token: 0x02000363 RID: 867
		// (Invoke) Token: 0x060015BD RID: 5565
		internal delegate void __codegen__OnUpdate_0000141C$PostfixBurstDelegate(IntPtr self, IntPtr state);

		// Token: 0x02000364 RID: 868
		internal static class __codegen__OnUpdate_0000141C$BurstDirectCall
		{
			// Token: 0x060015C0 RID: 5568 RVA: 0x0007B162 File Offset: 0x00079362
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (DestinationEntitySystem.__codegen__OnUpdate_0000141C$BurstDirectCall.Pointer == 0)
				{
					DestinationEntitySystem.__codegen__OnUpdate_0000141C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(DestinationEntitySystem.__codegen__OnUpdate_0000141C$BurstDirectCall.DeferredCompilation, methodof(DestinationEntitySystem.__codegen__OnUpdate$BurstManaged(IntPtr, IntPtr)).MethodHandle, typeof(DestinationEntitySystem.__codegen__OnUpdate_0000141C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = DestinationEntitySystem.__codegen__OnUpdate_0000141C$BurstDirectCall.Pointer;
			}

			// Token: 0x060015C1 RID: 5569 RVA: 0x0007B190 File Offset: 0x00079390
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				DestinationEntitySystem.__codegen__OnUpdate_0000141C$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060015C2 RID: 5570 RVA: 0x0007B1A8 File Offset: 0x000793A8
			public static void Constructor()
			{
				DestinationEntitySystem.__codegen__OnUpdate_0000141C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(DestinationEntitySystem.__codegen__OnUpdate(IntPtr, IntPtr)).MethodHandle);
			}

			// Token: 0x060015C3 RID: 5571 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x060015C4 RID: 5572 RVA: 0x0007B1B9 File Offset: 0x000793B9
			// Note: this type is marked as 'beforefieldinit'.
			static __codegen__OnUpdate_0000141C$BurstDirectCall()
			{
				DestinationEntitySystem.__codegen__OnUpdate_0000141C$BurstDirectCall.Constructor();
			}

			// Token: 0x060015C5 RID: 5573 RVA: 0x0007B1C0 File Offset: 0x000793C0
			public static void Invoke(IntPtr self, IntPtr state)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = DestinationEntitySystem.__codegen__OnUpdate_0000141C$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.IntPtr,System.IntPtr), self, state, functionPointer);
						return;
					}
				}
				DestinationEntitySystem.__codegen__OnUpdate$BurstManaged(self, state);
			}

			// Token: 0x04000F0D RID: 3853
			private static IntPtr Pointer;

			// Token: 0x04000F0E RID: 3854
			private static IntPtr DeferredCompilation;
		}
	}
}
