using System;
using System.Runtime.InteropServices;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001FC RID: 508
	internal struct JobConnectTilesSingle : IJob
	{
		// Token: 0x06000DAD RID: 3501 RVA: 0x000531E8 File Offset: 0x000513E8
		public void Execute()
		{
			NavmeshTile[] array = (NavmeshTile[])this.tiles.Target;
			NavmeshBase.ConnectTiles(array[this.tileIndex1], array[this.tileIndex2], this.tileWorldSize.x, this.tileWorldSize.y, this.maxTileConnectionEdgeDistance);
		}

		// Token: 0x040009AB RID: 2475
		public GCHandle tiles;

		// Token: 0x040009AC RID: 2476
		public int tileIndex1;

		// Token: 0x040009AD RID: 2477
		public int tileIndex2;

		// Token: 0x040009AE RID: 2478
		public Vector2 tileWorldSize;

		// Token: 0x040009AF RID: 2479
		public float maxTileConnectionEdgeDistance;
	}
}
