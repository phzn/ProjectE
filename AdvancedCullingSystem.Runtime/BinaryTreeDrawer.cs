using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem
{
	// Token: 0x02000005 RID: 5
	public class BinaryTreeDrawer
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002937 File Offset: 0x00000B37
		// (set) Token: 0x06000018 RID: 24 RVA: 0x0000293F File Offset: 0x00000B3F
		public Color Color { get; set; }

		// Token: 0x06000019 RID: 25 RVA: 0x00002948 File Offset: 0x00000B48
		public void DrawTreeGizmos(BinaryTreeNode root)
		{
			if (root == null)
			{
				return;
			}
			Bounds bounds = root.Bounds;
			Gizmos.color = this.Color;
			Gizmos.DrawWireCube(bounds.center, bounds.size);
			this.DrawTreeGizmos(root.GetLeft());
			this.DrawTreeGizmos(root.GetRight());
		}
	}
}
