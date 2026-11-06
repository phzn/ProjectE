using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DA_Assets.Extensions
{
	// Token: 0x0200002E RID: 46
	public static class ListExtensions
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00004497 File Offset: 0x00002697
		public static ConcurrentBag<T> ToConcurrentBag<T>(this IEnumerable<T> source)
		{
			return new ConcurrentBag<T>(source);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000044A0 File Offset: 0x000026A0
		public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
		{
			ConcurrentDictionary<TKey, TValue> concurrentDictionary = new ConcurrentDictionary<TKey, TValue>();
			foreach (TSource tsource in source)
			{
				concurrentDictionary.TryAdd(keySelector(tsource), valueSelector(tsource));
			}
			return concurrentDictionary;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004500 File Offset: 0x00002700
		public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
		{
			foreach (T parent in source)
			{
				yield return parent;
				IEnumerable<T> enumerable = selector(parent);
				foreach (T t in enumerable.SelectRecursive<T>(selector))
				{
					yield return t;
				}
				IEnumerator<T> enumerator2 = null;
				parent = default(T);
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004518 File Offset: 0x00002718
		public static bool TryAddValue<T>(this HashSet<T> hs, T value)
		{
			bool flag;
			try
			{
				hs.Add(value);
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004548 File Offset: 0x00002748
		public static bool TryAddValue<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
		{
			bool flag;
			try
			{
				dic.Add(key, value);
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004578 File Offset: 0x00002778
		public static void Shuffle<T>(this IList<T> array)
		{
			Random random = new Random();
			int i = array.Count<T>();
			while (i > 1)
			{
				int num = random.Next(i--);
				T t = array[i];
				array[i] = array[num];
				array[num] = t;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000045C2 File Offset: 0x000027C2
		public static bool ContainsAll<T>(this List<T> a, List<T> b)
		{
			return !b.Except<T>(a).Any<T>();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000045D4 File Offset: 0x000027D4
		public static IEnumerable<T> GetRangeSafe<T>(this IEnumerable<T> _list, int index, int count = 0)
		{
			if (_list.IsEmpty<T>())
			{
				return new List<T>();
			}
			if (_list.Count<T>() == 1)
			{
				return new List<T> { _list.First<T>() };
			}
			if (index < 0 || count < 0)
			{
				return new List<T>();
			}
			if (index > _list.Count<T>() - 1)
			{
				return new List<T>();
			}
			if ((index <= _list.Count<T>() - 1 && _list.Count<T>() - 1 < index + count) || count == 0)
			{
				return _list.ToList<T>().GetRange(index, _list.Count<T>() - index);
			}
			IEnumerable<T> range;
			try
			{
				range = _list.ToList<T>().GetRange(index, count);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return range;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000467C File Offset: 0x0000287C
		public static IEnumerable<T2> GetDuplicates<T1, T2>(this Dictionary<T1, List<T2>> data)
		{
			return new HashSet<T2>(from list1 in data.Values
				from list2 in data.Values
				where list1 != list2
				from item in list1.Intersect<T2>(list2)
				select item);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004740 File Offset: 0x00002940
		public static IEnumerable<TSource> Exclude<TSource, TKey>(this IEnumerable<TSource> source, IEnumerable<TSource> exclude, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> excludedSet = new HashSet<TKey>(exclude.Select<TSource, TKey>(keySelector));
			return source.Where<TSource>((TSource item) => !excludedSet.Contains(keySelector(item)));
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004784 File Offset: 0x00002984
		public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> values)
		{
			return values.All<T>((T value) => source.Contains(value));
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000047B0 File Offset: 0x000029B0
		public static bool ContainsList<T>(this IEnumerable<T> collection1, IEnumerable<T> collection2)
		{
			foreach (T t in collection1)
			{
				if (collection2.Contains(t))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004804 File Offset: 0x00002A04
		public static bool IsContainsElementOnly<T>(this IEnumerable<T> ienum, T element)
		{
			if (ienum == null)
			{
				return false;
			}
			if (ienum.Count<T>() == 0)
			{
				return false;
			}
			foreach (T t in ienum)
			{
				if (!t.Equals(element))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004870 File Offset: 0x00002A70
		public static T MaxBy<T, R>(this IEnumerable<T> en, Func<T, R> evaluate) where R : IComparable<R>
		{
			return en.Select<T, Tuple<T, R>>((T t) => new Tuple<T, R>(t, evaluate(t))).Aggregate<Tuple<T, R>>(delegate(Tuple<T, R> max, Tuple<T, R> next)
			{
				R item = next.Item2;
				if (item.CompareTo(max.Item2) <= 0)
				{
					return max;
				}
				return next;
			}).Item1;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000048C8 File Offset: 0x00002AC8
		public static IEnumerable<T> GetBetweenElement<T>(this T element, IEnumerable<T> ienumerable)
		{
			List<T> list = new List<T>();
			foreach (T t in ienumerable)
			{
				if (element.Equals(t))
				{
					break;
				}
				list.Add(t);
			}
			return list;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000492C File Offset: 0x00002B2C
		public static bool IsEmpty(this byte[] byteArray)
		{
			return byteArray == null || byteArray.Length == 0;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004938 File Offset: 0x00002B38
		public static bool IsEmpty<T>(this IEnumerable<T> collection)
		{
			return collection == null || collection.Count<T>() <= 0;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000494C File Offset: 0x00002B4C
		public static List<List<T>> Split<T>(this IEnumerable<T> array, int batchSize)
		{
			int i = 0;
			return (from g in array.GroupBy<T, int>(delegate(T s)
				{
					int j = i;
					i = j + 1;
					return j / batchSize;
				})
				select g.ToList<T>()).ToList<List<T>>();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000049A8 File Offset: 0x00002BA8
		public static int CountAll<T>(this IEnumerable<IEnumerable<T>> listOfLists)
		{
			if (listOfLists.IsEmpty<IEnumerable<T>>())
			{
				return 0;
			}
			int num = 0;
			foreach (IEnumerable<T> enumerable in listOfLists)
			{
				if (!enumerable.IsEmpty<T>())
				{
					num += enumerable.Count<T>();
				}
			}
			return num;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004A08 File Offset: 0x00002C08
		public static List<T> FromChunks<T>(this IEnumerable<IEnumerable<T>> chunks)
		{
			List<T> list;
			try
			{
				list = (from x in chunks.SelectMany<IEnumerable<T>, T>((IEnumerable<T> x) => x)
					where !x.IsDefault<T>()
					select x).ToList<T>();
			}
			catch (Exception)
			{
				list = new List<T>();
			}
			return list;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004A80 File Offset: 0x00002C80
		public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			foreach (T t in source)
			{
				target.Add(t);
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004AE4 File Offset: 0x00002CE4
		public static List<string> Reverse(this List<string> array)
		{
			for (int i = 0; i < array.Count<string>() / 2; i++)
			{
				string text = array[i];
				array[i] = array[array.Count<string>() - i - 1];
				array[array.Count<string>() - i - 1] = text;
			}
			return array;
		}
	}
}
