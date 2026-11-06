using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000014 RID: 20
	[UniqueComponent(tag = "ai.destination")]
	[AddComponentMenu("Pathfinding/AI/Behaviors/Patrol")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/patrol.html")]
	public class Patrol : VersionedMonoBehaviour
	{
		// Token: 0x0600008E RID: 142 RVA: 0x000048CF File Offset: 0x00002ACF
		protected override void Awake()
		{
			base.Awake();
			this.agent = base.GetComponent<IAstarAI>();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000048E4 File Offset: 0x00002AE4
		private void Update()
		{
			if (this.targets.Length == 0)
			{
				return;
			}
			if (this.agent.reachedEndOfPath && !this.agent.pathPending && float.IsPositiveInfinity(this.switchTime))
			{
				this.switchTime = Time.time + this.delay;
			}
			if (Time.time >= this.switchTime)
			{
				this.index++;
				this.switchTime = float.PositiveInfinity;
				this.index %= this.targets.Length;
				this.agent.destination = this.targets[this.index].position;
				this.agent.SearchPath();
				return;
			}
			if (this.updateDestinationEveryFrame)
			{
				this.index %= this.targets.Length;
				this.agent.destination = this.targets[this.index].position;
			}
		}

		// Token: 0x0400007A RID: 122
		public Transform[] targets;

		// Token: 0x0400007B RID: 123
		public float delay;

		// Token: 0x0400007C RID: 124
		public bool updateDestinationEveryFrame;

		// Token: 0x0400007D RID: 125
		private int index = -1;

		// Token: 0x0400007E RID: 126
		private IAstarAI agent;

		// Token: 0x0400007F RID: 127
		private float switchTime = float.NegativeInfinity;
	}
}
