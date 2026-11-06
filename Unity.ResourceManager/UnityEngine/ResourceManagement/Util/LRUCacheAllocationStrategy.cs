using System;
using System.Collections.Generic;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000022 RID: 34
	public class LRUCacheAllocationStrategy : IAllocationStrategy
	{
		// Token: 0x060000EB RID: 235 RVA: 0x00004BA4 File Offset: 0x00002DA4
		public LRUCacheAllocationStrategy(int poolMaxSize, int poolCapacity, int poolCacheMaxSize, int initialPoolCacheCapacity)
		{
			this.m_poolMaxSize = poolMaxSize;
			this.m_poolInitialCapacity = poolCapacity;
			this.m_poolCacheMaxSize = poolCacheMaxSize;
			for (int i = 0; i < initialPoolCacheCapacity; i++)
			{
				this.m_poolCache.Add(new List<object>(this.m_poolInitialCapacity));
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004C08 File Offset: 0x00002E08
		private List<object> GetPool()
		{
			int count = this.m_poolCache.Count;
			if (count == 0)
			{
				return new List<object>(this.m_poolInitialCapacity);
			}
			List<object> list = this.m_poolCache[count - 1];
			this.m_poolCache.RemoveAt(count - 1);
			return list;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004C4C File Offset: 0x00002E4C
		private void ReleasePool(List<object> pool)
		{
			if (this.m_poolCache.Count < this.m_poolCacheMaxSize)
			{
				this.m_poolCache.Add(pool);
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004C70 File Offset: 0x00002E70
		public object New(Type type, int typeHash)
		{
			List<object> list;
			if (this.m_cache.TryGetValue(typeHash, out list))
			{
				int count = list.Count;
				object obj = list[count - 1];
				list.RemoveAt(count - 1);
				if (count == 1)
				{
					this.m_cache.Remove(typeHash);
					this.ReleasePool(list);
				}
				return obj;
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public void Release(int typeHash, object obj)
		{
			List<object> pool;
			if (!this.m_cache.TryGetValue(typeHash, out pool))
			{
				this.m_cache.Add(typeHash, pool = this.GetPool());
			}
			if (pool.Count < this.m_poolMaxSize)
			{
				pool.Add(obj);
			}
		}

		// Token: 0x04000051 RID: 81
		private int m_poolMaxSize;

		// Token: 0x04000052 RID: 82
		private int m_poolInitialCapacity;

		// Token: 0x04000053 RID: 83
		private int m_poolCacheMaxSize;

		// Token: 0x04000054 RID: 84
		private List<List<object>> m_poolCache = new List<List<object>>();

		// Token: 0x04000055 RID: 85
		private Dictionary<int, List<object>> m_cache = new Dictionary<int, List<object>>();
	}
}
