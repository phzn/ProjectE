using System;

namespace UnityEngine.Rendering.Universal.LibTessDotNet
{
	// Token: 0x0200010E RID: 270
	internal class PriorityHeap<TValue> where TValue : class
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x0003594D File Offset: 0x00033B4D
		public bool Empty
		{
			get
			{
				return this._size == 0;
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00035958 File Offset: 0x00033B58
		public PriorityHeap(int initialSize, PriorityHeap<TValue>.LessOrEqual leq)
		{
			this._leq = leq;
			this._nodes = new int[initialSize + 1];
			this._handles = new PriorityHeap<TValue>.HandleElem[initialSize + 1];
			this._size = 0;
			this._max = initialSize;
			this._freeList = 0;
			this._initialized = false;
			this._nodes[1] = 1;
			this._handles[1] = new PriorityHeap<TValue>.HandleElem
			{
				_key = default(TValue)
			};
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x000359CC File Offset: 0x00033BCC
		private void FloatDown(int curr)
		{
			int num = this._nodes[curr];
			for (;;)
			{
				int num2 = curr << 1;
				if (num2 < this._size && this._leq(this._handles[this._nodes[num2 + 1]]._key, this._handles[this._nodes[num2]]._key))
				{
					num2++;
				}
				int num3 = this._nodes[num2];
				if (num2 > this._size || this._leq(this._handles[num]._key, this._handles[num3]._key))
				{
					break;
				}
				this._nodes[curr] = num3;
				this._handles[num3]._node = curr;
				curr = num2;
			}
			this._nodes[curr] = num;
			this._handles[num]._node = curr;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00035A9C File Offset: 0x00033C9C
		private void FloatUp(int curr)
		{
			int num = this._nodes[curr];
			for (;;)
			{
				int num2 = curr >> 1;
				int num3 = this._nodes[num2];
				if (num2 == 0 || this._leq(this._handles[num3]._key, this._handles[num]._key))
				{
					break;
				}
				this._nodes[curr] = num3;
				this._handles[num3]._node = curr;
				curr = num2;
			}
			this._nodes[curr] = num;
			this._handles[num]._node = curr;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00035B1C File Offset: 0x00033D1C
		public void Init()
		{
			for (int i = this._size; i >= 1; i--)
			{
				this.FloatDown(i);
			}
			this._initialized = true;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00035B48 File Offset: 0x00033D48
		public PQHandle Insert(TValue value)
		{
			int num = this._size + 1;
			this._size = num;
			int num2 = num;
			if (num2 * 2 > this._max)
			{
				this._max <<= 1;
				Array.Resize<int>(ref this._nodes, this._max + 1);
				Array.Resize<PriorityHeap<TValue>.HandleElem>(ref this._handles, this._max + 1);
			}
			int num3;
			if (this._freeList == 0)
			{
				num3 = num2;
			}
			else
			{
				num3 = this._freeList;
				this._freeList = this._handles[num3]._node;
			}
			this._nodes[num2] = num3;
			if (this._handles[num3] == null)
			{
				this._handles[num3] = new PriorityHeap<TValue>.HandleElem
				{
					_key = value,
					_node = num2
				};
			}
			else
			{
				this._handles[num3]._node = num2;
				this._handles[num3]._key = value;
			}
			if (this._initialized)
			{
				this.FloatUp(num2);
			}
			return new PQHandle
			{
				_handle = num3
			};
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00035C38 File Offset: 0x00033E38
		public TValue ExtractMin()
		{
			int num = this._nodes[1];
			TValue key = this._handles[num]._key;
			if (this._size > 0)
			{
				this._nodes[1] = this._nodes[this._size];
				this._handles[this._nodes[1]]._node = 1;
				this._handles[num]._key = default(TValue);
				this._handles[num]._node = this._freeList;
				this._freeList = num;
				int num2 = this._size - 1;
				this._size = num2;
				if (num2 > 0)
				{
					this.FloatDown(1);
				}
			}
			return key;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00035CD6 File Offset: 0x00033ED6
		public TValue Minimum()
		{
			return this._handles[this._nodes[1]]._key;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00035CEC File Offset: 0x00033EEC
		public void Remove(PQHandle handle)
		{
			int handle2 = handle._handle;
			int node = this._handles[handle2]._node;
			this._nodes[node] = this._nodes[this._size];
			this._handles[this._nodes[node]]._node = node;
			int num = node;
			int num2 = this._size - 1;
			this._size = num2;
			if (num <= num2)
			{
				if (node <= 1 || this._leq(this._handles[this._nodes[node >> 1]]._key, this._handles[this._nodes[node]]._key))
				{
					this.FloatDown(node);
				}
				else
				{
					this.FloatUp(node);
				}
			}
			this._handles[handle2]._key = default(TValue);
			this._handles[handle2]._node = this._freeList;
			this._freeList = handle2;
		}

		// Token: 0x040007C1 RID: 1985
		private PriorityHeap<TValue>.LessOrEqual _leq;

		// Token: 0x040007C2 RID: 1986
		private int[] _nodes;

		// Token: 0x040007C3 RID: 1987
		private PriorityHeap<TValue>.HandleElem[] _handles;

		// Token: 0x040007C4 RID: 1988
		private int _size;

		// Token: 0x040007C5 RID: 1989
		private int _max;

		// Token: 0x040007C6 RID: 1990
		private int _freeList;

		// Token: 0x040007C7 RID: 1991
		private bool _initialized;

		// Token: 0x020001D2 RID: 466
		// (Invoke) Token: 0x06000B73 RID: 2931
		public delegate bool LessOrEqual(TValue lhs, TValue rhs);

		// Token: 0x020001D3 RID: 467
		protected class HandleElem
		{
			// Token: 0x04000C03 RID: 3075
			internal TValue _key;

			// Token: 0x04000C04 RID: 3076
			internal int _node;
		}
	}
}
