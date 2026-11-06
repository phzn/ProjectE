using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000107 RID: 263
	public class MMShufflebag<T>
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x0001BFBF File Offset: 0x0001A1BF
		public virtual int Capacity
		{
			get
			{
				return this._contents.Capacity;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0001BFCC File Offset: 0x0001A1CC
		public virtual int Size
		{
			get
			{
				return this._contents.Count;
			}
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001BFD9 File Offset: 0x0001A1D9
		public MMShufflebag(int initialCapacity)
		{
			this._contents = new List<T>(initialCapacity);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001BFF4 File Offset: 0x0001A1F4
		public virtual void Add(T item, int quantity)
		{
			for (int i = 0; i < quantity; i++)
			{
				this._contents.Add(item);
			}
			this._currentIndex = this.Size - 1;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001C028 File Offset: 0x0001A228
		public T Pick()
		{
			if (this._currentIndex < 1)
			{
				this._currentIndex = this.Size - 1;
				this._currentItem = this._contents[0];
				return this._currentItem;
			}
			int num = Random.Range(0, this._currentIndex);
			this._currentItem = this._contents[num];
			this._contents[num] = this._contents[this._currentIndex];
			this._contents[this._currentIndex] = this._currentItem;
			this._currentIndex--;
			return this._currentItem;
		}

		// Token: 0x040006F7 RID: 1783
		protected List<T> _contents;

		// Token: 0x040006F8 RID: 1784
		protected T _currentItem;

		// Token: 0x040006F9 RID: 1785
		protected int _currentIndex = -1;
	}
}
