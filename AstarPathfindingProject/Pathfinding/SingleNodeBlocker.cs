using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000148 RID: 328
	[HelpURL("https://arongranberg.com/astar/documentation/stable/singlenodeblocker.html")]
	public class SingleNodeBlocker : VersionedMonoBehaviour
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x00038946 File Offset: 0x00036B46
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x0003894E File Offset: 0x00036B4E
		public GraphNode lastBlocked { get; private set; }

		// Token: 0x06000A96 RID: 2710 RVA: 0x00038957 File Offset: 0x00036B57
		public void BlockAtCurrentPosition()
		{
			this.BlockAt(base.transform.position);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0003896C File Offset: 0x00036B6C
		public void BlockAt(Vector3 position)
		{
			this.Unblock();
			GraphNode node = AstarPath.active.GetNearest(position).node;
			if (node != null)
			{
				this.Block(node);
			}
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0003899A File Offset: 0x00036B9A
		public void Block(GraphNode node)
		{
			this.Unblock();
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this.manager.InternalBlock(node, this);
			this.lastBlocked = node;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x000389C4 File Offset: 0x00036BC4
		public void Unblock()
		{
			if (this.lastBlocked == null || this.lastBlocked.Destroyed)
			{
				this.lastBlocked = null;
				return;
			}
			this.manager.InternalUnblock(this.lastBlocked, this);
			this.lastBlocked = null;
		}

		// Token: 0x040006FC RID: 1788
		public BlockManager manager;
	}
}
