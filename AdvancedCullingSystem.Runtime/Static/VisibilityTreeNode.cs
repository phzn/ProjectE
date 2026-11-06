using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x0200002F RID: 47
	public class VisibilityTreeNode : BinaryTreeNode
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000066F5 File Offset: 0x000048F5
		public VisibilityTreeNode Left
		{
			get
			{
				return this._left;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000066FD File Offset: 0x000048FD
		public VisibilityTreeNode Right
		{
			get
			{
				return this._right;
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006705 File Offset: 0x00004905
		public VisibilityTreeNode(VisibilityTree tree, Vector3 center, Vector3 size, bool isLeaf)
			: base(center, size, isLeaf)
		{
			this._tree = tree;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00006718 File Offset: 0x00004918
		public override BinaryTreeNode GetLeft()
		{
			return this.Left;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006720 File Offset: 0x00004920
		public override BinaryTreeNode GetRight()
		{
			return this.Right;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00006728 File Offset: 0x00004928
		public void SetChilds(VisibilityTreeNode left, VisibilityTreeNode right)
		{
			this._left = left;
			this._right = right;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006738 File Offset: 0x00004938
		public void AddVisibleCullingTarget(int targetIndex)
		{
			if (this._uniqTargets == null)
			{
				this._uniqTargets = new HashSet<int>();
			}
			this._uniqTargets.Add(targetIndex);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000675C File Offset: 0x0000495C
		public void RemoveDuplicatesFromChilds()
		{
			if (!base.HasChilds)
			{
				return;
			}
			HashSet<int> uniqTargets = this.Left._uniqTargets;
			HashSet<int> uniqTargets2 = this.Right._uniqTargets;
			if (uniqTargets == null || uniqTargets2 == null)
			{
				return;
			}
			foreach (int num in uniqTargets)
			{
				if (uniqTargets2.Contains(num))
				{
					this.AddVisibleCullingTarget(num);
				}
			}
			if (this._uniqTargets != null)
			{
				foreach (int num2 in this._uniqTargets)
				{
					uniqTargets.Remove(num2);
					uniqTargets2.Remove(num2);
				}
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006834 File Offset: 0x00004A34
		public void ApplyData()
		{
			if (this._uniqTargets == null || this._uniqTargets.Count == 0)
			{
				return;
			}
			if (this._uniqTargets.Any<int>((int t) => t >= 65535))
			{
				this._visibilityData = new VisibilityData(this._uniqTargets);
				return;
			}
			this._visibilityData = new CompactVisibilityData(this._uniqTargets);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000068A8 File Offset: 0x00004AA8
		public void SetVisible()
		{
			if (this._visibilityData == null)
			{
				return;
			}
			try
			{
				this._visibilityData.SetVisible(this._tree.CullingTargets);
			}
			catch (MissingReferenceException)
			{
				Debug.Log("Looks like some of baked objects was destroyed. Rebake scene");
			}
		}

		// Token: 0x040000D1 RID: 209
		[SerializeReference]
		private VisibilityTree _tree;

		// Token: 0x040000D2 RID: 210
		[SerializeReference]
		private VisibilityTreeNode _left;

		// Token: 0x040000D3 RID: 211
		[SerializeReference]
		private VisibilityTreeNode _right;

		// Token: 0x040000D4 RID: 212
		[SerializeReference]
		private IVisibilityData _visibilityData;

		// Token: 0x040000D5 RID: 213
		private HashSet<int> _uniqTargets;
	}
}
