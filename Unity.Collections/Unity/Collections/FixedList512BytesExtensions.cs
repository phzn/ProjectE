using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x0200005C RID: 92
	[GenerateTestsForBurstCompatibility]
	public static class FixedList512BytesExtensions
	{
		// Token: 0x06000354 RID: 852 RVA: 0x000099AC File Offset: 0x00007BAC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static int IndexOf<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList512Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>((void*)list.Buffer, list.Length, value);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000099C0 File Offset: 0x00007BC0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool Contains<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList512Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return (ref list).IndexOf(value) != -1;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x000099D0 File Offset: 0x00007BD0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool Remove<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList512Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			int num = (ref list).IndexOf(value);
			if (num < 0)
			{
				return false;
			}
			list.RemoveAt(num);
			return true;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000099F4 File Offset: 0x00007BF4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool RemoveSwapBack<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList512Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			int num = (ref list).IndexOf(value);
			if (num == -1)
			{
				return false;
			}
			list.RemoveAtSwapBack(num);
			return true;
		}
	}
}
