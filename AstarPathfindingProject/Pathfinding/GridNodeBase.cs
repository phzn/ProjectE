using System;
using System.Runtime.CompilerServices;
using Pathfinding.Serialization;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000EC RID: 236
	public abstract class GridNodeBase : GraphNode
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x0002AACA File Offset: 0x00028CCA
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x0002AAD8 File Offset: 0x00028CD8
		public int NodeInGridIndex
		{
			get
			{
				return this.nodeInGridIndex & 16777215;
			}
			set
			{
				this.nodeInGridIndex = (this.nodeInGridIndex & -16777216) | value;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0002AAEE File Offset: 0x00028CEE
		public int XCoordinateInGrid
		{
			get
			{
				return this.NodeInGridIndex % GridNode.GetGridGraph(base.GraphIndex).width;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x0002AB07 File Offset: 0x00028D07
		public int ZCoordinateInGrid
		{
			get
			{
				return this.NodeInGridIndex / GridNode.GetGridGraph(base.GraphIndex).width;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0002AB20 File Offset: 0x00028D20
		public Vector2Int CoordinatesInGrid
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				int width = GridNode.GetGridGraph(base.GraphIndex).width;
				int num = this.NodeInGridIndex;
				int num2 = num / width;
				return new Vector2Int(num - num2 * width, num2);
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x0002AB52 File Offset: 0x00028D52
		// (set) Token: 0x0600082F RID: 2095 RVA: 0x0002AB63 File Offset: 0x00028D63
		public bool WalkableErosion
		{
			get
			{
				return (this.gridFlags & 256) > 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -257) | (value ? 256 : 0));
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x0002AB84 File Offset: 0x00028D84
		// (set) Token: 0x06000831 RID: 2097 RVA: 0x0002AB95 File Offset: 0x00028D95
		public bool TmpWalkable
		{
			get
			{
				return (this.gridFlags & 512) > 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -513) | (value ? 512 : 0));
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000832 RID: 2098
		public abstract bool HasConnectionsToAllEightNeighbours { get; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000833 RID: 2099
		public abstract bool HasConnectionsToAllAxisAlignedNeighbours { get; }

		// Token: 0x06000834 RID: 2100 RVA: 0x0002ABB6 File Offset: 0x00028DB6
		public static int OppositeConnectionDirection(int dir)
		{
			if (dir >= 4)
			{
				return (dir - 2) % 4 + 4;
			}
			return (dir + 2) % 4;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0002ABC9 File Offset: 0x00028DC9
		public static int OffsetToConnectionDirection(int dx, int dz)
		{
			dx++;
			dz++;
			if (dx > 2 || dz > 2)
			{
				return -1;
			}
			return GridNodeBase.offsetToDirection[3 * dz + dx];
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0002ABEC File Offset: 0x00028DEC
		public Vector3 ProjectOnSurface(Vector3 point)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			Vector3 vector = (Vector3)this.position;
			Vector3 vector2 = gridGraph.transform.WorldUpAtGraphPosition(vector);
			return point - vector2 * Vector3.Dot(vector2, point - vector);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0002AC38 File Offset: 0x00028E38
		public override Vector3 ClosestPointOnNode(Vector3 p)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			Vector3 vector = (Vector3)this.position;
			Vector3 vector2 = gridGraph.transform.InverseTransformVector(p - vector);
			vector2.y = 0f;
			vector2.x = Mathf.Clamp(vector2.x, -0.5f, 0.5f);
			vector2.z = Mathf.Clamp(vector2.z, -0.5f, 0.5f);
			return vector + gridGraph.transform.TransformVector(vector2);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0002ACC8 File Offset: 0x00028EC8
		public override bool ContainsPoint(Vector3 point)
		{
			GridGraph gridGraph = base.Graph as GridGraph;
			return this.ContainsPointInGraphSpace((Int3)gridGraph.transform.InverseTransform(point));
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0002ACF8 File Offset: 0x00028EF8
		public override bool ContainsPointInGraphSpace(Int3 point)
		{
			int num = this.XCoordinateInGrid * 1000;
			int num2 = this.ZCoordinateInGrid * 1000;
			return point.x >= num && point.x <= num + 1000 && point.z >= num2 && point.z <= num2 + 1000;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0002AD54 File Offset: 0x00028F54
		public override float SurfaceArea()
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			return gridGraph.nodeSize * gridGraph.nodeSize;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0002AD7C File Offset: 0x00028F7C
		public override Vector3 RandomPointOnSurface()
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			Vector3 vector = gridGraph.transform.InverseTransform((Vector3)this.position);
			float2 @float = AstarMath.ThreadSafeRandomFloat2();
			return gridGraph.transform.Transform(vector + new Vector3(@float.x - 0.5f, 0f, @float.y - 0.5f));
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0002ADE4 File Offset: 0x00028FE4
		public Vector2 NormalizePoint(Vector3 worldPoint)
		{
			Vector3 vector = GridNode.GetGridGraph(base.GraphIndex).transform.InverseTransform(worldPoint);
			return new Vector2(vector.x - (float)this.XCoordinateInGrid, vector.z - (float)this.ZCoordinateInGrid);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0002AE2C File Offset: 0x0002902C
		public Vector3 UnNormalizePoint(Vector2 normalizedPointOnSurface)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			return (Vector3)this.position + gridGraph.transform.TransformVector(new Vector3(normalizedPointOnSurface.x - 0.5f, 0f, normalizedPointOnSurface.y - 0.5f));
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0002AE84 File Offset: 0x00029084
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
			return num ^ (int)(109 * this.gridFlags);
		}

		// Token: 0x0600083F RID: 2111
		public abstract GridNodeBase GetNeighbourAlongDirection(int direction);

		// Token: 0x06000840 RID: 2112 RVA: 0x0002AEDD File Offset: 0x000290DD
		public virtual bool HasConnectionInDirection(int direction)
		{
			return this.GetNeighbourAlongDirection(direction) != null;
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000841 RID: 2113
		public abstract bool HasAnyGridConnections { get; }

		// Token: 0x06000842 RID: 2114 RVA: 0x0002AEEC File Offset: 0x000290EC
		public override bool ContainsOutgoingConnection(GraphNode node)
		{
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node == node && this.connections[i].isOutgoing)
					{
						return true;
					}
				}
			}
			for (int j = 0; j < 8; j++)
			{
				if (node == this.GetNeighbourAlongDirection(j))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000843 RID: 2115
		public abstract void ResetConnectionsInternal();

		// Token: 0x06000844 RID: 2116 RVA: 0x0002AF56 File Offset: 0x00029156
		public override void OpenAtPoint(ref Path.SearchContext ctx, uint pathNodeIndex, Int3 pos, uint gScore)
		{
			ctx.OpenCandidateConnectionsToEndNode(pos, pathNodeIndex, pathNodeIndex, gScore, 0f);
			ctx.OpenCandidateConnection(pathNodeIndex, base.NodeIndex, gScore, 0U, this.position);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002AF80 File Offset: 0x00029180
		public override void Open(ref Path.SearchContext ctx, uint pathNodeIndex, uint gScore)
		{
			ctx.OpenCandidateConnectionsToEndNode(this.position, pathNodeIndex, pathNodeIndex, gScore, 0f);
			if (this.connections != null)
			{
				float traversalCostMultiplier = ctx.traversalCosts.GetTraversalCostMultiplier(this);
				for (int i = 0; i < this.connections.Length; i++)
				{
					GraphNode node = this.connections[i].node;
					if (this.connections[i].isOutgoing && ctx.traversalConstraint.CanTraverse(this, node))
					{
						uint num = ctx.traversalCosts.GetConnectionCost(this, node);
						num += (uint)((traversalCostMultiplier + ctx.traversalCosts.GetTraversalCostMultiplier(node)) * 0.5f * this.connections[i].cost);
						ctx.OpenCandidateConnection(pathNodeIndex, node.NodeIndex, gScore + num, 0U, node.position);
					}
				}
			}
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0002B058 File Offset: 0x00029258
		public void ClearCustomConnections(bool alsoReverse)
		{
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					this.connections[i].node.RemovePartialConnection(this);
				}
				this.connections = null;
				AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
			}
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0002B0AE File Offset: 0x000292AE
		public override void ClearConnections(bool alsoReverse)
		{
			this.ClearCustomConnections(alsoReverse);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0002B0B8 File Offset: 0x000292B8
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

		// Token: 0x06000849 RID: 2121 RVA: 0x0002B110 File Offset: 0x00029310
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
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0002B1E8 File Offset: 0x000293E8
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

		// Token: 0x0600084B RID: 2123 RVA: 0x0002B29D File Offset: 0x0002949D
		public override void SerializeReferences(GraphSerializationContext ctx)
		{
			ctx.SerializeConnections(this.connections, true);
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0002B2AC File Offset: 0x000294AC
		public override void DeserializeReferences(GraphSerializationContext ctx)
		{
			if (ctx.meta.version < AstarSerializer.V3_8_3)
			{
				return;
			}
			this.connections = ctx.DeserializeConnections(ctx.meta.version >= AstarSerializer.V4_3_85);
		}

		// Token: 0x0400052B RID: 1323
		private const int GridFlagsWalkableErosionOffset = 8;

		// Token: 0x0400052C RID: 1324
		private const int GridFlagsWalkableErosionMask = 256;

		// Token: 0x0400052D RID: 1325
		private const int GridFlagsWalkableTmpOffset = 9;

		// Token: 0x0400052E RID: 1326
		private const int GridFlagsWalkableTmpMask = 512;

		// Token: 0x0400052F RID: 1327
		public const int NodeInGridIndexLayerOffset = 24;

		// Token: 0x04000530 RID: 1328
		protected const int NodeInGridIndexMask = 16777215;

		// Token: 0x04000531 RID: 1329
		protected int nodeInGridIndex;

		// Token: 0x04000532 RID: 1330
		protected ushort gridFlags;

		// Token: 0x04000533 RID: 1331
		public Connection[] connections;

		// Token: 0x04000534 RID: 1332
		internal static readonly int[] offsetToDirection = new int[] { 7, 0, 4, 3, -1, 1, 6, 2, 5 };
	}
}
