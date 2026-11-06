using System;
using Pathfinding;
using Pathfinding.ECS;
using Pathfinding.ECS.RVO;
using Pathfinding.Graphs.Grid;
using Pathfinding.Graphs.Grid.Jobs;
using Pathfinding.Graphs.Grid.Rules;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.Graphs.Navmesh.Jobs;
using Pathfinding.Graphs.Navmesh.Voxelization.Burst;
using Pathfinding.Jobs;
using Pathfinding.RVO;
using Pathfinding.Util;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x020003C9 RID: 969
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__10569589673291187727
{
	// Token: 0x06001822 RID: 6178 RVA: 0x00081748 File Offset: 0x0007F948
	public static void CreateJobReflectionData()
	{
		try
		{
			Unity.Jobs.IJobExtensions.EarlyJobInit<GraphUpdateObject.JobGraphUpdate>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<NavmeshEdges.JobResizeObstacles>();
			IJobParallelForBatchExtensions.EarlyJobInit<NavmeshEdges.JobCalculateObstacles>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<NavmeshPrefab.SerializeJob>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<HierarchicalGraph.JobRecalculateComponents>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<PathUtilities.JobFormationPacked>();
			IJobParallelForExtensions.EarlyJobInit<JobRaycastAll.JobCreateCommands>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobRaycastAll.JobCombineResults>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobMaxHitCount>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobClampHitToRay>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCopyHits>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobDependencyTracker.JobRaycastCommandDummy>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobDependencyTracker.JobSpherecastCommandDummy>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobDependencyTracker.JobOverlapCapsuleCommandDummy>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobDependencyTracker.JobOverlapSphereCommandDummy>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<global::Pathfinding.Jobs.IJobExtensions.ManagedJob>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<global::Pathfinding.Jobs.IJobExtensions.ManagedActionJob>();
			IJobForExtensions.EarlyJobInit<RecastMeshGatherer.JobGenerateHeightmapChunk>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<TileCutter.JobCutTiles>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobBuildContours>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobBuildMesh>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobVoxelize>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobBuildCompactField>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobBuildConnections>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobErodeWalkableArea>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobBuildDistanceField>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobFilterLowHeightSpans>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobFilterLedges>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobBuildRegions>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobBuildTileMeshFromVertices>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobBuildTileMeshFromVertices.JobTransformTileCoordinates>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobBuildTileMeshFromVoxels>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCalculateTriangleConnections>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobConnectTiles>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobConnectTilesSingle>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobConvertAreasToTags>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCreateTiles>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobTransformTileCoordinates>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobWriteNodeConnections>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<RuleAnglePenalty.JobPenaltyAngle>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<RuleElevationPenalty.JobElevationPenalty>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<RuleTexture.JobTexturePosition>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<RuleTexture.JobTexturePenalty>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobAllocateNodes>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCheckCollisions>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobColliderHitsToBooleans>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCopyBuffers>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobMergeRaycastCollisionHits>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobNodeGridLayout>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobNodeWalkability>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobPrepareCapsuleCommands>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobPrepareGridRaycast>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobPrepareGridRaycastThick>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobPrepareRaycasts>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobPrepareSphereCommands>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobRelocateNodes>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<MeshUtility.JobMergeNearbyVertices>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<MeshUtility.JobRemoveDegenerateTriangles>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<RVOQuadtreeBurst.JobBuild>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<RVOQuadtreeBurst.DebugDrawJob>();
			JobChunkExtensions.EarlyJobInit<JobAlignAgentWithMovementDirection>();
			JobChunkExtensions.EarlyJobInit<JobApplyGravity>();
			JobChunkExtensions.EarlyJobInit<JobClearTemporaryData>();
			JobChunkExtensions.EarlyJobInit<JobControl>();
			JobChunkExtensions.EarlyJobInit<JobDrawFollowerGizmos>();
			JobChunkExtensions.EarlyJobInit<JobDrawFollowerGizmosBase>();
			JobChunkExtensions.EarlyJobInit<JobManagedMovementOverrideBeforeControl>();
			JobChunkExtensions.EarlyJobInit<JobManagedMovementOverrideAfterControl>();
			JobChunkExtensions.EarlyJobInit<JobManagedMovementOverrideBeforeMovement>();
			JobChunkExtensions.EarlyJobInit<JobManagedOffMeshLinkTransition>();
			JobChunkExtensions.EarlyJobInit<JobManagedOffMeshLinkTransitionCleanup>();
			JobChunkExtensions.EarlyJobInit<JobMoveAgent>();
			JobChunkExtensions.EarlyJobInit<JobPrepareAgentRaycasts>();
			JobChunkExtensions.EarlyJobInit<JobRepairPath>();
			IJobParallelForTransformExtensions.EarlyJobInit<JobSyncEntitiesToTransforms>();
			JobChunkExtensions.EarlyJobInit<JobUpdateMovementStatistics>();
			JobChunkExtensions.EarlyJobInit<DestinationEntitySystem.UpdateDestinationJob>();
			JobChunkExtensions.EarlyJobInit<DestinationMoveInCircleSystem.UpdateDestinationJob>();
			JobChunkExtensions.EarlyJobInit<FallbackResolveMovementSystem.CopyJob>();
			JobChunkExtensions.EarlyJobInit<FallbackResolveMovementSystem.CopyRotationJob>();
			JobChunkExtensions.EarlyJobInit<MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal>();
			JobChunkExtensions.EarlyJobInit<MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph>();
			JobChunkExtensions.EarlyJobInit<SchedulePathSearchSystem.JobCheckStaleness>();
			JobChunkExtensions.EarlyJobInit<SchedulePathSearchSystem.JobShouldRecalculatePaths>();
			JobChunkExtensions.EarlyJobInit<SchedulePathSearchSystem.JobRecalculatePaths>();
			IJobParallelForTransformExtensions.EarlyJobInit<SyncTransformsToEntitiesSystem.SyncTransformsToEntitiesJob>();
			JobChunkExtensions.EarlyJobInit<RVOSystem.JobCopyFromEntitiesToRVOSimulator>();
			JobChunkExtensions.EarlyJobInit<RVOSystem.JobCopyFromRVOSimulatorToEntities>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCopyRectangle<float4>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobErosion<FlatGridAdjacencyMapper>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobErosion<LayeredGridAdjacencyMapper>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCopyRectangle<Vector3>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCopyRectangle<ulong>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCopyRectangle<uint>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCopyRectangle<int>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCopyRectangle<bool>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobRVOPreprocess<XYMovementPlane>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobRVOPreprocess<XZMovementPlane>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobRVOPreprocess<ArbitraryMovementPlane>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobDestinationReached<XYMovementPlane>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobDestinationReached<XZMovementPlane>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobDestinationReached<ArbitraryMovementPlane>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobMemSet<bool>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobMemSet<float4>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobCopy<bool>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobRotate3DArray<Vector3>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobRotate3DArray<float4>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobRotate3DArray<ulong>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobRotate3DArray<uint>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobRotate3DArray<bool>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobRotate3DArray<int>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobMemSet<uint>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<IndexActionJob<JobAND>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobMemSet<float3>>();
			Unity.Jobs.IJobExtensions.EarlyJobInit<JobMemSet<int>>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x000819C4 File Offset: 0x0007FBC4
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__10569589673291187727.CreateJobReflectionData();
	}
}
