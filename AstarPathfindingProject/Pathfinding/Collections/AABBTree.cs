using System;
using System.Collections.Generic;
using System.Diagnostics;
using Pathfinding.Util;
using Unity.Collections;
using UnityEngine;

namespace Pathfinding.Collections
{
	// Token: 0x02000277 RID: 631
	public class AABBTree<T>
	{
		// Token: 0x06000FDF RID: 4063 RVA: 0x000602A0 File Offset: 0x0005E4A0
		private static float ExpansionRequired(Bounds b, Bounds b2)
		{
			Bounds bounds = b;
			bounds.Encapsulate(b2);
			return bounds.size.x * bounds.size.y * bounds.size.z - b.size.x * b.size.y * b.size.z;
		}

		// Token: 0x17000253 RID: 595
		public T this[AABBTree<T>.Key key]
		{
			get
			{
				return this.nodes[key.node].value;
			}
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00060320 File Offset: 0x0005E520
		public Bounds GetBounds(AABBTree<T>.Key key)
		{
			if (!key.isValid)
			{
				throw new ArgumentException("Key is not valid");
			}
			AABBTree<T>.Node node = this.nodes[key.node];
			if (!node.isAllocated)
			{
				throw new ArgumentException("Key does not point to an allocated node");
			}
			if (!node.isLeaf)
			{
				throw new ArgumentException("Key does not point to a leaf node");
			}
			return node.bounds;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00060384 File Offset: 0x0005E584
		private int AllocNode()
		{
			int num;
			if (!this.freeNodes.TryPop(out num))
			{
				int num2 = this.nodes.Length;
				Memory.Realloc<AABBTree<T>.Node>(ref this.nodes, Mathf.Max(8, this.nodes.Length * 2));
				for (int i = this.nodes.Length - 1; i >= num2; i--)
				{
					this.FreeNode(i);
				}
				num = this.freeNodes.Pop();
			}
			return num;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x000603ED File Offset: 0x0005E5ED
		private void FreeNode(int node)
		{
			this.nodes[node].isAllocated = false;
			this.nodes[node].value = default(T);
			this.freeNodes.Push(node);
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x00060424 File Offset: 0x0005E624
		public unsafe void Rebuild()
		{
			UnsafeSpan<int> unsafeSpan = new UnsafeSpan<int>(Allocator.Temp, this.nodes.Length);
			int num = 0;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i].isAllocated)
				{
					if (this.nodes[i].isLeaf)
					{
						*unsafeSpan[num++] = i;
					}
					else
					{
						this.FreeNode(i);
					}
				}
			}
			this.root = this.Rebuild(unsafeSpan.Slice(0, num), 536870911);
			this.rebuildCounter = Mathf.Max(64, num / 3);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x000604C0 File Offset: 0x0005E6C0
		public void Clear()
		{
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i].isAllocated)
				{
					this.FreeNode(i);
				}
			}
			this.root = -1;
			this.rebuildCounter = 64;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0006050C File Offset: 0x0005E70C
		private static int ArgMax(Vector3 v)
		{
			float num = Mathf.Max(v.x, Mathf.Max(v.y, v.z));
			if (num == v.x)
			{
				return 0;
			}
			if (num != v.y)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00060550 File Offset: 0x0005E750
		private unsafe int Rebuild(UnsafeSpan<int> leaves, int parent)
		{
			if (leaves.Length == 0)
			{
				return -1;
			}
			if (leaves.Length == 1)
			{
				this.nodes[*leaves[0]].parent = parent;
				return *leaves[0];
			}
			Bounds bounds = this.nodes[*leaves[0]].bounds;
			for (int i = 1; i < leaves.Length; i++)
			{
				bounds.Encapsulate(this.nodes[*leaves[i]].bounds);
			}
			leaves.Sort(new AABBTree<T>.AABBComparer
			{
				nodes = this.nodes,
				dim = AABBTree<T>.ArgMax(bounds.extents)
			});
			int num = this.AllocNode();
			this.nodes[num] = new AABBTree<T>.Node
			{
				bounds = bounds,
				left = this.Rebuild(leaves.Slice(0, leaves.Length / 2), num),
				right = this.Rebuild(leaves.Slice(leaves.Length / 2), num),
				parent = parent,
				isAllocated = true
			};
			return num;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x00060684 File Offset: 0x0005E884
		public void Move(AABBTree<T>.Key key, Bounds bounds)
		{
			T value = this.nodes[key.node].value;
			this.Remove(key);
			this.Add(bounds, value);
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x000606BC File Offset: 0x0005E8BC
		[Conditional("VALIDATE_AABB_TREE")]
		private void Validate(int node)
		{
			if (node == -1)
			{
				return;
			}
			AABBTree<T>.Node node2 = this.nodes[node];
			int num = this.root;
			bool isLeaf = node2.isLeaf;
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x000606EC File Offset: 0x0005E8EC
		public Bounds Remove(AABBTree<T>.Key key)
		{
			if (!key.isValid)
			{
				throw new ArgumentException("Key is not valid");
			}
			AABBTree<T>.Node node = this.nodes[key.node];
			if (!node.isAllocated)
			{
				throw new ArgumentException("Key does not point to an allocated node");
			}
			if (!node.isLeaf)
			{
				throw new ArgumentException("Key does not point to a leaf node");
			}
			if (key.node == this.root)
			{
				this.root = -1;
				this.FreeNode(key.node);
				return node.bounds;
			}
			int parent = node.parent;
			AABBTree<T>.Node node2 = this.nodes[parent];
			int num = ((node2.left == key.node) ? node2.right : node2.left);
			this.FreeNode(parent);
			this.FreeNode(key.node);
			this.nodes[num].parent = node2.parent;
			if (node2.parent == 536870911)
			{
				this.root = num;
			}
			else if (this.nodes[node2.parent].left == parent)
			{
				this.nodes[node2.parent].left = num;
			}
			else
			{
				this.nodes[node2.parent].right = num;
			}
			ref AABBTree<T>.Node ptr;
			for (int num2 = this.nodes[num].parent; num2 != 536870911; num2 = ptr.parent)
			{
				ptr = ref this.nodes[num2];
				Bounds bounds = this.nodes[ptr.left].bounds;
				bounds.Encapsulate(this.nodes[ptr.right].bounds);
				ptr.bounds = bounds;
				ptr.subtreePartiallyTagged = this.nodes[ptr.left].subtreePartiallyTagged | this.nodes[ptr.right].subtreePartiallyTagged;
			}
			return node.bounds;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x000608EC File Offset: 0x0005EAEC
		public AABBTree<T>.Key Add(Bounds bounds, T value)
		{
			int num = this.AllocNode();
			this.nodes[num] = new AABBTree<T>.Node
			{
				bounds = bounds,
				parent = 536870911,
				left = -1,
				right = -1,
				value = value,
				isAllocated = true
			};
			if (this.root == -1)
			{
				this.root = num;
				return new AABBTree<T>.Key(num);
			}
			int num2 = this.root;
			AABBTree<T>.Node node;
			for (;;)
			{
				node = this.nodes[num2];
				this.nodes[num2].wholeSubtreeTagged = false;
				if (node.isLeaf)
				{
					break;
				}
				this.nodes[num2].bounds.Encapsulate(bounds);
				float num3 = AABBTree<T>.ExpansionRequired(this.nodes[node.left].bounds, bounds);
				float num4 = AABBTree<T>.ExpansionRequired(this.nodes[node.right].bounds, bounds);
				num2 = ((num3 < num4) ? node.left : node.right);
			}
			int num5 = this.AllocNode();
			if (node.parent != 536870911)
			{
				if (this.nodes[node.parent].left == num2)
				{
					this.nodes[node.parent].left = num5;
				}
				else
				{
					this.nodes[node.parent].right = num5;
				}
			}
			bounds.Encapsulate(node.bounds);
			this.nodes[num5] = new AABBTree<T>.Node
			{
				bounds = bounds,
				left = num2,
				right = num,
				parent = node.parent,
				isAllocated = true
			};
			this.nodes[num].parent = (this.nodes[num2].parent = num5);
			if (this.root == num2)
			{
				this.root = num5;
			}
			int num6 = this.rebuildCounter;
			this.rebuildCounter = num6 - 1;
			if (num6 <= 0)
			{
				this.Rebuild();
			}
			return new AABBTree<T>.Key(num);
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00060B0D File Offset: 0x0005ED0D
		public void Query(Bounds bounds, List<T> buffer)
		{
			this.QueryNode(this.root, bounds, buffer);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00060B20 File Offset: 0x0005ED20
		private void QueryNode(int node, Bounds bounds, List<T> buffer)
		{
			if (node == -1 || !bounds.Intersects(this.nodes[node].bounds))
			{
				return;
			}
			if (this.nodes[node].isLeaf)
			{
				buffer.Add(this.nodes[node].value);
				return;
			}
			this.QueryNode(this.nodes[node].left, bounds, buffer);
			this.QueryNode(this.nodes[node].right, bounds, buffer);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00060BA9 File Offset: 0x0005EDA9
		public void QueryTagged(List<T> buffer, bool clearTags = false)
		{
			this.QueryTaggedNode(this.root, clearTags, buffer);
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00060BBC File Offset: 0x0005EDBC
		private void QueryTaggedNode(int node, bool clearTags, List<T> buffer)
		{
			if (node == -1 || !this.nodes[node].subtreePartiallyTagged)
			{
				return;
			}
			if (clearTags)
			{
				this.nodes[node].wholeSubtreeTagged = false;
				this.nodes[node].subtreePartiallyTagged = false;
			}
			if (this.nodes[node].isLeaf)
			{
				buffer.Add(this.nodes[node].value);
				return;
			}
			this.QueryTaggedNode(this.nodes[node].left, clearTags, buffer);
			this.QueryTaggedNode(this.nodes[node].right, clearTags, buffer);
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00060C68 File Offset: 0x0005EE68
		public void Tag(AABBTree<T>.Key key)
		{
			if (!key.isValid)
			{
				throw new ArgumentException("Key is not valid");
			}
			if (key.node < 0 || key.node >= this.nodes.Length)
			{
				throw new ArgumentException("Key does not point to a valid node");
			}
			AABBTree<T>.Node[] array = this.nodes;
			int node = key.node;
			if (!array[node].isAllocated)
			{
				throw new ArgumentException("Key does not point to an allocated node");
			}
			if (!array[node].isLeaf)
			{
				throw new ArgumentException("Key does not point to a leaf node");
			}
			array[node].wholeSubtreeTagged = true;
			for (int num = key.node; num != 536870911; num = this.nodes[num].parent)
			{
				this.nodes[num].subtreePartiallyTagged = true;
			}
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00060D23 File Offset: 0x0005EF23
		public void Tag(Bounds bounds)
		{
			this.TagNode(this.root, bounds);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00060D34 File Offset: 0x0005EF34
		private bool TagNode(int node, Bounds bounds)
		{
			if (node == -1 || this.nodes[node].wholeSubtreeTagged)
			{
				return true;
			}
			if (!bounds.Intersects(this.nodes[node].bounds))
			{
				return false;
			}
			this.nodes[node].subtreePartiallyTagged = true;
			if (this.nodes[node].isLeaf)
			{
				return this.nodes[node].wholeSubtreeTagged = true;
			}
			return this.nodes[node].wholeSubtreeTagged = this.TagNode(this.nodes[node].left, bounds) & this.TagNode(this.nodes[node].right, bounds);
		}

		// Token: 0x04000B9E RID: 2974
		private AABBTree<T>.Node[] nodes = new AABBTree<T>.Node[0];

		// Token: 0x04000B9F RID: 2975
		private int root = -1;

		// Token: 0x04000BA0 RID: 2976
		private readonly Stack<int> freeNodes = new Stack<int>();

		// Token: 0x04000BA1 RID: 2977
		private int rebuildCounter = 64;

		// Token: 0x04000BA2 RID: 2978
		private const int NoNode = -1;

		// Token: 0x02000278 RID: 632
		private struct Node
		{
			// Token: 0x17000254 RID: 596
			// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x00060E24 File Offset: 0x0005F024
			// (set) Token: 0x06000FF5 RID: 4085 RVA: 0x00060E35 File Offset: 0x0005F035
			public bool wholeSubtreeTagged
			{
				get
				{
					return (this.flags & 1073741824U) > 0U;
				}
				set
				{
					this.flags = (this.flags & 3221225471U) | (value ? 1073741824U : 0U);
				}
			}

			// Token: 0x17000255 RID: 597
			// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x00060E55 File Offset: 0x0005F055
			// (set) Token: 0x06000FF7 RID: 4087 RVA: 0x00060E66 File Offset: 0x0005F066
			public bool subtreePartiallyTagged
			{
				get
				{
					return (this.flags & 2147483648U) > 0U;
				}
				set
				{
					this.flags = (this.flags & 2147483647U) | (value ? 2147483648U : 0U);
				}
			}

			// Token: 0x17000256 RID: 598
			// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x00060E86 File Offset: 0x0005F086
			// (set) Token: 0x06000FF9 RID: 4089 RVA: 0x00060E97 File Offset: 0x0005F097
			public bool isAllocated
			{
				get
				{
					return (this.flags & 536870912U) > 0U;
				}
				set
				{
					this.flags = (this.flags & 3758096383U) | (value ? 536870912U : 0U);
				}
			}

			// Token: 0x17000257 RID: 599
			// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00060EB7 File Offset: 0x0005F0B7
			public bool isLeaf
			{
				get
				{
					return this.left == -1;
				}
			}

			// Token: 0x17000258 RID: 600
			// (get) Token: 0x06000FFB RID: 4091 RVA: 0x00060EC2 File Offset: 0x0005F0C2
			// (set) Token: 0x06000FFC RID: 4092 RVA: 0x00060ED0 File Offset: 0x0005F0D0
			public int parent
			{
				get
				{
					return (int)(this.flags & 536870911U);
				}
				set
				{
					this.flags = (this.flags & 3758096384U) | (uint)value;
				}
			}

			// Token: 0x04000BA3 RID: 2979
			public Bounds bounds;

			// Token: 0x04000BA4 RID: 2980
			public uint flags;

			// Token: 0x04000BA5 RID: 2981
			private const uint TagInsideBit = 1073741824U;

			// Token: 0x04000BA6 RID: 2982
			private const uint TagPartiallyInsideBit = 2147483648U;

			// Token: 0x04000BA7 RID: 2983
			private const uint AllocatedBit = 536870912U;

			// Token: 0x04000BA8 RID: 2984
			private const uint ParentMask = 536870911U;

			// Token: 0x04000BA9 RID: 2985
			public const int InvalidParent = 536870911;

			// Token: 0x04000BAA RID: 2986
			public int left;

			// Token: 0x04000BAB RID: 2987
			public int right;

			// Token: 0x04000BAC RID: 2988
			public T value;
		}

		// Token: 0x02000279 RID: 633
		public readonly struct Key
		{
			// Token: 0x17000259 RID: 601
			// (get) Token: 0x06000FFD RID: 4093 RVA: 0x00060EE6 File Offset: 0x0005F0E6
			public int node
			{
				get
				{
					return this.value - 1;
				}
			}

			// Token: 0x1700025A RID: 602
			// (get) Token: 0x06000FFE RID: 4094 RVA: 0x00060EF0 File Offset: 0x0005F0F0
			public bool isValid
			{
				get
				{
					return this.value != 0;
				}
			}

			// Token: 0x06000FFF RID: 4095 RVA: 0x00060EFB File Offset: 0x0005F0FB
			internal Key(int node)
			{
				this.value = node + 1;
			}

			// Token: 0x04000BAD RID: 2989
			internal readonly int value;
		}

		// Token: 0x0200027A RID: 634
		private struct AABBComparer : IComparer<int>
		{
			// Token: 0x06001000 RID: 4096 RVA: 0x00060F08 File Offset: 0x0005F108
			public int Compare(int a, int b)
			{
				return this.nodes[a].bounds.center[this.dim].CompareTo(this.nodes[b].bounds.center[this.dim]);
			}

			// Token: 0x04000BAE RID: 2990
			public AABBTree<T>.Node[] nodes;

			// Token: 0x04000BAF RID: 2991
			public int dim;
		}
	}
}
