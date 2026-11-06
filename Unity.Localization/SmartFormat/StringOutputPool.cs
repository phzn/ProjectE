using System;
using UnityEngine.Localization.SmartFormat.Core.Output;
using UnityEngine.Pool;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x02000041 RID: 65
	internal static class StringOutputPool
	{
		// Token: 0x06000256 RID: 598 RVA: 0x000084F2 File Offset: 0x000066F2
		public static StringOutput Get(int capacity)
		{
			StringOutput stringOutput = StringOutputPool.s_Pool.Get();
			stringOutput.SetCapacity(capacity);
			return stringOutput;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00008505 File Offset: 0x00006705
		public static PooledObject<StringOutput> Get(int capacity, out StringOutput value)
		{
			PooledObject<StringOutput> pooledObject = StringOutputPool.s_Pool.Get(out value);
			value.SetCapacity(capacity);
			return pooledObject;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000851A File Offset: 0x0000671A
		public static void Release(StringOutput toRelease)
		{
			StringOutputPool.s_Pool.Release(toRelease);
		}

		// Token: 0x0400008B RID: 139
		internal static readonly ObjectPool<StringOutput> s_Pool = new ObjectPool<StringOutput>(() => new StringOutput(), null, delegate(StringOutput so)
		{
			so.Clear();
		}, null, true, 10, 10000);
	}
}
