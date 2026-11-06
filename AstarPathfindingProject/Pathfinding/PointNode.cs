using System;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000EE RID: 238
	public class PointNode : GraphNode
	{
		// Token: 0x0600086B RID: 2155 RVA: 0x0002B402 File Offset: 0x00029602
		[Obsolete("Set node.position instead")]
		public void SetPosition(Int3 value)
		{
			this.position = value;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0001A4AB File Offset: 0x000186AB
		public PointNode()
		{
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0002BB78 File Offset: 0x00029D78
		public PointNode(AstarPath astar)
		{
			astar.InitializeNode(this);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0001A08F File Offset: 0x0001828F
		public override Vector3 ClosestPointOnNode(Vector3 p)
		{
			return (Vector3)this.position;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0001AA07 File Offset: 0x00018C07
		public override bool ContainsPoint(Vector3 point)
		{
			return false;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0001AA07 File Offset: 0x00018C07
		public override bool ContainsPointInGraphSpace(Int3 point)
		{
			return false;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0002BB88 File Offset: 0x00029D88
		public override void GetConnections<T>(GraphNode.NodeActionWithData<T> action, ref T data, int connectionFilter)
		{
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				if (((int)this.connections[i].shapeEdgeInfo & connectionFilter) != 0)
				{
					action(this.connections[i].node, ref data);
				}
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0002BBE0 File Offset: 0x00029DE0
		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse && this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					this.connections[i].node.RemovePartialConnection(this);
				}
			}
			this.connections = null;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0002BC3C File Offset: 0x00029E3C
		public override bool ContainsOutgoingConnection(GraphNode node)
		{
			if (this.connections == null)
			{
				return false;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				if (this.connections[i].node == node && this.connections[i].isOutgoing)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0002BC90 File Offset: 0x00029E90
		public override void AddPartialConnection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming)
		{
			if (node == null)
			{
				throw new ArgumentNullException();
			}
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node == node)
					{
						this.connections[i].cost = cost;
						this.connections[i].shapeEdgeInfo = Connection.PackShapeEdgeInfo(isOutgoing, isIncoming);
						return;
					}
				}
			}
			int num = ((this.connections != null) ? this.connections.Length : 0);
			Connection[] array = new Connection[num + 1];
			for (int j = 0; j < num; j++)
			{
				array[j] = this.connections[j];
			}
			array[num] = new Connection(node, cost, isOutgoing, isIncoming);
			this.connections = array;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
			PointGraph pointGraph = base.Graph as PointGraph;
			if (pointGraph != null)
			{
				pointGraph.RegisterConnectionLength((node.position - this.position).sqrMagnitudeLong);
			}
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0002BD9C File Offset: 0x00029F9C
		public override void RemovePartialConnection(GraphNode node)
		{
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				if (this.connections[i].node == node)
				{
					int num = this.connections.Length;
					Connection[] array = new Connection[num - 1];
					for (int j = 0; j < i; j++)
					{
						array[j] = this.connections[j];
					}
					for (int k = i + 1; k < num; k++)
					{
						array[k - 1] = this.connections[k];
					}
					this.connections = array;
					AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
					return;
				}
			}
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0002BE54 File Offset: 0x0002A054
		public override void Open(ref Path.SearchContext ctx, uint pathNodeIndex, uint gScore)
		{
			float traversalCostMultiplier = ctx.traversalCosts.GetTraversalCostMultiplier(this);
			ctx.OpenCandidateConnectionsToEndNode(this.position, pathNodeIndex, pathNodeIndex, gScore, traversalCostMultiplier);
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				GraphNode node = this.connections[i].node;
				if (this.connections[i].isOutgoing && ctx.traversalConstraint.CanTraverse(this, node))
				{
					uint num = ctx.traversalCosts.GetConnectionCost(this, node);
					if (node is PointNode)
					{
						num += (uint)Math.Round((double)(this.connections[i].cost * 0.5f * (traversalCostMultiplier + ctx.traversalCosts.GetTraversalCostMultiplier(node))));
						ctx.OpenCandidateConnection(pathNodeIndex, node.NodeIndex, gScore + num, 0U, node.position);
					}
					else
					{
						node.OpenAtPoint(ref ctx, pathNodeIndex, this.position, gScore + num);
					}
				}
			}
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0002BF4C File Offset: 0x0002A14C
		public override void OpenAtPoint(ref Path.SearchContext ctx, uint pathNodeIndex, Int3 pos, uint gScore)
		{
			float traversalCostMultiplier = ctx.traversalCosts.GetTraversalCostMultiplier(this);
			ctx.OpenCandidateConnectionsToEndNode(pos, pathNodeIndex, pathNodeIndex, gScore, traversalCostMultiplier);
			uint num = (uint)Math.Round((double)((pos - this.position).magnitude * traversalCostMultiplier));
			ctx.OpenCandidateConnection(pathNodeIndex, base.NodeIndex, gScore + num, 0U, this.position);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0002BFA8 File Offset: 0x0002A1A8
		public override int GetGizmoHashCode()
		{
			int num = base.GetGizmoHashCode();
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					num ^= 17 * this.connections[i].GetHashCode();
				}
			}
			return num;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0002BFF5 File Offset: 0x0002A1F5
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0002C00A File Offset: 0x0002A20A
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0002C01F File Offset: 0x0002A21F
		public override void SerializeReferences(GraphSerializationContext ctx)
		{
			ctx.SerializeConnections(this.connections, true);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0002C02E File Offset: 0x0002A22E
		public override void DeserializeReferences(GraphSerializationContext ctx)
		{
			this.connections = ctx.DeserializeConnections(ctx.meta.version >= AstarSerializer.V4_3_85);
		}

		// Token: 0x04000540 RID: 1344
		public Connection[] connections;

		// Token: 0x04000541 RID: 1345
		public GameObject gameObject;
	}
}
