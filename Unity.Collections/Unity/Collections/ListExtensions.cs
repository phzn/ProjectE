using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000082 RID: 130
	public static class ListExtensions
	{
		// Token: 0x060006DA RID: 1754 RVA: 0x00016BE8 File Offset: 0x00014DE8
		public static bool RemoveSwapBack<T>(this List<T> list, T value)
		{
			int num = list.IndexOf(value);
			if (num < 0)
			{
				return false;
			}
			list.RemoveAtSwapBack<T>(num);
			return true;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00016C0C File Offset: 0x00014E0C
		public static bool RemoveSwapBack<T>(this List<T> list, Predicate<T> matcher)
		{
			int num = list.FindIndex(matcher);
			if (num < 0)
			{
				return false;
			}
			list.RemoveAtSwapBack<T>(num);
			return true;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00016C30 File Offset: 0x00014E30
		public static void RemoveAtSwapBack<T>(this List<T> list, int index)
		{
			int num = list.Count - 1;
			list[index] = list[num];
			list.RemoveAt(num);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00016C5C File Offset: 0x00014E5C
		public static NativeList<T> ToNativeList<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this List<T> list, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
		{
			NativeList<T> nativeList = new NativeList<T>(list.Count, allocator);
			for (int i = 0; i < list.Count; i++)
			{
				nativeList.AddNoResize(list[i]);
			}
			return nativeList;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00016C98 File Offset: 0x00014E98
		public static NativeArray<T> ToNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this List<T> list, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
		{
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(list.Count, allocator, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < list.Count; i++)
			{
				nativeArray[i] = list[i];
			}
			return nativeArray;
		}
	}
}
