using System;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001CF RID: 463
	public struct TileLayout
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x0004A7EF File Offset: 0x000489EF
		public float CellHeight
		{
			get
			{
				return Mathf.Max(this.graphSpaceSize.y / 64000f, 0.001f);
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0004A80C File Offset: 0x00048A0C
		public Vector2 TileWorldSize
		{
			get
			{
				return new Vector2(this.TileWorldSizeX, this.TileWorldSizeZ);
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x0004A81F File Offset: 0x00048A1F
		public float TileWorldSizeX
		{
			get
			{
				return (float)this.tileSizeInVoxels.x * this.cellSize;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0004A834 File Offset: 0x00048A34
		public float TileWorldSizeZ
		{
			get
			{
				return (float)this.tileSizeInVoxels.y * this.cellSize;
			}
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0004A84C File Offset: 0x00048A4C
		public Bounds GetTileBoundsInGraphSpace(int x, int z, int width = 1, int depth = 1)
		{
			Bounds bounds = default(Bounds);
			bounds.SetMinMax(new Vector3((float)x * this.TileWorldSizeX, 0f, (float)z * this.TileWorldSizeZ), new Vector3((float)(x + width) * this.TileWorldSizeX, this.graphSpaceSize.y, (float)(z + depth) * this.TileWorldSizeZ));
			return bounds;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0004A8AC File Offset: 0x00048AAC
		public IntRect GetTouchingTiles(Bounds bounds, float margin = 0f)
		{
			bounds = this.transform.InverseTransform(bounds);
			return new IntRect(Mathf.FloorToInt((bounds.min.x - margin) / this.TileWorldSizeX), Mathf.FloorToInt((bounds.min.z - margin) / this.TileWorldSizeZ), Mathf.FloorToInt((bounds.max.x + margin) / this.TileWorldSizeX), Mathf.FloorToInt((bounds.max.z + margin) / this.TileWorldSizeZ));
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0004A934 File Offset: 0x00048B34
		public static IntRect GetTouchingTiles(ref float4x4 worldToGraphTransform, float2 tileSize, Bounds bounds, float margin = 0f)
		{
			bounds = MathExtensions.BoundsOfTransformedBounds(worldToGraphTransform, bounds);
			int2 @int = (int2)math.floor((new float2(bounds.min.x, bounds.min.z) - margin) / tileSize);
			int2 int2 = (int2)math.floor((new float2(bounds.max.x, bounds.max.z) + margin) / tileSize);
			return new IntRect(@int.x, @int.y, int2.x, int2.y);
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0004A9D4 File Offset: 0x00048BD4
		public IntRect GetTouchingTilesInGraphSpace(Rect rect)
		{
			return IntRect.Intersection(new IntRect(Mathf.FloorToInt(rect.xMin / this.TileWorldSizeX), Mathf.FloorToInt(rect.yMin / this.TileWorldSizeZ), Mathf.FloorToInt(rect.xMax / this.TileWorldSizeX), Mathf.FloorToInt(rect.yMax / this.TileWorldSizeZ)), new IntRect(0, 0, this.tileCount.x - 1, this.tileCount.y - 1));
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0004AA58 File Offset: 0x00048C58
		public static TileLayout FromGraph(NavmeshBase graph)
		{
			RecastGraph recastGraph = graph as RecastGraph;
			if (recastGraph != null)
			{
				return new TileLayout(new Bounds(recastGraph.forcedBoundsCenter, recastGraph.forcedBoundsSize), Quaternion.Euler(recastGraph.rotation), recastGraph.cellSize, recastGraph.editorTileSize, recastGraph.useTiles);
			}
			NavMeshGraph navMeshGraph = graph as NavMeshGraph;
			if (navMeshGraph != null)
			{
				return new TileLayout(new Bounds(navMeshGraph.transform.Transform(navMeshGraph.forcedBoundsSize * 0.5f), navMeshGraph.forcedBoundsSize), Quaternion.Euler(navMeshGraph.rotation), 0.001f, 0, false);
			}
			throw new ArgumentException("The graph must be a RecastGraph or NavMeshGraph");
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0004AAF8 File Offset: 0x00048CF8
		public TileLayout(Bounds bounds, Quaternion rotation, float cellSize, int tileSizeInVoxels, bool useTiles)
		{
			this.transform = RecastGraph.CalculateTransform(bounds, rotation);
			this.cellSize = cellSize;
			Vector3 size = bounds.size;
			this.graphSpaceSize = size;
			int num = (int)(size.x / cellSize + 0.5f);
			int num2 = (int)(size.z / cellSize + 0.5f);
			if (!useTiles)
			{
				this.tileSizeInVoxels = new Vector2Int(num, num2);
				this.tileCount = new Vector2Int(1, 1);
			}
			else
			{
				this.tileSizeInVoxels = new Vector2Int(tileSizeInVoxels, tileSizeInVoxels);
				this.tileCount = new Vector2Int(Mathf.Max(0, (num + this.tileSizeInVoxels.x - 1) / this.tileSizeInVoxels.x), Mathf.Max(0, (num2 + this.tileSizeInVoxels.y - 1) / this.tileSizeInVoxels.y));
			}
			if (this.tileCount.x * this.tileCount.y > 524288)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Too many tiles (",
					(this.tileCount.x * this.tileCount.y).ToString(),
					") maximum is ",
					524288.ToString(),
					"\nTry disabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* inspector."
				}));
			}
		}

		// Token: 0x040008BD RID: 2237
		public Vector2Int tileCount;

		// Token: 0x040008BE RID: 2238
		public GraphTransform transform;

		// Token: 0x040008BF RID: 2239
		public Vector2Int tileSizeInVoxels;

		// Token: 0x040008C0 RID: 2240
		public Vector3 graphSpaceSize;

		// Token: 0x040008C1 RID: 2241
		public float cellSize;
	}
}
