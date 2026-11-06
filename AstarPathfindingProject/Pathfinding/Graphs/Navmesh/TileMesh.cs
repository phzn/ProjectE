using System;
using Pathfinding.Collections;
using Unity.Collections;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001D0 RID: 464
	public struct TileMesh
	{
		// Token: 0x06000D10 RID: 3344 RVA: 0x0004AC40 File Offset: 0x00048E40
		public void Validate()
		{
			for (int i = 0; i < this.triangles.Length; i++)
			{
			}
		}

		// Token: 0x040008C2 RID: 2242
		public int[] triangles;

		// Token: 0x040008C3 RID: 2243
		public Int3[] verticesInTileSpace;

		// Token: 0x040008C4 RID: 2244
		public uint[] tags;

		// Token: 0x020001D1 RID: 465
		public struct TileMeshUnsafe
		{
			// Token: 0x06000D11 RID: 3345 RVA: 0x0004AC60 File Offset: 0x00048E60
			public void Dispose(Allocator allocator)
			{
				this.triangles.Free(allocator);
				this.verticesInTileSpace.Free(allocator);
				this.tags.Free(allocator);
			}

			// Token: 0x06000D12 RID: 3346 RVA: 0x0004AC88 File Offset: 0x00048E88
			public TileMesh ToManaged()
			{
				return new TileMesh
				{
					triangles = this.triangles.ToArray(),
					verticesInTileSpace = this.verticesInTileSpace.ToArray(),
					tags = this.tags.ToArray()
				};
			}

			// Token: 0x040008C5 RID: 2245
			public UnsafeSpan<int> triangles;

			// Token: 0x040008C6 RID: 2246
			public UnsafeSpan<Int3> verticesInTileSpace;

			// Token: 0x040008C7 RID: 2247
			public UnsafeSpan<uint> tags;
		}
	}
}
