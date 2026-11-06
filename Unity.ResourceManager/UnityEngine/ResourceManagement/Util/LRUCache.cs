using System;
using System.Collections.Generic;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000014 RID: 20
	internal struct LRUCache<TKey, TValue> where TKey : IEquatable<TKey>
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00004380 File Offset: 0x00002580
		public LRUCache(int limit)
		{
			this.entryLimit = limit;
			this.cache = new Dictionary<TKey, LRUCache<TKey, TValue>.Entry>();
			this.lru = new LinkedList<TKey>();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000043A0 File Offset: 0x000025A0
		public bool TryAdd(TKey id, TValue obj)
		{
			if (obj == null || this.entryLimit <= 0)
			{
				return false;
			}
			this.cache.Add(id, new LRUCache<TKey, TValue>.Entry
			{
				Value = obj,
				lruNode = this.lru.AddFirst(id)
			});
			while (this.lru.Count > this.entryLimit)
			{
				this.cache.Remove(this.lru.Last.Value);
				this.lru.RemoveLast();
			}
			return true;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004430 File Offset: 0x00002630
		public bool TryGet(TKey offset, out TValue val)
		{
			LRUCache<TKey, TValue>.Entry entry;
			if (this.cache.TryGetValue(offset, out entry))
			{
				val = entry.Value;
				if (entry.lruNode.Previous != null)
				{
					this.lru.Remove(entry.lruNode);
					this.lru.AddFirst(entry.lruNode);
				}
				return true;
			}
			val = default(TValue);
			return false;
		}

		// Token: 0x04000041 RID: 65
		private int entryLimit;

		// Token: 0x04000042 RID: 66
		private Dictionary<TKey, LRUCache<TKey, TValue>.Entry> cache;

		// Token: 0x04000043 RID: 67
		private LinkedList<TKey> lru;

		// Token: 0x02000071 RID: 113
		public struct Entry : IEquatable<LRUCache<TKey, TValue>.Entry>
		{
			// Token: 0x0600034A RID: 842 RVA: 0x0000B7DE File Offset: 0x000099DE
			public bool Equals(LRUCache<TKey, TValue>.Entry other)
			{
				return this.Value.Equals(other);
			}

			// Token: 0x0600034B RID: 843 RVA: 0x0000B7F7 File Offset: 0x000099F7
			public override int GetHashCode()
			{
				return this.Value.GetHashCode();
			}

			// Token: 0x0400014F RID: 335
			public LinkedListNode<TKey> lruNode;

			// Token: 0x04000150 RID: 336
			public TValue Value;
		}
	}
}
