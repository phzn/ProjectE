using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization
{
	// Token: 0x02000006 RID: 6
	internal static class NativeArrayUtility
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020CC File Offset: 0x000002CC
		public static NativeArray<T> Resize<[IsUnmanaged] T>(NativeArray<T> array, int length, Allocator allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory) where T : struct, ValueType
		{
			if (array.IsCreated && array.Length >= length)
			{
				return array;
			}
			NativeArray<T> nativeArray = new NativeArray<T>(length, allocator, options);
			if (!array.IsCreated)
			{
				return nativeArray;
			}
			UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<T>(), array.GetUnsafePtr<T>(), (long)(array.Length * sizeof(T)));
			array.Dispose();
			return nativeArray;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000212C File Offset: 0x0000032C
		public unsafe static T* Resize<[IsUnmanaged] T>(T* ptr, int fromLength, int toLength, int alignment, Allocator allocator) where T : struct, ValueType
		{
			T* ptr2 = (T*)UnsafeUtility.Malloc((long)(sizeof(T) * toLength), alignment, allocator);
			UnsafeUtility.MemCpy((void*)ptr2, (void*)ptr, (long)(fromLength * sizeof(T)));
			UnsafeUtility.Free((void*)ptr, allocator);
			return ptr2;
		}
	}
}
