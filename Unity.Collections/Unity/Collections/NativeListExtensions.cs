using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x0200009F RID: 159
	[GenerateTestsForBurstCompatibility]
	public static class NativeListExtensions
	{
		// Token: 0x060007D2 RID: 2002 RVA: 0x00018430 File Offset: 0x00016630
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static bool Contains<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeList<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>((void*)list.GetUnsafeReadOnlyPtr<T>(), list.Length, value) != -1;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0001844B File Offset: 0x0001664B
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static int IndexOf<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeList<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>((void*)list.GetUnsafeReadOnlyPtr<T>(), list.Length, value);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00018460 File Offset: 0x00016660
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static bool ArraysEqual<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T> container, in NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			NativeList<T> nativeList = other;
			return container.ArraysEqual<T>(nativeList.AsArray());
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00018481 File Offset: 0x00016681
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static bool ArraysEqual<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeList<T> container, in NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			return other.ArraysEqual<T>(in container);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00018490 File Offset: 0x00016690
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static bool ArraysEqual<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeList<T> container, in NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			NativeArray<T> nativeArray = container.AsArray();
			NativeList<T> nativeList = other;
			return nativeArray.ArraysEqual<T>(nativeList.AsArray());
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x000184B7 File Offset: 0x000166B7
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static bool ArraysEqual<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeList<T> container, in UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			return (*container.m_ListData).ArraysEqual<T>(in other);
		}
	}
}
