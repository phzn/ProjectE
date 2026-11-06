using System;
using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Pool;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x02000040 RID: 64
	internal static class SplitListPool
	{
		// Token: 0x06000253 RID: 595 RVA: 0x0000849B File Offset: 0x0000669B
		public static Format.SplitList Get(Format format, List<int> splits)
		{
			Format.SplitList splitList = SplitListPool.s_Pool.Get();
			splitList.Init(format, splits);
			return splitList;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000084AF File Offset: 0x000066AF
		public static void Release(Format.SplitList toRelease)
		{
			SplitListPool.s_Pool.Release(toRelease);
		}

		// Token: 0x0400008A RID: 138
		internal static readonly ObjectPool<Format.SplitList> s_Pool = new ObjectPool<Format.SplitList>(() => new Format.SplitList(), null, delegate(Format.SplitList sl)
		{
			sl.Clear();
		}, null, true, 10, 10000);
	}
}
