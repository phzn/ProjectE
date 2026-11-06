using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000060 RID: 96
	[GenerateTestsForBurstCompatibility]
	public static class FixedList4096BytesExtensions
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x0000A468 File Offset: 0x00008668
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static int IndexOf<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList4096Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>((void*)list.Buffer, list.Length, value);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000A47C File Offset: 0x0000867C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool Contains<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList4096Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			return (ref list).IndexOf(value) != -1;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000A48C File Offset: 0x0000868C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool Remove<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList4096Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
		{
			int num = (ref list).IndexOf(value);
			if (num < 0)
			{
				return false;
			}
			list.RemoveAt(num);
			return true;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000A4B0 File Offset: 0x000086B0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool RemoveSwapBack<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this FixedList4096Bytes<T> list, U value) where T : struct, ValueType, IEquatable<U>
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
