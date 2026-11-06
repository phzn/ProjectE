using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace MagicaCloth2
{
	// Token: 0x02000119 RID: 281
	public static class NativeArrayExtensions
	{
		// Token: 0x060005BA RID: 1466 RVA: 0x0002FA00 File Offset: 0x0002DC00
		public static void MC2DisposeSafe<[IsUnmanaged] T>(this NativeArray<T> array) where T : struct, ValueType
		{
			if (array.IsCreated)
			{
				array.Dispose();
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0002FA10 File Offset: 0x0002DC10
		public static void MC2Resize<[IsUnmanaged] T>(this NativeArray<T> array, int size, Allocator allocator = Allocator.Persistent, NativeArrayOptions options = NativeArrayOptions.ClearMemory) where T : struct, ValueType
		{
			if (!array.IsCreated || array.Length < size)
			{
				(ref array).MC2DisposeSafe<T>();
				array = new NativeArray<T>(size, allocator, options);
			}
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0002FA38 File Offset: 0x0002DC38
		public static byte[] MC2ToRawBytes<[IsUnmanaged] T>(this NativeArray<T> array) where T : struct, ValueType
		{
			if (!array.IsCreated || array.Length == 0)
			{
				return null;
			}
			NativeSlice<byte> nativeSlice = new NativeSlice<T>(array).SliceConvert<byte>();
			byte[] array2 = new byte[nativeSlice.Length];
			nativeSlice.CopyTo(array2);
			return array2;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0002FA84 File Offset: 0x0002DC84
		public static NativeArray<T> MC2FromRawBytes<[IsUnmanaged] T>(byte[] bytes, Allocator allocator = Allocator.Persistent) where T : struct, ValueType
		{
			if (bytes == null)
			{
				return default(NativeArray<T>);
			}
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = bytes.Length / num;
			NativeArray<T> nativeArray = new NativeArray<T>(num2, allocator, NativeArrayOptions.ClearMemory);
			if (num2 > 0)
			{
				using (NativeArray<byte> nativeArray2 = new NativeArray<byte>(bytes, Allocator.Temp))
				{
					new NativeSlice<byte>(nativeArray2).SliceConvert<T>().CopyTo(nativeArray);
				}
			}
			return nativeArray;
		}
	}
}
