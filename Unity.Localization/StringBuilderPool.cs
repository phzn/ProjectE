using System;
using System.Text;
using UnityEngine.Pool;

namespace UnityEngine.Localization
{
	// Token: 0x02000024 RID: 36
	internal static class StringBuilderPool
	{
		// Token: 0x0600010D RID: 269 RVA: 0x0000497B File Offset: 0x00002B7B
		public static StringBuilder Get()
		{
			return StringBuilderPool.s_Pool.Get();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004987 File Offset: 0x00002B87
		public static PooledObject<StringBuilder> Get(out StringBuilder value)
		{
			return StringBuilderPool.s_Pool.Get(out value);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004994 File Offset: 0x00002B94
		public static void Release(StringBuilder toRelease)
		{
			StringBuilderPool.s_Pool.Release(toRelease);
		}

		// Token: 0x04000040 RID: 64
		internal static readonly ObjectPool<StringBuilder> s_Pool = new ObjectPool<StringBuilder>(() => new StringBuilder(), null, delegate(StringBuilder sb)
		{
			sb.Clear();
		}, null, false, 10, 10000);
	}
}
