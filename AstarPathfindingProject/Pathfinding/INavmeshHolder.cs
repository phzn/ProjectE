using System;

namespace Pathfinding
{
	// Token: 0x020000EF RID: 239
	public interface INavmeshHolder : ITransformedGraph
	{
		// Token: 0x0600087D RID: 2173
		void GetNodes(Action<GraphNode> del);

		// Token: 0x0600087E RID: 2174
		Int3 GetVertex(int i);

		// Token: 0x0600087F RID: 2175
		Int3 GetVertexInGraphSpace(int i);

		// Token: 0x06000880 RID: 2176
		int GetVertexArrayIndex(int index);

		// Token: 0x06000881 RID: 2177
		void GetTileCoordinates(int tileIndex, out int x, out int z);
	}
}
