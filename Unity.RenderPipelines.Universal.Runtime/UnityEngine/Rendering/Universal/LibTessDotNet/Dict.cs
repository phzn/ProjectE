using System;

namespace UnityEngine.Rendering.Universal.LibTessDotNet
{
	// Token: 0x02000108 RID: 264
	internal class Dict<TValue> where TValue : class
	{
		// Token: 0x0600084B RID: 2123 RVA: 0x000345CC File Offset: 0x000327CC
		public Dict(Dict<TValue>.LessOrEqual leq)
		{
			this._leq = leq;
			this._head = new Dict<TValue>.Node
			{
				_key = default(TValue)
			};
			this._head._prev = this._head;
			this._head._next = this._head;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0003461F File Offset: 0x0003281F
		public Dict<TValue>.Node Insert(TValue key)
		{
			return this.InsertBefore(this._head, key);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00034630 File Offset: 0x00032830
		public Dict<TValue>.Node InsertBefore(Dict<TValue>.Node node, TValue key)
		{
			do
			{
				node = node._prev;
			}
			while (node._key != null && !this._leq(node._key, key));
			Dict<TValue>.Node node2 = new Dict<TValue>.Node
			{
				_key = key
			};
			node2._next = node._next;
			node._next._prev = node2;
			node2._prev = node;
			node._next = node2;
			return node2;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0003469C File Offset: 0x0003289C
		public Dict<TValue>.Node Find(TValue key)
		{
			Dict<TValue>.Node node = this._head;
			do
			{
				node = node._next;
			}
			while (node._key != null && !this._leq(key, node._key));
			return node;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x000346D9 File Offset: 0x000328D9
		public Dict<TValue>.Node Min()
		{
			return this._head._next;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x000346E6 File Offset: 0x000328E6
		public void Remove(Dict<TValue>.Node node)
		{
			node._next._prev = node._prev;
			node._prev._next = node._next;
		}

		// Token: 0x040007B4 RID: 1972
		private Dict<TValue>.LessOrEqual _leq;

		// Token: 0x040007B5 RID: 1973
		private Dict<TValue>.Node _head;

		// Token: 0x020001CB RID: 459
		public class Node
		{
			// Token: 0x1700025F RID: 607
			// (get) Token: 0x06000B4B RID: 2891 RVA: 0x0004BC28 File Offset: 0x00049E28
			public TValue Key
			{
				get
				{
					return this._key;
				}
			}

			// Token: 0x17000260 RID: 608
			// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0004BC30 File Offset: 0x00049E30
			public Dict<TValue>.Node Prev
			{
				get
				{
					return this._prev;
				}
			}

			// Token: 0x17000261 RID: 609
			// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0004BC38 File Offset: 0x00049E38
			public Dict<TValue>.Node Next
			{
				get
				{
					return this._next;
				}
			}

			// Token: 0x04000BE4 RID: 3044
			internal TValue _key;

			// Token: 0x04000BE5 RID: 3045
			internal Dict<TValue>.Node _prev;

			// Token: 0x04000BE6 RID: 3046
			internal Dict<TValue>.Node _next;
		}

		// Token: 0x020001CC RID: 460
		// (Invoke) Token: 0x06000B50 RID: 2896
		public delegate bool LessOrEqual(TValue lhs, TValue rhs);
	}
}
