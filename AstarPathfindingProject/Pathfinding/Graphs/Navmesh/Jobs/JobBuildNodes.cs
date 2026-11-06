using System;
using System.Runtime.InteropServices;
using Pathfinding.Jobs;
using Pathfinding.Sync;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001F3 RID: 499
	public struct JobBuildNodes
	{
		// Token: 0x06000D9A RID: 3482 RVA: 0x00051FCC File Offset: 0x000501CC
		internal JobBuildNodes(RecastGraph graph, TileLayout tileLayout)
		{
			this.tileLayout = tileLayout;
			this.graphIndex = graph.graphIndex;
			this.initialPenalty = graph.initialPenalty;
			this.recalculateNormals = graph.RecalculateNormals;
			this.maxTileConnectionEdgeDistance = graph.MaxTileConnectionEdgeDistance;
			this.graphToWorldSpace = tileLayout.transform.matrix;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00052024 File Offset: 0x00050224
		public Promise<JobBuildNodes.BuildNodeTilesOutput> Schedule(DisposeArena arena, Promise<TileBuilder.TileBuilderOutput> preCutDependency, Promise<TileCutter.TileCutterOutput> postCutDependency)
		{
			TileCutter.TileCutterOutput value = postCutDependency.GetValue();
			TileBuilder.TileBuilderOutput value2 = preCutDependency.GetValue();
			IntRect tileRect = value2.tileMeshes.tileRect;
			NativeArray<TileMesh.TileMeshUnsafe> nativeArray;
			if (value.tileMeshes.tileMeshes.IsCreated)
			{
				nativeArray = value.tileMeshes.tileMeshes;
			}
			else
			{
				nativeArray = value2.tileMeshes.tileMeshes;
			}
			NavmeshTile[] array = new NavmeshTile[tileRect.Area];
			GCHandle gchandle = GCHandle.Alloc(array);
			NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe> nativeArray2 = new NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe>(tileRect.Area, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			JobHandle jobHandle = new JobCalculateTriangleConnections
			{
				tileMeshes = nativeArray,
				nodeConnections = nativeArray2
			}.Schedule(postCutDependency.handle);
			Vector2 vector = new Vector2(this.tileLayout.TileWorldSizeX, this.tileLayout.TileWorldSizeZ);
			JobHandle jobHandle2 = new JobCreateTiles
			{
				preCutTileMeshes = (value.tileMeshes.tileMeshes.IsCreated ? value2.tileMeshes.tileMeshes : default(NativeArray<TileMesh.TileMeshUnsafe>)),
				tileMeshes = nativeArray,
				tiles = gchandle,
				tileRect = tileRect,
				graphTileCount = this.tileLayout.tileCount,
				graphIndex = this.graphIndex,
				initialPenalty = this.initialPenalty,
				recalculateNormals = this.recalculateNormals,
				graphToWorldSpace = this.graphToWorldSpace,
				tileWorldSize = vector
			}.Schedule(postCutDependency.handle);
			JobHandle jobHandle3 = new JobWriteNodeConnections
			{
				nodeConnections = nativeArray2,
				tiles = gchandle
			}.Schedule(JobHandle.CombineDependencies(jobHandle, jobHandle2));
			JobHandle jobHandle4 = JobConnectTiles.ScheduleBatch(gchandle, jobHandle3, tileRect, vector, this.maxTileConnectionEdgeDistance);
			arena.Add(gchandle);
			arena.Add<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe>(nativeArray2);
			return new Promise<JobBuildNodes.BuildNodeTilesOutput>(jobHandle4, new JobBuildNodes.BuildNodeTilesOutput
			{
				progressSource = value2,
				tiles = array
			});
		}

		// Token: 0x04000962 RID: 2402
		private uint graphIndex;

		// Token: 0x04000963 RID: 2403
		public uint initialPenalty;

		// Token: 0x04000964 RID: 2404
		public bool recalculateNormals;

		// Token: 0x04000965 RID: 2405
		public float maxTileConnectionEdgeDistance;

		// Token: 0x04000966 RID: 2406
		private Matrix4x4 graphToWorldSpace;

		// Token: 0x04000967 RID: 2407
		private TileLayout tileLayout;

		// Token: 0x020001F4 RID: 500
		public class BuildNodeTilesOutput : IProgress, IDisposable
		{
			// Token: 0x1700020F RID: 527
			// (get) Token: 0x06000D9C RID: 3484 RVA: 0x000521FD File Offset: 0x000503FD
			public float Progress
			{
				get
				{
					return this.progressSource.Progress;
				}
			}

			// Token: 0x06000D9D RID: 3485 RVA: 0x00003788 File Offset: 0x00001988
			public void Dispose()
			{
			}

			// Token: 0x04000968 RID: 2408
			public TileBuilder.TileBuilderOutput progressSource;

			// Token: 0x04000969 RID: 2409
			public NavmeshTile[] tiles;
		}
	}
}
