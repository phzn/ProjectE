using System;
using System.Collections.Generic;
using Pathfinding.Drawing;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000D9 RID: 217
	[JsonOptIn]
	[Preserve]
	public class LinkGraph : NavGraph
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x00019DC2 File Offset: 0x00017FC2
		public override bool isScanned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0001AA07 File Offset: 0x00018C07
		public override bool persistent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0001AA07 File Offset: 0x00018C07
		public override bool showInInspector
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000253A7 File Offset: 0x000235A7
		public override int CountNodes()
		{
			return this.nodeCount;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000253AF File Offset: 0x000235AF
		protected override void DestroyAllNodes()
		{
			base.DestroyAllNodes();
			this.nodes = new LinkNode[0];
			this.nodeCount = 0;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x000253CC File Offset: 0x000235CC
		public override void GetNodes<T>(GraphNode.NodeActionWithData<T> action, ref T data)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodeCount; i++)
			{
				action(this.nodes[i], ref data);
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00025404 File Offset: 0x00023604
		internal LinkNode AddNode()
		{
			base.AssertSafeToUpdateGraph();
			if (this.nodeCount >= this.nodes.Length)
			{
				Memory.Realloc<LinkNode>(ref this.nodes, Mathf.Max(16, this.nodeCount * 2));
			}
			this.nodeCount++;
			LinkNode[] array = this.nodes;
			int num = this.nodeCount - 1;
			LinkNode linkNode = new LinkNode(this.active);
			linkNode.nodeInGraphIndex = this.nodeCount - 1;
			linkNode.GraphIndex = this.graphIndex;
			linkNode.Walkable = true;
			LinkNode linkNode2 = linkNode;
			array[num] = linkNode;
			return linkNode2;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00025490 File Offset: 0x00023690
		internal void RemoveNode(LinkNode node)
		{
			if (this.nodes[node.nodeInGraphIndex] != node)
			{
				throw new ArgumentException("Node is not in this graph");
			}
			this.nodeCount--;
			this.nodes[node.nodeInGraphIndex] = this.nodes[this.nodeCount];
			this.nodes[node.nodeInGraphIndex].nodeInGraphIndex = node.nodeInGraphIndex;
			this.nodes[this.nodeCount] = null;
			node.Destroy();
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0002550C File Offset: 0x0002370C
		public override float NearestNodeDistanceSqrLowerBound(Vector3 position, ref NearestNodeConstraint constraint)
		{
			return float.PositiveInfinity;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00025514 File Offset: 0x00023714
		public override NNInfo GetNearest(Vector3 position, ref NearestNodeConstraint constraint)
		{
			return default(NNInfo);
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0002552A File Offset: 0x0002372A
		public override void OnDrawGizmos(DrawingData gizmos, bool drawNodes, RedrawScope redrawScope, bool renderInGame)
		{
			base.OnDrawGizmos(gizmos, drawNodes, redrawScope, renderInGame);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00025537 File Offset: 0x00023737
		protected override IGraphUpdatePromise ScanInternal()
		{
			return new LinkGraph.LinkGraphUpdatePromise
			{
				graph = this
			};
		}

		// Token: 0x040004C3 RID: 1219
		private LinkNode[] nodes = new LinkNode[0];

		// Token: 0x040004C4 RID: 1220
		private int nodeCount;

		// Token: 0x020000DA RID: 218
		private class LinkGraphUpdatePromise : IGraphUpdatePromise
		{
			// Token: 0x0600074B RID: 1867 RVA: 0x00025559 File Offset: 0x00023759
			public void Apply(IGraphUpdateContext ctx)
			{
				this.graph.DestroyAllNodes();
			}

			// Token: 0x0600074C RID: 1868 RVA: 0x00003792 File Offset: 0x00001992
			public IEnumerator<JobHandle> Prepare()
			{
				return null;
			}

			// Token: 0x040004C5 RID: 1221
			public LinkGraph graph;
		}
	}
}
