using System;
using Unity.Collections;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x02000205 RID: 517
	public struct FlatGridAdjacencyMapper : GridAdjacencyMapper
	{
		// Token: 0x06000DC0 RID: 3520 RVA: 0x00019DC2 File Offset: 0x00017FC2
		public int LayerCount(IntBounds bounds)
		{
			return 1;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00053E57 File Offset: 0x00052057
		public int GetNeighbourIndex(int nodeIndexXZ, int nodeIndex, int direction, NativeArray<ulong> nodeConnections, NativeArray<int> neighbourOffsets, int layerStride)
		{
			return nodeIndex + neighbourOffsets[direction];
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00053E63 File Offset: 0x00052063
		public bool HasConnection(int nodeIndex, int direction, NativeArray<ulong> nodeConnections)
		{
			return ((nodeConnections[nodeIndex] >> direction) & 1UL) > 0UL;
		}
	}
}
