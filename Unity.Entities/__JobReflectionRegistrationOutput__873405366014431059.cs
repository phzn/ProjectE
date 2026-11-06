using System;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

// Token: 0x020004DA RID: 1242
[Unity.Jobs.DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__873405366014431059
{
	// Token: 0x06001DDD RID: 7645 RVA: 0x00067EF0 File Offset: 0x000660F0
	public static void CreateJobReflectionData()
	{
		try
		{
			JobChunkExtensions.EarlyJobInit<EntityChangeSetFormatter.BuildEntityGuidToEntityHashMap>();
			IJobParallelForExtensions.EarlyJobInit<EntityChangeSetFormatter.GatherNamesForComponentChanges>();
			IJobParallelForExtensions.EarlyJobInit<EntityChangeSetFormatter.GatherNamesChangedEntities>();
			IJobParallelForDeferExtensions.EarlyJobInit<EntityDiffer.BuildChunkSequenceNumberMap>();
			IJobExtensions.EarlyJobInit<EntityDiffer.GatherArchetypeChunkChanges>();
			IJobExtensions.EarlyJobInit<EntityDiffer.SortBlobAssetPtr>();
			IJobExtensions.EarlyJobInit<EntityDiffer.GatherCreatedAndDestroyedBlobAssets>();
			IJobExtensions.EarlyJobInit<EntityDiffer.GatherBlobAssetChanges>();
			IJobParallelForDeferExtensions.EarlyJobInit<EntityDiffer.ClearMissingReferencesJob>();
			IJobExtensions.EarlyJobInit<EntityDiffer.GatherComponentChangesBuildPacked>();
			IJobExtensions.EarlyJobInit<EntityDiffer.GatherComponentChangesJob>();
			IJobExtensions.EarlyJobInit<EntityDiffer.GetEntityNamesJob>();
			IJobParallelForExtensions.EarlyJobInit<EntityDiffer.PatchAndAddClonedChunks>();
			IJobExtensions.EarlyJobInit<EntityDiffer.DestroyChunksJob>();
			IJobExtensions.EarlyJobInit<EntityDiffer.CollectSharedComponentIndices>();
			IJobExtensions.EarlyJobInit<EntityDiffer.CreateNewChunks>();
			IJobForExtensions.EarlyJobInit<EntityDiffer.CopyChunkBuffers>();
			IJobParallelForExtensions.EarlyJobInit<EntityDiffer.GatherEntityInChunkWithGuid>();
			IJobExtensions.EarlyJobInit<EntityDiffer.SortEntityInChunk>();
			IJobExtensions.EarlyJobInit<EntityDiffer.GatherEntityChanges>();
			IJobExtensions.EarlyJobInit<EntityDiffer.BuildEntityGuidToEntity>();
			JobChunkExtensions.EarlyJobInit<EntityPatcher.BuildComponentToEntityMultiHashMap>();
			JobChunkExtensions.EarlyJobInit<EntityPatcher.BuildComponentToEntityHashMap>();
			JobChunkExtensions.EarlyJobInit<EntityPatcher.BuildEntityToComponentHashMap>();
			IJobExtensions.EarlyJobInit<EntityPatcher.CalculateLinkedEntityGroupEntitiesLengthJob>();
			JobChunkExtensions.EarlyJobInit<EntityPatcher.BuildLinkedEntityGroupHashMap>();
			IJobParallelForExtensions.EarlyJobInit<EntityPatcher.BuildPackedEntityLookupJob>();
			IJobParallelForExtensions.EarlyJobInit<EntityPatcher.BuildPackedTypeLookupJob>();
			IJobExtensions.EarlyJobInit<EntityPatcher.ApplyNamesJob>();
			IJobForExtensions.EarlyJobInit<EntityPatcher.ApplyAddComponentsJob>();
			IJobForExtensions.EarlyJobInit<EntityPatcher.ApplySetSharedComponentsJob>();
			IJobExtensions.EarlyJobInit<EntityPatcher.ApplySetComponentsJob>();
			IJobExtensions.EarlyJobInit<EntityComponentStore.GetCreatedAndDestroyedEntitiesJob>();
			IJobExtensions.EarlyJobInit<EntityManager.RemapEntitiesManagedForDiffer>();
			IJobParallelForExtensions.EarlyJobInit<EntityManager.RemapEntitiesUnmanagedForDiffer>();
			IJobExtensions.EarlyJobInit<EntityManager.ChunkPatchEntities>();
			IJobExtensions.EarlyJobInit<EntityManager.MoveChunksJob>();
			IJobExtensions.EarlyJobInit<EntityManager.GatherAllManagedComponentIndicesJob>();
			IJobExtensions.EarlyJobInit<EntityManager.GatherManagedComponentIndicesInChunkJob>();
			IJobParallelForExtensions.EarlyJobInit<EntityManager.RemapChunksFilteredJob>();
			IJobExtensions.EarlyJobInit<EntityManager.MoveFilteredChunksBetweenArchetypeJob>();
			IJobParallelForExtensions.EarlyJobInit<EntityManager.RemapAllChunksJob>();
			IJobParallelForExtensions.EarlyJobInit<EntityManager.RemapAllArchetypesJob>();
			IJobExtensions.EarlyJobInit<EntityManager.MoveAllChunksJob>();
			IJobExtensions.EarlyJobInit<EntityManager.FreeAllChunksJob>();
			IJobParallelForExtensions.EarlyJobInit<GatherChunksToArrayJob>();
			IJobParallelForExtensions.EarlyJobInit<GatherChunksToArrayWithFilteringJob>();
			IJobParallelForExtensions.EarlyJobInit<JoinChunksJob>();
			IJobExtensions.EarlyJobInit<FilteredChunkIndexJob>();
			IJobExtensions.EarlyJobInit<ChunkBaseEntityIndexJob>();
			IJobExtensions.EarlyJobInit<GatherChunksJob>();
			JobChunkExtensions.EarlyJobInit<GatherEntitiesToArrayJob>();
			JobChunkExtensions.EarlyJobInit<GatherEntitiesJob>();
			JobChunkExtensions.EarlyJobInit<GatherComponentDataToArrayJob>();
			JobChunkExtensions.EarlyJobInit<GatherComponentDataJob>();
			JobChunkExtensions.EarlyJobInit<CopyComponentArrayToChunksJob>();
			JobChunkExtensions.EarlyJobInit<CopyComponentListToChunksJob>();
			JobChunkExtensions.EarlyJobInit<RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_3_Job>();
			JobChunkExtensions.EarlyJobInit<RetainBlobAssetSystem.RetainBlobAssetSystem_76247ABD_LambdaJob_4_Job>();
			IJobParallelForExtensions.EarlyJobInit<CopyIndexedInt>();
			IJobParallelForExtensions.EarlyJobInit<SegmentSortInt>();
			IJobExtensions.EarlyJobInit<SegmentSortMergeInt>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x06001DDE RID: 7646 RVA: 0x00068064 File Offset: 0x00066264
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__873405366014431059.CreateJobReflectionData();
	}
}
