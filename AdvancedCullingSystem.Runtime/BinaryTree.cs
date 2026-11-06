using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.AdvancedCullingSystem
{
	// Token: 0x02000004 RID: 4
	public abstract class BinaryTree<TNode, TData> where TNode : BinaryTreeNode
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public BinaryTreeNode Root
		{
			get
			{
				return this.RootInternal;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020CD File Offset: 0x000002CD
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020D5 File Offset: 0x000002D5
		private protected TNode RootInternal { protected get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020DE File Offset: 0x000002DE
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000020E6 File Offset: 0x000002E6
		public int Height { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020EF File Offset: 0x000002EF
		// (set) Token: 0x06000009 RID: 9 RVA: 0x000020F7 File Offset: 0x000002F7
		public float CellSize { get; private set; }

		// Token: 0x0600000A RID: 10 RVA: 0x00002100 File Offset: 0x00000300
		public BinaryTree(float cellSize)
		{
			this.CellSize = Mathf.Max(cellSize, 0.1f);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002120 File Offset: 0x00000320
		public BinaryTree(IList<TData> datas, int maxDepth)
		{
			if (maxDepth <= 1)
			{
				throw new ArgumentException("Max depth should be greater than 1");
			}
			this._maxDepth = maxDepth;
			Vector3 vector = Vector3.one * float.MaxValue;
			Vector3 vector2 = Vector3.one * float.MinValue;
			foreach (TData tdata in datas)
			{
				Bounds bounds = this.GetBounds(tdata);
				Vector3 min = bounds.min;
				Vector3 max = bounds.max;
				vector.x = Mathf.Min(vector.x, min.x);
				vector.y = Mathf.Min(vector.y, min.y);
				vector.z = Mathf.Min(vector.z, min.z);
				vector2.x = Mathf.Max(vector2.x, max.x);
				vector2.y = Mathf.Max(vector2.y, max.y);
				vector2.z = Mathf.Max(vector2.z, max.z);
			}
			this.RootInternal = this.CreateNode(vector + (vector2 - vector) / 2f, vector2 - vector + Vector3.one * 0.01f, false);
			this.Height = 1;
			foreach (TData tdata2 in datas)
			{
				this.Add(tdata2);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022DC File Offset: 0x000004DC
		public void Add(TData data)
		{
			if (this.Root == null)
			{
				this.RootInternal = this.CreateNode(this.GetBounds(data).center, Vector3.one * this.CellSize, true);
				this.Height = 1;
			}
			if (!this.Includes(this.RootInternal, data))
			{
				this.GrowTreeUp(data);
			}
			this.AddInternal(this.RootInternal, data, 1);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002348 File Offset: 0x00000548
		private TNode ExpandRoot(TNode root, TData target)
		{
			Bounds bounds = root.Bounds;
			Bounds bounds2 = this.GetBounds(target);
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			Vector3 vector3 = Vector3.zero;
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				if (bounds2.min[i] < bounds.min[i])
				{
					vector2 = bounds.size;
					ref Vector3 ptr = ref vector2;
					int num = i;
					ptr[num] *= 2f;
					vector = bounds.center;
					ptr = ref vector;
					num = i;
					ptr[num] -= bounds.size[i] / 2f;
					vector3 = bounds.center;
					ptr = ref vector3;
					num = i;
					ptr[num] -= bounds.size[i];
					break;
				}
				if (bounds2.max[i] > bounds.max[i])
				{
					vector2 = bounds.size;
					ref Vector3 ptr = ref vector2;
					int num = i;
					ptr[num] *= 2f;
					vector = bounds.center;
					ptr = ref vector;
					num = i;
					ptr[num] += bounds.size[i] / 2f;
					vector3 = bounds.center;
					ptr = ref vector3;
					num = i;
					ptr[num] += bounds.size[i];
					flag = true;
					break;
				}
			}
			TNode tnode = this.CreateNode(vector, vector2, false);
			TNode tnode2 = this.CreateNode(vector3, bounds.size, root.IsLeaf);
			if (flag)
			{
				this.SetChildsToNode(tnode, this.RootInternal, tnode2);
			}
			else
			{
				this.SetChildsToNode(tnode, tnode2, this.RootInternal);
			}
			return tnode;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002574 File Offset: 0x00000774
		protected void GrowTreeUp(TData target)
		{
			if (this.Height > 42)
			{
				Debug.Log("Increasing the binary tree can lead to memory overflow and crashes. Please make sure you are not trying to add “infinite” objects such as Skybox, fog, or water.");
				return;
			}
			if (this.Includes(this.RootInternal, target))
			{
				return;
			}
			this.RootInternal = this.ExpandRoot(this.RootInternal, target);
			int height = this.Height;
			this.Height = height + 1;
			this.GrowTreeUp(target);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000025D0 File Offset: 0x000007D0
		protected void GrowTreeDown(TNode node, TData target, int depth)
		{
			if (node.HasChilds)
			{
				throw new Exception("GrowTreeDown::" + depth.ToString() + " node already has childs");
			}
			Bounds bounds = node.Bounds;
			Vector3 vector;
			Vector3 vector2;
			if (bounds.size.x >= bounds.size.y && bounds.size.x >= bounds.size.z)
			{
				vector = new Vector3(bounds.size.x / 4f, 0f, 0f);
				vector2 = new Vector3(bounds.size.x / 2f, bounds.size.y, bounds.size.z);
			}
			else if (bounds.size.y >= bounds.size.x && bounds.size.y >= bounds.size.z)
			{
				vector = new Vector3(0f, bounds.size.y / 4f, 0f);
				vector2 = new Vector3(bounds.size.x, bounds.size.y / 2f, bounds.size.z);
			}
			else
			{
				vector = new Vector3(0f, 0f, bounds.size.z / 4f);
				vector2 = new Vector3(bounds.size.x, bounds.size.y, bounds.size.z / 2f);
			}
			bool flag = depth == this._maxDepth || (vector2.x <= this.CellSize && vector2.y <= this.CellSize && vector2.z <= this.CellSize);
			TNode tnode = this.CreateNode(bounds.center - vector, vector2, flag);
			TNode tnode2 = this.CreateNode(bounds.center + vector, vector2, flag);
			this.SetChildsToNode(node, tnode, tnode2);
			if (flag)
			{
				if (this.Height < depth)
				{
					this.Height = depth;
				}
				if (this.CellSize == 0f)
				{
					this.CellSize = Mathf.Min(new float[] { vector2.x, vector2.y, vector2.z });
				}
				return;
			}
			if (this.Intersects(tnode, target))
			{
				this.GrowTreeDown(tnode, target, depth + 1);
			}
			if (this.Intersects(tnode2, target))
			{
				this.GrowTreeDown(tnode2, target, depth + 1);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002870 File Offset: 0x00000A70
		protected bool Intersects(TNode node, TData data)
		{
			return node.Bounds.Intersects(this.GetBounds(data));
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002897 File Offset: 0x00000A97
		protected bool Includes(TNode node, TData data)
		{
			return node.Bounds.Contains(this.GetBounds(data));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000028B0 File Offset: 0x00000AB0
		protected virtual void AddInternal(TNode node, TData data, int depth)
		{
			if (node.IsLeaf)
			{
				this.AddDataToNode(node, data);
				return;
			}
			if (!node.HasChilds)
			{
				this.GrowTreeDown(node, data, depth + 1);
			}
			TNode tnode = (TNode)((object)node.GetLeft());
			TNode tnode2 = (TNode)((object)node.GetRight());
			if (this.Intersects(tnode, data))
			{
				this.AddInternal(tnode, data, depth + 1);
			}
			if (this.Intersects(tnode2, data))
			{
				this.AddInternal(tnode2, data, depth + 1);
			}
		}

		// Token: 0x06000013 RID: 19
		protected abstract Bounds GetBounds(TData data);

		// Token: 0x06000014 RID: 20
		protected abstract TNode CreateNode(Vector3 center, Vector3 size, bool isLeaf);

		// Token: 0x06000015 RID: 21
		protected abstract void SetChildsToNode(TNode parent, TNode leftChild, TNode rightChild);

		// Token: 0x06000016 RID: 22
		protected abstract void AddDataToNode(TNode node, TData data);

		// Token: 0x04000006 RID: 6
		private const int MAX_HEIGHT = 42;

		// Token: 0x0400000A RID: 10
		private int _maxDepth = -1;
	}
}
