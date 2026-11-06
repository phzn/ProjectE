using System;
using System.Collections.Generic;

namespace System.Linq
{
	// Token: 0x020000EC RID: 236
	internal static class Utilities
	{
		// Token: 0x06000846 RID: 2118 RVA: 0x0001CA68 File Offset: 0x0001AC68
		public static bool AreEqualityComparersEqual<TSource>(IEqualityComparer<TSource> left, IEqualityComparer<TSource> right)
		{
			if (left == right)
			{
				return true;
			}
			EqualityComparer<TSource> @default = EqualityComparer<TSource>.Default;
			if (left == null)
			{
				return right == @default || right.Equals(@default);
			}
			if (right == null)
			{
				return left == @default || left.Equals(@default);
			}
			return left.Equals(right);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001CAAA File Offset: 0x0001ACAA
		public static Func<TSource, bool> CombinePredicates<TSource>(Func<TSource, bool> predicate1, Func<TSource, bool> predicate2)
		{
			return (TSource x) => predicate1(x) && predicate2(x);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001CACA File Offset: 0x0001ACCA
		public static Func<TSource, TResult> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, TMiddle> selector1, Func<TMiddle, TResult> selector2)
		{
			return (TSource x) => selector2(selector1(x));
		}
	}
}
