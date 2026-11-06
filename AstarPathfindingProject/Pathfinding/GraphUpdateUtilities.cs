using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Pathfinding.Util;

namespace Pathfinding
{
	// Token: 0x02000156 RID: 342
	public static class GraphUpdateUtilities
	{
		// Token: 0x06000AF5 RID: 2805 RVA: 0x0003AB9C File Offset: 0x00038D9C
		public static bool UpdateGraphsNoBlock(GraphUpdateObject guo, GraphNode node1, GraphNode node2, bool alwaysRevert = false)
		{
			List<GraphNode> list = ListPool<GraphNode>.Claim();
			list.Add(node1);
			list.Add(node2);
			bool flag = GraphUpdateUtilities.UpdateGraphsNoBlock(guo, list, alwaysRevert);
			ListPool<GraphNode>.Release(ref list);
			return flag;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0003ABCC File Offset: 0x00038DCC
		public static bool UpdateGraphsNoBlock(GraphUpdateObject guo, List<GraphNode> nodes, bool alwaysRevert = false)
		{
			PathProcessor.GraphUpdateLock graphUpdateLock = AstarPath.active.PausePathfinding();
			bool flag;
			try
			{
				AstarPath.active.FlushGraphUpdates();
				for (int i = 0; i < nodes.Count; i++)
				{
					if (!nodes[i].Walkable)
					{
						return false;
					}
				}
				GraphSnapshot graphSnapshot = AstarPath.active.Snapshot(guo.bounds, guo.graphMask);
				AstarPath.active.UpdateGraphs(guo);
				AstarPath.active.FlushGraphUpdates();
				flag = PathUtilities.IsPathPossible(nodes);
				if (!flag || alwaysRevert)
				{
					AstarPath.active.AddWorkItem(new Action<IWorkItemContext>(graphSnapshot.Restore));
					AstarPath.active.FlushWorkItems();
				}
				graphSnapshot.Dispose();
			}
			finally
			{
				graphUpdateLock.Release();
			}
			return flag;
		}
	}
}
