using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000087 RID: 135
	public class NodeLink3Node : PointNode
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x000176BC File Offset: 0x000158BC
		public NodeLink3Node(AstarPath astar)
		{
			astar.InitializeNode(this);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000176CC File Offset: 0x000158CC
		public override bool GetPortal(GraphNode other, out Vector3 left, out Vector3 right)
		{
			left = this.portalA;
			right = this.portalB;
			if (this.connections.Length < 2)
			{
				return false;
			}
			if (this.connections.Length != 2)
			{
				throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + this.connections.Length.ToString());
			}
			return true;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001772C File Offset: 0x0001592C
		public GraphNode GetOther(GraphNode a)
		{
			if (this.connections.Length < 2)
			{
				return null;
			}
			if (this.connections.Length != 2)
			{
				throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + this.connections.Length.ToString());
			}
			if (a != this.connections[0].node)
			{
				return (this.connections[0].node as NodeLink3Node).GetOtherInternal(this);
			}
			return (this.connections[1].node as NodeLink3Node).GetOtherInternal(this);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000177C0 File Offset: 0x000159C0
		private GraphNode GetOtherInternal(GraphNode a)
		{
			if (this.connections.Length < 2)
			{
				return null;
			}
			if (a != this.connections[0].node)
			{
				return this.connections[0].node;
			}
			return this.connections[1].node;
		}

		// Token: 0x040002E8 RID: 744
		public NodeLink3 link;

		// Token: 0x040002E9 RID: 745
		public Vector3 portalA;

		// Token: 0x040002EA RID: 746
		public Vector3 portalB;
	}
}
