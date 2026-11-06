using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200004E RID: 78
	public static class UnsafeGenericPool<T> where T : new()
	{
		// Token: 0x0600029A RID: 666 RVA: 0x0000C441 File Offset: 0x0000A641
		public static T Get()
		{
			return UnsafeGenericPool<T>.s_Pool.Get();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000C44D File Offset: 0x0000A64D
		public static ObjectPool<T>.PooledObject Get(out T value)
		{
			return UnsafeGenericPool<T>.s_Pool.Get(out value);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000C45A File Offset: 0x0000A65A
		public static void Release(T toRelease)
		{
			UnsafeGenericPool<T>.s_Pool.Release(toRelease);
		}

		// Token: 0x04000196 RID: 406
		private static readonly ObjectPool<T> s_Pool = new ObjectPool<T>(null, null, false);
	}
}
