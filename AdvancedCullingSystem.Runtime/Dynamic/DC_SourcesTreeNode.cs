using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000055 RID: 85
	public class DC_SourcesTreeNode : BinaryTreeNode
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00009366 File Offset: 0x00007566
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000936E File Offset: 0x0000756E
		public DC_SourcesTreeNode Left { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00009377 File Offset: 0x00007577
		// (set) Token: 0x06000216 RID: 534 RVA: 0x0000937F File Offset: 0x0000757F
		public DC_SourcesTreeNode Right { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00009388 File Offset: 0x00007588
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00009390 File Offset: 0x00007590
		public DC_MultiSource Source { get; private set; }

		// Token: 0x06000219 RID: 537 RVA: 0x00009399 File Offset: 0x00007599
		public DC_SourcesTreeNode(Vector3 center, Vector3 size, bool isLeaf)
			: base(center, size, isLeaf)
		{
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000093A4 File Offset: 0x000075A4
		public override BinaryTreeNode GetLeft()
		{
			return this.Left;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000093AC File Offset: 0x000075AC
		public override BinaryTreeNode GetRight()
		{
			return this.Right;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000093B4 File Offset: 0x000075B4
		public void SetChilds(DC_SourcesTreeNode left, DC_SourcesTreeNode right)
		{
			this.Left = left;
			this.Right = right;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000093C4 File Offset: 0x000075C4
		public void AddCullingTarget(ICullingTarget cullingTarget)
		{
			if (this.Source == null)
			{
				this.Source = new GameObject("DC_MultiSource").AddComponent<DC_MultiSource>();
			}
			this.Source.SetCullingTarget(cullingTarget);
		}
	}
}
