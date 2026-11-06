using System;
using System.Collections.Generic;
using System.Text;
using Pathfinding.Pooling;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000136 RID: 310
	public class ABPath : Path
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x00036705 File Offset: 0x00034905
		public GraphNode startNode
		{
			get
			{
				if (this.path.Count <= 0)
				{
					return null;
				}
				return this.path[0];
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x00036723 File Offset: 0x00034923
		public GraphNode endNode
		{
			get
			{
				if (this.path.Count <= 0)
				{
					return null;
				}
				return this.path[this.path.Count - 1];
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x00019DC2 File Offset: 0x00017FC2
		protected virtual bool hasEndPoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x00019DC2 File Offset: 0x00017FC2
		public virtual bool endPointKnownBeforeCalculation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00036763 File Offset: 0x00034963
		public static ABPath Construct(Vector3 start, Vector3 end, OnPathDelegate callback = null)
		{
			ABPath path = PathPool.GetPath<ABPath>();
			path.Setup(start, end, callback);
			return path;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00036773 File Offset: 0x00034973
		protected void Setup(Vector3 start, Vector3 end, OnPathDelegate callbackDelegate)
		{
			this.callback = callbackDelegate;
			this.UpdateStartEnd(start, end);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00036784 File Offset: 0x00034984
		public static ABPath FakePath(List<Vector3> vectorPath, List<GraphNode> nodePath = null)
		{
			ABPath path = PathPool.GetPath<ABPath>();
			for (int i = 0; i < vectorPath.Count; i++)
			{
				path.vectorPath.Add(vectorPath[i]);
			}
			path.completeState = PathCompleteState.Complete;
			((IPathInternals)path).AdvanceState(PathState.Returned);
			if (vectorPath.Count > 0)
			{
				path.UpdateStartEnd(vectorPath[0], vectorPath[vectorPath.Count - 1]);
			}
			if (nodePath != null)
			{
				for (int j = 0; j < nodePath.Count; j++)
				{
					path.path.Add(nodePath[j]);
				}
			}
			return path;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00036813 File Offset: 0x00034A13
		protected void UpdateStartEnd(Vector3 start, Vector3 end)
		{
			this.originalStartPoint = start;
			this.originalEndPoint = end;
			this.startPoint = start;
			this.endPoint = end;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00036834 File Offset: 0x00034A34
		protected override void Reset()
		{
			base.Reset();
			this.originalStartPoint = Vector3.zero;
			this.originalEndPoint = Vector3.zero;
			this.startPoint = Vector3.zero;
			this.endPoint = Vector3.zero;
			this.calculatePartial = false;
			this.partialBestTargetPathNodeIndex = 0U;
			this.partialBestTargetHScore = uint.MaxValue;
			this.partialBestTargetGScore = uint.MaxValue;
			this.cost = 0U;
			this.endingCondition = null;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000368A0 File Offset: 0x00034AA0
		protected virtual bool EndPointGridGraphSpecialCase(ref Path.SearchContext ctx, GraphNode closestWalkableEndNode, Vector3 originalEndPoint, int targetIndex)
		{
			GridNode gridNode = closestWalkableEndNode as GridNode;
			if (gridNode != null)
			{
				GridGraph gridGraph = GridNode.GetGridGraph(gridNode.GraphIndex);
				NearestNodeConstraint none = NearestNodeConstraint.None;
				none.maxDistanceSqr = 0.0001f;
				none.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
				GridNode gridNode2 = gridGraph.GetNearest(originalEndPoint, ref none).node as GridNode;
				if (gridNode != gridNode2 && gridNode2 != null)
				{
					int num = gridNode.NodeInGridIndex % gridGraph.width;
					int num2 = gridNode.NodeInGridIndex / gridGraph.width;
					int num3 = gridNode2.NodeInGridIndex % gridGraph.width;
					int num4 = gridNode2.NodeInGridIndex / gridGraph.width;
					bool flag = false;
					switch (gridGraph.neighbours)
					{
					case NumNeighbours.Four:
						if ((num == num3 && Math.Abs(num2 - num4) == 1) || (num2 == num4 && Math.Abs(num - num3) == 1))
						{
							flag = true;
						}
						break;
					case NumNeighbours.Eight:
						if (Math.Abs(num - num3) <= 1 && Math.Abs(num2 - num4) <= 1)
						{
							flag = true;
						}
						break;
					case NumNeighbours.Six:
					{
						for (int i = 0; i < 6; i++)
						{
							int num5 = num3 + GridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[i]];
							int num6 = num4 + GridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[i]];
							if (num == num5 && num2 == num6)
							{
								flag = true;
								break;
							}
						}
						break;
					}
					default:
						throw new Exception("Unhandled NumNeighbours");
					}
					if (flag)
					{
						this.AddEndpointsForSurroundingGridNodes(ref ctx, gridNode2, originalEndPoint, targetIndex);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00036A18 File Offset: 0x00034C18
		private void AddEndpointsForSurroundingGridNodes(ref Path.SearchContext ctx, GridNode gridNode, Vector3 desiredPoint, int targetIndex)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(gridNode.GraphIndex);
			int num = ((gridGraph.neighbours == NumNeighbours.Four) ? 4 : ((gridGraph.neighbours == NumNeighbours.Eight) ? 8 : 6));
			int num2 = gridNode.NodeInGridIndex % gridGraph.width;
			int num3 = gridNode.NodeInGridIndex / gridGraph.width;
			for (int i = 0; i < num; i++)
			{
				int num4;
				int num5;
				if (gridGraph.neighbours == NumNeighbours.Six)
				{
					num4 = num2 + GridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[i]];
					num5 = num3 + GridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[i]];
				}
				else
				{
					num4 = num2 + GridGraph.neighbourXOffsets[i];
					num5 = num3 + GridGraph.neighbourZOffsets[i];
				}
				GridNodeBase node = gridGraph.GetNode(num4, num5);
				if (node != null && this.traversalConstraint.CanTraverse(node))
				{
					ctx.pathHandler.AddTemporaryNode(new TemporaryNode
					{
						type = TemporaryNodeType.End,
						position = (Int3)node.ClosestPointOnNode(desiredPoint),
						associatedNode = node.NodeIndex,
						targetIndex = targetIndex
					});
				}
			}
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x00036B2C File Offset: 0x00034D2C
		protected override void Prepare(ref Path.SearchContext ctx)
		{
			NearestNodeConstraint nearestNodeConstraint = base.nearestNodeConstraint;
			this.FindStartAndEndNodes(ref ctx, ref nearestNodeConstraint);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00036B4C File Offset: 0x00034D4C
		protected void FindStartAndEndNodes(ref Path.SearchContext ctx, ref NearestNodeConstraint constraint)
		{
			NNInfo nearest = AstarPath.active.GetNearest(this.startPoint, constraint);
			this.startPoint = nearest.position;
			if (nearest.node == null)
			{
				base.FailWithError("Couldn't find a node close to the start point");
				return;
			}
			ctx.pathHandler.AddTemporaryNode(new TemporaryNode
			{
				associatedNode = nearest.node.NodeIndex,
				position = (Int3)nearest.position,
				type = TemporaryNodeType.Start
			});
			uint num = 0U;
			if (this.hasEndPoint)
			{
				constraint.area = (int)nearest.node.Area;
				NNInfo nearest2 = AstarPath.active.GetNearest(this.originalEndPoint, constraint);
				this.endPoint = nearest2.position;
				if (nearest2.node == null)
				{
					base.FailWithError("Couldn't find a node close to the end point");
					return;
				}
				num = nearest2.node.NodeIndex;
				if (!this.EndPointGridGraphSpecialCase(ref ctx, nearest2.node, this.originalEndPoint, 0))
				{
					ctx.pathHandler.AddTemporaryNode(new TemporaryNode
					{
						associatedNode = nearest2.node.NodeIndex,
						position = (Int3)nearest2.position,
						type = TemporaryNodeType.End
					});
				}
			}
			int3 @int;
			int3 int2;
			ctx.TemporaryEndNodesBoundingBox(out @int, out int2);
			ctx.heuristicObjective = new HeuristicObjective(@int, int2, this.heuristic, this.heuristicScale, num, AstarPath.active.euclideanEmbedding);
			ctx.traversalConstraint = this.traversalConstraint;
			ctx.traversalCosts = this.traversalCosts;
			ctx.MarkNodesAdjacentToTemporaryEndNodes();
			ctx.AddStartNodesToHeap();
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00036CE4 File Offset: 0x00034EE4
		private void CompletePartial(ref Path.SearchContext ctx)
		{
			base.CompleteState = PathCompleteState.Partial;
			this.endPoint = ctx.pathHandler.GetNode(this.partialBestTargetPathNodeIndex).ClosestPointOnNode(this.originalEndPoint);
			this.cost = this.partialBestTargetGScore;
			this.Trace(ref ctx, this.partialBestTargetPathNodeIndex);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00036D34 File Offset: 0x00034F34
		public virtual void RefreshPathEndpoints(Vector3 start, Vector3 end)
		{
			lock (this)
			{
				if (base.PipelineState == PathState.PathQueue || base.PipelineState == PathState.Created)
				{
					this.UpdateStartEnd(start, end);
				}
			}
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00036D84 File Offset: 0x00034F84
		protected override void OnHeapExhausted(ref Path.SearchContext ctx)
		{
			if (this.calculatePartial && this.partialBestTargetPathNodeIndex != 0U)
			{
				this.CompletePartial(ref ctx);
				return;
			}
			base.FailWithError("Searched all reachable nodes, but could not find target. This can happen if you have nodes with a different tag blocking the way to the goal. You can enable path.calculatePartial to handle that case as a workaround (though this comes with a performance cost).");
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00036DAC File Offset: 0x00034FAC
		protected unsafe override void OnFoundEndNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
		{
			PathHandler pathHandler = ctx.pathHandler;
			if (pathHandler.IsTemporaryNode(pathNode))
			{
				TemporaryNode temporaryNode = *pathHandler.GetTemporaryNode(pathNode);
				GraphNode node = pathHandler.GetNode(temporaryNode.associatedNode);
				if (this.endingCondition != null && !this.endingCondition.TargetFound(node, this.partialBestTargetHScore, gScore))
				{
					return;
				}
				this.endPoint = (Vector3)temporaryNode.position;
				this.endPoint = node.ClosestPointOnNode(this.endPoint);
			}
			else
			{
				GraphNode node2 = pathHandler.GetNode(pathNode);
				this.endPoint = (Vector3)node2.position;
			}
			this.cost = gScore;
			base.CompleteState = PathCompleteState.Complete;
			this.Trace(ref ctx, pathNode);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00036E58 File Offset: 0x00035058
		public override void OnVisitNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
		{
			if (base.CompleteState != PathCompleteState.NotCalculated)
			{
				return;
			}
			if (this.endingCondition != null)
			{
				GraphNode node = ctx.pathHandler.GetNode(pathNode);
				if (this.endingCondition.TargetFound(node, hScore, gScore))
				{
					this.OnFoundEndNode(ref ctx, pathNode, hScore, gScore);
					if (base.CompleteState == PathCompleteState.Complete)
					{
						return;
					}
				}
			}
			if (hScore < this.partialBestTargetHScore)
			{
				this.partialBestTargetPathNodeIndex = pathNode;
				this.partialBestTargetHScore = hScore;
				this.partialBestTargetGScore = gScore;
			}
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x00036EC8 File Offset: 0x000350C8
		protected override void DebugString(StringBuilder builder, PathLog logMode)
		{
			if (logMode == PathLog.None || (!base.error && logMode == PathLog.OnlyErrors))
			{
				return;
			}
			base.DebugStringPrefix(logMode, builder);
			if (!base.error)
			{
				builder.Append(" Path Cost: ");
				builder.Append(this.cost);
			}
			if (!base.error && logMode == PathLog.Heavy)
			{
				Vector3 vector;
				if (this.hasEndPoint && this.endNode != null)
				{
					builder.Append("\n\tPoint: ");
					vector = this.endPoint;
					builder.Append(vector.ToString());
					builder.Append("\n\tGraph: ");
					builder.Append(this.endNode.GraphIndex);
				}
				builder.Append("\nStart Node");
				builder.Append("\n\tPoint: ");
				vector = this.startPoint;
				builder.Append(vector.ToString());
				builder.Append("\n\tGraph: ");
				if (this.startNode != null)
				{
					builder.Append(this.startNode.GraphIndex);
				}
				else
				{
					builder.Append("< null startNode >");
				}
			}
			base.DebugStringSuffix(logMode, builder);
		}

		// Token: 0x040006BB RID: 1723
		public Vector3 originalStartPoint;

		// Token: 0x040006BC RID: 1724
		public Vector3 originalEndPoint;

		// Token: 0x040006BD RID: 1725
		public Vector3 startPoint;

		// Token: 0x040006BE RID: 1726
		public Vector3 endPoint;

		// Token: 0x040006BF RID: 1727
		public uint cost;

		// Token: 0x040006C0 RID: 1728
		public bool calculatePartial;

		// Token: 0x040006C1 RID: 1729
		protected uint partialBestTargetPathNodeIndex;

		// Token: 0x040006C2 RID: 1730
		protected uint partialBestTargetHScore = uint.MaxValue;

		// Token: 0x040006C3 RID: 1731
		protected uint partialBestTargetGScore = uint.MaxValue;

		// Token: 0x040006C4 RID: 1732
		public PathEndingCondition endingCondition;
	}
}
