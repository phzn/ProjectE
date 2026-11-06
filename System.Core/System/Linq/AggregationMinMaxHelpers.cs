using System;
using System.Collections.Generic;
using System.Linq.Parallel;

namespace System.Linq
{
	// Token: 0x02000079 RID: 121
	internal static class AggregationMinMaxHelpers<T>
	{
		// Token: 0x06000288 RID: 648 RVA: 0x00007F04 File Offset: 0x00006104
		private static T Reduce(IEnumerable<T> source, int sign)
		{
			Func<Pair<bool, T>, T, Pair<bool, T>> func = AggregationMinMaxHelpers<T>.MakeIntermediateReduceFunction(sign);
			Func<Pair<bool, T>, Pair<bool, T>, Pair<bool, T>> func2 = AggregationMinMaxHelpers<T>.MakeFinalReduceFunction(sign);
			Func<Pair<bool, T>, T> func3 = AggregationMinMaxHelpers<T>.MakeResultSelectorFunction();
			return new AssociativeAggregationOperator<T, Pair<bool, T>, T>(source, new Pair<bool, T>(false, default(T)), null, true, func, func2, func3, default(T) != null, QueryAggregationOptions.AssociativeCommutative).Aggregate();
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00007F56 File Offset: 0x00006156
		internal static T ReduceMin(IEnumerable<T> source)
		{
			return AggregationMinMaxHelpers<T>.Reduce(source, -1);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00007F5F File Offset: 0x0000615F
		internal static T ReduceMax(IEnumerable<T> source)
		{
			return AggregationMinMaxHelpers<T>.Reduce(source, 1);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00007F68 File Offset: 0x00006168
		private static Func<Pair<bool, T>, T, Pair<bool, T>> MakeIntermediateReduceFunction(int sign)
		{
			Comparer<T> comparer = Util.GetDefaultComparer<T>();
			return delegate(Pair<bool, T> accumulator, T element)
			{
				if ((default(T) != null || element != null) && (!accumulator.First || Util.Sign(comparer.Compare(element, accumulator.Second)) == sign))
				{
					return new Pair<bool, T>(true, element);
				}
				return accumulator;
			};
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00007F8C File Offset: 0x0000618C
		private static Func<Pair<bool, T>, Pair<bool, T>, Pair<bool, T>> MakeFinalReduceFunction(int sign)
		{
			Comparer<T> comparer = Util.GetDefaultComparer<T>();
			return delegate(Pair<bool, T> accumulator, Pair<bool, T> element)
			{
				if (element.First && (!accumulator.First || Util.Sign(comparer.Compare(element.Second, accumulator.Second)) == sign))
				{
					return new Pair<bool, T>(true, element.Second);
				}
				return accumulator;
			};
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00007FB0 File Offset: 0x000061B0
		private static Func<Pair<bool, T>, T> MakeResultSelectorFunction()
		{
			return (Pair<bool, T> accumulator) => accumulator.Second;
		}
	}
}
