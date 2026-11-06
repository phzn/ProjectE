using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
	/// <summary>Provides a set of <see langword="static" /> (<see langword="Shared" /> in Visual Basic) methods for querying objects that implement <see cref="T:System.Collections.Generic.IEnumerable`1" />.</summary>
	// Token: 0x02000099 RID: 153
	public static class Enumerable
	{
		/// <summary>Applies an accumulator function over a sequence.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to aggregate over.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="func" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x060004D7 RID: 1239 RVA: 0x0000F8B8 File Offset: 0x0000DAB8
		public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (func == null)
			{
				throw Error.ArgumentNull("func");
			}
			TSource tsource3;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				TSource tsource = enumerator.Current;
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					tsource = func(tsource, tsource2);
				}
				tsource3 = tsource;
			}
			return tsource3;
		}

		/// <summary>Applies an accumulator function over a sequence. The specified seed value is used as the initial accumulator value.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to aggregate over.</param>
		/// <param name="seed">The initial accumulator value.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="func" /> is <see langword="null" />.</exception>
		// Token: 0x060004D8 RID: 1240 RVA: 0x0000F934 File Offset: 0x0000DB34
		public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (func == null)
			{
				throw Error.ArgumentNull("func");
			}
			TAccumulate taccumulate = seed;
			foreach (TSource tsource in source)
			{
				taccumulate = func(taccumulate, tsource);
			}
			return taccumulate;
		}

		/// <summary>Applies an accumulator function over a sequence. The specified seed value is used as the initial accumulator value, and the specified function is used to select the result value.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to aggregate over.</param>
		/// <param name="seed">The initial accumulator value.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
		/// <typeparam name="TResult">The type of the resulting value.</typeparam>
		/// <returns>The transformed final accumulator value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="func" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
		// Token: 0x060004D9 RID: 1241 RVA: 0x0000F9A0 File Offset: 0x0000DBA0
		public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (func == null)
			{
				throw Error.ArgumentNull("func");
			}
			if (resultSelector == null)
			{
				throw Error.ArgumentNull("resultSelector");
			}
			TAccumulate taccumulate = seed;
			foreach (TSource tsource in source)
			{
				taccumulate = func(taccumulate, tsource);
			}
			return resultSelector(taccumulate);
		}

		/// <summary>Determines whether a sequence contains any elements.</summary>
		/// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> to check for emptiness.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>
		///     <see langword="true" /> if the source sequence contains any elements; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x060004DA RID: 1242 RVA: 0x0000FA20 File Offset: 0x0000DC20
		public static bool Any<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			bool flag;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				flag = enumerator.MoveNext();
			}
			return flag;
		}

		/// <summary>Determines whether any element of a sequence satisfies a condition.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements to apply the predicate to.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>
		///     <see langword="true" /> if any elements in the source sequence pass the test in the specified predicate; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		// Token: 0x060004DB RID: 1243 RVA: 0x0000FA68 File Offset: 0x0000DC68
		public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			foreach (TSource tsource in source)
			{
				if (predicate(tsource))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Determines whether all elements of a sequence satisfy a condition.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements to apply the predicate to.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>
		///     <see langword="true" /> if every element of the source sequence passes the test in the specified predicate, or if the sequence is empty; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		// Token: 0x060004DC RID: 1244 RVA: 0x0000FAD8 File Offset: 0x0000DCD8
		public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			foreach (TSource tsource in source)
			{
				if (!predicate(tsource))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Appends a value to the end of the sequence.</summary>
		/// <param name="source">A sequence of values. </param>
		/// <param name="element">The value to append to <paramref name="source" />.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />. </typeparam>
		/// <returns>A new sequence that ends with <paramref name="element" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x060004DD RID: 1245 RVA: 0x0000FB48 File Offset: 0x0000DD48
		public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource element)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			Enumerable.AppendPrependIterator<TSource> appendPrependIterator = source as Enumerable.AppendPrependIterator<TSource>;
			if (appendPrependIterator == null)
			{
				return new Enumerable.AppendPrepend1Iterator<TSource>(source, element, true);
			}
			return appendPrependIterator.Append(element);
		}

		/// <summary>Adds a value to the beginning of the sequence.</summary>
		/// <param name="source">A sequence of values. </param>
		/// <param name="element">The value to prepend to <paramref name="source" />. </param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>A new sequence that begins with <paramref name="element" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source" /> is <see langword="null" />. </exception>
		// Token: 0x060004DE RID: 1246 RVA: 0x0000FB80 File Offset: 0x0000DD80
		public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource element)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			Enumerable.AppendPrependIterator<TSource> appendPrependIterator = source as Enumerable.AppendPrependIterator<TSource>;
			if (appendPrependIterator == null)
			{
				return new Enumerable.AppendPrepend1Iterator<TSource>(source, element, false);
			}
			return appendPrependIterator.Prepend(element);
		}

		/// <summary>Computes the average of a sequence of <see cref="T:System.Int32" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Int32" /> values to calculate the average of.</param>
		/// <returns>The average of the sequence of values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x060004DF RID: 1247 RVA: 0x0000FBB8 File Offset: 0x0000DDB8
		public static double Average(this IEnumerable<int> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			double num4;
			using (IEnumerator<int> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				long num = (long)enumerator.Current;
				long num2 = 1L;
				checked
				{
					while (enumerator.MoveNext())
					{
						int num3 = enumerator.Current;
						num += unchecked((long)num3);
						num2 += 1L;
					}
					num4 = (double)num / (double)num2;
				}
			}
			return num4;
		}

		/// <summary>Computes the average of a sequence of nullable <see cref="T:System.Int32" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int32" /> values to calculate the average of.</param>
		/// <returns>The average of the sequence of values, or <see langword="null" /> if the source sequence is empty or contains only values that are <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum of the elements in the sequence is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
		// Token: 0x060004E0 RID: 1248 RVA: 0x0000FC30 File Offset: 0x0000DE30
		public static double? Average(this IEnumerable<int?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			using (IEnumerator<int?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int? num = enumerator.Current;
					if (num != null)
					{
						long num2 = (long)num.GetValueOrDefault();
						long num3 = 1L;
						checked
						{
							while (enumerator.MoveNext())
							{
								num = enumerator.Current;
								if (num != null)
								{
									num2 += unchecked((long)num.GetValueOrDefault());
									num3 += 1L;
								}
							}
							return new double?((double)num2 / (double)num3);
						}
					}
				}
			}
			return null;
		}

		/// <summary>Computes the average of a sequence of <see cref="T:System.Int64" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Int64" /> values to calculate the average of.</param>
		/// <returns>The average of the sequence of values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x060004E1 RID: 1249 RVA: 0x0000FCDC File Offset: 0x0000DEDC
		public static double Average(this IEnumerable<long> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			checked
			{
				double num4;
				using (IEnumerator<long> enumerator = source.GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						throw Error.NoElements();
					}
					long num = enumerator.Current;
					long num2 = 1L;
					while (enumerator.MoveNext())
					{
						long num3 = enumerator.Current;
						num += num3;
						num2 += 1L;
					}
					num4 = (double)num / (double)num2;
				}
				return num4;
			}
		}

		/// <summary>Computes the average of a sequence of nullable <see cref="T:System.Int64" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int64" /> values to calculate the average of.</param>
		/// <returns>The average of the sequence of values, or <see langword="null" /> if the source sequence is empty or contains only values that are <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum of the elements in the sequence is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
		// Token: 0x060004E2 RID: 1250 RVA: 0x0000FD54 File Offset: 0x0000DF54
		public static double? Average(this IEnumerable<long?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			checked
			{
				using (IEnumerator<long?> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						long? num = enumerator.Current;
						if (num != null)
						{
							long num2 = num.GetValueOrDefault();
							long num3 = 1L;
							while (enumerator.MoveNext())
							{
								num = enumerator.Current;
								if (num != null)
								{
									num2 += num.GetValueOrDefault();
									num3 += 1L;
								}
							}
							return new double?((double)num2 / (double)num3);
						}
					}
				}
				return null;
			}
		}

		/// <summary>Computes the average of a sequence of <see cref="T:System.Single" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Single" /> values to calculate the average of.</param>
		/// <returns>The average of the sequence of values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x060004E3 RID: 1251 RVA: 0x0000FDFC File Offset: 0x0000DFFC
		public static float Average(this IEnumerable<float> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			float num4;
			using (IEnumerator<float> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				double num = (double)enumerator.Current;
				long num2 = 1L;
				while (enumerator.MoveNext())
				{
					float num3 = enumerator.Current;
					num += (double)num3;
					num2 += 1L;
				}
				num4 = (float)(num / (double)num2);
			}
			return num4;
		}

		/// <summary>Computes the average of a sequence of nullable <see cref="T:System.Single" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Single" /> values to calculate the average of.</param>
		/// <returns>The average of the sequence of values, or <see langword="null" /> if the source sequence is empty or contains only values that are <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x060004E4 RID: 1252 RVA: 0x0000FE74 File Offset: 0x0000E074
		public static float? Average(this IEnumerable<float?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			using (IEnumerator<float?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					float? num = enumerator.Current;
					if (num != null)
					{
						double num2 = (double)num.GetValueOrDefault();
						long num3 = 1L;
						while (enumerator.MoveNext())
						{
							num = enumerator.Current;
							if (num != null)
							{
								num2 += (double)num.GetValueOrDefault();
								checked
								{
									num3 += 1L;
								}
							}
						}
						return new float?((float)(num2 / (double)num3));
					}
				}
			}
			return null;
		}

		/// <summary>Computes the average of a sequence of <see cref="T:System.Double" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Double" /> values to calculate the average of.</param>
		/// <returns>The average of the sequence of values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x060004E5 RID: 1253 RVA: 0x0000FF20 File Offset: 0x0000E120
		public static double Average(this IEnumerable<double> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			double num4;
			using (IEnumerator<double> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				double num = enumerator.Current;
				long num2 = 1L;
				while (enumerator.MoveNext())
				{
					double num3 = enumerator.Current;
					num += num3;
					num2 += 1L;
				}
				num4 = num / (double)num2;
			}
			return num4;
		}

		/// <summary>Computes the average of a sequence of nullable <see cref="T:System.Double" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Double" /> values to calculate the average of.</param>
		/// <returns>The average of the sequence of values, or <see langword="null" /> if the source sequence is empty or contains only values that are <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x060004E6 RID: 1254 RVA: 0x0000FF94 File Offset: 0x0000E194
		public static double? Average(this IEnumerable<double?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			using (IEnumerator<double?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					double? num = enumerator.Current;
					if (num != null)
					{
						double num2 = num.GetValueOrDefault();
						long num3 = 1L;
						while (enumerator.MoveNext())
						{
							num = enumerator.Current;
							if (num != null)
							{
								num2 += num.GetValueOrDefault();
								checked
								{
									num3 += 1L;
								}
							}
						}
						return new double?(num2 / (double)num3);
					}
				}
			}
			return null;
		}

		/// <summary>Computes the average of a sequence of <see cref="T:System.Decimal" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Decimal" /> values to calculate the average of.</param>
		/// <returns>The average of the sequence of values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x060004E7 RID: 1255 RVA: 0x0001003C File Offset: 0x0000E23C
		public static decimal Average(this IEnumerable<decimal> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			decimal num4;
			using (IEnumerator<decimal> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				decimal num = enumerator.Current;
				long num2 = 1L;
				while (enumerator.MoveNext())
				{
					decimal num3 = enumerator.Current;
					num += num3;
					num2 += 1L;
				}
				num4 = num / num2;
			}
			return num4;
		}

		/// <summary>Computes the average of a sequence of nullable <see cref="T:System.Decimal" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Decimal" /> values to calculate the average of.</param>
		/// <returns>The average of the sequence of values, or <see langword="null" /> if the source sequence is empty or contains only values that are <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum of the elements in the sequence is larger than <see cref="F:System.Decimal.MaxValue" />.</exception>
		// Token: 0x060004E8 RID: 1256 RVA: 0x000100BC File Offset: 0x0000E2BC
		public static decimal? Average(this IEnumerable<decimal?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			using (IEnumerator<decimal?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					decimal? num = enumerator.Current;
					if (num != null)
					{
						decimal num2 = num.GetValueOrDefault();
						long num3 = 1L;
						while (enumerator.MoveNext())
						{
							num = enumerator.Current;
							if (num != null)
							{
								num2 += num.GetValueOrDefault();
								num3 += 1L;
							}
						}
						return new decimal?(num2 / num3);
					}
				}
			}
			return null;
		}

		/// <summary>Computes the average of a sequence of <see cref="T:System.Int32" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The average of the sequence of values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		/// <exception cref="T:System.OverflowException">The sum of the elements in the sequence is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
		// Token: 0x060004E9 RID: 1257 RVA: 0x00010170 File Offset: 0x0000E370
		public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			double num3;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				long num = (long)selector(enumerator.Current);
				long num2 = 1L;
				checked
				{
					while (enumerator.MoveNext())
					{
						TSource tsource = enumerator.Current;
						num += unchecked((long)selector(tsource));
						num2 += 1L;
					}
					num3 = (double)num / (double)num2;
				}
			}
			return num3;
		}

		/// <summary>Computes the average of a sequence of nullable <see cref="T:System.Int32" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The average of the sequence of values, or <see langword="null" /> if the source sequence is empty or contains only values that are <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum of the elements in the sequence is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
		// Token: 0x060004EA RID: 1258 RVA: 0x00010204 File Offset: 0x0000E404
		public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					int? num = selector(tsource);
					if (num != null)
					{
						long num2 = (long)num.GetValueOrDefault();
						long num3 = 1L;
						checked
						{
							while (enumerator.MoveNext())
							{
								TSource tsource2 = enumerator.Current;
								num = selector(tsource2);
								if (num != null)
								{
									num2 += unchecked((long)num.GetValueOrDefault());
									num3 += 1L;
								}
							}
							return new double?((double)num2 / (double)num3);
						}
					}
				}
			}
			return null;
		}

		/// <summary>Computes the average of a sequence of <see cref="T:System.Int64" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <returns>The average of the sequence of values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		/// <exception cref="T:System.OverflowException">The sum of the elements in the sequence is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
		// Token: 0x060004EB RID: 1259 RVA: 0x000102C8 File Offset: 0x0000E4C8
		public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			checked
			{
				double num3;
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						throw Error.NoElements();
					}
					long num = selector(enumerator.Current);
					long num2 = 1L;
					while (enumerator.MoveNext())
					{
						TSource tsource = enumerator.Current;
						num += selector(tsource);
						num2 += 1L;
					}
					num3 = (double)num / (double)num2;
				}
				return num3;
			}
		}

		/// <summary>Computes the average of a sequence of nullable <see cref="T:System.Int64" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The average of the sequence of values, or <see langword="null" /> if the source sequence is empty or contains only values that are <see langword="null" />.</returns>
		// Token: 0x060004EC RID: 1260 RVA: 0x00010358 File Offset: 0x0000E558
		public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			checked
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TSource tsource = enumerator.Current;
						long? num = selector(tsource);
						if (num != null)
						{
							long num2 = num.GetValueOrDefault();
							long num3 = 1L;
							while (enumerator.MoveNext())
							{
								TSource tsource2 = enumerator.Current;
								num = selector(tsource2);
								if (num != null)
								{
									num2 += num.GetValueOrDefault();
									num3 += 1L;
								}
							}
							return new double?((double)num2 / (double)num3);
						}
					}
				}
				return null;
			}
		}

		/// <summary>Computes the average of a sequence of <see cref="T:System.Single" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The average of the sequence of values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x060004ED RID: 1261 RVA: 0x0001041C File Offset: 0x0000E61C
		public static float Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			float num3;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				double num = (double)selector(enumerator.Current);
				long num2 = 1L;
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num += (double)selector(tsource);
					num2 += 1L;
				}
				num3 = (float)(num / (double)num2);
			}
			return num3;
		}

		/// <summary>Computes the average of a sequence of nullable <see cref="T:System.Single" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The average of the sequence of values, or <see langword="null" /> if the source sequence is empty or contains only values that are <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x060004EE RID: 1262 RVA: 0x000104B0 File Offset: 0x0000E6B0
		public static float? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					float? num = selector(tsource);
					if (num != null)
					{
						double num2 = (double)num.GetValueOrDefault();
						long num3 = 1L;
						while (enumerator.MoveNext())
						{
							TSource tsource2 = enumerator.Current;
							num = selector(tsource2);
							if (num != null)
							{
								num2 += (double)num.GetValueOrDefault();
								checked
								{
									num3 += 1L;
								}
							}
						}
						return new float?((float)(num2 / (double)num3));
					}
				}
			}
			return null;
		}

		/// <summary>Computes the average of a sequence of <see cref="T:System.Double" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The average of the sequence of values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x060004EF RID: 1263 RVA: 0x00010574 File Offset: 0x0000E774
		public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			double num3;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				double num = selector(enumerator.Current);
				long num2 = 1L;
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num += selector(tsource);
					num2 += 1L;
				}
				num3 = num / (double)num2;
			}
			return num3;
		}

		/// <summary>Computes the average of a sequence of nullable <see cref="T:System.Double" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The average of the sequence of values, or <see langword="null" /> if the source sequence is empty or contains only values that are <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x060004F0 RID: 1264 RVA: 0x00010604 File Offset: 0x0000E804
		public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					double? num = selector(tsource);
					if (num != null)
					{
						double num2 = num.GetValueOrDefault();
						long num3 = 1L;
						while (enumerator.MoveNext())
						{
							TSource tsource2 = enumerator.Current;
							num = selector(tsource2);
							if (num != null)
							{
								num2 += num.GetValueOrDefault();
								checked
								{
									num3 += 1L;
								}
							}
						}
						return new double?(num2 / (double)num3);
					}
				}
			}
			return null;
		}

		/// <summary>Computes the average of a sequence of <see cref="T:System.Decimal" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values that are used to calculate an average.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The average of the sequence of values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		/// <exception cref="T:System.OverflowException">The sum of the elements in the sequence is larger than <see cref="F:System.Decimal.MaxValue" />.</exception>
		// Token: 0x060004F1 RID: 1265 RVA: 0x000106C4 File Offset: 0x0000E8C4
		public static decimal Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			decimal num3;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				decimal num = selector(enumerator.Current);
				long num2 = 1L;
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num += selector(tsource);
					num2 += 1L;
				}
				num3 = num / num2;
			}
			return num3;
		}

		/// <summary>Computes the average of a sequence of nullable <see cref="T:System.Decimal" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The average of the sequence of values, or <see langword="null" /> if the source sequence is empty or contains only values that are <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum of the elements in the sequence is larger than <see cref="F:System.Decimal.MaxValue" />.</exception>
		// Token: 0x060004F2 RID: 1266 RVA: 0x00010760 File Offset: 0x0000E960
		public static decimal? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					decimal? num = selector(tsource);
					if (num != null)
					{
						decimal num2 = num.GetValueOrDefault();
						long num3 = 1L;
						while (enumerator.MoveNext())
						{
							TSource tsource2 = enumerator.Current;
							num = selector(tsource2);
							if (num != null)
							{
								num2 += num.GetValueOrDefault();
								num3 += 1L;
							}
						}
						return new decimal?(num2 / num3);
					}
				}
			}
			return null;
		}

		/// <summary>Filters the elements of an <see cref="T:System.Collections.IEnumerable" /> based on a specified type.</summary>
		/// <param name="source">The <see cref="T:System.Collections.IEnumerable" /> whose elements to filter.</param>
		/// <typeparam name="TResult">The type to filter the elements of the sequence on.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains elements from the input sequence of type <paramref name="TResult" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x060004F3 RID: 1267 RVA: 0x0001082C File Offset: 0x0000EA2C
		public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			return Enumerable.OfTypeIterator<TResult>(source);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00010842 File Offset: 0x0000EA42
		private static IEnumerable<TResult> OfTypeIterator<TResult>(IEnumerable source)
		{
			foreach (object obj in source)
			{
				if (obj is TResult)
				{
					yield return (TResult)((object)obj);
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		/// <summary>Casts the elements of an <see cref="T:System.Collections.IEnumerable" /> to the specified type.</summary>
		/// <param name="source">The <see cref="T:System.Collections.IEnumerable" /> that contains the elements to be cast to type <paramref name="TResult" />.</param>
		/// <typeparam name="TResult">The type to cast the elements of <paramref name="source" /> to.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains each element of the source sequence cast to the specified type.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidCastException">An element in the sequence cannot be cast to type <paramref name="TResult" />.</exception>
		// Token: 0x060004F5 RID: 1269 RVA: 0x00010854 File Offset: 0x0000EA54
		public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
		{
			IEnumerable<TResult> enumerable = source as IEnumerable<TResult>;
			if (enumerable != null)
			{
				return enumerable;
			}
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			return Enumerable.CastIterator<TResult>(source);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00010881 File Offset: 0x0000EA81
		private static IEnumerable<TResult> CastIterator<TResult>(IEnumerable source)
		{
			foreach (object obj in source)
			{
				yield return (TResult)((object)obj);
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		/// <summary>Concatenates two sequences.</summary>
		/// <param name="first">The first sequence to concatenate.</param>
		/// <param name="second">The sequence to concatenate to the first sequence.</param>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the concatenated elements of the two input sequences.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
		// Token: 0x060004F7 RID: 1271 RVA: 0x00010894 File Offset: 0x0000EA94
		public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null)
			{
				throw Error.ArgumentNull("first");
			}
			if (second == null)
			{
				throw Error.ArgumentNull("second");
			}
			Enumerable.ConcatIterator<TSource> concatIterator = first as Enumerable.ConcatIterator<TSource>;
			if (concatIterator == null)
			{
				return new Enumerable.Concat2Iterator<TSource>(first, second);
			}
			return concatIterator.Concat(second);
		}

		/// <summary>Determines whether a sequence contains a specified element by using the default equality comparer.</summary>
		/// <param name="source">A sequence in which to locate a value.</param>
		/// <param name="value">The value to locate in the sequence.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>
		///     <see langword="true" /> if the source sequence contains an element that has the specified value; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x060004F8 RID: 1272 RVA: 0x000108D8 File Offset: 0x0000EAD8
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
		{
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection == null)
			{
				return source.Contains(value, null);
			}
			return collection.Contains(value);
		}

		/// <summary>Determines whether a sequence contains a specified element by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="source">A sequence in which to locate a value.</param>
		/// <param name="value">The value to locate in the sequence.</param>
		/// <param name="comparer">An equality comparer to compare values.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>
		///     <see langword="true" /> if the source sequence contains an element that has the specified value; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x060004F9 RID: 1273 RVA: 0x00010900 File Offset: 0x0000EB00
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (comparer == null)
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TSource tsource = enumerator.Current;
						if (EqualityComparer<TSource>.Default.Equals(tsource, value))
						{
							return true;
						}
					}
					return false;
				}
			}
			foreach (TSource tsource2 in source)
			{
				if (comparer.Equals(tsource2, value))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Returns the number of elements in a sequence.</summary>
		/// <param name="source">A sequence that contains elements to be counted.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The number of elements in the input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source" /> is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x060004FA RID: 1274 RVA: 0x000109A8 File Offset: 0x0000EBA8
		public static int Count<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null)
			{
				return collection.Count;
			}
			IIListProvider<TSource> iilistProvider = source as IIListProvider<TSource>;
			if (iilistProvider != null)
			{
				return iilistProvider.GetCount(false);
			}
			ICollection collection2 = source as ICollection;
			if (collection2 != null)
			{
				return collection2.Count;
			}
			int num = 0;
			checked
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						num++;
					}
				}
				return num;
			}
		}

		/// <summary>Returns a number that represents how many elements in the specified sequence satisfy a condition.</summary>
		/// <param name="source">A sequence that contains elements to be tested and counted.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>A number that represents how many elements in the sequence satisfy the condition in the predicate function.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source" /> is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x060004FB RID: 1275 RVA: 0x00010A30 File Offset: 0x0000EC30
		public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			int num = 0;
			checked
			{
				foreach (TSource tsource in source)
				{
					if (predicate(tsource))
					{
						num++;
					}
				}
				return num;
			}
		}

		/// <summary>Returns an <see cref="T:System.Int64" /> that represents the total number of elements in a sequence.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements to be counted.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The number of elements in the source sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The number of elements exceeds <see cref="F:System.Int64.MaxValue" />.</exception>
		// Token: 0x060004FC RID: 1276 RVA: 0x00010AA0 File Offset: 0x0000ECA0
		public static long LongCount<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			long num = 0L;
			checked
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						num += 1L;
					}
				}
				return num;
			}
		}

		/// <summary>Returns an <see cref="T:System.Int64" /> that represents how many elements in a sequence satisfy a condition.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements to be counted.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>A number that represents how many elements in the sequence satisfy the condition in the predicate function.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The number of matching elements exceeds <see cref="F:System.Int64.MaxValue" />.</exception>
		// Token: 0x060004FD RID: 1277 RVA: 0x00010AF4 File Offset: 0x0000ECF4
		public static long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			long num = 0L;
			checked
			{
				foreach (TSource tsource in source)
				{
					if (predicate(tsource))
					{
						num += 1L;
					}
				}
				return num;
			}
		}

		/// <summary>Returns the elements of the specified sequence or the type parameter's default value in a singleton collection if the sequence is empty.</summary>
		/// <param name="source">The sequence to return a default value for if it is empty.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> object that contains the default value for the <paramref name="TSource" /> type if <paramref name="source" /> is empty; otherwise, <paramref name="source" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x060004FE RID: 1278 RVA: 0x00010B64 File Offset: 0x0000ED64
		public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
		{
			return source.DefaultIfEmpty(default(TSource));
		}

		/// <summary>Returns the elements of the specified sequence or the specified value in a singleton collection if the sequence is empty.</summary>
		/// <param name="source">The sequence to return the specified value for if it is empty.</param>
		/// <param name="defaultValue">The value to return if the sequence is empty.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <paramref name="defaultValue" /> if <paramref name="source" /> is empty; otherwise, <paramref name="source" />.</returns>
		// Token: 0x060004FF RID: 1279 RVA: 0x00010B80 File Offset: 0x0000ED80
		public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			return new Enumerable.DefaultIfEmptyIterator<TSource>(source, defaultValue);
		}

		/// <summary>Returns distinct elements from a sequence by using the default equality comparer to compare values.</summary>
		/// <param name="source">The sequence to remove duplicate elements from.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains distinct elements from the source sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000500 RID: 1280 RVA: 0x00010B97 File Offset: 0x0000ED97
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
		{
			return source.Distinct<TSource>(null);
		}

		/// <summary>Returns distinct elements from a sequence by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</summary>
		/// <param name="source">The sequence to remove duplicate elements from.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains distinct elements from the source sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000501 RID: 1281 RVA: 0x00010BA0 File Offset: 0x0000EDA0
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			return new Enumerable.DistinctIterator<TSource>(source, comparer);
		}

		/// <summary>Returns the element at a specified index in a sequence.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return an element from.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The element at the specified position in the source sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0 or greater than or equal to the number of elements in <paramref name="source" />.</exception>
		// Token: 0x06000502 RID: 1282 RVA: 0x00010BB8 File Offset: 0x0000EDB8
		public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			IPartition<TSource> partition = source as IPartition<TSource>;
			if (partition != null)
			{
				bool flag;
				TSource tsource = partition.TryGetElementAt(index, out flag);
				if (flag)
				{
					return tsource;
				}
			}
			else
			{
				IList<TSource> list = source as IList<TSource>;
				if (list != null)
				{
					return list[index];
				}
				if (index >= 0)
				{
					using (IEnumerator<TSource> enumerator = source.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (index == 0)
							{
								return enumerator.Current;
							}
							index--;
						}
					}
				}
			}
			throw Error.ArgumentOutOfRange("index");
		}

		/// <summary>Returns the element at a specified index in a sequence or a default value if the index is out of range.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return an element from.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>
		///     <see langword="default" />(<paramref name="TSource" />) if the index is outside the bounds of the source sequence; otherwise, the element at the specified position in the source sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000503 RID: 1283 RVA: 0x00010C54 File Offset: 0x0000EE54
		public static TSource ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, int index)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			IPartition<TSource> partition = source as IPartition<TSource>;
			if (partition != null)
			{
				bool flag;
				return partition.TryGetElementAt(index, out flag);
			}
			if (index >= 0)
			{
				IList<TSource> list = source as IList<TSource>;
				if (list != null)
				{
					if (index < list.Count)
					{
						return list[index];
					}
				}
				else
				{
					using (IEnumerator<TSource> enumerator = source.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (index == 0)
							{
								return enumerator.Current;
							}
							index--;
						}
					}
				}
			}
			return default(TSource);
		}

		/// <summary>Returns the input typed as <see cref="T:System.Collections.Generic.IEnumerable`1" />.</summary>
		/// <param name="source">The sequence to type as <see cref="T:System.Collections.Generic.IEnumerable`1" />.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The input sequence typed as <see cref="T:System.Collections.Generic.IEnumerable`1" />.</returns>
		// Token: 0x06000504 RID: 1284 RVA: 0x000022A7 File Offset: 0x000004A7
		public static IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source)
		{
			return source;
		}

		/// <summary>Returns an empty <see cref="T:System.Collections.Generic.IEnumerable`1" /> that has the specified type argument.</summary>
		/// <typeparam name="TResult">The type to assign to the type parameter of the returned generic <see cref="T:System.Collections.Generic.IEnumerable`1" />.</typeparam>
		/// <returns>An empty <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose type argument is <paramref name="TResult" />.</returns>
		// Token: 0x06000505 RID: 1285 RVA: 0x00010CF0 File Offset: 0x0000EEF0
		public static IEnumerable<TResult> Empty<TResult>()
		{
			return Array.Empty<TResult>();
		}

		/// <summary>Produces the set difference of two sequences by using the default equality comparer to compare values.</summary>
		/// <param name="first">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements that are not also in <paramref name="second" /> will be returned.</param>
		/// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
		// Token: 0x06000506 RID: 1286 RVA: 0x00010CF7 File Offset: 0x0000EEF7
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null)
			{
				throw Error.ArgumentNull("first");
			}
			if (second == null)
			{
				throw Error.ArgumentNull("second");
			}
			return Enumerable.ExceptIterator<TSource>(first, second, null);
		}

		/// <summary>Produces the set difference of two sequences by using the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</summary>
		/// <param name="first">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements that are not also in <paramref name="second" /> will be returned.</param>
		/// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</param>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
		// Token: 0x06000507 RID: 1287 RVA: 0x00010D1D File Offset: 0x0000EF1D
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			if (first == null)
			{
				throw Error.ArgumentNull("first");
			}
			if (second == null)
			{
				throw Error.ArgumentNull("second");
			}
			return Enumerable.ExceptIterator<TSource>(first, second, comparer);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00010D43 File Offset: 0x0000EF43
		private static IEnumerable<TSource> ExceptIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Set<TSource> set = new Set<TSource>(comparer);
			foreach (TSource tsource in second)
			{
				set.Add(tsource);
			}
			foreach (TSource tsource2 in first)
			{
				if (set.Add(tsource2))
				{
					yield return tsource2;
				}
			}
			IEnumerator<TSource> enumerator2 = null;
			yield break;
			yield break;
		}

		/// <summary>Returns the first element of a sequence.</summary>
		/// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return the first element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The first element in the specified sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The source sequence is empty.</exception>
		// Token: 0x06000509 RID: 1289 RVA: 0x00010D64 File Offset: 0x0000EF64
		public static TSource First<TSource>(this IEnumerable<TSource> source)
		{
			bool flag;
			TSource tsource = source.TryGetFirst<TSource>(out flag);
			if (!flag)
			{
				throw Error.NoElements();
			}
			return tsource;
		}

		/// <summary>Returns the first element in a sequence that satisfies a specified condition.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return an element from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The first element in the sequence that passes the test in the specified predicate function.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate" />.-or-The source sequence is empty.</exception>
		// Token: 0x0600050A RID: 1290 RVA: 0x00010D84 File Offset: 0x0000EF84
		public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			bool flag;
			TSource tsource = source.TryGetFirst<TSource>(predicate, out flag);
			if (!flag)
			{
				throw Error.NoMatch();
			}
			return tsource;
		}

		/// <summary>Returns the first element of a sequence, or a default value if the sequence contains no elements.</summary>
		/// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return the first element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>
		///     <see langword="default" />(<paramref name="TSource" />) if <paramref name="source" /> is empty; otherwise, the first element in <paramref name="source" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600050B RID: 1291 RVA: 0x00010DA4 File Offset: 0x0000EFA4
		public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
		{
			bool flag;
			return source.TryGetFirst<TSource>(out flag);
		}

		/// <summary>Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return an element from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>
		///     <see langword="default" />(<paramref name="TSource" />) if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		// Token: 0x0600050C RID: 1292 RVA: 0x00010DBC File Offset: 0x0000EFBC
		public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			bool flag;
			return source.TryGetFirst<TSource>(predicate, out flag);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00010DD4 File Offset: 0x0000EFD4
		private static TSource TryGetFirst<TSource>(this IEnumerable<TSource> source, out bool found)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			IPartition<TSource> partition = source as IPartition<TSource>;
			if (partition != null)
			{
				return partition.TryGetFirst(out found);
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				if (list.Count > 0)
				{
					found = true;
					return list[0];
				}
			}
			else
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						found = true;
						return enumerator.Current;
					}
				}
			}
			found = false;
			return default(TSource);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00010E68 File Offset: 0x0000F068
		private static TSource TryGetFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out bool found)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			OrderedEnumerable<TSource> orderedEnumerable = source as OrderedEnumerable<TSource>;
			if (orderedEnumerable != null)
			{
				return orderedEnumerable.TryGetFirst(predicate, out found);
			}
			foreach (TSource tsource in source)
			{
				if (predicate(tsource))
				{
					found = true;
					return tsource;
				}
			}
			found = false;
			return default(TSource);
		}

		/// <summary>Correlates the elements of two sequences based on equality of keys and groups the results. The default equality comparer is used to compare keys.</summary>
		/// <param name="outer">The first sequence to join.</param>
		/// <param name="inner">The sequence to join to the first sequence.</param>
		/// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
		/// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
		/// <param name="resultSelector">A function to create a result element from an element from the first sequence and a collection of matching elements from the second sequence.</param>
		/// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
		/// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
		/// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
		/// <typeparam name="TResult">The type of the result elements.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains elements of type <paramref name="TResult" /> that are obtained by performing a grouped join on two sequences.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="outer" /> or <paramref name="inner" /> or <paramref name="outerKeySelector" /> or <paramref name="innerKeySelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
		// Token: 0x0600050F RID: 1295 RVA: 0x00010EF8 File Offset: 0x0000F0F8
		public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
		{
			if (outer == null)
			{
				throw Error.ArgumentNull("outer");
			}
			if (inner == null)
			{
				throw Error.ArgumentNull("inner");
			}
			if (outerKeySelector == null)
			{
				throw Error.ArgumentNull("outerKeySelector");
			}
			if (innerKeySelector == null)
			{
				throw Error.ArgumentNull("innerKeySelector");
			}
			if (resultSelector == null)
			{
				throw Error.ArgumentNull("resultSelector");
			}
			return Enumerable.GroupJoinIterator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, null);
		}

		/// <summary>Correlates the elements of two sequences based on key equality and groups the results. A specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> is used to compare keys.</summary>
		/// <param name="outer">The first sequence to join.</param>
		/// <param name="inner">The sequence to join to the first sequence.</param>
		/// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
		/// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
		/// <param name="resultSelector">A function to create a result element from an element from the first sequence and a collection of matching elements from the second sequence.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to hash and compare keys.</param>
		/// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
		/// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
		/// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
		/// <typeparam name="TResult">The type of the result elements.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains elements of type <paramref name="TResult" /> that are obtained by performing a grouped join on two sequences.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="outer" /> or <paramref name="inner" /> or <paramref name="outerKeySelector" /> or <paramref name="innerKeySelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000510 RID: 1296 RVA: 0x00010F58 File Offset: 0x0000F158
		public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			if (outer == null)
			{
				throw Error.ArgumentNull("outer");
			}
			if (inner == null)
			{
				throw Error.ArgumentNull("inner");
			}
			if (outerKeySelector == null)
			{
				throw Error.ArgumentNull("outerKeySelector");
			}
			if (innerKeySelector == null)
			{
				throw Error.ArgumentNull("innerKeySelector");
			}
			if (resultSelector == null)
			{
				throw Error.ArgumentNull("resultSelector");
			}
			return Enumerable.GroupJoinIterator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00010FB9 File Offset: 0x0000F1B9
		private static IEnumerable<TResult> GroupJoinIterator<TOuter, TInner, TKey, TResult>(IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			using (IEnumerator<TOuter> e = outer.GetEnumerator())
			{
				if (e.MoveNext())
				{
					Lookup<TKey, TInner> lookup = Lookup<TKey, TInner>.CreateForJoin(inner, innerKeySelector, comparer);
					do
					{
						TOuter touter = e.Current;
						yield return resultSelector(touter, lookup[outerKeySelector(touter)]);
					}
					while (e.MoveNext());
					lookup = null;
				}
			}
			IEnumerator<TOuter> e = null;
			yield break;
			yield break;
		}

		/// <summary>Groups the elements of a sequence according to a specified key selector function.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements to group.</param>
		/// <param name="keySelector">A function to extract the key for each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>An IEnumerable&lt;IGrouping&lt;TKey, TSource&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TSource)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2" /> object contains a sequence of objects and a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000512 RID: 1298 RVA: 0x00010FEE File Offset: 0x0000F1EE
		public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return new GroupedEnumerable<TSource, TKey>(source, keySelector, null);
		}

		/// <summary>Groups the elements of a sequence according to a specified key selector function and compares the keys by using a specified comparer.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements to group.</param>
		/// <param name="keySelector">A function to extract the key for each element.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>An IEnumerable&lt;IGrouping&lt;TKey, TSource&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TSource)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2" /> object contains a collection of objects and a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000513 RID: 1299 RVA: 0x00010FF8 File Offset: 0x0000F1F8
		public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			return new GroupedEnumerable<TSource, TKey>(source, keySelector, comparer);
		}

		/// <summary>Groups the elements of a sequence according to a specified key selector function and projects the elements for each group by using a specified function.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements to group.</param>
		/// <param name="keySelector">A function to extract the key for each element.</param>
		/// <param name="elementSelector">A function to map each source element to an element in the <see cref="T:System.Linq.IGrouping`2" />.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <typeparam name="TElement">The type of the elements in the <see cref="T:System.Linq.IGrouping`2" />.</typeparam>
		/// <returns>An IEnumerable&lt;IGrouping&lt;TKey, TElement&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TElement)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2" /> object contains a collection of objects of type <paramref name="TElement" /> and a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> or <paramref name="elementSelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000514 RID: 1300 RVA: 0x00011002 File Offset: 0x0000F202
		public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return new GroupedEnumerable<TSource, TKey, TElement>(source, keySelector, elementSelector, null);
		}

		/// <summary>Groups the elements of a sequence according to a key selector function. The keys are compared by using a comparer and each group's elements are projected by using a specified function.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements to group.</param>
		/// <param name="keySelector">A function to extract the key for each element.</param>
		/// <param name="elementSelector">A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2" />.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <typeparam name="TElement">The type of the elements in the <see cref="T:System.Linq.IGrouping`2" />.</typeparam>
		/// <returns>An IEnumerable&lt;IGrouping&lt;TKey, TElement&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TElement)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2" /> object contains a collection of objects of type <paramref name="TElement" /> and a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> or <paramref name="elementSelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000515 RID: 1301 RVA: 0x0001100D File Offset: 0x0000F20D
		public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			return new GroupedEnumerable<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
		}

		/// <summary>Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements to group.</param>
		/// <param name="keySelector">A function to extract the key for each element.</param>
		/// <param name="resultSelector">A function to create a result value from each group.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector" />.</typeparam>
		/// <returns>A collection of elements of type <paramref name="TResult" /> where each element represents a projection over a group and its key.</returns>
		// Token: 0x06000516 RID: 1302 RVA: 0x00011018 File Offset: 0x0000F218
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
		{
			return new GroupedResultEnumerable<TSource, TKey, TResult>(source, keySelector, resultSelector, null);
		}

		/// <summary>Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The elements of each group are projected by using a specified function.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements to group.</param>
		/// <param name="keySelector">A function to extract the key for each element.</param>
		/// <param name="elementSelector">A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2" />.</param>
		/// <param name="resultSelector">A function to create a result value from each group.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <typeparam name="TElement">The type of the elements in each <see cref="T:System.Linq.IGrouping`2" />.</typeparam>
		/// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector" />.</typeparam>
		/// <returns>A collection of elements of type <paramref name="TResult" /> where each element represents a projection over a group and its key.</returns>
		// Token: 0x06000517 RID: 1303 RVA: 0x00011023 File Offset: 0x0000F223
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
		{
			return new GroupedResultEnumerable<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, null);
		}

		/// <summary>Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The keys are compared by using a specified comparer.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements to group.</param>
		/// <param name="keySelector">A function to extract the key for each element.</param>
		/// <param name="resultSelector">A function to create a result value from each group.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys with.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector" />.</typeparam>
		/// <returns>A collection of elements of type <paramref name="TResult" /> where each element represents a projection over a group and its key.</returns>
		// Token: 0x06000518 RID: 1304 RVA: 0x0001102F File Offset: 0x0000F22F
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			return new GroupedResultEnumerable<TSource, TKey, TResult>(source, keySelector, resultSelector, comparer);
		}

		/// <summary>Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. Key values are compared by using a specified comparer, and the elements of each group are projected by using a specified function.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements to group.</param>
		/// <param name="keySelector">A function to extract the key for each element.</param>
		/// <param name="elementSelector">A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2" />.</param>
		/// <param name="resultSelector">A function to create a result value from each group.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys with.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <typeparam name="TElement">The type of the elements in each <see cref="T:System.Linq.IGrouping`2" />.</typeparam>
		/// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector" />.</typeparam>
		/// <returns>A collection of elements of type <paramref name="TResult" /> where each element represents a projection over a group and its key.</returns>
		// Token: 0x06000519 RID: 1305 RVA: 0x0001103A File Offset: 0x0000F23A
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			return new GroupedResultEnumerable<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, comparer);
		}

		/// <summary>Produces the set intersection of two sequences by using the default equality comparer to compare values.</summary>
		/// <param name="first">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements that also appear in <paramref name="second" /> will be returned.</param>
		/// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements that also appear in the first sequence will be returned.</param>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
		// Token: 0x0600051A RID: 1306 RVA: 0x00011047 File Offset: 0x0000F247
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null)
			{
				throw Error.ArgumentNull("first");
			}
			if (second == null)
			{
				throw Error.ArgumentNull("second");
			}
			return Enumerable.IntersectIterator<TSource>(first, second, null);
		}

		/// <summary>Produces the set intersection of two sequences by using the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</summary>
		/// <param name="first">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements that also appear in <paramref name="second" /> will be returned.</param>
		/// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements that also appear in the first sequence will be returned.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</param>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
		// Token: 0x0600051B RID: 1307 RVA: 0x0001106D File Offset: 0x0000F26D
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			if (first == null)
			{
				throw Error.ArgumentNull("first");
			}
			if (second == null)
			{
				throw Error.ArgumentNull("second");
			}
			return Enumerable.IntersectIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00011093 File Offset: 0x0000F293
		private static IEnumerable<TSource> IntersectIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Set<TSource> set = new Set<TSource>(comparer);
			foreach (TSource tsource in second)
			{
				set.Add(tsource);
			}
			foreach (TSource tsource2 in first)
			{
				if (set.Remove(tsource2))
				{
					yield return tsource2;
				}
			}
			IEnumerator<TSource> enumerator2 = null;
			yield break;
			yield break;
		}

		/// <summary>Correlates the elements of two sequences based on matching keys. The default equality comparer is used to compare keys.</summary>
		/// <param name="outer">The first sequence to join.</param>
		/// <param name="inner">The sequence to join to the first sequence.</param>
		/// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
		/// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
		/// <param name="resultSelector">A function to create a result element from two matching elements.</param>
		/// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
		/// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
		/// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
		/// <typeparam name="TResult">The type of the result elements.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that has elements of type <paramref name="TResult" /> that are obtained by performing an inner join on two sequences.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="outer" /> or <paramref name="inner" /> or <paramref name="outerKeySelector" /> or <paramref name="innerKeySelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
		// Token: 0x0600051D RID: 1309 RVA: 0x000110B4 File Offset: 0x0000F2B4
		public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
		{
			if (outer == null)
			{
				throw Error.ArgumentNull("outer");
			}
			if (inner == null)
			{
				throw Error.ArgumentNull("inner");
			}
			if (outerKeySelector == null)
			{
				throw Error.ArgumentNull("outerKeySelector");
			}
			if (innerKeySelector == null)
			{
				throw Error.ArgumentNull("innerKeySelector");
			}
			if (resultSelector == null)
			{
				throw Error.ArgumentNull("resultSelector");
			}
			return Enumerable.JoinIterator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, null);
		}

		/// <summary>Correlates the elements of two sequences based on matching keys. A specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> is used to compare keys.</summary>
		/// <param name="outer">The first sequence to join.</param>
		/// <param name="inner">The sequence to join to the first sequence.</param>
		/// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
		/// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
		/// <param name="resultSelector">A function to create a result element from two matching elements.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to hash and compare keys.</param>
		/// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
		/// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
		/// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
		/// <typeparam name="TResult">The type of the result elements.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that has elements of type <paramref name="TResult" /> that are obtained by performing an inner join on two sequences.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="outer" /> or <paramref name="inner" /> or <paramref name="outerKeySelector" /> or <paramref name="innerKeySelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
		// Token: 0x0600051E RID: 1310 RVA: 0x00011114 File Offset: 0x0000F314
		public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			if (outer == null)
			{
				throw Error.ArgumentNull("outer");
			}
			if (inner == null)
			{
				throw Error.ArgumentNull("inner");
			}
			if (outerKeySelector == null)
			{
				throw Error.ArgumentNull("outerKeySelector");
			}
			if (innerKeySelector == null)
			{
				throw Error.ArgumentNull("innerKeySelector");
			}
			if (resultSelector == null)
			{
				throw Error.ArgumentNull("resultSelector");
			}
			return Enumerable.JoinIterator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00011175 File Offset: 0x0000F375
		private static IEnumerable<TResult> JoinIterator<TOuter, TInner, TKey, TResult>(IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			using (IEnumerator<TOuter> e = outer.GetEnumerator())
			{
				if (e.MoveNext())
				{
					Lookup<TKey, TInner> lookup = Lookup<TKey, TInner>.CreateForJoin(inner, innerKeySelector, comparer);
					if (lookup.Count != 0)
					{
						do
						{
							TOuter item = e.Current;
							Grouping<TKey, TInner> grouping = lookup.GetGrouping(outerKeySelector(item), false);
							if (grouping != null)
							{
								int count = grouping._count;
								TInner[] elements = grouping._elements;
								int num;
								for (int i = 0; i != count; i = num)
								{
									yield return resultSelector(item, elements[i]);
									num = i + 1;
								}
								elements = null;
							}
							item = default(TOuter);
						}
						while (e.MoveNext());
					}
					lookup = null;
				}
			}
			IEnumerator<TOuter> e = null;
			yield break;
			yield break;
		}

		/// <summary>Returns the last element of a sequence.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return the last element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The value at the last position in the source sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The source sequence is empty.</exception>
		// Token: 0x06000520 RID: 1312 RVA: 0x000111AC File Offset: 0x0000F3AC
		public static TSource Last<TSource>(this IEnumerable<TSource> source)
		{
			bool flag;
			TSource tsource = source.TryGetLast<TSource>(out flag);
			if (!flag)
			{
				throw Error.NoElements();
			}
			return tsource;
		}

		/// <summary>Returns the last element of a sequence that satisfies a specified condition.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return an element from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The last element in the sequence that passes the test in the specified predicate function.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate" />.-or-The source sequence is empty.</exception>
		// Token: 0x06000521 RID: 1313 RVA: 0x000111CC File Offset: 0x0000F3CC
		public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			bool flag;
			TSource tsource = source.TryGetLast<TSource>(predicate, out flag);
			if (!flag)
			{
				throw Error.NoMatch();
			}
			return tsource;
		}

		/// <summary>Returns the last element of a sequence, or a default value if the sequence contains no elements.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return the last element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>
		///     <see langword="default" />(<paramref name="TSource" />) if the source sequence is empty; otherwise, the last element in the <see cref="T:System.Collections.Generic.IEnumerable`1" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000522 RID: 1314 RVA: 0x000111EC File Offset: 0x0000F3EC
		public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source)
		{
			bool flag;
			return source.TryGetLast<TSource>(out flag);
		}

		/// <summary>Returns the last element of a sequence that satisfies a condition or a default value if no such element is found.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return an element from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>
		///     <see langword="default" />(<paramref name="TSource" />) if the sequence is empty or if no elements pass the test in the predicate function; otherwise, the last element that passes the test in the predicate function.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		// Token: 0x06000523 RID: 1315 RVA: 0x00011204 File Offset: 0x0000F404
		public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			bool flag;
			return source.TryGetLast<TSource>(predicate, out flag);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001121C File Offset: 0x0000F41C
		private static TSource TryGetLast<TSource>(this IEnumerable<TSource> source, out bool found)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			IPartition<TSource> partition = source as IPartition<TSource>;
			if (partition != null)
			{
				return partition.TryGetLast(out found);
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				int count = list.Count;
				if (count > 0)
				{
					found = true;
					return list[count - 1];
				}
			}
			else
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						TSource tsource;
						do
						{
							tsource = enumerator.Current;
						}
						while (enumerator.MoveNext());
						found = true;
						return tsource;
					}
				}
			}
			found = false;
			return default(TSource);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000112C0 File Offset: 0x0000F4C0
		private static TSource TryGetLast<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out bool found)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			OrderedEnumerable<TSource> orderedEnumerable = source as OrderedEnumerable<TSource>;
			if (orderedEnumerable != null)
			{
				return orderedEnumerable.TryGetLast(predicate, out found);
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					TSource tsource = list[i];
					if (predicate(tsource))
					{
						found = true;
						return tsource;
					}
				}
			}
			else
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						if (predicate(tsource2))
						{
							while (enumerator.MoveNext())
							{
								TSource tsource3 = enumerator.Current;
								if (predicate(tsource3))
								{
									tsource2 = tsource3;
								}
							}
							found = true;
							return tsource2;
						}
					}
				}
			}
			found = false;
			return default(TSource);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Lookup`2" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> according to a specified key selector function.</summary>
		/// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Linq.Lookup`2" /> from.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>A <see cref="T:System.Linq.Lookup`2" /> that contains keys and values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000526 RID: 1318 RVA: 0x000113B0 File Offset: 0x0000F5B0
		public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ToLookup<TSource, TKey>(keySelector, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Lookup`2" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> according to a specified key selector function and key comparer.</summary>
		/// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Linq.Lookup`2" /> from.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>A <see cref="T:System.Linq.Lookup`2" /> that contains keys and values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000527 RID: 1319 RVA: 0x000113BA File Offset: 0x0000F5BA
		public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (keySelector == null)
			{
				throw Error.ArgumentNull("keySelector");
			}
			return Lookup<TKey, TSource>.Create(source, keySelector, comparer);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Lookup`2" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> according to specified key selector and element selector functions.</summary>
		/// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Linq.Lookup`2" /> from.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <typeparam name="TElement">The type of the value returned by <paramref name="elementSelector" />.</typeparam>
		/// <returns>A <see cref="T:System.Linq.Lookup`2" /> that contains values of type <paramref name="TElement" /> selected from the input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> or <paramref name="elementSelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000528 RID: 1320 RVA: 0x000113E0 File Offset: 0x0000F5E0
		public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.ToLookup<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Lookup`2" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> according to a specified key selector function, a comparer and an element selector function.</summary>
		/// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Linq.Lookup`2" /> from.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <typeparam name="TElement">The type of the value returned by <paramref name="elementSelector" />.</typeparam>
		/// <returns>A <see cref="T:System.Linq.Lookup`2" /> that contains values of type <paramref name="TElement" /> selected from the input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> or <paramref name="elementSelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000529 RID: 1321 RVA: 0x000113EB File Offset: 0x0000F5EB
		public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (keySelector == null)
			{
				throw Error.ArgumentNull("keySelector");
			}
			if (elementSelector == null)
			{
				throw Error.ArgumentNull("elementSelector");
			}
			return Lookup<TKey, TElement>.Create<TSource>(source, keySelector, elementSelector, comparer);
		}

		/// <summary>Returns the maximum value in a sequence of <see cref="T:System.Int32" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Int32" /> values to determine the maximum value of.</param>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x0600052A RID: 1322 RVA: 0x00011420 File Offset: 0x0000F620
		public static int Max(this IEnumerable<int> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			int num;
			using (IEnumerator<int> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = enumerator.Current;
				while (enumerator.MoveNext())
				{
					int num2 = enumerator.Current;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Returns the maximum value in a sequence of nullable <see cref="T:System.Int32" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int32" /> values to determine the maximum value of.</param>
		/// <returns>A value of type Nullable&lt;Int32&gt; in C# or Nullable(Of Int32) in Visual Basic that corresponds to the maximum value in the sequence. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600052B RID: 1323 RVA: 0x0001148C File Offset: 0x0000F68C
		public static int? Max(this IEnumerable<int?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			int? num = null;
			using (IEnumerator<int?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num = enumerator.Current;
					if (num != null)
					{
						int num2 = num.GetValueOrDefault();
						if (num2 >= 0)
						{
							while (enumerator.MoveNext())
							{
								int? num3 = enumerator.Current;
								int valueOrDefault = num3.GetValueOrDefault();
								if (valueOrDefault > num2)
								{
									num2 = valueOrDefault;
									num = num3;
								}
							}
							return num;
						}
						while (enumerator.MoveNext())
						{
							int? num4 = enumerator.Current;
							int valueOrDefault2 = num4.GetValueOrDefault();
							if ((num4 != null) & (valueOrDefault2 > num2))
							{
								num2 = valueOrDefault2;
								num = num4;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Returns the maximum value in a sequence of <see cref="T:System.Int64" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Int64" /> values to determine the maximum value of.</param>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x0600052C RID: 1324 RVA: 0x00011558 File Offset: 0x0000F758
		public static long Max(this IEnumerable<long> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			long num;
			using (IEnumerator<long> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = enumerator.Current;
				while (enumerator.MoveNext())
				{
					long num2 = enumerator.Current;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Returns the maximum value in a sequence of nullable <see cref="T:System.Int64" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int64" /> values to determine the maximum value of.</param>
		/// <returns>A value of type Nullable&lt;Int64&gt; in C# or Nullable(Of Int64) in Visual Basic that corresponds to the maximum value in the sequence. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600052D RID: 1325 RVA: 0x000115C4 File Offset: 0x0000F7C4
		public static long? Max(this IEnumerable<long?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			long? num = null;
			using (IEnumerator<long?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num = enumerator.Current;
					if (num != null)
					{
						long num2 = num.GetValueOrDefault();
						if (num2 >= 0L)
						{
							while (enumerator.MoveNext())
							{
								long? num3 = enumerator.Current;
								long valueOrDefault = num3.GetValueOrDefault();
								if (valueOrDefault > num2)
								{
									num2 = valueOrDefault;
									num = num3;
								}
							}
							return num;
						}
						while (enumerator.MoveNext())
						{
							long? num4 = enumerator.Current;
							long valueOrDefault2 = num4.GetValueOrDefault();
							if ((num4 != null) & (valueOrDefault2 > num2))
							{
								num2 = valueOrDefault2;
								num = num4;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Returns the maximum value in a sequence of <see cref="T:System.Double" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Double" /> values to determine the maximum value of.</param>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x0600052E RID: 1326 RVA: 0x00011690 File Offset: 0x0000F890
		public static double Max(this IEnumerable<double> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			double num;
			using (IEnumerator<double> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = enumerator.Current;
				while (double.IsNaN(num))
				{
					if (!enumerator.MoveNext())
					{
						return num;
					}
					num = enumerator.Current;
				}
				while (enumerator.MoveNext())
				{
					double num2 = enumerator.Current;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Returns the maximum value in a sequence of nullable <see cref="T:System.Double" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Double" /> values to determine the maximum value of.</param>
		/// <returns>A value of type Nullable&lt;Double&gt; in C# or Nullable(Of Double) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600052F RID: 1327 RVA: 0x0001171C File Offset: 0x0000F91C
		public static double? Max(this IEnumerable<double?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			double? num = null;
			using (IEnumerator<double?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num = enumerator.Current;
					if (num != null)
					{
						double num2 = num.GetValueOrDefault();
						while (double.IsNaN(num2))
						{
							if (!enumerator.MoveNext())
							{
								return num;
							}
							double? num3 = enumerator.Current;
							if (num3 != null)
							{
								double? num4;
								num = (num4 = num3);
								num2 = num4.GetValueOrDefault();
							}
						}
						while (enumerator.MoveNext())
						{
							double? num5 = enumerator.Current;
							double valueOrDefault = num5.GetValueOrDefault();
							if ((num5 != null) & (valueOrDefault > num2))
							{
								num2 = valueOrDefault;
								num = num5;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Returns the maximum value in a sequence of <see cref="T:System.Single" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Single" /> values to determine the maximum value of.</param>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000530 RID: 1328 RVA: 0x000117F4 File Offset: 0x0000F9F4
		public static float Max(this IEnumerable<float> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			float num;
			using (IEnumerator<float> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = enumerator.Current;
				while (float.IsNaN(num))
				{
					if (!enumerator.MoveNext())
					{
						return num;
					}
					num = enumerator.Current;
				}
				while (enumerator.MoveNext())
				{
					float num2 = enumerator.Current;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Returns the maximum value in a sequence of nullable <see cref="T:System.Single" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Single" /> values to determine the maximum value of.</param>
		/// <returns>A value of type Nullable&lt;Single&gt; in C# or Nullable(Of Single) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000531 RID: 1329 RVA: 0x00011880 File Offset: 0x0000FA80
		public static float? Max(this IEnumerable<float?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			float? num = null;
			using (IEnumerator<float?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num = enumerator.Current;
					if (num != null)
					{
						float num2 = num.GetValueOrDefault();
						while (float.IsNaN(num2))
						{
							if (!enumerator.MoveNext())
							{
								return num;
							}
							float? num3 = enumerator.Current;
							if (num3 != null)
							{
								float? num4;
								num = (num4 = num3);
								num2 = num4.GetValueOrDefault();
							}
						}
						while (enumerator.MoveNext())
						{
							float? num5 = enumerator.Current;
							float valueOrDefault = num5.GetValueOrDefault();
							if ((num5 != null) & (valueOrDefault > num2))
							{
								num2 = valueOrDefault;
								num = num5;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Returns the maximum value in a sequence of <see cref="T:System.Decimal" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Decimal" /> values to determine the maximum value of.</param>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000532 RID: 1330 RVA: 0x00011958 File Offset: 0x0000FB58
		public static decimal Max(this IEnumerable<decimal> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			decimal num;
			using (IEnumerator<decimal> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = enumerator.Current;
				while (enumerator.MoveNext())
				{
					decimal num2 = enumerator.Current;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Returns the maximum value in a sequence of nullable <see cref="T:System.Decimal" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Decimal" /> values to determine the maximum value of.</param>
		/// <returns>A value of type Nullable&lt;Decimal&gt; in C# or Nullable(Of Decimal) in Visual Basic that corresponds to the maximum value in the sequence. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000533 RID: 1331 RVA: 0x000119C8 File Offset: 0x0000FBC8
		public static decimal? Max(this IEnumerable<decimal?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			decimal? num = null;
			using (IEnumerator<decimal?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num = enumerator.Current;
					if (num != null)
					{
						decimal num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							decimal? num3 = enumerator.Current;
							decimal valueOrDefault = num3.GetValueOrDefault();
							if (num3 != null && valueOrDefault > num2)
							{
								num2 = valueOrDefault;
								num = num3;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Returns the maximum value in a generic sequence.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000534 RID: 1332 RVA: 0x00011A6C File Offset: 0x0000FC6C
		public static TSource Max<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			Comparer<TSource> @default = Comparer<TSource>.Default;
			TSource tsource = default(TSource);
			if (tsource == null)
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						tsource = enumerator.Current;
						if (tsource != null)
						{
							while (enumerator.MoveNext())
							{
								TSource tsource2 = enumerator.Current;
								if (tsource2 != null && @default.Compare(tsource2, tsource) > 0)
								{
									tsource = tsource2;
								}
							}
							return tsource;
						}
					}
					return tsource;
				}
			}
			using (IEnumerator<TSource> enumerator2 = source.GetEnumerator())
			{
				if (!enumerator2.MoveNext())
				{
					throw Error.NoElements();
				}
				tsource = enumerator2.Current;
				while (enumerator2.MoveNext())
				{
					TSource tsource3 = enumerator2.Current;
					if (@default.Compare(tsource3, tsource) > 0)
					{
						tsource = tsource3;
					}
				}
			}
			return tsource;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the maximum <see cref="T:System.Int32" /> value.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000535 RID: 1333 RVA: 0x00011B68 File Offset: 0x0000FD68
		public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			int num;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = selector(enumerator.Current);
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					int num2 = selector(tsource);
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="T:System.Int32" /> value.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The value of type Nullable&lt;Int32&gt; in C# or Nullable(Of Int32) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000536 RID: 1334 RVA: 0x00011BF0 File Offset: 0x0000FDF0
		public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			int? num = null;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num = selector(tsource);
					if (num != null)
					{
						int num2 = num.GetValueOrDefault();
						if (num2 >= 0)
						{
							while (enumerator.MoveNext())
							{
								TSource tsource2 = enumerator.Current;
								int? num3 = selector(tsource2);
								int valueOrDefault = num3.GetValueOrDefault();
								if (valueOrDefault > num2)
								{
									num2 = valueOrDefault;
									num = num3;
								}
							}
							return num;
						}
						while (enumerator.MoveNext())
						{
							TSource tsource3 = enumerator.Current;
							int? num4 = selector(tsource3);
							int valueOrDefault2 = num4.GetValueOrDefault();
							if ((num4 != null) & (valueOrDefault2 > num2))
							{
								num2 = valueOrDefault2;
								num = num4;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the maximum <see cref="T:System.Int64" /> value.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000537 RID: 1335 RVA: 0x00011CDC File Offset: 0x0000FEDC
		public static long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			long num;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = selector(enumerator.Current);
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					long num2 = selector(tsource);
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="T:System.Int64" /> value.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The value of type Nullable&lt;Int64&gt; in C# or Nullable(Of Int64) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000538 RID: 1336 RVA: 0x00011D64 File Offset: 0x0000FF64
		public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			long? num = null;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num = selector(tsource);
					if (num != null)
					{
						long num2 = num.GetValueOrDefault();
						if (num2 >= 0L)
						{
							while (enumerator.MoveNext())
							{
								TSource tsource2 = enumerator.Current;
								long? num3 = selector(tsource2);
								long valueOrDefault = num3.GetValueOrDefault();
								if (valueOrDefault > num2)
								{
									num2 = valueOrDefault;
									num = num3;
								}
							}
							return num;
						}
						while (enumerator.MoveNext())
						{
							TSource tsource3 = enumerator.Current;
							long? num4 = selector(tsource3);
							long valueOrDefault2 = num4.GetValueOrDefault();
							if ((num4 != null) & (valueOrDefault2 > num2))
							{
								num2 = valueOrDefault2;
								num = num4;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the maximum <see cref="T:System.Single" /> value.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000539 RID: 1337 RVA: 0x00011E50 File Offset: 0x00010050
		public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			float num;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = selector(enumerator.Current);
				while (float.IsNaN(num))
				{
					if (!enumerator.MoveNext())
					{
						return num;
					}
					num = selector(enumerator.Current);
				}
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					float num2 = selector(tsource);
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="T:System.Single" /> value.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The value of type Nullable&lt;Single&gt; in C# or Nullable(Of Single) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x0600053A RID: 1338 RVA: 0x00011EFC File Offset: 0x000100FC
		public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			float? num = null;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num = selector(tsource);
					if (num != null)
					{
						float num2 = num.GetValueOrDefault();
						while (float.IsNaN(num2))
						{
							if (!enumerator.MoveNext())
							{
								return num;
							}
							float? num3 = selector(enumerator.Current);
							if (num3 != null)
							{
								float? num4;
								num = (num4 = num3);
								num2 = num4.GetValueOrDefault();
							}
						}
						while (enumerator.MoveNext())
						{
							TSource tsource2 = enumerator.Current;
							float? num5 = selector(tsource2);
							float valueOrDefault = num5.GetValueOrDefault();
							if ((num5 != null) & (valueOrDefault > num2))
							{
								num2 = valueOrDefault;
								num = num5;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the maximum <see cref="T:System.Double" /> value.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x0600053B RID: 1339 RVA: 0x00011FF4 File Offset: 0x000101F4
		public static double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			double num;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = selector(enumerator.Current);
				while (double.IsNaN(num))
				{
					if (!enumerator.MoveNext())
					{
						return num;
					}
					num = selector(enumerator.Current);
				}
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					double num2 = selector(tsource);
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="T:System.Double" /> value.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The value of type Nullable&lt;Double&gt; in C# or Nullable(Of Double) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x0600053C RID: 1340 RVA: 0x000120A0 File Offset: 0x000102A0
		public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			double? num = null;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num = selector(tsource);
					if (num != null)
					{
						double num2 = num.GetValueOrDefault();
						while (double.IsNaN(num2))
						{
							if (!enumerator.MoveNext())
							{
								return num;
							}
							double? num3 = selector(enumerator.Current);
							if (num3 != null)
							{
								double? num4;
								num = (num4 = num3);
								num2 = num4.GetValueOrDefault();
							}
						}
						while (enumerator.MoveNext())
						{
							TSource tsource2 = enumerator.Current;
							double? num5 = selector(tsource2);
							double valueOrDefault = num5.GetValueOrDefault();
							if ((num5 != null) & (valueOrDefault > num2))
							{
								num2 = valueOrDefault;
								num = num5;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the maximum <see cref="T:System.Decimal" /> value.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x0600053D RID: 1341 RVA: 0x00012198 File Offset: 0x00010398
		public static decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			decimal num;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = selector(enumerator.Current);
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					decimal num2 = selector(tsource);
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="T:System.Decimal" /> value.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The value of type Nullable&lt;Decimal&gt; in C# or Nullable(Of Decimal) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x0600053E RID: 1342 RVA: 0x00012224 File Offset: 0x00010424
		public static decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			decimal? num = null;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num = selector(tsource);
					if (num != null)
					{
						decimal num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							TSource tsource2 = enumerator.Current;
							decimal? num3 = selector(tsource2);
							decimal valueOrDefault = num3.GetValueOrDefault();
							if (num3 != null && valueOrDefault > num2)
							{
								num2 = valueOrDefault;
								num = num3;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a generic sequence and returns the maximum resulting value.</summary>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
		/// <returns>The maximum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x0600053F RID: 1343 RVA: 0x000122E0 File Offset: 0x000104E0
		public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			Comparer<TResult> @default = Comparer<TResult>.Default;
			TResult tresult = default(TResult);
			if (tresult == null)
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TSource tsource = enumerator.Current;
						tresult = selector(tsource);
						if (tresult != null)
						{
							while (enumerator.MoveNext())
							{
								TSource tsource2 = enumerator.Current;
								TResult tresult2 = selector(tsource2);
								if (tresult2 != null && @default.Compare(tresult2, tresult) > 0)
								{
									tresult = tresult2;
								}
							}
							return tresult;
						}
					}
					return tresult;
				}
			}
			using (IEnumerator<TSource> enumerator2 = source.GetEnumerator())
			{
				if (!enumerator2.MoveNext())
				{
					throw Error.NoElements();
				}
				tresult = selector(enumerator2.Current);
				while (enumerator2.MoveNext())
				{
					TSource tsource3 = enumerator2.Current;
					TResult tresult3 = selector(tsource3);
					if (@default.Compare(tresult3, tresult) > 0)
					{
						tresult = tresult3;
					}
				}
			}
			return tresult;
		}

		/// <summary>Returns the minimum value in a sequence of <see cref="T:System.Int32" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Int32" /> values to determine the minimum value of.</param>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000540 RID: 1344 RVA: 0x00012404 File Offset: 0x00010604
		public static int Min(this IEnumerable<int> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			int num;
			using (IEnumerator<int> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = enumerator.Current;
				while (enumerator.MoveNext())
				{
					int num2 = enumerator.Current;
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Returns the minimum value in a sequence of nullable <see cref="T:System.Int32" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int32" /> values to determine the minimum value of.</param>
		/// <returns>A value of type Nullable&lt;Int32&gt; in C# or Nullable(Of Int32) in Visual Basic that corresponds to the minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000541 RID: 1345 RVA: 0x00012470 File Offset: 0x00010670
		public static int? Min(this IEnumerable<int?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			int? num = null;
			using (IEnumerator<int?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num = enumerator.Current;
					if (num != null)
					{
						int num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							int? num3 = enumerator.Current;
							int valueOrDefault = num3.GetValueOrDefault();
							if ((num3 != null) & (valueOrDefault < num2))
							{
								num2 = valueOrDefault;
								num = num3;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Returns the minimum value in a sequence of <see cref="T:System.Int64" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Int64" /> values to determine the minimum value of.</param>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000542 RID: 1346 RVA: 0x00012510 File Offset: 0x00010710
		public static long Min(this IEnumerable<long> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			long num;
			using (IEnumerator<long> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = enumerator.Current;
				while (enumerator.MoveNext())
				{
					long num2 = enumerator.Current;
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Returns the minimum value in a sequence of nullable <see cref="T:System.Int64" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int64" /> values to determine the minimum value of.</param>
		/// <returns>A value of type Nullable&lt;Int64&gt; in C# or Nullable(Of Int64) in Visual Basic that corresponds to the minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000543 RID: 1347 RVA: 0x0001257C File Offset: 0x0001077C
		public static long? Min(this IEnumerable<long?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			long? num = null;
			using (IEnumerator<long?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num = enumerator.Current;
					if (num != null)
					{
						long num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							long? num3 = enumerator.Current;
							long valueOrDefault = num3.GetValueOrDefault();
							if ((num3 != null) & (valueOrDefault < num2))
							{
								num2 = valueOrDefault;
								num = num3;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Returns the minimum value in a sequence of <see cref="T:System.Single" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Single" /> values to determine the minimum value of.</param>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000544 RID: 1348 RVA: 0x0001261C File Offset: 0x0001081C
		public static float Min(this IEnumerable<float> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			float num;
			using (IEnumerator<float> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = enumerator.Current;
				while (enumerator.MoveNext())
				{
					float num2 = enumerator.Current;
					if (num2 < num)
					{
						num = num2;
					}
					else if (float.IsNaN(num2))
					{
						return num2;
					}
				}
			}
			return num;
		}

		/// <summary>Returns the minimum value in a sequence of nullable <see cref="T:System.Single" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Single" /> values to determine the minimum value of.</param>
		/// <returns>A value of type Nullable&lt;Single&gt; in C# or Nullable(Of Single) in Visual Basic that corresponds to the minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000545 RID: 1349 RVA: 0x00012698 File Offset: 0x00010898
		public static float? Min(this IEnumerable<float?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			float? num = null;
			using (IEnumerator<float?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num = enumerator.Current;
					if (num != null)
					{
						float num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							float? num3 = enumerator.Current;
							if (num3 != null)
							{
								float valueOrDefault = num3.GetValueOrDefault();
								if (valueOrDefault < num2)
								{
									num2 = valueOrDefault;
									num = num3;
								}
								else if (float.IsNaN(valueOrDefault))
								{
									return num3;
								}
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Returns the minimum value in a sequence of <see cref="T:System.Double" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Double" /> values to determine the minimum value of.</param>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000546 RID: 1350 RVA: 0x00012744 File Offset: 0x00010944
		public static double Min(this IEnumerable<double> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			double num;
			using (IEnumerator<double> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = enumerator.Current;
				while (enumerator.MoveNext())
				{
					double num2 = enumerator.Current;
					if (num2 < num)
					{
						num = num2;
					}
					else if (double.IsNaN(num2))
					{
						return num2;
					}
				}
			}
			return num;
		}

		/// <summary>Returns the minimum value in a sequence of nullable <see cref="T:System.Double" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Double" /> values to determine the minimum value of.</param>
		/// <returns>A value of type Nullable&lt;Double&gt; in C# or Nullable(Of Double) in Visual Basic that corresponds to the minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000547 RID: 1351 RVA: 0x000127C0 File Offset: 0x000109C0
		public static double? Min(this IEnumerable<double?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			double? num = null;
			using (IEnumerator<double?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num = enumerator.Current;
					if (num != null)
					{
						double num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							double? num3 = enumerator.Current;
							if (num3 != null)
							{
								double valueOrDefault = num3.GetValueOrDefault();
								if (valueOrDefault < num2)
								{
									num2 = valueOrDefault;
									num = num3;
								}
								else if (double.IsNaN(valueOrDefault))
								{
									return num3;
								}
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Returns the minimum value in a sequence of <see cref="T:System.Decimal" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Decimal" /> values to determine the minimum value of.</param>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000548 RID: 1352 RVA: 0x0001286C File Offset: 0x00010A6C
		public static decimal Min(this IEnumerable<decimal> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			decimal num;
			using (IEnumerator<decimal> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = enumerator.Current;
				while (enumerator.MoveNext())
				{
					decimal num2 = enumerator.Current;
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Returns the minimum value in a sequence of nullable <see cref="T:System.Decimal" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Decimal" /> values to determine the minimum value of.</param>
		/// <returns>A value of type Nullable&lt;Decimal&gt; in C# or Nullable(Of Decimal) in Visual Basic that corresponds to the minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000549 RID: 1353 RVA: 0x000128DC File Offset: 0x00010ADC
		public static decimal? Min(this IEnumerable<decimal?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			decimal? num = null;
			using (IEnumerator<decimal?> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num = enumerator.Current;
					if (num != null)
					{
						decimal num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							decimal? num3 = enumerator.Current;
							decimal valueOrDefault = num3.GetValueOrDefault();
							if (num3 != null && valueOrDefault < num2)
							{
								num2 = valueOrDefault;
								num = num3;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Returns the minimum value in a generic sequence.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600054A RID: 1354 RVA: 0x00012980 File Offset: 0x00010B80
		public static TSource Min<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			Comparer<TSource> @default = Comparer<TSource>.Default;
			TSource tsource = default(TSource);
			if (tsource == null)
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						tsource = enumerator.Current;
						if (tsource != null)
						{
							while (enumerator.MoveNext())
							{
								TSource tsource2 = enumerator.Current;
								if (tsource2 != null && @default.Compare(tsource2, tsource) < 0)
								{
									tsource = tsource2;
								}
							}
							return tsource;
						}
					}
					return tsource;
				}
			}
			using (IEnumerator<TSource> enumerator2 = source.GetEnumerator())
			{
				if (!enumerator2.MoveNext())
				{
					throw Error.NoElements();
				}
				tsource = enumerator2.Current;
				while (enumerator2.MoveNext())
				{
					TSource tsource3 = enumerator2.Current;
					if (@default.Compare(tsource3, tsource) < 0)
					{
						tsource = tsource3;
					}
				}
			}
			return tsource;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the minimum <see cref="T:System.Int32" /> value.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x0600054B RID: 1355 RVA: 0x00012A7C File Offset: 0x00010C7C
		public static int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			int num;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = selector(enumerator.Current);
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					int num2 = selector(tsource);
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the minimum nullable <see cref="T:System.Int32" /> value.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The value of type Nullable&lt;Int32&gt; in C# or Nullable(Of Int32) in Visual Basic that corresponds to the minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x0600054C RID: 1356 RVA: 0x00012B04 File Offset: 0x00010D04
		public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			int? num = null;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num = selector(tsource);
					if (num != null)
					{
						int num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							TSource tsource2 = enumerator.Current;
							int? num3 = selector(tsource2);
							int valueOrDefault = num3.GetValueOrDefault();
							if ((num3 != null) & (valueOrDefault < num2))
							{
								num2 = valueOrDefault;
								num = num3;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the minimum <see cref="T:System.Int64" /> value.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x0600054D RID: 1357 RVA: 0x00012BBC File Offset: 0x00010DBC
		public static long Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			long num;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = selector(enumerator.Current);
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					long num2 = selector(tsource);
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the minimum nullable <see cref="T:System.Int64" /> value.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The value of type Nullable&lt;Int64&gt; in C# or Nullable(Of Int64) in Visual Basic that corresponds to the minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x0600054E RID: 1358 RVA: 0x00012C44 File Offset: 0x00010E44
		public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			long? num = null;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num = selector(tsource);
					if (num != null)
					{
						long num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							TSource tsource2 = enumerator.Current;
							long? num3 = selector(tsource2);
							long valueOrDefault = num3.GetValueOrDefault();
							if ((num3 != null) & (valueOrDefault < num2))
							{
								num2 = valueOrDefault;
								num = num3;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the minimum <see cref="T:System.Single" /> value.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x0600054F RID: 1359 RVA: 0x00012CFC File Offset: 0x00010EFC
		public static float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			float num;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = selector(enumerator.Current);
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					float num2 = selector(tsource);
					if (num2 < num)
					{
						num = num2;
					}
					else if (float.IsNaN(num2))
					{
						return num2;
					}
				}
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the minimum nullable <see cref="T:System.Single" /> value.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The value of type Nullable&lt;Single&gt; in C# or Nullable(Of Single) in Visual Basic that corresponds to the minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000550 RID: 1360 RVA: 0x00012D94 File Offset: 0x00010F94
		public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			float? num = null;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num = selector(tsource);
					if (num != null)
					{
						float num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							TSource tsource2 = enumerator.Current;
							float? num3 = selector(tsource2);
							if (num3 != null)
							{
								float valueOrDefault = num3.GetValueOrDefault();
								if (valueOrDefault < num2)
								{
									num2 = valueOrDefault;
									num = num3;
								}
								else if (float.IsNaN(valueOrDefault))
								{
									return num3;
								}
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the minimum <see cref="T:System.Double" /> value.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000551 RID: 1361 RVA: 0x00012E5C File Offset: 0x0001105C
		public static double Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			double num;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = selector(enumerator.Current);
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					double num2 = selector(tsource);
					if (num2 < num)
					{
						num = num2;
					}
					else if (double.IsNaN(num2))
					{
						return num2;
					}
				}
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the minimum nullable <see cref="T:System.Double" /> value.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The value of type Nullable&lt;Double&gt; in C# or Nullable(Of Double) in Visual Basic that corresponds to the minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000552 RID: 1362 RVA: 0x00012EF4 File Offset: 0x000110F4
		public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			double? num = null;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num = selector(tsource);
					if (num != null)
					{
						double num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							TSource tsource2 = enumerator.Current;
							double? num3 = selector(tsource2);
							if (num3 != null)
							{
								double valueOrDefault = num3.GetValueOrDefault();
								if (valueOrDefault < num2)
								{
									num2 = valueOrDefault;
									num = num3;
								}
								else if (double.IsNaN(valueOrDefault))
								{
									return num3;
								}
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the minimum <see cref="T:System.Decimal" /> value.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="source" /> contains no elements.</exception>
		// Token: 0x06000553 RID: 1363 RVA: 0x00012FBC File Offset: 0x000111BC
		public static decimal Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			decimal num;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Error.NoElements();
				}
				num = selector(enumerator.Current);
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					decimal num2 = selector(tsource);
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a sequence and returns the minimum nullable <see cref="T:System.Decimal" /> value.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The value of type Nullable&lt;Decimal&gt; in C# or Nullable(Of Decimal) in Visual Basic that corresponds to the minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000554 RID: 1364 RVA: 0x00013048 File Offset: 0x00011248
		public static decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			decimal? num = null;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					num = selector(tsource);
					if (num != null)
					{
						decimal num2 = num.GetValueOrDefault();
						while (enumerator.MoveNext())
						{
							TSource tsource2 = enumerator.Current;
							decimal? num3 = selector(tsource2);
							decimal valueOrDefault = num3.GetValueOrDefault();
							if (num3 != null && valueOrDefault < num2)
							{
								num2 = valueOrDefault;
								num = num3;
							}
						}
						return num;
					}
				}
				return num;
			}
			return num;
		}

		/// <summary>Invokes a transform function on each element of a generic sequence and returns the minimum resulting value.</summary>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
		/// <returns>The minimum value in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000555 RID: 1365 RVA: 0x00013104 File Offset: 0x00011304
		public static TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			Comparer<TResult> @default = Comparer<TResult>.Default;
			TResult tresult = default(TResult);
			if (tresult == null)
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TSource tsource = enumerator.Current;
						tresult = selector(tsource);
						if (tresult != null)
						{
							while (enumerator.MoveNext())
							{
								TSource tsource2 = enumerator.Current;
								TResult tresult2 = selector(tsource2);
								if (tresult2 != null && @default.Compare(tresult2, tresult) < 0)
								{
									tresult = tresult2;
								}
							}
							return tresult;
						}
					}
					return tresult;
				}
			}
			using (IEnumerator<TSource> enumerator2 = source.GetEnumerator())
			{
				if (!enumerator2.MoveNext())
				{
					throw Error.NoElements();
				}
				tresult = selector(enumerator2.Current);
				while (enumerator2.MoveNext())
				{
					TSource tsource3 = enumerator2.Current;
					TResult tresult3 = selector(tsource3);
					if (@default.Compare(tresult3, tresult) < 0)
					{
						tresult = tresult3;
					}
				}
			}
			return tresult;
		}

		/// <summary>Sorts the elements of a sequence in ascending order according to a key.</summary>
		/// <param name="source">A sequence of values to order.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted according to a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000556 RID: 1366 RVA: 0x00013228 File Offset: 0x00011428
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return new OrderedEnumerable<TSource, TKey>(source, keySelector, null, false, null);
		}

		/// <summary>Sorts the elements of a sequence in ascending order by using a specified comparer.</summary>
		/// <param name="source">A sequence of values to order.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IComparer`1" /> to compare keys.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted according to a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000557 RID: 1367 RVA: 0x00013234 File Offset: 0x00011434
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			return new OrderedEnumerable<TSource, TKey>(source, keySelector, comparer, false, null);
		}

		/// <summary>Sorts the elements of a sequence in descending order according to a key.</summary>
		/// <param name="source">A sequence of values to order.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted in descending order according to a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000558 RID: 1368 RVA: 0x00013240 File Offset: 0x00011440
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return new OrderedEnumerable<TSource, TKey>(source, keySelector, null, true, null);
		}

		/// <summary>Sorts the elements of a sequence in descending order by using a specified comparer.</summary>
		/// <param name="source">A sequence of values to order.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IComparer`1" /> to compare keys.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted in descending order according to a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000559 RID: 1369 RVA: 0x0001324C File Offset: 0x0001144C
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			return new OrderedEnumerable<TSource, TKey>(source, keySelector, comparer, true, null);
		}

		/// <summary>Performs a subsequent ordering of the elements in a sequence in ascending order according to a key.</summary>
		/// <param name="source">An <see cref="T:System.Linq.IOrderedEnumerable`1" /> that contains elements to sort.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted according to a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x0600055A RID: 1370 RVA: 0x00013258 File Offset: 0x00011458
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			return source.CreateOrderedEnumerable<TKey>(keySelector, null, false);
		}

		/// <summary>Performs a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.</summary>
		/// <param name="source">An <see cref="T:System.Linq.IOrderedEnumerable`1" /> that contains elements to sort.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IComparer`1" /> to compare keys.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted according to a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x0600055B RID: 1371 RVA: 0x00013271 File Offset: 0x00011471
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, false);
		}

		/// <summary>Performs a subsequent ordering of the elements in a sequence in descending order, according to a key.</summary>
		/// <param name="source">An <see cref="T:System.Linq.IOrderedEnumerable`1" /> that contains elements to sort.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted in descending order according to a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x0600055C RID: 1372 RVA: 0x0001328A File Offset: 0x0001148A
		public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			return source.CreateOrderedEnumerable<TKey>(keySelector, null, true);
		}

		/// <summary>Performs a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.</summary>
		/// <param name="source">An <see cref="T:System.Linq.IOrderedEnumerable`1" /> that contains elements to sort.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IComparer`1" /> to compare keys.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted in descending order according to a key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
		// Token: 0x0600055D RID: 1373 RVA: 0x000132A3 File Offset: 0x000114A3
		public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, true);
		}

		/// <summary>Generates a sequence of integral numbers within a specified range.</summary>
		/// <param name="start">The value of the first integer in the sequence.</param>
		/// <param name="count">The number of sequential integers to generate.</param>
		/// <returns>An IEnumerable&lt;Int32&gt; in C# or IEnumerable(Of Int32) in Visual Basic that contains a range of sequential integral numbers.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="count" /> is less than 0.-or-
		///         <paramref name="start" /> + <paramref name="count" /> -1 is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x0600055E RID: 1374 RVA: 0x000132BC File Offset: 0x000114BC
		public static IEnumerable<int> Range(int start, int count)
		{
			long num = (long)start + (long)count - 1L;
			if (count < 0 || num > 2147483647L)
			{
				throw Error.ArgumentOutOfRange("count");
			}
			if (count == 0)
			{
				return EmptyPartition<int>.Instance;
			}
			return new Enumerable.RangeIterator(start, count);
		}

		/// <summary>Generates a sequence that contains one repeated value.</summary>
		/// <param name="element">The value to be repeated.</param>
		/// <param name="count">The number of times to repeat the value in the generated sequence.</param>
		/// <typeparam name="TResult">The type of the value to be repeated in the result sequence.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains a repeated value.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="count" /> is less than 0.</exception>
		// Token: 0x0600055F RID: 1375 RVA: 0x000132FA File Offset: 0x000114FA
		public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
		{
			if (count < 0)
			{
				throw Error.ArgumentOutOfRange("count");
			}
			if (count == 0)
			{
				return EmptyPartition<TResult>.Instance;
			}
			return new Enumerable.RepeatIterator<TResult>(element, count);
		}

		/// <summary>Inverts the order of the elements in a sequence.</summary>
		/// <param name="source">A sequence of values to reverse.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>A sequence whose elements correspond to those of the input sequence in reverse order.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000560 RID: 1376 RVA: 0x0001331B File Offset: 0x0001151B
		public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			return new Enumerable.ReverseIterator<TSource>(source);
		}

		/// <summary>Projects each element of a sequence into a new form.</summary>
		/// <param name="source">A sequence of values to invoke a transform function on.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are the result of invoking the transform function on each element of <paramref name="source" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000561 RID: 1377 RVA: 0x00013334 File Offset: 0x00011534
		public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			Enumerable.Iterator<TSource> iterator = source as Enumerable.Iterator<TSource>;
			if (iterator != null)
			{
				return iterator.Select<TResult>(selector);
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				TSource[] array = source as TSource[];
				if (array != null)
				{
					if (array.Length != 0)
					{
						return new Enumerable.SelectArrayIterator<TSource, TResult>(array, selector);
					}
					return EmptyPartition<TResult>.Instance;
				}
				else
				{
					List<TSource> list2 = source as List<TSource>;
					if (list2 != null)
					{
						return new Enumerable.SelectListIterator<TSource, TResult>(list2, selector);
					}
					return new Enumerable.SelectIListIterator<TSource, TResult>(list, selector);
				}
			}
			else
			{
				IPartition<TSource> partition = source as IPartition<TSource>;
				if (partition == null)
				{
					return new Enumerable.SelectEnumerableIterator<TSource, TResult>(source, selector);
				}
				if (!(partition is EmptyPartition<TSource>))
				{
					return new Enumerable.SelectIPartitionIterator<TSource, TResult>(partition, selector);
				}
				return EmptyPartition<TResult>.Instance;
			}
		}

		/// <summary>Projects each element of a sequence into a new form by incorporating the element's index.</summary>
		/// <param name="source">A sequence of values to invoke a transform function on.</param>
		/// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are the result of invoking the transform function on each element of <paramref name="source" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000562 RID: 1378 RVA: 0x000133E1 File Offset: 0x000115E1
		public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			return Enumerable.SelectIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00013406 File Offset: 0x00011606
		private static IEnumerable<TResult> SelectIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			int index = -1;
			foreach (TSource tsource in source)
			{
				int num = index;
				index = checked(num + 1);
				yield return selector(tsource, index);
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		/// <summary>Projects each element of a sequence to an <see cref="T:System.Collections.Generic.IEnumerable`1" /> and flattens the resulting sequences into one sequence.</summary>
		/// <param name="source">A sequence of values to project.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TResult">The type of the elements of the sequence returned by <paramref name="selector" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000564 RID: 1380 RVA: 0x0001341D File Offset: 0x0001161D
		public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			return new Enumerable.SelectManySingleSelectorIterator<TSource, TResult>(source, selector);
		}

		/// <summary>Projects each element of a sequence to an <see cref="T:System.Collections.Generic.IEnumerable`1" />, and flattens the resulting sequences into one sequence. The index of each source element is used in the projected form of that element.</summary>
		/// <param name="source">A sequence of values to project.</param>
		/// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TResult">The type of the elements of the sequence returned by <paramref name="selector" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are the result of invoking the one-to-many transform function on each element of an input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000565 RID: 1381 RVA: 0x00013442 File Offset: 0x00011642
		public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			return Enumerable.SelectManyIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00013467 File Offset: 0x00011667
		private static IEnumerable<TResult> SelectManyIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
		{
			int index = -1;
			foreach (TSource tsource in source)
			{
				int num = index;
				index = checked(num + 1);
				foreach (TResult tresult in selector(tsource, index))
				{
					yield return tresult;
				}
				IEnumerator<TResult> enumerator2 = null;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		/// <summary>Projects each element of a sequence to an <see cref="T:System.Collections.Generic.IEnumerable`1" />, flattens the resulting sequences into one sequence, and invokes a result selector function on each element therein. The index of each source element is used in the intermediate projected form of that element.</summary>
		/// <param name="source">A sequence of values to project.</param>
		/// <param name="collectionSelector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
		/// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TCollection">The type of the intermediate elements collected by <paramref name="collectionSelector" />.</typeparam>
		/// <typeparam name="TResult">The type of the elements of the resulting sequence.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are the result of invoking the one-to-many transform function <paramref name="collectionSelector" /> on each element of <paramref name="source" /> and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000567 RID: 1383 RVA: 0x0001347E File Offset: 0x0001167E
		public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (collectionSelector == null)
			{
				throw Error.ArgumentNull("collectionSelector");
			}
			if (resultSelector == null)
			{
				throw Error.ArgumentNull("resultSelector");
			}
			return Enumerable.SelectManyIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000134B2 File Offset: 0x000116B2
		private static IEnumerable<TResult> SelectManyIterator<TSource, TCollection, TResult>(IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			int index = -1;
			foreach (TSource element in source)
			{
				int num = index;
				index = checked(num + 1);
				foreach (TCollection tcollection in collectionSelector(element, index))
				{
					yield return resultSelector(element, tcollection);
				}
				IEnumerator<TCollection> enumerator2 = null;
				element = default(TSource);
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		/// <summary>Projects each element of a sequence to an <see cref="T:System.Collections.Generic.IEnumerable`1" />, flattens the resulting sequences into one sequence, and invokes a result selector function on each element therein.</summary>
		/// <param name="source">A sequence of values to project.</param>
		/// <param name="collectionSelector">A transform function to apply to each element of the input sequence.</param>
		/// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TCollection">The type of the intermediate elements collected by <paramref name="collectionSelector" />.</typeparam>
		/// <typeparam name="TResult">The type of the elements of the resulting sequence.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are the result of invoking the one-to-many transform function <paramref name="collectionSelector" /> on each element of <paramref name="source" /> and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
		// Token: 0x06000569 RID: 1385 RVA: 0x000134D0 File Offset: 0x000116D0
		public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (collectionSelector == null)
			{
				throw Error.ArgumentNull("collectionSelector");
			}
			if (resultSelector == null)
			{
				throw Error.ArgumentNull("resultSelector");
			}
			return Enumerable.SelectManyIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00013504 File Offset: 0x00011704
		private static IEnumerable<TResult> SelectManyIterator<TSource, TCollection, TResult>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			foreach (TSource element in source)
			{
				foreach (TCollection tcollection in collectionSelector(element))
				{
					yield return resultSelector(element, tcollection);
				}
				IEnumerator<TCollection> enumerator2 = null;
				element = default(TSource);
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		/// <summary>Determines whether two sequences are equal by comparing the elements by using the default equality comparer for their type.</summary>
		/// <param name="first">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to compare to <paramref name="second" />.</param>
		/// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to compare to the first sequence.</param>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <returns>
		///     <see langword="true" /> if the two source sequences are of equal length and their corresponding elements are equal according to the default equality comparer for their type; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
		// Token: 0x0600056B RID: 1387 RVA: 0x00013522 File Offset: 0x00011722
		public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.SequenceEqual<TSource>(second, null);
		}

		/// <summary>Determines whether two sequences are equal by comparing their elements by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="first">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to compare to <paramref name="second" />.</param>
		/// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to compare to the first sequence.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to use to compare elements.</param>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <returns>
		///     <see langword="true" /> if the two source sequences are of equal length and their corresponding elements compare equal according to <paramref name="comparer" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
		// Token: 0x0600056C RID: 1388 RVA: 0x0001352C File Offset: 0x0001172C
		public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			if (first == null)
			{
				throw Error.ArgumentNull("first");
			}
			if (second == null)
			{
				throw Error.ArgumentNull("second");
			}
			ICollection<TSource> collection = first as ICollection<TSource>;
			if (collection != null)
			{
				ICollection<TSource> collection2 = second as ICollection<TSource>;
				if (collection2 != null)
				{
					if (collection.Count != collection2.Count)
					{
						return false;
					}
					IList<TSource> list = collection as IList<TSource>;
					if (list != null)
					{
						IList<TSource> list2 = collection2 as IList<TSource>;
						if (list2 != null)
						{
							int count = collection.Count;
							for (int i = 0; i < count; i++)
							{
								if (!comparer.Equals(list[i], list2[i]))
								{
									return false;
								}
							}
							return true;
						}
					}
				}
			}
			bool flag;
			using (IEnumerator<TSource> enumerator = first.GetEnumerator())
			{
				using (IEnumerator<TSource> enumerator2 = second.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator2.MoveNext() || !comparer.Equals(enumerator.Current, enumerator2.Current))
						{
							return false;
						}
					}
					flag = !enumerator2.MoveNext();
				}
			}
			return flag;
		}

		/// <summary>Returns the only element of a sequence, and throws an exception if there is not exactly one element in the sequence.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return the single element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The single element of the input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The input sequence contains more than one element.-or-The input sequence is empty.</exception>
		// Token: 0x0600056D RID: 1389 RVA: 0x00013650 File Offset: 0x00011850
		public static TSource Single<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				int count = list.Count;
				if (count == 0)
				{
					throw Error.NoElements();
				}
				if (count == 1)
				{
					return list[0];
				}
			}
			else
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						throw Error.NoElements();
					}
					TSource tsource = enumerator.Current;
					if (!enumerator.MoveNext())
					{
						return tsource;
					}
				}
			}
			throw Error.MoreThanOneElement();
		}

		/// <summary>Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one such element exists.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return a single element from.</param>
		/// <param name="predicate">A function to test an element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The single element of the input sequence that satisfies a condition.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate" />.-or-More than one element satisfies the condition in <paramref name="predicate" />.-or-The source sequence is empty.</exception>
		// Token: 0x0600056E RID: 1390 RVA: 0x000136E0 File Offset: 0x000118E0
		public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					if (predicate(tsource))
					{
						while (enumerator.MoveNext())
						{
							if (predicate(enumerator.Current))
							{
								throw Error.MoreThanOneMatch();
							}
						}
						return tsource;
					}
				}
			}
			throw Error.NoMatch();
		}

		/// <summary>Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return the single element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The single element of the input sequence, or <see langword="default" />(<paramref name="TSource" />) if the sequence contains no elements.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The input sequence contains more than one element.</exception>
		// Token: 0x0600056F RID: 1391 RVA: 0x00013770 File Offset: 0x00011970
		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				int count = list.Count;
				if (count == 0)
				{
					TSource tsource = default(TSource);
					return tsource;
				}
				if (count == 1)
				{
					return list[0];
				}
			}
			else
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						TSource tsource = default(TSource);
						return tsource;
					}
					TSource tsource2 = enumerator.Current;
					if (!enumerator.MoveNext())
					{
						return tsource2;
					}
				}
			}
			throw Error.MoreThanOneElement();
		}

		/// <summary>Returns the only element of a sequence that satisfies a specified condition or a default value if no such element exists; this method throws an exception if more than one element satisfies the condition.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return a single element from.</param>
		/// <param name="predicate">A function to test an element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The single element of the input sequence that satisfies the condition, or <see langword="default" />(<paramref name="TSource" />) if no such element is found.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		// Token: 0x06000570 RID: 1392 RVA: 0x0001380C File Offset: 0x00011A0C
		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					if (predicate(tsource))
					{
						while (enumerator.MoveNext())
						{
							if (predicate(enumerator.Current))
							{
								throw Error.MoreThanOneMatch();
							}
						}
						return tsource;
					}
				}
			}
			return default(TSource);
		}

		/// <summary>Bypasses a specified number of elements in a sequence and then returns the remaining elements.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return elements from.</param>
		/// <param name="count">The number of elements to skip before returning the remaining elements.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements that occur after the specified index in the input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000571 RID: 1393 RVA: 0x000138A0 File Offset: 0x00011AA0
		public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (count <= 0)
			{
				if (source is Enumerable.Iterator<TSource> || source is IPartition<TSource>)
				{
					return source;
				}
				count = 0;
			}
			else
			{
				IPartition<TSource> partition = source as IPartition<TSource>;
				if (partition != null)
				{
					return partition.Skip(count);
				}
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				return new Enumerable.ListPartition<TSource>(list, count, int.MaxValue);
			}
			return new Enumerable.EnumerablePartition<TSource>(source, count, -1);
		}

		/// <summary>Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return elements from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		// Token: 0x06000572 RID: 1394 RVA: 0x00013907 File Offset: 0x00011B07
		public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			return Enumerable.SkipWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001392C File Offset: 0x00011B2C
		private static IEnumerable<TSource> SkipWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			using (IEnumerator<TSource> e = source.GetEnumerator())
			{
				while (e.MoveNext())
				{
					TSource tsource = e.Current;
					if (!predicate(tsource))
					{
						yield return tsource;
						while (e.MoveNext())
						{
							TSource tsource2 = e.Current;
							yield return tsource2;
						}
						yield break;
					}
				}
			}
			IEnumerator<TSource> e = null;
			yield break;
			yield break;
		}

		/// <summary>Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements. The element's index is used in the logic of the predicate function.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return elements from.</param>
		/// <param name="predicate">A function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		// Token: 0x06000574 RID: 1396 RVA: 0x00013943 File Offset: 0x00011B43
		public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			return Enumerable.SkipWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00013968 File Offset: 0x00011B68
		private static IEnumerable<TSource> SkipWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			checked
			{
				using (IEnumerator<TSource> e = source.GetEnumerator())
				{
					int num = -1;
					while (e.MoveNext())
					{
						num++;
						TSource tsource = e.Current;
						if (!predicate(tsource, num))
						{
							yield return tsource;
							while (e.MoveNext())
							{
								TSource tsource2 = e.Current;
								yield return tsource2;
							}
							yield break;
						}
					}
				}
				IEnumerator<TSource> e = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001397F File Offset: 0x00011B7F
		public static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (count <= 0)
			{
				return source.Skip<TSource>(0);
			}
			return Enumerable.SkipLastIterator<TSource>(source, count);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x000139A2 File Offset: 0x00011BA2
		private static IEnumerable<TSource> SkipLastIterator<TSource>(IEnumerable<TSource> source, int count)
		{
			Queue<TSource> queue = new Queue<TSource>();
			using (IEnumerator<TSource> e = source.GetEnumerator())
			{
				while (e.MoveNext())
				{
					if (queue.Count == count)
					{
						do
						{
							yield return queue.Dequeue();
							queue.Enqueue(e.Current);
						}
						while (e.MoveNext());
						break;
					}
					queue.Enqueue(e.Current);
				}
			}
			IEnumerator<TSource> e = null;
			yield break;
			yield break;
		}

		/// <summary>Computes the sum of a sequence of <see cref="T:System.Int32" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Int32" /> values to calculate the sum of.</param>
		/// <returns>The sum of the values in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x06000578 RID: 1400 RVA: 0x000139BC File Offset: 0x00011BBC
		public static int Sum(this IEnumerable<int> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			int num = 0;
			checked
			{
				foreach (int num2 in source)
				{
					num += num2;
				}
				return num;
			}
		}

		/// <summary>Computes the sum of a sequence of nullable <see cref="T:System.Int32" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int32" /> values to calculate the sum of.</param>
		/// <returns>The sum of the values in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x06000579 RID: 1401 RVA: 0x00013A14 File Offset: 0x00011C14
		public static int? Sum(this IEnumerable<int?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			int num = 0;
			checked
			{
				foreach (int? num2 in source)
				{
					if (num2 != null)
					{
						num += num2.GetValueOrDefault();
					}
				}
				return new int?(num);
			}
		}

		/// <summary>Computes the sum of a sequence of <see cref="T:System.Int64" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Int64" /> values to calculate the sum of.</param>
		/// <returns>The sum of the values in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
		// Token: 0x0600057A RID: 1402 RVA: 0x00013A80 File Offset: 0x00011C80
		public static long Sum(this IEnumerable<long> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			long num = 0L;
			checked
			{
				foreach (long num2 in source)
				{
					num += num2;
				}
				return num;
			}
		}

		/// <summary>Computes the sum of a sequence of nullable <see cref="T:System.Int64" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int64" /> values to calculate the sum of.</param>
		/// <returns>The sum of the values in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
		// Token: 0x0600057B RID: 1403 RVA: 0x00013AD8 File Offset: 0x00011CD8
		public static long? Sum(this IEnumerable<long?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			long num = 0L;
			checked
			{
				foreach (long? num2 in source)
				{
					if (num2 != null)
					{
						num += num2.GetValueOrDefault();
					}
				}
				return new long?(num);
			}
		}

		/// <summary>Computes the sum of a sequence of <see cref="T:System.Single" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Single" /> values to calculate the sum of.</param>
		/// <returns>The sum of the values in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600057C RID: 1404 RVA: 0x00013B44 File Offset: 0x00011D44
		public static float Sum(this IEnumerable<float> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			double num = 0.0;
			foreach (float num2 in source)
			{
				num += (double)num2;
			}
			return (float)num;
		}

		/// <summary>Computes the sum of a sequence of nullable <see cref="T:System.Single" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Single" /> values to calculate the sum of.</param>
		/// <returns>The sum of the values in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600057D RID: 1405 RVA: 0x00013BA4 File Offset: 0x00011DA4
		public static float? Sum(this IEnumerable<float?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			double num = 0.0;
			foreach (float? num2 in source)
			{
				if (num2 != null)
				{
					num += (double)num2.GetValueOrDefault();
				}
			}
			return new float?((float)num);
		}

		/// <summary>Computes the sum of a sequence of <see cref="T:System.Double" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Double" /> values to calculate the sum of.</param>
		/// <returns>The sum of the values in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600057E RID: 1406 RVA: 0x00013C18 File Offset: 0x00011E18
		public static double Sum(this IEnumerable<double> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			double num = 0.0;
			foreach (double num2 in source)
			{
				num += num2;
			}
			return num;
		}

		/// <summary>Computes the sum of a sequence of nullable <see cref="T:System.Double" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Double" /> values to calculate the sum of.</param>
		/// <returns>The sum of the values in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600057F RID: 1407 RVA: 0x00013C78 File Offset: 0x00011E78
		public static double? Sum(this IEnumerable<double?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			double num = 0.0;
			foreach (double? num2 in source)
			{
				if (num2 != null)
				{
					num += num2.GetValueOrDefault();
				}
			}
			return new double?(num);
		}

		/// <summary>Computes the sum of a sequence of <see cref="T:System.Decimal" /> values.</summary>
		/// <param name="source">A sequence of <see cref="T:System.Decimal" /> values to calculate the sum of.</param>
		/// <returns>The sum of the values in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue" />.</exception>
		// Token: 0x06000580 RID: 1408 RVA: 0x00013CEC File Offset: 0x00011EEC
		public static decimal Sum(this IEnumerable<decimal> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			decimal num = 0m;
			foreach (decimal num2 in source)
			{
				num += num2;
			}
			return num;
		}

		/// <summary>Computes the sum of a sequence of nullable <see cref="T:System.Decimal" /> values.</summary>
		/// <param name="source">A sequence of nullable <see cref="T:System.Decimal" /> values to calculate the sum of.</param>
		/// <returns>The sum of the values in the sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue" />.</exception>
		// Token: 0x06000581 RID: 1409 RVA: 0x00013D4C File Offset: 0x00011F4C
		public static decimal? Sum(this IEnumerable<decimal?> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			decimal num = 0m;
			foreach (decimal? num2 in source)
			{
				if (num2 != null)
				{
					num += num2.GetValueOrDefault();
				}
			}
			return new decimal?(num);
		}

		/// <summary>Computes the sum of the sequence of <see cref="T:System.Int32" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values that are used to calculate a sum.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The sum of the projected values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x06000582 RID: 1410 RVA: 0x00013DC0 File Offset: 0x00011FC0
		public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			int num = 0;
			checked
			{
				foreach (TSource tsource in source)
				{
					num += selector(tsource);
				}
				return num;
			}
		}

		/// <summary>Computes the sum of the sequence of nullable <see cref="T:System.Int32" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values that are used to calculate a sum.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The sum of the projected values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x06000583 RID: 1411 RVA: 0x00013E2C File Offset: 0x0001202C
		public static int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			int num = 0;
			checked
			{
				foreach (TSource tsource in source)
				{
					int? num2 = selector(tsource);
					if (num2 != null)
					{
						num += num2.GetValueOrDefault();
					}
				}
				return new int?(num);
			}
		}

		/// <summary>Computes the sum of the sequence of <see cref="T:System.Int64" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values that are used to calculate a sum.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The sum of the projected values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
		// Token: 0x06000584 RID: 1412 RVA: 0x00013EAC File Offset: 0x000120AC
		public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			long num = 0L;
			checked
			{
				foreach (TSource tsource in source)
				{
					num += selector(tsource);
				}
				return num;
			}
		}

		/// <summary>Computes the sum of the sequence of nullable <see cref="T:System.Int64" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values that are used to calculate a sum.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The sum of the projected values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
		// Token: 0x06000585 RID: 1413 RVA: 0x00013F18 File Offset: 0x00012118
		public static long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			long num = 0L;
			checked
			{
				foreach (TSource tsource in source)
				{
					long? num2 = selector(tsource);
					if (num2 != null)
					{
						num += num2.GetValueOrDefault();
					}
				}
				return new long?(num);
			}
		}

		/// <summary>Computes the sum of the sequence of <see cref="T:System.Single" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values that are used to calculate a sum.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The sum of the projected values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000586 RID: 1414 RVA: 0x00013F9C File Offset: 0x0001219C
		public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			double num = 0.0;
			foreach (TSource tsource in source)
			{
				num += (double)selector(tsource);
			}
			return (float)num;
		}

		/// <summary>Computes the sum of the sequence of nullable <see cref="T:System.Single" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values that are used to calculate a sum.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The sum of the projected values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000587 RID: 1415 RVA: 0x00014010 File Offset: 0x00012210
		public static float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			double num = 0.0;
			foreach (TSource tsource in source)
			{
				float? num2 = selector(tsource);
				if (num2 != null)
				{
					num += (double)num2.GetValueOrDefault();
				}
			}
			return new float?((float)num);
		}

		/// <summary>Computes the sum of the sequence of <see cref="T:System.Double" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values that are used to calculate a sum.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The sum of the projected values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000588 RID: 1416 RVA: 0x0001409C File Offset: 0x0001229C
		public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			double num = 0.0;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
			}
			return num;
		}

		/// <summary>Computes the sum of the sequence of nullable <see cref="T:System.Double" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values that are used to calculate a sum.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The sum of the projected values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		// Token: 0x06000589 RID: 1417 RVA: 0x00014110 File Offset: 0x00012310
		public static double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			double num = 0.0;
			foreach (TSource tsource in source)
			{
				double? num2 = selector(tsource);
				if (num2 != null)
				{
					num += num2.GetValueOrDefault();
				}
			}
			return new double?(num);
		}

		/// <summary>Computes the sum of the sequence of <see cref="T:System.Decimal" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values that are used to calculate a sum.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The sum of the projected values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue" />.</exception>
		// Token: 0x0600058A RID: 1418 RVA: 0x00014198 File Offset: 0x00012398
		public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			decimal num = 0m;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
			}
			return num;
		}

		/// <summary>Computes the sum of the sequence of nullable <see cref="T:System.Decimal" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
		/// <param name="source">A sequence of values that are used to calculate a sum.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>The sum of the projected values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue" />.</exception>
		// Token: 0x0600058B RID: 1419 RVA: 0x0001420C File Offset: 0x0001240C
		public static decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (selector == null)
			{
				throw Error.ArgumentNull("selector");
			}
			decimal num = 0m;
			foreach (TSource tsource in source)
			{
				decimal? num2 = selector(tsource);
				if (num2 != null)
				{
					num += num2.GetValueOrDefault();
				}
			}
			return new decimal?(num);
		}

		/// <summary>Returns a specified number of contiguous elements from the start of a sequence.</summary>
		/// <param name="source">The sequence to return elements from.</param>
		/// <param name="count">The number of elements to return.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the specified number of elements from the start of the input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600058C RID: 1420 RVA: 0x00014298 File Offset: 0x00012498
		public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (count <= 0)
			{
				return EmptyPartition<TSource>.Instance;
			}
			IPartition<TSource> partition = source as IPartition<TSource>;
			if (partition != null)
			{
				return partition.Take(count);
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				return new Enumerable.ListPartition<TSource>(list, 0, count - 1);
			}
			return new Enumerable.EnumerablePartition<TSource>(source, 0, count - 1);
		}

		/// <summary>Returns elements from a sequence as long as a specified condition is true.</summary>
		/// <param name="source">A sequence to return elements from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		// Token: 0x0600058D RID: 1421 RVA: 0x000142EE File Offset: 0x000124EE
		public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			return Enumerable.TakeWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00014313 File Offset: 0x00012513
		private static IEnumerable<TSource> TakeWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			foreach (TSource tsource in source)
			{
				if (!predicate(tsource))
				{
					break;
				}
				yield return tsource;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		/// <summary>Returns elements from a sequence as long as a specified condition is true. The element's index is used in the logic of the predicate function.</summary>
		/// <param name="source">The sequence to return elements from.</param>
		/// <param name="predicate">A function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains elements from the input sequence that occur before the element at which the test no longer passes.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		// Token: 0x0600058F RID: 1423 RVA: 0x0001432A File Offset: 0x0001252A
		public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			return Enumerable.TakeWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001434F File Offset: 0x0001254F
		private static IEnumerable<TSource> TakeWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			int index = -1;
			foreach (TSource tsource in source)
			{
				int num = index;
				index = checked(num + 1);
				if (!predicate(tsource, index))
				{
					break;
				}
				yield return tsource;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00014366 File Offset: 0x00012566
		public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (count <= 0)
			{
				return EmptyPartition<TSource>.Instance;
			}
			return Enumerable.TakeLastIterator<TSource>(source, count);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00014387 File Offset: 0x00012587
		private static IEnumerable<TSource> TakeLastIterator<TSource>(IEnumerable<TSource> source, int count)
		{
			Queue<TSource> queue;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					yield break;
				}
				queue = new Queue<TSource>();
				queue.Enqueue(enumerator.Current);
				while (enumerator.MoveNext())
				{
					if (queue.Count >= count)
					{
						do
						{
							queue.Dequeue();
							queue.Enqueue(enumerator.Current);
						}
						while (enumerator.MoveNext());
						break;
					}
					queue.Enqueue(enumerator.Current);
				}
			}
			do
			{
				yield return queue.Dequeue();
			}
			while (queue.Count > 0);
			yield break;
		}

		/// <summary>Creates an array from a <see cref="T:System.Collections.Generic.IEnumerable`1" />.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create an array from.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An array that contains the elements from the input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000593 RID: 1427 RVA: 0x000143A0 File Offset: 0x000125A0
		public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			IIListProvider<TSource> iilistProvider = source as IIListProvider<TSource>;
			if (iilistProvider == null)
			{
				return global::System.Collections.Generic.EnumerableHelpers.ToArray<TSource>(source);
			}
			return iilistProvider.ToArray();
		}

		/// <summary>Creates a <see cref="T:System.Collections.Generic.List`1" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" />.</summary>
		/// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Collections.Generic.List`1" /> from.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>A <see cref="T:System.Collections.Generic.List`1" /> that contains elements from the input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x06000594 RID: 1428 RVA: 0x000143D4 File Offset: 0x000125D4
		public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			IIListProvider<TSource> iilistProvider = source as IIListProvider<TSource>;
			if (iilistProvider == null)
			{
				return new List<TSource>(source);
			}
			return iilistProvider.ToList();
		}

		/// <summary>Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> according to a specified key selector function.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Collections.Generic.Dictionary`2" /> from.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains keys and values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.-or-
		///         <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
		// Token: 0x06000595 RID: 1429 RVA: 0x00014406 File Offset: 0x00012606
		public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ToDictionary<TSource, TKey>(keySelector, null);
		}

		/// <summary>Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> according to a specified key selector function and key comparer.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Collections.Generic.Dictionary`2" /> from.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the keys returned by <paramref name="keySelector" />.</typeparam>
		/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains keys and values.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.-or-
		///         <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
		// Token: 0x06000596 RID: 1430 RVA: 0x00014410 File Offset: 0x00012610
		public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (keySelector == null)
			{
				throw Error.ArgumentNull("keySelector");
			}
			int num = 0;
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null)
			{
				num = collection.Count;
				if (num == 0)
				{
					return new Dictionary<TKey, TSource>(comparer);
				}
				TSource[] array = collection as TSource[];
				if (array != null)
				{
					return Enumerable.ToDictionary<TSource, TKey>(array, keySelector, comparer);
				}
				List<TSource> list = collection as List<TSource>;
				if (list != null)
				{
					return Enumerable.ToDictionary<TSource, TKey>(list, keySelector, comparer);
				}
			}
			Dictionary<TKey, TSource> dictionary = new Dictionary<TKey, TSource>(num, comparer);
			foreach (TSource tsource in source)
			{
				dictionary.Add(keySelector(tsource), tsource);
			}
			return dictionary;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000144D4 File Offset: 0x000126D4
		private static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(TSource[] source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Dictionary<TKey, TSource> dictionary = new Dictionary<TKey, TSource>(source.Length, comparer);
			for (int i = 0; i < source.Length; i++)
			{
				dictionary.Add(keySelector(source[i]), source[i]);
			}
			return dictionary;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00014514 File Offset: 0x00012714
		private static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(List<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Dictionary<TKey, TSource> dictionary = new Dictionary<TKey, TSource>(source.Count, comparer);
			foreach (TSource tsource in source)
			{
				dictionary.Add(keySelector(tsource), tsource);
			}
			return dictionary;
		}

		/// <summary>Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> according to specified key selector and element selector functions.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Collections.Generic.Dictionary`2" /> from.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <typeparam name="TElement">The type of the value returned by <paramref name="elementSelector" />.</typeparam>
		/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains values of type <paramref name="TElement" /> selected from the input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> or <paramref name="elementSelector" /> is <see langword="null" />.-or-
		///         <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
		// Token: 0x06000599 RID: 1433 RVA: 0x00014578 File Offset: 0x00012778
		public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.ToDictionary<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		/// <summary>Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> according to a specified key selector function, a comparer, and an element selector function.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Collections.Generic.Dictionary`2" /> from.</param>
		/// <param name="keySelector">A function to extract a key from each element.</param>
		/// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
		/// <typeparam name="TElement">The type of the value returned by <paramref name="elementSelector" />.</typeparam>
		/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains values of type <paramref name="TElement" /> selected from the input sequence.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="keySelector" /> or <paramref name="elementSelector" /> is <see langword="null" />.-or-
		///         <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
		// Token: 0x0600059A RID: 1434 RVA: 0x00014584 File Offset: 0x00012784
		public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (keySelector == null)
			{
				throw Error.ArgumentNull("keySelector");
			}
			if (elementSelector == null)
			{
				throw Error.ArgumentNull("elementSelector");
			}
			int num = 0;
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null)
			{
				num = collection.Count;
				if (num == 0)
				{
					return new Dictionary<TKey, TElement>(comparer);
				}
				TSource[] array = collection as TSource[];
				if (array != null)
				{
					return Enumerable.ToDictionary<TSource, TKey, TElement>(array, keySelector, elementSelector, comparer);
				}
				List<TSource> list = collection as List<TSource>;
				if (list != null)
				{
					return Enumerable.ToDictionary<TSource, TKey, TElement>(list, keySelector, elementSelector, comparer);
				}
			}
			Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(num, comparer);
			foreach (TSource tsource in source)
			{
				dictionary.Add(keySelector(tsource), elementSelector(tsource));
			}
			return dictionary;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001465C File Offset: 0x0001285C
		private static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(TSource[] source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(source.Length, comparer);
			for (int i = 0; i < source.Length; i++)
			{
				dictionary.Add(keySelector(source[i]), elementSelector(source[i]));
			}
			return dictionary;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x000146A4 File Offset: 0x000128A4
		private static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(List<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(source.Count, comparer);
			foreach (TSource tsource in source)
			{
				dictionary.Add(keySelector(tsource), elementSelector(tsource));
			}
			return dictionary;
		}

		/// <summary>Creates a <see cref="T:System.Collections.Generic.HashSet`1" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" />.</summary>
		/// <param name="source">
		///       An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Collections.Generic.HashSet`1" /> from.
		///     </param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>A <see cref="T:System.Collections.Generic.HashSet`1" /> that contains values of type TSource selected from the input sequence.</returns>
		// Token: 0x0600059D RID: 1437 RVA: 0x00014710 File Offset: 0x00012910
		public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source)
		{
			return source.ToHashSet<TSource>(null);
		}

		/// <summary>Creates a <see cref="T:System.Collections.Generic.HashSet`1" /> from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> using the <paramref name="comparer" /> to compare keys</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create a <see cref="T:System.Collections.Generic.HashSet`1" /> from.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" /></typeparam>
		/// <returns>A <see cref="T:System.Collections.Generic.HashSet`1" /> that contains values of type <paramref name="TSource" /> selected from the input sequence.</returns>
		// Token: 0x0600059E RID: 1438 RVA: 0x00014719 File Offset: 0x00012919
		public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			return new HashSet<TSource>(source, comparer);
		}

		/// <summary>Produces the set union of two sequences by using the default equality comparer.</summary>
		/// <param name="first">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements form the first set for the union.</param>
		/// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements form the second set for the union.</param>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements from both input sequences, excluding duplicates.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
		// Token: 0x0600059F RID: 1439 RVA: 0x00014730 File Offset: 0x00012930
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.Union<TSource>(second, null);
		}

		/// <summary>Produces the set union of two sequences by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="first">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements form the first set for the union.</param>
		/// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements form the second set for the union.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</param>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements from both input sequences, excluding duplicates.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
		// Token: 0x060005A0 RID: 1440 RVA: 0x0001473C File Offset: 0x0001293C
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			if (first == null)
			{
				throw Error.ArgumentNull("first");
			}
			if (second == null)
			{
				throw Error.ArgumentNull("second");
			}
			Enumerable.UnionIterator<TSource> unionIterator = first as Enumerable.UnionIterator<TSource>;
			if (unionIterator == null || !Utilities.AreEqualityComparersEqual<TSource>(comparer, unionIterator._comparer))
			{
				return new Enumerable.UnionIterator2<TSource>(first, second, comparer);
			}
			return unionIterator.Union(second);
		}

		/// <summary>Filters a sequence of values based on a predicate.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to filter.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains elements from the input sequence that satisfy the condition.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		// Token: 0x060005A1 RID: 1441 RVA: 0x00014790 File Offset: 0x00012990
		public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			Enumerable.Iterator<TSource> iterator = source as Enumerable.Iterator<TSource>;
			if (iterator != null)
			{
				return iterator.Where(predicate);
			}
			TSource[] array = source as TSource[];
			if (array != null)
			{
				if (array.Length != 0)
				{
					return new Enumerable.WhereArrayIterator<TSource>(array, predicate);
				}
				return EmptyPartition<TSource>.Instance;
			}
			else
			{
				List<TSource> list = source as List<TSource>;
				if (list != null)
				{
					return new Enumerable.WhereListIterator<TSource>(list, predicate);
				}
				return new Enumerable.WhereEnumerableIterator<TSource>(source, predicate);
			}
		}

		/// <summary>Filters a sequence of values based on a predicate. Each element's index is used in the logic of the predicate function.</summary>
		/// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to filter.</param>
		/// <param name="predicate">A function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains elements from the input sequence that satisfy the condition.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
		// Token: 0x060005A2 RID: 1442 RVA: 0x00014804 File Offset: 0x00012A04
		public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate");
			}
			return Enumerable.WhereIterator<TSource>(source, predicate);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00014829 File Offset: 0x00012A29
		private static IEnumerable<TSource> WhereIterator<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			int index = -1;
			foreach (TSource tsource in source)
			{
				int num = index;
				index = checked(num + 1);
				if (predicate(tsource, index))
				{
					yield return tsource;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		/// <summary>Applies a specified function to the corresponding elements of two sequences, producing a sequence of the results.</summary>
		/// <param name="first">The first sequence to merge.</param>
		/// <param name="second">The second sequence to merge.</param>
		/// <param name="resultSelector">A function that specifies how to merge the elements from the two sequences.</param>
		/// <typeparam name="TFirst">The type of the elements of the first input sequence.</typeparam>
		/// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
		/// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains merged elements of two input sequences.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
		// Token: 0x060005A4 RID: 1444 RVA: 0x00014840 File Offset: 0x00012A40
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
		{
			if (first == null)
			{
				throw Error.ArgumentNull("first");
			}
			if (second == null)
			{
				throw Error.ArgumentNull("second");
			}
			if (resultSelector == null)
			{
				throw Error.ArgumentNull("resultSelector");
			}
			return Enumerable.ZipIterator<TFirst, TSecond, TResult>(first, second, resultSelector);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00014874 File Offset: 0x00012A74
		private static IEnumerable<TResult> ZipIterator<TFirst, TSecond, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
		{
			using (IEnumerator<TFirst> e = first.GetEnumerator())
			{
				using (IEnumerator<TSecond> e2 = second.GetEnumerator())
				{
					while (e.MoveNext() && e2.MoveNext())
					{
						yield return resultSelector(e.Current, e2.Current);
					}
				}
				IEnumerator<TSecond> e2 = null;
			}
			IEnumerator<TFirst> e = null;
			yield break;
			yield break;
		}

		// Token: 0x0200009A RID: 154
		private abstract class AppendPrependIterator<TSource> : Enumerable.Iterator<TSource>, IIListProvider<TSource>, IEnumerable<TSource>, IEnumerable
		{
			// Token: 0x060005A6 RID: 1446 RVA: 0x00014892 File Offset: 0x00012A92
			protected AppendPrependIterator(IEnumerable<TSource> source)
			{
				this._source = source;
			}

			// Token: 0x060005A7 RID: 1447 RVA: 0x000148A1 File Offset: 0x00012AA1
			protected void GetSourceEnumerator()
			{
				this._enumerator = this._source.GetEnumerator();
			}

			// Token: 0x060005A8 RID: 1448
			public abstract Enumerable.AppendPrependIterator<TSource> Append(TSource item);

			// Token: 0x060005A9 RID: 1449
			public abstract Enumerable.AppendPrependIterator<TSource> Prepend(TSource item);

			// Token: 0x060005AA RID: 1450 RVA: 0x000148B4 File Offset: 0x00012AB4
			protected bool LoadFromEnumerator()
			{
				if (this._enumerator.MoveNext())
				{
					this._current = this._enumerator.Current;
					return true;
				}
				this.Dispose();
				return false;
			}

			// Token: 0x060005AB RID: 1451 RVA: 0x000148DD File Offset: 0x00012ADD
			public override void Dispose()
			{
				if (this._enumerator != null)
				{
					this._enumerator.Dispose();
					this._enumerator = null;
				}
				base.Dispose();
			}

			// Token: 0x060005AC RID: 1452
			public abstract TSource[] ToArray();

			// Token: 0x060005AD RID: 1453
			public abstract List<TSource> ToList();

			// Token: 0x060005AE RID: 1454
			public abstract int GetCount(bool onlyIfCheap);

			// Token: 0x0400045A RID: 1114
			protected readonly IEnumerable<TSource> _source;

			// Token: 0x0400045B RID: 1115
			protected IEnumerator<TSource> _enumerator;
		}

		// Token: 0x0200009B RID: 155
		private class AppendPrepend1Iterator<TSource> : Enumerable.AppendPrependIterator<TSource>
		{
			// Token: 0x060005AF RID: 1455 RVA: 0x000148FF File Offset: 0x00012AFF
			public AppendPrepend1Iterator(IEnumerable<TSource> source, TSource item, bool appending)
				: base(source)
			{
				this._item = item;
				this._appending = appending;
			}

			// Token: 0x060005B0 RID: 1456 RVA: 0x00014916 File Offset: 0x00012B16
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.AppendPrepend1Iterator<TSource>(this._source, this._item, this._appending);
			}

			// Token: 0x060005B1 RID: 1457 RVA: 0x00014930 File Offset: 0x00012B30
			public override bool MoveNext()
			{
				switch (this._state)
				{
				case 1:
					this._state = 2;
					if (!this._appending)
					{
						this._current = this._item;
						return true;
					}
					break;
				case 2:
					break;
				case 3:
					goto IL_0047;
				default:
					goto IL_0067;
				}
				base.GetSourceEnumerator();
				this._state = 3;
				IL_0047:
				if (base.LoadFromEnumerator())
				{
					return true;
				}
				if (this._appending)
				{
					this._current = this._item;
					return true;
				}
				IL_0067:
				this.Dispose();
				return false;
			}

			// Token: 0x060005B2 RID: 1458 RVA: 0x000149AC File Offset: 0x00012BAC
			public override Enumerable.AppendPrependIterator<TSource> Append(TSource item)
			{
				if (this._appending)
				{
					return new Enumerable.AppendPrependN<TSource>(this._source, null, new SingleLinkedNode<TSource>(this._item).Add(item), 0, 2);
				}
				return new Enumerable.AppendPrependN<TSource>(this._source, new SingleLinkedNode<TSource>(this._item), new SingleLinkedNode<TSource>(item), 1, 1);
			}

			// Token: 0x060005B3 RID: 1459 RVA: 0x00014A00 File Offset: 0x00012C00
			public override Enumerable.AppendPrependIterator<TSource> Prepend(TSource item)
			{
				if (this._appending)
				{
					return new Enumerable.AppendPrependN<TSource>(this._source, new SingleLinkedNode<TSource>(item), new SingleLinkedNode<TSource>(this._item), 1, 1);
				}
				return new Enumerable.AppendPrependN<TSource>(this._source, new SingleLinkedNode<TSource>(this._item).Add(item), null, 2, 0);
			}

			// Token: 0x060005B4 RID: 1460 RVA: 0x00014A54 File Offset: 0x00012C54
			private TSource[] LazyToArray()
			{
				global::System.Collections.Generic.LargeArrayBuilder<TSource> largeArrayBuilder = new global::System.Collections.Generic.LargeArrayBuilder<TSource>(true);
				if (!this._appending)
				{
					largeArrayBuilder.SlowAdd(this._item);
				}
				largeArrayBuilder.AddRange(this._source);
				if (this._appending)
				{
					largeArrayBuilder.SlowAdd(this._item);
				}
				return largeArrayBuilder.ToArray();
			}

			// Token: 0x060005B5 RID: 1461 RVA: 0x00014AA8 File Offset: 0x00012CA8
			public override TSource[] ToArray()
			{
				int count = this.GetCount(true);
				if (count == -1)
				{
					return this.LazyToArray();
				}
				TSource[] array = new TSource[count];
				int num;
				if (this._appending)
				{
					num = 0;
				}
				else
				{
					array[0] = this._item;
					num = 1;
				}
				global::System.Collections.Generic.EnumerableHelpers.Copy<TSource>(this._source, array, num, count - 1);
				if (this._appending)
				{
					array[array.Length - 1] = this._item;
				}
				return array;
			}

			// Token: 0x060005B6 RID: 1462 RVA: 0x00014B14 File Offset: 0x00012D14
			public override List<TSource> ToList()
			{
				int count = this.GetCount(true);
				List<TSource> list = ((count == -1) ? new List<TSource>() : new List<TSource>(count));
				if (!this._appending)
				{
					list.Add(this._item);
				}
				list.AddRange(this._source);
				if (this._appending)
				{
					list.Add(this._item);
				}
				return list;
			}

			// Token: 0x060005B7 RID: 1463 RVA: 0x00014B70 File Offset: 0x00012D70
			public override int GetCount(bool onlyIfCheap)
			{
				IIListProvider<TSource> iilistProvider = this._source as IIListProvider<TSource>;
				if (iilistProvider != null)
				{
					int count = iilistProvider.GetCount(onlyIfCheap);
					if (count != -1)
					{
						return count + 1;
					}
					return -1;
				}
				else
				{
					if (onlyIfCheap && !(this._source is ICollection<TSource>))
					{
						return -1;
					}
					return this._source.Count<TSource>() + 1;
				}
			}

			// Token: 0x0400045C RID: 1116
			private readonly TSource _item;

			// Token: 0x0400045D RID: 1117
			private readonly bool _appending;
		}

		// Token: 0x0200009C RID: 156
		private class AppendPrependN<TSource> : Enumerable.AppendPrependIterator<TSource>
		{
			// Token: 0x060005B8 RID: 1464 RVA: 0x00014BBD File Offset: 0x00012DBD
			public AppendPrependN(IEnumerable<TSource> source, SingleLinkedNode<TSource> prepended, SingleLinkedNode<TSource> appended, int prependCount, int appendCount)
				: base(source)
			{
				this._prepended = prepended;
				this._appended = appended;
				this._prependCount = prependCount;
				this._appendCount = appendCount;
			}

			// Token: 0x060005B9 RID: 1465 RVA: 0x00014BE4 File Offset: 0x00012DE4
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.AppendPrependN<TSource>(this._source, this._prepended, this._appended, this._prependCount, this._appendCount);
			}

			// Token: 0x060005BA RID: 1466 RVA: 0x00014C0C File Offset: 0x00012E0C
			public override bool MoveNext()
			{
				switch (this._state)
				{
				case 1:
					this._node = this._prepended;
					this._state = 2;
					break;
				case 2:
					break;
				case 3:
					goto IL_0070;
				case 4:
					goto IL_00A2;
				default:
					this.Dispose();
					return false;
				}
				if (this._node != null)
				{
					this._current = this._node.Item;
					this._node = this._node.Linked;
					return true;
				}
				base.GetSourceEnumerator();
				this._state = 3;
				IL_0070:
				if (base.LoadFromEnumerator())
				{
					return true;
				}
				if (this._appended == null)
				{
					return false;
				}
				this._enumerator = this._appended.GetEnumerator(this._appendCount);
				this._state = 4;
				IL_00A2:
				return base.LoadFromEnumerator();
			}

			// Token: 0x060005BB RID: 1467 RVA: 0x00014CCC File Offset: 0x00012ECC
			public override Enumerable.AppendPrependIterator<TSource> Append(TSource item)
			{
				SingleLinkedNode<TSource> singleLinkedNode = ((this._appended != null) ? this._appended.Add(item) : new SingleLinkedNode<TSource>(item));
				return new Enumerable.AppendPrependN<TSource>(this._source, this._prepended, singleLinkedNode, this._prependCount, this._appendCount + 1);
			}

			// Token: 0x060005BC RID: 1468 RVA: 0x00014D18 File Offset: 0x00012F18
			public override Enumerable.AppendPrependIterator<TSource> Prepend(TSource item)
			{
				SingleLinkedNode<TSource> singleLinkedNode = ((this._prepended != null) ? this._prepended.Add(item) : new SingleLinkedNode<TSource>(item));
				return new Enumerable.AppendPrependN<TSource>(this._source, singleLinkedNode, this._appended, this._prependCount + 1, this._appendCount);
			}

			// Token: 0x060005BD RID: 1469 RVA: 0x00014D64 File Offset: 0x00012F64
			private TSource[] LazyToArray()
			{
				SparseArrayBuilder<TSource> sparseArrayBuilder = new SparseArrayBuilder<TSource>(true);
				if (this._prepended != null)
				{
					sparseArrayBuilder.Reserve(this._prependCount);
				}
				sparseArrayBuilder.AddRange(this._source);
				if (this._appended != null)
				{
					sparseArrayBuilder.Reserve(this._appendCount);
				}
				TSource[] array = sparseArrayBuilder.ToArray();
				int num = 0;
				for (SingleLinkedNode<TSource> singleLinkedNode = this._prepended; singleLinkedNode != null; singleLinkedNode = singleLinkedNode.Linked)
				{
					array[num++] = singleLinkedNode.Item;
				}
				num = array.Length - 1;
				for (SingleLinkedNode<TSource> singleLinkedNode2 = this._appended; singleLinkedNode2 != null; singleLinkedNode2 = singleLinkedNode2.Linked)
				{
					array[num--] = singleLinkedNode2.Item;
				}
				return array;
			}

			// Token: 0x060005BE RID: 1470 RVA: 0x00014E10 File Offset: 0x00013010
			public override TSource[] ToArray()
			{
				int count = this.GetCount(true);
				if (count == -1)
				{
					return this.LazyToArray();
				}
				TSource[] array = new TSource[count];
				int num = 0;
				for (SingleLinkedNode<TSource> singleLinkedNode = this._prepended; singleLinkedNode != null; singleLinkedNode = singleLinkedNode.Linked)
				{
					array[num] = singleLinkedNode.Item;
					num++;
				}
				ICollection<TSource> collection = this._source as ICollection<TSource>;
				if (collection != null)
				{
					collection.CopyTo(array, num);
				}
				else
				{
					foreach (TSource tsource in this._source)
					{
						array[num] = tsource;
						num++;
					}
				}
				num = array.Length;
				for (SingleLinkedNode<TSource> singleLinkedNode2 = this._appended; singleLinkedNode2 != null; singleLinkedNode2 = singleLinkedNode2.Linked)
				{
					num--;
					array[num] = singleLinkedNode2.Item;
				}
				return array;
			}

			// Token: 0x060005BF RID: 1471 RVA: 0x00014EF8 File Offset: 0x000130F8
			public override List<TSource> ToList()
			{
				int count = this.GetCount(true);
				List<TSource> list = ((count == -1) ? new List<TSource>() : new List<TSource>(count));
				for (SingleLinkedNode<TSource> singleLinkedNode = this._prepended; singleLinkedNode != null; singleLinkedNode = singleLinkedNode.Linked)
				{
					list.Add(singleLinkedNode.Item);
				}
				list.AddRange(this._source);
				if (this._appended != null)
				{
					IEnumerator<TSource> enumerator = this._appended.GetEnumerator(this._appendCount);
					while (enumerator.MoveNext())
					{
						TSource tsource = enumerator.Current;
						list.Add(tsource);
					}
				}
				return list;
			}

			// Token: 0x060005C0 RID: 1472 RVA: 0x00014F7C File Offset: 0x0001317C
			public override int GetCount(bool onlyIfCheap)
			{
				IIListProvider<TSource> iilistProvider = this._source as IIListProvider<TSource>;
				if (iilistProvider != null)
				{
					int count = iilistProvider.GetCount(onlyIfCheap);
					if (count != -1)
					{
						return count + this._appendCount + this._prependCount;
					}
					return -1;
				}
				else
				{
					if (onlyIfCheap && !(this._source is ICollection<TSource>))
					{
						return -1;
					}
					return this._source.Count<TSource>() + this._appendCount + this._prependCount;
				}
			}

			// Token: 0x0400045E RID: 1118
			private readonly SingleLinkedNode<TSource> _prepended;

			// Token: 0x0400045F RID: 1119
			private readonly SingleLinkedNode<TSource> _appended;

			// Token: 0x04000460 RID: 1120
			private readonly int _prependCount;

			// Token: 0x04000461 RID: 1121
			private readonly int _appendCount;

			// Token: 0x04000462 RID: 1122
			private SingleLinkedNode<TSource> _node;
		}

		// Token: 0x0200009D RID: 157
		private sealed class Concat2Iterator<TSource> : Enumerable.ConcatIterator<TSource>
		{
			// Token: 0x060005C1 RID: 1473 RVA: 0x00014FE1 File Offset: 0x000131E1
			internal Concat2Iterator(IEnumerable<TSource> first, IEnumerable<TSource> second)
			{
				this._first = first;
				this._second = second;
			}

			// Token: 0x060005C2 RID: 1474 RVA: 0x00014FF7 File Offset: 0x000131F7
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.Concat2Iterator<TSource>(this._first, this._second);
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x0001500C File Offset: 0x0001320C
			internal override Enumerable.ConcatIterator<TSource> Concat(IEnumerable<TSource> next)
			{
				bool flag = next is ICollection<TSource> && this._first is ICollection<TSource> && this._second is ICollection<TSource>;
				return new Enumerable.ConcatNIterator<TSource>(this, next, 2, flag);
			}

			// Token: 0x060005C4 RID: 1476 RVA: 0x0001504C File Offset: 0x0001324C
			public override int GetCount(bool onlyIfCheap)
			{
				int num;
				if (!global::System.Collections.Generic.EnumerableHelpers.TryGetCount<TSource>(this._first, out num))
				{
					if (onlyIfCheap)
					{
						return -1;
					}
					num = this._first.Count<TSource>();
				}
				int num2;
				if (!global::System.Collections.Generic.EnumerableHelpers.TryGetCount<TSource>(this._second, out num2))
				{
					if (onlyIfCheap)
					{
						return -1;
					}
					num2 = this._second.Count<TSource>();
				}
				return checked(num + num2);
			}

			// Token: 0x060005C5 RID: 1477 RVA: 0x0001509C File Offset: 0x0001329C
			internal override IEnumerable<TSource> GetEnumerable(int index)
			{
				if (index == 0)
				{
					return this._first;
				}
				if (index != 1)
				{
					return null;
				}
				return this._second;
			}

			// Token: 0x060005C6 RID: 1478 RVA: 0x000150B8 File Offset: 0x000132B8
			public override TSource[] ToArray()
			{
				SparseArrayBuilder<TSource> sparseArrayBuilder = new SparseArrayBuilder<TSource>(true);
				bool flag = sparseArrayBuilder.ReserveOrAdd(this._first);
				bool flag2 = sparseArrayBuilder.ReserveOrAdd(this._second);
				TSource[] array = sparseArrayBuilder.ToArray();
				if (flag)
				{
					Marker marker = sparseArrayBuilder.Markers.First();
					global::System.Collections.Generic.EnumerableHelpers.Copy<TSource>(this._first, array, 0, marker.Count);
				}
				if (flag2)
				{
					Marker marker2 = sparseArrayBuilder.Markers.Last();
					global::System.Collections.Generic.EnumerableHelpers.Copy<TSource>(this._second, array, marker2.Index, marker2.Count);
				}
				return array;
			}

			// Token: 0x04000463 RID: 1123
			internal readonly IEnumerable<TSource> _first;

			// Token: 0x04000464 RID: 1124
			internal readonly IEnumerable<TSource> _second;
		}

		// Token: 0x0200009E RID: 158
		private sealed class ConcatNIterator<TSource> : Enumerable.ConcatIterator<TSource>
		{
			// Token: 0x060005C7 RID: 1479 RVA: 0x00015147 File Offset: 0x00013347
			internal ConcatNIterator(Enumerable.ConcatIterator<TSource> tail, IEnumerable<TSource> head, int headIndex, bool hasOnlyCollections)
			{
				this._tail = tail;
				this._head = head;
				this._headIndex = headIndex;
				this._hasOnlyCollections = hasOnlyCollections;
			}

			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0001516C File Offset: 0x0001336C
			private Enumerable.ConcatNIterator<TSource> PreviousN
			{
				get
				{
					return this._tail as Enumerable.ConcatNIterator<TSource>;
				}
			}

			// Token: 0x060005C9 RID: 1481 RVA: 0x00015179 File Offset: 0x00013379
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.ConcatNIterator<TSource>(this._tail, this._head, this._headIndex, this._hasOnlyCollections);
			}

			// Token: 0x060005CA RID: 1482 RVA: 0x00015198 File Offset: 0x00013398
			internal override Enumerable.ConcatIterator<TSource> Concat(IEnumerable<TSource> next)
			{
				if (this._headIndex == 2147483645)
				{
					return new Enumerable.Concat2Iterator<TSource>(this, next);
				}
				bool flag = this._hasOnlyCollections && next is ICollection<TSource>;
				return new Enumerable.ConcatNIterator<TSource>(this, next, this._headIndex + 1, flag);
			}

			// Token: 0x060005CB RID: 1483 RVA: 0x000151E0 File Offset: 0x000133E0
			public override int GetCount(bool onlyIfCheap)
			{
				if (onlyIfCheap && !this._hasOnlyCollections)
				{
					return -1;
				}
				int num = 0;
				Enumerable.ConcatNIterator<TSource> concatNIterator = this;
				checked
				{
					Enumerable.ConcatNIterator<TSource> concatNIterator2;
					do
					{
						concatNIterator2 = concatNIterator;
						IEnumerable<TSource> head = concatNIterator2._head;
						ICollection<TSource> collection = head as ICollection<TSource>;
						int num2 = ((collection != null) ? collection.Count : head.Count<TSource>());
						num += num2;
					}
					while ((concatNIterator = concatNIterator2.PreviousN) != null);
					return num + concatNIterator2._tail.GetCount(onlyIfCheap);
				}
			}

			// Token: 0x060005CC RID: 1484 RVA: 0x00015240 File Offset: 0x00013440
			internal override IEnumerable<TSource> GetEnumerable(int index)
			{
				if (index > this._headIndex)
				{
					return null;
				}
				Enumerable.ConcatNIterator<TSource> concatNIterator = this;
				Enumerable.ConcatNIterator<TSource> concatNIterator2;
				for (;;)
				{
					concatNIterator2 = concatNIterator;
					if (index == concatNIterator2._headIndex)
					{
						break;
					}
					if ((concatNIterator = concatNIterator2.PreviousN) == null)
					{
						goto Block_3;
					}
				}
				return concatNIterator2._head;
				Block_3:
				return concatNIterator2._tail.GetEnumerable(index);
			}

			// Token: 0x060005CD RID: 1485 RVA: 0x00015282 File Offset: 0x00013482
			public override TSource[] ToArray()
			{
				if (!this._hasOnlyCollections)
				{
					return this.LazyToArray();
				}
				return this.PreallocatingToArray();
			}

			// Token: 0x060005CE RID: 1486 RVA: 0x0001529C File Offset: 0x0001349C
			private TSource[] LazyToArray()
			{
				SparseArrayBuilder<TSource> sparseArrayBuilder = new SparseArrayBuilder<TSource>(true);
				global::System.Collections.Generic.ArrayBuilder<int> arrayBuilder = default(global::System.Collections.Generic.ArrayBuilder<int>);
				int num = 0;
				for (;;)
				{
					IEnumerable<TSource> enumerable = this.GetEnumerable(num);
					if (enumerable == null)
					{
						break;
					}
					if (sparseArrayBuilder.ReserveOrAdd(enumerable))
					{
						arrayBuilder.Add(num);
					}
					num++;
				}
				TSource[] array = sparseArrayBuilder.ToArray();
				global::System.Collections.Generic.ArrayBuilder<Marker> markers = sparseArrayBuilder.Markers;
				for (int i = 0; i < markers.Count; i++)
				{
					Marker marker = markers[i];
					global::System.Collections.Generic.EnumerableHelpers.Copy<TSource>(this.GetEnumerable(arrayBuilder[i]), array, marker.Index, marker.Count);
				}
				return array;
			}

			// Token: 0x060005CF RID: 1487 RVA: 0x0001533C File Offset: 0x0001353C
			private TSource[] PreallocatingToArray()
			{
				int count = this.GetCount(true);
				if (count == 0)
				{
					return Array.Empty<TSource>();
				}
				TSource[] array = new TSource[count];
				int num = array.Length;
				Enumerable.ConcatNIterator<TSource> concatNIterator = this;
				checked
				{
					Enumerable.ConcatNIterator<TSource> concatNIterator2;
					do
					{
						concatNIterator2 = concatNIterator;
						ICollection<TSource> collection = (ICollection<TSource>)concatNIterator2._head;
						int count2 = collection.Count;
						if (count2 > 0)
						{
							num -= count2;
							collection.CopyTo(array, num);
						}
					}
					while ((concatNIterator = concatNIterator2.PreviousN) != null);
					Enumerable.Concat2Iterator<TSource> concat2Iterator = (Enumerable.Concat2Iterator<TSource>)concatNIterator2._tail;
					ICollection<TSource> collection2 = (ICollection<TSource>)concat2Iterator._second;
					int count3 = collection2.Count;
					if (count3 > 0)
					{
						collection2.CopyTo(array, num - count3);
					}
					if (num > count3)
					{
						((ICollection<TSource>)concat2Iterator._first).CopyTo(array, 0);
					}
					return array;
				}
			}

			// Token: 0x04000465 RID: 1125
			private readonly Enumerable.ConcatIterator<TSource> _tail;

			// Token: 0x04000466 RID: 1126
			private readonly IEnumerable<TSource> _head;

			// Token: 0x04000467 RID: 1127
			private readonly int _headIndex;

			// Token: 0x04000468 RID: 1128
			private readonly bool _hasOnlyCollections;
		}

		// Token: 0x0200009F RID: 159
		private abstract class ConcatIterator<TSource> : Enumerable.Iterator<TSource>, IIListProvider<TSource>, IEnumerable<TSource>, IEnumerable
		{
			// Token: 0x060005D0 RID: 1488 RVA: 0x000153ED File Offset: 0x000135ED
			public override void Dispose()
			{
				if (this._enumerator != null)
				{
					this._enumerator.Dispose();
					this._enumerator = null;
				}
				base.Dispose();
			}

			// Token: 0x060005D1 RID: 1489
			internal abstract IEnumerable<TSource> GetEnumerable(int index);

			// Token: 0x060005D2 RID: 1490
			internal abstract Enumerable.ConcatIterator<TSource> Concat(IEnumerable<TSource> next);

			// Token: 0x060005D3 RID: 1491 RVA: 0x00015410 File Offset: 0x00013610
			public override bool MoveNext()
			{
				if (this._state == 1)
				{
					this._enumerator = this.GetEnumerable(0).GetEnumerator();
					this._state = 2;
				}
				if (this._state > 1)
				{
					while (!this._enumerator.MoveNext())
					{
						int state = this._state;
						this._state = state + 1;
						IEnumerable<TSource> enumerable = this.GetEnumerable(state - 1);
						if (enumerable == null)
						{
							this.Dispose();
							return false;
						}
						this._enumerator.Dispose();
						this._enumerator = enumerable.GetEnumerator();
					}
					this._current = this._enumerator.Current;
					return true;
				}
				return false;
			}

			// Token: 0x060005D4 RID: 1492
			public abstract int GetCount(bool onlyIfCheap);

			// Token: 0x060005D5 RID: 1493
			public abstract TSource[] ToArray();

			// Token: 0x060005D6 RID: 1494 RVA: 0x000154A8 File Offset: 0x000136A8
			public List<TSource> ToList()
			{
				int count = this.GetCount(true);
				List<TSource> list = ((count != -1) ? new List<TSource>(count) : new List<TSource>());
				int num = 0;
				for (;;)
				{
					IEnumerable<TSource> enumerable = this.GetEnumerable(num);
					if (enumerable == null)
					{
						break;
					}
					list.AddRange(enumerable);
					num++;
				}
				return list;
			}

			// Token: 0x04000469 RID: 1129
			private IEnumerator<TSource> _enumerator;
		}

		// Token: 0x020000A0 RID: 160
		private sealed class DefaultIfEmptyIterator<TSource> : Enumerable.Iterator<TSource>, IIListProvider<TSource>, IEnumerable<TSource>, IEnumerable
		{
			// Token: 0x060005D8 RID: 1496 RVA: 0x000154F2 File Offset: 0x000136F2
			public DefaultIfEmptyIterator(IEnumerable<TSource> source, TSource defaultValue)
			{
				this._source = source;
				this._default = defaultValue;
			}

			// Token: 0x060005D9 RID: 1497 RVA: 0x00015508 File Offset: 0x00013708
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.DefaultIfEmptyIterator<TSource>(this._source, this._default);
			}

			// Token: 0x060005DA RID: 1498 RVA: 0x0001551C File Offset: 0x0001371C
			public override bool MoveNext()
			{
				int state = this._state;
				if (state != 1)
				{
					if (state == 2)
					{
						if (this._enumerator.MoveNext())
						{
							this._current = this._enumerator.Current;
							return true;
						}
					}
					this.Dispose();
					return false;
				}
				this._enumerator = this._source.GetEnumerator();
				if (this._enumerator.MoveNext())
				{
					this._current = this._enumerator.Current;
					this._state = 2;
				}
				else
				{
					this._current = this._default;
					this._state = -1;
				}
				return true;
			}

			// Token: 0x060005DB RID: 1499 RVA: 0x000155AE File Offset: 0x000137AE
			public override void Dispose()
			{
				if (this._enumerator != null)
				{
					this._enumerator.Dispose();
					this._enumerator = null;
				}
				base.Dispose();
			}

			// Token: 0x060005DC RID: 1500 RVA: 0x000155D0 File Offset: 0x000137D0
			public TSource[] ToArray()
			{
				TSource[] array = this._source.ToArray<TSource>();
				if (array.Length != 0)
				{
					return array;
				}
				return new TSource[] { this._default };
			}

			// Token: 0x060005DD RID: 1501 RVA: 0x00015604 File Offset: 0x00013804
			public List<TSource> ToList()
			{
				List<TSource> list = this._source.ToList<TSource>();
				if (list.Count == 0)
				{
					list.Add(this._default);
				}
				return list;
			}

			// Token: 0x060005DE RID: 1502 RVA: 0x00015634 File Offset: 0x00013834
			public int GetCount(bool onlyIfCheap)
			{
				int num;
				if (!onlyIfCheap || this._source is ICollection<TSource> || this._source is ICollection)
				{
					num = this._source.Count<TSource>();
				}
				else
				{
					IIListProvider<TSource> iilistProvider = this._source as IIListProvider<TSource>;
					num = ((iilistProvider != null) ? iilistProvider.GetCount(true) : (-1));
				}
				if (num != 0)
				{
					return num;
				}
				return 1;
			}

			// Token: 0x0400046A RID: 1130
			private readonly IEnumerable<TSource> _source;

			// Token: 0x0400046B RID: 1131
			private readonly TSource _default;

			// Token: 0x0400046C RID: 1132
			private IEnumerator<TSource> _enumerator;
		}

		// Token: 0x020000A1 RID: 161
		private sealed class DistinctIterator<TSource> : Enumerable.Iterator<TSource>, IIListProvider<TSource>, IEnumerable<TSource>, IEnumerable
		{
			// Token: 0x060005DF RID: 1503 RVA: 0x0001568C File Offset: 0x0001388C
			public DistinctIterator(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
			{
				this._source = source;
				this._comparer = comparer;
			}

			// Token: 0x060005E0 RID: 1504 RVA: 0x000156A2 File Offset: 0x000138A2
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.DistinctIterator<TSource>(this._source, this._comparer);
			}

			// Token: 0x060005E1 RID: 1505 RVA: 0x000156B8 File Offset: 0x000138B8
			public override bool MoveNext()
			{
				int state = this._state;
				TSource tsource;
				if (state != 1)
				{
					if (state == 2)
					{
						while (this._enumerator.MoveNext())
						{
							tsource = this._enumerator.Current;
							if (this._set.Add(tsource))
							{
								this._current = tsource;
								return true;
							}
						}
					}
					this.Dispose();
					return false;
				}
				this._enumerator = this._source.GetEnumerator();
				if (!this._enumerator.MoveNext())
				{
					this.Dispose();
					return false;
				}
				tsource = this._enumerator.Current;
				this._set = new Set<TSource>(this._comparer);
				this._set.Add(tsource);
				this._current = tsource;
				this._state = 2;
				return true;
			}

			// Token: 0x060005E2 RID: 1506 RVA: 0x00015773 File Offset: 0x00013973
			public override void Dispose()
			{
				if (this._enumerator != null)
				{
					this._enumerator.Dispose();
					this._enumerator = null;
					this._set = null;
				}
				base.Dispose();
			}

			// Token: 0x060005E3 RID: 1507 RVA: 0x0001579C File Offset: 0x0001399C
			private Set<TSource> FillSet()
			{
				Set<TSource> set = new Set<TSource>(this._comparer);
				set.UnionWith(this._source);
				return set;
			}

			// Token: 0x060005E4 RID: 1508 RVA: 0x000157B5 File Offset: 0x000139B5
			public TSource[] ToArray()
			{
				return this.FillSet().ToArray();
			}

			// Token: 0x060005E5 RID: 1509 RVA: 0x000157C2 File Offset: 0x000139C2
			public List<TSource> ToList()
			{
				return this.FillSet().ToList();
			}

			// Token: 0x060005E6 RID: 1510 RVA: 0x000157CF File Offset: 0x000139CF
			public int GetCount(bool onlyIfCheap)
			{
				if (!onlyIfCheap)
				{
					return this.FillSet().Count;
				}
				return -1;
			}

			// Token: 0x0400046D RID: 1133
			private readonly IEnumerable<TSource> _source;

			// Token: 0x0400046E RID: 1134
			private readonly IEqualityComparer<TSource> _comparer;

			// Token: 0x0400046F RID: 1135
			private Set<TSource> _set;

			// Token: 0x04000470 RID: 1136
			private IEnumerator<TSource> _enumerator;
		}

		// Token: 0x020000A2 RID: 162
		internal abstract class Iterator<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IDisposable, IEnumerator
		{
			// Token: 0x060005E7 RID: 1511 RVA: 0x000157E1 File Offset: 0x000139E1
			protected Iterator()
			{
				this._threadId = Environment.CurrentManagedThreadId;
			}

			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x060005E8 RID: 1512 RVA: 0x000157F4 File Offset: 0x000139F4
			public TSource Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x060005E9 RID: 1513
			public abstract Enumerable.Iterator<TSource> Clone();

			// Token: 0x060005EA RID: 1514 RVA: 0x000157FC File Offset: 0x000139FC
			public virtual void Dispose()
			{
				this._current = default(TSource);
				this._state = -1;
			}

			// Token: 0x060005EB RID: 1515 RVA: 0x00015811 File Offset: 0x00013A11
			public IEnumerator<TSource> GetEnumerator()
			{
				Enumerable.Iterator iterator = ((this._state == 0 && this._threadId == Environment.CurrentManagedThreadId) ? this : this.Clone());
				iterator._state = 1;
				return iterator;
			}

			// Token: 0x060005EC RID: 1516
			public abstract bool MoveNext();

			// Token: 0x060005ED RID: 1517 RVA: 0x00015838 File Offset: 0x00013A38
			public virtual IEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
			{
				return new Enumerable.SelectEnumerableIterator<TSource, TResult>(this, selector);
			}

			// Token: 0x060005EE RID: 1518 RVA: 0x00015841 File Offset: 0x00013A41
			public virtual IEnumerable<TSource> Where(Func<TSource, bool> predicate)
			{
				return new Enumerable.WhereEnumerableIterator<TSource>(this, predicate);
			}

			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x060005EF RID: 1519 RVA: 0x0001584A File Offset: 0x00013A4A
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060005F0 RID: 1520 RVA: 0x00015857 File Offset: 0x00013A57
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060005F1 RID: 1521 RVA: 0x0001585F File Offset: 0x00013A5F
			void IEnumerator.Reset()
			{
				throw Error.NotSupported();
			}

			// Token: 0x04000471 RID: 1137
			private readonly int _threadId;

			// Token: 0x04000472 RID: 1138
			internal int _state;

			// Token: 0x04000473 RID: 1139
			internal TSource _current;
		}

		// Token: 0x020000A3 RID: 163
		private sealed class ListPartition<TSource> : Enumerable.Iterator<TSource>, IPartition<TSource>, IIListProvider<TSource>, IEnumerable<TSource>, IEnumerable
		{
			// Token: 0x060005F2 RID: 1522 RVA: 0x00015866 File Offset: 0x00013A66
			public ListPartition(IList<TSource> source, int minIndexInclusive, int maxIndexInclusive)
			{
				this._source = source;
				this._minIndexInclusive = minIndexInclusive;
				this._maxIndexInclusive = maxIndexInclusive;
			}

			// Token: 0x060005F3 RID: 1523 RVA: 0x00015883 File Offset: 0x00013A83
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.ListPartition<TSource>(this._source, this._minIndexInclusive, this._maxIndexInclusive);
			}

			// Token: 0x060005F4 RID: 1524 RVA: 0x0001589C File Offset: 0x00013A9C
			public override bool MoveNext()
			{
				int num = this._state - 1;
				if (num <= this._maxIndexInclusive - this._minIndexInclusive && num < this._source.Count - this._minIndexInclusive)
				{
					this._current = this._source[this._minIndexInclusive + num];
					this._state++;
					return true;
				}
				this.Dispose();
				return false;
			}

			// Token: 0x060005F5 RID: 1525 RVA: 0x00015907 File Offset: 0x00013B07
			public override IEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
			{
				return new Enumerable.SelectListPartitionIterator<TSource, TResult>(this._source, selector, this._minIndexInclusive, this._maxIndexInclusive);
			}

			// Token: 0x060005F6 RID: 1526 RVA: 0x00015924 File Offset: 0x00013B24
			public IPartition<TSource> Skip(int count)
			{
				int num = this._minIndexInclusive + count;
				if (num <= this._maxIndexInclusive)
				{
					return new Enumerable.ListPartition<TSource>(this._source, num, this._maxIndexInclusive);
				}
				return EmptyPartition<TSource>.Instance;
			}

			// Token: 0x060005F7 RID: 1527 RVA: 0x00015960 File Offset: 0x00013B60
			public IPartition<TSource> Take(int count)
			{
				int num = this._minIndexInclusive + count - 1;
				if (num < this._maxIndexInclusive)
				{
					return new Enumerable.ListPartition<TSource>(this._source, this._minIndexInclusive, num);
				}
				return this;
			}

			// Token: 0x060005F8 RID: 1528 RVA: 0x00015998 File Offset: 0x00013B98
			public TSource TryGetElementAt(int index, out bool found)
			{
				if (index <= this._maxIndexInclusive - this._minIndexInclusive && index < this._source.Count - this._minIndexInclusive)
				{
					found = true;
					return this._source[this._minIndexInclusive + index];
				}
				found = false;
				return default(TSource);
			}

			// Token: 0x060005F9 RID: 1529 RVA: 0x000159F0 File Offset: 0x00013BF0
			public TSource TryGetFirst(out bool found)
			{
				if (this._source.Count > this._minIndexInclusive)
				{
					found = true;
					return this._source[this._minIndexInclusive];
				}
				found = false;
				return default(TSource);
			}

			// Token: 0x060005FA RID: 1530 RVA: 0x00015A34 File Offset: 0x00013C34
			public TSource TryGetLast(out bool found)
			{
				int num = this._source.Count - 1;
				if (num >= this._minIndexInclusive)
				{
					found = true;
					return this._source[Math.Min(num, this._maxIndexInclusive)];
				}
				found = false;
				return default(TSource);
			}

			// Token: 0x170000AA RID: 170
			// (get) Token: 0x060005FB RID: 1531 RVA: 0x00015A80 File Offset: 0x00013C80
			private int Count
			{
				get
				{
					int count = this._source.Count;
					if (count <= this._minIndexInclusive)
					{
						return 0;
					}
					return Math.Min(count - 1, this._maxIndexInclusive) - this._minIndexInclusive + 1;
				}
			}

			// Token: 0x060005FC RID: 1532 RVA: 0x00015ABC File Offset: 0x00013CBC
			public TSource[] ToArray()
			{
				int count = this.Count;
				if (count == 0)
				{
					return Array.Empty<TSource>();
				}
				TSource[] array = new TSource[count];
				int num = 0;
				int num2 = this._minIndexInclusive;
				while (num != array.Length)
				{
					array[num] = this._source[num2];
					num++;
					num2++;
				}
				return array;
			}

			// Token: 0x060005FD RID: 1533 RVA: 0x00015B10 File Offset: 0x00013D10
			public List<TSource> ToList()
			{
				int count = this.Count;
				if (count == 0)
				{
					return new List<TSource>();
				}
				List<TSource> list = new List<TSource>(count);
				int num = this._minIndexInclusive + count;
				for (int num2 = this._minIndexInclusive; num2 != num; num2++)
				{
					list.Add(this._source[num2]);
				}
				return list;
			}

			// Token: 0x060005FE RID: 1534 RVA: 0x00015B61 File Offset: 0x00013D61
			public int GetCount(bool onlyIfCheap)
			{
				return this.Count;
			}

			// Token: 0x04000474 RID: 1140
			private readonly IList<TSource> _source;

			// Token: 0x04000475 RID: 1141
			private readonly int _minIndexInclusive;

			// Token: 0x04000476 RID: 1142
			private readonly int _maxIndexInclusive;
		}

		// Token: 0x020000A4 RID: 164
		private sealed class EnumerablePartition<TSource> : Enumerable.Iterator<TSource>, IPartition<TSource>, IIListProvider<TSource>, IEnumerable<TSource>, IEnumerable
		{
			// Token: 0x060005FF RID: 1535 RVA: 0x00015B69 File Offset: 0x00013D69
			internal EnumerablePartition(IEnumerable<TSource> source, int minIndexInclusive, int maxIndexInclusive)
			{
				this._source = source;
				this._minIndexInclusive = minIndexInclusive;
				this._maxIndexInclusive = maxIndexInclusive;
			}

			// Token: 0x170000AB RID: 171
			// (get) Token: 0x06000600 RID: 1536 RVA: 0x00015B86 File Offset: 0x00013D86
			private bool HasLimit
			{
				get
				{
					return this._maxIndexInclusive != -1;
				}
			}

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x06000601 RID: 1537 RVA: 0x00015B94 File Offset: 0x00013D94
			private int Limit
			{
				get
				{
					return this._maxIndexInclusive + 1 - this._minIndexInclusive;
				}
			}

			// Token: 0x06000602 RID: 1538 RVA: 0x00015BA5 File Offset: 0x00013DA5
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.EnumerablePartition<TSource>(this._source, this._minIndexInclusive, this._maxIndexInclusive);
			}

			// Token: 0x06000603 RID: 1539 RVA: 0x00015BBE File Offset: 0x00013DBE
			public override void Dispose()
			{
				if (this._enumerator != null)
				{
					this._enumerator.Dispose();
					this._enumerator = null;
				}
				base.Dispose();
			}

			// Token: 0x06000604 RID: 1540 RVA: 0x00015BE0 File Offset: 0x00013DE0
			public int GetCount(bool onlyIfCheap)
			{
				if (onlyIfCheap)
				{
					return -1;
				}
				if (!this.HasLimit)
				{
					return Math.Max(this._source.Count<TSource>() - this._minIndexInclusive, 0);
				}
				int num;
				using (IEnumerator<TSource> enumerator = this._source.GetEnumerator())
				{
					num = Math.Max((int)(Enumerable.EnumerablePartition<TSource>.SkipAndCount((uint)(this._maxIndexInclusive + 1), enumerator) - (uint)this._minIndexInclusive), 0);
				}
				return num;
			}

			// Token: 0x06000605 RID: 1541 RVA: 0x00015C58 File Offset: 0x00013E58
			public override bool MoveNext()
			{
				int num = this._state - 3;
				if (num < -2)
				{
					this.Dispose();
					return false;
				}
				int state = this._state;
				if (state != 1)
				{
					if (state != 2)
					{
						goto IL_0054;
					}
				}
				else
				{
					this._enumerator = this._source.GetEnumerator();
					this._state = 2;
				}
				if (!this.SkipBeforeFirst(this._enumerator))
				{
					goto IL_009B;
				}
				this._state = 3;
				IL_0054:
				if ((!this.HasLimit || num < this.Limit) && this._enumerator.MoveNext())
				{
					if (this.HasLimit)
					{
						this._state++;
					}
					this._current = this._enumerator.Current;
					return true;
				}
				IL_009B:
				this.Dispose();
				return false;
			}

			// Token: 0x06000606 RID: 1542 RVA: 0x00015D07 File Offset: 0x00013F07
			public override IEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
			{
				return new Enumerable.SelectIPartitionIterator<TSource, TResult>(this, selector);
			}

			// Token: 0x06000607 RID: 1543 RVA: 0x00015D10 File Offset: 0x00013F10
			public IPartition<TSource> Skip(int count)
			{
				int num = this._minIndexInclusive + count;
				if (!this.HasLimit)
				{
					if (num < 0)
					{
						return new Enumerable.EnumerablePartition<TSource>(this, count, -1);
					}
				}
				else if (num > this._maxIndexInclusive)
				{
					return EmptyPartition<TSource>.Instance;
				}
				return new Enumerable.EnumerablePartition<TSource>(this._source, num, this._maxIndexInclusive);
			}

			// Token: 0x06000608 RID: 1544 RVA: 0x00015D5C File Offset: 0x00013F5C
			public IPartition<TSource> Take(int count)
			{
				int num = this._minIndexInclusive + count - 1;
				if (!this.HasLimit)
				{
					if (num < 0)
					{
						return new Enumerable.EnumerablePartition<TSource>(this, 0, count - 1);
					}
				}
				else if (num >= this._maxIndexInclusive)
				{
					return this;
				}
				return new Enumerable.EnumerablePartition<TSource>(this._source, this._minIndexInclusive, num);
			}

			// Token: 0x06000609 RID: 1545 RVA: 0x00015DA8 File Offset: 0x00013FA8
			public TSource TryGetElementAt(int index, out bool found)
			{
				if (index >= 0 && (!this.HasLimit || index < this.Limit))
				{
					using (IEnumerator<TSource> enumerator = this._source.GetEnumerator())
					{
						if (Enumerable.EnumerablePartition<TSource>.SkipBefore(this._minIndexInclusive + index, enumerator) && enumerator.MoveNext())
						{
							found = true;
							return enumerator.Current;
						}
					}
				}
				found = false;
				return default(TSource);
			}

			// Token: 0x0600060A RID: 1546 RVA: 0x00015E24 File Offset: 0x00014024
			public TSource TryGetFirst(out bool found)
			{
				using (IEnumerator<TSource> enumerator = this._source.GetEnumerator())
				{
					if (this.SkipBeforeFirst(enumerator) && enumerator.MoveNext())
					{
						found = true;
						return enumerator.Current;
					}
				}
				found = false;
				return default(TSource);
			}

			// Token: 0x0600060B RID: 1547 RVA: 0x00015E84 File Offset: 0x00014084
			public TSource TryGetLast(out bool found)
			{
				using (IEnumerator<TSource> enumerator = this._source.GetEnumerator())
				{
					if (this.SkipBeforeFirst(enumerator) && enumerator.MoveNext())
					{
						int num = this.Limit - 1;
						int num2 = (this.HasLimit ? 0 : int.MinValue);
						TSource tsource;
						do
						{
							num--;
							tsource = enumerator.Current;
						}
						while (num >= num2 && enumerator.MoveNext());
						found = true;
						return tsource;
					}
				}
				found = false;
				return default(TSource);
			}

			// Token: 0x0600060C RID: 1548 RVA: 0x00015F14 File Offset: 0x00014114
			public TSource[] ToArray()
			{
				using (IEnumerator<TSource> enumerator = this._source.GetEnumerator())
				{
					if (this.SkipBeforeFirst(enumerator) && enumerator.MoveNext())
					{
						int num = this.Limit - 1;
						int num2 = (this.HasLimit ? 0 : int.MinValue);
						int num3 = (this.HasLimit ? this.Limit : int.MaxValue);
						global::System.Collections.Generic.LargeArrayBuilder<TSource> largeArrayBuilder = new global::System.Collections.Generic.LargeArrayBuilder<TSource>(num3);
						do
						{
							num--;
							largeArrayBuilder.Add(enumerator.Current);
						}
						while (num >= num2 && enumerator.MoveNext());
						return largeArrayBuilder.ToArray();
					}
				}
				return Array.Empty<TSource>();
			}

			// Token: 0x0600060D RID: 1549 RVA: 0x00015FC4 File Offset: 0x000141C4
			public List<TSource> ToList()
			{
				List<TSource> list = new List<TSource>();
				using (IEnumerator<TSource> enumerator = this._source.GetEnumerator())
				{
					if (this.SkipBeforeFirst(enumerator) && enumerator.MoveNext())
					{
						int num = this.Limit - 1;
						int num2 = (this.HasLimit ? 0 : int.MinValue);
						do
						{
							num--;
							list.Add(enumerator.Current);
						}
						while (num >= num2 && enumerator.MoveNext());
					}
				}
				return list;
			}

			// Token: 0x0600060E RID: 1550 RVA: 0x00016048 File Offset: 0x00014248
			private bool SkipBeforeFirst(IEnumerator<TSource> en)
			{
				return Enumerable.EnumerablePartition<TSource>.SkipBefore(this._minIndexInclusive, en);
			}

			// Token: 0x0600060F RID: 1551 RVA: 0x00016056 File Offset: 0x00014256
			private static bool SkipBefore(int index, IEnumerator<TSource> en)
			{
				return Enumerable.EnumerablePartition<TSource>.SkipAndCount(index, en) == index;
			}

			// Token: 0x06000610 RID: 1552 RVA: 0x00016062 File Offset: 0x00014262
			private static int SkipAndCount(int index, IEnumerator<TSource> en)
			{
				return (int)Enumerable.EnumerablePartition<TSource>.SkipAndCount((uint)index, en);
			}

			// Token: 0x06000611 RID: 1553 RVA: 0x0001606C File Offset: 0x0001426C
			private static uint SkipAndCount(uint index, IEnumerator<TSource> en)
			{
				for (uint num = 0U; num < index; num += 1U)
				{
					if (!en.MoveNext())
					{
						return num;
					}
				}
				return index;
			}

			// Token: 0x04000477 RID: 1143
			private readonly IEnumerable<TSource> _source;

			// Token: 0x04000478 RID: 1144
			private readonly int _minIndexInclusive;

			// Token: 0x04000479 RID: 1145
			private readonly int _maxIndexInclusive;

			// Token: 0x0400047A RID: 1146
			private IEnumerator<TSource> _enumerator;
		}

		// Token: 0x020000A5 RID: 165
		private sealed class RangeIterator : Enumerable.Iterator<int>, IPartition<int>, IIListProvider<int>, IEnumerable<int>, IEnumerable
		{
			// Token: 0x06000612 RID: 1554 RVA: 0x00016090 File Offset: 0x00014290
			public RangeIterator(int start, int count)
			{
				this._start = start;
				this._end = start + count;
			}

			// Token: 0x06000613 RID: 1555 RVA: 0x000160A8 File Offset: 0x000142A8
			public override Enumerable.Iterator<int> Clone()
			{
				return new Enumerable.RangeIterator(this._start, this._end - this._start);
			}

			// Token: 0x06000614 RID: 1556 RVA: 0x000160C4 File Offset: 0x000142C4
			public override bool MoveNext()
			{
				int state = this._state;
				if (state != 1)
				{
					if (state == 2)
					{
						int num = this._current + 1;
						this._current = num;
						if (num != this._end)
						{
							return true;
						}
					}
					this._state = -1;
					return false;
				}
				this._current = this._start;
				this._state = 2;
				return true;
			}

			// Token: 0x06000615 RID: 1557 RVA: 0x0001611A File Offset: 0x0001431A
			public override void Dispose()
			{
				this._state = -1;
			}

			// Token: 0x06000616 RID: 1558 RVA: 0x00016123 File Offset: 0x00014323
			public override IEnumerable<TResult> Select<TResult>(Func<int, TResult> selector)
			{
				return new Enumerable.SelectIPartitionIterator<int, TResult>(this, selector);
			}

			// Token: 0x06000617 RID: 1559 RVA: 0x0001612C File Offset: 0x0001432C
			public int[] ToArray()
			{
				int[] array = new int[this._end - this._start];
				int num = this._start;
				for (int num2 = 0; num2 != array.Length; num2++)
				{
					array[num2] = num;
					num++;
				}
				return array;
			}

			// Token: 0x06000618 RID: 1560 RVA: 0x0001616C File Offset: 0x0001436C
			public List<int> ToList()
			{
				List<int> list = new List<int>(this._end - this._start);
				for (int num = this._start; num != this._end; num++)
				{
					list.Add(num);
				}
				return list;
			}

			// Token: 0x06000619 RID: 1561 RVA: 0x000161AA File Offset: 0x000143AA
			public int GetCount(bool onlyIfCheap)
			{
				return this._end - this._start;
			}

			// Token: 0x0600061A RID: 1562 RVA: 0x000161B9 File Offset: 0x000143B9
			public IPartition<int> Skip(int count)
			{
				if (count >= this._end - this._start)
				{
					return EmptyPartition<int>.Instance;
				}
				return new Enumerable.RangeIterator(this._start + count, this._end - this._start - count);
			}

			// Token: 0x0600061B RID: 1563 RVA: 0x000161F0 File Offset: 0x000143F0
			public IPartition<int> Take(int count)
			{
				int num = this._end - this._start;
				if (count >= num)
				{
					return this;
				}
				return new Enumerable.RangeIterator(this._start, count);
			}

			// Token: 0x0600061C RID: 1564 RVA: 0x0001621D File Offset: 0x0001441D
			public int TryGetElementAt(int index, out bool found)
			{
				if (index < this._end - this._start)
				{
					found = true;
					return this._start + index;
				}
				found = false;
				return 0;
			}

			// Token: 0x0600061D RID: 1565 RVA: 0x0001623F File Offset: 0x0001443F
			public int TryGetFirst(out bool found)
			{
				found = true;
				return this._start;
			}

			// Token: 0x0600061E RID: 1566 RVA: 0x0001624A File Offset: 0x0001444A
			public int TryGetLast(out bool found)
			{
				found = true;
				return this._end - 1;
			}

			// Token: 0x0400047B RID: 1147
			private readonly int _start;

			// Token: 0x0400047C RID: 1148
			private readonly int _end;
		}

		// Token: 0x020000A6 RID: 166
		private sealed class RepeatIterator<TResult> : Enumerable.Iterator<TResult>, IPartition<TResult>, IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
		{
			// Token: 0x0600061F RID: 1567 RVA: 0x00016257 File Offset: 0x00014457
			public RepeatIterator(TResult element, int count)
			{
				this._current = element;
				this._count = count;
			}

			// Token: 0x06000620 RID: 1568 RVA: 0x0001626D File Offset: 0x0001446D
			public override Enumerable.Iterator<TResult> Clone()
			{
				return new Enumerable.RepeatIterator<TResult>(this._current, this._count);
			}

			// Token: 0x06000621 RID: 1569 RVA: 0x00016280 File Offset: 0x00014480
			public override void Dispose()
			{
				this._state = -1;
			}

			// Token: 0x06000622 RID: 1570 RVA: 0x0001628C File Offset: 0x0001448C
			public override bool MoveNext()
			{
				int num = this._state - 1;
				if (num >= 0 && num != this._count)
				{
					this._state++;
					return true;
				}
				this.Dispose();
				return false;
			}

			// Token: 0x06000623 RID: 1571 RVA: 0x00015D07 File Offset: 0x00013F07
			public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
			{
				return new Enumerable.SelectIPartitionIterator<TResult, TResult2>(this, selector);
			}

			// Token: 0x06000624 RID: 1572 RVA: 0x000162C8 File Offset: 0x000144C8
			public TResult[] ToArray()
			{
				TResult[] array = new TResult[this._count];
				if (this._current != null)
				{
					Array.Fill<TResult>(array, this._current);
				}
				return array;
			}

			// Token: 0x06000625 RID: 1573 RVA: 0x000162FC File Offset: 0x000144FC
			public List<TResult> ToList()
			{
				List<TResult> list = new List<TResult>(this._count);
				for (int num = 0; num != this._count; num++)
				{
					list.Add(this._current);
				}
				return list;
			}

			// Token: 0x06000626 RID: 1574 RVA: 0x00016333 File Offset: 0x00014533
			public int GetCount(bool onlyIfCheap)
			{
				return this._count;
			}

			// Token: 0x06000627 RID: 1575 RVA: 0x0001633B File Offset: 0x0001453B
			public IPartition<TResult> Skip(int count)
			{
				if (count >= this._count)
				{
					return EmptyPartition<TResult>.Instance;
				}
				return new Enumerable.RepeatIterator<TResult>(this._current, this._count - count);
			}

			// Token: 0x06000628 RID: 1576 RVA: 0x0001635F File Offset: 0x0001455F
			public IPartition<TResult> Take(int count)
			{
				if (count >= this._count)
				{
					return this;
				}
				return new Enumerable.RepeatIterator<TResult>(this._current, count);
			}

			// Token: 0x06000629 RID: 1577 RVA: 0x00016378 File Offset: 0x00014578
			public TResult TryGetElementAt(int index, out bool found)
			{
				if (index < this._count)
				{
					found = true;
					return this._current;
				}
				found = false;
				return default(TResult);
			}

			// Token: 0x0600062A RID: 1578 RVA: 0x000163A4 File Offset: 0x000145A4
			public TResult TryGetFirst(out bool found)
			{
				found = true;
				return this._current;
			}

			// Token: 0x0600062B RID: 1579 RVA: 0x000163A4 File Offset: 0x000145A4
			public TResult TryGetLast(out bool found)
			{
				found = true;
				return this._current;
			}

			// Token: 0x0400047D RID: 1149
			private readonly int _count;
		}

		// Token: 0x020000A7 RID: 167
		private sealed class ReverseIterator<TSource> : Enumerable.Iterator<TSource>, IIListProvider<TSource>, IEnumerable<TSource>, IEnumerable
		{
			// Token: 0x0600062C RID: 1580 RVA: 0x000163AF File Offset: 0x000145AF
			public ReverseIterator(IEnumerable<TSource> source)
			{
				this._source = source;
			}

			// Token: 0x0600062D RID: 1581 RVA: 0x000163BE File Offset: 0x000145BE
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.ReverseIterator<TSource>(this._source);
			}

			// Token: 0x0600062E RID: 1582 RVA: 0x000163CC File Offset: 0x000145CC
			public override bool MoveNext()
			{
				if (this._state - 2 <= -2)
				{
					this.Dispose();
					return false;
				}
				if (this._state == 1)
				{
					Buffer<TSource> buffer = new Buffer<TSource>(this._source);
					this._buffer = buffer._items;
					this._state = buffer._count + 2;
				}
				int num = this._state - 3;
				if (num != -1)
				{
					this._current = this._buffer[num];
					this._state--;
					return true;
				}
				this.Dispose();
				return false;
			}

			// Token: 0x0600062F RID: 1583 RVA: 0x00016453 File Offset: 0x00014653
			public override void Dispose()
			{
				this._buffer = null;
				base.Dispose();
			}

			// Token: 0x06000630 RID: 1584 RVA: 0x00016462 File Offset: 0x00014662
			public TSource[] ToArray()
			{
				TSource[] array = this._source.ToArray<TSource>();
				Array.Reverse<TSource>(array);
				return array;
			}

			// Token: 0x06000631 RID: 1585 RVA: 0x00016475 File Offset: 0x00014675
			public List<TSource> ToList()
			{
				List<TSource> list = this._source.ToList<TSource>();
				list.Reverse();
				return list;
			}

			// Token: 0x06000632 RID: 1586 RVA: 0x00016488 File Offset: 0x00014688
			public int GetCount(bool onlyIfCheap)
			{
				if (!onlyIfCheap)
				{
					return this._source.Count<TSource>();
				}
				IEnumerable<TSource> source = this._source;
				IIListProvider<TSource> iilistProvider = source as IIListProvider<TSource>;
				if (iilistProvider != null)
				{
					return iilistProvider.GetCount(true);
				}
				ICollection<TSource> collection = source as ICollection<TSource>;
				if (collection != null)
				{
					return collection.Count;
				}
				ICollection collection2 = source as ICollection;
				if (collection2 == null)
				{
					return -1;
				}
				return collection2.Count;
			}

			// Token: 0x0400047E RID: 1150
			private readonly IEnumerable<TSource> _source;

			// Token: 0x0400047F RID: 1151
			private TSource[] _buffer;
		}

		// Token: 0x020000A8 RID: 168
		private sealed class SelectEnumerableIterator<TSource, TResult> : Enumerable.Iterator<TResult>, IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
		{
			// Token: 0x06000633 RID: 1587 RVA: 0x000164E2 File Offset: 0x000146E2
			public SelectEnumerableIterator(IEnumerable<TSource> source, Func<TSource, TResult> selector)
			{
				this._source = source;
				this._selector = selector;
			}

			// Token: 0x06000634 RID: 1588 RVA: 0x000164F8 File Offset: 0x000146F8
			public override Enumerable.Iterator<TResult> Clone()
			{
				return new Enumerable.SelectEnumerableIterator<TSource, TResult>(this._source, this._selector);
			}

			// Token: 0x06000635 RID: 1589 RVA: 0x0001650B File Offset: 0x0001470B
			public override void Dispose()
			{
				if (this._enumerator != null)
				{
					this._enumerator.Dispose();
					this._enumerator = null;
				}
				base.Dispose();
			}

			// Token: 0x06000636 RID: 1590 RVA: 0x00016530 File Offset: 0x00014730
			public override bool MoveNext()
			{
				int state = this._state;
				if (state != 1)
				{
					if (state != 2)
					{
						return false;
					}
				}
				else
				{
					this._enumerator = this._source.GetEnumerator();
					this._state = 2;
				}
				if (this._enumerator.MoveNext())
				{
					this._current = this._selector(this._enumerator.Current);
					return true;
				}
				this.Dispose();
				return false;
			}

			// Token: 0x06000637 RID: 1591 RVA: 0x00016598 File Offset: 0x00014798
			public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
			{
				return new Enumerable.SelectEnumerableIterator<TSource, TResult2>(this._source, Utilities.CombineSelectors<TSource, TResult, TResult2>(this._selector, selector));
			}

			// Token: 0x06000638 RID: 1592 RVA: 0x000165B4 File Offset: 0x000147B4
			public TResult[] ToArray()
			{
				global::System.Collections.Generic.LargeArrayBuilder<TResult> largeArrayBuilder = new global::System.Collections.Generic.LargeArrayBuilder<TResult>(true);
				foreach (TSource tsource in this._source)
				{
					largeArrayBuilder.Add(this._selector(tsource));
				}
				return largeArrayBuilder.ToArray();
			}

			// Token: 0x06000639 RID: 1593 RVA: 0x0001661C File Offset: 0x0001481C
			public List<TResult> ToList()
			{
				List<TResult> list = new List<TResult>();
				foreach (TSource tsource in this._source)
				{
					list.Add(this._selector(tsource));
				}
				return list;
			}

			// Token: 0x0600063A RID: 1594 RVA: 0x0001667C File Offset: 0x0001487C
			public int GetCount(bool onlyIfCheap)
			{
				if (onlyIfCheap)
				{
					return -1;
				}
				int num = 0;
				checked
				{
					foreach (TSource tsource in this._source)
					{
						this._selector(tsource);
						num++;
					}
					return num;
				}
			}

			// Token: 0x04000480 RID: 1152
			private readonly IEnumerable<TSource> _source;

			// Token: 0x04000481 RID: 1153
			private readonly Func<TSource, TResult> _selector;

			// Token: 0x04000482 RID: 1154
			private IEnumerator<TSource> _enumerator;
		}

		// Token: 0x020000A9 RID: 169
		private sealed class SelectArrayIterator<TSource, TResult> : Enumerable.Iterator<TResult>, IPartition<TResult>, IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
		{
			// Token: 0x0600063B RID: 1595 RVA: 0x000166DC File Offset: 0x000148DC
			public SelectArrayIterator(TSource[] source, Func<TSource, TResult> selector)
			{
				this._source = source;
				this._selector = selector;
			}

			// Token: 0x0600063C RID: 1596 RVA: 0x000166F2 File Offset: 0x000148F2
			public override Enumerable.Iterator<TResult> Clone()
			{
				return new Enumerable.SelectArrayIterator<TSource, TResult>(this._source, this._selector);
			}

			// Token: 0x0600063D RID: 1597 RVA: 0x00016708 File Offset: 0x00014908
			public override bool MoveNext()
			{
				if ((this._state < 1) | (this._state == this._source.Length + 1))
				{
					this.Dispose();
					return false;
				}
				int state = this._state;
				this._state = state + 1;
				int num = state - 1;
				this._current = this._selector(this._source[num]);
				return true;
			}

			// Token: 0x0600063E RID: 1598 RVA: 0x0001676D File Offset: 0x0001496D
			public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
			{
				return new Enumerable.SelectArrayIterator<TSource, TResult2>(this._source, Utilities.CombineSelectors<TSource, TResult, TResult2>(this._selector, selector));
			}

			// Token: 0x0600063F RID: 1599 RVA: 0x00016788 File Offset: 0x00014988
			public TResult[] ToArray()
			{
				TResult[] array = new TResult[this._source.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._selector(this._source[i]);
				}
				return array;
			}

			// Token: 0x06000640 RID: 1600 RVA: 0x000167D0 File Offset: 0x000149D0
			public List<TResult> ToList()
			{
				TSource[] source = this._source;
				List<TResult> list = new List<TResult>(source.Length);
				for (int i = 0; i < source.Length; i++)
				{
					list.Add(this._selector(source[i]));
				}
				return list;
			}

			// Token: 0x06000641 RID: 1601 RVA: 0x00016814 File Offset: 0x00014A14
			public int GetCount(bool onlyIfCheap)
			{
				if (!onlyIfCheap)
				{
					foreach (TSource tsource in this._source)
					{
						this._selector(tsource);
					}
				}
				return this._source.Length;
			}

			// Token: 0x06000642 RID: 1602 RVA: 0x00016856 File Offset: 0x00014A56
			public IPartition<TResult> Skip(int count)
			{
				if (count >= this._source.Length)
				{
					return EmptyPartition<TResult>.Instance;
				}
				return new Enumerable.SelectListPartitionIterator<TSource, TResult>(this._source, this._selector, count, int.MaxValue);
			}

			// Token: 0x06000643 RID: 1603 RVA: 0x00016880 File Offset: 0x00014A80
			public IPartition<TResult> Take(int count)
			{
				if (count < this._source.Length)
				{
					return new Enumerable.SelectListPartitionIterator<TSource, TResult>(this._source, this._selector, 0, count - 1);
				}
				return this;
			}

			// Token: 0x06000644 RID: 1604 RVA: 0x000168B4 File Offset: 0x00014AB4
			public TResult TryGetElementAt(int index, out bool found)
			{
				if (index < this._source.Length)
				{
					found = true;
					return this._selector(this._source[index]);
				}
				found = false;
				return default(TResult);
			}

			// Token: 0x06000645 RID: 1605 RVA: 0x000168F3 File Offset: 0x00014AF3
			public TResult TryGetFirst(out bool found)
			{
				found = true;
				return this._selector(this._source[0]);
			}

			// Token: 0x06000646 RID: 1606 RVA: 0x0001690F File Offset: 0x00014B0F
			public TResult TryGetLast(out bool found)
			{
				found = true;
				return this._selector(this._source[this._source.Length - 1]);
			}

			// Token: 0x04000483 RID: 1155
			private readonly TSource[] _source;

			// Token: 0x04000484 RID: 1156
			private readonly Func<TSource, TResult> _selector;
		}

		// Token: 0x020000AA RID: 170
		private sealed class SelectListIterator<TSource, TResult> : Enumerable.Iterator<TResult>, IPartition<TResult>, IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
		{
			// Token: 0x06000647 RID: 1607 RVA: 0x00016934 File Offset: 0x00014B34
			public SelectListIterator(List<TSource> source, Func<TSource, TResult> selector)
			{
				this._source = source;
				this._selector = selector;
			}

			// Token: 0x06000648 RID: 1608 RVA: 0x0001694A File Offset: 0x00014B4A
			public override Enumerable.Iterator<TResult> Clone()
			{
				return new Enumerable.SelectListIterator<TSource, TResult>(this._source, this._selector);
			}

			// Token: 0x06000649 RID: 1609 RVA: 0x00016960 File Offset: 0x00014B60
			public override bool MoveNext()
			{
				int state = this._state;
				if (state != 1)
				{
					if (state != 2)
					{
						return false;
					}
				}
				else
				{
					this._enumerator = this._source.GetEnumerator();
					this._state = 2;
				}
				if (this._enumerator.MoveNext())
				{
					this._current = this._selector(this._enumerator.Current);
					return true;
				}
				this.Dispose();
				return false;
			}

			// Token: 0x0600064A RID: 1610 RVA: 0x000169C8 File Offset: 0x00014BC8
			public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
			{
				return new Enumerable.SelectListIterator<TSource, TResult2>(this._source, Utilities.CombineSelectors<TSource, TResult, TResult2>(this._selector, selector));
			}

			// Token: 0x0600064B RID: 1611 RVA: 0x000169E4 File Offset: 0x00014BE4
			public TResult[] ToArray()
			{
				int count = this._source.Count;
				if (count == 0)
				{
					return Array.Empty<TResult>();
				}
				TResult[] array = new TResult[count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._selector(this._source[i]);
				}
				return array;
			}

			// Token: 0x0600064C RID: 1612 RVA: 0x00016A3C File Offset: 0x00014C3C
			public List<TResult> ToList()
			{
				int count = this._source.Count;
				List<TResult> list = new List<TResult>(count);
				for (int i = 0; i < count; i++)
				{
					list.Add(this._selector(this._source[i]));
				}
				return list;
			}

			// Token: 0x0600064D RID: 1613 RVA: 0x00016A88 File Offset: 0x00014C88
			public int GetCount(bool onlyIfCheap)
			{
				int count = this._source.Count;
				if (!onlyIfCheap)
				{
					for (int i = 0; i < count; i++)
					{
						this._selector(this._source[i]);
					}
				}
				return count;
			}

			// Token: 0x0600064E RID: 1614 RVA: 0x00016AC9 File Offset: 0x00014CC9
			public IPartition<TResult> Skip(int count)
			{
				return new Enumerable.SelectListPartitionIterator<TSource, TResult>(this._source, this._selector, count, int.MaxValue);
			}

			// Token: 0x0600064F RID: 1615 RVA: 0x00016AE2 File Offset: 0x00014CE2
			public IPartition<TResult> Take(int count)
			{
				return new Enumerable.SelectListPartitionIterator<TSource, TResult>(this._source, this._selector, 0, count - 1);
			}

			// Token: 0x06000650 RID: 1616 RVA: 0x00016AFC File Offset: 0x00014CFC
			public TResult TryGetElementAt(int index, out bool found)
			{
				if (index < this._source.Count)
				{
					found = true;
					return this._selector(this._source[index]);
				}
				found = false;
				return default(TResult);
			}

			// Token: 0x06000651 RID: 1617 RVA: 0x00016B40 File Offset: 0x00014D40
			public TResult TryGetFirst(out bool found)
			{
				if (this._source.Count != 0)
				{
					found = true;
					return this._selector(this._source[0]);
				}
				found = false;
				return default(TResult);
			}

			// Token: 0x06000652 RID: 1618 RVA: 0x00016B84 File Offset: 0x00014D84
			public TResult TryGetLast(out bool found)
			{
				int count = this._source.Count;
				if (count != 0)
				{
					found = true;
					return this._selector(this._source[count - 1]);
				}
				found = false;
				return default(TResult);
			}

			// Token: 0x04000485 RID: 1157
			private readonly List<TSource> _source;

			// Token: 0x04000486 RID: 1158
			private readonly Func<TSource, TResult> _selector;

			// Token: 0x04000487 RID: 1159
			private List<TSource>.Enumerator _enumerator;
		}

		// Token: 0x020000AB RID: 171
		private sealed class SelectIListIterator<TSource, TResult> : Enumerable.Iterator<TResult>, IPartition<TResult>, IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
		{
			// Token: 0x06000653 RID: 1619 RVA: 0x00016BC9 File Offset: 0x00014DC9
			public SelectIListIterator(IList<TSource> source, Func<TSource, TResult> selector)
			{
				this._source = source;
				this._selector = selector;
			}

			// Token: 0x06000654 RID: 1620 RVA: 0x00016BDF File Offset: 0x00014DDF
			public override Enumerable.Iterator<TResult> Clone()
			{
				return new Enumerable.SelectIListIterator<TSource, TResult>(this._source, this._selector);
			}

			// Token: 0x06000655 RID: 1621 RVA: 0x00016BF4 File Offset: 0x00014DF4
			public override bool MoveNext()
			{
				int state = this._state;
				if (state != 1)
				{
					if (state != 2)
					{
						return false;
					}
				}
				else
				{
					this._enumerator = this._source.GetEnumerator();
					this._state = 2;
				}
				if (this._enumerator.MoveNext())
				{
					this._current = this._selector(this._enumerator.Current);
					return true;
				}
				this.Dispose();
				return false;
			}

			// Token: 0x06000656 RID: 1622 RVA: 0x00016C5C File Offset: 0x00014E5C
			public override void Dispose()
			{
				if (this._enumerator != null)
				{
					this._enumerator.Dispose();
					this._enumerator = null;
				}
				base.Dispose();
			}

			// Token: 0x06000657 RID: 1623 RVA: 0x00016C7E File Offset: 0x00014E7E
			public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
			{
				return new Enumerable.SelectIListIterator<TSource, TResult2>(this._source, Utilities.CombineSelectors<TSource, TResult, TResult2>(this._selector, selector));
			}

			// Token: 0x06000658 RID: 1624 RVA: 0x00016C98 File Offset: 0x00014E98
			public TResult[] ToArray()
			{
				int count = this._source.Count;
				if (count == 0)
				{
					return Array.Empty<TResult>();
				}
				TResult[] array = new TResult[count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._selector(this._source[i]);
				}
				return array;
			}

			// Token: 0x06000659 RID: 1625 RVA: 0x00016CF0 File Offset: 0x00014EF0
			public List<TResult> ToList()
			{
				int count = this._source.Count;
				List<TResult> list = new List<TResult>(count);
				for (int i = 0; i < count; i++)
				{
					list.Add(this._selector(this._source[i]));
				}
				return list;
			}

			// Token: 0x0600065A RID: 1626 RVA: 0x00016D3C File Offset: 0x00014F3C
			public int GetCount(bool onlyIfCheap)
			{
				int count = this._source.Count;
				if (!onlyIfCheap)
				{
					for (int i = 0; i < count; i++)
					{
						this._selector(this._source[i]);
					}
				}
				return count;
			}

			// Token: 0x0600065B RID: 1627 RVA: 0x00016D7D File Offset: 0x00014F7D
			public IPartition<TResult> Skip(int count)
			{
				return new Enumerable.SelectListPartitionIterator<TSource, TResult>(this._source, this._selector, count, int.MaxValue);
			}

			// Token: 0x0600065C RID: 1628 RVA: 0x00016D96 File Offset: 0x00014F96
			public IPartition<TResult> Take(int count)
			{
				return new Enumerable.SelectListPartitionIterator<TSource, TResult>(this._source, this._selector, 0, count - 1);
			}

			// Token: 0x0600065D RID: 1629 RVA: 0x00016DB0 File Offset: 0x00014FB0
			public TResult TryGetElementAt(int index, out bool found)
			{
				if (index < this._source.Count)
				{
					found = true;
					return this._selector(this._source[index]);
				}
				found = false;
				return default(TResult);
			}

			// Token: 0x0600065E RID: 1630 RVA: 0x00016DF4 File Offset: 0x00014FF4
			public TResult TryGetFirst(out bool found)
			{
				if (this._source.Count != 0)
				{
					found = true;
					return this._selector(this._source[0]);
				}
				found = false;
				return default(TResult);
			}

			// Token: 0x0600065F RID: 1631 RVA: 0x00016E38 File Offset: 0x00015038
			public TResult TryGetLast(out bool found)
			{
				int count = this._source.Count;
				if (count != 0)
				{
					found = true;
					return this._selector(this._source[count - 1]);
				}
				found = false;
				return default(TResult);
			}

			// Token: 0x04000488 RID: 1160
			private readonly IList<TSource> _source;

			// Token: 0x04000489 RID: 1161
			private readonly Func<TSource, TResult> _selector;

			// Token: 0x0400048A RID: 1162
			private IEnumerator<TSource> _enumerator;
		}

		// Token: 0x020000AC RID: 172
		private sealed class SelectIPartitionIterator<TSource, TResult> : Enumerable.Iterator<TResult>, IPartition<TResult>, IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
		{
			// Token: 0x06000660 RID: 1632 RVA: 0x00016E7D File Offset: 0x0001507D
			public SelectIPartitionIterator(IPartition<TSource> source, Func<TSource, TResult> selector)
			{
				this._source = source;
				this._selector = selector;
			}

			// Token: 0x06000661 RID: 1633 RVA: 0x00016E93 File Offset: 0x00015093
			public override Enumerable.Iterator<TResult> Clone()
			{
				return new Enumerable.SelectIPartitionIterator<TSource, TResult>(this._source, this._selector);
			}

			// Token: 0x06000662 RID: 1634 RVA: 0x00016EA8 File Offset: 0x000150A8
			public override bool MoveNext()
			{
				int state = this._state;
				if (state != 1)
				{
					if (state != 2)
					{
						return false;
					}
				}
				else
				{
					this._enumerator = this._source.GetEnumerator();
					this._state = 2;
				}
				if (this._enumerator.MoveNext())
				{
					this._current = this._selector(this._enumerator.Current);
					return true;
				}
				this.Dispose();
				return false;
			}

			// Token: 0x06000663 RID: 1635 RVA: 0x00016F10 File Offset: 0x00015110
			public override void Dispose()
			{
				if (this._enumerator != null)
				{
					this._enumerator.Dispose();
					this._enumerator = null;
				}
				base.Dispose();
			}

			// Token: 0x06000664 RID: 1636 RVA: 0x00016F32 File Offset: 0x00015132
			public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
			{
				return new Enumerable.SelectIPartitionIterator<TSource, TResult2>(this._source, Utilities.CombineSelectors<TSource, TResult, TResult2>(this._selector, selector));
			}

			// Token: 0x06000665 RID: 1637 RVA: 0x00016F4B File Offset: 0x0001514B
			public IPartition<TResult> Skip(int count)
			{
				return new Enumerable.SelectIPartitionIterator<TSource, TResult>(this._source.Skip(count), this._selector);
			}

			// Token: 0x06000666 RID: 1638 RVA: 0x00016F64 File Offset: 0x00015164
			public IPartition<TResult> Take(int count)
			{
				return new Enumerable.SelectIPartitionIterator<TSource, TResult>(this._source.Take(count), this._selector);
			}

			// Token: 0x06000667 RID: 1639 RVA: 0x00016F80 File Offset: 0x00015180
			public TResult TryGetElementAt(int index, out bool found)
			{
				bool flag;
				TSource tsource = this._source.TryGetElementAt(index, out flag);
				found = flag;
				if (!flag)
				{
					return default(TResult);
				}
				return this._selector(tsource);
			}

			// Token: 0x06000668 RID: 1640 RVA: 0x00016FB8 File Offset: 0x000151B8
			public TResult TryGetFirst(out bool found)
			{
				bool flag;
				TSource tsource = this._source.TryGetFirst(out flag);
				found = flag;
				if (!flag)
				{
					return default(TResult);
				}
				return this._selector(tsource);
			}

			// Token: 0x06000669 RID: 1641 RVA: 0x00016FF0 File Offset: 0x000151F0
			public TResult TryGetLast(out bool found)
			{
				bool flag;
				TSource tsource = this._source.TryGetLast(out flag);
				found = flag;
				if (!flag)
				{
					return default(TResult);
				}
				return this._selector(tsource);
			}

			// Token: 0x0600066A RID: 1642 RVA: 0x00017028 File Offset: 0x00015228
			private TResult[] LazyToArray()
			{
				global::System.Collections.Generic.LargeArrayBuilder<TResult> largeArrayBuilder = new global::System.Collections.Generic.LargeArrayBuilder<TResult>(true);
				foreach (TSource tsource in this._source)
				{
					largeArrayBuilder.Add(this._selector(tsource));
				}
				return largeArrayBuilder.ToArray();
			}

			// Token: 0x0600066B RID: 1643 RVA: 0x00017090 File Offset: 0x00015290
			private TResult[] PreallocatingToArray(int count)
			{
				TResult[] array = new TResult[count];
				int num = 0;
				foreach (TSource tsource in this._source)
				{
					array[num] = this._selector(tsource);
					num++;
				}
				return array;
			}

			// Token: 0x0600066C RID: 1644 RVA: 0x000170F8 File Offset: 0x000152F8
			public TResult[] ToArray()
			{
				int count = this._source.GetCount(true);
				if (count == -1)
				{
					return this.LazyToArray();
				}
				if (count != 0)
				{
					return this.PreallocatingToArray(count);
				}
				return Array.Empty<TResult>();
			}

			// Token: 0x0600066D RID: 1645 RVA: 0x00017130 File Offset: 0x00015330
			public List<TResult> ToList()
			{
				int count = this._source.GetCount(true);
				List<TResult> list;
				if (count != -1)
				{
					if (count == 0)
					{
						return new List<TResult>();
					}
					list = new List<TResult>(count);
				}
				else
				{
					list = new List<TResult>();
				}
				foreach (TSource tsource in this._source)
				{
					list.Add(this._selector(tsource));
				}
				return list;
			}

			// Token: 0x0600066E RID: 1646 RVA: 0x000171B4 File Offset: 0x000153B4
			public int GetCount(bool onlyIfCheap)
			{
				if (!onlyIfCheap)
				{
					foreach (TSource tsource in this._source)
					{
						this._selector(tsource);
					}
				}
				return this._source.GetCount(onlyIfCheap);
			}

			// Token: 0x0400048B RID: 1163
			private readonly IPartition<TSource> _source;

			// Token: 0x0400048C RID: 1164
			private readonly Func<TSource, TResult> _selector;

			// Token: 0x0400048D RID: 1165
			private IEnumerator<TSource> _enumerator;
		}

		// Token: 0x020000AD RID: 173
		private sealed class SelectListPartitionIterator<TSource, TResult> : Enumerable.Iterator<TResult>, IPartition<TResult>, IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
		{
			// Token: 0x0600066F RID: 1647 RVA: 0x00017218 File Offset: 0x00015418
			public SelectListPartitionIterator(IList<TSource> source, Func<TSource, TResult> selector, int minIndexInclusive, int maxIndexInclusive)
			{
				this._source = source;
				this._selector = selector;
				this._minIndexInclusive = minIndexInclusive;
				this._maxIndexInclusive = maxIndexInclusive;
			}

			// Token: 0x06000670 RID: 1648 RVA: 0x0001723D File Offset: 0x0001543D
			public override Enumerable.Iterator<TResult> Clone()
			{
				return new Enumerable.SelectListPartitionIterator<TSource, TResult>(this._source, this._selector, this._minIndexInclusive, this._maxIndexInclusive);
			}

			// Token: 0x06000671 RID: 1649 RVA: 0x0001725C File Offset: 0x0001545C
			public override bool MoveNext()
			{
				int num = this._state - 1;
				if (num <= this._maxIndexInclusive - this._minIndexInclusive && num < this._source.Count - this._minIndexInclusive)
				{
					this._current = this._selector(this._source[this._minIndexInclusive + num]);
					this._state++;
					return true;
				}
				this.Dispose();
				return false;
			}

			// Token: 0x06000672 RID: 1650 RVA: 0x000172D2 File Offset: 0x000154D2
			public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
			{
				return new Enumerable.SelectListPartitionIterator<TSource, TResult2>(this._source, Utilities.CombineSelectors<TSource, TResult, TResult2>(this._selector, selector), this._minIndexInclusive, this._maxIndexInclusive);
			}

			// Token: 0x06000673 RID: 1651 RVA: 0x000172F8 File Offset: 0x000154F8
			public IPartition<TResult> Skip(int count)
			{
				int num = this._minIndexInclusive + count;
				if (num <= this._maxIndexInclusive)
				{
					return new Enumerable.SelectListPartitionIterator<TSource, TResult>(this._source, this._selector, num, this._maxIndexInclusive);
				}
				return EmptyPartition<TResult>.Instance;
			}

			// Token: 0x06000674 RID: 1652 RVA: 0x00017338 File Offset: 0x00015538
			public IPartition<TResult> Take(int count)
			{
				int num = this._minIndexInclusive + count - 1;
				if (num < this._maxIndexInclusive)
				{
					return new Enumerable.SelectListPartitionIterator<TSource, TResult>(this._source, this._selector, this._minIndexInclusive, num);
				}
				return this;
			}

			// Token: 0x06000675 RID: 1653 RVA: 0x00017374 File Offset: 0x00015574
			public TResult TryGetElementAt(int index, out bool found)
			{
				if (index <= this._maxIndexInclusive - this._minIndexInclusive && index < this._source.Count - this._minIndexInclusive)
				{
					found = true;
					return this._selector(this._source[this._minIndexInclusive + index]);
				}
				found = false;
				return default(TResult);
			}

			// Token: 0x06000676 RID: 1654 RVA: 0x000173D4 File Offset: 0x000155D4
			public TResult TryGetFirst(out bool found)
			{
				if (this._source.Count > this._minIndexInclusive)
				{
					found = true;
					return this._selector(this._source[this._minIndexInclusive]);
				}
				found = false;
				return default(TResult);
			}

			// Token: 0x06000677 RID: 1655 RVA: 0x00017420 File Offset: 0x00015620
			public TResult TryGetLast(out bool found)
			{
				int num = this._source.Count - 1;
				if (num >= this._minIndexInclusive)
				{
					found = true;
					return this._selector(this._source[Math.Min(num, this._maxIndexInclusive)]);
				}
				found = false;
				return default(TResult);
			}

			// Token: 0x170000AD RID: 173
			// (get) Token: 0x06000678 RID: 1656 RVA: 0x00017478 File Offset: 0x00015678
			private int Count
			{
				get
				{
					int count = this._source.Count;
					if (count <= this._minIndexInclusive)
					{
						return 0;
					}
					return Math.Min(count - 1, this._maxIndexInclusive) - this._minIndexInclusive + 1;
				}
			}

			// Token: 0x06000679 RID: 1657 RVA: 0x000174B4 File Offset: 0x000156B4
			public TResult[] ToArray()
			{
				int count = this.Count;
				if (count == 0)
				{
					return Array.Empty<TResult>();
				}
				TResult[] array = new TResult[count];
				int num = 0;
				int num2 = this._minIndexInclusive;
				while (num != array.Length)
				{
					array[num] = this._selector(this._source[num2]);
					num++;
					num2++;
				}
				return array;
			}

			// Token: 0x0600067A RID: 1658 RVA: 0x00017510 File Offset: 0x00015710
			public List<TResult> ToList()
			{
				int count = this.Count;
				if (count == 0)
				{
					return new List<TResult>();
				}
				List<TResult> list = new List<TResult>(count);
				int num = this._minIndexInclusive + count;
				for (int num2 = this._minIndexInclusive; num2 != num; num2++)
				{
					list.Add(this._selector(this._source[num2]));
				}
				return list;
			}

			// Token: 0x0600067B RID: 1659 RVA: 0x0001756C File Offset: 0x0001576C
			public int GetCount(bool onlyIfCheap)
			{
				int count = this.Count;
				if (!onlyIfCheap)
				{
					int num = this._minIndexInclusive + count;
					for (int num2 = this._minIndexInclusive; num2 != num; num2++)
					{
						this._selector(this._source[num2]);
					}
				}
				return count;
			}

			// Token: 0x0400048E RID: 1166
			private readonly IList<TSource> _source;

			// Token: 0x0400048F RID: 1167
			private readonly Func<TSource, TResult> _selector;

			// Token: 0x04000490 RID: 1168
			private readonly int _minIndexInclusive;

			// Token: 0x04000491 RID: 1169
			private readonly int _maxIndexInclusive;
		}

		// Token: 0x020000AE RID: 174
		private sealed class SelectManySingleSelectorIterator<TSource, TResult> : Enumerable.Iterator<TResult>, IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
		{
			// Token: 0x0600067C RID: 1660 RVA: 0x000175B6 File Offset: 0x000157B6
			internal SelectManySingleSelectorIterator(IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
			{
				this._source = source;
				this._selector = selector;
			}

			// Token: 0x0600067D RID: 1661 RVA: 0x000175CC File Offset: 0x000157CC
			public override Enumerable.Iterator<TResult> Clone()
			{
				return new Enumerable.SelectManySingleSelectorIterator<TSource, TResult>(this._source, this._selector);
			}

			// Token: 0x0600067E RID: 1662 RVA: 0x000175DF File Offset: 0x000157DF
			public override void Dispose()
			{
				if (this._subEnumerator != null)
				{
					this._subEnumerator.Dispose();
					this._subEnumerator = null;
				}
				if (this._sourceEnumerator != null)
				{
					this._sourceEnumerator.Dispose();
					this._sourceEnumerator = null;
				}
				base.Dispose();
			}

			// Token: 0x0600067F RID: 1663 RVA: 0x0001761C File Offset: 0x0001581C
			public int GetCount(bool onlyIfCheap)
			{
				if (onlyIfCheap)
				{
					return -1;
				}
				int num = 0;
				checked
				{
					foreach (TSource tsource in this._source)
					{
						num += this._selector(tsource).Count<TResult>();
					}
					return num;
				}
			}

			// Token: 0x06000680 RID: 1664 RVA: 0x00017680 File Offset: 0x00015880
			public override bool MoveNext()
			{
				switch (this._state)
				{
				case 1:
					this._sourceEnumerator = this._source.GetEnumerator();
					this._state = 2;
					break;
				case 2:
					break;
				case 3:
					goto IL_006F;
				default:
					goto IL_00AA;
				}
				IL_0038:
				if (!this._sourceEnumerator.MoveNext())
				{
					goto IL_00AA;
				}
				TSource tsource = this._sourceEnumerator.Current;
				this._subEnumerator = this._selector(tsource).GetEnumerator();
				this._state = 3;
				IL_006F:
				if (!this._subEnumerator.MoveNext())
				{
					this._subEnumerator.Dispose();
					this._subEnumerator = null;
					this._state = 2;
					goto IL_0038;
				}
				this._current = this._subEnumerator.Current;
				return true;
				IL_00AA:
				this.Dispose();
				return false;
			}

			// Token: 0x06000681 RID: 1665 RVA: 0x00017740 File Offset: 0x00015940
			public TResult[] ToArray()
			{
				SparseArrayBuilder<TResult> sparseArrayBuilder = new SparseArrayBuilder<TResult>(true);
				global::System.Collections.Generic.ArrayBuilder<IEnumerable<TResult>> arrayBuilder = default(global::System.Collections.Generic.ArrayBuilder<IEnumerable<TResult>>);
				foreach (TSource tsource in this._source)
				{
					IEnumerable<TResult> enumerable = this._selector(tsource);
					if (sparseArrayBuilder.ReserveOrAdd(enumerable))
					{
						arrayBuilder.Add(enumerable);
					}
				}
				TResult[] array = sparseArrayBuilder.ToArray();
				global::System.Collections.Generic.ArrayBuilder<Marker> markers = sparseArrayBuilder.Markers;
				for (int i = 0; i < markers.Count; i++)
				{
					Marker marker = markers[i];
					global::System.Collections.Generic.EnumerableHelpers.Copy<TResult>(arrayBuilder[i], array, marker.Index, marker.Count);
				}
				return array;
			}

			// Token: 0x06000682 RID: 1666 RVA: 0x00017810 File Offset: 0x00015A10
			public List<TResult> ToList()
			{
				List<TResult> list = new List<TResult>();
				foreach (TSource tsource in this._source)
				{
					list.AddRange(this._selector(tsource));
				}
				return list;
			}

			// Token: 0x04000492 RID: 1170
			private readonly IEnumerable<TSource> _source;

			// Token: 0x04000493 RID: 1171
			private readonly Func<TSource, IEnumerable<TResult>> _selector;

			// Token: 0x04000494 RID: 1172
			private IEnumerator<TSource> _sourceEnumerator;

			// Token: 0x04000495 RID: 1173
			private IEnumerator<TResult> _subEnumerator;
		}

		// Token: 0x020000AF RID: 175
		private abstract class UnionIterator<TSource> : Enumerable.Iterator<TSource>, IIListProvider<TSource>, IEnumerable<TSource>, IEnumerable
		{
			// Token: 0x06000683 RID: 1667 RVA: 0x00017870 File Offset: 0x00015A70
			protected UnionIterator(IEqualityComparer<TSource> comparer)
			{
				this._comparer = comparer;
			}

			// Token: 0x06000684 RID: 1668 RVA: 0x0001787F File Offset: 0x00015A7F
			public sealed override void Dispose()
			{
				if (this._enumerator != null)
				{
					this._enumerator.Dispose();
					this._enumerator = null;
					this._set = null;
				}
				base.Dispose();
			}

			// Token: 0x06000685 RID: 1669
			internal abstract IEnumerable<TSource> GetEnumerable(int index);

			// Token: 0x06000686 RID: 1670
			internal abstract Enumerable.UnionIterator<TSource> Union(IEnumerable<TSource> next);

			// Token: 0x06000687 RID: 1671 RVA: 0x000178A8 File Offset: 0x00015AA8
			private void SetEnumerator(IEnumerator<TSource> enumerator)
			{
				IEnumerator<TSource> enumerator2 = this._enumerator;
				if (enumerator2 != null)
				{
					enumerator2.Dispose();
				}
				this._enumerator = enumerator;
			}

			// Token: 0x06000688 RID: 1672 RVA: 0x000178C4 File Offset: 0x00015AC4
			private void StoreFirst()
			{
				Set<TSource> set = new Set<TSource>(this._comparer);
				TSource tsource = this._enumerator.Current;
				set.Add(tsource);
				this._current = tsource;
				this._set = set;
			}

			// Token: 0x06000689 RID: 1673 RVA: 0x00017900 File Offset: 0x00015B00
			private bool GetNext()
			{
				Set<TSource> set = this._set;
				while (this._enumerator.MoveNext())
				{
					TSource tsource = this._enumerator.Current;
					if (set.Add(tsource))
					{
						this._current = tsource;
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600068A RID: 1674 RVA: 0x00017944 File Offset: 0x00015B44
			public sealed override bool MoveNext()
			{
				if (this._state == 1)
				{
					for (IEnumerable<TSource> enumerable = this.GetEnumerable(0); enumerable != null; enumerable = this.GetEnumerable(this._state - 1))
					{
						IEnumerator<TSource> enumerator = enumerable.GetEnumerator();
						this._state++;
						if (enumerator.MoveNext())
						{
							this.SetEnumerator(enumerator);
							this.StoreFirst();
							return true;
						}
					}
				}
				else if (this._state > 0)
				{
					while (!this.GetNext())
					{
						IEnumerable<TSource> enumerable2 = this.GetEnumerable(this._state - 1);
						if (enumerable2 == null)
						{
							goto IL_0094;
						}
						this.SetEnumerator(enumerable2.GetEnumerator());
						this._state++;
					}
					return true;
				}
				IL_0094:
				this.Dispose();
				return false;
			}

			// Token: 0x0600068B RID: 1675 RVA: 0x000179EC File Offset: 0x00015BEC
			private Set<TSource> FillSet()
			{
				Set<TSource> set = new Set<TSource>(this._comparer);
				int num = 0;
				for (;;)
				{
					IEnumerable<TSource> enumerable = this.GetEnumerable(num);
					if (enumerable == null)
					{
						break;
					}
					set.UnionWith(enumerable);
					num++;
				}
				return set;
			}

			// Token: 0x0600068C RID: 1676 RVA: 0x00017A20 File Offset: 0x00015C20
			public TSource[] ToArray()
			{
				return this.FillSet().ToArray();
			}

			// Token: 0x0600068D RID: 1677 RVA: 0x00017A2D File Offset: 0x00015C2D
			public List<TSource> ToList()
			{
				return this.FillSet().ToList();
			}

			// Token: 0x0600068E RID: 1678 RVA: 0x00017A3A File Offset: 0x00015C3A
			public int GetCount(bool onlyIfCheap)
			{
				if (!onlyIfCheap)
				{
					return this.FillSet().Count;
				}
				return -1;
			}

			// Token: 0x04000496 RID: 1174
			internal readonly IEqualityComparer<TSource> _comparer;

			// Token: 0x04000497 RID: 1175
			private IEnumerator<TSource> _enumerator;

			// Token: 0x04000498 RID: 1176
			private Set<TSource> _set;
		}

		// Token: 0x020000B0 RID: 176
		private sealed class UnionIterator2<TSource> : Enumerable.UnionIterator<TSource>
		{
			// Token: 0x0600068F RID: 1679 RVA: 0x00017A4C File Offset: 0x00015C4C
			public UnionIterator2(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
				: base(comparer)
			{
				this._first = first;
				this._second = second;
			}

			// Token: 0x06000690 RID: 1680 RVA: 0x00017A63 File Offset: 0x00015C63
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.UnionIterator2<TSource>(this._first, this._second, this._comparer);
			}

			// Token: 0x06000691 RID: 1681 RVA: 0x00017A7C File Offset: 0x00015C7C
			internal override IEnumerable<TSource> GetEnumerable(int index)
			{
				if (index == 0)
				{
					return this._first;
				}
				if (index != 1)
				{
					return null;
				}
				return this._second;
			}

			// Token: 0x06000692 RID: 1682 RVA: 0x00017A96 File Offset: 0x00015C96
			internal override Enumerable.UnionIterator<TSource> Union(IEnumerable<TSource> next)
			{
				return new Enumerable.UnionIteratorN<TSource>(new SingleLinkedNode<IEnumerable<TSource>>(this._first).Add(this._second).Add(next), 2, this._comparer);
			}

			// Token: 0x04000499 RID: 1177
			private readonly IEnumerable<TSource> _first;

			// Token: 0x0400049A RID: 1178
			private readonly IEnumerable<TSource> _second;
		}

		// Token: 0x020000B1 RID: 177
		private sealed class UnionIteratorN<TSource> : Enumerable.UnionIterator<TSource>
		{
			// Token: 0x06000693 RID: 1683 RVA: 0x00017AC0 File Offset: 0x00015CC0
			public UnionIteratorN(SingleLinkedNode<IEnumerable<TSource>> sources, int headIndex, IEqualityComparer<TSource> comparer)
				: base(comparer)
			{
				this._sources = sources;
				this._headIndex = headIndex;
			}

			// Token: 0x06000694 RID: 1684 RVA: 0x00017AD7 File Offset: 0x00015CD7
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.UnionIteratorN<TSource>(this._sources, this._headIndex, this._comparer);
			}

			// Token: 0x06000695 RID: 1685 RVA: 0x00017AF0 File Offset: 0x00015CF0
			internal override IEnumerable<TSource> GetEnumerable(int index)
			{
				if (index <= this._headIndex)
				{
					return this._sources.GetNode(this._headIndex - index).Item;
				}
				return null;
			}

			// Token: 0x06000696 RID: 1686 RVA: 0x00017B15 File Offset: 0x00015D15
			internal override Enumerable.UnionIterator<TSource> Union(IEnumerable<TSource> next)
			{
				if (this._headIndex == 2147483645)
				{
					return new Enumerable.UnionIterator2<TSource>(this, next, this._comparer);
				}
				return new Enumerable.UnionIteratorN<TSource>(this._sources.Add(next), this._headIndex + 1, this._comparer);
			}

			// Token: 0x0400049B RID: 1179
			private readonly SingleLinkedNode<IEnumerable<TSource>> _sources;

			// Token: 0x0400049C RID: 1180
			private readonly int _headIndex;
		}

		// Token: 0x020000B2 RID: 178
		private sealed class WhereEnumerableIterator<TSource> : Enumerable.Iterator<TSource>, IIListProvider<TSource>, IEnumerable<TSource>, IEnumerable
		{
			// Token: 0x06000697 RID: 1687 RVA: 0x00017B51 File Offset: 0x00015D51
			public WhereEnumerableIterator(IEnumerable<TSource> source, Func<TSource, bool> predicate)
			{
				this._source = source;
				this._predicate = predicate;
			}

			// Token: 0x06000698 RID: 1688 RVA: 0x00017B67 File Offset: 0x00015D67
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.WhereEnumerableIterator<TSource>(this._source, this._predicate);
			}

			// Token: 0x06000699 RID: 1689 RVA: 0x00017B7A File Offset: 0x00015D7A
			public override void Dispose()
			{
				if (this._enumerator != null)
				{
					this._enumerator.Dispose();
					this._enumerator = null;
				}
				base.Dispose();
			}

			// Token: 0x0600069A RID: 1690 RVA: 0x00017B9C File Offset: 0x00015D9C
			public int GetCount(bool onlyIfCheap)
			{
				if (onlyIfCheap)
				{
					return -1;
				}
				int num = 0;
				checked
				{
					foreach (TSource tsource in this._source)
					{
						if (this._predicate(tsource))
						{
							num++;
						}
					}
					return num;
				}
			}

			// Token: 0x0600069B RID: 1691 RVA: 0x00017BFC File Offset: 0x00015DFC
			public override bool MoveNext()
			{
				int state = this._state;
				if (state != 1)
				{
					if (state != 2)
					{
						return false;
					}
				}
				else
				{
					this._enumerator = this._source.GetEnumerator();
					this._state = 2;
				}
				while (this._enumerator.MoveNext())
				{
					TSource tsource = this._enumerator.Current;
					if (this._predicate(tsource))
					{
						this._current = tsource;
						return true;
					}
				}
				this.Dispose();
				return false;
			}

			// Token: 0x0600069C RID: 1692 RVA: 0x00017C6B File Offset: 0x00015E6B
			public override IEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
			{
				return new Enumerable.WhereSelectEnumerableIterator<TSource, TResult>(this._source, this._predicate, selector);
			}

			// Token: 0x0600069D RID: 1693 RVA: 0x00017C80 File Offset: 0x00015E80
			public TSource[] ToArray()
			{
				global::System.Collections.Generic.LargeArrayBuilder<TSource> largeArrayBuilder = new global::System.Collections.Generic.LargeArrayBuilder<TSource>(true);
				foreach (TSource tsource in this._source)
				{
					if (this._predicate(tsource))
					{
						largeArrayBuilder.Add(tsource);
					}
				}
				return largeArrayBuilder.ToArray();
			}

			// Token: 0x0600069E RID: 1694 RVA: 0x00017CEC File Offset: 0x00015EEC
			public List<TSource> ToList()
			{
				List<TSource> list = new List<TSource>();
				foreach (TSource tsource in this._source)
				{
					if (this._predicate(tsource))
					{
						list.Add(tsource);
					}
				}
				return list;
			}

			// Token: 0x0600069F RID: 1695 RVA: 0x00017D50 File Offset: 0x00015F50
			public override IEnumerable<TSource> Where(Func<TSource, bool> predicate)
			{
				return new Enumerable.WhereEnumerableIterator<TSource>(this._source, Utilities.CombinePredicates<TSource>(this._predicate, predicate));
			}

			// Token: 0x0400049D RID: 1181
			private readonly IEnumerable<TSource> _source;

			// Token: 0x0400049E RID: 1182
			private readonly Func<TSource, bool> _predicate;

			// Token: 0x0400049F RID: 1183
			private IEnumerator<TSource> _enumerator;
		}

		// Token: 0x020000B3 RID: 179
		internal sealed class WhereArrayIterator<TSource> : Enumerable.Iterator<TSource>, IIListProvider<TSource>, IEnumerable<TSource>, IEnumerable
		{
			// Token: 0x060006A0 RID: 1696 RVA: 0x00017D69 File Offset: 0x00015F69
			public WhereArrayIterator(TSource[] source, Func<TSource, bool> predicate)
			{
				this._source = source;
				this._predicate = predicate;
			}

			// Token: 0x060006A1 RID: 1697 RVA: 0x00017D7F File Offset: 0x00015F7F
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.WhereArrayIterator<TSource>(this._source, this._predicate);
			}

			// Token: 0x060006A2 RID: 1698 RVA: 0x00017D94 File Offset: 0x00015F94
			public int GetCount(bool onlyIfCheap)
			{
				if (onlyIfCheap)
				{
					return -1;
				}
				int num = 0;
				checked
				{
					foreach (TSource tsource in this._source)
					{
						if (this._predicate(tsource))
						{
							num++;
						}
					}
					return num;
				}
			}

			// Token: 0x060006A3 RID: 1699 RVA: 0x00017DD8 File Offset: 0x00015FD8
			public override bool MoveNext()
			{
				int i = this._state - 1;
				TSource[] source = this._source;
				while (i < source.Length)
				{
					TSource tsource = source[i];
					int state = this._state;
					this._state = state + 1;
					i = state;
					if (this._predicate(tsource))
					{
						this._current = tsource;
						return true;
					}
				}
				this.Dispose();
				return false;
			}

			// Token: 0x060006A4 RID: 1700 RVA: 0x00017E35 File Offset: 0x00016035
			public override IEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
			{
				return new Enumerable.WhereSelectArrayIterator<TSource, TResult>(this._source, this._predicate, selector);
			}

			// Token: 0x060006A5 RID: 1701 RVA: 0x00017E4C File Offset: 0x0001604C
			public TSource[] ToArray()
			{
				global::System.Collections.Generic.LargeArrayBuilder<TSource> largeArrayBuilder = new global::System.Collections.Generic.LargeArrayBuilder<TSource>(this._source.Length);
				foreach (TSource tsource in this._source)
				{
					if (this._predicate(tsource))
					{
						largeArrayBuilder.Add(tsource);
					}
				}
				return largeArrayBuilder.ToArray();
			}

			// Token: 0x060006A6 RID: 1702 RVA: 0x00017EA4 File Offset: 0x000160A4
			public List<TSource> ToList()
			{
				List<TSource> list = new List<TSource>();
				foreach (TSource tsource in this._source)
				{
					if (this._predicate(tsource))
					{
						list.Add(tsource);
					}
				}
				return list;
			}

			// Token: 0x060006A7 RID: 1703 RVA: 0x00017EEA File Offset: 0x000160EA
			public override IEnumerable<TSource> Where(Func<TSource, bool> predicate)
			{
				return new Enumerable.WhereArrayIterator<TSource>(this._source, Utilities.CombinePredicates<TSource>(this._predicate, predicate));
			}

			// Token: 0x040004A0 RID: 1184
			private readonly TSource[] _source;

			// Token: 0x040004A1 RID: 1185
			private readonly Func<TSource, bool> _predicate;
		}

		// Token: 0x020000B4 RID: 180
		private sealed class WhereListIterator<TSource> : Enumerable.Iterator<TSource>, IIListProvider<TSource>, IEnumerable<TSource>, IEnumerable
		{
			// Token: 0x060006A8 RID: 1704 RVA: 0x00017F03 File Offset: 0x00016103
			public WhereListIterator(List<TSource> source, Func<TSource, bool> predicate)
			{
				this._source = source;
				this._predicate = predicate;
			}

			// Token: 0x060006A9 RID: 1705 RVA: 0x00017F19 File Offset: 0x00016119
			public override Enumerable.Iterator<TSource> Clone()
			{
				return new Enumerable.WhereListIterator<TSource>(this._source, this._predicate);
			}

			// Token: 0x060006AA RID: 1706 RVA: 0x00017F2C File Offset: 0x0001612C
			public int GetCount(bool onlyIfCheap)
			{
				if (onlyIfCheap)
				{
					return -1;
				}
				int num = 0;
				for (int i = 0; i < this._source.Count; i++)
				{
					TSource tsource = this._source[i];
					checked
					{
						if (this._predicate(tsource))
						{
							num++;
						}
					}
				}
				return num;
			}

			// Token: 0x060006AB RID: 1707 RVA: 0x00017F78 File Offset: 0x00016178
			public override bool MoveNext()
			{
				int state = this._state;
				if (state != 1)
				{
					if (state != 2)
					{
						return false;
					}
				}
				else
				{
					this._enumerator = this._source.GetEnumerator();
					this._state = 2;
				}
				while (this._enumerator.MoveNext())
				{
					TSource tsource = this._enumerator.Current;
					if (this._predicate(tsource))
					{
						this._current = tsource;
						return true;
					}
				}
				this.Dispose();
				return false;
			}

			// Token: 0x060006AC RID: 1708 RVA: 0x00017FE7 File Offset: 0x000161E7
			public override IEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
			{
				return new Enumerable.WhereSelectListIterator<TSource, TResult>(this._source, this._predicate, selector);
			}

			// Token: 0x060006AD RID: 1709 RVA: 0x00017FFC File Offset: 0x000161FC
			public TSource[] ToArray()
			{
				global::System.Collections.Generic.LargeArrayBuilder<TSource> largeArrayBuilder = new global::System.Collections.Generic.LargeArrayBuilder<TSource>(this._source.Count);
				for (int i = 0; i < this._source.Count; i++)
				{
					TSource tsource = this._source[i];
					if (this._predicate(tsource))
					{
						largeArrayBuilder.Add(tsource);
					}
				}
				return largeArrayBuilder.ToArray();
			}

			// Token: 0x060006AE RID: 1710 RVA: 0x0001805C File Offset: 0x0001625C
			public List<TSource> ToList()
			{
				List<TSource> list = new List<TSource>();
				for (int i = 0; i < this._source.Count; i++)
				{
					TSource tsource = this._source[i];
					if (this._predicate(tsource))
					{
						list.Add(tsource);
					}
				}
				return list;
			}

			// Token: 0x060006AF RID: 1711 RVA: 0x000180A8 File Offset: 0x000162A8
			public override IEnumerable<TSource> Where(Func<TSource, bool> predicate)
			{
				return new Enumerable.WhereListIterator<TSource>(this._source, Utilities.CombinePredicates<TSource>(this._predicate, predicate));
			}

			// Token: 0x040004A2 RID: 1186
			private readonly List<TSource> _source;

			// Token: 0x040004A3 RID: 1187
			private readonly Func<TSource, bool> _predicate;

			// Token: 0x040004A4 RID: 1188
			private List<TSource>.Enumerator _enumerator;
		}

		// Token: 0x020000B5 RID: 181
		private sealed class WhereSelectArrayIterator<TSource, TResult> : Enumerable.Iterator<TResult>, IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
		{
			// Token: 0x060006B0 RID: 1712 RVA: 0x000180C1 File Offset: 0x000162C1
			public WhereSelectArrayIterator(TSource[] source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
			{
				this._source = source;
				this._predicate = predicate;
				this._selector = selector;
			}

			// Token: 0x060006B1 RID: 1713 RVA: 0x000180DE File Offset: 0x000162DE
			public override Enumerable.Iterator<TResult> Clone()
			{
				return new Enumerable.WhereSelectArrayIterator<TSource, TResult>(this._source, this._predicate, this._selector);
			}

			// Token: 0x060006B2 RID: 1714 RVA: 0x000180F8 File Offset: 0x000162F8
			public int GetCount(bool onlyIfCheap)
			{
				if (onlyIfCheap)
				{
					return -1;
				}
				int num = 0;
				checked
				{
					foreach (TSource tsource in this._source)
					{
						if (this._predicate(tsource))
						{
							this._selector(tsource);
							num++;
						}
					}
					return num;
				}
			}

			// Token: 0x060006B3 RID: 1715 RVA: 0x0001814C File Offset: 0x0001634C
			public override bool MoveNext()
			{
				int i = this._state - 1;
				TSource[] source = this._source;
				while (i < source.Length)
				{
					TSource tsource = source[i];
					int state = this._state;
					this._state = state + 1;
					i = state;
					if (this._predicate(tsource))
					{
						this._current = this._selector(tsource);
						return true;
					}
				}
				this.Dispose();
				return false;
			}

			// Token: 0x060006B4 RID: 1716 RVA: 0x000181B4 File Offset: 0x000163B4
			public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
			{
				return new Enumerable.WhereSelectArrayIterator<TSource, TResult2>(this._source, this._predicate, Utilities.CombineSelectors<TSource, TResult, TResult2>(this._selector, selector));
			}

			// Token: 0x060006B5 RID: 1717 RVA: 0x000181D4 File Offset: 0x000163D4
			public TResult[] ToArray()
			{
				global::System.Collections.Generic.LargeArrayBuilder<TResult> largeArrayBuilder = new global::System.Collections.Generic.LargeArrayBuilder<TResult>(this._source.Length);
				foreach (TSource tsource in this._source)
				{
					if (this._predicate(tsource))
					{
						largeArrayBuilder.Add(this._selector(tsource));
					}
				}
				return largeArrayBuilder.ToArray();
			}

			// Token: 0x060006B6 RID: 1718 RVA: 0x00018238 File Offset: 0x00016438
			public List<TResult> ToList()
			{
				List<TResult> list = new List<TResult>();
				foreach (TSource tsource in this._source)
				{
					if (this._predicate(tsource))
					{
						list.Add(this._selector(tsource));
					}
				}
				return list;
			}

			// Token: 0x040004A5 RID: 1189
			private readonly TSource[] _source;

			// Token: 0x040004A6 RID: 1190
			private readonly Func<TSource, bool> _predicate;

			// Token: 0x040004A7 RID: 1191
			private readonly Func<TSource, TResult> _selector;
		}

		// Token: 0x020000B6 RID: 182
		private sealed class WhereSelectListIterator<TSource, TResult> : Enumerable.Iterator<TResult>, IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
		{
			// Token: 0x060006B7 RID: 1719 RVA: 0x00018289 File Offset: 0x00016489
			public WhereSelectListIterator(List<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
			{
				this._source = source;
				this._predicate = predicate;
				this._selector = selector;
			}

			// Token: 0x060006B8 RID: 1720 RVA: 0x000182A6 File Offset: 0x000164A6
			public override Enumerable.Iterator<TResult> Clone()
			{
				return new Enumerable.WhereSelectListIterator<TSource, TResult>(this._source, this._predicate, this._selector);
			}

			// Token: 0x060006B9 RID: 1721 RVA: 0x000182C0 File Offset: 0x000164C0
			public int GetCount(bool onlyIfCheap)
			{
				if (onlyIfCheap)
				{
					return -1;
				}
				int num = 0;
				for (int i = 0; i < this._source.Count; i++)
				{
					TSource tsource = this._source[i];
					checked
					{
						if (this._predicate(tsource))
						{
							this._selector(tsource);
							num++;
						}
					}
				}
				return num;
			}

			// Token: 0x060006BA RID: 1722 RVA: 0x00018318 File Offset: 0x00016518
			public override bool MoveNext()
			{
				int state = this._state;
				if (state != 1)
				{
					if (state != 2)
					{
						return false;
					}
				}
				else
				{
					this._enumerator = this._source.GetEnumerator();
					this._state = 2;
				}
				while (this._enumerator.MoveNext())
				{
					TSource tsource = this._enumerator.Current;
					if (this._predicate(tsource))
					{
						this._current = this._selector(tsource);
						return true;
					}
				}
				this.Dispose();
				return false;
			}

			// Token: 0x060006BB RID: 1723 RVA: 0x00018392 File Offset: 0x00016592
			public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
			{
				return new Enumerable.WhereSelectListIterator<TSource, TResult2>(this._source, this._predicate, Utilities.CombineSelectors<TSource, TResult, TResult2>(this._selector, selector));
			}

			// Token: 0x060006BC RID: 1724 RVA: 0x000183B4 File Offset: 0x000165B4
			public TResult[] ToArray()
			{
				global::System.Collections.Generic.LargeArrayBuilder<TResult> largeArrayBuilder = new global::System.Collections.Generic.LargeArrayBuilder<TResult>(this._source.Count);
				for (int i = 0; i < this._source.Count; i++)
				{
					TSource tsource = this._source[i];
					if (this._predicate(tsource))
					{
						largeArrayBuilder.Add(this._selector(tsource));
					}
				}
				return largeArrayBuilder.ToArray();
			}

			// Token: 0x060006BD RID: 1725 RVA: 0x00018420 File Offset: 0x00016620
			public List<TResult> ToList()
			{
				List<TResult> list = new List<TResult>();
				for (int i = 0; i < this._source.Count; i++)
				{
					TSource tsource = this._source[i];
					if (this._predicate(tsource))
					{
						list.Add(this._selector(tsource));
					}
				}
				return list;
			}

			// Token: 0x040004A8 RID: 1192
			private readonly List<TSource> _source;

			// Token: 0x040004A9 RID: 1193
			private readonly Func<TSource, bool> _predicate;

			// Token: 0x040004AA RID: 1194
			private readonly Func<TSource, TResult> _selector;

			// Token: 0x040004AB RID: 1195
			private List<TSource>.Enumerator _enumerator;
		}

		// Token: 0x020000B7 RID: 183
		private sealed class WhereSelectEnumerableIterator<TSource, TResult> : Enumerable.Iterator<TResult>, IIListProvider<TResult>, IEnumerable<TResult>, IEnumerable
		{
			// Token: 0x060006BE RID: 1726 RVA: 0x00018477 File Offset: 0x00016677
			public WhereSelectEnumerableIterator(IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
			{
				this._source = source;
				this._predicate = predicate;
				this._selector = selector;
			}

			// Token: 0x060006BF RID: 1727 RVA: 0x00018494 File Offset: 0x00016694
			public override Enumerable.Iterator<TResult> Clone()
			{
				return new Enumerable.WhereSelectEnumerableIterator<TSource, TResult>(this._source, this._predicate, this._selector);
			}

			// Token: 0x060006C0 RID: 1728 RVA: 0x000184AD File Offset: 0x000166AD
			public override void Dispose()
			{
				if (this._enumerator != null)
				{
					this._enumerator.Dispose();
					this._enumerator = null;
				}
				base.Dispose();
			}

			// Token: 0x060006C1 RID: 1729 RVA: 0x000184D0 File Offset: 0x000166D0
			public int GetCount(bool onlyIfCheap)
			{
				if (onlyIfCheap)
				{
					return -1;
				}
				int num = 0;
				checked
				{
					foreach (TSource tsource in this._source)
					{
						if (this._predicate(tsource))
						{
							this._selector(tsource);
							num++;
						}
					}
					return num;
				}
			}

			// Token: 0x060006C2 RID: 1730 RVA: 0x00018540 File Offset: 0x00016740
			public override bool MoveNext()
			{
				int state = this._state;
				if (state != 1)
				{
					if (state != 2)
					{
						return false;
					}
				}
				else
				{
					this._enumerator = this._source.GetEnumerator();
					this._state = 2;
				}
				while (this._enumerator.MoveNext())
				{
					TSource tsource = this._enumerator.Current;
					if (this._predicate(tsource))
					{
						this._current = this._selector(tsource);
						return true;
					}
				}
				this.Dispose();
				return false;
			}

			// Token: 0x060006C3 RID: 1731 RVA: 0x000185BA File Offset: 0x000167BA
			public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
			{
				return new Enumerable.WhereSelectEnumerableIterator<TSource, TResult2>(this._source, this._predicate, Utilities.CombineSelectors<TSource, TResult, TResult2>(this._selector, selector));
			}

			// Token: 0x060006C4 RID: 1732 RVA: 0x000185DC File Offset: 0x000167DC
			public TResult[] ToArray()
			{
				global::System.Collections.Generic.LargeArrayBuilder<TResult> largeArrayBuilder = new global::System.Collections.Generic.LargeArrayBuilder<TResult>(true);
				foreach (TSource tsource in this._source)
				{
					if (this._predicate(tsource))
					{
						largeArrayBuilder.Add(this._selector(tsource));
					}
				}
				return largeArrayBuilder.ToArray();
			}

			// Token: 0x060006C5 RID: 1733 RVA: 0x00018654 File Offset: 0x00016854
			public List<TResult> ToList()
			{
				List<TResult> list = new List<TResult>();
				foreach (TSource tsource in this._source)
				{
					if (this._predicate(tsource))
					{
						list.Add(this._selector(tsource));
					}
				}
				return list;
			}

			// Token: 0x040004AC RID: 1196
			private readonly IEnumerable<TSource> _source;

			// Token: 0x040004AD RID: 1197
			private readonly Func<TSource, bool> _predicate;

			// Token: 0x040004AE RID: 1198
			private readonly Func<TSource, TResult> _selector;

			// Token: 0x040004AF RID: 1199
			private IEnumerator<TSource> _enumerator;
		}
	}
}
