using System;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000ED RID: 237
	public class LevelGridNode : GridNodeBase
	{
		// Token: 0x0600084F RID: 2127 RVA: 0x0002A2B2 File Offset: 0x000284B2
		public LevelGridNode()
		{
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0002A2BA File Offset: 0x000284BA
		public LevelGridNode(AstarPath astar)
		{
			astar.InitializeNode(this);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0002B300 File Offset: 0x00029500
		public static LayerGridGraph GetGridGraph(uint graphIndex)
		{
			return LevelGridNode._gridGraphs[(int)graphIndex];
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0002B30C File Offset: 0x0002950C
		public static void SetGridGraph(int graphIndex, LayerGridGraph graph)
		{
			GridNode.SetGridGraph(graphIndex, graph);
			if (LevelGridNode._gridGraphs.Length <= graphIndex)
			{
				LayerGridGraph[] array = new LayerGridGraph[graphIndex + 1];
				for (int i = 0; i < LevelGridNode._gridGraphs.Length; i++)
				{
					array[i] = LevelGridNode._gridGraphs[i];
				}
				LevelGridNode._gridGraphs = array;
			}
			LevelGridNode._gridGraphs[graphIndex] = graph;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0002B35D File Offset: 0x0002955D
		public static void ClearGridGraph(int graphIndex, LayerGridGraph graph)
		{
			if (graphIndex < LevelGridNode._gridGraphs.Length && LevelGridNode._gridGraphs[graphIndex] == graph)
			{
				LevelGridNode._gridGraphs[graphIndex] = null;
			}
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0002B37B File Offset: 0x0002957B
		public override void ResetConnectionsInternal()
		{
			this.gridConnections = uint.MaxValue;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x0002B394 File Offset: 0x00029594
		public override bool HasAnyGridConnections
		{
			get
			{
				return this.gridConnections != uint.MaxValue;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0002B3A4 File Offset: 0x000295A4
		public override bool HasConnectionsToAllEightNeighbours
		{
			get
			{
				for (int i = 0; i < 8; i++)
				{
					if (!this.HasConnectionInDirection(i))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x0002B3C9 File Offset: 0x000295C9
		public override bool HasConnectionsToAllAxisAlignedNeighbours
		{
			get
			{
				return (this.gridConnections & 65535U) == 65535U;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x0002B3DE File Offset: 0x000295DE
		// (set) Token: 0x06000859 RID: 2137 RVA: 0x0002B3E9 File Offset: 0x000295E9
		public int LayerCoordinateInGrid
		{
			get
			{
				return this.nodeInGridIndex >> 24;
			}
			set
			{
				this.nodeInGridIndex = (this.nodeInGridIndex & 16777215) | (value << 24);
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0002B402 File Offset: 0x00029602
		public void SetPosition(Int3 position)
		{
			this.position = position;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0002B40B File Offset: 0x0002960B
		public override int GetGizmoHashCode()
		{
			return base.GetGizmoHashCode() ^ (int)((805306457UL * (ulong)this.gridConnections) ^ (402653189UL * (ulong)this.gridConnections));
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0002B434 File Offset: 0x00029634
		public override GridNodeBase GetNeighbourAlongDirection(int direction)
		{
			int connectionValue = this.GetConnectionValue(direction);
			if (connectionValue != 15)
			{
				LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
				return gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[direction] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
			}
			return null;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0002B484 File Offset: 0x00029684
		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse)
			{
				LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
				int[] neighbourOffsets = gridGraph.neighbourOffsets;
				GridNodeBase[] nodes = gridGraph.nodes;
				for (int i = 0; i < 8; i++)
				{
					int connectionValue = this.GetConnectionValue(i);
					if (connectionValue != 15)
					{
						LevelGridNode levelGridNode = nodes[base.NodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue] as LevelGridNode;
						if (levelGridNode != null)
						{
							levelGridNode.SetConnectionValue((i + 2) % 4, 15);
						}
					}
				}
			}
			this.ResetConnectionsInternal();
			base.ClearConnections(alsoReverse);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0002B50C File Offset: 0x0002970C
		public override void GetConnections<T>(GraphNode.NodeActionWithData<T> action, ref T data, int connectionFilter)
		{
			if ((connectionFilter & 48) == 0)
			{
				return;
			}
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNodeBase[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 8; i++)
			{
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 15)
				{
					GridNodeBase gridNodeBase = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					if (gridNodeBase != null)
					{
						action(gridNodeBase, ref data);
					}
				}
			}
			base.GetConnections<T>(action, ref data, connectionFilter);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0002B592 File Offset: 0x00029792
		public override bool HasConnectionInDirection(int direction)
		{
			return ((this.gridConnections >> direction * 4) & 15U) != 15U;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0002B5AB File Offset: 0x000297AB
		public void SetConnectionValue(int dir, int value)
		{
			this.gridConnections = (this.gridConnections & ~(15U << dir * 4)) | (uint)((uint)value << dir * 4);
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0002B5DD File Offset: 0x000297DD
		public void SetAllConnectionInternal(ulong value)
		{
			this.gridConnections = (uint)value;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0002B5E7 File Offset: 0x000297E7
		public int GetConnectionValue(int dir)
		{
			return (int)((this.gridConnections >> dir * 4) & 15U);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0002B5FC File Offset: 0x000297FC
		public override void AddPartialConnection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming)
		{
			LevelGridNode levelGridNode = node as LevelGridNode;
			if (levelGridNode != null && levelGridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(levelGridNode);
			}
			base.AddPartialConnection(node, cost, isOutgoing, isIncoming);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0002B634 File Offset: 0x00029834
		public override void RemovePartialConnection(GraphNode node)
		{
			base.RemovePartialConnection(node);
			LevelGridNode levelGridNode = node as LevelGridNode;
			if (levelGridNode != null && levelGridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(levelGridNode);
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0002B668 File Offset: 0x00029868
		protected void RemoveGridConnection(LevelGridNode node)
		{
			int nodeInGridIndex = base.NodeInGridIndex;
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			for (int i = 0; i < 8; i++)
			{
				if (nodeInGridIndex + gridGraph.neighbourOffsets[i] == node.NodeInGridIndex && this.GetNeighbourAlongDirection(i) == node)
				{
					this.SetConnectionValue(i, 15);
					return;
				}
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0002B6BC File Offset: 0x000298BC
		public override bool GetPortal(GraphNode other, out Vector3 left, out Vector3 right)
		{
			if (other.GraphIndex != base.GraphIndex)
			{
				left = (right = Vector3.zero);
				return false;
			}
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			Vector2Int vector2Int = (other as GridNodeBase).CoordinatesInGrid - base.CoordinatesInGrid;
			int num = GridNodeBase.OffsetToConnectionDirection(vector2Int.x, vector2Int.y);
			if (num == -1 || this.GetNeighbourAlongDirection(num) != other)
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
				GridNodeBase gridNodeBase = this.GetNeighbourAlongDirection(num - 4);
				if (gridNodeBase != null && gridNodeBase.Walkable && gridNodeBase.GetNeighbourAlongDirection((num - 4 + 1) % 4) == other)
				{
					flag = true;
				}
				gridNodeBase = this.GetNeighbourAlongDirection((num - 4 + 1) % 4);
				if (gridNodeBase != null && gridNodeBase.Walkable && gridNodeBase.GetNeighbourAlongDirection(num - 4) == other)
				{
					flag2 = true;
				}
				Vector3 vector3 = (Vector3)(this.position + other.position) * 0.5f;
				Vector3 vector4 = gridGraph.transform.TransformVector(new Vector3((float)vector2Int.y, 0f, (float)(-(float)vector2Int.x)));
				left = vector3 - (flag2 ? vector4 : Vector3.zero);
				right = vector3 + (flag ? vector4 : Vector3.zero);
			}
			return true;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0002B8A0 File Offset: 0x00029AA0
		public override void Open(ref Path.SearchContext ctx, uint pathNodeIndex, uint gScore)
		{
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			float[] neighbourCosts = gridGraph.neighbourCosts;
			GridNodeBase[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			float traversalCostMultiplier = ctx.traversalCosts.GetTraversalCostMultiplier(this);
			int num = 255;
			for (int i = 0; i < 8; i++)
			{
				if (i == 4 && ctx.traversalConstraint.filterDiagonalGridConnections)
				{
					num = GridNode.FilterDiagonalConnections(num, gridGraph.neighbours, gridGraph.cutCorners);
				}
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 15 && ((num >> i) & 1) != 0)
				{
					GraphNode graphNode = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					if (!ctx.traversalConstraint.CanTraverse(this, graphNode))
					{
						num &= ~(1 << i);
					}
					else
					{
						uint num2 = ctx.traversalCosts.GetConnectionCost(this, graphNode);
						num2 += (uint)Math.Round((double)((traversalCostMultiplier + ctx.traversalCosts.GetTraversalCostMultiplier(graphNode)) * (0.5f * neighbourCosts[i])));
						ctx.OpenCandidateConnection(pathNodeIndex, graphNode.NodeIndex, gScore + num2, 0U, graphNode.position);
					}
				}
				else
				{
					num &= ~(1 << i);
				}
			}
			base.Open(ref ctx, pathNodeIndex, gScore);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0002B9F0 File Offset: 0x00029BF0
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
			ctx.writer.Write(this.gridFlags);
			ulong num = 0UL;
			for (int i = 0; i < 8; i++)
			{
				num |= (ulong)((ulong)((long)this.GetConnectionValue(i)) << i * 8);
			}
			ctx.writer.Write(num);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0002BA50 File Offset: 0x00029C50
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
			this.gridFlags = ctx.reader.ReadUInt16();
			if (ctx.meta.version < AstarSerializer.V4_3_12)
			{
				ulong num;
				if (ctx.meta.version < AstarSerializer.V3_9_0)
				{
					num = (ulong)ctx.reader.ReadUInt32() | 1085102592318504960UL;
				}
				else
				{
					num = ctx.reader.ReadUInt64();
				}
				this.gridConnections = 0U;
				for (int i = 0; i < 8; i++)
				{
					ulong num2 = (num >> i * 8) & 255UL;
					if ((num2 & 15UL) != num2)
					{
						num2 = 15UL;
					}
					this.SetConnectionValue(i, (int)num2);
				}
				return;
			}
			ulong num3 = ctx.reader.ReadUInt64();
			uint num4 = 0U;
			if (ctx.meta.version < AstarSerializer.V4_3_83)
			{
				num4 = (uint)num3;
			}
			else
			{
				for (int j = 0; j < 8; j++)
				{
					num4 |= ((uint)(num3 >> j * 8) & 15U) << 4 * j;
				}
			}
			this.gridConnections = num4;
		}

		// Token: 0x04000535 RID: 1333
		private static LayerGridGraph[] _gridGraphs = new LayerGridGraph[0];

		// Token: 0x04000536 RID: 1334
		public uint gridConnections;

		// Token: 0x04000537 RID: 1335
		protected static LayerGridGraph[] gridGraphs;

		// Token: 0x04000538 RID: 1336
		private const int MaxNeighbours = 8;

		// Token: 0x04000539 RID: 1337
		public const int ConnectionMask = 15;

		// Token: 0x0400053A RID: 1338
		public const int ConnectionStride = 4;

		// Token: 0x0400053B RID: 1339
		public const int AxisAlignedConnectionsMask = 65535;

		// Token: 0x0400053C RID: 1340
		public const uint AllConnectionsMask = 4294967295U;

		// Token: 0x0400053D RID: 1341
		public const int NoConnection = 15;

		// Token: 0x0400053E RID: 1342
		internal const ulong DiagonalConnectionsMask = 4294901760UL;

		// Token: 0x0400053F RID: 1343
		public const int MaxLayerCount = 15;
	}
}
