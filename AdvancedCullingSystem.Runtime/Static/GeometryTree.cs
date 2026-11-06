using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000014 RID: 20
	public class GeometryTree : BinaryTree<GeometryTreeNode, CullingTarget>
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000032DD File Offset: 0x000014DD
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000032E5 File Offset: 0x000014E5
		public IReadOnlyList<CullingTarget> CullingTargets { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000032EE File Offset: 0x000014EE
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000032F6 File Offset: 0x000014F6
		public int NodesCount { get; private set; }

		// Token: 0x06000073 RID: 115 RVA: 0x000032FF File Offset: 0x000014FF
		public GeometryTree(CullingTarget[] targets, int maxDepth)
			: base(targets, maxDepth)
		{
			this.CullingTargets = targets;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003310 File Offset: 0x00001510
		protected override GeometryTreeNode CreateNode(Vector3 center, Vector3 size, bool isLeaf)
		{
			int nodesCount = this.NodesCount;
			this.NodesCount = nodesCount + 1;
			return new GeometryTreeNode(center, size, isLeaf);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003335 File Offset: 0x00001535
		protected override Bounds GetBounds(CullingTarget target)
		{
			return target.Bounds;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000333D File Offset: 0x0000153D
		protected override void AddInternal(GeometryTreeNode node, CullingTarget data, int depth)
		{
			node.IsEmpty = false;
			base.AddInternal(node, data, depth);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000334F File Offset: 0x0000154F
		protected override void AddDataToNode(GeometryTreeNode node, CullingTarget target)
		{
			node.AddCullingTarget(target);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003358 File Offset: 0x00001558
		protected override void SetChildsToNode(GeometryTreeNode parent, GeometryTreeNode left, GeometryTreeNode right)
		{
			parent.SetChilds(left, right);
		}
	}
}
