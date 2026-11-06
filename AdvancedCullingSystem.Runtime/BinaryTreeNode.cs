using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem
{
	// Token: 0x02000006 RID: 6
	public abstract class BinaryTreeNode
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002996 File Offset: 0x00000B96
		public Vector3 Center
		{
			get
			{
				return this._bounds.center;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000029A3 File Offset: 0x00000BA3
		public Vector3 Size
		{
			get
			{
				return this._bounds.size;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000029B0 File Offset: 0x00000BB0
		public Bounds Bounds
		{
			get
			{
				return this._bounds;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000029B8 File Offset: 0x00000BB8
		public bool HasChilds
		{
			get
			{
				return this.GetLeft() != null;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000029C3 File Offset: 0x00000BC3
		public bool IsLeaf
		{
			get
			{
				return this._isLeaf;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000029CB File Offset: 0x00000BCB
		public BinaryTreeNode(Vector3 center, Vector3 size, bool isLeaf)
		{
			this._bounds = new Bounds(center, size);
			this._isLeaf = isLeaf;
		}

		// Token: 0x06000021 RID: 33
		public abstract BinaryTreeNode GetLeft();

		// Token: 0x06000022 RID: 34
		public abstract BinaryTreeNode GetRight();

		// Token: 0x0400000C RID: 12
		[SerializeField]
		private Bounds _bounds;

		// Token: 0x0400000D RID: 13
		[SerializeField]
		private bool _isLeaf;
	}
}
