using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities.Internal;
using Unity.Mathematics;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000268 RID: 616
	[RequireMatchingQueriesForUpdate]
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	[CompilerGenerated]
	internal class RetainBlobAssetSystem : SystemBase
	{
		// Token: 0x060010A6 RID: 4262 RVA: 0x000470BE File Offset: 0x000452BE
		[Preserve]
		protected override void OnUpdate()
		{
			this.RetainBlobAssetSystem_2062D824_LambdaJob_0_Execute();
			this.RetainBlobAssetSystem_2062D824_LambdaJob_1_Execute();
			this.RetainBlobAssetSystem_2062D824_LambdaJob_2_Execute();
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x000470D2 File Offset: 0x000452D2
		[Preserve]
		protected override void OnDestroy()
		{
			this.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Execute();
			this.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Execute();
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x000470E0 File Offset: 0x000452E0
		private void RetainBlobAssetSystem_2062D824_LambdaJob_0_Execute()
		{
			this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(base.CheckedStateRef);
			this.__TypeHandle.__Unity_Entities_RetainBlobAssets_RW_ComponentTypeHandle.Update(base.CheckedStateRef);
			RetainBlobAssetSystem.RetainBlobAssetSystem_2062D824_LambdaJob_0_Job retainBlobAssetSystem_2062D824_LambdaJob_0_Job = default(RetainBlobAssetSystem.RetainBlobAssetSystem_2062D824_LambdaJob_0_Job);
			retainBlobAssetSystem_2062D824_LambdaJob_0_Job.__this = this;
			RetainBlobAssetSystem.RetainBlobAssetSystem_2062D824_LambdaJob_0_Job retainBlobAssetSystem_2062D824_LambdaJob_0_Job2 = retainBlobAssetSystem_2062D824_LambdaJob_0_Job;
			if (!this.__query_975679986_0.IsEmptyIgnoreFilter)
			{
				base.CheckedStateRef.CompleteDependency();
				retainBlobAssetSystem_2062D824_LambdaJob_0_Job2.RunWithStructuralChange(this.__query_975679986_0);
			}
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00047150 File Offset: 0x00045350
		private void RetainBlobAssetSystem_2062D824_LambdaJob_1_Execute()
		{
			this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(base.CheckedStateRef);
			this.__TypeHandle.__Unity_Entities_RetainBlobAssets_RW_ComponentTypeHandle.Update(base.CheckedStateRef);
			this.__TypeHandle.__Unity_Entities_RetainBlobAssetBatchPtr_RW_ComponentTypeHandle.Update(base.CheckedStateRef);
			RetainBlobAssetSystem.RetainBlobAssetSystem_2062D824_LambdaJob_1_Job retainBlobAssetSystem_2062D824_LambdaJob_1_Job = default(RetainBlobAssetSystem.RetainBlobAssetSystem_2062D824_LambdaJob_1_Job);
			retainBlobAssetSystem_2062D824_LambdaJob_1_Job.__this = this;
			RetainBlobAssetSystem.RetainBlobAssetSystem_2062D824_LambdaJob_1_Job retainBlobAssetSystem_2062D824_LambdaJob_1_Job2 = retainBlobAssetSystem_2062D824_LambdaJob_1_Job;
			if (!this.__query_975679986_1.IsEmptyIgnoreFilter)
			{
				base.CheckedStateRef.CompleteDependency();
				retainBlobAssetSystem_2062D824_LambdaJob_1_Job2.RunWithStructuralChange(this.__query_975679986_1);
			}
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x000471D8 File Offset: 0x000453D8
		private void RetainBlobAssetSystem_2062D824_LambdaJob_2_Execute()
		{
			this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(base.CheckedStateRef);
			this.__TypeHandle.__Unity_Entities_RetainBlobAssets_RW_ComponentTypeHandle.Update(base.CheckedStateRef);
			this.__TypeHandle.__Unity_Entities_RetainBlobAssetPtr_RW_ComponentTypeHandle.Update(base.CheckedStateRef);
			RetainBlobAssetSystem.RetainBlobAssetSystem_2062D824_LambdaJob_2_Job retainBlobAssetSystem_2062D824_LambdaJob_2_Job = default(RetainBlobAssetSystem.RetainBlobAssetSystem_2062D824_LambdaJob_2_Job);
			retainBlobAssetSystem_2062D824_LambdaJob_2_Job.__this = this;
			RetainBlobAssetSystem.RetainBlobAssetSystem_2062D824_LambdaJob_2_Job retainBlobAssetSystem_2062D824_LambdaJob_2_Job2 = retainBlobAssetSystem_2062D824_LambdaJob_2_Job;
			if (!this.__query_975679986_2.IsEmptyIgnoreFilter)
			{
				base.CheckedStateRef.CompleteDependency();
				retainBlobAssetSystem_2062D824_LambdaJob_2_Job2.RunWithStructuralChange(this.__query_975679986_2);
			}
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00047260 File Offset: 0x00045460
		private void RetainBlobAssetSystem_76247ABD_LambdaJob_3_Execute()
		{
			this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(base.CheckedStateRef);
			this.__TypeHandle.__Unity_Entities_RetainBlobAssets_RW_ComponentTypeHandle.Update(base.CheckedStateRef);
			this.__TypeHandle.__Unity_Entities_RetainBlobAssetBatchPtr_RW_ComponentTypeHandle.Update(base.CheckedStateRef);
			RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job retainBlobAssetSystem_76247ABD_LambdaJob_3_Job = default(RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job);
			retainBlobAssetSystem_76247ABD_LambdaJob_3_Job.__eTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
			retainBlobAssetSystem_76247ABD_LambdaJob_3_Job.__retainTypeHandle = this.__TypeHandle.__Unity_Entities_RetainBlobAssets_RW_ComponentTypeHandle;
			retainBlobAssetSystem_76247ABD_LambdaJob_3_Job.__retainPtrTypeHandle = this.__TypeHandle.__Unity_Entities_RetainBlobAssetBatchPtr_RW_ComponentTypeHandle;
			RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job retainBlobAssetSystem_76247ABD_LambdaJob_3_Job2 = retainBlobAssetSystem_76247ABD_LambdaJob_3_Job;
			if (!this.__query_975679986_3.IsEmptyIgnoreFilter)
			{
				base.CheckedStateRef.CompleteDependency();
				IntPtr intPtr = InternalCompilerInterface.AddressOf<RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job>(ref retainBlobAssetSystem_76247ABD_LambdaJob_3_Job2);
				RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem(ref this.__query_975679986_3, intPtr);
			}
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0004731C File Offset: 0x0004551C
		private void RetainBlobAssetSystem_76247ABD_LambdaJob_4_Execute()
		{
			this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(base.CheckedStateRef);
			this.__TypeHandle.__Unity_Entities_RetainBlobAssets_RW_ComponentTypeHandle.Update(base.CheckedStateRef);
			this.__TypeHandle.__Unity_Entities_RetainBlobAssetPtr_RW_ComponentTypeHandle.Update(base.CheckedStateRef);
			RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job retainBlobAssetSystem_76247ABD_LambdaJob_4_Job = default(RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job);
			retainBlobAssetSystem_76247ABD_LambdaJob_4_Job.__eTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
			retainBlobAssetSystem_76247ABD_LambdaJob_4_Job.__retainTypeHandle = this.__TypeHandle.__Unity_Entities_RetainBlobAssets_RW_ComponentTypeHandle;
			retainBlobAssetSystem_76247ABD_LambdaJob_4_Job.__retainPtrTypeHandle = this.__TypeHandle.__Unity_Entities_RetainBlobAssetPtr_RW_ComponentTypeHandle;
			RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job retainBlobAssetSystem_76247ABD_LambdaJob_4_Job2 = retainBlobAssetSystem_76247ABD_LambdaJob_4_Job;
			if (!this.__query_975679986_4.IsEmptyIgnoreFilter)
			{
				base.CheckedStateRef.CompleteDependency();
				IntPtr intPtr = InternalCompilerInterface.AddressOf<RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job>(ref retainBlobAssetSystem_76247ABD_LambdaJob_4_Job2);
				RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem(ref this.__query_975679986_4, intPtr);
			}
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x000473D8 File Offset: 0x000455D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			this.__query_975679986_0 = entityQueryBuilder.WithNone<RetainBlobAssetBatchPtr>().WithAllRW<BlobAssetOwner>().WithAllRW<RetainBlobAssets>()
				.Build(ref state);
			entityQueryBuilder.Reset();
			this.__query_975679986_1 = entityQueryBuilder.WithNone<BlobAssetOwner>().WithAllRW<RetainBlobAssets>().WithAllRW<RetainBlobAssetBatchPtr>()
				.Build(ref state);
			entityQueryBuilder.Reset();
			this.__query_975679986_2 = entityQueryBuilder.WithNone<BlobAssetOwner>().WithAllRW<RetainBlobAssets>().WithAllRW<RetainBlobAssetPtr>()
				.Build(ref state);
			entityQueryBuilder.Reset();
			this.__query_975679986_3 = entityQueryBuilder.WithAllRW<RetainBlobAssets>().WithAllRW<RetainBlobAssetBatchPtr>().Build(ref state);
			entityQueryBuilder.Reset();
			this.__query_975679986_4 = entityQueryBuilder.WithAllRW<RetainBlobAssets>().WithAllRW<RetainBlobAssetPtr>().Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x000474CF File Offset: 0x000456CF
		protected override void OnCreateForCompiler()
		{
			base.OnCreateForCompiler();
			this.__AssignQueries(base.CheckedStateRef);
			this.__TypeHandle.__AssignHandles(base.CheckedStateRef);
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x00016D44 File Offset: 0x00014F44
		[Preserve]
		public RetainBlobAssetSystem()
		{
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x000475DA File Offset: 0x000457DA
		public static void Initialize$RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job_RunWithoutJobSystem_00000F19$BurstDirectCall()
		{
			RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem_00000F19$BurstDirectCall.Initialize();
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x000475E1 File Offset: 0x000457E1
		public static void Initialize$RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job_RunWithoutJobSystem_00000F1D$BurstDirectCall()
		{
			RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem_00000F1D$BurstDirectCall.Initialize();
		}

		// Token: 0x0400065F RID: 1631
		private RetainBlobAssetSystem.TypeHandle __TypeHandle;

		// Token: 0x04000660 RID: 1632
		private EntityQuery __query_975679986_0;

		// Token: 0x04000661 RID: 1633
		private EntityQuery __query_975679986_1;

		// Token: 0x04000662 RID: 1634
		private EntityQuery __query_975679986_2;

		// Token: 0x04000663 RID: 1635
		private EntityQuery __query_975679986_3;

		// Token: 0x04000664 RID: 1636
		private EntityQuery __query_975679986_4;

		// Token: 0x02000269 RID: 617
		private struct RetainBlobAssetSystem_2062D824_LambdaJob_0_Job
		{
			// Token: 0x060010B5 RID: 4277 RVA: 0x000475E8 File Offset: 0x000457E8
			private void OriginalLambdaBody(Entity e, BlobAssetOwner blobOwner, ref RetainBlobAssets retain)
			{
				BlobAssetBatch.Retain(blobOwner.BlobAssetBatchPtr);
				this.__this.EntityManager.AddComponentData<RetainBlobAssetBatchPtr>(e, new RetainBlobAssetBatchPtr
				{
					BlobAssetBatchPtr = blobOwner.BlobAssetBatchPtr
				});
			}

			// Token: 0x060010B6 RID: 4278 RVA: 0x0004762C File Offset: 0x0004582C
			public void RunWithStructuralChange(EntityQuery query)
			{
				EntityQueryMask entityQueryMask = query.GetEntityQueryMask();
				InternalGatherEntitiesResult internalGatherEntitiesResult;
				InternalCompilerInterface.UnsafeCreateGatherEntitiesResult(ref query, out internalGatherEntitiesResult);
				TypeIndex typeIndex = TypeManager.GetTypeIndex<RetainBlobAssets>();
				try
				{
					int entityCount = internalGatherEntitiesResult.EntityCount;
					for (int num = 0; num != entityCount; num++)
					{
						Entity entity = InternalCompilerInterface.UnsafeGetEntityFromGatheredEntities(ref internalGatherEntitiesResult, num);
						if (entityQueryMask.MatchesIgnoreFilter(entity))
						{
							BlobAssetOwner sharedComponentManaged = this.__this.EntityManager.GetSharedComponentManaged<BlobAssetOwner>(entity);
							RetainBlobAssets retainBlobAssets;
							RetainBlobAssets componentData = InternalCompilerInterface.GetComponentData<RetainBlobAssets>(this.__this.EntityManager, entity, typeIndex, out retainBlobAssets);
							this.OriginalLambdaBody(entity, sharedComponentManaged, ref componentData);
							InternalCompilerInterface.WriteComponentData<RetainBlobAssets>(this.__this.EntityManager, entity, typeIndex, ref componentData, ref retainBlobAssets);
						}
					}
				}
				finally
				{
					InternalCompilerInterface.UnsafeReleaseGatheredEntities(ref query, ref internalGatherEntitiesResult);
				}
			}

			// Token: 0x04000665 RID: 1637
			public RetainBlobAssetSystem __this;
		}

		// Token: 0x0200026A RID: 618
		private struct RetainBlobAssetSystem_2062D824_LambdaJob_1_Job
		{
			// Token: 0x060010B7 RID: 4279 RVA: 0x000476EC File Offset: 0x000458EC
			private void OriginalLambdaBody(Entity e, ref RetainBlobAssets retain, ref RetainBlobAssetBatchPtr retainPtr)
			{
				int framesToRetainBlobAssets = retain.FramesToRetainBlobAssets;
				retain.FramesToRetainBlobAssets = framesToRetainBlobAssets - 1;
				if (framesToRetainBlobAssets <= 0)
				{
					BlobAssetBatch.Release(retainPtr.BlobAssetBatchPtr);
					this.__this.EntityManager.RemoveComponent<RetainBlobAssets>(e);
					this.__this.EntityManager.RemoveComponent<RetainBlobAssetBatchPtr>(e);
				}
			}

			// Token: 0x060010B8 RID: 4280 RVA: 0x00047740 File Offset: 0x00045940
			public void RunWithStructuralChange(EntityQuery query)
			{
				EntityQueryMask entityQueryMask = query.GetEntityQueryMask();
				InternalGatherEntitiesResult internalGatherEntitiesResult;
				InternalCompilerInterface.UnsafeCreateGatherEntitiesResult(ref query, out internalGatherEntitiesResult);
				TypeIndex typeIndex = TypeManager.GetTypeIndex<RetainBlobAssets>();
				TypeIndex typeIndex2 = TypeManager.GetTypeIndex<RetainBlobAssetBatchPtr>();
				try
				{
					int entityCount = internalGatherEntitiesResult.EntityCount;
					for (int num = 0; num != entityCount; num++)
					{
						Entity entity = InternalCompilerInterface.UnsafeGetEntityFromGatheredEntities(ref internalGatherEntitiesResult, num);
						if (entityQueryMask.MatchesIgnoreFilter(entity))
						{
							RetainBlobAssets retainBlobAssets;
							RetainBlobAssets componentData = InternalCompilerInterface.GetComponentData<RetainBlobAssets>(this.__this.EntityManager, entity, typeIndex, out retainBlobAssets);
							RetainBlobAssetBatchPtr retainBlobAssetBatchPtr;
							RetainBlobAssetBatchPtr componentData2 = InternalCompilerInterface.GetComponentData<RetainBlobAssetBatchPtr>(this.__this.EntityManager, entity, typeIndex2, out retainBlobAssetBatchPtr);
							this.OriginalLambdaBody(entity, ref componentData, ref componentData2);
							InternalCompilerInterface.WriteComponentData<RetainBlobAssets>(this.__this.EntityManager, entity, typeIndex, ref componentData, ref retainBlobAssets);
							InternalCompilerInterface.WriteComponentData<RetainBlobAssetBatchPtr>(this.__this.EntityManager, entity, typeIndex2, ref componentData2, ref retainBlobAssetBatchPtr);
						}
					}
				}
				finally
				{
					InternalCompilerInterface.UnsafeReleaseGatheredEntities(ref query, ref internalGatherEntitiesResult);
				}
			}

			// Token: 0x04000666 RID: 1638
			public RetainBlobAssetSystem __this;
		}

		// Token: 0x0200026B RID: 619
		private struct RetainBlobAssetSystem_2062D824_LambdaJob_2_Job
		{
			// Token: 0x060010B9 RID: 4281 RVA: 0x00047824 File Offset: 0x00045A24
			private unsafe void OriginalLambdaBody(Entity e, ref RetainBlobAssets retain, ref RetainBlobAssetPtr retainPtr)
			{
				int framesToRetainBlobAssets = retain.FramesToRetainBlobAssets;
				retain.FramesToRetainBlobAssets = framesToRetainBlobAssets - 1;
				if (framesToRetainBlobAssets <= 0)
				{
					retainPtr.BlobAsset->Invalidate();
					Memory.Unmanaged.Free<BlobAssetHeader>(retainPtr.BlobAsset, Allocator.Persistent);
					this.__this.EntityManager.RemoveComponent<RetainBlobAssets>(e);
					this.__this.EntityManager.RemoveComponent<RetainBlobAssetPtr>(e);
				}
			}

			// Token: 0x060010BA RID: 4282 RVA: 0x00047888 File Offset: 0x00045A88
			public void RunWithStructuralChange(EntityQuery query)
			{
				EntityQueryMask entityQueryMask = query.GetEntityQueryMask();
				InternalGatherEntitiesResult internalGatherEntitiesResult;
				InternalCompilerInterface.UnsafeCreateGatherEntitiesResult(ref query, out internalGatherEntitiesResult);
				TypeIndex typeIndex = TypeManager.GetTypeIndex<RetainBlobAssets>();
				TypeIndex typeIndex2 = TypeManager.GetTypeIndex<RetainBlobAssetPtr>();
				try
				{
					int entityCount = internalGatherEntitiesResult.EntityCount;
					for (int num = 0; num != entityCount; num++)
					{
						Entity entity = InternalCompilerInterface.UnsafeGetEntityFromGatheredEntities(ref internalGatherEntitiesResult, num);
						if (entityQueryMask.MatchesIgnoreFilter(entity))
						{
							RetainBlobAssets retainBlobAssets;
							RetainBlobAssets componentData = InternalCompilerInterface.GetComponentData<RetainBlobAssets>(this.__this.EntityManager, entity, typeIndex, out retainBlobAssets);
							RetainBlobAssetPtr retainBlobAssetPtr;
							RetainBlobAssetPtr componentData2 = InternalCompilerInterface.GetComponentData<RetainBlobAssetPtr>(this.__this.EntityManager, entity, typeIndex2, out retainBlobAssetPtr);
							this.OriginalLambdaBody(entity, ref componentData, ref componentData2);
							InternalCompilerInterface.WriteComponentData<RetainBlobAssets>(this.__this.EntityManager, entity, typeIndex, ref componentData, ref retainBlobAssets);
							InternalCompilerInterface.WriteComponentData<RetainBlobAssetPtr>(this.__this.EntityManager, entity, typeIndex2, ref componentData2, ref retainBlobAssetPtr);
						}
					}
				}
				finally
				{
					InternalCompilerInterface.UnsafeReleaseGatheredEntities(ref query, ref internalGatherEntitiesResult);
				}
			}

			// Token: 0x04000667 RID: 1639
			public RetainBlobAssetSystem __this;
		}

		// Token: 0x0200026C RID: 620
		[NoAlias]
		[BurstCompile]
		private struct RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job : IJobChunk
		{
			// Token: 0x060010BB RID: 4283 RVA: 0x0004796C File Offset: 0x00045B6C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void OriginalLambdaBody(Entity e, [NoAlias] ref RetainBlobAssets retain, [NoAlias] ref RetainBlobAssetBatchPtr retainPtr)
			{
				BlobAssetBatch.Release(retainPtr.BlobAssetBatchPtr);
			}

			// Token: 0x060010BC RID: 4284 RVA: 0x0004797C File Offset: 0x00045B7C
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int batchIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, this.__eTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<RetainBlobAssets>(chunk, ref this.__retainTypeHandle);
				IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<RetainBlobAssetBatchPtr>(chunk, ref this.__retainPtrTypeHandle);
				int count = chunk.Count;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						this.OriginalLambdaBody(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, i), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssets>(intPtr2, i), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssetBatchPtr>(intPtr3, i));
					}
					return;
				}
				if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
				{
					int j = 0;
					int num = 0;
					while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num, out j, out num))
					{
						while (j < num)
						{
							this.OriginalLambdaBody(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, j), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssets>(intPtr2, j), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssetBatchPtr>(intPtr3, j));
							j++;
						}
					}
					return;
				}
				ulong num2 = chunkEnabledMask.ULong0;
				int num3 = math.min(64, count);
				for (int k = 0; k < num3; k++)
				{
					if ((num2 & 1UL) != 0UL)
					{
						this.OriginalLambdaBody(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, k), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssets>(intPtr2, k), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssetBatchPtr>(intPtr3, k));
					}
					num2 >>= 1;
				}
				num2 = chunkEnabledMask.ULong1;
				for (int l = 64; l < count; l++)
				{
					if ((num2 & 1UL) != 0UL)
					{
						this.OriginalLambdaBody(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, l), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssets>(intPtr2, l), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssetBatchPtr>(intPtr3, l));
					}
					num2 >>= 1;
				}
			}

			// Token: 0x060010BD RID: 4285 RVA: 0x00047B0A File Offset: 0x00045D0A
			[BurstCompile]
			public static void RunWithoutJobSystem(ref EntityQuery query, IntPtr jobPtr)
			{
				RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem_00000F19$BurstDirectCall.Invoke(ref query, jobPtr);
			}

			// Token: 0x060010BE RID: 4286 RVA: 0x00047B13 File Offset: 0x00045D13
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x060010BF RID: 4287 RVA: 0x00047B20 File Offset: 0x00045D20
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void RunWithoutJobSystem$BurstManaged(ref EntityQuery query, IntPtr jobPtr)
			{
				InternalCompilerInterface.JobChunkInterface.RunWithoutJobsInternal<RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job>(InternalCompilerInterface.UnsafeAsRef<RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job>(jobPtr), ref query);
			}

			// Token: 0x04000668 RID: 1640
			[ReadOnly]
			public EntityTypeHandle __eTypeHandle;

			// Token: 0x04000669 RID: 1641
			public ComponentTypeHandle<RetainBlobAssets> __retainTypeHandle;

			// Token: 0x0400066A RID: 1642
			public ComponentTypeHandle<RetainBlobAssetBatchPtr> __retainPtrTypeHandle;

			// Token: 0x0200026D RID: 621
			// (Invoke) Token: 0x060010C1 RID: 4289
			internal delegate void RunWithoutJobSystem_00000F19$PostfixBurstDelegate(ref EntityQuery query, IntPtr jobPtr);

			// Token: 0x0200026E RID: 622
			internal static class RunWithoutJobSystem_00000F19$BurstDirectCall
			{
				// Token: 0x060010C4 RID: 4292 RVA: 0x00047B2E File Offset: 0x00045D2E
				[BurstDiscard]
				private static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem_00000F19$BurstDirectCall.Pointer == 0)
					{
						RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem_00000F19$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem_00000F19$BurstDirectCall.DeferredCompilation, methodof(RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem$BurstManaged(ref EntityQuery, IntPtr)).MethodHandle, typeof(RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem_00000F19$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem_00000F19$BurstDirectCall.Pointer;
				}

				// Token: 0x060010C5 RID: 4293 RVA: 0x00047B5C File Offset: 0x00045D5C
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem_00000F19$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x060010C6 RID: 4294 RVA: 0x00047B74 File Offset: 0x00045D74
				public static void Constructor()
				{
					RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem_00000F19$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem(ref EntityQuery, IntPtr)).MethodHandle);
				}

				// Token: 0x060010C7 RID: 4295 RVA: 0x00002287 File Offset: 0x00000487
				public static void Initialize()
				{
				}

				// Token: 0x060010C8 RID: 4296 RVA: 0x00047B85 File Offset: 0x00045D85
				// Note: this type is marked as 'beforefieldinit'.
				static RunWithoutJobSystem_00000F19$BurstDirectCall()
				{
					RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem_00000F19$BurstDirectCall.Constructor();
				}

				// Token: 0x060010C9 RID: 4297 RVA: 0x00047B8C File Offset: 0x00045D8C
				public static void Invoke(ref EntityQuery query, IntPtr jobPtr)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem_00000F19$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(Unity.Entities.EntityQuery&,System.IntPtr), ref query, jobPtr, functionPointer);
							return;
						}
					}
					RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job.RunWithoutJobSystem$BurstManaged(ref query, jobPtr);
				}

				// Token: 0x0400066B RID: 1643
				private static IntPtr Pointer;

				// Token: 0x0400066C RID: 1644
				private static IntPtr DeferredCompilation;
			}
		}

		// Token: 0x0200026F RID: 623
		[NoAlias]
		[BurstCompile]
		private struct RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job : IJobChunk
		{
			// Token: 0x060010CA RID: 4298 RVA: 0x00047BBF File Offset: 0x00045DBF
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private unsafe void OriginalLambdaBody(Entity e, [NoAlias] ref RetainBlobAssets retain, [NoAlias] ref RetainBlobAssetPtr retainPtr)
			{
				retainPtr.BlobAsset->Invalidate();
				Memory.Unmanaged.Free<BlobAssetHeader>(retainPtr.BlobAsset, Allocator.Persistent);
			}

			// Token: 0x060010CB RID: 4299 RVA: 0x00047BE0 File Offset: 0x00045DE0
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int batchIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, this.__eTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<RetainBlobAssets>(chunk, ref this.__retainTypeHandle);
				IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<RetainBlobAssetPtr>(chunk, ref this.__retainPtrTypeHandle);
				int count = chunk.Count;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						this.OriginalLambdaBody(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, i), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssets>(intPtr2, i), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssetPtr>(intPtr3, i));
					}
					return;
				}
				if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
				{
					int j = 0;
					int num = 0;
					while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num, out j, out num))
					{
						while (j < num)
						{
							this.OriginalLambdaBody(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, j), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssets>(intPtr2, j), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssetPtr>(intPtr3, j));
							j++;
						}
					}
					return;
				}
				ulong num2 = chunkEnabledMask.ULong0;
				int num3 = math.min(64, count);
				for (int k = 0; k < num3; k++)
				{
					if ((num2 & 1UL) != 0UL)
					{
						this.OriginalLambdaBody(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, k), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssets>(intPtr2, k), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssetPtr>(intPtr3, k));
					}
					num2 >>= 1;
				}
				num2 = chunkEnabledMask.ULong1;
				for (int l = 64; l < count; l++)
				{
					if ((num2 & 1UL) != 0UL)
					{
						this.OriginalLambdaBody(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, l), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssets>(intPtr2, l), InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RetainBlobAssetPtr>(intPtr3, l));
					}
					num2 >>= 1;
				}
			}

			// Token: 0x060010CC RID: 4300 RVA: 0x00047D6E File Offset: 0x00045F6E
			[BurstCompile]
			public static void RunWithoutJobSystem(ref EntityQuery query, IntPtr jobPtr)
			{
				RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem_00000F1D$BurstDirectCall.Invoke(ref query, jobPtr);
			}

			// Token: 0x060010CD RID: 4301 RVA: 0x00047D77 File Offset: 0x00045F77
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x060010CE RID: 4302 RVA: 0x00047D84 File Offset: 0x00045F84
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void RunWithoutJobSystem$BurstManaged(ref EntityQuery query, IntPtr jobPtr)
			{
				InternalCompilerInterface.JobChunkInterface.RunWithoutJobsInternal<RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job>(InternalCompilerInterface.UnsafeAsRef<RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job>(jobPtr), ref query);
			}

			// Token: 0x0400066D RID: 1645
			[ReadOnly]
			public EntityTypeHandle __eTypeHandle;

			// Token: 0x0400066E RID: 1646
			public ComponentTypeHandle<RetainBlobAssets> __retainTypeHandle;

			// Token: 0x0400066F RID: 1647
			public ComponentTypeHandle<RetainBlobAssetPtr> __retainPtrTypeHandle;

			// Token: 0x02000270 RID: 624
			// (Invoke) Token: 0x060010D0 RID: 4304
			internal delegate void RunWithoutJobSystem_00000F1D$PostfixBurstDelegate(ref EntityQuery query, IntPtr jobPtr);

			// Token: 0x02000271 RID: 625
			internal static class RunWithoutJobSystem_00000F1D$BurstDirectCall
			{
				// Token: 0x060010D3 RID: 4307 RVA: 0x00047D92 File Offset: 0x00045F92
				[BurstDiscard]
				private static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem_00000F1D$BurstDirectCall.Pointer == 0)
					{
						RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem_00000F1D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem_00000F1D$BurstDirectCall.DeferredCompilation, methodof(RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem$BurstManaged(ref EntityQuery, IntPtr)).MethodHandle, typeof(RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem_00000F1D$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem_00000F1D$BurstDirectCall.Pointer;
				}

				// Token: 0x060010D4 RID: 4308 RVA: 0x00047DC0 File Offset: 0x00045FC0
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem_00000F1D$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x060010D5 RID: 4309 RVA: 0x00047DD8 File Offset: 0x00045FD8
				public static void Constructor()
				{
					RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem_00000F1D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem(ref EntityQuery, IntPtr)).MethodHandle);
				}

				// Token: 0x060010D6 RID: 4310 RVA: 0x00002287 File Offset: 0x00000487
				public static void Initialize()
				{
				}

				// Token: 0x060010D7 RID: 4311 RVA: 0x00047DE9 File Offset: 0x00045FE9
				// Note: this type is marked as 'beforefieldinit'.
				static RunWithoutJobSystem_00000F1D$BurstDirectCall()
				{
					RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem_00000F1D$BurstDirectCall.Constructor();
				}

				// Token: 0x060010D8 RID: 4312 RVA: 0x00047DF0 File Offset: 0x00045FF0
				public static void Invoke(ref EntityQuery query, IntPtr jobPtr)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem_00000F1D$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(Unity.Entities.EntityQuery&,System.IntPtr), ref query, jobPtr, functionPointer);
							return;
						}
					}
					RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job.RunWithoutJobSystem$BurstManaged(ref query, jobPtr);
				}

				// Token: 0x04000670 RID: 1648
				private static IntPtr Pointer;

				// Token: 0x04000671 RID: 1649
				private static IntPtr DeferredCompilation;
			}
		}

		// Token: 0x02000272 RID: 626
		private struct TypeHandle
		{
			// Token: 0x060010D9 RID: 4313 RVA: 0x00047E23 File Offset: 0x00046023
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				this.__Unity_Entities_RetainBlobAssets_RW_ComponentTypeHandle = state.GetComponentTypeHandle<RetainBlobAssets>(false);
				this.__Unity_Entities_RetainBlobAssetBatchPtr_RW_ComponentTypeHandle = state.GetComponentTypeHandle<RetainBlobAssetBatchPtr>(false);
				this.__Unity_Entities_RetainBlobAssetPtr_RW_ComponentTypeHandle = state.GetComponentTypeHandle<RetainBlobAssetPtr>(false);
			}

			// Token: 0x04000672 RID: 1650
			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			// Token: 0x04000673 RID: 1651
			public ComponentTypeHandle<RetainBlobAssets> __Unity_Entities_RetainBlobAssets_RW_ComponentTypeHandle;

			// Token: 0x04000674 RID: 1652
			public ComponentTypeHandle<RetainBlobAssetBatchPtr> __Unity_Entities_RetainBlobAssetBatchPtr_RW_ComponentTypeHandle;

			// Token: 0x04000675 RID: 1653
			public ComponentTypeHandle<RetainBlobAssetPtr> __Unity_Entities_RetainBlobAssetPtr_RW_ComponentTypeHandle;
		}
	}
}
