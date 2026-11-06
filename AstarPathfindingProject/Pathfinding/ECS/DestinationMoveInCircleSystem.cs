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
	// Token: 0x02000365 RID: 869
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[UpdateBefore(typeof(SchedulePathSearchSystem))]
	[BurstCompile]
	[RequireMatchingQueriesForUpdate]
	[CompilerGenerated]
	public struct DestinationMoveInCircleSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x060015C6 RID: 5574 RVA: 0x0007B1F4 File Offset: 0x000793F4
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			DestinationMoveInCircleSystem.UpdateDestinationJob updateDestinationJob = default(DestinationMoveInCircleSystem.UpdateDestinationJob);
			updateDestinationJob.TransformLookup = InternalCompilerInterface.GetComponentLookup<LocalToWorld>(ref this.__TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref state);
			state.Dependency = this.__ScheduleViaJobChunkExtension_0(updateDestinationJob, this.__TypeHandle.__Pathfinding_ECS_DestinationMoveInCircleSystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, false);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0007B248 File Offset: 0x00079448
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_0(DestinationMoveInCircleSystem.UpdateDestinationJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_DestinationMoveInCircleSystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_DestinationMoveInCircleSystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_DestinationMoveInCircleSystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_DestinationMoveInCircleSystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.Schedule(ref job, query, dependency);
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0007B2B4 File Offset: 0x000794B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			entityQueryBuilder.Dispose();
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x0007B2D5 File Offset: 0x000794D5
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x0007B2EA File Offset: 0x000794EA
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			DestinationMoveInCircleSystem.__codegen__OnUpdate_00001448$BurstDirectCall.Invoke(self, state);
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x0007B2F3 File Offset: 0x000794F3
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((DestinationMoveInCircleSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x0007B30C File Offset: 0x0007950C
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void __codegen__OnUpdate$BurstManaged(IntPtr self, IntPtr state)
		{
			((DestinationMoveInCircleSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000F0F RID: 3855
		private DestinationMoveInCircleSystem.TypeHandle __TypeHandle;

		// Token: 0x02000366 RID: 870
		[BurstCompile]
		[CompilerGenerated]
		private struct UpdateDestinationJob : IJobEntity, IJobChunk
		{
			// Token: 0x060015CD RID: 5581 RVA: 0x0007B328 File Offset: 0x00079528
			public void Execute(ref DestinationPoint destPoint, in LocalTransform localTransform, in MoveInCircle.DestinationMoveInCircle destCircle)
			{
				if (this.TransformLookup.HasComponent(destCircle.target))
				{
					LocalToWorld localToWorld = this.TransformLookup[destCircle.target];
					destPoint.destination = MoveInCircle.CalculateDestination(localTransform.Position, localToWorld.Position, localToWorld.Up, destCircle.radius, destCircle.offset);
					destPoint.facingDirection = float3.zero;
				}
			}

			// Token: 0x060015CE RID: 5582 RVA: 0x0007B3A4 File Offset: 0x000795A4
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<DestinationPoint>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
				IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MoveInCircle.DestinationMoveInCircle>(chunk, ref this.__TypeHandle.__Pathfinding_MoveInCircle_DestinationMoveInCircle_RO_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						ref DestinationPoint ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr, i);
						ref LocalTransform ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, i);
						ref MoveInCircle.DestinationMoveInCircle ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MoveInCircle.DestinationMoveInCircle>(intPtr3, i);
						this.Execute(ref ptr, in ptr2, in ptr3);
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
							ref DestinationPoint ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr, j);
							ref LocalTransform ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, j);
							ref MoveInCircle.DestinationMoveInCircle ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MoveInCircle.DestinationMoveInCircle>(intPtr3, j);
							this.Execute(ref ptr4, in ptr5, in ptr6);
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
						ref DestinationPoint ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr, k);
						ref LocalTransform ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, k);
						ref MoveInCircle.DestinationMoveInCircle ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MoveInCircle.DestinationMoveInCircle>(intPtr3, k);
						this.Execute(ref ptr7, in ptr8, in ptr9);
						num++;
					}
					num3 >>= 1;
				}
				num3 = chunkEnabledMask.ULong1;
				for (int l = 64; l < count; l++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						ref DestinationPoint ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr, l);
						ref LocalTransform ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, l);
						ref MoveInCircle.DestinationMoveInCircle ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MoveInCircle.DestinationMoveInCircle>(intPtr3, l);
						this.Execute(ref ptr10, in ptr11, in ptr12);
						num++;
					}
					num3 >>= 1;
				}
			}

			// Token: 0x060015CF RID: 5583 RVA: 0x0007B58C File Offset: 0x0007978C
			private JobHandle __ThrowCodeGenException()
			{
				throw new Exception("This method should have been replaced by source gen.");
			}

			// Token: 0x060015D0 RID: 5584 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void Run()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015D1 RID: 5585 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void RunByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015D2 RID: 5586 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void Run(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015D3 RID: 5587 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void RunByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015D4 RID: 5588 RVA: 0x0007B5AC File Offset: 0x000797AC
			public JobHandle Schedule(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015D5 RID: 5589 RVA: 0x0007B5AC File Offset: 0x000797AC
			public JobHandle ScheduleByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015D6 RID: 5590 RVA: 0x0007B5AC File Offset: 0x000797AC
			public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015D7 RID: 5591 RVA: 0x0007B5AC File Offset: 0x000797AC
			public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015D8 RID: 5592 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void Schedule()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015D9 RID: 5593 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void ScheduleByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015DA RID: 5594 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void Schedule(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015DB RID: 5595 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void ScheduleByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015DC RID: 5596 RVA: 0x0007B5AC File Offset: 0x000797AC
			public JobHandle ScheduleParallel(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015DD RID: 5597 RVA: 0x0007B5AC File Offset: 0x000797AC
			public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015DE RID: 5598 RVA: 0x0007B5AC File Offset: 0x000797AC
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015DF RID: 5599 RVA: 0x0007B5AC File Offset: 0x000797AC
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015E0 RID: 5600 RVA: 0x0007B5AC File Offset: 0x000797AC
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015E1 RID: 5601 RVA: 0x0007B5AC File Offset: 0x000797AC
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060015E2 RID: 5602 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void ScheduleParallel()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015E3 RID: 5603 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void ScheduleParallelByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015E4 RID: 5604 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void ScheduleParallel(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015E5 RID: 5605 RVA: 0x0007B5A3 File Offset: 0x000797A3
			public void ScheduleParallelByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060015E6 RID: 5606 RVA: 0x0007B5B4 File Offset: 0x000797B4
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000F10 RID: 3856
			[ReadOnly]
			public ComponentLookup<LocalToWorld> TransformLookup;

			// Token: 0x04000F11 RID: 3857
			private DestinationMoveInCircleSystem.UpdateDestinationJob.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x02000367 RID: 871
			public struct InternalCompilerQueryAndHandleData
			{
				// Token: 0x060015E7 RID: 5607 RVA: 0x0007B5C4 File Offset: 0x000797C4
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void __AssignQueries(ref SystemState state)
				{
					EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
					this.DefaultQuery = entityQueryBuilder.WithAll<LocalTransform>().WithAll<MoveInCircle.DestinationMoveInCircle>().WithAllRW<DestinationPoint>()
						.Build(ref state);
					entityQueryBuilder.Reset();
					entityQueryBuilder.Dispose();
				}

				// Token: 0x060015E8 RID: 5608 RVA: 0x0007B613 File Offset: 0x00079813
				public void Init(ref SystemState state, bool assignDefaultQuery)
				{
					if (assignDefaultQuery)
					{
						this.__AssignQueries(ref state);
					}
					this.__TypeHandle.__AssignHandles(ref state);
				}

				// Token: 0x060015E9 RID: 5609 RVA: 0x0007B62B File Offset: 0x0007982B
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Run(ref DestinationMoveInCircleSystem.UpdateDestinationJob job, EntityQuery query)
				{
					job.__TypeHandle = this.__TypeHandle;
					(ref job).RunByRef<DestinationMoveInCircleSystem.UpdateDestinationJob>(query);
				}

				// Token: 0x060015EA RID: 5610 RVA: 0x0007B640 File Offset: 0x00079840
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle Schedule(ref DestinationMoveInCircleSystem.UpdateDestinationJob job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleByRef<DestinationMoveInCircleSystem.UpdateDestinationJob>(query, dependency);
				}

				// Token: 0x060015EB RID: 5611 RVA: 0x0007B656 File Offset: 0x00079856
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle ScheduleParallel(ref DestinationMoveInCircleSystem.UpdateDestinationJob job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleParallelByRef<DestinationMoveInCircleSystem.UpdateDestinationJob>(query, dependency);
				}

				// Token: 0x060015EC RID: 5612 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void UpdateBaseEntityIndexArray(ref DestinationMoveInCircleSystem.UpdateDestinationJob job, EntityQuery query, ref SystemState state)
				{
				}

				// Token: 0x060015ED RID: 5613 RVA: 0x000743B8 File Offset: 0x000725B8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle UpdateBaseEntityIndexArray(ref DestinationMoveInCircleSystem.UpdateDestinationJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
				{
					return dependency;
				}

				// Token: 0x060015EE RID: 5614 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void AssignEntityManager(ref DestinationMoveInCircleSystem.UpdateDestinationJob job, EntityManager entityManager)
				{
				}

				// Token: 0x04000F12 RID: 3858
				public DestinationMoveInCircleSystem.UpdateDestinationJob.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

				// Token: 0x04000F13 RID: 3859
				public EntityQuery DefaultQuery;

				// Token: 0x02000368 RID: 872
				public struct TypeHandle
				{
					// Token: 0x060015EF RID: 5615 RVA: 0x0007B66C File Offset: 0x0007986C
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public void __AssignHandles(ref SystemState state)
					{
						this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle = state.GetComponentTypeHandle<DestinationPoint>(false);
						this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(true);
						this.__Pathfinding_MoveInCircle_DestinationMoveInCircle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MoveInCircle.DestinationMoveInCircle>(true);
					}

					// Token: 0x060015F0 RID: 5616 RVA: 0x0007B695 File Offset: 0x00079895
					public void Update(ref SystemState state)
					{
						this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle.Update(ref state);
						this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_MoveInCircle_DestinationMoveInCircle_RO_ComponentTypeHandle.Update(ref state);
					}

					// Token: 0x04000F14 RID: 3860
					public ComponentTypeHandle<DestinationPoint> __Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle;

					// Token: 0x04000F15 RID: 3861
					[ReadOnly]
					public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

					// Token: 0x04000F16 RID: 3862
					[ReadOnly]
					public ComponentTypeHandle<MoveInCircle.DestinationMoveInCircle> __Pathfinding_MoveInCircle_DestinationMoveInCircle_RO_ComponentTypeHandle;
				}
			}

			// Token: 0x02000369 RID: 873
			public struct InternalCompiler
			{
				// Token: 0x060015F1 RID: 5617 RVA: 0x00003788 File Offset: 0x00001988
				[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void CheckForErrors(int scheduleType)
				{
				}
			}
		}

		// Token: 0x0200036A RID: 874
		private struct TypeHandle
		{
			// Token: 0x060015F2 RID: 5618 RVA: 0x0007B6BB File Offset: 0x000798BB
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Unity_Transforms_LocalToWorld_RO_ComponentLookup = state.GetComponentLookup<LocalToWorld>(true);
				this.__Pathfinding_ECS_DestinationMoveInCircleSystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
			}

			// Token: 0x04000F17 RID: 3863
			[ReadOnly]
			public ComponentLookup<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentLookup;

			// Token: 0x04000F18 RID: 3864
			public DestinationMoveInCircleSystem.UpdateDestinationJob.InternalCompilerQueryAndHandleData __Pathfinding_ECS_DestinationMoveInCircleSystem_UpdateDestinationJob_WithDefaultQuery_JobEntityTypeHandle;
		}

		// Token: 0x0200036B RID: 875
		// (Invoke) Token: 0x060015F4 RID: 5620
		internal delegate void __codegen__OnUpdate_00001448$PostfixBurstDelegate(IntPtr self, IntPtr state);

		// Token: 0x0200036C RID: 876
		internal static class __codegen__OnUpdate_00001448$BurstDirectCall
		{
			// Token: 0x060015F7 RID: 5623 RVA: 0x0007B6D7 File Offset: 0x000798D7
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (DestinationMoveInCircleSystem.__codegen__OnUpdate_00001448$BurstDirectCall.Pointer == 0)
				{
					DestinationMoveInCircleSystem.__codegen__OnUpdate_00001448$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(DestinationMoveInCircleSystem.__codegen__OnUpdate_00001448$BurstDirectCall.DeferredCompilation, methodof(DestinationMoveInCircleSystem.__codegen__OnUpdate$BurstManaged(IntPtr, IntPtr)).MethodHandle, typeof(DestinationMoveInCircleSystem.__codegen__OnUpdate_00001448$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = DestinationMoveInCircleSystem.__codegen__OnUpdate_00001448$BurstDirectCall.Pointer;
			}

			// Token: 0x060015F8 RID: 5624 RVA: 0x0007B704 File Offset: 0x00079904
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				DestinationMoveInCircleSystem.__codegen__OnUpdate_00001448$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060015F9 RID: 5625 RVA: 0x0007B71C File Offset: 0x0007991C
			public static void Constructor()
			{
				DestinationMoveInCircleSystem.__codegen__OnUpdate_00001448$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(DestinationMoveInCircleSystem.__codegen__OnUpdate(IntPtr, IntPtr)).MethodHandle);
			}

			// Token: 0x060015FA RID: 5626 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x060015FB RID: 5627 RVA: 0x0007B72D File Offset: 0x0007992D
			// Note: this type is marked as 'beforefieldinit'.
			static __codegen__OnUpdate_00001448$BurstDirectCall()
			{
				DestinationMoveInCircleSystem.__codegen__OnUpdate_00001448$BurstDirectCall.Constructor();
			}

			// Token: 0x060015FC RID: 5628 RVA: 0x0007B734 File Offset: 0x00079934
			public static void Invoke(IntPtr self, IntPtr state)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = DestinationMoveInCircleSystem.__codegen__OnUpdate_00001448$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.IntPtr,System.IntPtr), self, state, functionPointer);
						return;
					}
				}
				DestinationMoveInCircleSystem.__codegen__OnUpdate$BurstManaged(self, state);
			}

			// Token: 0x04000F19 RID: 3865
			private static IntPtr Pointer;

			// Token: 0x04000F1A RID: 3866
			private static IntPtr DeferredCompilation;
		}
	}
}
