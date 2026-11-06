using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001BE RID: 446
	public class Quadtree<T>
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x0005BB70 File Offset: 0x00059D70
		// (set) Token: 0x06000D43 RID: 3395 RVA: 0x0005BB78 File Offset: 0x00059D78
		public int Count { get; private set; }

		// Token: 0x06000D44 RID: 3396 RVA: 0x0005BB81 File Offset: 0x00059D81
		public Quadtree(Rect boundaries, int nodeCapacity = 32)
		{
			this.boundaries = boundaries;
			this.nodeCapacity = nodeCapacity;
			this.nodes = new List<Quadtree<T>.QuadtreeNode>(nodeCapacity);
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0005BBAC File Offset: 0x00059DAC
		public bool Insert(float x, float y, T value)
		{
			Quadtree<T>.QuadtreeNode quadtreeNode = new Quadtree<T>.QuadtreeNode(new Vector2(x, y), value);
			return this.Insert(quadtreeNode);
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0005BBD0 File Offset: 0x00059DD0
		public bool Insert(Vector2 position, T value)
		{
			Quadtree<T>.QuadtreeNode quadtreeNode = new Quadtree<T>.QuadtreeNode(position, value);
			return this.Insert(quadtreeNode);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0005BBEC File Offset: 0x00059DEC
		private bool Insert(Quadtree<T>.QuadtreeNode node)
		{
			if (!this.boundaries.Contains(node.Position))
			{
				return false;
			}
			if (this.children != null)
			{
				Quadtree<T> quadtree;
				if (node.Position.y < this.children[2].boundaries.yMin)
				{
					if (node.Position.x < this.children[1].boundaries.xMin)
					{
						quadtree = this.children[0];
					}
					else
					{
						quadtree = this.children[1];
					}
				}
				else if (node.Position.x < this.children[1].boundaries.xMin)
				{
					quadtree = this.children[2];
				}
				else
				{
					quadtree = this.children[3];
				}
				if (quadtree.Insert(node))
				{
					int num = this.Count;
					this.Count = num + 1;
					return true;
				}
			}
			if (this.nodes.Count < this.nodeCapacity)
			{
				this.nodes.Add(node);
				int num = this.Count;
				this.Count = num + 1;
				return true;
			}
			this.Subdivide();
			return this.Insert(node);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0005BCF9 File Offset: 0x00059EF9
		public IEnumerable<T> Find(Rect range)
		{
			if (this.Count == 0)
			{
				yield break;
			}
			if (!this.boundaries.Overlaps(range, false))
			{
				yield break;
			}
			if (this.children == null)
			{
				int num;
				for (int index = 0; index < this.nodes.Count; index = num + 1)
				{
					Quadtree<T>.QuadtreeNode quadtreeNode = this.nodes[index];
					if (range.Contains(quadtreeNode.Position))
					{
						yield return quadtreeNode.Value;
					}
					num = index;
				}
			}
			else
			{
				int num;
				for (int index = 0; index < this.children.Length; index = num + 1)
				{
					Quadtree<T> quadtree = this.children[index];
					foreach (T t in quadtree.Find(range))
					{
						yield return t;
					}
					IEnumerator<T> enumerator = null;
					num = index;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0005BD10 File Offset: 0x00059F10
		public bool Remove(float x, float z, T value)
		{
			return this.Remove(new Vector2(x, z), value);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0005BD20 File Offset: 0x00059F20
		public bool Remove(Vector2 position, T value)
		{
			if (this.Count == 0)
			{
				return false;
			}
			if (!this.boundaries.Contains(position))
			{
				return false;
			}
			if (this.children != null)
			{
				bool flag = false;
				Quadtree<T> quadtree;
				if (position.y < this.children[2].boundaries.yMin)
				{
					if (position.x < this.children[1].boundaries.xMin)
					{
						quadtree = this.children[0];
					}
					else
					{
						quadtree = this.children[1];
					}
				}
				else if (position.x < this.children[1].boundaries.xMin)
				{
					quadtree = this.children[2];
				}
				else
				{
					quadtree = this.children[3];
				}
				if (quadtree.Remove(position, value))
				{
					flag = true;
					int num = this.Count;
					this.Count = num - 1;
				}
				if (this.Count <= this.nodeCapacity)
				{
					this.Combine();
				}
				return flag;
			}
			for (int i = 0; i < this.nodes.Count; i++)
			{
				if (this.nodes[i].Position.Equals(position))
				{
					this.nodes.RemoveAt(i);
					int num = this.Count;
					this.Count = num - 1;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0005BE50 File Offset: 0x0005A050
		private void Subdivide()
		{
			this.children = new Quadtree<T>[4];
			float num = this.boundaries.width * 0.5f;
			float num2 = this.boundaries.height * 0.5f;
			for (int i = 0; i < this.children.Length; i++)
			{
				Rect rect = new Rect(this.boundaries.xMin + num * (float)(i % 2), this.boundaries.yMin + num2 * (float)(i / 2), num, num2);
				this.children[i] = new Quadtree<T>(rect, 32);
			}
			this.Count = 0;
			for (int j = 0; j < this.nodes.Count; j++)
			{
				Quadtree<T>.QuadtreeNode quadtreeNode = this.nodes[j];
				this.Insert(quadtreeNode);
			}
			this.nodes.Clear();
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0005BF24 File Offset: 0x0005A124
		private void Combine()
		{
			for (int i = 0; i < this.children.Length; i++)
			{
				Quadtree<T> quadtree = this.children[i];
				this.nodes.AddRange(quadtree.nodes);
			}
			this.children = null;
		}

		// Token: 0x04000D97 RID: 3479
		private readonly int nodeCapacity = 32;

		// Token: 0x04000D98 RID: 3480
		private readonly List<Quadtree<T>.QuadtreeNode> nodes;

		// Token: 0x04000D99 RID: 3481
		private Quadtree<T>[] children;

		// Token: 0x04000D9A RID: 3482
		private Rect boundaries;

		// Token: 0x020001BF RID: 447
		private class QuadtreeNode
		{
			// Token: 0x17000258 RID: 600
			// (get) Token: 0x06000D4D RID: 3405 RVA: 0x0005BF65 File Offset: 0x0005A165
			// (set) Token: 0x06000D4E RID: 3406 RVA: 0x0005BF6D File Offset: 0x0005A16D
			public Vector2 Position { get; private set; }

			// Token: 0x17000259 RID: 601
			// (get) Token: 0x06000D4F RID: 3407 RVA: 0x0005BF76 File Offset: 0x0005A176
			// (set) Token: 0x06000D50 RID: 3408 RVA: 0x0005BF7E File Offset: 0x0005A17E
			public T Value { get; private set; }

			// Token: 0x06000D51 RID: 3409 RVA: 0x0005BF87 File Offset: 0x0005A187
			public QuadtreeNode(Vector2 position, T value)
			{
				this.Position = position;
				this.Value = value;
			}
		}
	}
}
