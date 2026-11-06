using System;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh.Jobs;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001A9 RID: 425
	public struct RecastBuilder
	{
		// Token: 0x06000C54 RID: 3156 RVA: 0x000442EB File Offset: 0x000424EB
		public static TileBuilder BuildTileMeshes(RecastGraph graph, TileLayout tileLayout, IntRect tileRect)
		{
			return new TileBuilder(graph, tileLayout, tileRect);
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x000442F5 File Offset: 0x000424F5
		public static JobBuildNodes BuildNodeTiles(RecastGraph graph, TileLayout tileLayout)
		{
			return new JobBuildNodes(graph, tileLayout);
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x000442FE File Offset: 0x000424FE
		public static TileCutter CutTiles(NavmeshBase graph, GridLookup<NavmeshClipper> cuts, TileLayout tileLayout)
		{
			return new TileCutter(graph, cuts, tileLayout);
		}
	}
}
