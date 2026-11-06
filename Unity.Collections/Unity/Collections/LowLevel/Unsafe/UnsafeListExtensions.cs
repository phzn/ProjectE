using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200011A RID: 282
	[GenerateTestsForBurstCompatibility]
	public static class UnsafeListExtensions
	{
		// Token: 0x06000BFF RID: 3071 RVA: 0x00024218 File Offset: 0x00022418
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static int IndexOf<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this UnsafeList<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>((void*)list.Ptr, list.Length, value);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x0002422D File Offset: 0x0002242D
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool Contains<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this UnsafeList<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return list.IndexOf(value) != -1;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0002423C File Offset: 0x0002243C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static int IndexOf<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this UnsafeList<T>.ReadOnly list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>((void*)list.Ptr, list.Length, value);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00024250 File Offset: 0x00022450
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool Contains<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this UnsafeList<T>.ReadOnly list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return list.IndexOf(value) != -1;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0002425F File Offset: 0x0002245F
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static int IndexOf<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this UnsafeList<T>.ParallelReader list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>((void*)list.Ptr, list.Length, value);
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00024273 File Offset: 0x00022473
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool Contains<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this UnsafeList<T>.ParallelReader list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return list.IndexOf(value) != -1;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00024284 File Offset: 0x00022484
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static bool ArraysEqual<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeList<T> container, in UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			if (container.Length != other.Length)
			{
				return false;
			}
			for (int num = 0; num != container.Length; num++)
			{
				T t = container[num];
				UnsafeList<T> unsafeList = other;
				if (!t.Equals(unsafeList[num]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
