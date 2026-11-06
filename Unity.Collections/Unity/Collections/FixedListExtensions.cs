using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000062 RID: 98
	public static class FixedListExtensions
	{
		// Token: 0x060003A8 RID: 936 RVA: 0x0000A4EF File Offset: 0x000086EF
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this FixedList32Bytes<T> list) where T : struct, ValueType, IComparable<T>
		{
			NativeSortExtension.Sort<T>((T*)(list.buffer + FixedList.PaddingBytes<T>()), list.Length);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000A508 File Offset: 0x00008708
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList32Bytes<T> list, U comp) where T : struct, ValueType, IComparable<T> where U : IComparer<T>
		{
			NativeSortExtension.Sort<T, U>((T*)(list.buffer + FixedList.PaddingBytes<T>()), list.Length, comp);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000A522 File Offset: 0x00008722
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this FixedList64Bytes<T> list) where T : struct, ValueType, IComparable<T>
		{
			NativeSortExtension.Sort<T>((T*)(list.buffer + FixedList.PaddingBytes<T>()), list.Length);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000A53B File Offset: 0x0000873B
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList64Bytes<T> list, U comp) where T : struct, ValueType, IComparable<T> where U : IComparer<T>
		{
			NativeSortExtension.Sort<T, U>((T*)(list.buffer + FixedList.PaddingBytes<T>()), list.Length, comp);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000A555 File Offset: 0x00008755
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this FixedList128Bytes<T> list) where T : struct, ValueType, IComparable<T>
		{
			NativeSortExtension.Sort<T>((T*)(list.buffer + FixedList.PaddingBytes<T>()), list.Length);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000A56E File Offset: 0x0000876E
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList128Bytes<T> list, U comp) where T : struct, ValueType, IComparable<T> where U : IComparer<T>
		{
			NativeSortExtension.Sort<T, U>((T*)(list.buffer + FixedList.PaddingBytes<T>()), list.Length, comp);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000A588 File Offset: 0x00008788
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this FixedList512Bytes<T> list) where T : struct, ValueType, IComparable<T>
		{
			NativeSortExtension.Sort<T>((T*)(list.buffer + FixedList.PaddingBytes<T>()), list.Length);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000A5A1 File Offset: 0x000087A1
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList512Bytes<T> list, U comp) where T : struct, ValueType, IComparable<T> where U : IComparer<T>
		{
			NativeSortExtension.Sort<T, U>((T*)(list.buffer + FixedList.PaddingBytes<T>()), list.Length, comp);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000A5BB File Offset: 0x000087BB
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this FixedList4096Bytes<T> list) where T : struct, ValueType, IComparable<T>
		{
			NativeSortExtension.Sort<T>((T*)(list.buffer + FixedList.PaddingBytes<T>()), list.Length);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000A5D4 File Offset: 0x000087D4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(NativeSortExtension.DefaultComparer<int>)
		})]
		public unsafe static void Sort<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList4096Bytes<T> list, U comp) where T : struct, ValueType, IComparable<T> where U : IComparer<T>
		{
			NativeSortExtension.Sort<T, U>((T*)(list.buffer + FixedList.PaddingBytes<T>()), list.Length, comp);
		}
	}
}
