using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding.Collections
{
	// Token: 0x02000288 RID: 648
	public class GridLookup<T> where T : class
	{
		// Token: 0x0600103B RID: 4155 RVA: 0x000621BC File Offset: 0x000603BC
		public GridLookup(Vector2Int size)
		{
			this.size = size;
			this.cells = new GridLookup<T>.Item[size.x * size.y];
			for (int i = 0; i < this.cells.Length; i++)
			{
				this.cells[i] = new GridLookup<T>.Item();
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600103C RID: 4156 RVA: 0x00062231 File Offset: 0x00060431
		public GridLookup<T>.Root AllItems
		{
			get
			{
				return this.all.next;
			}
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x00062240 File Offset: 0x00060440
		public void Clear()
		{
			this.rootLookup.Clear();
			this.all.next = null;
			GridLookup<T>.Item[] array = this.cells;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].next = null;
			}
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x00062284 File Offset: 0x00060484
		public GridLookup<T>.Root GetRoot(T item)
		{
			GridLookup<T>.Root root;
			this.rootLookup.TryGetValue(item, out root);
			return root;
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x000622A4 File Offset: 0x000604A4
		public GridLookup<T>.Root Add(T item, IntRect bounds)
		{
			GridLookup<T>.Root root = new GridLookup<T>.Root
			{
				obj = item,
				prev = this.all,
				next = this.all.next
			};
			this.all.next = root;
			if (root.next != null)
			{
				root.next.prev = root;
			}
			this.rootLookup.Add(item, root);
			this.Move(item, bounds);
			return root;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x00062314 File Offset: 0x00060514
		public void Remove(T item)
		{
			GridLookup<T>.Root root;
			if (!this.rootLookup.TryGetValue(item, out root))
			{
				return;
			}
			this.Move(item, new IntRect(0, 0, -1, -1));
			this.rootLookup.Remove(item);
			root.prev.next = root.next;
			if (root.next != null)
			{
				root.next.prev = root.prev;
			}
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0006237C File Offset: 0x0006057C
		public void Dirty(T item)
		{
			GridLookup<T>.Root root;
			if (!this.rootLookup.TryGetValue(item, out root))
			{
				return;
			}
			root.previousPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x000623B4 File Offset: 0x000605B4
		public void Move(T item, IntRect bounds)
		{
			GridLookup<T>.Root root;
			if (!this.rootLookup.TryGetValue(item, out root))
			{
				throw new ArgumentException("The item has not been added to this object");
			}
			if (root.previousBounds == bounds)
			{
				return;
			}
			for (int i = 0; i < root.items.Count; i++)
			{
				GridLookup<T>.Item item2 = root.items[i];
				item2.prev.next = item2.next;
				if (item2.next != null)
				{
					item2.next.prev = item2.prev;
				}
			}
			root.previousBounds = bounds;
			int num = 0;
			for (int j = bounds.ymin; j <= bounds.ymax; j++)
			{
				for (int k = bounds.xmin; k <= bounds.xmax; k++)
				{
					GridLookup<T>.Item item3;
					if (num < root.items.Count)
					{
						item3 = root.items[num];
					}
					else
					{
						item3 = ((this.itemPool.Count > 0) ? this.itemPool.Pop() : new GridLookup<T>.Item());
						item3.root = root;
						root.items.Add(item3);
					}
					num++;
					item3.prev = this.cells[k + j * this.size.x];
					item3.next = item3.prev.next;
					item3.prev.next = item3;
					if (item3.next != null)
					{
						item3.next.prev = item3;
					}
				}
			}
			for (int l = root.items.Count - 1; l >= num; l--)
			{
				GridLookup<T>.Item item4 = root.items[l];
				item4.root = null;
				item4.next = null;
				item4.prev = null;
				root.items.RemoveAt(l);
				this.itemPool.Push(item4);
			}
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00062590 File Offset: 0x00060790
		public List<U> QueryRect<U>(IntRect r) where U : class, T
		{
			List<U> list = ListPool<U>.Claim();
			for (int i = r.ymin; i <= r.ymax; i++)
			{
				int num = i * this.size.x;
				for (int j = r.xmin; j <= r.xmax; j++)
				{
					GridLookup<T>.Item item = this.cells[j + num];
					while (item.next != null)
					{
						item = item.next;
						U u = item.root.obj as U;
						if (!item.root.flag && u != null)
						{
							item.root.flag = true;
							list.Add(u);
						}
					}
				}
			}
			for (int k = r.ymin; k <= r.ymax; k++)
			{
				int num2 = k * this.size.x;
				for (int l = r.xmin; l <= r.xmax; l++)
				{
					GridLookup<T>.Item item2 = this.cells[l + num2];
					while (item2.next != null)
					{
						item2 = item2.next;
						item2.root.flag = false;
					}
				}
			}
			return list;
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x000626C0 File Offset: 0x000608C0
		public void Resize(IntRect newBounds)
		{
			GridLookup<T>.Item[] array = new GridLookup<T>.Item[newBounds.Width * newBounds.Height];
			for (int i = 0; i < this.size.y; i++)
			{
				for (int j = 0; j < this.size.x; j++)
				{
					if (newBounds.Contains(j, i))
					{
						array[j - newBounds.xmin + (i - newBounds.ymin) * newBounds.Width] = this.cells[j + i * this.size.x];
					}
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k] == null)
				{
					array[k] = new GridLookup<T>.Item();
				}
			}
			this.size = new Vector2Int(newBounds.Width, newBounds.Height);
			this.cells = array;
			GridLookup<T>.Root root = this.AllItems;
			Vector2Int vector2Int = new Vector2Int(-newBounds.xmin, -newBounds.ymin);
			IntRect intRect = new IntRect(0, 0, newBounds.Width - 1, newBounds.Height - 1);
			while (root != null)
			{
				root.previousBounds = IntRect.Intersection(root.previousBounds.Offset(vector2Int), intRect);
				root = root.next;
			}
		}

		// Token: 0x04000BD9 RID: 3033
		private Vector2Int size;

		// Token: 0x04000BDA RID: 3034
		private GridLookup<T>.Item[] cells;

		// Token: 0x04000BDB RID: 3035
		private GridLookup<T>.Root all = new GridLookup<T>.Root();

		// Token: 0x04000BDC RID: 3036
		private Dictionary<T, GridLookup<T>.Root> rootLookup = new Dictionary<T, GridLookup<T>.Root>();

		// Token: 0x04000BDD RID: 3037
		private Stack<GridLookup<T>.Item> itemPool = new Stack<GridLookup<T>.Item>();

		// Token: 0x02000289 RID: 649
		internal class Item
		{
			// Token: 0x04000BDE RID: 3038
			public GridLookup<T>.Root root;

			// Token: 0x04000BDF RID: 3039
			public GridLookup<T>.Item prev;

			// Token: 0x04000BE0 RID: 3040
			public GridLookup<T>.Item next;
		}

		// Token: 0x0200028A RID: 650
		public class Root
		{
			// Token: 0x04000BE1 RID: 3041
			public T obj;

			// Token: 0x04000BE2 RID: 3042
			public GridLookup<T>.Root next;

			// Token: 0x04000BE3 RID: 3043
			internal GridLookup<T>.Root prev;

			// Token: 0x04000BE4 RID: 3044
			internal IntRect previousBounds = new IntRect(0, 0, -1, -1);

			// Token: 0x04000BE5 RID: 3045
			internal List<GridLookup<T>.Item> items = new List<GridLookup<T>.Item>();

			// Token: 0x04000BE6 RID: 3046
			internal bool flag;

			// Token: 0x04000BE7 RID: 3047
			public Vector3 previousPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

			// Token: 0x04000BE8 RID: 3048
			public Quaternion previousRotation;
		}
	}
}
