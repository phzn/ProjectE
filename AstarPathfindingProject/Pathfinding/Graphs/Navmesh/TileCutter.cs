using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Sync;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001C0 RID: 448
	public struct TileCutter
	{
		// Token: 0x06000CC8 RID: 3272 RVA: 0x00047BBB File Offset: 0x00045DBB
		public TileCutter(NavmeshBase graph, GridLookup<NavmeshClipper> cuts, TileLayout tileLayout)
		{
			this.graph = graph;
			this.cuts = cuts;
			this.tileLayout = tileLayout;
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00047BD4 File Offset: 0x00045DD4
		private static void DisposeTileData(UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags, Allocator allocator, bool skipFirst)
		{
			for (int i = 0; i < tileVertices.Length; i++)
			{
				for (int j = (skipFirst ? 1 : 0); j < tileVertices[i].Length; j++)
				{
					tileVertices[i][j].Free(allocator);
					tileTriangles[i][j].Free(allocator);
					tileTags[i][j].Free(allocator);
				}
				tileVertices[i].Dispose();
				tileTriangles[i].Dispose();
				tileTags[i].Dispose();
			}
			tileVertices.Free(allocator);
			tileTriangles.Free(allocator);
			tileTags.Free(allocator);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00047C9C File Offset: 0x00045E9C
		public unsafe static void EnsurePreCutDataExists(NavmeshBase graph, NavmeshTile tile)
		{
			if (!tile.isCut)
			{
				tile.preCutTris = tile.tris.Clone(Allocator.Persistent);
				tile.preCutVertsInTileSpace = tile.vertsInGraphSpace.Clone(Allocator.Persistent);
				Int3 @int = (Int3)graph.GetTileBoundsInGraphSpace(tile.x, tile.z, 1, 1).min;
				for (int i = 0; i < tile.preCutVertsInTileSpace.Length; i++)
				{
					*tile.preCutVertsInTileSpace[i] -= @int;
				}
				tile.preCutTags = new UnsafeSpan<uint>(Allocator.Persistent, tile.nodes.Length);
				for (int j = 0; j < tile.nodes.Length; j++)
				{
					*tile.preCutTags[j] = tile.nodes[j].Tag;
				}
				tile.isCut = true;
			}
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00019DC2 File Offset: 0x00017FC2
		private static bool CheckVersion()
		{
			return true;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00047D78 File Offset: 0x00045F78
		public unsafe Promise<TileCutter.TileCutterOutput> Schedule(List<Vector2Int> tileCoordinates)
		{
			if (this.cuts == null)
			{
				return new Promise<TileCutter.TileCutterOutput>(default(JobHandle), default(TileCutter.TileCutterOutput));
			}
			int count = tileCoordinates.Count;
			UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> unsafeSpan = new UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>>(Allocator.Persistent, count);
			UnsafeSpan<UnsafeList<UnsafeSpan<int>>> unsafeSpan2 = new UnsafeSpan<UnsafeList<UnsafeSpan<int>>>(Allocator.Persistent, count);
			UnsafeSpan<UnsafeList<UnsafeSpan<int>>> unsafeSpan3 = new UnsafeSpan<UnsafeList<UnsafeSpan<int>>>(Allocator.Persistent, count);
			for (int i = 0; i < unsafeSpan.Length; i++)
			{
				*unsafeSpan[i] = new UnsafeList<UnsafeSpan<Int3>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				*unsafeSpan2[i] = new UnsafeList<UnsafeSpan<int>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				*unsafeSpan3[i] = new UnsafeList<UnsafeSpan<int>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			TileHandler.CutCollection cutCollection = TileHandler.CollectCuts(this.cuts, tileCoordinates, this.graph.NavmeshCuttingCharacterRadius, this.tileLayout, ref unsafeSpan, ref unsafeSpan2, ref unsafeSpan3);
			if (!cutCollection.cuttingRequired || !TileCutter.CheckVersion())
			{
				TileCutter.DisposeTileData(unsafeSpan, unsafeSpan2, unsafeSpan3, Allocator.Persistent, false);
				cutCollection.Dispose();
				return new Promise<TileCutter.TileCutterOutput>(default(JobHandle), default(TileCutter.TileCutterOutput));
			}
			NativeArray<TileMesh.TileMeshUnsafe> nativeArray = new NativeArray<TileMesh.TileMeshUnsafe>(count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int j = 0; j < tileCoordinates.Count; j++)
			{
				NavmeshTile tile = this.graph.GetTile(tileCoordinates[j].x, tileCoordinates[j].y);
				TileCutter.EnsurePreCutDataExists(this.graph, tile);
				nativeArray[j] = new TileMesh.TileMeshUnsafe
				{
					triangles = tile.preCutTris,
					verticesInTileSpace = tile.preCutVertsInTileSpace,
					tags = tile.preCutTags
				};
			}
			Vector2 vector = new Vector2(this.graph.TileWorldSizeX, this.graph.TileWorldSizeZ);
			TileMeshesUnsafe tileMeshesUnsafe = new TileMeshesUnsafe(nativeArray, new IntRect(0, 0, -1, -1), vector);
			NativeArray<TileMesh.TileMeshUnsafe> nativeArray2 = new NativeArray<TileMesh.TileMeshUnsafe>(count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			TileMeshesUnsafe tileMeshesUnsafe2 = new TileMeshesUnsafe(nativeArray2, new IntRect(0, 0, -1, -1), vector);
			TileCutter.TileCutterOutput tileCutterOutput = new TileCutter.TileCutterOutput
			{
				tileMeshes = tileMeshesUnsafe2
			};
			TileHandler.InitDelegates();
			JobHandle jobHandle = new TileCutter.JobCutTiles
			{
				tileVertices = unsafeSpan,
				tileTriangles = unsafeSpan2,
				tileTags = unsafeSpan3,
				cutCollection = cutCollection,
				inputTileMeshes = tileMeshesUnsafe,
				outputTileMeshes = nativeArray2
			}.Schedule(default(JobHandle));
			nativeArray.Dispose(jobHandle);
			return new Promise<TileCutter.TileCutterOutput>(jobHandle, tileCutterOutput);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00047FF8 File Offset: 0x000461F8
		public unsafe Promise<TileCutter.TileCutterOutput> Schedule(Promise<TileBuilder.TileBuilderOutput> builderOutput)
		{
			if (this.cuts == null)
			{
				return new Promise<TileCutter.TileCutterOutput>(builderOutput.handle, default(TileCutter.TileCutterOutput));
			}
			TileBuilder.TileBuilderOutput value = builderOutput.GetValue();
			IntRect tileRect = value.tileMeshes.tileRect;
			List<Vector2Int> innerCoordinates = tileRect.GetInnerCoordinates();
			int count = innerCoordinates.Count;
			UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> unsafeSpan = new UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>>(Allocator.Persistent, count);
			UnsafeSpan<UnsafeList<UnsafeSpan<int>>> unsafeSpan2 = new UnsafeSpan<UnsafeList<UnsafeSpan<int>>>(Allocator.Persistent, count);
			UnsafeSpan<UnsafeList<UnsafeSpan<int>>> unsafeSpan3 = new UnsafeSpan<UnsafeList<UnsafeSpan<int>>>(Allocator.Persistent, count);
			for (int i = 0; i < unsafeSpan.Length; i++)
			{
				*unsafeSpan[i] = new UnsafeList<UnsafeSpan<Int3>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				*unsafeSpan2[i] = new UnsafeList<UnsafeSpan<int>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				*unsafeSpan3[i] = new UnsafeList<UnsafeSpan<int>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			TileHandler.CutCollection cutCollection = TileHandler.CollectCuts(this.cuts, innerCoordinates, this.graph.NavmeshCuttingCharacterRadius, this.tileLayout, ref unsafeSpan, ref unsafeSpan2, ref unsafeSpan3);
			if (!cutCollection.cuttingRequired || !TileCutter.CheckVersion())
			{
				TileCutter.DisposeTileData(unsafeSpan, unsafeSpan2, unsafeSpan3, Allocator.Persistent, false);
				cutCollection.Dispose();
				return new Promise<TileCutter.TileCutterOutput>(builderOutput.handle, default(TileCutter.TileCutterOutput));
			}
			NativeArray<TileMesh.TileMeshUnsafe> nativeArray = new NativeArray<TileMesh.TileMeshUnsafe>(value.tileMeshes.tileMeshes.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			TileMeshesUnsafe tileMeshesUnsafe = new TileMeshesUnsafe(nativeArray, value.tileMeshes.tileRect, value.tileMeshes.tileWorldSize);
			TileCutter.TileCutterOutput tileCutterOutput = new TileCutter.TileCutterOutput
			{
				tileMeshes = tileMeshesUnsafe
			};
			TileHandler.InitDelegates();
			return new Promise<TileCutter.TileCutterOutput>(new TileCutter.JobCutTiles
			{
				tileVertices = unsafeSpan,
				tileTriangles = unsafeSpan2,
				tileTags = unsafeSpan3,
				cutCollection = cutCollection,
				inputTileMeshes = value.tileMeshes,
				outputTileMeshes = nativeArray
			}.Schedule(builderOutput.handle), tileCutterOutput);
		}

		// Token: 0x04000891 RID: 2193
		private NavmeshBase graph;

		// Token: 0x04000892 RID: 2194
		private GridLookup<NavmeshClipper> cuts;

		// Token: 0x04000893 RID: 2195
		private TileLayout tileLayout;

		// Token: 0x020001C1 RID: 449
		public struct TileCutterOutput : IProgress, IDisposable
		{
			// Token: 0x17000201 RID: 513
			// (get) Token: 0x06000CCE RID: 3278 RVA: 0x00005C0D File Offset: 0x00003E0D
			public float Progress
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x06000CCF RID: 3279 RVA: 0x000481CF File Offset: 0x000463CF
			public void Dispose()
			{
				this.tileMeshes.Dispose(Allocator.Persistent);
			}

			// Token: 0x04000894 RID: 2196
			public TileMeshesUnsafe tileMeshes;
		}

		// Token: 0x020001C2 RID: 450
		[BurstCompile]
		private struct JobCutTiles : IJob
		{
			// Token: 0x06000CD0 RID: 3280 RVA: 0x000481E0 File Offset: 0x000463E0
			public void Execute()
			{
				int length = this.inputTileMeshes.tileMeshes.Length;
				for (int i = 0; i < length; i++)
				{
					this.tileVertices[i].InsertRange(0, 1);
					this.tileTriangles[i].InsertRange(0, 1);
					this.tileTags[i].InsertRange(0, 1);
					this.tileVertices[i][0] = this.inputTileMeshes.tileMeshes[i].verticesInTileSpace;
					this.tileTriangles[i][0] = this.inputTileMeshes.tileMeshes[i].triangles;
					this.tileTags[i][0] = this.inputTileMeshes.tileMeshes[i].tags.Reinterpret<int>();
				}
				UnsafeSpan<TileMesh.TileMeshUnsafe> unsafeSpan = this.outputTileMeshes.AsUnsafeSpan<TileMesh.TileMeshUnsafe>();
				Vector2Int vector2Int = new Vector2Int(Mathf.RoundToInt(this.inputTileMeshes.tileWorldSize.x * 1000f), Mathf.RoundToInt(this.inputTileMeshes.tileWorldSize.y * 1000f));
				TileHandler.CutTiles(ref this.tileVertices, ref this.tileTriangles, ref this.tileTags, ref vector2Int, ref this.cutCollection, ref unsafeSpan, Allocator.Persistent);
				TileCutter.DisposeTileData(this.tileVertices, this.tileTriangles, this.tileTags, Allocator.Persistent, true);
				this.cutCollection.Dispose();
			}

			// Token: 0x04000895 RID: 2197
			public UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices;

			// Token: 0x04000896 RID: 2198
			public UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles;

			// Token: 0x04000897 RID: 2199
			public UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags;

			// Token: 0x04000898 RID: 2200
			public TileHandler.CutCollection cutCollection;

			// Token: 0x04000899 RID: 2201
			public TileMeshesUnsafe inputTileMeshes;

			// Token: 0x0400089A RID: 2202
			public NativeArray<TileMesh.TileMeshUnsafe> outputTileMeshes;
		}
	}
}
