using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020002DF RID: 735
	[HelpURL("https://arongranberg.com/astar/documentation/stable/turnbasedai.html")]
	public class TurnBasedAI : VersionedMonoBehaviour
	{
		// Token: 0x0600123F RID: 4671 RVA: 0x0007054A File Offset: 0x0006E74A
		private void Start()
		{
			this.blocker.BlockAtCurrentPosition();
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00070557 File Offset: 0x0006E757
		protected override void Awake()
		{
			base.Awake();
			this.traversalProvider = new BlockManager.TraversalProvider(this.blockManager, BlockManager.BlockMode.AllExceptSelector, new List<SingleNodeBlocker> { this.blocker });
		}

		// Token: 0x04000D72 RID: 3442
		public int movementPoints = 2;

		// Token: 0x04000D73 RID: 3443
		public BlockManager blockManager;

		// Token: 0x04000D74 RID: 3444
		public SingleNodeBlocker blocker;

		// Token: 0x04000D75 RID: 3445
		public GraphNode targetNode;

		// Token: 0x04000D76 RID: 3446
		public BlockManager.TraversalProvider traversalProvider;
	}
}
