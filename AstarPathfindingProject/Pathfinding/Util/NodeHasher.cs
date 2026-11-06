using System;
using Pathfinding.Drawing;

namespace Pathfinding.Util
{
	// Token: 0x020002A5 RID: 677
	public struct NodeHasher
	{
		// Token: 0x060010F4 RID: 4340 RVA: 0x00066200 File Offset: 0x00064400
		public NodeHasher(AstarPath active)
		{
			this.hasher = default(DrawingData.Hasher);
			this.debugData = active.debugPathData;
			this.includePathSearchInfo = this.debugData != null && (active.debugMode == GraphDebugMode.F || active.debugMode == GraphDebugMode.G || active.debugMode == GraphDebugMode.H || active.showSearchTree);
			this.includeAreaInfo = active.debugMode == GraphDebugMode.Areas;
			this.includeHierarchicalNodeInfo = active.debugMode == GraphDebugMode.HierarchicalNode;
			this.hasher.Add<GraphDebugMode>(active.debugMode);
			this.hasher.Add<float>(active.debugFloor);
			this.hasher.Add<float>(active.debugRoof);
			this.hasher.Add<bool>(active.showSearchTree);
			this.hasher.Add<int>(AstarColor.ColorHash());
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x000662CC File Offset: 0x000644CC
		public unsafe void HashNode(GraphNode node)
		{
			this.hasher.Add<int>(node.GetGizmoHashCode());
			if (this.includeAreaInfo)
			{
				this.hasher.Add<int>((int)node.Area);
			}
			if (this.includeHierarchicalNodeInfo)
			{
				this.hasher.Add<int>(node.HierarchicalNodeIndex);
			}
			if (this.includePathSearchInfo)
			{
				PathNode pathNode = *this.debugData.pathNodes[node.NodeIndex];
				this.hasher.Add<ushort>(pathNode.pathID);
				this.hasher.Add<bool>(pathNode.pathID == this.debugData.PathID);
			}
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0006636F File Offset: 0x0006456F
		public void Add<T>(T v)
		{
			this.hasher.Add<T>(v);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0006637D File Offset: 0x0006457D
		public static implicit operator DrawingData.Hasher(NodeHasher hasher)
		{
			return hasher.hasher;
		}

		// Token: 0x04000C2A RID: 3114
		private readonly bool includePathSearchInfo;

		// Token: 0x04000C2B RID: 3115
		private readonly bool includeAreaInfo;

		// Token: 0x04000C2C RID: 3116
		private readonly bool includeHierarchicalNodeInfo;

		// Token: 0x04000C2D RID: 3117
		private readonly PathHandler debugData;

		// Token: 0x04000C2E RID: 3118
		public DrawingData.Hasher hasher;
	}
}
