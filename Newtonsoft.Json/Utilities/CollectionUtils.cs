using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000045 RID: 69
	[NullableContext(1)]
	[Nullable(0)]
	internal static class CollectionUtils
	{
		// Token: 0x06000445 RID: 1093 RVA: 0x00010BB8 File Offset: 0x0000EDB8
		public static bool IsNullOrEmpty<[Nullable(2)] T>(ICollection<T> collection)
		{
			return collection == null || collection.Count == 0;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00010BC8 File Offset: 0x0000EDC8
		public static void AddRange<[Nullable(2)] T>(this IList<T> initial, IEnumerable<T> collection)
		{
			if (initial == null)
			{
				throw new ArgumentNullException("initial");
			}
			if (collection == null)
			{
				return;
			}
			foreach (T t in collection)
			{
				initial.Add(t);
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00010C24 File Offset: 0x0000EE24
		public static bool IsDictionaryType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			return typeof(IDictionary).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof(IDictionary<, >)) || ReflectionUtils.ImplementsGenericDefinition(type, typeof(IReadOnlyDictionary<, >));
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00010C7C File Offset: 0x0000EE7C
		[return: Nullable(2)]
		public static ConstructorInfo ResolveEnumerableCollectionConstructor(Type collectionType, Type collectionItemType)
		{
			Type type = typeof(IList<>).MakeGenericType(new Type[] { collectionItemType });
			return CollectionUtils.ResolveEnumerableCollectionConstructor(collectionType, collectionItemType, type);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00010CAC File Offset: 0x0000EEAC
		[return: Nullable(2)]
		public static ConstructorInfo ResolveEnumerableCollectionConstructor(Type collectionType, Type collectionItemType, Type constructorArgumentType)
		{
			Type type = typeof(IEnumerable<>).MakeGenericType(new Type[] { collectionItemType });
			ConstructorInfo constructorInfo = null;
			foreach (ConstructorInfo constructorInfo2 in collectionType.GetConstructors(BindingFlags.Instance | BindingFlags.Public))
			{
				IList<ParameterInfo> parameters = constructorInfo2.GetParameters();
				if (parameters.Count == 1)
				{
					Type parameterType = parameters[0].ParameterType;
					if (type == parameterType)
					{
						constructorInfo = constructorInfo2;
						break;
					}
					if (constructorInfo == null && parameterType.IsAssignableFrom(constructorArgumentType))
					{
						constructorInfo = constructorInfo2;
					}
				}
			}
			return constructorInfo;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00010D39 File Offset: 0x0000EF39
		public static bool AddDistinct<[Nullable(2)] T>(this IList<T> list, T value)
		{
			return list.AddDistinct(value, EqualityComparer<T>.Default);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00010D47 File Offset: 0x0000EF47
		public static bool AddDistinct<[Nullable(2)] T>(this IList<T> list, T value, IEqualityComparer<T> comparer)
		{
			if (list.ContainsValue(value, comparer))
			{
				return false;
			}
			list.Add(value);
			return true;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00010D60 File Offset: 0x0000EF60
		public static bool ContainsValue<[Nullable(2)] TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
		{
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			foreach (TSource tsource in source)
			{
				if (comparer.Equals(tsource, value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00010DCC File Offset: 0x0000EFCC
		public static bool AddRangeDistinct<[Nullable(2)] T>(this IList<T> list, IEnumerable<T> values, IEqualityComparer<T> comparer)
		{
			bool flag = true;
			foreach (T t in values)
			{
				if (!list.AddDistinct(t, comparer))
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00010E1C File Offset: 0x0000F01C
		public static int IndexOf<[Nullable(2)] T>(this IEnumerable<T> collection, Func<T, bool> predicate)
		{
			int num = 0;
			foreach (T t in collection)
			{
				if (predicate(t))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00010E74 File Offset: 0x0000F074
		public static bool Contains<[Nullable(2)] T>(this List<T> list, T value, IEqualityComparer comparer)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (comparer.Equals(value, list[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00010EB0 File Offset: 0x0000F0B0
		public static int IndexOfReference<[Nullable(2)] T>(this List<T> list, T item)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (item == list[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00010EE8 File Offset: 0x0000F0E8
		public static void FastReverse<[Nullable(2)] T>(this List<T> list)
		{
			int i = 0;
			int num = list.Count - 1;
			while (i < num)
			{
				T t = list[i];
				list[i] = list[num];
				list[num] = t;
				i++;
				num--;
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00010F2C File Offset: 0x0000F12C
		private static IList<int> GetDimensions(IList values, int dimensionsCount)
		{
			IList<int> list = new List<int>();
			IList list2 = values;
			for (;;)
			{
				list.Add(list2.Count);
				if (list.Count == dimensionsCount || list2.Count == 0)
				{
					break;
				}
				IList list3 = list2[0] as IList;
				if (list3 == null)
				{
					break;
				}
				list2 = list3;
			}
			return list;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00010F74 File Offset: 0x0000F174
		private static void CopyFromJaggedToMultidimensionalArray(IList values, Array multidimensionalArray, int[] indices)
		{
			int num = indices.Length;
			if (num == multidimensionalArray.Rank)
			{
				multidimensionalArray.SetValue(CollectionUtils.JaggedArrayGetValue(values, indices), indices);
				return;
			}
			int length = multidimensionalArray.GetLength(num);
			if (((IList)CollectionUtils.JaggedArrayGetValue(values, indices)).Count != length)
			{
				throw new Exception("Cannot deserialize non-cubical array as multidimensional array.");
			}
			int[] array = new int[num + 1];
			for (int i = 0; i < num; i++)
			{
				array[i] = indices[i];
			}
			for (int j = 0; j < multidimensionalArray.GetLength(num); j++)
			{
				array[num] = j;
				CollectionUtils.CopyFromJaggedToMultidimensionalArray(values, multidimensionalArray, array);
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00011004 File Offset: 0x0000F204
		private static object JaggedArrayGetValue(IList values, int[] indices)
		{
			IList list = values;
			for (int i = 0; i < indices.Length; i++)
			{
				int num = indices[i];
				if (i == indices.Length - 1)
				{
					return list[num];
				}
				list = (IList)list[num];
			}
			return list;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00011044 File Offset: 0x0000F244
		public static Array ToMultidimensionalArray(IList values, Type type, int rank)
		{
			IList<int> dimensions = CollectionUtils.GetDimensions(values, rank);
			while (dimensions.Count < rank)
			{
				dimensions.Add(0);
			}
			Array array = Array.CreateInstance(type, dimensions.ToArray<int>());
			CollectionUtils.CopyFromJaggedToMultidimensionalArray(values, array, CollectionUtils.ArrayEmpty<int>());
			return array;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00011085 File Offset: 0x0000F285
		public static T[] ArrayEmpty<[Nullable(2)] T>()
		{
			return CollectionUtils.EmptyArrayContainer<T>.Empty;
		}

		// Token: 0x02000166 RID: 358
		[NullableContext(0)]
		private static class EmptyArrayContainer<[Nullable(2)] T>
		{
			// Token: 0x04000697 RID: 1687
			[Nullable(1)]
			public static readonly T[] Empty = new T[0];
		}
	}
}
