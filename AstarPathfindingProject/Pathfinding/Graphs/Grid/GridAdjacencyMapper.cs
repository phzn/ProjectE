using System;
using Unity.Collections;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x02000204 RID: 516
	public interface GridAdjacencyMapper
	{
		// Token: 0x06000DBD RID: 3517
		int LayerCount(IntBounds bounds);

		// Token: 0x06000DBE RID: 3518
		int GetNeighbourIndex(int nodeIndexXZ, int nodeIndex, int direction, NativeArray<ulong> nodeConnections, NativeArray<int> neighbourOffsets, int layerStride);

		// Token: 0x06000DBF RID: 3519
		bool HasConnection(int nodeIndex, int direction, NativeArray<ulong> nodeConnections);
	}
}
