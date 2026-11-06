using System;
using Unity.Collections;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x02000206 RID: 518
	public struct LayeredGridAdjacencyMapper : GridAdjacencyMapper
	{
		// Token: 0x06000DC3 RID: 3523 RVA: 0x00053E79 File Offset: 0x00052079
		public int LayerCount(IntBounds bounds)
		{
			return bounds.size.y;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00053E87 File Offset: 0x00052087
		public int GetNeighbourIndex(int nodeIndexXZ, int nodeIndex, int direction, NativeArray<ulong> nodeConnections, NativeArray<int> neighbourOffsets, int layerStride)
		{
			return nodeIndexXZ + neighbourOffsets[direction] + (int)((nodeConnections[nodeIndex] >> 4 * direction) & 15UL) * layerStride;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00053EAB File Offset: 0x000520AB
		public bool HasConnection(int nodeIndex, int direction, NativeArray<ulong> nodeConnections)
		{
			return ((nodeConnections[nodeIndex] >> 4 * direction) & 15UL) != 15UL;
		}
	}
}
