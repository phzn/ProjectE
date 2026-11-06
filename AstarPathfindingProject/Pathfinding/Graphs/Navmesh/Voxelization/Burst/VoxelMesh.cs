using System;
using Pathfinding.Jobs;
using Unity.Collections;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001E6 RID: 486
	public struct VoxelMesh : IArenaDisposable
	{
		// Token: 0x06000D75 RID: 3445 RVA: 0x0004EB92 File Offset: 0x0004CD92
		void IArenaDisposable.DisposeWith(DisposeArena arena)
		{
			arena.Add<Int3>(this.verts);
			arena.Add<int>(this.tris);
			arena.Add<int>(this.areas);
		}

		// Token: 0x0400091D RID: 2333
		public NativeList<Int3> verts;

		// Token: 0x0400091E RID: 2334
		public NativeList<int> tris;

		// Token: 0x0400091F RID: 2335
		public NativeList<int> areas;
	}
}
