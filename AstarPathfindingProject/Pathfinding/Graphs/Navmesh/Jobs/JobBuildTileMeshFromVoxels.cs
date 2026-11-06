using System;
using System.Threading;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh.Voxelization.Burst;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001F8 RID: 504
	[BurstCompile(CompileSynchronously = true)]
	public struct JobBuildTileMeshFromVoxels : IJob
	{
		// Token: 0x06000DA4 RID: 3492 RVA: 0x0005261D File Offset: 0x0005081D
		public unsafe void SetOutputMeshes(NativeArray<TileMesh.TileMeshUnsafe> arr)
		{
			this.outputMeshes = (TileMesh.TileMeshUnsafe*)arr.GetUnsafeReadOnlyPtr<TileMesh.TileMeshUnsafe>();
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0005262B File Offset: 0x0005082B
		public void SetCounter(NativeReference<int> counter)
		{
			this.currentTileCounter = counter.GetUnsafePtr<int>();
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0005263C File Offset: 0x0005083C
		public unsafe void Execute()
		{
			for (int i = 0; i < this.maxTiles; i++)
			{
				int num = Interlocked.Increment(UnsafeUtility.AsRef<int>((void*)this.currentTileCounter)) - 1;
				if (num >= this.tileGraphSpaceBounds.Length)
				{
					return;
				}
				this.tileBuilder.linkedVoxelField.ResetLinkedVoxelSpans();
				if (this.dimensionMode == RecastGraph.DimensionMode.Dimension2D && this.backgroundTraversability == RecastGraph.BackgroundTraversability.Walkable)
				{
					this.tileBuilder.linkedVoxelField.SetWalkableBackground();
				}
				int num2 = ((num > 0) ? this.inputMeshes.bucketRanges[num - 1] : 0);
				int num3 = this.inputMeshes.bucketRanges[num];
				JobVoxelize jobVoxelize = default(JobVoxelize);
				jobVoxelize.inputMeshes = this.inputMeshes.meshes;
				jobVoxelize.bucket = this.inputMeshes.pointers.GetSubArray(num2, num3 - num2);
				jobVoxelize.voxelWalkableClimb = this.voxelWalkableClimb;
				jobVoxelize.voxelWalkableHeight = this.voxelWalkableHeight;
				jobVoxelize.cellSize = this.cellSize;
				jobVoxelize.cellHeight = this.cellHeight;
				jobVoxelize.maxSlope = this.maxSlope;
				jobVoxelize.graphTransform = this.graphToWorldSpace;
				jobVoxelize.graphSpaceBounds = this.tileGraphSpaceBounds[num];
				jobVoxelize.graphSpaceLimits = this.graphSpaceLimits;
				jobVoxelize.voxelArea = this.tileBuilder.linkedVoxelField;
				jobVoxelize.Execute();
				JobFilterLedges jobFilterLedges = default(JobFilterLedges);
				jobFilterLedges.field = this.tileBuilder.linkedVoxelField;
				jobFilterLedges.voxelWalkableClimb = this.voxelWalkableClimb;
				jobFilterLedges.voxelWalkableHeight = this.voxelWalkableHeight;
				jobFilterLedges.cellSize = this.cellSize;
				jobFilterLedges.cellHeight = this.cellHeight;
				jobFilterLedges.Execute();
				JobFilterLowHeightSpans jobFilterLowHeightSpans = default(JobFilterLowHeightSpans);
				jobFilterLowHeightSpans.field = this.tileBuilder.linkedVoxelField;
				jobFilterLowHeightSpans.voxelWalkableHeight = this.voxelWalkableHeight;
				jobFilterLowHeightSpans.Execute();
				JobBuildCompactField jobBuildCompactField = default(JobBuildCompactField);
				jobBuildCompactField.input = this.tileBuilder.linkedVoxelField;
				jobBuildCompactField.output = this.tileBuilder.compactVoxelField;
				jobBuildCompactField.Execute();
				JobBuildConnections jobBuildConnections = default(JobBuildConnections);
				jobBuildConnections.field = this.tileBuilder.compactVoxelField;
				jobBuildConnections.voxelWalkableHeight = (int)this.voxelWalkableHeight;
				jobBuildConnections.voxelWalkableClimb = this.voxelWalkableClimb;
				jobBuildConnections.Execute();
				JobErodeWalkableArea jobErodeWalkableArea = default(JobErodeWalkableArea);
				jobErodeWalkableArea.field = this.tileBuilder.compactVoxelField;
				jobErodeWalkableArea.radius = this.characterRadiusInVoxels;
				jobErodeWalkableArea.Execute();
				JobBuildDistanceField jobBuildDistanceField = default(JobBuildDistanceField);
				jobBuildDistanceField.field = this.tileBuilder.compactVoxelField;
				jobBuildDistanceField.output = this.tileBuilder.distanceField;
				jobBuildDistanceField.Execute();
				JobBuildRegions jobBuildRegions = default(JobBuildRegions);
				jobBuildRegions.field = this.tileBuilder.compactVoxelField;
				jobBuildRegions.distanceField = this.tileBuilder.distanceField;
				jobBuildRegions.borderSize = this.tileBorderSizeInVoxels;
				jobBuildRegions.minRegionSize = Mathf.RoundToInt((float)this.minRegionSize);
				jobBuildRegions.srcQue = this.tileBuilder.tmpQueue1;
				jobBuildRegions.dstQue = this.tileBuilder.tmpQueue2;
				jobBuildRegions.relevantGraphSurfaces = this.relevantGraphSurfaces;
				jobBuildRegions.relevantGraphSurfaceMode = this.relevantGraphSurfaceMode;
				jobBuildRegions.cellSize = this.cellSize;
				jobBuildRegions.cellHeight = this.cellHeight;
				jobBuildRegions.graphTransform = this.graphToWorldSpace;
				jobBuildRegions.graphSpaceBounds = this.tileGraphSpaceBounds[num];
				jobBuildRegions.Execute();
				JobBuildContours jobBuildContours = default(JobBuildContours);
				jobBuildContours.field = this.tileBuilder.compactVoxelField;
				jobBuildContours.maxError = this.contourMaxError;
				jobBuildContours.maxEdgeLength = this.maxEdgeLength;
				jobBuildContours.buildFlags = 5;
				jobBuildContours.cellSize = this.cellSize;
				jobBuildContours.outputContours = this.tileBuilder.contours;
				jobBuildContours.outputVerts = this.tileBuilder.contourVertices;
				jobBuildContours.Execute();
				JobBuildMesh jobBuildMesh = default(JobBuildMesh);
				jobBuildMesh.contours = this.tileBuilder.contours;
				jobBuildMesh.contourVertices = this.tileBuilder.contourVertices;
				jobBuildMesh.mesh = this.tileBuilder.voxelMesh;
				jobBuildMesh.field = this.tileBuilder.compactVoxelField;
				jobBuildMesh.Execute();
				ref TileMesh.TileMeshUnsafe ptr = ref this.outputMeshes[num];
				JobConvertAreasToTags jobConvertAreasToTags = default(JobConvertAreasToTags);
				jobConvertAreasToTags.areas = this.tileBuilder.voxelMesh.areas;
				jobConvertAreasToTags.Execute();
				MeshUtility.JobMergeNearbyVertices jobMergeNearbyVertices = default(MeshUtility.JobMergeNearbyVertices);
				jobMergeNearbyVertices.vertices = this.tileBuilder.voxelMesh.verts;
				jobMergeNearbyVertices.triangles = this.tileBuilder.voxelMesh.tris;
				jobMergeNearbyVertices.mergeRadiusSq = 0;
				jobMergeNearbyVertices.Execute();
				MeshUtility.JobRemoveDegenerateTriangles jobRemoveDegenerateTriangles = default(MeshUtility.JobRemoveDegenerateTriangles);
				jobRemoveDegenerateTriangles.vertices = this.tileBuilder.voxelMesh.verts;
				jobRemoveDegenerateTriangles.triangles = this.tileBuilder.voxelMesh.tris;
				jobRemoveDegenerateTriangles.tags = this.tileBuilder.voxelMesh.areas;
				jobRemoveDegenerateTriangles.Execute();
				JobTransformTileCoordinates jobTransformTileCoordinates = default(JobTransformTileCoordinates);
				jobTransformTileCoordinates.vertices = this.tileBuilder.voxelMesh.verts.AsUnsafeSpan<Int3>();
				jobTransformTileCoordinates.matrix = this.voxelToTileSpace;
				jobTransformTileCoordinates.Execute();
				ptr = new TileMesh.TileMeshUnsafe
				{
					verticesInTileSpace = this.tileBuilder.voxelMesh.verts.AsUnsafeSpan<Int3>().Clone(Allocator.Persistent),
					triangles = this.tileBuilder.voxelMesh.tris.AsUnsafeSpan<int>().Clone(Allocator.Persistent),
					tags = this.tileBuilder.voxelMesh.areas.AsUnsafeSpan<int>().Reinterpret<uint>().Clone(Allocator.Persistent)
				};
			}
		}

		// Token: 0x0400097A RID: 2426
		public TileBuilderBurst tileBuilder;

		// Token: 0x0400097B RID: 2427
		[ReadOnly]
		public TileBuilder.BucketMapping inputMeshes;

		// Token: 0x0400097C RID: 2428
		[ReadOnly]
		public NativeArray<Bounds> tileGraphSpaceBounds;

		// Token: 0x0400097D RID: 2429
		public Matrix4x4 voxelToTileSpace;

		// Token: 0x0400097E RID: 2430
		public Vector2 graphSpaceLimits;

		// Token: 0x0400097F RID: 2431
		[NativeDisableUnsafePtrRestriction]
		public unsafe TileMesh.TileMeshUnsafe* outputMeshes;

		// Token: 0x04000980 RID: 2432
		public int maxTiles;

		// Token: 0x04000981 RID: 2433
		public int voxelWalkableClimb;

		// Token: 0x04000982 RID: 2434
		public uint voxelWalkableHeight;

		// Token: 0x04000983 RID: 2435
		public float cellSize;

		// Token: 0x04000984 RID: 2436
		public float cellHeight;

		// Token: 0x04000985 RID: 2437
		public float maxSlope;

		// Token: 0x04000986 RID: 2438
		public RecastGraph.DimensionMode dimensionMode;

		// Token: 0x04000987 RID: 2439
		public RecastGraph.BackgroundTraversability backgroundTraversability;

		// Token: 0x04000988 RID: 2440
		public Matrix4x4 graphToWorldSpace;

		// Token: 0x04000989 RID: 2441
		public int characterRadiusInVoxels;

		// Token: 0x0400098A RID: 2442
		public int tileBorderSizeInVoxels;

		// Token: 0x0400098B RID: 2443
		public int minRegionSize;

		// Token: 0x0400098C RID: 2444
		public float maxEdgeLength;

		// Token: 0x0400098D RID: 2445
		public float contourMaxError;

		// Token: 0x0400098E RID: 2446
		[ReadOnly]
		public NativeArray<JobBuildRegions.RelevantGraphSurfaceInfo> relevantGraphSurfaces;

		// Token: 0x0400098F RID: 2447
		public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		// Token: 0x04000990 RID: 2448
		[NativeDisableUnsafePtrRestriction]
		public unsafe int* currentTileCounter;

		// Token: 0x04000991 RID: 2449
		private static readonly ProfilerMarker MarkerVoxelize = new ProfilerMarker("Voxelize");

		// Token: 0x04000992 RID: 2450
		private static readonly ProfilerMarker MarkerFilterLedges = new ProfilerMarker("FilterLedges");

		// Token: 0x04000993 RID: 2451
		private static readonly ProfilerMarker MarkerFilterLowHeightSpans = new ProfilerMarker("FilterLowHeightSpans");

		// Token: 0x04000994 RID: 2452
		private static readonly ProfilerMarker MarkerBuildCompactField = new ProfilerMarker("BuildCompactField");

		// Token: 0x04000995 RID: 2453
		private static readonly ProfilerMarker MarkerBuildConnections = new ProfilerMarker("BuildConnections");

		// Token: 0x04000996 RID: 2454
		private static readonly ProfilerMarker MarkerErodeWalkableArea = new ProfilerMarker("ErodeWalkableArea");

		// Token: 0x04000997 RID: 2455
		private static readonly ProfilerMarker MarkerBuildDistanceField = new ProfilerMarker("BuildDistanceField");

		// Token: 0x04000998 RID: 2456
		private static readonly ProfilerMarker MarkerBuildRegions = new ProfilerMarker("BuildRegions");

		// Token: 0x04000999 RID: 2457
		private static readonly ProfilerMarker MarkerBuildContours = new ProfilerMarker("BuildContours");

		// Token: 0x0400099A RID: 2458
		private static readonly ProfilerMarker MarkerBuildMesh = new ProfilerMarker("BuildMesh");

		// Token: 0x0400099B RID: 2459
		private static readonly ProfilerMarker MarkerConvertAreasToTags = new ProfilerMarker("ConvertAreasToTags");

		// Token: 0x0400099C RID: 2460
		private static readonly ProfilerMarker MarkerRemoveDuplicateVertices = new ProfilerMarker("RemoveDuplicateVertices");

		// Token: 0x0400099D RID: 2461
		private static readonly ProfilerMarker MarkerTransformTileCoordinates = new ProfilerMarker("TransformTileCoordinates");
	}
}
