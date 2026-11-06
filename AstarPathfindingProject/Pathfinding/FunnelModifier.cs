using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200011C RID: 284
	[AddComponentMenu("Pathfinding/Modifiers/Funnel Modifier")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/funnelmodifier.html")]
	[Serializable]
	public class FunnelModifier : MonoModifier
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x000321ED File Offset: 0x000303ED
		public override int Order
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0003238C File Offset: 0x0003058C
		public override void Apply(Path p)
		{
			if (p.path == null || p.path.Count == 0 || p.vectorPath == null || p.vectorPath.Count == 0)
			{
				return;
			}
			List<Vector3> list = ListPool<Vector3>.Claim();
			List<Funnel.PathPart> list2 = Funnel.SplitIntoParts(p);
			if (list2.Count == 0)
			{
				return;
			}
			if (this.quality == FunnelModifier.FunnelQuality.High)
			{
				Funnel.Simplify(list2, ref p.path, ref p.traversalCosts, ref p.traversalConstraint);
			}
			for (int i = 0; i < list2.Count; i++)
			{
				Funnel.PathPart pathPart = list2[i];
				if (pathPart.type == Funnel.PartType.NodeSequence)
				{
					GridGraph gridGraph = p.path[pathPart.startIndex].Graph as GridGraph;
					if (gridGraph != null && gridGraph.neighbours != NumNeighbours.Six)
					{
						TraversalCosts traversalCosts = (this.accountForGridPenalties ? p.traversalCosts : default(TraversalCosts));
						List<Vector3> list3 = GridStringPulling.Calculate(p.path, pathPart.startIndex, pathPart.endIndex, pathPart.startPoint, pathPart.endPoint, ref traversalCosts, ref p.traversalConstraint, int.MaxValue);
						list.AddRange(list3);
						ListPool<Vector3>.Release(ref list3);
					}
					else
					{
						Funnel.FunnelPortals funnelPortals = Funnel.ConstructFunnelPortals(p.path, pathPart);
						List<Vector3> list4 = Funnel.Calculate(funnelPortals, this.splitAtEveryPortal);
						list.AddRange(list4);
						ListPool<Vector3>.Release(ref funnelPortals.left);
						ListPool<Vector3>.Release(ref funnelPortals.right);
						ListPool<Vector3>.Release(ref list4);
					}
				}
				else
				{
					if (i == 0 || list2[i - 1].type == Funnel.PartType.OffMeshLink)
					{
						list.Add(pathPart.startPoint);
					}
					if (i == list2.Count - 1 || list2[i + 1].type == Funnel.PartType.OffMeshLink)
					{
						list.Add(pathPart.endPoint);
					}
				}
			}
			ListPool<Funnel.PathPart>.Release(ref list2);
			ListPool<Vector3>.Release(ref p.vectorPath);
			p.vectorPath = list;
		}

		// Token: 0x0400062C RID: 1580
		public FunnelModifier.FunnelQuality quality;

		// Token: 0x0400062D RID: 1581
		public bool splitAtEveryPortal;

		// Token: 0x0400062E RID: 1582
		public bool accountForGridPenalties;

		// Token: 0x0200011D RID: 285
		public enum FunnelQuality
		{
			// Token: 0x04000630 RID: 1584
			Medium,
			// Token: 0x04000631 RID: 1585
			High
		}
	}
}
