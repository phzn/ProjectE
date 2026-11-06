using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh.Jobs;
using Pathfinding.Graphs.Navmesh.Voxelization.Burst;
using Pathfinding.Jobs;
using Pathfinding.Sync;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001BB RID: 443
	[BurstCompile]
	public struct TileBuilder
	{
		// Token: 0x06000CB3 RID: 3251 RVA: 0x00046F80 File Offset: 0x00045180
		public TileBuilder(RecastGraph graph, TileLayout tileLayout, IntRect tileRect)
		{
			this.tileLayout = tileLayout;
			this.tileRect = tileRect;
			this.walkableClimb = Mathf.Min(graph.walkableClimb, graph.walkableHeight);
			this.collectionSettings = graph.collectionSettings;
			this.dimensionMode = graph.dimensionMode;
			this.backgroundTraversability = graph.backgroundTraversability;
			this.tileBorderSizeInVoxels = graph.TileBorderSizeInVoxels;
			this.walkableHeight = graph.walkableHeight;
			this.maxSlope = graph.maxSlope;
			this.characterRadiusInVoxels = graph.CharacterRadiusInVoxels;
			this.minRegionSize = Mathf.RoundToInt(graph.minRegionSize);
			this.maxEdgeLength = graph.maxEdgeLength;
			this.contourMaxError = graph.contourMaxError;
			this.relevantGraphSurfaceMode = graph.relevantGraphSurfaceMode;
			this.perLayerModifications = graph.perLayerModifications;
			this.perTerrainLayerModifications = graph.perTerrainLayerModifications;
			if (this.collectionSettings.physicsScene == null)
			{
				this.collectionSettings.physicsScene = new PhysicsScene?(graph.active.gameObject.scene.GetPhysicsScene());
			}
			if (this.collectionSettings.physicsScene2D == null)
			{
				this.collectionSettings.physicsScene2D = new PhysicsScene2D?(graph.active.gameObject.scene.GetPhysicsScene2D());
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x000470C1 File Offset: 0x000452C1
		private int TileBorderSizeInVoxels
		{
			get
			{
				return this.characterRadiusInVoxels + 3;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x000470CB File Offset: 0x000452CB
		private float TileBorderSizeInWorldUnits
		{
			get
			{
				return (float)this.TileBorderSizeInVoxels * this.tileLayout.cellSize;
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000470E0 File Offset: 0x000452E0
		public Bounds GetWorldSpaceBounds(float xzPadding = 0f)
		{
			Bounds tileBoundsInGraphSpace = this.tileLayout.GetTileBoundsInGraphSpace(this.tileRect.xmin, this.tileRect.ymin, this.tileRect.Width, this.tileRect.Height);
			tileBoundsInGraphSpace.Expand(new Vector3(2f * xzPadding, 0f, 2f * xzPadding));
			return this.tileLayout.transform.Transform(tileBoundsInGraphSpace);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00047158 File Offset: 0x00045358
		public RecastMeshGatherer.MeshCollection CollectMeshes(Bounds bounds)
		{
			LayerMask layerMask = this.collectionSettings.layerMask;
			List<string> list = this.collectionSettings.tagMask;
			if (this.collectionSettings.collectionMode == RecastGraph.CollectionSettings.FilterMode.Layers)
			{
				list = null;
			}
			else
			{
				layerMask = 0;
			}
			RecastMeshGatherer recastMeshGatherer = new RecastMeshGatherer(this.collectionSettings.physicsScene.Value, this.collectionSettings.physicsScene2D.Value, bounds, this.collectionSettings.terrainHeightmapDownsamplingFactor, layerMask, list, this.perLayerModifications, this.perTerrainLayerModifications, this.tileLayout.cellSize / this.collectionSettings.colliderRasterizeDetail);
			if (this.collectionSettings.rasterizeMeshes && this.dimensionMode == RecastGraph.DimensionMode.Dimension3D)
			{
				recastMeshGatherer.CollectSceneMeshes();
			}
			recastMeshGatherer.CollectRecastNavmeshModifiers();
			if (this.collectionSettings.rasterizeTerrain && this.dimensionMode == RecastGraph.DimensionMode.Dimension3D)
			{
				float num = 0.51f * this.tileLayout.cellSize * (float)(math.max(this.tileLayout.tileSizeInVoxels.x, this.tileLayout.tileSizeInVoxels.y) + 2 * this.TileBorderSizeInVoxels);
				recastMeshGatherer.CollectTerrainMeshes(this.collectionSettings.rasterizeTrees, num);
			}
			if (this.collectionSettings.rasterizeColliders || this.dimensionMode == RecastGraph.DimensionMode.Dimension2D)
			{
				if (this.dimensionMode == RecastGraph.DimensionMode.Dimension3D)
				{
					recastMeshGatherer.CollectColliderMeshes();
				}
				else
				{
					recastMeshGatherer.Collect2DColliderMeshes();
				}
			}
			if (this.collectionSettings.onCollectMeshes != null)
			{
				this.collectionSettings.onCollectMeshes(recastMeshGatherer);
			}
			RecastMeshGatherer.MeshCollection meshCollection = recastMeshGatherer.Finalize();
			if (this.tileRect == new IntRect(0, 0, this.tileLayout.tileCount.x - 1, this.tileLayout.tileCount.y - 1) && meshCollection.meshes.Length == 0)
			{
				Debug.LogWarning("No rasterizable objects were found contained in the layers specified by the 'mask' variables");
			}
			return meshCollection;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0004731C File Offset: 0x0004551C
		[BurstCompile]
		private static void PutMeshesIntoTileBuckets(ref UnsafeSpan<RasterizationMesh> meshes, ref IntRect tileBuckets, ref float4x4 worldToGraphMatrix, ref float2 tileSize, int borderExpansion, out UnsafeSpan<int> bucketRanges, out UnsafeSpan<int> pointers)
		{
			TileBuilder.PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall.Invoke(ref meshes, ref tileBuckets, ref worldToGraphMatrix, ref tileSize, borderExpansion, out bucketRanges, out pointers);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x00047330 File Offset: 0x00045530
		public Promise<TileBuilder.TileBuilderOutput> Schedule(DisposeArena arena)
		{
			int area = this.tileRect.Area;
			int width = this.tileRect.Width;
			int height = this.tileRect.Height;
			Bounds worldSpaceBounds = this.GetWorldSpaceBounds(this.TileBorderSizeInWorldUnits);
			if (this.dimensionMode == RecastGraph.DimensionMode.Dimension2D)
			{
				worldSpaceBounds.extents = new Vector3(worldSpaceBounds.extents.x, worldSpaceBounds.extents.y, float.PositiveInfinity);
			}
			RecastMeshGatherer.MeshCollection meshCollection = this.CollectMeshes(worldSpaceBounds);
			UnsafeSpan<RasterizationMesh> unsafeSpan = meshCollection.meshes.AsUnsafeSpan<RasterizationMesh>();
			float4x4 float4x = this.tileLayout.transform.inverseMatrix;
			float2 @float = new float2(this.tileLayout.TileWorldSize);
			UnsafeSpan<int> unsafeSpan2;
			UnsafeSpan<int> unsafeSpan3;
			TileBuilder.PutMeshesIntoTileBuckets(ref unsafeSpan, ref this.tileRect, ref float4x, ref @float, this.TileBorderSizeInVoxels, out unsafeSpan2, out unsafeSpan3);
			TileBuilder.BucketMapping bucketMapping = new TileBuilder.BucketMapping
			{
				meshes = meshCollection.meshes,
				pointers = unsafeSpan3.MoveToNativeArray(Allocator.Persistent),
				bucketRanges = unsafeSpan2.MoveToNativeArray(Allocator.Persistent)
			};
			NativeArray<TileMesh.TileMeshUnsafe> nativeArray = new NativeArray<TileMesh.TileMeshUnsafe>(area, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			int num = this.tileLayout.tileSizeInVoxels.x + this.tileBorderSizeInVoxels * 2;
			int num2 = this.tileLayout.tileSizeInVoxels.y + this.tileBorderSizeInVoxels * 2;
			float cellHeight = this.tileLayout.CellHeight;
			uint num3 = (uint)(this.walkableHeight / cellHeight);
			int num4 = Mathf.RoundToInt(this.walkableClimb / cellHeight);
			NativeArray<Bounds> nativeArray2 = new NativeArray<Bounds>(area, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int num5 = j + i * width;
					Bounds tileBoundsInGraphSpace = this.tileLayout.GetTileBoundsInGraphSpace(this.tileRect.xmin + j, this.tileRect.ymin + i, 1, 1);
					tileBoundsInGraphSpace.Expand(new Vector3(1f, 0f, 1f) * this.TileBorderSizeInWorldUnits * 2f);
					nativeArray2[num5] = tileBoundsInGraphSpace;
				}
			}
			TileBuilderBurst[] array = new TileBuilderBurst[Mathf.Max(1, Mathf.Min(area, JobsUtility.JobWorkerCount + 1))];
			NativeReference<int> nativeReference = new NativeReference<int>(0, Allocator.Persistent);
			JobHandle jobHandle = default(JobHandle);
			NativeList<JobBuildRegions.RelevantGraphSurfaceInfo> nativeList = new NativeList<JobBuildRegions.RelevantGraphSurfaceInfo>(Allocator.Persistent);
			RelevantGraphSurface relevantGraphSurface = RelevantGraphSurface.Root;
			while (relevantGraphSurface != null)
			{
				JobBuildRegions.RelevantGraphSurfaceInfo relevantGraphSurfaceInfo = default(JobBuildRegions.RelevantGraphSurfaceInfo);
				relevantGraphSurfaceInfo.position = relevantGraphSurface.transform.position;
				relevantGraphSurfaceInfo.range = relevantGraphSurface.maxRange;
				nativeList.Add(in relevantGraphSurfaceInfo);
				relevantGraphSurface = relevantGraphSurface.Next;
			}
			int num6 = Mathf.CeilToInt(Mathf.Sqrt((float)area));
			int num7 = num6 * array.Length;
			int num8 = 2 * (area + num7 - 1) / num7;
			JobBuildTileMeshFromVoxels jobBuildTileMeshFromVoxels = new JobBuildTileMeshFromVoxels
			{
				tileBuilder = array[0],
				inputMeshes = bucketMapping,
				tileGraphSpaceBounds = nativeArray2,
				voxelWalkableClimb = num4,
				voxelWalkableHeight = num3,
				voxelToTileSpace = Matrix4x4.Scale(new Vector3(this.tileLayout.cellSize, cellHeight, this.tileLayout.cellSize)) * Matrix4x4.Translate(-new Vector3(1f, 0f, 1f) * (float)this.TileBorderSizeInVoxels),
				cellSize = this.tileLayout.cellSize,
				cellHeight = cellHeight,
				maxSlope = Mathf.Max(this.maxSlope, 0.0001f),
				dimensionMode = this.dimensionMode,
				backgroundTraversability = this.backgroundTraversability,
				graphToWorldSpace = this.tileLayout.transform.matrix,
				graphSpaceLimits = new Vector2(this.tileLayout.graphSpaceSize.x + (float)(this.characterRadiusInVoxels - 1) * this.tileLayout.cellSize, this.tileLayout.graphSpaceSize.z + (float)(this.characterRadiusInVoxels - 1) * this.tileLayout.cellSize),
				characterRadiusInVoxels = this.characterRadiusInVoxels,
				tileBorderSizeInVoxels = this.tileBorderSizeInVoxels,
				minRegionSize = this.minRegionSize,
				maxEdgeLength = this.maxEdgeLength,
				contourMaxError = this.contourMaxError,
				maxTiles = num6,
				relevantGraphSurfaces = nativeList.AsArray(),
				relevantGraphSurfaceMode = this.relevantGraphSurfaceMode
			};
			jobBuildTileMeshFromVoxels.SetOutputMeshes(nativeArray);
			jobBuildTileMeshFromVoxels.SetCounter(nativeReference);
			int num9 = (int)(this.tileLayout.graphSpaceSize.y / cellHeight);
			for (int k = 0; k < array.Length; k++)
			{
				jobBuildTileMeshFromVoxels.tileBuilder = (array[k] = new TileBuilderBurst(num, num2, (int)num3, num9));
				JobHandle jobHandle2 = default(JobHandle);
				for (int l = 0; l < num8; l++)
				{
					jobHandle2 = jobBuildTileMeshFromVoxels.Schedule(jobHandle2);
				}
				jobHandle = JobHandle.CombineDependencies(jobHandle, jobHandle2);
			}
			JobHandle.ScheduleBatchedJobs();
			arena.Add<Bounds>(nativeArray2);
			arena.Add<JobBuildRegions.RelevantGraphSurfaceInfo>(nativeList);
			arena.Add<int>(bucketMapping.bucketRanges);
			arena.Add<int>(bucketMapping.pointers);
			arena.Add<RecastMeshGatherer.MeshCollection>(meshCollection);
			for (int m = 0; m < array.Length; m++)
			{
				arena.Add<TileBuilderBurst>(array[m]);
			}
			return new Promise<TileBuilder.TileBuilderOutput>(jobHandle, new TileBuilder.TileBuilderOutput
			{
				tileMeshes = new TileMeshesUnsafe(nativeArray, this.tileRect, new Vector2(this.tileLayout.TileWorldSizeX, this.tileLayout.TileWorldSizeZ)),
				currentTileCounter = nativeReference
			});
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x000478D4 File Offset: 0x00045AD4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void PutMeshesIntoTileBuckets$BurstManaged(ref UnsafeSpan<RasterizationMesh> meshes, ref IntRect tileBuckets, ref float4x4 worldToGraphMatrix, ref float2 tileSize, int borderExpansion, out UnsafeSpan<int> bucketRanges, out UnsafeSpan<int> pointers)
		{
			int num = tileBuckets.Width * tileBuckets.Height;
			UnsafeSpan<UnsafeList<int>> unsafeSpan = new UnsafeSpan<UnsafeList<int>>(Allocator.Temp, num);
			for (int i = 0; i < unsafeSpan.Length; i++)
			{
				*unsafeSpan[i] = new UnsafeList<int>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			}
			Vector2Int vector2Int = -tileBuckets.Min;
			IntRect intRect = new IntRect(0, 0, tileBuckets.Width - 1, tileBuckets.Height - 1);
			for (int j = 0; j < meshes.Length; j++)
			{
				Bounds bounds = meshes[j]->bounds;
				IntRect intRect2 = IntRect.Intersection(TileLayout.GetTouchingTiles(ref worldToGraphMatrix, tileSize, bounds, (float)borderExpansion).Offset(vector2Int), intRect);
				for (int k = intRect2.ymin; k <= intRect2.ymax; k++)
				{
					for (int l = intRect2.xmin; l <= intRect2.xmax; l++)
					{
						unsafeSpan[l + k * tileBuckets.Width].Add(in j);
					}
				}
			}
			int num2 = 0;
			for (int m = 0; m < unsafeSpan.Length; m++)
			{
				num2 += unsafeSpan[m].Length;
			}
			pointers = new UnsafeSpan<int>(Allocator.Persistent, num2);
			bucketRanges = new UnsafeSpan<int>(Allocator.Persistent, num);
			num2 = 0;
			for (int n = 0; n < unsafeSpan.Length; n++)
			{
				(*unsafeSpan[n]).AsUnsafeSpan<int>().CopyTo(pointers.Slice(num2, unsafeSpan[n].Length));
				num2 += unsafeSpan[n].Length;
				*bucketRanges[n] = num2;
				unsafeSpan[n].Dispose();
			}
		}

		// Token: 0x0400087A RID: 2170
		public float walkableClimb;

		// Token: 0x0400087B RID: 2171
		public RecastGraph.CollectionSettings collectionSettings;

		// Token: 0x0400087C RID: 2172
		public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		// Token: 0x0400087D RID: 2173
		public RecastGraph.DimensionMode dimensionMode;

		// Token: 0x0400087E RID: 2174
		public RecastGraph.BackgroundTraversability backgroundTraversability;

		// Token: 0x0400087F RID: 2175
		public int tileBorderSizeInVoxels;

		// Token: 0x04000880 RID: 2176
		public float walkableHeight;

		// Token: 0x04000881 RID: 2177
		public float maxSlope;

		// Token: 0x04000882 RID: 2178
		public int characterRadiusInVoxels;

		// Token: 0x04000883 RID: 2179
		public int minRegionSize;

		// Token: 0x04000884 RID: 2180
		public float maxEdgeLength;

		// Token: 0x04000885 RID: 2181
		public float contourMaxError;

		// Token: 0x04000886 RID: 2182
		public TileLayout tileLayout;

		// Token: 0x04000887 RID: 2183
		public IntRect tileRect;

		// Token: 0x04000888 RID: 2184
		public List<RecastGraph.PerLayerModification> perLayerModifications;

		// Token: 0x04000889 RID: 2185
		public List<RecastGraph.PerTerrainLayerModification> perTerrainLayerModifications;

		// Token: 0x020001BC RID: 444
		public class TileBuilderOutput : IProgress, IDisposable
		{
			// Token: 0x17000200 RID: 512
			// (get) Token: 0x06000CBB RID: 3259 RVA: 0x00047AB4 File Offset: 0x00045CB4
			public float Progress
			{
				get
				{
					int area = this.tileMeshes.tileRect.Area;
					int num = Mathf.Min(area, this.currentTileCounter.Value);
					if (area <= 0)
					{
						return 0f;
					}
					return (float)num / (float)area;
				}
			}

			// Token: 0x06000CBC RID: 3260 RVA: 0x00047AF3 File Offset: 0x00045CF3
			public void Dispose()
			{
				this.tileMeshes.Dispose(Allocator.Persistent);
				if (this.currentTileCounter.IsCreated)
				{
					this.currentTileCounter.Dispose();
				}
			}

			// Token: 0x0400088A RID: 2186
			public NativeReference<int> currentTileCounter;

			// Token: 0x0400088B RID: 2187
			public TileMeshesUnsafe tileMeshes;
		}

		// Token: 0x020001BD RID: 445
		public struct BucketMapping
		{
			// Token: 0x0400088C RID: 2188
			public NativeArray<RasterizationMesh> meshes;

			// Token: 0x0400088D RID: 2189
			public NativeArray<int> pointers;

			// Token: 0x0400088E RID: 2190
			public NativeArray<int> bucketRanges;
		}

		// Token: 0x020001BE RID: 446
		// (Invoke) Token: 0x06000CBF RID: 3263
		internal delegate void PutMeshesIntoTileBuckets_00000BC6$PostfixBurstDelegate(ref UnsafeSpan<RasterizationMesh> meshes, ref IntRect tileBuckets, ref float4x4 worldToGraphMatrix, ref float2 tileSize, int borderExpansion, out UnsafeSpan<int> bucketRanges, out UnsafeSpan<int> pointers);

		// Token: 0x020001BF RID: 447
		internal static class PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall
		{
			// Token: 0x06000CC2 RID: 3266 RVA: 0x00047B19 File Offset: 0x00045D19
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TileBuilder.PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall.Pointer == 0)
				{
					TileBuilder.PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TileBuilder.PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall.DeferredCompilation, methodof(TileBuilder.PutMeshesIntoTileBuckets$BurstManaged(ref UnsafeSpan<RasterizationMesh>, ref IntRect, ref float4x4, ref float2, int, ref UnsafeSpan<int>, ref UnsafeSpan<int>)).MethodHandle, typeof(TileBuilder.PutMeshesIntoTileBuckets_00000BC6$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TileBuilder.PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall.Pointer;
			}

			// Token: 0x06000CC3 RID: 3267 RVA: 0x00047B48 File Offset: 0x00045D48
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				TileBuilder.PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000CC4 RID: 3268 RVA: 0x00047B60 File Offset: 0x00045D60
			public static void Constructor()
			{
				TileBuilder.PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TileBuilder.PutMeshesIntoTileBuckets(ref UnsafeSpan<RasterizationMesh>, ref IntRect, ref float4x4, ref float2, int, ref UnsafeSpan<int>, ref UnsafeSpan<int>)).MethodHandle);
			}

			// Token: 0x06000CC5 RID: 3269 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000CC6 RID: 3270 RVA: 0x00047B71 File Offset: 0x00045D71
			// Note: this type is marked as 'beforefieldinit'.
			static PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall()
			{
				TileBuilder.PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall.Constructor();
			}

			// Token: 0x06000CC7 RID: 3271 RVA: 0x00047B78 File Offset: 0x00045D78
			public static void Invoke(ref UnsafeSpan<RasterizationMesh> meshes, ref IntRect tileBuckets, ref float4x4 worldToGraphMatrix, ref float2 tileSize, int borderExpansion, out UnsafeSpan<int> bucketRanges, out UnsafeSpan<int> pointers)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TileBuilder.PutMeshesIntoTileBuckets_00000BC6$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Graphs.Navmesh.Voxelization.Burst.RasterizationMesh>&,Pathfinding.IntRect&,Unity.Mathematics.float4x4&,Unity.Mathematics.float2&,System.Int32,Pathfinding.Collections.UnsafeSpan`1<System.Int32>&,Pathfinding.Collections.UnsafeSpan`1<System.Int32>&), ref meshes, ref tileBuckets, ref worldToGraphMatrix, ref tileSize, borderExpansion, ref bucketRanges, ref pointers, functionPointer);
						return;
					}
				}
				TileBuilder.PutMeshesIntoTileBuckets$BurstManaged(ref meshes, ref tileBuckets, ref worldToGraphMatrix, ref tileSize, borderExpansion, out bucketRanges, out pointers);
			}

			// Token: 0x0400088F RID: 2191
			private static IntPtr Pointer;

			// Token: 0x04000890 RID: 2192
			private static IntPtr DeferredCompilation;
		}
	}
}
