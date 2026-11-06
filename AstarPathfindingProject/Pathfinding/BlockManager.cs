using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000143 RID: 323
	[HelpURL("https://arongranberg.com/astar/documentation/stable/blockmanager.html")]
	public class BlockManager : VersionedMonoBehaviour
	{
		// Token: 0x06000A86 RID: 2694 RVA: 0x0003869E File Offset: 0x0003689E
		private void Start()
		{
			if (!AstarPath.active)
			{
				throw new Exception("No AstarPath object in the scene");
			}
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x000386B8 File Offset: 0x000368B8
		public bool NodeContainsAnyOf(GraphNode node, List<SingleNodeBlocker> selector)
		{
			List<SingleNodeBlocker> list;
			if (!this.blocked.TryGetValue(node, out list))
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				SingleNodeBlocker singleNodeBlocker = list[i];
				for (int j = 0; j < selector.Count; j++)
				{
					if (singleNodeBlocker == selector[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00038710 File Offset: 0x00036910
		public bool NodeContainsAnyExcept(GraphNode node, List<SingleNodeBlocker> selector)
		{
			List<SingleNodeBlocker> list;
			if (!this.blocked.TryGetValue(node, out list))
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				SingleNodeBlocker singleNodeBlocker = list[i];
				bool flag = false;
				for (int j = 0; j < selector.Count; j++)
				{
					if (singleNodeBlocker == selector[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00038774 File Offset: 0x00036974
		public void InternalBlock(GraphNode node, SingleNodeBlocker blocker)
		{
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate
			{
				List<SingleNodeBlocker> list;
				if (!this.blocked.TryGetValue(node, out list))
				{
					list = (this.blocked[node] = ListPool<SingleNodeBlocker>.Claim());
				}
				list.Add(blocker);
			}, null));
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x000387B8 File Offset: 0x000369B8
		public void InternalUnblock(GraphNode node, SingleNodeBlocker blocker)
		{
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate
			{
				List<SingleNodeBlocker> list;
				if (this.blocked.TryGetValue(node, out list))
				{
					list.Remove(blocker);
					if (list.Count == 0)
					{
						this.blocked.Remove(node);
						ListPool<SingleNodeBlocker>.Release(ref list);
					}
				}
			}, null));
		}

		// Token: 0x040006EE RID: 1774
		private Dictionary<GraphNode, List<SingleNodeBlocker>> blocked = new Dictionary<GraphNode, List<SingleNodeBlocker>>();

		// Token: 0x02000144 RID: 324
		public enum BlockMode
		{
			// Token: 0x040006F0 RID: 1776
			AllExceptSelector,
			// Token: 0x040006F1 RID: 1777
			OnlySelector
		}

		// Token: 0x02000145 RID: 325
		public class TraversalProvider : ITraversalProvider
		{
			// Token: 0x170001D4 RID: 468
			// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0003880F File Offset: 0x00036A0F
			// (set) Token: 0x06000A8D RID: 2701 RVA: 0x00038817 File Offset: 0x00036A17
			public BlockManager.BlockMode mode { get; private set; }

			// Token: 0x06000A8E RID: 2702 RVA: 0x00038820 File Offset: 0x00036A20
			public TraversalProvider(BlockManager blockManager, BlockManager.BlockMode mode, List<SingleNodeBlocker> selector)
			{
				if (blockManager == null)
				{
					throw new ArgumentNullException("blockManager");
				}
				if (selector == null)
				{
					throw new ArgumentNullException("selector");
				}
				this.blockManager = blockManager;
				this.mode = mode;
				this.selector = selector;
			}

			// Token: 0x06000A8F RID: 2703 RVA: 0x0003885F File Offset: 0x00036A5F
			public bool CanTraverse(ref TraversalConstraint traversalConstraint, GraphNode node)
			{
				if (this.mode == BlockManager.BlockMode.OnlySelector)
				{
					return !this.blockManager.NodeContainsAnyOf(node, this.selector);
				}
				return !this.blockManager.NodeContainsAnyExcept(node, this.selector);
			}

			// Token: 0x040006F2 RID: 1778
			private readonly BlockManager blockManager;

			// Token: 0x040006F4 RID: 1780
			private readonly List<SingleNodeBlocker> selector;
		}
	}
}
