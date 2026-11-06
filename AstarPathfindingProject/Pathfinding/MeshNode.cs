using System;
using Pathfinding.Pooling;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200009F RID: 159
	public abstract class MeshNode : GraphNode
	{
		// Token: 0x06000560 RID: 1376
		public abstract Int3 GetVertex(int i);

		// Token: 0x06000561 RID: 1377
		public abstract int GetVertexCount();

		// Token: 0x06000562 RID: 1378
		public abstract Vector3 ClosestPointOnNodeXZ(Vector3 p);

		// Token: 0x06000563 RID: 1379 RVA: 0x0001A178 File Offset: 0x00018378
		public override void ClearConnections(bool alsoReverse = true)
		{
			if (alsoReverse && this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					this.connections[i].node.RemovePartialConnection(this);
				}
			}
			ArrayPool<Connection>.Release(ref this.connections, true);
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001A1D8 File Offset: 0x000183D8
		public override void GetConnections<T>(GraphNode.NodeActionWithData<T> action, ref T data, int connectionFilter = 32)
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

		// Token: 0x06000565 RID: 1381 RVA: 0x0001A230 File Offset: 0x00018430
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
			return false;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001A282 File Offset: 0x00018482
		public override void AddPartialConnection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming)
		{
			this.AddPartialConnection(node, cost, Connection.PackShapeEdgeInfo(isOutgoing, isIncoming));
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001A294 File Offset: 0x00018494
		public void AddPartialConnection(GraphNode node, uint cost, byte shapeEdgeInfo)
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
						this.connections[i].shapeEdgeInfo = shapeEdgeInfo;
						return;
					}
				}
			}
			int num = ((this.connections != null) ? this.connections.Length : 0);
			Connection[] array = ArrayPool<Connection>.ClaimWithExactLength(num + 1);
			for (int j = 0; j < num; j++)
			{
				array[j] = this.connections[j];
			}
			array[num] = new Connection(node, cost, shapeEdgeInfo);
			if (this.connections != null)
			{
				ArrayPool<Connection>.Release(ref this.connections, true);
			}
			this.connections = array;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001A374 File Offset: 0x00018574
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
					Connection[] array = ArrayPool<Connection>.ClaimWithExactLength(num - 1);
					for (int j = 0; j < i; j++)
					{
						array[j] = this.connections[j];
					}
					for (int k = i + 1; k < num; k++)
					{
						array[k - 1] = this.connections[k];
					}
					if (this.connections != null)
					{
						ArrayPool<Connection>.Release(ref this.connections, true);
					}
					this.connections = array;
					AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
					return;
				}
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001A440 File Offset: 0x00018640
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

		// Token: 0x0600056A RID: 1386 RVA: 0x0001A48D File Offset: 0x0001868D
		public override void SerializeReferences(GraphSerializationContext ctx)
		{
			ctx.SerializeConnections(this.connections, true);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001A49C File Offset: 0x0001869C
		public override void DeserializeReferences(GraphSerializationContext ctx)
		{
			this.connections = ctx.DeserializeConnections(true);
		}

		// Token: 0x0400036A RID: 874
		public Connection[] connections;
	}
}
