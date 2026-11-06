using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000015 RID: 21
	public class GeometryTreeNode : BinaryTreeNode
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003362 File Offset: 0x00001562
		// (set) Token: 0x0600007A RID: 122 RVA: 0x0000336A File Offset: 0x0000156A
		public int Index { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003373 File Offset: 0x00001573
		// (set) Token: 0x0600007C RID: 124 RVA: 0x0000337B File Offset: 0x0000157B
		public bool IsEmpty { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003384 File Offset: 0x00001584
		// (set) Token: 0x0600007E RID: 126 RVA: 0x0000338C File Offset: 0x0000158C
		public GeometryTreeNode Left { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003395 File Offset: 0x00001595
		// (set) Token: 0x06000080 RID: 128 RVA: 0x0000339D File Offset: 0x0000159D
		public GeometryTreeNode Right { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000033A6 File Offset: 0x000015A6
		public IReadOnlyList<CullingTarget> CullingTargets
		{
			get
			{
				return this._targets;
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000033AE File Offset: 0x000015AE
		public GeometryTreeNode(Vector3 center, Vector3 size, bool isLeaf)
			: base(center, size, isLeaf)
		{
			this.IsEmpty = true;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000033C0 File Offset: 0x000015C0
		public override BinaryTreeNode GetLeft()
		{
			return this.Left;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000033C8 File Offset: 0x000015C8
		public override BinaryTreeNode GetRight()
		{
			return this.Right;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000033D0 File Offset: 0x000015D0
		public void SetChilds(GeometryTreeNode left, GeometryTreeNode right)
		{
			this.Left = left;
			this.Right = right;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000033E0 File Offset: 0x000015E0
		public void AddCullingTarget(CullingTarget target)
		{
			if (this._targetsSet == null)
			{
				this._targetsSet = new HashSet<CullingTarget>();
				this._targets = new List<CullingTarget>();
			}
			if (this._targetsSet.Add(target))
			{
				this._targets.Add(target);
			}
		}

		// Token: 0x04000031 RID: 49
		private HashSet<CullingTarget> _targetsSet;

		// Token: 0x04000032 RID: 50
		private List<CullingTarget> _targets;
	}
}
