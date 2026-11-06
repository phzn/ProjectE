using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200011B RID: 283
	[AddComponentMenu("Pathfinding/Modifiers/Alternative Path Modifier")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/alternativepath.html")]
	[Serializable]
	public class AlternativePath : MonoModifier
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x000321ED File Offset: 0x000303ED
		public override int Order
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x000321F1 File Offset: 0x000303F1
		public override void Apply(Path p)
		{
			if (this == null)
			{
				return;
			}
			this.ApplyNow(p.path);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00032209 File Offset: 0x00030409
		protected void OnDestroy()
		{
			this.destroyed = true;
			this.ClearOnDestroy();
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x00032218 File Offset: 0x00030418
		private void ClearOnDestroy()
		{
			this.InversePrevious();
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x00032220 File Offset: 0x00030420
		private void InversePrevious()
		{
			if (this.prevNodes != null)
			{
				bool flag = false;
				for (int i = 0; i < this.prevNodes.Count; i++)
				{
					if ((ulong)this.prevNodes[i].Penalty < (ulong)((long)this.prevPenalty))
					{
						flag = true;
						this.prevNodes[i].Penalty = 0U;
					}
					else
					{
						this.prevNodes[i].Penalty = (uint)((ulong)this.prevNodes[i].Penalty - (ulong)((long)this.prevPenalty));
					}
				}
				if (flag)
				{
					Debug.LogWarning("Penalty for some nodes has been reset while the AlternativePath modifier was active (possibly because of a graph update). Some penalties might be incorrect (they may be lower than expected for the affected nodes)");
				}
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x000322BC File Offset: 0x000304BC
		private void ApplyNow(List<GraphNode> nodes)
		{
			this.InversePrevious();
			this.prevNodes.Clear();
			if (this.destroyed)
			{
				return;
			}
			if (nodes != null)
			{
				for (int i = this.rnd.Next(this.randomStep); i < nodes.Count; i += this.rnd.Next(1, this.randomStep))
				{
					nodes[i].Penalty = (uint)((ulong)nodes[i].Penalty + (ulong)((long)this.penalty));
					this.prevNodes.Add(nodes[i]);
				}
			}
			this.prevPenalty = this.penalty;
		}

		// Token: 0x04000626 RID: 1574
		public int penalty = 1000;

		// Token: 0x04000627 RID: 1575
		public int randomStep = 10;

		// Token: 0x04000628 RID: 1576
		private List<GraphNode> prevNodes = new List<GraphNode>();

		// Token: 0x04000629 RID: 1577
		private int prevPenalty;

		// Token: 0x0400062A RID: 1578
		private readonly Random rnd = new Random();

		// Token: 0x0400062B RID: 1579
		private bool destroyed;
	}
}
