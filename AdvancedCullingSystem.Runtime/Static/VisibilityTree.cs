using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x0200002E RID: 46
	public class VisibilityTree : BinaryTree<VisibilityTreeNode, Vector3>
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000064EF File Offset: 0x000046EF
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000064F7 File Offset: 0x000046F7
		public CullingTarget[] CullingTargets { get; private set; }

		// Token: 0x06000107 RID: 263 RVA: 0x00006500 File Offset: 0x00004700
		public VisibilityTree(float cellSize)
			: base(cellSize)
		{
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006509 File Offset: 0x00004709
		public void SetTargets(CullingTarget[] targets)
		{
			this.CullingTargets = targets;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006512 File Offset: 0x00004712
		public void Optimize()
		{
			this.Optimize(base.RootInternal);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006520 File Offset: 0x00004720
		public void Apply()
		{
			this.ApplyData(base.RootInternal);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006530 File Offset: 0x00004730
		public void SetVisible(Vector3 point, Vector2 tolerance)
		{
			Bounds bounds = new Bounds(point, new Vector3(tolerance.x, tolerance.y, tolerance.x));
			this.SetVisibleInternal(base.RootInternal, bounds);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000656C File Offset: 0x0000476C
		public void DrawCellsGizmo(Vector3 point, Vector2 tolerance)
		{
			Bounds bounds = new Bounds(point, new Vector3(tolerance.x, tolerance.y, tolerance.x));
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(bounds.center, bounds.size);
			this.DrawCellsGizmoInternal(base.RootInternal, bounds);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000065C4 File Offset: 0x000047C4
		private void SetVisibleInternal(VisibilityTreeNode node, Bounds cameraBounds)
		{
			if (!node.Bounds.Intersects(cameraBounds))
			{
				return;
			}
			node.SetVisible();
			if (node.HasChilds)
			{
				this.SetVisibleInternal(node.Left, cameraBounds);
				this.SetVisibleInternal(node.Right, cameraBounds);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000660B File Offset: 0x0000480B
		private void Optimize(VisibilityTreeNode current)
		{
			if (current.IsLeaf)
			{
				return;
			}
			if (current.HasChilds)
			{
				this.Optimize(current.Left);
				this.Optimize(current.Right);
			}
			current.RemoveDuplicatesFromChilds();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000663C File Offset: 0x0000483C
		private void ApplyData(VisibilityTreeNode current)
		{
			current.ApplyData();
			if (current.HasChilds)
			{
				this.ApplyData(current.Left);
				this.ApplyData(current.Right);
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006664 File Offset: 0x00004864
		private void DrawCellsGizmoInternal(VisibilityTreeNode node, Bounds cameraBounds)
		{
			if (!node.Bounds.Intersects(cameraBounds))
			{
				return;
			}
			if (node.IsLeaf)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireCube(node.Center, node.Size);
				return;
			}
			if (node.HasChilds)
			{
				this.DrawCellsGizmoInternal(node.Left, cameraBounds);
				this.DrawCellsGizmoInternal(node.Right, cameraBounds);
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000066C9 File Offset: 0x000048C9
		protected override Bounds GetBounds(Vector3 point)
		{
			return new Bounds(point, Vector3.one * 0.1f);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000066E0 File Offset: 0x000048E0
		protected override VisibilityTreeNode CreateNode(Vector3 center, Vector3 size, bool isLeaf)
		{
			return new VisibilityTreeNode(this, center, size, isLeaf);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000066EB File Offset: 0x000048EB
		protected override void SetChildsToNode(VisibilityTreeNode parent, VisibilityTreeNode leftChild, VisibilityTreeNode rightChild)
		{
			parent.SetChilds(leftChild, rightChild);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005C79 File Offset: 0x00003E79
		protected override void AddDataToNode(VisibilityTreeNode node, Vector3 point)
		{
		}
	}
}
