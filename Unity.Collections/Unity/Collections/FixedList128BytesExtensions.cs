using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000058 RID: 88
	[GenerateTestsForBurstCompatibility]
	public static class FixedList128BytesExtensions
	{
		// Token: 0x06000306 RID: 774 RVA: 0x00008EF0 File Offset: 0x000070F0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static int IndexOf<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList128Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>((void*)list.Buffer, list.Length, value);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00008F04 File Offset: 0x00007104
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool Contains<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList128Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return (ref list).IndexOf(value) != -1;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00008F14 File Offset: 0x00007114
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool Remove<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList128Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			int num = (ref list).IndexOf(value);
			if (num < 0)
			{
				return false;
			}
			list.RemoveAt(num);
			return true;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00008F38 File Offset: 0x00007138
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool RemoveSwapBack<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList128Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
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
