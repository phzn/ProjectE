using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200004D RID: 77
	public static class GenericPool<T> where T : new()
	{
		// Token: 0x06000296 RID: 662 RVA: 0x0000C40C File Offset: 0x0000A60C
		public static T Get()
		{
			return GenericPool<T>.s_Pool.Get();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000C418 File Offset: 0x0000A618
		public static ObjectPool<T>.PooledObject Get(out T value)
		{
			return GenericPool<T>.s_Pool.Get(out value);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000C425 File Offset: 0x0000A625
		public static void Release(T toRelease)
		{
			GenericPool<T>.s_Pool.Release(toRelease);
		}

		// Token: 0x04000195 RID: 405
		private static readonly ObjectPool<T> s_Pool = new ObjectPool<T>(null, null, true);
	}
}
