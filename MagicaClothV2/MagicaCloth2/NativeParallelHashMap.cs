using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace MagicaCloth2
{
	// Token: 0x0200011C RID: 284
	public static class NativeParallelHashMap
	{
		// Token: 0x060005C7 RID: 1479 RVA: 0x0002FE2C File Offset: 0x0002E02C
		public static void MC2DisposeSafe<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(this NativeParallelHashMap<TKey, TValue> map) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType, IEquatable<TValue>
		{
			if (map.IsCreated)
			{
				map.Dispose();
			}
		}
	}
}
