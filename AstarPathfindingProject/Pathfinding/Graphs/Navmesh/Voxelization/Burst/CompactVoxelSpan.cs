using System;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001E0 RID: 480
	public struct CompactVoxelSpan
	{
		// Token: 0x06000D5D RID: 3421 RVA: 0x0004D023 File Offset: 0x0004B223
		public CompactVoxelSpan(ushort bottom, uint height)
		{
			this.con = 24U;
			this.y = bottom;
			this.h = height;
			this.reg = 0;
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0004D044 File Offset: 0x0004B244
		public void SetConnection(int dir, uint value)
		{
			int num = dir * 6;
			this.con = (uint)(((ulong)this.con & (ulong)(~(63L << (num & 31)))) | (ulong)((ulong)(value & 63U) << num));
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0004D078 File Offset: 0x0004B278
		public int GetConnection(int dir)
		{
			return ((int)this.con >> dir * 6) & 63;
		}

		// Token: 0x040008FC RID: 2300
		public ushort y;

		// Token: 0x040008FD RID: 2301
		public uint con;

		// Token: 0x040008FE RID: 2302
		public uint h;

		// Token: 0x040008FF RID: 2303
		public int reg;
	}
}
