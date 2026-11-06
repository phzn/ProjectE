using System;
using Pathfinding.Collections;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Profiling;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001A8 RID: 424
	public class NavmeshTile : INavmeshHolder, ITransformedGraph
	{
		// Token: 0x06000C4C RID: 3148 RVA: 0x000441A3 File Offset: 0x000423A3
		public void GetTileCoordinates(int tileIndex, out int x, out int z)
		{
			x = this.x;
			z = this.z;
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00025EA6 File Offset: 0x000240A6
		public int GetVertexArrayIndex(int index)
		{
			return index & 4095;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x000441B8 File Offset: 0x000423B8
		public unsafe Int3 GetVertex(int index)
		{
			int num = index & 4095;
			return *this.verts[num];
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x000441DE File Offset: 0x000423DE
		[IgnoredByDeepProfiler]
		public unsafe Int3 GetVertexInGraphSpace(int index)
		{
			return *this.vertsInGraphSpace[index & 4095];
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x000441F7 File Offset: 0x000423F7
		public GraphTransform transform
		{
			get
			{
				return this.graph.transform;
			}
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00044204 File Offset: 0x00042404
		public void GetNodes(Action<GraphNode> action)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodes.Length; i++)
			{
				action(this.nodes[i]);
			}
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0004423C File Offset: 0x0004243C
		public void Dispose()
		{
			this.bbTree.Dispose();
			this.vertsInGraphSpace.Free(Allocator.Persistent);
			this.verts.Free(Allocator.Persistent);
			this.tris.Free(Allocator.Persistent);
			this.preCutTags.Free(Allocator.Persistent);
			this.preCutVertsInTileSpace.Free(Allocator.Persistent);
			this.preCutTris.Free(Allocator.Persistent);
			this.vertsInGraphSpace = default(UnsafeSpan<Int3>);
			this.verts = default(UnsafeSpan<Int3>);
			this.tris = default(UnsafeSpan<int>);
			this.preCutTags = default(UnsafeSpan<uint>);
			this.preCutVertsInTileSpace = default(UnsafeSpan<Int3>);
			this.preCutTris = default(UnsafeSpan<int>);
			this.isCut = false;
		}

		// Token: 0x04000822 RID: 2082
		public UnsafeSpan<Int3> vertsInGraphSpace;

		// Token: 0x04000823 RID: 2083
		public UnsafeSpan<Int3> verts;

		// Token: 0x04000824 RID: 2084
		public UnsafeSpan<int> tris;

		// Token: 0x04000825 RID: 2085
		public bool isCut;

		// Token: 0x04000826 RID: 2086
		public UnsafeSpan<Int3> preCutVertsInTileSpace;

		// Token: 0x04000827 RID: 2087
		public UnsafeSpan<int> preCutTris;

		// Token: 0x04000828 RID: 2088
		public UnsafeSpan<uint> preCutTags;

		// Token: 0x04000829 RID: 2089
		public int x;

		// Token: 0x0400082A RID: 2090
		public int z;

		// Token: 0x0400082B RID: 2091
		public int w;

		// Token: 0x0400082C RID: 2092
		public int d;

		// Token: 0x0400082D RID: 2093
		public TriangleMeshNode[] nodes;

		// Token: 0x0400082E RID: 2094
		public BBTree bbTree;

		// Token: 0x0400082F RID: 2095
		public bool flag;

		// Token: 0x04000830 RID: 2096
		public NavmeshBase graph;
	}
}
