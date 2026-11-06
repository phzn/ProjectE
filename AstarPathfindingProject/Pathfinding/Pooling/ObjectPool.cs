using System;

namespace Pathfinding.Pooling
{
	// Token: 0x02000258 RID: 600
	public static class ObjectPool<T> where T : class, IAstarPooledObject, new()
	{
		// Token: 0x06000EEE RID: 3822 RVA: 0x0005C248 File Offset: 0x0005A448
		public static T Claim()
		{
			return ObjectPoolSimple<T>.Claim();
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0005C24F File Offset: 0x0005A44F
		public static void Release(ref T obj)
		{
			obj.OnEnterPool();
			ObjectPoolSimple<T>.Release(ref obj);
		}
	}
}
