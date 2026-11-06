using System;
using System.Runtime.CompilerServices;
using System.Text;
using Pathfinding.Collections;
using Unity.Collections;
using Unity.Profiling;

namespace Pathfinding
{
	// Token: 0x020000BF RID: 191
	public class PathHandler
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0001DD3F File Offset: 0x0001BF3F
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x0001DD47 File Offset: 0x0001BF47
		public int numTemporaryNodes
		{
			[IgnoredByDeepProfiler]
			get;
			private set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0001DD50 File Offset: 0x0001BF50
		// (set) Token: 0x0600065C RID: 1628 RVA: 0x0001DD58 File Offset: 0x0001BF58
		public uint temporaryNodeStartIndex
		{
			[IgnoredByDeepProfiler]
			get;
			private set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001DD61 File Offset: 0x0001BF61
		public ushort PathID
		{
			get
			{
				return this.pathID;
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001DD6C File Offset: 0x0001BF6C
		internal PathHandler(GlobalNodeStorage nodeStorage, int threadID, int totalThreadCount)
		{
			this.threadID = threadID;
			this.totalThreadCount = totalThreadCount;
			this.nodeStorage = nodeStorage;
			this.temporaryNodes = default(UnsafeSpan<TemporaryNode>);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001DDBC File Offset: 0x0001BFBC
		public void InitializeForPath(Path p)
		{
			ushort num = this.pathID;
			this.pathID = p.pathID;
			this.numTemporaryNodes = 0;
			this.pathNodes = this.nodeStorage.pathfindingThreadData[this.threadID].pathNodes;
			this.temporaryNodeStartIndex = this.nodeStorage.reservedPathNodeData;
			int num2 = this.pathNodes.Length - (int)this.temporaryNodeStartIndex;
			if (num2 > this.temporaryNodes.Length)
			{
				this.temporaryNodes = this.temporaryNodes.Reallocate(Allocator.Persistent, num2);
			}
			if (this.pathID < num)
			{
				this.ClearPathIDs();
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001DE58 File Offset: 0x0001C058
		public unsafe PathNode GetPathNode(GraphNode node, uint variant = 0U)
		{
			return *this.pathNodes[node.NodeIndex + variant];
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001DE72 File Offset: 0x0001C072
		public bool IsTemporaryNode(uint pathNodeIndex)
		{
			return pathNodeIndex >= this.temporaryNodeStartIndex;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001DE80 File Offset: 0x0001C080
		public unsafe uint AddTemporaryNode(TemporaryNode node)
		{
			if (this.numTemporaryNodes >= this.temporaryNodes.Length)
			{
				this.nodeStorage.GrowTemporaryNodeStorage(this.threadID);
				this.pathNodes = this.nodeStorage.pathfindingThreadData[this.threadID].pathNodes;
				this.temporaryNodes = this.temporaryNodes.Reallocate(Allocator.Persistent, this.pathNodes.Length - (int)this.temporaryNodeStartIndex);
			}
			uint num = this.temporaryNodeStartIndex + (uint)this.numTemporaryNodes;
			*this.temporaryNodes[this.numTemporaryNodes] = node;
			*this.pathNodes[num] = PathNode.Default;
			int numTemporaryNodes = this.numTemporaryNodes;
			this.numTemporaryNodes = numTemporaryNodes + 1;
			return num;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001DF42 File Offset: 0x0001C142
		public GraphNode GetNode(uint nodeIndex)
		{
			return this.nodeStorage.GetNode(nodeIndex);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001DF50 File Offset: 0x0001C150
		public ref TemporaryNode GetTemporaryNode(uint nodeIndex)
		{
			if (nodeIndex < this.temporaryNodeStartIndex || (ulong)nodeIndex >= (ulong)this.temporaryNodeStartIndex + (ulong)((long)this.numTemporaryNodes))
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.temporaryNodes[(int)(nodeIndex - this.temporaryNodeStartIndex)];
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00003788 File Offset: 0x00001988
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void LogVisitedNode(uint pathNodeIndex, uint h, uint g)
		{
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0001DF88 File Offset: 0x0001C188
		public void ClearPathIDs()
		{
			for (int i = 0; i < this.pathNodes.Length; i++)
			{
				this.pathNodes[i].pathID = 0;
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001DFBD File Offset: 0x0001C1BD
		public void Dispose()
		{
			this.heap.Dispose();
			this.temporaryNodes.Free(Allocator.Persistent);
			this.pathNodes = default(UnsafeSpan<PathNode>);
		}

		// Token: 0x0400040C RID: 1036
		private ushort pathID;

		// Token: 0x0400040D RID: 1037
		public readonly int threadID;

		// Token: 0x0400040E RID: 1038
		public readonly int totalThreadCount;

		// Token: 0x0400040F RID: 1039
		internal readonly GlobalNodeStorage nodeStorage;

		// Token: 0x04000412 RID: 1042
		private UnsafeSpan<TemporaryNode> temporaryNodes;

		// Token: 0x04000413 RID: 1043
		public UnsafeSpan<PathNode> pathNodes;

		// Token: 0x04000414 RID: 1044
		public BinaryHeap heap = new BinaryHeap(128);

		// Token: 0x04000415 RID: 1045
		public readonly StringBuilder DebugStringBuilder = new StringBuilder();
	}
}
