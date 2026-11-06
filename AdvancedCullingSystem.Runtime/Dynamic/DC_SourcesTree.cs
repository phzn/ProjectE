using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000054 RID: 84
	public class DC_SourcesTree : BinaryTree<DC_SourcesTreeNode, ICullingTarget>, IDC_SourcesProvider
	{
		// Token: 0x0600020C RID: 524 RVA: 0x000092B3 File Offset: 0x000074B3
		public DC_SourcesTree(float nodeSize)
			: base(nodeSize)
		{
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000092BC File Offset: 0x000074BC
		public DC_Source GetSource(ICullingTarget cullingTarget)
		{
			this._lastModifiedSource = null;
			base.Add(cullingTarget);
			return this._lastModifiedSource;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x000092D4 File Offset: 0x000074D4
		protected override void AddInternal(DC_SourcesTreeNode node, ICullingTarget data, int depth)
		{
			if (node.IsLeaf)
			{
				this.AddDataToNode(node, data);
				return;
			}
			if (!node.HasChilds)
			{
				base.GrowTreeDown(node, data, depth + 1);
			}
			if (base.Intersects(node.Left, data))
			{
				this.AddInternal(node.Left, data, depth + 1);
				return;
			}
			this.AddInternal(node.Right, data, depth + 1);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00009335 File Offset: 0x00007535
		protected override Bounds GetBounds(ICullingTarget data)
		{
			return data.Bounds;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000933D File Offset: 0x0000753D
		protected override DC_SourcesTreeNode CreateNode(Vector3 center, Vector3 size, bool isLeaf)
		{
			return new DC_SourcesTreeNode(center, size, isLeaf);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00009347 File Offset: 0x00007547
		protected override void SetChildsToNode(DC_SourcesTreeNode parent, DC_SourcesTreeNode leftChild, DC_SourcesTreeNode rightChild)
		{
			parent.SetChilds(leftChild, rightChild);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00009351 File Offset: 0x00007551
		protected override void AddDataToNode(DC_SourcesTreeNode node, ICullingTarget data)
		{
			node.AddCullingTarget(data);
			this._lastModifiedSource = node.Source;
		}

		// Token: 0x04000153 RID: 339
		private DC_Source _lastModifiedSource;
	}
}
