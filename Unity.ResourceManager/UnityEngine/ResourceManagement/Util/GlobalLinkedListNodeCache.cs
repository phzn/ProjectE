using System;
using System.Collections.Generic;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000025 RID: 37
	internal static class GlobalLinkedListNodeCache<T>
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004E2C File Offset: 0x0000302C
		public static bool CacheExists
		{
			get
			{
				return GlobalLinkedListNodeCache<T>.m_globalCache != null;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004E36 File Offset: 0x00003036
		public static void SetCacheSize(int length)
		{
			if (GlobalLinkedListNodeCache<T>.m_globalCache == null)
			{
				GlobalLinkedListNodeCache<T>.m_globalCache = new LinkedListNodeCache<T>();
			}
			GlobalLinkedListNodeCache<T>.m_globalCache.CachedNodeCount = length;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004E54 File Offset: 0x00003054
		public static LinkedListNode<T> Acquire(T val)
		{
			if (GlobalLinkedListNodeCache<T>.m_globalCache == null)
			{
				GlobalLinkedListNodeCache<T>.m_globalCache = new LinkedListNodeCache<T>();
			}
			return GlobalLinkedListNodeCache<T>.m_globalCache.Acquire(val);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004E72 File Offset: 0x00003072
		public static void Release(LinkedListNode<T> node)
		{
			if (GlobalLinkedListNodeCache<T>.m_globalCache == null)
			{
				GlobalLinkedListNodeCache<T>.m_globalCache = new LinkedListNodeCache<T>();
			}
			GlobalLinkedListNodeCache<T>.m_globalCache.Release(node);
		}

		// Token: 0x04000059 RID: 89
		private static LinkedListNodeCache<T> m_globalCache;
	}
}
