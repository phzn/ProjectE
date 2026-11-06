using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000123 RID: 291
	[AddComponentMenu("Pathfinding/Modifiers/Raycast Modifier")]
	[RequireComponent(typeof(Seeker))]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/raycastmodifier.html")]
	[Serializable]
	public class RaycastModifier : MonoModifier
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x00030F05 File Offset: 0x0002F105
		public override int Order
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00032D30 File Offset: 0x00030F30
		public override void Apply(Path p)
		{
			if (!this.useRaycasting && !this.useGraphRaycasting)
			{
				return;
			}
			List<Vector3> list = p.vectorPath;
			if (this.ValidateLine(null, null, p.vectorPath[0], p.vectorPath[p.vectorPath.Count - 1], ref p.traversalConstraint))
			{
				Vector3 vector = p.vectorPath[0];
				Vector3 vector2 = p.vectorPath[p.vectorPath.Count - 1];
				list.ClearFast<Vector3>();
				list.Add(vector);
				list.Add(vector2);
			}
			else
			{
				int num = RaycastModifier.iterationsByQuality[(int)this.quality];
				for (int i = 0; i < num; i++)
				{
					if (i != 0)
					{
						Polygon.Subdivide(list, RaycastModifier.buffer, 3);
						Memory.Swap<List<Vector3>>(ref RaycastModifier.buffer, ref list);
						RaycastModifier.buffer.ClearFast<Vector3>();
						list.Reverse();
					}
					list = ((this.quality >= RaycastModifier.Quality.High) ? this.ApplyDP(p, list, ref p.traversalConstraint) : this.ApplyGreedy(p, list, ref p.traversalConstraint));
				}
				if (num % 2 == 0)
				{
					list.Reverse();
				}
			}
			p.vectorPath = list;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00032E48 File Offset: 0x00031048
		private List<Vector3> ApplyGreedy(Path p, List<Vector3> points, ref TraversalConstraint traversalConstraint)
		{
			bool flag = points.Count == p.path.Count;
			int i = 0;
			while (i < points.Count)
			{
				Vector3 vector = points[i];
				GraphNode graphNode = ((flag && points[i] == (Vector3)p.path[i].position) ? p.path[i] : null);
				RaycastModifier.buffer.Add(vector);
				int num = 1;
				int num2 = 2;
				for (;;)
				{
					int num3 = i + num2;
					if (num3 >= points.Count)
					{
						goto Block_2;
					}
					Vector3 vector2 = points[num3];
					GraphNode graphNode2 = ((flag && vector2 == (Vector3)p.path[num3].position) ? p.path[num3] : null);
					if (!this.ValidateLine(graphNode, graphNode2, vector, vector2, ref traversalConstraint))
					{
						break;
					}
					num = num2;
					num2 *= 2;
				}
				IL_014B:
				while (num + 1 < num2)
				{
					int num4 = (num + num2) / 2;
					int num5 = i + num4;
					Vector3 vector3 = points[num5];
					GraphNode graphNode3 = ((flag && vector3 == (Vector3)p.path[num5].position) ? p.path[num5] : null);
					if (this.ValidateLine(graphNode, graphNode3, vector, vector3, ref traversalConstraint))
					{
						num = num4;
					}
					else
					{
						num2 = num4;
					}
				}
				i += num;
				continue;
				Block_2:
				num2 = points.Count - i;
				goto IL_014B;
			}
			Memory.Swap<List<Vector3>>(ref RaycastModifier.buffer, ref points);
			RaycastModifier.buffer.ClearFast<Vector3>();
			return points;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00032FD0 File Offset: 0x000311D0
		private List<Vector3> ApplyDP(Path p, List<Vector3> points, ref TraversalConstraint traversalConstraint)
		{
			if (RaycastModifier.DPCosts.Length < points.Count)
			{
				RaycastModifier.DPCosts = new float[points.Count];
				RaycastModifier.DPParents = new int[points.Count];
			}
			for (int i = 0; i < RaycastModifier.DPParents.Length; i++)
			{
				RaycastModifier.DPCosts[i] = (float)(RaycastModifier.DPParents[i] = -1);
			}
			bool flag = points.Count == p.path.Count;
			for (int j = 0; j < points.Count; j++)
			{
				float num = RaycastModifier.DPCosts[j];
				Vector3 vector = points[j];
				bool flag2 = flag && vector == (Vector3)p.path[j].position;
				for (int k = j + 1; k < points.Count; k++)
				{
					float num2 = num + (points[k] - vector).magnitude + 0.0001f;
					if (RaycastModifier.DPParents[k] == -1 || num2 < RaycastModifier.DPCosts[k])
					{
						bool flag3 = flag && points[k] == (Vector3)p.path[k].position;
						if (k != j + 1 && !this.ValidateLine(flag2 ? p.path[j] : null, flag3 ? p.path[k] : null, vector, points[k], ref traversalConstraint))
						{
							break;
						}
						RaycastModifier.DPCosts[k] = num2;
						RaycastModifier.DPParents[k] = j;
					}
				}
			}
			for (int num3 = points.Count - 1; num3 != -1; num3 = RaycastModifier.DPParents[num3])
			{
				RaycastModifier.buffer.Add(points[num3]);
			}
			RaycastModifier.buffer.Reverse();
			Memory.Swap<List<Vector3>>(ref RaycastModifier.buffer, ref points);
			RaycastModifier.buffer.ClearFast<Vector3>();
			return points;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x000331C4 File Offset: 0x000313C4
		protected bool ValidateLine(GraphNode n1, GraphNode n2, Vector3 v1, Vector3 v2, ref TraversalConstraint traversalConstraint)
		{
			if (this.useRaycasting)
			{
				if (this.use2DPhysics)
				{
					if (this.thickRaycast && this.thickRaycastRadius > 0f && Physics2D.CircleCast(v1 + this.raycastOffset, this.thickRaycastRadius, v2 - v1, (v2 - v1).magnitude, this.mask))
					{
						return false;
					}
					if (Physics2D.Linecast(v1 + this.raycastOffset, v2 + this.raycastOffset, this.mask))
					{
						return false;
					}
				}
				else
				{
					if (Physics.Linecast(v1 + this.raycastOffset, v2 + this.raycastOffset, this.mask))
					{
						return false;
					}
					if (this.thickRaycast && this.thickRaycastRadius > 0f)
					{
						if (Physics.CheckSphere(v1 + this.raycastOffset + (v2 - v1).normalized * this.thickRaycastRadius, this.thickRaycastRadius, this.mask))
						{
							return false;
						}
						if (Physics.SphereCast(new Ray(v1 + this.raycastOffset, v2 - v1), this.thickRaycastRadius, (v2 - v1).magnitude, this.mask))
						{
							return false;
						}
					}
				}
			}
			if (this.useGraphRaycasting)
			{
				bool flag = n1 != null && n2 != null;
				NearestNodeConstraint none = NearestNodeConstraint.None;
				none.graphMask = traversalConstraint.graphMask;
				if (n1 == null)
				{
					n1 = AstarPath.active.GetNearest(v1, ref none).node;
				}
				if (n2 == null)
				{
					n2 = AstarPath.active.GetNearest(v2, ref none).node;
				}
				if (n1 != null && n2 != null)
				{
					NavGraph graph = n1.Graph;
					NavGraph graph2 = n2.Graph;
					if (graph != graph2)
					{
						return false;
					}
					IRaycastableGraph raycastableGraph = graph as IRaycastableGraph;
					GridGraph gridGraph = graph as GridGraph;
					if (flag && gridGraph != null)
					{
						return !gridGraph.Linecast(n1 as GridNodeBase, n2 as GridNodeBase, ref traversalConstraint);
					}
					if (raycastableGraph != null)
					{
						GraphHitInfo graphHitInfo;
						return !raycastableGraph.Linecast(v1, v2, out graphHitInfo, ref traversalConstraint, null);
					}
				}
			}
			return true;
		}

		// Token: 0x04000641 RID: 1601
		public bool useRaycasting;

		// Token: 0x04000642 RID: 1602
		public LayerMask mask = -1;

		// Token: 0x04000643 RID: 1603
		[Tooltip("Checks around the line between two points, not just the exact line.\nMake sure the ground is either too far below or is not inside the mask since otherwise the raycast might always hit the ground.")]
		public bool thickRaycast;

		// Token: 0x04000644 RID: 1604
		[Tooltip("Distance from the ray which will be checked for colliders")]
		public float thickRaycastRadius;

		// Token: 0x04000645 RID: 1605
		[Tooltip("Check for intersections with 2D colliders instead of 3D colliders.")]
		public bool use2DPhysics;

		// Token: 0x04000646 RID: 1606
		[Tooltip("Offset from the original positions to perform the raycast.\nCan be useful to avoid the raycast intersecting the ground or similar things you do not want to it intersect")]
		public Vector3 raycastOffset = Vector3.zero;

		// Token: 0x04000647 RID: 1607
		[Tooltip("Use raycasting on the graphs. Only currently works with GridGraph and NavmeshGraph and RecastGraph. This is a pro version feature.")]
		public bool useGraphRaycasting = true;

		// Token: 0x04000648 RID: 1608
		[Tooltip("When using the high quality mode the script will try harder to find a shorter path. This is significantly slower than the greedy low quality approach.")]
		public RaycastModifier.Quality quality = RaycastModifier.Quality.Medium;

		// Token: 0x04000649 RID: 1609
		private static readonly int[] iterationsByQuality = new int[] { 1, 2, 1, 3 };

		// Token: 0x0400064A RID: 1610
		private static List<Vector3> buffer = new List<Vector3>();

		// Token: 0x0400064B RID: 1611
		private static float[] DPCosts = new float[16];

		// Token: 0x0400064C RID: 1612
		private static int[] DPParents = new int[16];

		// Token: 0x02000124 RID: 292
		public enum Quality
		{
			// Token: 0x0400064E RID: 1614
			Low,
			// Token: 0x0400064F RID: 1615
			Medium,
			// Token: 0x04000650 RID: 1616
			High,
			// Token: 0x04000651 RID: 1617
			Highest
		}
	}
}
