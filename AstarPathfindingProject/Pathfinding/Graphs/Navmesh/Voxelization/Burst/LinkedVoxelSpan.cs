using System;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001E3 RID: 483
	public struct LinkedVoxelSpan
	{
		// Token: 0x06000D69 RID: 3433 RVA: 0x0004D612 File Offset: 0x0004B812
		public LinkedVoxelSpan(uint bottom, uint top, int area)
		{
			this.bottom = bottom;
			this.top = top;
			this.area = area;
			this.next = -1;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0004D630 File Offset: 0x0004B830
		public LinkedVoxelSpan(uint bottom, uint top, int area, int next)
		{
			this.bottom = bottom;
			this.top = top;
			this.area = area;
			this.next = next;
		}

		// Token: 0x0400090E RID: 2318
		public uint bottom;

		// Token: 0x0400090F RID: 2319
		public uint top;

		// Token: 0x04000910 RID: 2320
		public int next;

		// Token: 0x04000911 RID: 2321
		public int area;
	}
}
