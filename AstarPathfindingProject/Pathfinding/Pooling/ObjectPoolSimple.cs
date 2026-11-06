using System;
using System.Collections.Generic;

namespace Pathfinding.Pooling
{
	// Token: 0x02000259 RID: 601
	public static class ObjectPoolSimple<T> where T : class, new()
	{
		// Token: 0x06000EF0 RID: 3824 RVA: 0x0005C264 File Offset: 0x0005A464
		public static T Claim()
		{
			List<T> list = ObjectPoolSimple<T>.pool;
			T t2;
			lock (list)
			{
				if (ObjectPoolSimple<T>.pool.Count > 0)
				{
					T t = ObjectPoolSimple<T>.pool[ObjectPoolSimple<T>.pool.Count - 1];
					ObjectPoolSimple<T>.pool.RemoveAt(ObjectPoolSimple<T>.pool.Count - 1);
					ObjectPoolSimple<T>.inPool.Remove(t);
					t2 = t;
				}
				else
				{
					t2 = new T();
				}
			}
			return t2;
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0005C2F0 File Offset: 0x0005A4F0
		public static void Release(ref T obj)
		{
			List<T> list = ObjectPoolSimple<T>.pool;
			lock (list)
			{
				ObjectPoolSimple<T>.pool.Add(obj);
			}
			obj = default(T);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0005C340 File Offset: 0x0005A540
		public static void Clear()
		{
			List<T> list = ObjectPoolSimple<T>.pool;
			lock (list)
			{
				ObjectPoolSimple<T>.pool.Clear();
			}
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0005C384 File Offset: 0x0005A584
		public static int GetSize()
		{
			return ObjectPoolSimple<T>.pool.Count;
		}

		// Token: 0x04000B2C RID: 2860
		private static List<T> pool = new List<T>();

		// Token: 0x04000B2D RID: 2861
		private static readonly HashSet<T> inPool = new HashSet<T>();
	}
}
