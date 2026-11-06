using System;
using System.Collections.Generic;

namespace Microsoft.Internal.Collections
{
	// Token: 0x0200001B RID: 27
	internal class WeakReferenceCollection<T> where T : class
	{
		// Token: 0x060000FB RID: 251 RVA: 0x00003D3B File Offset: 0x00001F3B
		public void Add(T item)
		{
			if (this._items.Capacity == this._items.Count)
			{
				this.CleanupDeadReferences();
			}
			this._items.Add(new WeakReference(item));
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00003D74 File Offset: 0x00001F74
		public void Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num != -1)
			{
				this._items.RemoveAt(num);
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00003D99 File Offset: 0x00001F99
		public bool Contains(T item)
		{
			return this.IndexOf(item) >= 0;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00003DA8 File Offset: 0x00001FA8
		public void Clear()
		{
			this._items.Clear();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003DB8 File Offset: 0x00001FB8
		private int IndexOf(T item)
		{
			int count = this._items.Count;
			for (int i = 0; i < count; i++)
			{
				if (this._items[i].Target == item)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00003DF9 File Offset: 0x00001FF9
		private void CleanupDeadReferences()
		{
			this._items.RemoveAll((WeakReference w) => !w.IsAlive);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00003E28 File Offset: 0x00002028
		public List<T> AliveItemsToList()
		{
			List<T> list = new List<T>();
			foreach (WeakReference weakReference in this._items)
			{
				T t = weakReference.Target as T;
				if (t != null)
				{
					list.Add(t);
				}
			}
			return list;
		}

		// Token: 0x04000064 RID: 100
		private readonly List<WeakReference> _items = new List<WeakReference>();
	}
}
