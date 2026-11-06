using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	// Token: 0x020001E5 RID: 485
	internal static class CancellableEnumerable
	{
		// Token: 0x06000BF0 RID: 3056 RVA: 0x0002A04C File Offset: 0x0002824C
		internal static IEnumerable<TElement> Wrap<TElement>(IEnumerable<TElement> source, CancellationToken token)
		{
			int count = 0;
			foreach (TElement telement in source)
			{
				int num = count;
				count = num + 1;
				if ((num & 63) == 0)
				{
					CancellationState.ThrowIfCanceled(token);
				}
				yield return telement;
			}
			IEnumerator<TElement> enumerator = null;
			yield break;
			yield break;
		}
	}
}
