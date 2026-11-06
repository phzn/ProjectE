using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Collections
{
	// Token: 0x020000C3 RID: 195
	[GenerateTestsForBurstCompatibility]
	public static class NativeSortExtension
	{
		// Token: 0x0600090D RID: 2317 RVA: 0x0001B1F8 File Offset: 0x000193F8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T* array, int length) where T : struct, ValueType, IComparable<T>
		{
			NativeSortExtension.IntroSort<T, NativeSortExtension.DefaultComparer<T>>((void*)array, length, default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0001B215 File Offset: 0x00019415
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(T* array, int length, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			NativeSortExtension.IntroSort<T, U>((void*)array, length, comp);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0001B220 File Offset: 0x00019420
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static SortJob<T, NativeSortExtension.DefaultComparer<T>> SortJob<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T* array, int length) where T : struct, ValueType, IComparable<T>
		{
			return new SortJob<T, NativeSortExtension.DefaultComparer<T>>
			{
				Data = array,
				Length = length,
				Comp = default(NativeSortExtension.DefaultComparer<T>)
			};
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0001B254 File Offset: 0x00019454
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static SortJob<T, U> SortJob<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(T* array, int length, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			return new SortJob<T, U>
			{
				Data = array,
				Length = length,
				Comp = comp
			};
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001B284 File Offset: 0x00019484
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T* ptr, int length, T value) where T : struct, ValueType, IComparable<T>
		{
			return NativeSortExtension.BinarySearch<T, NativeSortExtension.DefaultComparer<T>>(ptr, length, value, default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0001B2A4 File Offset: 0x000194A4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(T* ptr, int length, T value, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			int num = 0;
			for (int num2 = length; num2 != 0; num2 >>= 1)
			{
				int num3 = num + (num2 >> 1);
				T t = ptr[(IntPtr)num3 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
				int num4 = comp.Compare(value, t);
				if (num4 == 0)
				{
					return num3;
				}
				if (num4 > 0)
				{
					num = num3 + 1;
					num2--;
				}
			}
			return ~num;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0001B2FC File Offset: 0x000194FC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T> array) where T : struct, ValueType, IComparable<T>
		{
			NativeSortExtension.IntroSortStruct<T, NativeSortExtension.DefaultComparer<T>>(array.GetUnsafePtr<T>(), array.Length, default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001B324 File Offset: 0x00019524
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeArray<T> array, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			T* unsafePtr = (T*)array.GetUnsafePtr<T>();
			int length = array.Length;
			NativeSortExtension.IntroSortStruct<T, U>((void*)unsafePtr, length, comp);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001B348 File Offset: 0x00019548
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static SortJob<T, NativeSortExtension.DefaultComparer<T>> SortJob<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T> array) where T : struct, ValueType, IComparable<T>
		{
			return NativeSortExtension.SortJob<T, NativeSortExtension.DefaultComparer<T>>((T*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(array), array.Length, default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001B370 File Offset: 0x00019570
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static SortJob<T, U> SortJob<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeArray<T> array, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			T* unsafeBufferPointerWithoutChecks = (T*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(array);
			int length = array.Length;
			return new SortJob<T, U>
			{
				Data = unsafeBufferPointerWithoutChecks,
				Length = length,
				Comp = comp
			};
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0001B3B0 File Offset: 0x000195B0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T> array, T value) where T : struct, ValueType, IComparable<T>
		{
			return array.BinarySearch(value, default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001B3CD File Offset: 0x000195CD
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeArray<T> array, T value, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			return NativeSortExtension.BinarySearch<T, U>((T*)array.GetUnsafeReadOnlyPtr<T>(), array.Length, value, comp);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001B3E4 File Offset: 0x000195E4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T>.ReadOnly array, T value) where T : struct, ValueType, IComparable<T>
		{
			return array.BinarySearch(value, default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0001B401 File Offset: 0x00019601
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeArray<T>.ReadOnly array, T value, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			return NativeSortExtension.BinarySearch<T, U>((T*)array.GetUnsafeReadOnlyPtr<T>(), array.Length, value, comp);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001B418 File Offset: 0x00019618
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeList<T> list) where T : struct, ValueType, IComparable<T>
		{
			list.Sort(default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0001B434 File Offset: 0x00019634
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeList<T> list, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			NativeSortExtension.IntroSort<T, U>((void*)list.GetUnsafePtr<T>(), list.Length, comp);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001B44C File Offset: 0x0001964C
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[] { typeof(int) })]
		public static SortJob<T, NativeSortExtension.DefaultComparer<T>> SortJob<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeList<T> list) where T : struct, ValueType, IComparable<T>
		{
			return list.SortJob(default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0001B468 File Offset: 0x00019668
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public static SortJob<T, U> SortJob<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeList<T> list, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			return NativeSortExtension.SortJob<T, U>(list.GetUnsafePtr<T>(), list.Length, comp);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0001B480 File Offset: 0x00019680
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[] { typeof(int) })]
		public static SortJobDefer<T, NativeSortExtension.DefaultComparer<T>> SortJobDefer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeList<T> list) where T : struct, ValueType, IComparable<T>
		{
			return list.SortJobDefer(default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001B49C File Offset: 0x0001969C
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public static SortJobDefer<T, U> SortJobDefer<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeList<T> list, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			return new SortJobDefer<T, U>
			{
				Data = list,
				Comp = comp
			};
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0001B4C4 File Offset: 0x000196C4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeList<T> list, T value) where T : struct, ValueType, IComparable<T>
		{
			return list.AsReadOnly().BinarySearch(value, default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001B4E7 File Offset: 0x000196E7
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeList<T> list, T value, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			return list.AsReadOnly().BinarySearch(value, comp);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001B4F8 File Offset: 0x000196F8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeList<T> list) where T : struct, ValueType, IComparable<T>
		{
			list.Sort(default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001B514 File Offset: 0x00019714
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this UnsafeList<T> list, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			NativeSortExtension.IntroSort<T, U>((void*)list.Ptr, list.Length, comp);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001B52C File Offset: 0x0001972C
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[] { typeof(int) })]
		public static SortJob<T, NativeSortExtension.DefaultComparer<T>> SortJob<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeList<T> list) where T : struct, ValueType, IComparable<T>
		{
			return NativeSortExtension.SortJob<T, NativeSortExtension.DefaultComparer<T>>(list.Ptr, list.Length, default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001B554 File Offset: 0x00019754
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public static SortJob<T, U> SortJob<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this UnsafeList<T> list, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			return NativeSortExtension.SortJob<T, U>(list.Ptr, list.Length, comp);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001B56C File Offset: 0x0001976C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeList<T> list, T value) where T : struct, ValueType, IComparable<T>
		{
			return list.BinarySearch(value, default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001B589 File Offset: 0x00019789
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this UnsafeList<T> list, T value, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			return NativeSortExtension.BinarySearch<T, U>(list.Ptr, list.Length, value, comp);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001B5A0 File Offset: 0x000197A0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeSlice<T> slice) where T : struct, ValueType, IComparable<T>
		{
			slice.Sort(default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001B5BC File Offset: 0x000197BC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeSlice<T> slice, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			T* unsafePtr = (T*)slice.GetUnsafePtr<T>();
			int length = slice.Length;
			NativeSortExtension.IntroSortStruct<T, U>((void*)unsafePtr, length, comp);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001B5E0 File Offset: 0x000197E0
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static SortJob<T, NativeSortExtension.DefaultComparer<T>> SortJob<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeSlice<T> slice) where T : struct, ValueType, IComparable<T>
		{
			return NativeSortExtension.SortJob<T, NativeSortExtension.DefaultComparer<T>>((T*)slice.GetUnsafePtr<T>(), slice.Length, default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001B608 File Offset: 0x00019808
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static SortJob<T, U> SortJob<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeSlice<T> slice, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			return NativeSortExtension.SortJob<T, U>((T*)slice.GetUnsafePtr<T>(), slice.Length, comp);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001B620 File Offset: 0x00019820
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeSlice<T> slice, T value) where T : struct, ValueType, IComparable<T>
		{
			return slice.BinarySearch(value, default(NativeSortExtension.DefaultComparer<T>));
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0001B63D File Offset: 0x0001983D
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static int BinarySearch<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeSlice<T> slice, T value, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			return NativeSortExtension.BinarySearch<T, U>((T*)slice.GetUnsafeReadOnlyPtr<T>(), slice.Length, value, comp);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001B653 File Offset: 0x00019853
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		internal unsafe static void IntroSort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, int length, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			NativeSortExtension.IntroSort_R<T, U>(array, 0, length - 1, 2 * CollectionHelper.Log2Floor(length), comp);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001B668 File Offset: 0x00019868
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		internal unsafe static void IntroSort_R<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, int lo, int hi, int depth, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			while (hi > lo)
			{
				int num = hi - lo + 1;
				if (num <= 16)
				{
					if (num == 1)
					{
						return;
					}
					if (num == 2)
					{
						NativeSortExtension.SwapIfGreaterWithItems<T, U>(array, lo, hi, comp);
						return;
					}
					if (num == 3)
					{
						NativeSortExtension.SwapIfGreaterWithItems<T, U>(array, lo, hi - 1, comp);
						NativeSortExtension.SwapIfGreaterWithItems<T, U>(array, lo, hi, comp);
						NativeSortExtension.SwapIfGreaterWithItems<T, U>(array, hi - 1, hi, comp);
						return;
					}
					NativeSortExtension.InsertionSort<T, U>(array, lo, hi, comp);
					return;
				}
				else
				{
					if (depth == 0)
					{
						NativeSortExtension.HeapSort<T, U>(array, lo, hi, comp);
						return;
					}
					depth--;
					int num2 = NativeSortExtension.Partition<T, U>(array, lo, hi, comp);
					NativeSortExtension.IntroSort_R<T, U>(array, num2 + 1, hi, depth, comp);
					hi = num2 - 1;
				}
			}
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001B704 File Offset: 0x00019904
		private unsafe static void InsertionSort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, int lo, int hi, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			for (int i = lo; i < hi; i++)
			{
				int num = i;
				T t = UnsafeUtility.ReadArrayElement<T>(array, i + 1);
				while (num >= lo && comp.Compare(t, UnsafeUtility.ReadArrayElement<T>(array, num)) < 0)
				{
					UnsafeUtility.WriteArrayElement<T>(array, num + 1, UnsafeUtility.ReadArrayElement<T>(array, num));
					num--;
				}
				UnsafeUtility.WriteArrayElement<T>(array, num + 1, t);
			}
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001B768 File Offset: 0x00019968
		private unsafe static int Partition<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, int lo, int hi, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			int num = lo + (hi - lo) / 2;
			NativeSortExtension.SwapIfGreaterWithItems<T, U>(array, lo, num, comp);
			NativeSortExtension.SwapIfGreaterWithItems<T, U>(array, lo, hi, comp);
			NativeSortExtension.SwapIfGreaterWithItems<T, U>(array, num, hi, comp);
			T t = UnsafeUtility.ReadArrayElement<T>(array, num);
			NativeSortExtension.Swap<T>(array, num, hi - 1);
			int i = lo;
			int num2 = hi - 1;
			while (i < num2)
			{
				while (i < hi && comp.Compare(t, UnsafeUtility.ReadArrayElement<T>(array, ++i)) > 0)
				{
				}
				while (num2 > i && comp.Compare(t, UnsafeUtility.ReadArrayElement<T>(array, --num2)) < 0)
				{
				}
				if (i >= num2)
				{
					break;
				}
				NativeSortExtension.Swap<T>(array, i, num2);
			}
			NativeSortExtension.Swap<T>(array, i, hi - 1);
			return i;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001B810 File Offset: 0x00019A10
		private unsafe static void HeapSort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, int lo, int hi, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			int num = hi - lo + 1;
			for (int i = num / 2; i >= 1; i--)
			{
				NativeSortExtension.Heapify<T, U>(array, i, num, lo, comp);
			}
			for (int j = num; j > 1; j--)
			{
				NativeSortExtension.Swap<T>(array, lo, lo + j - 1);
				NativeSortExtension.Heapify<T, U>(array, 1, j - 1, lo, comp);
			}
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0001B860 File Offset: 0x00019A60
		private unsafe static void Heapify<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, int i, int n, int lo, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			T t = UnsafeUtility.ReadArrayElement<T>(array, lo + i - 1);
			while (i <= n / 2)
			{
				int num = 2 * i;
				if (num < n && comp.Compare(UnsafeUtility.ReadArrayElement<T>(array, lo + num - 1), UnsafeUtility.ReadArrayElement<T>(array, lo + num)) < 0)
				{
					num++;
				}
				if (comp.Compare(UnsafeUtility.ReadArrayElement<T>(array, lo + num - 1), t) < 0)
				{
					break;
				}
				UnsafeUtility.WriteArrayElement<T>(array, lo + i - 1, UnsafeUtility.ReadArrayElement<T>(array, lo + num - 1));
				i = num;
			}
			UnsafeUtility.WriteArrayElement<T>(array, lo + i - 1, t);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001B8F4 File Offset: 0x00019AF4
		private unsafe static void Swap<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(void* array, int lhs, int rhs) where T : struct, ValueType
		{
			T t = UnsafeUtility.ReadArrayElement<T>(array, lhs);
			UnsafeUtility.WriteArrayElement<T>(array, lhs, UnsafeUtility.ReadArrayElement<T>(array, rhs));
			UnsafeUtility.WriteArrayElement<T>(array, rhs, t);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001B91F File Offset: 0x00019B1F
		private unsafe static void SwapIfGreaterWithItems<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, int lhs, int rhs, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			if (lhs != rhs && comp.Compare(UnsafeUtility.ReadArrayElement<T>(array, lhs), UnsafeUtility.ReadArrayElement<T>(array, rhs)) > 0)
			{
				NativeSortExtension.Swap<T>(array, lhs, rhs);
			}
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0001B94C File Offset: 0x00019B4C
		private unsafe static void IntroSortStruct<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, int length, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			int num = 0;
			int num2 = length - 1;
			NativeSortExtension.IntroSortStruct_R<T, U>(array, in num, in num2, 2 * CollectionHelper.Log2Floor(length), comp);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0001B974 File Offset: 0x00019B74
		private unsafe static void IntroSortStruct_R<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, in int lo, in int _hi, int depth, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			int i = _hi;
			while (i > lo)
			{
				int num = i - lo + 1;
				if (num <= 16)
				{
					if (num == 1)
					{
						return;
					}
					if (num == 2)
					{
						NativeSortExtension.SwapIfGreaterWithItemsStruct<T, U>(array, lo, i, comp);
						return;
					}
					if (num == 3)
					{
						NativeSortExtension.SwapIfGreaterWithItemsStruct<T, U>(array, lo, i - 1, comp);
						NativeSortExtension.SwapIfGreaterWithItemsStruct<T, U>(array, lo, i, comp);
						NativeSortExtension.SwapIfGreaterWithItemsStruct<T, U>(array, i - 1, i, comp);
						return;
					}
					NativeSortExtension.InsertionSortStruct<T, U>(array, in lo, in i, comp);
					return;
				}
				else
				{
					if (depth == 0)
					{
						NativeSortExtension.HeapSortStruct<T, U>(array, in lo, in i, comp);
						return;
					}
					depth--;
					int num2 = NativeSortExtension.PartitionStruct<T, U>(array, in lo, in i, comp);
					int num3 = num2 + 1;
					NativeSortExtension.IntroSortStruct_R<T, U>(array, in num3, in i, depth, comp);
					i = num2 - 1;
				}
			}
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0001BA1C File Offset: 0x00019C1C
		private unsafe static void InsertionSortStruct<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, in int lo, in int hi, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			for (int i = lo; i < hi; i++)
			{
				int num = i;
				T t = UnsafeUtility.ReadArrayElement<T>(array, i + 1);
				while (num >= lo && comp.Compare(t, UnsafeUtility.ReadArrayElement<T>(array, num)) < 0)
				{
					UnsafeUtility.WriteArrayElement<T>(array, num + 1, UnsafeUtility.ReadArrayElement<T>(array, num));
					num--;
				}
				UnsafeUtility.WriteArrayElement<T>(array, num + 1, t);
			}
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0001BA80 File Offset: 0x00019C80
		private unsafe static int PartitionStruct<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, in int lo, in int hi, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			int num = lo + (hi - lo) / 2;
			NativeSortExtension.SwapIfGreaterWithItemsStruct<T, U>(array, lo, num, comp);
			NativeSortExtension.SwapIfGreaterWithItemsStruct<T, U>(array, lo, hi, comp);
			NativeSortExtension.SwapIfGreaterWithItemsStruct<T, U>(array, num, hi, comp);
			T t = UnsafeUtility.ReadArrayElement<T>(array, num);
			NativeSortExtension.SwapStruct<T>(array, num, hi - 1);
			int i = lo;
			int num2 = hi - 1;
			while (i < num2)
			{
				while (i < hi && comp.Compare(t, UnsafeUtility.ReadArrayElement<T>(array, ++i)) > 0)
				{
				}
				while (num2 > i && comp.Compare(t, UnsafeUtility.ReadArrayElement<T>(array, --num2)) < 0)
				{
				}
				if (i >= num2)
				{
					break;
				}
				NativeSortExtension.SwapStruct<T>(array, i, num2);
			}
			NativeSortExtension.SwapStruct<T>(array, i, hi - 1);
			return i;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0001BB34 File Offset: 0x00019D34
		private unsafe static void HeapSortStruct<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, in int lo, in int hi, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			int num = hi - lo + 1;
			for (int i = num / 2; i >= 1; i--)
			{
				NativeSortExtension.HeapifyStruct<T, U>(array, i, num, in lo, comp);
			}
			for (int j = num; j > 1; j--)
			{
				NativeSortExtension.SwapStruct<T>(array, lo, lo + j - 1);
				NativeSortExtension.HeapifyStruct<T, U>(array, 1, j - 1, in lo, comp);
			}
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0001BB88 File Offset: 0x00019D88
		private unsafe static void HeapifyStruct<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, int i, int n, in int lo, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			T t = UnsafeUtility.ReadArrayElement<T>(array, lo + i - 1);
			while (i <= n / 2)
			{
				int num = 2 * i;
				if (num < n && comp.Compare(UnsafeUtility.ReadArrayElement<T>(array, lo + num - 1), UnsafeUtility.ReadArrayElement<T>(array, lo + num)) < 0)
				{
					num++;
				}
				if (comp.Compare(UnsafeUtility.ReadArrayElement<T>(array, lo + num - 1), t) < 0)
				{
					break;
				}
				UnsafeUtility.WriteArrayElement<T>(array, lo + i - 1, UnsafeUtility.ReadArrayElement<T>(array, lo + num - 1));
				i = num;
			}
			UnsafeUtility.WriteArrayElement<T>(array, lo + i - 1, t);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0001BC24 File Offset: 0x00019E24
		private unsafe static void SwapStruct<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(void* array, int lhs, int rhs) where T : struct, ValueType
		{
			T t = UnsafeUtility.ReadArrayElement<T>(array, lhs);
			UnsafeUtility.WriteArrayElement<T>(array, lhs, UnsafeUtility.ReadArrayElement<T>(array, rhs));
			UnsafeUtility.WriteArrayElement<T>(array, rhs, t);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0001BC4F File Offset: 0x00019E4F
		private unsafe static void SwapIfGreaterWithItemsStruct<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* array, int lhs, int rhs, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			if (lhs != rhs && comp.Compare(UnsafeUtility.ReadArrayElement<T>(array, lhs), UnsafeUtility.ReadArrayElement<T>(array, rhs)) > 0)
			{
				NativeSortExtension.SwapStruct<T>(array, lhs, rhs);
			}
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0001BC7B File Offset: 0x00019E7B
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckStrideMatchesSize<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int stride) where T : struct, ValueType
		{
			if (stride != UnsafeUtility.SizeOf<T>())
			{
				throw new InvalidOperationException("Sort requires that stride matches the size of the source type");
			}
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0001BC90 File Offset: 0x00019E90
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe static void CheckComparer<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(T* array, int length, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			if (length > 0)
			{
				T t = *array;
				if (comp.Compare(t, t) != 0)
				{
					throw new InvalidOperationException("Comparison function is incorrect. Compare(a, a) must return 0/equal.");
				}
				int i = 1;
				int num = math.min(length, 8);
				while (i < num)
				{
					T t2 = array[(IntPtr)i * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
					if (comp.Compare(t, t2) != 0 || comp.Compare(t2, t) != 0)
					{
						if (comp.Compare(t, t2) == 0)
						{
							throw new InvalidOperationException("Comparison function is incorrect. Compare(a, b) of two different values should not return 0/equal.");
						}
						if (comp.Compare(t2, t) == 0)
						{
							throw new InvalidOperationException("Comparison function is incorrect. Compare(b, a) of two different values should not return 0/equal.");
						}
						if (comp.Compare(t, t2) == comp.Compare(t2, t))
						{
							throw new InvalidOperationException("Comparison function is incorrect. Compare(a, b) when a and b are different values should not return the same value as Compare(b, a).");
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x040003F1 RID: 1009
		private const int k_IntrosortSizeThreshold = 16;

		// Token: 0x020000C4 RID: 196
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct DefaultComparer<T> : IComparer<T> where T : IComparable<T>
		{
			// Token: 0x06000941 RID: 2369 RVA: 0x0001BD77 File Offset: 0x00019F77
			public int Compare(T x, T y)
			{
				return x.CompareTo(y);
			}
		}
	}
}
