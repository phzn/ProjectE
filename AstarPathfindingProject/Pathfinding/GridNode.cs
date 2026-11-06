using System;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000EB RID: 235
	public class GridNode : GridNodeBase
	{
		// Token: 0x0600080B RID: 2059 RVA: 0x0002A2B2 File Offset: 0x000284B2
		public GridNode()
		{
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0002A2BA File Offset: 0x000284BA
		public GridNode(AstarPath astar)
		{
			astar.InitializeNode(this);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0002A2C9 File Offset: 0x000284C9
		public static GridGraph GetGridGraph(uint graphIndex)
		{
			return GridNode._gridGraphs[(int)graphIndex];
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0002A2D4 File Offset: 0x000284D4
		public static void SetGridGraph(int graphIndex, GridGraph graph)
		{
			if (GridNode._gridGraphs.Length <= graphIndex)
			{
				GridGraph[] array = new GridGraph[graphIndex + 1];
				for (int i = 0; i < GridNode._gridGraphs.Length; i++)
				{
					array[i] = GridNode._gridGraphs[i];
				}
				GridNode._gridGraphs = array;
			}
			GridNode._gridGraphs[graphIndex] = graph;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0002A31E File Offset: 0x0002851E
		public static void ClearGridGraph(int graphIndex, GridGraph graph)
		{
			if (graphIndex < GridNode._gridGraphs.Length && GridNode._gridGraphs[graphIndex] == graph)
			{
				GridNode._gridGraphs[graphIndex] = null;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0002A33C File Offset: 0x0002853C
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x0002A344 File Offset: 0x00028544
		internal ushort InternalGridFlags
		{
			get
			{
				return this.gridFlags;
			}
			set
			{
				this.gridFlags = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x0002A34D File Offset: 0x0002854D
		public override bool HasConnectionsToAllEightNeighbours
		{
			get
			{
				return (this.InternalGridFlags & 255) == 255;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x0002A362 File Offset: 0x00028562
		public override bool HasConnectionsToAllAxisAlignedNeighbours
		{
			get
			{
				return (this.InternalGridFlags & 15) == 15;
			}
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0002A371 File Offset: 0x00028571
		public override bool HasConnectionInDirection(int dir)
		{
			return ((this.gridFlags >> dir) & 1) != 0;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0002A383 File Offset: 0x00028583
		public void SetConnection(int dir, bool value)
		{
			this.SetConnectionInternal(dir, value);
			GridNode.GetGridGraph(base.GraphIndex).nodeDataRef.connections[base.NodeInGridIndex] = (ulong)((long)this.GetAllConnectionInternal());
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0002A3B4 File Offset: 0x000285B4
		public void SetConnectionInternal(int dir, bool value)
		{
			this.gridFlags = (ushort)(((int)this.gridFlags & ~(1 << dir)) | ((value ? 1 : 0) << (dir & 31)));
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0002A3E8 File Offset: 0x000285E8
		public void SetAllConnectionInternal(int connections)
		{
			this.gridFlags = (ushort)(((int)this.gridFlags & -256) | connections);
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0002A40F File Offset: 0x0002860F
		public int GetAllConnectionInternal()
		{
			return (int)(this.gridFlags & 255);
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x0002A41D File Offset: 0x0002861D
		public override bool HasAnyGridConnections
		{
			get
			{
				return this.GetAllConnectionInternal() != 0;
			}
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0002A428 File Offset: 0x00028628
		public override void ResetConnectionsInternal()
		{
			this.gridFlags = (ushort)((int)this.gridFlags & -256);
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0002A44D File Offset: 0x0002864D
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x0002A45E File Offset: 0x0002865E
		public bool EdgeNode
		{
			get
			{
				return (this.gridFlags & 1024) > 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -1025) | (value ? 1024 : 0));
			}
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0002A480 File Offset: 0x00028680
		public override GridNodeBase GetNeighbourAlongDirection(int direction)
		{
			if (this.HasConnectionInDirection(direction))
			{
				GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
				return gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[direction]];
			}
			return null;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0002A4BC File Offset: 0x000286BC
		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse)
			{
				for (int i = 0; i < 8; i++)
				{
					GridNode gridNode = this.GetNeighbourAlongDirection(i) as GridNode;
					if (gridNode != null)
					{
						gridNode.SetConnectionInternal(GridNodeBase.OppositeConnectionDirection(i), false);
					}
				}
			}
			this.ResetConnectionsInternal();
			base.ClearConnections(alsoReverse);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0002A504 File Offset: 0x00028704
		public override void GetConnections<T>(GraphNode.NodeActionWithData<T> action, ref T data, int connectionFilter)
		{
			if ((connectionFilter & 48) == 0)
			{
				return;
			}
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNodeBase[] nodes = gridGraph.nodes;
			for (int i = 0; i < 8; i++)
			{
				if (((this.gridFlags >> i) & 1) != 0)
				{
					GridNodeBase gridNodeBase = nodes[base.NodeInGridIndex + neighbourOffsets[i]];
					if (gridNodeBase != null)
					{
						action(gridNodeBase, ref data);
					}
				}
			}
			base.GetConnections<T>(action, ref data, connectionFilter);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0002A56C File Offset: 0x0002876C
		public override bool GetPortal(GraphNode other, out Vector3 left, out Vector3 right)
		{
			if (other.GraphIndex != base.GraphIndex)
			{
				left = (right = Vector3.zero);
				return false;
			}
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			Vector2Int vector2Int = (other as GridNodeBase).CoordinatesInGrid - base.CoordinatesInGrid;
			int num = GridNodeBase.OffsetToConnectionDirection(vector2Int.x, vector2Int.y);
			if (num == -1 || !this.HasConnectionInDirection(num))
			{
				left = (right = Vector3.zero);
				return false;
			}
			if (num < 4)
			{
				Vector3 vector = (Vector3)(this.position + other.position) * 0.5f;
				Vector3 vector2 = gridGraph.transform.TransformVector(new Vector3((float)vector2Int.y, 0f, (float)(-(float)vector2Int.x)) * 0.5f);
				left = vector - vector2;
				right = vector + vector2;
			}
			else
			{
				bool flag = false;
				bool flag2 = false;
				if (this.HasConnectionInDirection(num - 4))
				{
					GridNodeBase gridNodeBase = gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[num - 4]];
					if (gridNodeBase.Walkable && gridNodeBase.HasConnectionInDirection((num - 4 + 1) % 4))
					{
						flag = true;
					}
				}
				if (this.HasConnectionInDirection((num - 4 + 1) % 4))
				{
					GridNodeBase gridNodeBase2 = gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[(num - 4 + 1) % 4]];
					if (gridNodeBase2.Walkable && gridNodeBase2.HasConnectionInDirection(num - 4))
					{
						flag2 = true;
					}
				}
				Vector3 vector3 = (Vector3)(this.position + other.position) * 0.5f;
				Vector3 vector4 = gridGraph.transform.TransformVector(new Vector3((float)vector2Int.y, 0f, (float)(-(float)vector2Int.x)));
				left = vector3 - (flag2 ? vector4 : Vector3.zero);
				right = vector3 + (flag ? vector4 : Vector3.zero);
			}
			return true;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0002A780 File Offset: 0x00028980
		public static int FilterDiagonalConnections(int conns, NumNeighbours neighbours, bool cutCorners)
		{
			switch (neighbours)
			{
			case NumNeighbours.Four:
				return conns & 15;
			case NumNeighbours.Six:
				return conns & 175;
			}
			if (cutCorners)
			{
				int num = conns & 15;
				int num2 = (num | ((num >> 1) | (num << 3))) << 4;
				num2 &= conns;
				return num | num2;
			}
			int num3 = conns & 15;
			int num4 = (num3 & ((num3 >> 1) | (num3 << 3))) << 4;
			num4 &= conns;
			return num3 | num4;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0002A7E4 File Offset: 0x000289E4
		public override void Open(ref Path.SearchContext ctx, uint pathNodeIndex, uint gScore)
		{
			float traversalCostMultiplier = ctx.traversalCosts.GetTraversalCostMultiplier(this);
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNodeBase[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			int num = (int)(this.gridFlags & 255);
			float num2 = gridGraph.neighbourCosts[0];
			for (int i = 0; i < 4; i++)
			{
				if (((num >> i) & 1) != 0)
				{
					GridNodeBase gridNodeBase = nodes[nodeInGridIndex + neighbourOffsets[i]];
					if (ctx.traversalConstraint.CanTraverse(this, gridNodeBase))
					{
						if (ctx.ShouldConsiderPathNode(gridNodeBase.NodeIndex))
						{
							uint num3 = ctx.traversalCosts.GetConnectionCost(this, gridNodeBase);
							num3 += (uint)Mathf.Round((traversalCostMultiplier + ctx.traversalCosts.GetTraversalCostMultiplier(gridNodeBase)) * (0.5f * num2));
							ctx.OpenCandidateConnection(pathNodeIndex, gridNodeBase.NodeIndex, gScore + num3, 0U, gridNodeBase.position);
						}
					}
					else
					{
						num &= ~(1 << i);
					}
				}
			}
			if (ctx.traversalConstraint.filterDiagonalGridConnections)
			{
				num = GridNode.FilterDiagonalConnections(num, gridGraph.neighbours, gridGraph.cutCorners);
			}
			float num4 = gridGraph.neighbourCosts[4];
			for (int j = 4; j < 8; j++)
			{
				if (((num >> j) & 1) != 0)
				{
					GridNodeBase gridNodeBase2 = nodes[nodeInGridIndex + neighbourOffsets[j]];
					if (ctx.ShouldConsiderPathNode(gridNodeBase2.NodeIndex) && ctx.traversalConstraint.CanTraverse(this, gridNodeBase2))
					{
						uint num5 = ctx.traversalCosts.GetConnectionCost(this, gridNodeBase2);
						num5 += (uint)Mathf.Round((traversalCostMultiplier + ctx.traversalCosts.GetTraversalCostMultiplier(gridNodeBase2)) * (0.5f * num4));
						ctx.OpenCandidateConnection(pathNodeIndex, gridNodeBase2.NodeIndex, gScore + num5, 0U, gridNodeBase2.position);
					}
				}
			}
			base.Open(ref ctx, pathNodeIndex, gScore);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0002A9B1 File Offset: 0x00028BB1
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
			ctx.writer.Write(this.gridFlags);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0002A9D7 File Offset: 0x00028BD7
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
			this.gridFlags = ctx.reader.ReadUInt16();
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0002AA00 File Offset: 0x00028C00
		public override void AddPartialConnection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming)
		{
			GridNode gridNode = node as GridNode;
			if (gridNode != null && gridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(gridNode);
			}
			base.AddPartialConnection(node, cost, isOutgoing, isIncoming);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0002AA38 File Offset: 0x00028C38
		public override void RemovePartialConnection(GraphNode node)
		{
			base.RemovePartialConnection(node);
			GridNode gridNode = node as GridNode;
			if (gridNode != null && gridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(gridNode);
			}
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0002AA6C File Offset: 0x00028C6C
		protected void RemoveGridConnection(GridNode node)
		{
			int nodeInGridIndex = base.NodeInGridIndex;
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			for (int i = 0; i < 8; i++)
			{
				if (nodeInGridIndex + gridGraph.neighbourOffsets[i] == node.NodeInGridIndex && this.GetNeighbourAlongDirection(i) == node)
				{
					this.SetConnectionInternal(i, false);
					return;
				}
			}
		}

		// Token: 0x04000524 RID: 1316
		private static GridGraph[] _gridGraphs = new GridGraph[0];

		// Token: 0x04000525 RID: 1317
		private const int GridFlagsConnectionOffset = 0;

		// Token: 0x04000526 RID: 1318
		private const int GridFlagsConnectionBit0 = 1;

		// Token: 0x04000527 RID: 1319
		private const int GridFlagsConnectionMask = 255;

		// Token: 0x04000528 RID: 1320
		private const int GridFlagsAxisAlignedConnectionMask = 15;

		// Token: 0x04000529 RID: 1321
		private const int GridFlagsEdgeNodeOffset = 10;

		// Token: 0x0400052A RID: 1322
		private const int GridFlagsEdgeNodeMask = 1024;
	}
}
