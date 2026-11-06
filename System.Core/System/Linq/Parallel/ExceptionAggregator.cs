using System;
using System.Collections.Generic;

namespace System.Linq.Parallel
{
	// Token: 0x020001E7 RID: 487
	internal static class ExceptionAggregator
	{
		// Token: 0x06000BFA RID: 3066 RVA: 0x0002A20B File Offset: 0x0002840B
		internal static IEnumerable<TElement> WrapEnumerable<TElement>(IEnumerable<TElement> source, CancellationState cancellationState)
		{
			using (IEnumerator<TElement> enumerator = source.GetEnumerator())
			{
				for (;;)
				{
					TElement telement = default(TElement);
					try
					{
						if (!enumerator.MoveNext())
						{
							yield break;
						}
						telement = enumerator.Current;
					}
					catch (Exception ex)
					{
						ExceptionAggregator.ThrowOCEorAggregateException(ex, cancellationState);
					}
					yield return telement;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0002A222 File Offset: 0x00028422
		internal static IEnumerable<TElement> WrapQueryEnumerator<TElement, TIgnoreKey>(QueryOperatorEnumerator<TElement, TIgnoreKey> source, CancellationState cancellationState)
		{
			TElement elem = default(TElement);
			TIgnoreKey ignoreKey = default(TIgnoreKey);
			try
			{
				for (;;)
				{
					try
					{
						if (!source.MoveNext(ref elem, ref ignoreKey))
						{
							yield break;
						}
					}
					catch (Exception ex)
					{
						ExceptionAggregator.ThrowOCEorAggregateException(ex, cancellationState);
					}
					yield return elem;
				}
			}
			finally
			{
				source.Dispose();
			}
			yield break;
			yield break;
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0002A239 File Offset: 0x00028439
		internal static void ThrowOCEorAggregateException(Exception ex, CancellationState cancellationState)
		{
			if (ExceptionAggregator.ThrowAnOCE(ex, cancellationState))
			{
				CancellationState.ThrowWithStandardMessageIfCanceled(cancellationState.ExternalCancellationToken);
				return;
			}
			throw new AggregateException(new Exception[] { ex });
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0002A25F File Offset: 0x0002845F
		internal static Func<T, U> WrapFunc<T, U>(Func<T, U> f, CancellationState cancellationState)
		{
			return delegate(T t)
			{
				U u = default(U);
				try
				{
					u = f(t);
				}
				catch (Exception ex)
				{
					ExceptionAggregator.ThrowOCEorAggregateException(ex, cancellationState);
				}
				return u;
			};
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0002A280 File Offset: 0x00028480
		private static bool ThrowAnOCE(Exception ex, CancellationState cancellationState)
		{
			OperationCanceledException ex2 = ex as OperationCanceledException;
			return (ex2 != null && ex2.CancellationToken == cancellationState.ExternalCancellationToken && cancellationState.ExternalCancellationToken.IsCancellationRequested) || (ex2 != null && ex2.CancellationToken == cancellationState.MergedCancellationToken && cancellationState.MergedCancellationToken.IsCancellationRequested && cancellationState.ExternalCancellationToken.IsCancellationRequested);
		}
	}
}
