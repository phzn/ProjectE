using System;
using Pathfinding;
using Pathfinding.Collections;
using Pathfinding.ECS;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.RVO;
using Pathfinding.Util;
using UnityEngine;

// Token: 0x020003CA RID: 970
internal static class $BurstDirectCallInitializer
{
	// Token: 0x06001824 RID: 6180 RVA: 0x000819CC File Offset: 0x0007FBCC
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		Polygon.ContainsPoint_00000357$BurstDirectCall.Initialize();
		Polygon.ClosestPointOnTriangleByRef_0000035D$BurstDirectCall.Initialize();
		Polygon.ClosestPointOnTriangleProjected_00000360$BurstDirectCall.Initialize();
		BinaryHeap.Add_00000373$BurstDirectCall.Initialize();
		BinaryHeap.Remove_00000376$BurstDirectCall.Initialize();
		HeuristicObjective.Calculate_00000560$BurstDirectCall.Initialize();
		Path.SearchContext.OpenCandidateConnectionBurst_000005EA$BurstDirectCall.Initialize();
		TriangleMeshNode.InterpolateEdge_0000084C$BurstDirectCall.Initialize();
		TriangleMeshNode.OpenSingleEdgeBurst_00000851$BurstDirectCall.Initialize();
		TriangleMeshNode.CalculateBestEdgePosition_00000852$BurstDirectCall.Initialize();
		NavmeshCutJobs.CalculateContour_0000099F$BurstDirectCall.Initialize();
		Funnel.Calculate_00000A41$BurstDirectCall.Initialize();
		Funnel.FunnelState.PushStart_00000A4F$BurstDirectCall.Initialize();
		Funnel.FunnelState.ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall.Initialize();
		PathTracer.ContainsAndProject_00000A85$BurstDirectCall.Initialize();
		PathTracer.EstimateRemainingPath_00000A98$BurstDirectCall.Initialize();
		PathTracer.RemainingDistanceLowerBound_00000A9C$BurstDirectCall.Initialize();
		ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000B85$BurstDirectCall.Initialize();
		RecastMeshGatherer.CalculateBounds_00000B92$BurstDirectCall.Initialize();
		RecastMeshGatherer.PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall.Initialize();
		RecastMeshGatherer.CreateRasterizationMeshes_00000B96$BurstDirectCall.Initialize();
		RecastMeshGatherer.ConvertTreesToMeshes_00000BA9$BurstDirectCall.Initialize();
		TileBuilder.PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall.Initialize();
		TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall.Initialize();
		TileHandler.CutTiles_00000BD7$BurstDirectCall.Initialize();
		HierarchicalBitset.Iterator.MoveNextBurst_00000E4F$BurstDirectCall.Initialize();
		BBTree.Build_00000EE6$BurstDirectCall.Initialize();
		BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall.Initialize();
		BBTree.Initialize$NearbyNodesIterator_MoveNext_00000EF8$BurstDirectCall();
		MeshUtility.MakeTrianglesClockwise_00000FC7$BurstDirectCall.Initialize();
		RVOObstacleCache.TraceContours_0000109A$BurstDirectCall.Initialize();
		DrawGizmosJobUtils.DrawPath_00001274$BurstDirectCall.Initialize();
		JobDrawFollowerGizmosBase.DrawGizmos_0000127C$BurstDirectCall.Initialize();
		JobRepairPathHelpers.UpdateReachedEndInfo_000013B7$BurstDirectCall.Initialize();
		DestinationEntitySystem.__codegen__OnUpdate_0000141C$BurstDirectCall.Initialize();
		DestinationMoveInCircleSystem.__codegen__OnUpdate_00001448$BurstDirectCall.Initialize();
		MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal_000014EB$BurstDirectCall.Initialize();
	}
}
