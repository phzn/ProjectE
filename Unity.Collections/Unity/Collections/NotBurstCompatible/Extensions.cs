using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections.NotBurstCompatible
{
	// Token: 0x020000FD RID: 253
	public static class Extensions
	{
		// Token: 0x06000ADF RID: 2783 RVA: 0x0002122C File Offset: 0x0001F42C
		[ExcludeFromBurstCompatTesting("Returns managed array")]
		public static T[] ToArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> set) where T : struct, ValueType, IEquatable<T>
		{
			NativeArray<T> nativeArray = set.ToNativeArray(Allocator.TempJob);
			T[] array = nativeArray.ToArray();
			nativeArray.Dispose();
			return array;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00021258 File Offset: 0x0001F458
		[ExcludeFromBurstCompatTesting("Returns managed array")]
		public static T[] ToArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> set) where T : struct, ValueType, IEquatable<T>
		{
			NativeArray<T> nativeArray = set.ToNativeArray(Allocator.TempJob);
			T[] array = nativeArray.ToArray();
			nativeArray.Dispose();
			return array;
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00021284 File Offset: 0x0001F484
		[ExcludeFromBurstCompatTesting("Returns managed array")]
		public static T[] ToArrayNBC<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeList<T> list) where T : struct, ValueType
		{
			return list.AsArray().ToArray();
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000212A0 File Offset: 0x0001F4A0
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public static void CopyFromNBC<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeList<T> list, T[] array) where T : struct, ValueType
		{
			list.Clear();
			list.Resize(array.Length, NativeArrayOptions.UninitializedMemory);
			list.AsArray().CopyFrom(array);
		}
	}
}
