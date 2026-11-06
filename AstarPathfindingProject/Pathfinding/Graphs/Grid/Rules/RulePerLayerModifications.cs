using System;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Rules
{
	// Token: 0x0200021B RID: 539
	[Preserve]
	public class RulePerLayerModifications : GridGraphRule
	{
		// Token: 0x06000E19 RID: 3609 RVA: 0x00056200 File Offset: 0x00054400
		public override void Register(GridGraphRules rules)
		{
			int[] layerToTag = new int[32];
			bool[] layerToUnwalkable = new bool[32];
			for (int i = 0; i < this.layerRules.Length; i++)
			{
				RulePerLayerModifications.PerLayerRule perLayerRule = this.layerRules[i];
				if (perLayerRule.action == RulePerLayerModifications.RuleAction.SetTag)
				{
					layerToTag[perLayerRule.layer] = 1073741824 | perLayerRule.tag;
				}
				else
				{
					layerToUnwalkable[perLayerRule.layer] = true;
				}
			}
			rules.AddMainThreadPass(GridGraphRule.Pass.BeforeConnections, delegate(GridGraphRules.Context context)
			{
				if (!context.data.heightHits.IsCreated)
				{
					Debug.LogError("RulePerLayerModifications requires height testing to be enabled on the grid graph", context.graph.active);
					return;
				}
				NativeArray<RaycastHit> heightHits = context.data.heightHits;
				NativeArray<bool> walkable = context.data.nodes.walkable;
				NativeArray<int> tags = context.data.nodes.tags;
				Slice3D slice3D = new Slice3D(context.data.nodes.bounds, context.data.heightHitsBounds);
				int3 size = slice3D.slice.size;
				for (int j = 0; j < size.y; j++)
				{
					for (int k = 0; k < size.z; k++)
					{
						int num = j * size.x * size.z + k * size.x;
						for (int l = 0; l < size.x; l++)
						{
							int num2 = num + l;
							int num3 = slice3D.InnerCoordinateToOuterIndex(l, j, k);
							Collider collider = heightHits[num2].collider;
							if (collider != null)
							{
								int layer = collider.gameObject.layer;
								if (layerToUnwalkable[layer])
								{
									walkable[num3] = false;
								}
								int num4 = layerToTag[layer];
								if ((num4 & 1073741824) != 0)
								{
									tags[num3] = num4 & 255;
								}
							}
						}
					}
				}
			});
		}

		// Token: 0x04000A18 RID: 2584
		public RulePerLayerModifications.PerLayerRule[] layerRules = new RulePerLayerModifications.PerLayerRule[0];

		// Token: 0x04000A19 RID: 2585
		private const int SetTagBit = 1073741824;

		// Token: 0x0200021C RID: 540
		public struct PerLayerRule
		{
			// Token: 0x04000A1A RID: 2586
			public int layer;

			// Token: 0x04000A1B RID: 2587
			public RulePerLayerModifications.RuleAction action;

			// Token: 0x04000A1C RID: 2588
			public int tag;
		}

		// Token: 0x0200021D RID: 541
		public enum RuleAction
		{
			// Token: 0x04000A1E RID: 2590
			SetTag,
			// Token: 0x04000A1F RID: 2591
			MakeUnwalkable
		}
	}
}
