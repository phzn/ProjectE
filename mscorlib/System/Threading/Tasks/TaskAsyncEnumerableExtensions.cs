using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
	// Token: 0x02000386 RID: 902
	public static class TaskAsyncEnumerableExtensions
	{
		// Token: 0x06002553 RID: 9555 RVA: 0x00084795 File Offset: 0x00082995
		public static ConfiguredAsyncDisposable ConfigureAwait(this IAsyncDisposable source, bool continueOnCapturedContext)
		{
			return new ConfiguredAsyncDisposable(source, continueOnCapturedContext);
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000847A0 File Offset: 0x000829A0
		public static ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait<T>(this IAsyncEnumerable<T> source, bool continueOnCapturedContext)
		{
			return new ConfiguredCancelableAsyncEnumerable<T>(source, continueOnCapturedContext, default(CancellationToken));
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000847BD File Offset: 0x000829BD
		public static ConfiguredCancelableAsyncEnumerable<T> WithCancellation<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken)
		{
			return new ConfiguredCancelableAsyncEnumerable<T>(source, true, cancellationToken);
		}
	}
}
