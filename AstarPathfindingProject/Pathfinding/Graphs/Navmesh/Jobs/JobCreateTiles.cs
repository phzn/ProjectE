using System;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001FE RID: 510
	public struct JobCreateTiles : IJob
	{
		// Token: 0x06000DAF RID: 3503 RVA: 0x0005328C File Offset: 0x0005148C
		public unsafe void Execute()
		{
			NavmeshTile[] array = (NavmeshTile[])this.tiles.Target;
			int width = this.tileRect.Width;
			int height = this.tileRect.Height;
			bool isCreated = this.preCutTileMeshes.IsCreated;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int num = i * width + j;
					int num2 = (i + this.tileRect.ymin) * this.graphTileCount.x + (j + this.tileRect.xmin);
					TileMesh.TileMeshUnsafe tileMeshUnsafe = this.tileMeshes[num];
					TileMesh.TileMeshUnsafe tileMeshUnsafe2 = (isCreated ? this.preCutTileMeshes[num] : default(TileMesh.TileMeshUnsafe));
					int num3 = math.max(tileMeshUnsafe.verticesInTileSpace.Length, tileMeshUnsafe2.verticesInTileSpace.Length);
					if (num3 > 4095)
					{
						Debug.LogError(string.Format("Too many vertices in the tile ({0} > {1})\nYou can enable ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* Inspector to raise this limit. Or you can use a higher simplification level (RecastGraph.contourMaxError) or a smaller tile size to reduce the likelihood of this happening (a tile size between 64 and 256 is recommended, but you can go outside it if necessary). The affected tile will be left empty.", num3, 4095));
						tileMeshUnsafe = default(TileMesh.TileMeshUnsafe);
						tileMeshUnsafe2 = default(TileMesh.TileMeshUnsafe);
					}
					UnsafeSpan<Int3> unsafeSpan = tileMeshUnsafe.verticesInTileSpace.Clone(Allocator.Persistent);
					UnsafeSpan<Int3> unsafeSpan2 = unsafeSpan.Clone(Allocator.Persistent);
					Int3 @int = (Int3)new Vector3(this.tileWorldSize.x * (float)(j + this.tileRect.xmin), 0f, this.tileWorldSize.y * (float)(i + this.tileRect.ymin));
					for (int k = 0; k < unsafeSpan.Length; k++)
					{
						Int3 int2 = *unsafeSpan[k] + @int;
						*unsafeSpan[k] = int2;
						*unsafeSpan2[k] = (Int3)this.graphToWorldSpace.MultiplyPoint3x4((Vector3)int2);
					}
					UnsafeSpan<int> unsafeSpan3 = tileMeshUnsafe.triangles.Clone(Allocator.Persistent);
					NavmeshTile navmeshTile = new NavmeshTile
					{
						x = j + this.tileRect.xmin,
						z = i + this.tileRect.ymin,
						w = 1,
						d = 1,
						tris = unsafeSpan3,
						vertsInGraphSpace = unsafeSpan,
						verts = unsafeSpan2,
						bbTree = new BBTree(unsafeSpan3, unsafeSpan),
						nodes = new TriangleMeshNode[unsafeSpan3.Length / 3],
						graph = null,
						isCut = false
					};
					if (isCreated)
					{
						navmeshTile.preCutVertsInTileSpace = tileMeshUnsafe2.verticesInTileSpace.Clone(Allocator.Persistent);
						navmeshTile.preCutTris = tileMeshUnsafe2.triangles.Clone(Allocator.Persistent);
						navmeshTile.preCutTags = tileMeshUnsafe2.tags.Clone(Allocator.Persistent);
						navmeshTile.isCut = true;
					}
					NavmeshBase.CreateNodes(navmeshTile, navmeshTile.tris, num2, this.graphIndex, tileMeshUnsafe.tags, false, null, this.initialPenalty, false);
					array[num] = navmeshTile;
				}
			}
		}

		// Token: 0x040009B1 RID: 2481
		[ReadOnly]
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<TileMesh.TileMeshUnsafe> preCutTileMeshes;

		// Token: 0x040009B2 RID: 2482
		[ReadOnly]
		public NativeArray<TileMesh.TileMeshUnsafe> tileMeshes;

		// Token: 0x040009B3 RID: 2483
		public GCHandle tiles;

		// Token: 0x040009B4 RID: 2484
		public uint graphIndex;

		// Token: 0x040009B5 RID: 2485
		public Vector2Int graphTileCount;

		// Token: 0x040009B6 RID: 2486
		public IntRect tileRect;

		// Token: 0x040009B7 RID: 2487
		public uint initialPenalty;

		// Token: 0x040009B8 RID: 2488
		public bool recalculateNormals;

		// Token: 0x040009B9 RID: 2489
		public Vector2 tileWorldSize;

		// Token: 0x040009BA RID: 2490
		public Matrix4x4 graphToWorldSpace;
	}
}
