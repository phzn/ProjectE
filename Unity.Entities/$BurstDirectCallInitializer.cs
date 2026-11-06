using System;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Entities.Internal;
using Unity.Entities.Serialization;
using UnityEngine;

// Token: 0x020004DB RID: 1243
internal static class $BurstDirectCallInitializer
{
	// Token: 0x06001DDF RID: 7647 RVA: 0x0006806C File Offset: 0x0006626C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		BlobAssetOwner.__codegen__Retain_00000056$BurstDirectCall.Initialize();
		BlobAssetOwner.__codegen__Release_00000057$BurstDirectCall.Initialize();
		ChunkDataUtility.SwapComponents_000000DB$BurstDirectCall.Initialize();
		ComponentSystemSorter.SortInternal_000001AE$BurstDirectCall.Initialize();
		ComponentSystemSorter.FindConstraints_000001B2$BurstDirectCall.Initialize();
		EntityPatcher.ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall.Initialize();
		EntityPatcher.ApplyDestroyEntities_0000032E$BurstDirectCall.Initialize();
		EntityPatcher.ApplyRemoveComponents_00000330$BurstDirectCall.Initialize();
		EntityPatcher.ApplyUnmanagedEntityPatches_00000334$BurstDirectCall.Initialize();
		EntityPatcher.ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall.Initialize();
		EntityPatcher.ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall.Initialize();
		ECBInterop._mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall.Initialize();
		EntityCommandBuffer.DisposeInternal_000003D2$BurstDirectCall.Initialize();
		EntityComponentStore.EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall.Initialize();
		EntityComponentStore.SortEntityInChunk_0000063A$BurstDirectCall.Initialize();
		EntityComponentStore.GatherEntityInChunkForEntities_0000063B$BurstDirectCall.Initialize();
		EntityComponentStore.GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall.Initialize();
		EntityDataAccess.SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall.Initialize();
		EntityDataAccess.BuildSharedComponentMapForMoving_0000077D$BurstDirectCall.Initialize();
		EntityManager.RemapChunksForFilteredMove_0000089C$BurstDirectCall.Initialize();
		ChunkIterationUtility.ToArchetypeChunkList_00000A41$BurstDirectCall.Initialize();
		ChunkIterationUtility.ToChunkIndexList_00000A42$BurstDirectCall.Initialize();
		ChunkIterationUtility.GatherEntitiesWithFilter_00000A43$BurstDirectCall.Initialize();
		ChunkIterationUtility.GatherEntitiesWithoutFilter_00000A44$BurstDirectCall.Initialize();
		ChunkIterationUtility.GatherComponentDataWithFilter_00000A48$BurstDirectCall.Initialize();
		ChunkIterationUtility.GatherComponentDataWithoutFilter_00000A49$BurstDirectCall.Initialize();
		ChunkIterationUtility.CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall.Initialize();
		ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall.Initialize();
		ChunkIterationUtility.CalculateChunkCount_00000A52$BurstDirectCall.Initialize();
		ChunkIterationUtility.IsEmpty_00000A53$BurstDirectCall.Initialize();
		ChunkIterationUtility.CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall.Initialize();
		ChunkIterationUtility.CalculateBaseEntityIndexArray_00000A55$BurstDirectCall.Initialize();
		ChunkIterationUtility.CalculateEntityCount_00000A56$BurstDirectCall.Initialize();
		ChunkIterationUtility.CalculateEntityCountAndSingleton_00000A57$BurstDirectCall.Initialize();
		ChunkIterationUtility.GetEnabledMask_00000A5B$BurstDirectCall.Initialize();
		ChunkIterationUtility.SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall.Initialize();
		EnabledBitUtility.ShiftRightBurstForTests_00000A64$BurstDirectCall.Initialize();
		UnsafeCachedChunkList.Rebuild_00000C31$BurstDirectCall.Initialize();
		RetainBlobAssetSystem.Initialize$RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job_RunWithoutJobSystem_00000F19$BurstDirectCall();
		RetainBlobAssetSystem.Initialize$RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job_RunWithoutJobSystem_00000F1D$BurstDirectCall();
		ScratchpadAllocator.Try_00000F31$BurstDirectCall.Initialize();
		StructuralChange.AddComponentEntitiesBatch_00000FAA$BurstDirectCall.Initialize();
		StructuralChange.AddComponentsEntitiesBatch_00000FAB$BurstDirectCall.Initialize();
		StructuralChange.AddComponentEntity_00000FAC$BurstDirectCall.Initialize();
		StructuralChange.AddComponentsEntity_00000FAD$BurstDirectCall.Initialize();
		StructuralChange.AddComponentChunks_00000FAE$BurstDirectCall.Initialize();
		StructuralChange.AddComponentQuery_00000FAF$BurstDirectCall.Initialize();
		StructuralChange.AddComponentsQuery_00000FB0$BurstDirectCall.Initialize();
		StructuralChange.RemoveComponentEntity_00000FB1$BurstDirectCall.Initialize();
		StructuralChange.RemoveComponentsEntity_00000FB2$BurstDirectCall.Initialize();
		StructuralChange.RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall.Initialize();
		StructuralChange.RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall.Initialize();
		StructuralChange.RemoveComponentChunks_00000FB5$BurstDirectCall.Initialize();
		StructuralChange.RemoveComponentQuery_00000FB6$BurstDirectCall.Initialize();
		StructuralChange.RemoveComponentsQuery_00000FB7$BurstDirectCall.Initialize();
		StructuralChange.AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall.Initialize();
		StructuralChange.SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall.Initialize();
		StructuralChange.SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall.Initialize();
		StructuralChange.AddSharedComponentChunks_00000FBB$BurstDirectCall.Initialize();
		StructuralChange.AddSharedComponentQuery_00000FBC$BurstDirectCall.Initialize();
		StructuralChange.MoveEntityArchetype_00000FBD$BurstDirectCall.Initialize();
		StructuralChange.SetChunkComponent_00000FBE$BurstDirectCall.Initialize();
		StructuralChange.CreateEntity_00000FBF$BurstDirectCall.Initialize();
		StructuralChange.DestroyEntity_00000FC0$BurstDirectCall.Initialize();
		StructuralChange.DestroyChunksQuery_00000FC1$BurstDirectCall.Initialize();
		StructuralChange.InstantiateEntity_00000FC2$BurstDirectCall.Initialize();
		StructuralChange.InstantiateEntities_00000FC3$BurstDirectCall.Initialize();
		FastEquality.GetHashCodeBlittable_00001202$BurstDirectCall.Initialize();
		FastEquality.Hash32_00001203$BurstDirectCall.Initialize();
		FastEquality.EqualsBlittable_00001207$BurstDirectCall.Initialize();
		WorldUnmanagedImpl.UnmanagedUpdate_00001411$BurstDirectCall.Initialize();
		SerializeUtilityInterop.AllocAndQueueReadChunkCommands_00001516$BurstDirectCall.Initialize();
		SerializeUtilityInterop.AddExistingChunk_00001517$BurstDirectCall.Initialize();
		SerializeUtilityInterop.ImportChunks_00001518$BurstDirectCall.Initialize();
		InternalCompilerInterface.MergeWith_0000184E$BurstDirectCall.Initialize();
		RuntimeContentManager.LoadObjectAsync_0000192A$BurstDirectCall.Initialize();
		RuntimeContentManager.ReleaseObjectAsync_0000192B$BurstDirectCall.Initialize();
		RuntimeContentManager.ProcessQueuedCommands_00001930$BurstDirectCall.Initialize();
		RuntimeContentManager.ReleaseObjects_00001939$BurstDirectCall.Initialize();
		RuntimeContentManager.LoadObjectsAsync_0000193A$BurstDirectCall.Initialize();
		RuntimeContentManager.GetObjectLoadingStatus_00001942$BurstDirectCall.Initialize();
		RuntimeContentManager.GetObjectHandle_00001944$BurstDirectCall.Initialize();
	}
}
