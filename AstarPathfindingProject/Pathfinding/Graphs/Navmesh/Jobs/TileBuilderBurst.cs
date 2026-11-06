using System;
using Pathfinding.Graphs.Navmesh.Voxelization.Burst;
using Pathfinding.Jobs;
using Unity.Collections;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001F7 RID: 503
	public struct TileBuilderBurst : IArenaDisposable
	{
		// Token: 0x06000DA2 RID: 3490 RVA: 0x000524E8 File Offset: 0x000506E8
		public TileBuilderBurst(int width, int depth, int voxelWalkableHeight, int maximumVoxelYCoord)
		{
			this.linkedVoxelField = new LinkedVoxelField(width, depth, maximumVoxelYCoord);
			this.compactVoxelField = new CompactVoxelField(width, depth, voxelWalkableHeight, Allocator.Persistent);
			this.tmpQueue1 = new NativeQueue<Int3>(Allocator.Persistent);
			this.tmpQueue2 = new NativeQueue<Int3>(Allocator.Persistent);
			this.distanceField = new NativeList<ushort>(0, Allocator.Persistent);
			this.contours = new NativeList<VoxelContour>(Allocator.Persistent);
			this.contourVertices = new NativeList<int>(Allocator.Persistent);
			this.voxelMesh = new VoxelMesh
			{
				verts = new NativeList<Int3>(Allocator.Persistent),
				tris = new NativeList<int>(Allocator.Persistent),
				areas = new NativeList<int>(Allocator.Persistent)
			};
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x000525B0 File Offset: 0x000507B0
		void IArenaDisposable.DisposeWith(DisposeArena arena)
		{
			arena.Add<LinkedVoxelField>(this.linkedVoxelField);
			arena.Add<CompactVoxelField>(this.compactVoxelField);
			arena.Add<ushort>(this.distanceField);
			arena.Add<Int3>(this.tmpQueue1);
			arena.Add<Int3>(this.tmpQueue2);
			arena.Add<VoxelContour>(this.contours);
			arena.Add<int>(this.contourVertices);
			arena.Add<VoxelMesh>(this.voxelMesh);
		}

		// Token: 0x04000972 RID: 2418
		public LinkedVoxelField linkedVoxelField;

		// Token: 0x04000973 RID: 2419
		public CompactVoxelField compactVoxelField;

		// Token: 0x04000974 RID: 2420
		public NativeList<ushort> distanceField;

		// Token: 0x04000975 RID: 2421
		public NativeQueue<Int3> tmpQueue1;

		// Token: 0x04000976 RID: 2422
		public NativeQueue<Int3> tmpQueue2;

		// Token: 0x04000977 RID: 2423
		public NativeList<VoxelContour> contours;

		// Token: 0x04000978 RID: 2424
		public NativeList<int> contourVertices;

		// Token: 0x04000979 RID: 2425
		public VoxelMesh voxelMesh;
	}
}
