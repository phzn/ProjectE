using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000127 RID: 295
	[Serializable]
	public class StartEndModifier : PathModifier
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0001AA07 File Offset: 0x00018C07
		public override int Order
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00033DC0 File Offset: 0x00031FC0
		public override void Apply(Path _p)
		{
			ABPath abpath = _p as ABPath;
			if (abpath == null || abpath.vectorPath.Count == 0)
			{
				return;
			}
			bool flag = false;
			if (abpath.vectorPath.Count == 1 && !this.addPoints)
			{
				abpath.vectorPath.Add(abpath.vectorPath[0]);
				flag = true;
			}
			bool flag2;
			int num;
			Vector3 vector = this.Snap(abpath, this.exactStartPoint, true, out flag2, out num);
			bool flag3;
			int num2;
			Vector3 vector2 = this.Snap(abpath, this.exactEndPoint, false, out flag3, out num2);
			if (flag)
			{
				if (num == num2)
				{
					flag2 = false;
					flag3 = false;
				}
				else
				{
					flag2 = false;
				}
			}
			if ((flag2 || this.addPoints) && this.exactStartPoint != StartEndModifier.Exactness.SnapToNode)
			{
				abpath.vectorPath.Insert(0, vector);
			}
			else
			{
				abpath.vectorPath[0] = vector;
			}
			if ((flag3 || this.addPoints) && this.exactEndPoint != StartEndModifier.Exactness.SnapToNode)
			{
				abpath.vectorPath.Add(vector2);
				return;
			}
			abpath.vectorPath[abpath.vectorPath.Count - 1] = vector2;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00033EBC File Offset: 0x000320BC
		private Vector3 Snap(ABPath path, StartEndModifier.Exactness mode, bool start, out bool forceAddPoint, out int closestConnectionIndex)
		{
			int num = (start ? 0 : (path.path.Count - 1));
			GraphNode graphNode = path.path[num];
			Vector3 vector = (Vector3)graphNode.position;
			closestConnectionIndex = 0;
			forceAddPoint = false;
			switch (mode)
			{
			case StartEndModifier.Exactness.SnapToNode:
				return vector;
			case StartEndModifier.Exactness.Original:
			case StartEndModifier.Exactness.Interpolate:
			case StartEndModifier.Exactness.NodeConnection:
			{
				Vector3 vector2;
				if (start)
				{
					vector2 = ((this.adjustStartPoint != null) ? this.adjustStartPoint() : path.originalStartPoint);
				}
				else
				{
					vector2 = path.originalEndPoint;
				}
				switch (mode)
				{
				case StartEndModifier.Exactness.Original:
					return this.GetClampedPoint(vector, vector2, graphNode);
				case StartEndModifier.Exactness.Interpolate:
				{
					GraphNode graphNode2 = path.path[Mathf.Clamp(num + (start ? 1 : (-1)), 0, path.path.Count - 1)];
					return VectorMath.ClosestPointOnSegment(vector, (Vector3)graphNode2.position, vector2);
				}
				case StartEndModifier.Exactness.NodeConnection:
				{
					this.connectionBuffer = this.connectionBuffer ?? new List<GraphNode>();
					Action<GraphNode> action;
					if ((action = this.connectionBufferAddDelegate) == null)
					{
						action = new Action<GraphNode>(this.connectionBuffer.Add);
					}
					this.connectionBufferAddDelegate = action;
					GraphNode graphNode2 = path.path[Mathf.Clamp(num + (start ? 1 : (-1)), 0, path.path.Count - 1)];
					graphNode.GetConnections(this.connectionBufferAddDelegate, 32);
					Vector3 vector3 = vector;
					float num2 = float.PositiveInfinity;
					for (int i = this.connectionBuffer.Count - 1; i >= 0; i--)
					{
						GraphNode graphNode3 = this.connectionBuffer[i];
						if (path.traversalConstraint.CanTraverse(graphNode, graphNode3))
						{
							Vector3 vector4 = VectorMath.ClosestPointOnSegment(vector, (Vector3)graphNode3.position, vector2);
							float sqrMagnitude = (vector4 - vector2).sqrMagnitude;
							if (sqrMagnitude < num2)
							{
								vector3 = vector4;
								num2 = sqrMagnitude;
								closestConnectionIndex = i;
								forceAddPoint = graphNode3 != graphNode2;
							}
						}
					}
					this.connectionBuffer.Clear();
					return vector3;
				}
				}
				throw new ArgumentException("Cannot reach this point, but the compiler is not smart enough to realize that.");
			}
			case StartEndModifier.Exactness.ClosestOnNode:
				if (!start)
				{
					return path.endPoint;
				}
				return path.startPoint;
			default:
				throw new ArgumentException("Invalid mode");
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x000340DC File Offset: 0x000322DC
		protected Vector3 GetClampedPoint(Vector3 from, Vector3 to, GraphNode hint)
		{
			Vector3 vector = to;
			RaycastHit raycastHit;
			if (this.useRaycasting && Physics.Linecast(from, to, out raycastHit, this.mask))
			{
				vector = raycastHit.point;
			}
			if (this.useGraphRaycasting && hint != null)
			{
				IRaycastableGraph raycastableGraph = AstarData.GetGraph(hint) as IRaycastableGraph;
				GraphHitInfo graphHitInfo;
				if (raycastableGraph != null && raycastableGraph.Linecast(from, vector, out graphHitInfo))
				{
					vector = graphHitInfo.point;
				}
			}
			return vector;
		}

		// Token: 0x04000660 RID: 1632
		public bool addPoints;

		// Token: 0x04000661 RID: 1633
		public StartEndModifier.Exactness exactStartPoint = StartEndModifier.Exactness.ClosestOnNode;

		// Token: 0x04000662 RID: 1634
		public StartEndModifier.Exactness exactEndPoint = StartEndModifier.Exactness.ClosestOnNode;

		// Token: 0x04000663 RID: 1635
		public Func<Vector3> adjustStartPoint;

		// Token: 0x04000664 RID: 1636
		public bool useRaycasting;

		// Token: 0x04000665 RID: 1637
		public LayerMask mask = -1;

		// Token: 0x04000666 RID: 1638
		public bool useGraphRaycasting;

		// Token: 0x04000667 RID: 1639
		private List<GraphNode> connectionBuffer;

		// Token: 0x04000668 RID: 1640
		private Action<GraphNode> connectionBufferAddDelegate;

		// Token: 0x02000128 RID: 296
		public enum Exactness
		{
			// Token: 0x0400066A RID: 1642
			SnapToNode,
			// Token: 0x0400066B RID: 1643
			Original,
			// Token: 0x0400066C RID: 1644
			Interpolate,
			// Token: 0x0400066D RID: 1645
			ClosestOnNode,
			// Token: 0x0400066E RID: 1646
			NodeConnection
		}
	}
}
