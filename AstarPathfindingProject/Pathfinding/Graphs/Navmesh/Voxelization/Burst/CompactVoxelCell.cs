using System;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001DF RID: 479
	public struct CompactVoxelCell
	{
		// Token: 0x06000D5C RID: 3420 RVA: 0x0004D013 File Offset: 0x0004B213
		public CompactVoxelCell(int i, int c)
		{
			this.index = i;
			this.count = c;
		}

		// Token: 0x040008FA RID: 2298
		public int index;

		// Token: 0x040008FB RID: 2299
		public int count;
	}
}
